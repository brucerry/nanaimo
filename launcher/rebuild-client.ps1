$ErrorActionPreference='Stop'
$workspace=Split-Path $PSScriptRoot -Parent
$development=Join-Path $workspace 'server'
$root=[IO.Path]::GetFullPath($workspace+'-archive')
$output=Join-Path $root 'artifacts\game.rebuilt.exe'
py -3.10 (Join-Path $development 'tools\unpack_client.py') `
    (Join-Path $root 'backups\game.original.exe') $output `
    --reference (Join-Path $root 'backups\game.recovery-reference.exe')
if($LASTEXITCODE) {throw 'Client rebuild failed.'}
$hash=(Get-FileHash -LiteralPath $output -Algorithm SHA256).Hash
Write-Host "CLIENT_REBUILD_PASS $output $hash"
