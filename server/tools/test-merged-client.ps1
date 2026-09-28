param([switch]$ServerOnly)
$ErrorActionPreference='Stop'
$development=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$workspace=Split-Path $development -Parent
$root=Join-Path $workspace 'client\server'
$artifacts=Join-Path ($workspace+'-archive') 'artifacts'
$hostExe=Join-Path $root 'server-merged\bin\Nanaimo.Server.exe'
$testData=Join-Path $artifacts 'migration-live'
$native=Join-Path $root 'server-merged\bin\nanaimo_gameplay_bridge.exe'
$profileFile=Join-Path $root 'config\profile.ini'
$arguments='--data "'+$testData+'" --native "'+$native+'" --profile "'+$profileFile+'" --login-port 11005 --world-port 12050 --profile-port 11999'
$server=Start-Process -FilePath $hostExe -ArgumentList $arguments -WorkingDirectory $root -WindowStyle Hidden -RedirectStandardOutput (Join-Path $artifacts 'migration-live.log') -RedirectStandardError (Join-Path $artifacts 'migration-live-error.log') -PassThru
Write-Output "MERGED_SERVER_PID=$($server.Id)"
for($attempt=0;$attempt -lt 50;$attempt++) {
    if($server.HasExited){throw 'Merged server exited; check migration-live-error.log in the archive artifacts folder'}
    try { $registration=[Net.Sockets.TcpClient]::new('127.0.0.1',11999); break } catch { Start-Sleep -Milliseconds 100 }
}
if(-not $registration){throw 'Profile listener did not start'}
try {
    $s=$registration.GetStream(); $s.ReadTimeout=5000
    $profile=[IO.File]::ReadAllBytes((Join-Path $root 'config\profile.ini'))
    $len=[BitConverter]::GetBytes([int]$profile.Length)
    $s.Write($len,0,4); $s.Write($profile,0,$profile.Length)
    $ack=New-Object byte[] 3; $offset=0
    while($offset -lt 3) { $n=$s.Read($ack,$offset,3-$offset); if($n -eq 0){throw 'Profile connection closed'}; $offset+=$n }
    if([Text.Encoding]::ASCII.GetString($ack) -ne "OK`n"){throw 'Profile rejected'}
} finally { $registration.Dispose() }
if(-not $ServerOnly) {
    $info=[Diagnostics.ProcessStartInfo]::new()
    $info.FileName=Join-Path $workspace 'client\game.exe'
    $info.WorkingDirectory=Split-Path -Parent $info.FileName
    $info.UseShellExecute=$false
    $info.Arguments='-q :1:1:0:3:4:-i 5:-r 6:7:1:127.0.0.1:'
    $game=[Diagnostics.Process]::Start($info)
    Write-Output "GAME_PID=$($game.Id)"
}
