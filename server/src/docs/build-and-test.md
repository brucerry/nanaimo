# Build and test

Use the [workspace README](../../../README.md#build-from-source) for prerequisites and the
supported build commands. Run commands below from the workspace root.

## Current application

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File server/src/scripts/build_merged.ps1 -ModernOnly -Test
powershell -NoProfile -ExecutionPolicy Bypass -File launcher/build.ps1 -ModernOnly
dotnet run --project server/tools/launcher-checks/LauncherChecks.csproj -c Release -- server/tools/checks/launcher-local
```

The server build publishes the modern x64 runtime, compiles the native bridge,
and writes `client/server/server-merged/build-manifest*.json`. Test saves are
created under `server/tools/checks/`, separately from the player's database.
Stop every running game/server copy before integration checks; they bind real
loopback ports. The launcher checks create their own fixtures and include a
temporary Windows Forms window for layout checks.

The host's `--self-test` exercises migration, protocol handshakes, database
transactions, native dungeon state transfer, shops, saves, and GM operations.
`MigrationChecks.cs`, `CompatibilityChecks.cs`, `ShopDungeonChecks.cs`, and
`DungeonSaveChecks.cs` describe the exact coverage.

## Historical adapter checks

```powershell
python -B -m unittest discover -s server/src/scripts -p "test_*.py"
```

These tests also cover the retained PowerShell launcher, distribution allowlists,
and original native adapter. They are separate from the merged server's integration
checks. Some assume `server/src/tools/tcc/tcc.exe`, while the active toolchain is
at `server/tools/tcc/tcc.exe`; check each script's tool lookup before running it.
Original client byte/hash baselines intentionally differ from localized resources.
Never regenerate a client baseline merely to make a mismatch disappear.

`scripts/verify_package.py --source-only` checks the historical source manifest.
Manifest refresh tools record current source hashes; the historical export allowlist
is not a complete distribution specification for the merged application.

## Localization checks

The resource conversion helper operates locally and preserves ASCII fields,
numeric IDs, delimiters, GBK encoding, and per-field byte limits. It decrypts and
reencrypts catalogs and verifies the resulting plaintext before writing.

```powershell
python -m venv .venv-localization
.venv-localization/Scripts/python -m pip install -r server/tools/requirements-localization.txt
.venv-localization/Scripts/python server/tools/localize_catalogs.py
.venv-localization/Scripts/python server/tools/audit_language.py
.venv-localization/Scripts/python -B -m unittest discover -s server/tools -p "test_catalog_localization.py"
```

Without `--apply`, this is a preview. Edit the centralized locale files and use
`python server/tools/build_game_locale.py --apply`, then rebuild the application
when managed strings change. No historical backup is needed by this compiler.
An unchanged locale build produces no changes.

Automated checks do not establish complete gameplay coverage, correct rendering
of every glyph, or Windows 7 compatibility. In-game menus, tutorials, fonts,
artwork, multiplayer, and all dungeon stages need client-level acceptance testing.
