param([switch]$Apply)
$ErrorActionPreference = 'Stop'
if (-not $Apply) {
    Write-Output 'PREVIEW ONLY: no files will be removed.'
    Write-Output ('To delete the listed files, close the game and run: & "' + $PSCommandPath + '" -Apply')
}
$workspace = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../..')).TrimEnd('\')
if (-not (Test-Path -LiteralPath (Join-Path $workspace 'launcher/Nanaimo.Launcher.csproj'))) { throw 'Workspace marker missing.' }
$running = Get-CimInstance Win32_Process | Where-Object {
    $_.ExecutablePath -and $_.ExecutablePath.StartsWith($workspace + '\', [StringComparison]::OrdinalIgnoreCase)
}
if ($Apply -and $running) { throw 'Close all game, server, launcher, and workspace tool processes first.' }
$relativePaths = @(
    '.work', 'artifacts', 'game-unpacked.exe', 'original-installers',
    'client/server/server-merged/data', 'client/server/save-backups', 'client/server/server-merged/save-backups',
    'client/server/launcher/local-account.json', 'client/server/launcher-win7/local-account.json',
    'client/StateOption/gamestartoption.ini', 'server/tools/checks',
    'server/tools/launcher-check-output', 'server/src/build',
    'launcher/native-entry/entry.o', 'launcher/native-entry/entry.res',
    'server/tools/tcc-0.9.27-win32-bin.zip'
)
$targets = @($relativePaths | ForEach-Object { Join-Path $workspace $_ })
$targets += @(Get-ChildItem -LiteralPath (Join-Path $workspace 'client/server/server-merged/bin') -File -ErrorAction SilentlyContinue |
    Where-Object { $_.Extension -in @('.dat', '.handles') } | Select-Object -ExpandProperty FullName)
# Build intermediates and logs are disposable; game catalog and artwork inputs are not.
$targets += @(Get-ChildItem -LiteralPath $workspace -Recurse -Force -Directory |
    Where-Object { $_.Name -in @('obj', '__pycache__') -and $_.FullName -notlike "$workspace\.git\*" } |
    Select-Object -ExpandProperty FullName)
$targets += @(Get-ChildItem -LiteralPath $workspace -Recurse -Force -File |
    Where-Object { $_.Extension -in @('.log', '.bak', '.dmp') -and $_.FullName -notlike "$workspace\.git\*" } |
    Select-Object -ExpandProperty FullName)
foreach ($candidate in ($targets | Sort-Object -Unique)) {
    $target = [IO.Path]::GetFullPath($candidate)
    if (-not $target.StartsWith($workspace + '\', [StringComparison]::OrdinalIgnoreCase)) { throw "Unsafe cleanup target: $target" }
    if (-not (Test-Path -LiteralPath $target)) { continue }
    $item = Get-Item -LiteralPath $target -Force
    if ($item.Attributes -band [IO.FileAttributes]::ReparsePoint) { throw "Refusing linked target: $target" }
    if ($item.PSIsContainer -and (Get-ChildItem -LiteralPath $target -Recurse -Force | Where-Object { $_.Attributes -band [IO.FileAttributes]::ReparsePoint })) { throw "Linked descendant: $target" }
    Write-Output "$(if ($Apply) { 'REMOVE' } else { 'PREVIEW' }) $target"
    if ($Apply) { Remove-Item -LiteralPath $target -Recurse -Force }
}
if ($Apply) {
    $profile = Join-Path $workspace 'client/server/config/profile.ini'
    if (Test-Path -LiteralPath (Split-Path $profile -Parent)) {
        Copy-Item -LiteralPath (Join-Path $workspace 'server/config/profile.ini') -Destination $profile -Force
    }
    Write-Output 'WORKSPACE_CLEAN_PASS'
}
