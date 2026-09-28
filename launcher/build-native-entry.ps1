$ErrorActionPreference = 'Stop'
$workspace = Split-Path $PSScriptRoot -Parent
$serverName = 'server'
$clientName = 'client'
$entryName = 'Start-Game.exe'
$client = Join-Path $workspace $clientName
$source = Join-Path $PSScriptRoot 'native-entry'
$iconPath = Join-Path $source 'game.ico'
if (-not (Test-Path -LiteralPath $iconPath)) {
    Add-Type -AssemblyName System.Drawing
    $icon = [Drawing.Icon]::ExtractAssociatedIcon((Join-Path $client 'game.exe'))
    $stream = [IO.File]::Create($iconPath)
    try { $icon.Save($stream) } finally { $stream.Dispose(); $icon.Dispose() }
}
$tcc = Join-Path (Join-Path $workspace $serverName) 'tools\tcc\tcc.exe'
$output = Join-Path $client $entryName
& $tcc '-Wl,-subsystem=windows' (Join-Path $source 'entry.c') -o $output
if ($LASTEXITCODE) { throw 'Native entry build failed.' }
if (-not ('NativeEntryIcon' -as [type])) { Add-Type -Path (Join-Path $source 'ApplyIcon.cs') }
[NativeEntryIcon]::Apply($output, $iconPath)
Write-Output "NATIVE_ENTRY_BUILD_PASS $output"
