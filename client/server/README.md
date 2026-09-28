# Portable runtime

Open `../Start-Game.exe` and use **Local login** in the server manager.
Copy the entire parent `client` folder to move the game and its saves.

| System | Selected runtime |
| --- | --- |
| Windows 10/11 x64 | `launcher` and `server-merged/bin`, self-contained .NET 8 |
| Windows 7 SP1 x64 (optional local build) | `launcher-win7` and `server-merged/bin-win7`; not included in the clone-and-run package |
| Windows 10 x86 / Windows 11 x86 process (optional local build) | `launcher-x86` and `server-merged/bin-x86` |
| Windows 7 SP1 x86 (optional local build) | `launcher-win7-x86` and `server-merged/bin-win7-x86` |

Windows 7 also requires KB3063858 or KB2533623. The compatibility build has not
been tested on Windows 7 hardware; .NET 6 is out of support. The optional x86
builds run in CI on 64-bit Windows under WOW64, not on 32-bit Windows installations.

When built locally, both variants share `server-merged/data/game.db` and the adjacent native dungeon
saves. Run only one variant at a time. Stop it with **Stop server**.
Back up the complete `server-merged/data` folder after stopping both game and server.

`launchsettings.json` uses relative paths. `config/profile.ini` holds native
compatibility defaults; it does not replace existing character progress.
Logs are `launcher.log`, `server-merged/server.log`,
`server-merged/server-error.log`, and `server-merged/data/native.log`.

Source and build tools are outside the portable folder in `../../server` and
`../../launcher`. See the [workspace README](../../README.md) for development.
