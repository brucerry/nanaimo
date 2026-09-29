$ErrorActionPreference = 'Stop'
$workspace = Split-Path $PSScriptRoot -Parent
$outputDirectory = Join-Path $workspace 'server/tools/checks/native-display'
New-Item -ItemType Directory -Force -Path $outputDirectory | Out-Null
$output = Join-Path $outputDirectory 'checks.exe'
& (Join-Path $workspace 'server/tools/tcc/tcc.exe') (Join-Path $PSScriptRoot 'native-display/checks.c') -o $output -luser32 -lgdi32
if ($LASTEXITCODE) { throw 'Native display checks failed to compile.' }
& $output
if ($LASTEXITCODE) { throw "Native display checks failed: $LASTEXITCODE" }
