param([Parameter(Mandatory=$true)][string]$Destination,
      [ValidateSet('x64','x86')][string]$Architecture = 'x64')
$ErrorActionPreference='Stop'
$redist=Join-Path ${env:ProgramFiles(x86)} "Windows Kits\10\Redist\ucrt\DLLs\$Architecture"
if(-not (Test-Path -LiteralPath (Join-Path $redist 'ucrtbase.dll'))) {
    throw 'Windows SDK Universal CRT redistributable is required to build the Win7 package.'
}
[void][IO.Directory]::CreateDirectory($Destination)
Get-ChildItem -LiteralPath $redist -Filter '*.dll' -File | Copy-Item -Destination $Destination -Force
Write-Output "WIN7_LOCAL_CRT_PASS $Architecture $Destination"
