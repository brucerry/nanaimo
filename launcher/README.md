# Launcher

The active launcher is a C# 12 Windows Forms application. It provides local
account login, server lifecycle controls, profile settings, save archives, and
GM tools. A small native Windows executable selects the packaged runtime.

From the workspace root, with the game and launcher closed:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File launcher/build.ps1 -ModernOnly
```

This publishes `Nanaimo.Launcher.csproj` for `net8.0-windows` into
`client/server/launcher`, then builds
`client/Start-Game.exe`. It references `server/src/managed/Nanaimo.Gameplay.csproj`.
The .NET 8 SDK and TinyCC are required. Run `server/tools/setup-tcc.ps1` to
download and verify the compiler. Omitting `-ModernOnly` additionally builds
the legacy .NET 6 target and requires Windows SDK UCRT redistributables.
Players use the self-contained outputs and do not need those build tools.
Add `-Architecture x86` to build separate 32-bit managed launchers. The native
entry selects the matching x86 folder on 32-bit Windows; `--x86` forces it on a
64-bit test machine. `--win7` selects the .NET 6 variant.

To rebuild only the native entry point:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File launcher/build-native-entry.ps1
```

| File | Responsibility |
| --- | --- |
| `native-entry/entry.c` | Detect Windows version and launch the matching server manager |
| `Program.cs` | Locate runtime, start/stop server, register account, launch client |
| `AccountLoginControl.cs` | Account input and remembered account |
| `ServerManagerForm.cs` | Runtime status, ports, logs, and profile controls |
| `GmManagementControl.cs` | Offline character editing and runtime inspection |
| `ClientLocator.cs` | Resolve the client without escaping the package directory |
| `SaveReset.cs` | Archive saves after checking that the game and server are stopped |

Run launcher fixtures with the command in the [main README](../README.md#build-from-source).
The Windows 7 build remains a compatibility target without Windows 7 hardware validation.
