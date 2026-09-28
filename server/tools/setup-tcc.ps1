param([string]$Destination = (Join-Path $PSScriptRoot 'tcc'))
$ErrorActionPreference = 'Stop'
$uri = 'https://download.savannah.gnu.org/releases/tinycc/tcc-0.9.27-win32-bin.zip'
$expected = '02E2BFE8C272A549B15E4BFA4507BD7E05304692AF1761DB6C1E8E88AF675651'
$temporary = Join-Path ([IO.Path]::GetTempPath()) ('nanaimo-tcc-' + [guid]::NewGuid().ToString('N'))
[IO.Directory]::CreateDirectory($temporary) | Out-Null
try {
    $zip = Join-Path $temporary 'tcc.zip'
    Invoke-WebRequest -Uri $uri -OutFile $zip -UseBasicParsing
    if ((Get-FileHash -LiteralPath $zip -Algorithm SHA256).Hash -ne $expected) { throw 'TinyCC archive checksum mismatch.' }
    Expand-Archive -LiteralPath $zip -DestinationPath $temporary
    [IO.Directory]::CreateDirectory($Destination) | Out-Null
    Copy-Item -Path (Join-Path $temporary 'tcc/*') -Destination $Destination -Recurse -Force
    & (Join-Path $Destination 'tcc.exe') -v
    if ($LASTEXITCODE) { throw 'TinyCC verification failed.' }
} finally {
    $resolved = [IO.Path]::GetFullPath($temporary)
    $allowed = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd('\') + '\'
    if (-not $resolved.StartsWith($allowed, [StringComparison]::OrdinalIgnoreCase)) { throw 'Unsafe temporary directory.' }
    Remove-Item -LiteralPath $resolved -Recurse -Force
}
