param([string]$Python = 'python',
      [ValidateSet('Modern','Win7')][string]$Target = 'Modern',
      [ValidateSet('x64','x86')][string]$Architecture = 'x64')
$ErrorActionPreference = 'Stop'
$workspace = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
Push-Location $workspace
try {
    if (-not [Environment]::Is64BitOperatingSystem) { throw 'The build host must be Windows x64.' }
    if (-not (Test-Path -LiteralPath 'server/tools/tcc/tcc.exe')) { & ./server/tools/setup-tcc.ps1 }
    $suffix = if ($Architecture -eq 'x86') { '-x86' } else { '' }
    $serverDirectory = if ($Target -eq 'Win7') { "bin-win7$suffix" } else { "bin$suffix" }
    $launcherDirectory = if ($Target -eq 'Win7') { "launcher-win7$suffix" } else { "launcher$suffix" }
    if ($Target -eq 'Win7') {
        & ./server/src/scripts/build_merged.ps1 -LegacyOnly -Architecture $Architecture -Test
        & ./launcher/build.ps1 -LegacyOnly -Architecture $Architecture
    } else {
        & ./server/src/scripts/build_merged.ps1 -ModernOnly -Architecture $Architecture -Test
        & ./launcher/build.ps1 -ModernOnly -Architecture $Architecture
    }
    if ($Target -eq 'Modern' -and $Architecture -eq 'x64') {
        dotnet run --project server/tools/launcher-checks/LauncherChecks.csproj -c Release -- server/tools/checks/launcher-ci
        if ($LASTEXITCODE) { throw 'Launcher checks failed.' }
        foreach ($suite in @('test_catalog_localization.py', 'test_client_text_localization.py')) {
            & $Python -B -m unittest discover -s server/tools -p $suite
            if ($LASTEXITCODE) { throw "Python checks failed: $suite" }
        }
    }
    $machine = if ($Architecture -eq 'x86') { 0x014c } else { 0x8664 }
    foreach ($file in @("client/server/$launcherDirectory/Nanaimo.Launcher.exe", "client/server/server-merged/$serverDirectory/Nanaimo.Server.exe")) {
        $bytes = [IO.File]::ReadAllBytes((Join-Path $workspace $file))
        $pe = [BitConverter]::ToInt32($bytes, 60)
        if ([BitConverter]::ToUInt16($bytes, $pe + 4) -ne $machine) { throw "Wrong executable architecture: $file" }
    }
    if (Test-Path -LiteralPath 'client/server/server-merged/data/game.db') { throw 'Build wrote into the player save directory.' }
    if ($Target -eq 'Modern' -and $Architecture -eq 'x64' -and (Test-Path -LiteralPath 'client/game.exe')) {
        & $Python server/tools/verify_portable.py
        if ($LASTEXITCODE) { throw 'Portable package verification failed.' }
        & $Python server/tools/verify_branding.py
        if ($LASTEXITCODE) { throw 'Brand verification failed.' }
        & $Python -B -m unittest discover -s server/tools -p test_startup_patch.py
        if ($LASTEXITCODE) { throw 'Client startup checks failed.' }
    }
    $launcher = Join-Path $workspace "client/server/$launcherDirectory/Nanaimo.Launcher.exe"
    & $launcher --verify
    if ($LASTEXITCODE) { throw "Launcher cannot use its $Architecture $Target server." }
    Write-Output "WINDOWS_BUILD_CHECKS_PASS target=$Target architecture=$Architecture"
} catch {
    $message = $_.Exception.Message.Replace('%', '%25').Replace("`r", '%0D').Replace("`n", '%0A')
    if ($env:GITHUB_ACTIONS -eq 'true') { Write-Host "::error title=Windows build check::$message" }
    throw
} finally { Pop-Location }
