# Runtime dependencies

The supported runtime is the complete `client/` directory described in the
[workspace README](../../../README.md). Players launch `Start-Game.exe` and do
not install development tools.

Keep these components together:

- `game.exe`, the client data tables, graphics, audio, and required legacy DLLs.
- `server/launcher/` and `server/server-merged/bin/` for Windows 10/11.
- The corresponding `launcher-win7/` and `bin-win7/` folders for the legacy target.
- `server/launchsettings.json` and `server/config/profile.ini`.
- `resources/data/` beside each managed executable; catalog loaders resolve these
  paths relative to `AppContext.BaseDirectory`.

The included managed programs are self-contained x64 deployments. Optional local
x86 outputs use `launcher-x86`/`bin-x86` or `launcher-win7-x86`/`bin-win7-x86`.
The native game and dungeon worker retain the original 32-bit formats. Development needs the .NET 8
SDK, bundled TinyCC, and Windows SDK UCRT files; Python is used by optional checks.

`game-unpacked.exe` and `original-installers/` are reference inputs, not part of
the normal launch sequence. Do not replace the active client with them without
understanding the existing compatibility patches and hash baseline.

The source does not include a complete implementation of the proprietary client.
Some displayed text is embedded in executable resources, compressed files, or
images and cannot be changed by editing server strings alone.
