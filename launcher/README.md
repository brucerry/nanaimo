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
`client/Start-Game.exe` and the x86 `client/ddraw.dll` display shim. It references `server/src/managed/Nanaimo.Gameplay.csproj`.
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
| `AccountLoginControl.cs` | Account input and remembered account/display preferences |
| `GameDisplaySettings.cs` | Resolution presets and generated display/renderer configuration |
| `GameGraphicsQuality.cs` | Detect the highest supported MSAA level on the default GPU |
| `native-display/display.c` | Scale the completed 800 x 600 frame and map mouse input back to it |
| `ServerManagerForm.cs` | Runtime status, ports, logs, and profile controls |
| `GmManagementControl.cs` | Offline character editing and runtime inspection |
| `ClientLocator.cs` | Resolve the client without escaping the package directory |
| `SaveReset.cs` | Archive saves after checking that the game and server are stopped |

Run launcher fixtures with the command in the [main README](../README.md#build-from-source).
The Windows 7 build remains a compatibility target without Windows 7 hardware validation.

## Display compatibility

The original client mixes Direct3D sprites and GDI text on an 800 x 600 surface.
Scaling those operations independently can lose text or move it away from its
background. The game-specific shim forwards the client's two DirectDraw imports
to the bundled dgVoodoo2 renderer, intercepts only the final full-frame primary
surface blit, and scales window mouse messages back to native coordinates. It is
an x86 DLL for the original x86 game, regardless of the managed launcher target.
The renderer stays at `Resolution = unforced`; do not force its internal size.

`display.ini` and `dgVoodoo.conf` are regenerated beside the client on launch.
Preferences live in `client/server/launcher/display-settings.json`. The cleanup
script removes these generated files. Renderer provenance and separate terms
are recorded in `client/dgVoodoo-NOTICE.txt`.

Rebuild and test just the shim with:

```powershell
./launcher/build-native-display.ps1
./launcher/test-native-display.ps1
```

The native checks cover all presets, frame destinations, edge/centre clicks,
wheel coordinates, and unchanged offscreen blits. They run in the Windows build
matrix without needing a GPU. They do not replace desktop gameplay checks for
dialogue, tooltips, fullscreen, and focus switching.

On every launch, `GameGraphicsQuality` queries the first hardware adapter through
D3D11 at feature level 10.0, matching the generated renderer settings. It selects
16x, 8x, 4x, or 2x MSAA, requiring positive quality-level support for RGBA8, BGRA8,
and D24S8 formats. Failed device creation or unsupported formats fall back to off.
The renderer is pinned to adapter 1 so it uses the same default adapter as the probe.
The `Bilinear2DOperations` setting smooths the final DirectDraw stretch separately
from MSAA; native resolution and mouse-coordinate mapping remain unchanged.

Capability detection follows [CheckMultisampleQualityLevels](https://learn.microsoft.com/en-us/windows/win32/api/d3d11/nf-d3d11-id3d11device-checkmultisamplequalitylevels).
The sample-count limit and 2D filtering option match the bundled dgVoodoo 2.87.5
configuration template. The launcher fixtures cover sample-count selection and
format-dependent fallback without requiring a particular GPU on CI runners.
