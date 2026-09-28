$ErrorActionPreference='Stop'
$workspace=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$prefix=$workspace+[IO.Path]::DirectorySeparatorChar
$moves=@(
    @('launcher','Operating environment\launcher'),
    @('server','Operating environment\server'),
    @('server-merged','Operating environment\server-merged'),
    @('QQFlying Island Shell Dehumingclient','Operating environment\QQFlying Island Shell Dehumingclient'),
    @('launchsettings.json','Operating environment\launchsettings.json'),
    @('launcher.log','Operating environment\launcher.log'),
    @('launcher-src','Development of information\launcher-src'),
    @('server/src','Development of information\server/src'),
    @('backups','History Archive\backups'),
    @('artifacts','History Archive\artifacts'),
    @('README.md','History Archive\Pre-recognition.md'),
    @('tools','Development of information\tools')
)
$plans=@($moves | ForEach-Object {
    $from=[IO.Path]::GetFullPath((Join-Path $workspace $_[0]))
    $to=[IO.Path]::GetFullPath((Join-Path $workspace $_[1]))
    if(-not $from.StartsWith($prefix,[StringComparison]::OrdinalIgnoreCase) -or -not $to.StartsWith($prefix,[StringComparison]::OrdinalIgnoreCase)){throw 'Move escapes workspace'}
    if(-not (Test-Path -LiteralPath $from)){throw "Source missing: $from"}
    if(Test-Path -LiteralPath $to){throw "Destination exists: $to"}
    [pscustomobject]@{From=$from;To=$to;RelativeFrom=$_[0];RelativeTo=$_[1]}
})
$processes=Get-CimInstance Win32_Process
foreach($process in $processes){
    if($process.ExecutablePath -and $process.ExecutablePath.StartsWith($prefix,[StringComparison]::OrdinalIgnoreCase) -and
       $process.Name -in @('game.exe','Nanaimo.Launcher.exe','Nanaimo.Server.exe','nanaimo_gameplay_bridge.exe','nanaimo_adapter.exe')){
        throw "Close running application before moving: $($process.Name) $($process.ProcessId)"
    }
}
$critical=@('QQFlying Island Shell Dehumingclient\game.exe','server\profile.ini','server\nanaimo_adapter.exe','server-merged\data\game.db','server-merged\build-manifest.json')
$hashes=@($critical | ForEach-Object {
    [pscustomobject]@{Before=$_;After=('Operating environment\'+$_);SHA256=(Get-FileHash -LiteralPath (Join-Path $workspace $_) -Algorithm SHA256).Hash}
})
$records=@()
foreach($plan in $plans){
    $files=@(Get-ChildItem -LiteralPath $plan.From -Recurse -File -Force)
    $bytes=($files|Measure-Object Length -Sum).Sum
    [IO.Directory]::CreateDirectory((Split-Path $plan.To -Parent))|Out-Null
    Move-Item -LiteralPath $plan.From -Destination $plan.To
    $after=@(Get-ChildItem -LiteralPath $plan.To -Recurse -File -Force)
    $afterBytes=($after|Measure-Object Length -Sum).Sum
    if($files.Count -ne $after.Count -or $bytes -ne $afterBytes){throw "File inventory mismatch: $($plan.RelativeFrom)"}
    $records += [pscustomobject]@{From=$plan.RelativeFrom;To=$plan.RelativeTo;Files=$after.Count;Bytes=$afterBytes}
    Write-Output "MOVED $($plan.RelativeFrom) -> $($plan.RelativeTo) files=$($after.Count)"
}
foreach($file in $hashes){
    if((Get-FileHash -LiteralPath (Join-Path $workspace $file.After) -Algorithm SHA256).Hash -ne $file.SHA256){throw "Critical file changed: $($file.After)"}
}
$record=[ordered]@{CompletedAt=[DateTimeOffset]::Now.ToString('O');Moves=$records;CriticalFiles=$hashes;DeletedFiles=0}
[IO.File]::WriteAllText((Join-Path $workspace 'History Archive\Directory Record.json'),($record|ConvertTo-Json -Depth 5),[Text.UTF8Encoding]::new($false))
Write-Output 'WORKSPACE_ORGANIZED files_preserved=true critical_hashes_match=true'
