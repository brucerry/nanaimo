param([switch]$Test, [switch]$ModernOnly, [switch]$LegacyOnly,
      [ValidateSet('x64','x86')][string]$Architecture = 'x64')
$ErrorActionPreference='Stop'
if ($ModernOnly -and $LegacyOnly) { throw 'Choose one runtime target.' }
$source=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$development=Split-Path $source -Parent
$workspace=Split-Path $development -Parent
$root=Join-Path $workspace 'client\server'
[IO.Directory]::CreateDirectory((Join-Path $root 'config')) | Out-Null
foreach ($config in @('profile.ini', 'launchsettings.json')) {
    $destination = if ($config -eq 'profile.ini') { Join-Path $root 'config/profile.ini' } else { Join-Path $root $config }
    if (-not (Test-Path -LiteralPath $destination)) { Copy-Item -LiteralPath (Join-Path $development "config/$config") -Destination $destination }
}
$suffix = if ($Architecture -eq 'x86') { '-x86' } else { '' }
$variants = @()
if (-not $LegacyOnly) { $variants += ,@('net8.0',"bin$suffix","build-manifest$suffix.json") }
if (-not $ModernOnly) { $variants += ,@('net6.0',"bin-win7$suffix","build-manifest-win7$suffix.json") }
foreach($variant in $variants) {
$output=Join-Path $root ('server-merged\'+$variant[1])
[IO.Directory]::CreateDirectory($output)|Out-Null
$tcc=Join-Path $development 'tools\tcc\tcc.exe'
& $tcc -I $source -I (Join-Path $source 'adapter') -I (Join-Path $source 'release') (Join-Path $source 'adapter\nanaimo_gameplay_bridge.c') -o (Join-Path $output 'nanaimo_gameplay_bridge.exe')
if($LASTEXITCODE){throw 'Native dungeon bridge build failed'}
dotnet publish (Join-Path $source 'managed-host\Nanaimo.Server.csproj') -c Release -f $variant[0] -r "win-$Architecture" --self-contained true -o $output --nologo
if($LASTEXITCODE){throw 'Managed gameplay build failed'}
& (Join-Path $development 'tools\prune-windows-runtimes.ps1') -Destination $output
if($variant[0] -eq 'net6.0') { & (Join-Path $development 'tools\copy-win7-runtime.ps1') -Destination $output -Architecture $Architecture }
$manifest=[ordered]@{version=1; files=@()}
$manifest.files=@(Get-ChildItem -LiteralPath $output -Recurse -File | Where-Object {
    $_.Extension -notin @('.dat', '.handles', '.log', '.db', '.pk8', '.bak', '.dmp')
} | ForEach-Object {
    [ordered]@{path=$_.FullName.Substring($output.Length+1).Replace('\','/');sha256=(Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash;size=$_.Length}
})
$manifestPath=Join-Path $root ('server-merged\'+$variant[2])
[IO.File]::WriteAllText($manifestPath,($manifest|ConvertTo-Json -Depth 5),[Text.UTF8Encoding]::new($false))
if($Test){
    $testData=Join-Path $development ('tools\checks\'+$variant[1]+'-'+(Get-Date -Format 'yyyyMMdd-HHmmss'))
    [IO.Directory]::CreateDirectory($testData) | Out-Null
    $testProfile = Join-Path $testData 'test-profile.ini'
    $profileText = Get-Content -LiteralPath (Join-Path $development 'config/profile.ini') -Raw
    # Synthetic character data stays in the isolated test directory.
    $profileText = $profileText.Replace('name_hex=', 'name_hex=546573744865726F').Replace('level=1', 'level=25')
    [IO.File]::WriteAllText($testProfile, $profileText, [Text.Encoding]::ASCII)
    & (Join-Path $output 'Nanaimo.Server.exe') --self-test --data $testData --native (Join-Path $output 'nanaimo_gameplay_bridge.exe') --profile $testProfile
    if($LASTEXITCODE){throw 'Merged gameplay verification failed'}
}
Write-Output "MERGED_BUILD_PASS $output"
}
