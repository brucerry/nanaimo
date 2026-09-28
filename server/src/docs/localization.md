# Editing the game text

The editable language files are in `localization/zh-HK/` at the workspace root.
They use UTF-8 and Traditional Chinese characters, with colloquial Hong Kong
Cantonese wording. Keep filenames, identifiers, comments, and documentation in English.

| File | Content |
| --- | --- |
| `catalogs.json` | Client dialogue, stories, items, pets, quests, and other encrypted catalog text |
| `managed.json` | Launcher, GM controls, server messages, and village bots |
| `ui.json` | Text rendered into buttons, menus, tutorial pictures, and other sprites |

Search for the displayed words and edit the corresponding JSON value. Dialogue
keys such as `dialog.10059` correspond to native string-table IDs; `story.*` keys
identify story fields. Bitmap keys name a tutorial, sprite, or interface resource.
Other catalog keys are stable identifiers shared by repeated text.

The binding JSON files alongside `zh-HK/` map those keys to their destinations.
Do not change IDs, field indexes, format arguments, C# interpolation expressions,
keyboard shortcuts, or sprite coordinates when making a wording-only change.
For C# literals, values preserve the source's existing escape sequences.

## Build the edited text

Python is only needed by developers. Players can use `client/Start-Game.exe`.
From the workspace root, set up the localization environment once:

```powershell
py -m venv "$env:LOCALAPPDATA\Nanaimo\localization-venv"
$python = "$env:LOCALAPPDATA\Nanaimo\localization-venv\Scripts\python.exe"
& $python -m pip install -r server/tools/requirements-localization.txt
```

Then validate and apply:

```powershell
& $python server/tools/build_game_locale.py
& $python server/tools/build_game_locale.py --apply
& $python server/tools/audit_language.py --active-only
```

Close the launcher, client, and server before applying. Changes to catalog and
sprite files take effect the next time the game starts. Changes to `managed.json`
also require rebuilding the server and launcher, **one after the other** because
they share the gameplay project:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File server/src/scripts/build_merged.ps1 -Test
powershell -NoProfile -ExecutionPolicy Bypass -File launcher/build.ps1
```

The compiler synchronizes encrypted client catalogs with the development and
published server catalogs. It preserves numeric fields, image dimensions, frame
metadata, and interface resource IDs. Templates contain the background artwork
with the managed text regions erased; historical asset backups are not needed.
A cache skips rendering unchanged sprites. Editing a JSON value invalidates its
cache entry automatically. Do not edit generated client artwork directly.

## Encoding and layout constraints

The legacy client reads GBK (code page 936). Common Cantonese characters work,
but some Hong Kong extension characters are outside that encoding. The compiler
rejects unencodable text, altered format placeholders, excess field bytes, and
text that does not fit a sprite. It never silently truncates a translation.
Bitmap text uses Microsoft JhengHei Bold and can use its wider Unicode coverage.

The native `^&` sequence separates dialogue lines. The pet confirmation contains
three explicit lines because the client draws three buffers; blank trailing
lines previously exposed uninitialized text. Its reviewed binding also checks
individual line lengths against the client's fixed display buffers.

The legacy `Animaloption.ini` keys are Korean encoded in CP949. They are not
Simplified Chinese and must keep their original bytes to match the client.
The language audit uses this explicit encoding instead of misreading them as GBK.

## Verification and limits

```powershell
& $python -B -m unittest discover -s server/tools -p "test_*localization.py"
& $python -B -m unittest discover -s server/tools -p "test_*sprites.py"
& $python -B -m unittest discover -s server/tools -p "test_game_ui.py"
& $python -B -m unittest discover -s server/tools -p "test_startup_patch.py"
```

Checks cover catalog round trips, preflight failure without partial writes,
repeat-build stability, sprite geometry, text edits changing rendered pixels,
and the startup-cover removal. The installed sprite test compares generated
output with every bound client resource, including its loose copies.

The workspace audit covers readable text and decrypted client catalogs. It does
not interpret arbitrary binary bytes as Chinese, rewrite player saves/chat, or
modify third-party conversion dictionaries. Font-rendered regions are checked
against the editable text, rather than trusting OCR to distinguish scripts.
OCR was used to discover old artwork; small or stylized text can evade it.
A complete interactive playthrough and every proprietary image are not certified.

The startup cover and its identified load/draw code remain removed. The earlier
gold tutorial banner remains empty. Current player saves are preserved. Historical
recovery copies and obsolete evidence are not needed by the compiler. Automatic
approval review blocked their requested deletion, including explicit backup paths,
with only the reason "blocked by policy". They remain in the workspace. Unused
generated templates also remain because their deletion was blocked. The active
bindings determine which templates are built; unreferenced files are not installed.

The project retains one canonical AGPLv3 license at the workspace root. Third-party
notices are documented separately in [third-party-notices.md](third-party-notices.md).

The `--active-only` audit excludes historical `.work` evidence, generated build
output, test fixtures, and third-party dependency source. Omit that flag to inspect
the whole workspace; retained historical files still produce findings. A passing
active audit is not a claim that those historical copies were cleaned.

The separate historical Python script suite has three previously established
failures: `test_launcher_and_connector_pin_baseline`,
`test_connection_details_are_not_presented`, and
`test_fixed_transport_and_profile_contract_remain_internal`. Those legacy
contracts are not the locale compiler tests above.
