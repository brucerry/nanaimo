# Server development

This folder contains source and build tools. The deployed server and real saves
live in `../client/server/`; there is only one portable runtime.

From the workspace root, after closing the game and server:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File server/src/scripts/build_merged.ps1 -ModernOnly -Test
```

The build compiles the native dungeon bridge with TinyCC and publishes
the C# host for .NET 8. Run `tools/setup-tcc.ps1` to download the verified compiler. It writes runtime file manifests and runs
integration checks in fresh `tools/checks/` directories. The .NET 8 SDK is required. The optional legacy build also requires Windows
SDK Universal CRT redistributables. See the [main README](../README.md).
Pass `-Architecture x86` for a separate 32-bit managed host and
`-LegacyOnly` for just the Windows 7 .NET 6 target.

| Path | Purpose |
| --- | --- |
| `src/managed/` | Gameplay, protocol handlers, catalogs, and SQLite persistence |
| `src/managed-host/` | Console host, native worker management, and integration checks |
| `src/adapter/` | Native bridge entry points |
| `src/release/components/` | Native dungeon implementation retained from the original project |
| `src/gui_launcher/` | Historical PowerShell UI, catalogs, and inventory helpers |
| `tools/` | Compiler, diagnostics, launcher checks, and resource conversion |

Launch the app with `../client/Start-Game.exe` or this folder's `Start-Server.cmd`.
Both open the current C# server manager. Do not use the historical PowerShell UI
as the portable package entry point.

[Architecture](src/docs/merged-server.md) · [GM guide](src/docs/gm-guide.md) ·
[Build checks](src/docs/build-and-test.md) · [Protocol research](src/knowledge/README.md)
