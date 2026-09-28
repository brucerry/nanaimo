param([Parameter(Mandatory=$true)][string]$Donor)
$ErrorActionPreference='Stop'
$root=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$target=Join-Path $root 'src\managed'
$records=@()
foreach($folder in @('Services','Models')) {
    foreach($file in Get-ChildItem -LiteralPath (Join-Path $Donor $folder) -Filter '*.cs') {
        $relative=$folder+'/'+$file.Name
        $copy=Join-Path $target $relative
        $excluded=$file.Name -in @('WebAdminServer.cs','ClientItemIconService.cs','WebChannelConfiguration.cs','ServerConfigurationItem.cs')
        if(-not $excluded -and -not (Test-Path -LiteralPath $copy)){throw "Missing donor source: $relative"}
        $originalHash=(Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash
        $currentHash=if(Test-Path -LiteralPath $copy){(Get-FileHash -LiteralPath $copy -Algorithm SHA256).Hash}else{$null}
        $records += [ordered]@{path=$relative;sourceSha256=$originalHash;importedSha256=$currentHash;status=$(if($excluded){'excluded-ui-or-web'}elseif($originalHash -eq $currentHash){'identical'}else{'adapted'})}
    }
}
$resources=@(Get-ChildItem -LiteralPath (Join-Path $Donor 'resources\data') -File | ForEach-Object {
    $copy=Join-Path $target ('resources\data\'+$_.Name)
    $expected=(Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash
    if((Get-FileHash -LiteralPath $copy -Algorithm SHA256).Hash -ne $expected){throw "Resource mismatch: $($_.Name)"}
    [ordered]@{path=('resources/data/'+$_.Name);sha256=$expected;size=$_.Length}
})
$manifest=[ordered]@{version=1;scope='gameplay services, models, and data; no donor UI or web server';sources=$records;resources=$resources}
$path=Join-Path ((Split-Path $root -Parent)+'-archive') 'artifacts\gameplay-import-manifest.json'
[IO.File]::WriteAllText($path,($manifest|ConvertTo-Json -Depth 6),[Text.UTF8Encoding]::new($false))
$records|ForEach-Object {[pscustomobject]$_}|Group-Object status|Select-Object Name,Count
Write-Output "IMPORT_AUDIT_PASS resources=$($resources.Count) manifest=$path"
