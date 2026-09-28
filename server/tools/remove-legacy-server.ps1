param([switch]$Apply)
$ErrorActionPreference = 'Stop'
$runtime = Split-Path $PSScriptRoot -Parent
$workspace = Split-Path $runtime -Parent
$sourceName = [string][char]0x6E90 + [char]0x7801
$source = Join-Path $runtime $sourceName
$prefix = [IO.Path]::GetFullPath($source).TrimEnd('\') + '\'
if (-not (Test-Path -LiteralPath (Join-Path $source 'managed-host\Nanaimo.Server.csproj'))) {
    throw 'Expected merged-server workspace was not found.'
}
$relative = @(
    'gui_launcher',
    'build',
    'start_nanaimo_launcher.bat',
    'scripts\build_adapter.ps1',
    'scripts\test_launcher_contract.py',
    'scripts\verify_package.ps1'
)
$targets = @($relative | ForEach-Object { [IO.Path]::GetFullPath((Join-Path $source $_)) })
foreach ($path in $targets) {
    if (-not $path.StartsWith($prefix, [StringComparison]::OrdinalIgnoreCase)) {
        throw "Target outside source directory: $path"
    }
    $ancestor = $path
    while ($ancestor) {
        if (Test-Path -LiteralPath $ancestor) {
            if ((Get-Item -LiteralPath $ancestor -Force).Attributes -band [IO.FileAttributes]::ReparsePoint) {
                throw "Reparse point in target path: $ancestor"
            }
        }
        $ancestor = Split-Path $ancestor -Parent
    }
    if (Test-Path -LiteralPath $path) {
        $links = @(Get-ChildItem -LiteralPath $path -Recurse -Force | Where-Object {
            $_.Attributes -band [IO.FileAttributes]::ReparsePoint
        })
        if ($links.Count) { throw "Reparse point below target: $path" }
        Write-Output "TARGET $path"
    }
}
if (-not $Apply) {
    Write-Output 'PREVIEW_ONLY: no files changed. Use -Apply to delete these legacy files.'
    exit 0
}
$active = @(Get-CimInstance Win32_Process | Where-Object {
    $_.ExecutablePath -and $_.ExecutablePath.StartsWith($workspace + '\', [StringComparison]::OrdinalIgnoreCase) -and
    $_.Name -in @('game.exe', 'Nanaimo.Server.exe', 'Nanaimo.Launcher.exe', 'nanaimo_gameplay_bridge.exe')
})
if ($active.Count) { throw 'Close the game, server and manager before cleanup.' }
foreach ($path in $targets) {
    if (Test-Path -LiteralPath $path) { Remove-Item -LiteralPath $path -Recurse -Force }
}
foreach ($path in $targets) {
    if (Test-Path -LiteralPath $path) { throw "Target still exists: $path" }
}
Write-Output 'LEGACY_REMOVAL_PASS: only the six listed paths inside this workspace were removed.'
