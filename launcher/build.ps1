param([switch]$ModernOnly, [switch]$LegacyOnly,
      [ValidateSet('x64','x86')][string]$Architecture = 'x64')
$ErrorActionPreference='Stop'
if ($ModernOnly -and $LegacyOnly) { throw 'Choose one runtime target.' }
$root=Join-Path (Split-Path $PSScriptRoot -Parent) 'client\server'
$suffix = if ($Architecture -eq 'x86') { '-x86' } else { '' }
$variants = @()
if (-not $LegacyOnly) { $variants += ,@('net8.0-windows',"launcher$suffix") }
if (-not $ModernOnly) { $variants += ,@('net6.0-windows',"launcher-win7$suffix") }
foreach($variant in $variants) {
    $output = Join-Path $root $variant[1]
    dotnet publish (Join-Path $PSScriptRoot 'Nanaimo.Launcher.csproj') -c Release -f $variant[0] -r "win-$Architecture" --self-contained true -o $output
    if($LASTEXITCODE) {throw "Launcher build failed: $($variant[0])"}
    & (Join-Path (Split-Path $PSScriptRoot -Parent) 'server\tools\prune-windows-runtimes.ps1') -Destination $output
    if($variant[0] -eq 'net6.0-windows') {
        & (Join-Path (Split-Path $PSScriptRoot -Parent) 'server\tools\copy-win7-runtime.ps1') -Destination (Join-Path $root $variant[1]) -Architecture $Architecture
    }
}
& (Join-Path $PSScriptRoot 'build-native-entry.ps1')
