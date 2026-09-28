# Startup and source map

The supported startup chain is:

```text
client/Start-Game.exe
  -> launcher/native-entry/entry.c selects the Windows runtime
  -> client/server/launcher[/legacy variant]/Nanaimo.Launcher.exe --server-ui
  -> Local login -> Program.RunAsync
  -> managed host + native dungeon bridge
  -> local account registration
  -> client/game.exe with loopback connection arguments
```

The actual legacy folder is `launcher-win7`, selected together with
`server-merged/bin-win7`. Modern Windows uses `launcher` and `server-merged/bin`.
Both share one save directory. A per-root mutex focuses an already-open manager.

| Source | Responsibility |
| --- | --- |
| `../../../launcher/Program.cs` | Find runtime settings; verify required files; register profiles/accounts; start/stop processes |
| `../../../launcher/ClientLocator.cs` | Prefer adjacent game.exe, then validate configured paths |
| `managed-host/Program.cs` | Initialize SQLite, recover journals, start workers and listeners, run self-tests, stop |
| `managed/Services/NetworkHostService.cs` | Connections, dispatch, world and gameplay handlers |
| `managed/Services/DatabaseService.cs` and partial classes | Persistence and transactional gameplay operations |
| `managed/Services/NativeDungeonPool.cs` | Isolated native room workers |
| `managed/Services/CardCatalog.cs` | Shared AES/GBK catalog decoding |
| `adapter/nanaimo_gameplay_bridge.c` | Native bridge executable entry |
| `release/components/` | Native packet, session, combat, inventory, and progression components |

The launcher configures the host with login port 11005, world port 12050, and
profile port 11999. The host also needs 22051–22054, 51005, 51999, 52050, and
62050–62055 for auxiliary and native services. Self-tests use different primary
login/world/profile ports but share native ports, so stop the normal server first.

The launcher writes `StateOption/gamestartoption.ini`, registers the account on
the loopback profile listener, then starts `game.exe` from its own directory.
The native entry does not require CMD or PowerShell during normal play.

The historical `gui_launcher/nanaimo_launcher.ps1`, `client_connect.ps1`, and
`start_nanaimo_launcher.bat` describe the older standalone adapter workflow.
They remain research references and are not the portable package's launch entry.
