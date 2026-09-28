# Game language source

Edit the UTF-8 files in [zh-HK](zh-HK/), then run
`python server/tools/build_game_locale.py --apply` from the workspace root.
See the [editing guide](../server/src/docs/localization.md) for setup, constraints,
rebuild commands, and verification.

- `zh-HK/catalogs.json`: encrypted catalog strings, including native dialogue IDs.
- `zh-HK/managed.json`: launcher, GM, diagnostics, and bot wording.
- `zh-HK/ui.json`: bitmap labels and tutorial wording.
- `*-bindings.json`: stable resource destinations and size/format constraints.
- `templates/`: current background artwork with managed lettering removed.
- `generated-state.json`: render cache; rebuilt automatically when inputs change.

Do not put source-language copies or old client backups in this directory.

`branding.json` records the current game name and reviewed logo backgrounds.
The former logo areas now contain repaired background artwork, with no replacement
wordmark. See [restoration details and prompts](branding-restoration.md).
The compiler updates their hashes; `python server/tools/verify_branding.py` checks
the installed artwork and native title. The game executable title is separately
pinned by `server/tools/client-startup-patches.json`.
