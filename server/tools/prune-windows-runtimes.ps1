param([Parameter(Mandatory)][string]$Destination)
$ErrorActionPreference = 'Stop'

# SQLite's NuGet package publishes native libraries for every supported OS.
# This portable game targets Windows x64 and x86 only.
$output = [IO.Path]::GetFullPath($Destination)
$runtimes = Join-Path $output 'runtimes'
if (-not (Test-Path -LiteralPath $runtimes -PathType Container)) { return }

$allowed = @('win-x64', 'win-x86')
foreach ($directory in Get-ChildItem -LiteralPath $runtimes -Directory) {
    if ($directory.Name -in $allowed) { continue }
    $resolved = [IO.Path]::GetFullPath($directory.FullName)
    $parent = [IO.Path]::GetFullPath($runtimes).TrimEnd('\') + '\'
    if (-not $resolved.StartsWith($parent, [StringComparison]::OrdinalIgnoreCase)) {
        throw "Runtime directory escaped the publish output: $resolved"
    }
    Remove-Item -LiteralPath $resolved -Recurse -Force
}
