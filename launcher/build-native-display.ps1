$ErrorActionPreference = 'Stop'
$workspace = Split-Path $PSScriptRoot -Parent
$tcc = Join-Path $workspace 'server/tools/tcc/tcc.exe'
$output = Join-Path $workspace 'client/ddraw.dll'
& $tcc -shared (Join-Path $PSScriptRoot 'native-display/display.c') -o $output -luser32 -lgdi32
if ($LASTEXITCODE) { throw 'Native display shim build failed.' }
# TinyCC decorates stdcall exports. Keep the calling convention but export the
# exact names imported by the legacy client without changing the PE layout.
$bytes = [IO.File]::ReadAllBytes($output)
$peOffset = [BitConverter]::ToInt32($bytes, 60)
if ([BitConverter]::ToUInt16($bytes, $peOffset + 4) -ne 0x014c) { throw 'The game requires an x86 display shim.' }
foreach ($entry in @(@('_DirectDrawCreateEx@16', 'DirectDrawCreateEx'), @('_DirectDrawEnumerateExA@12', 'DirectDrawEnumerateExA'))) {
    $old = [Text.Encoding]::ASCII.GetBytes($entry[0] + [char]0)
    $new = [Text.Encoding]::ASCII.GetBytes($entry[1] + [char]0)
    $matches = 0
    for ($i = 0; $i -le $bytes.Length - $old.Length; $i++) {
        $equal = $true
        for ($j = 0; $j -lt $old.Length; $j++) { if ($bytes[$i + $j] -ne $old[$j]) { $equal = $false; break } }
        if ($equal) {
            [Array]::Clear($bytes, $i, $old.Length)
            [Array]::Copy($new, 0, $bytes, $i, $new.Length)
            $matches++
        }
    }
    if ($matches -ne 1) { throw "Unexpected export layout: $($entry[0]) ($matches matches)." }
}
[IO.File]::WriteAllBytes($output, $bytes)
$exportsFile = [IO.Path]::ChangeExtension($output, '.def')
if (Test-Path -LiteralPath $exportsFile) { Remove-Item -LiteralPath $exportsFile }
Write-Output "NATIVE_DISPLAY_BUILD_PASS $output"
