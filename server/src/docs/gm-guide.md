# GM tools

Open `client/Start-Game.exe` and select **GM tools**. The account list reads the
actual SQLite accounts and characters. **Character settings** is a separate legacy
profile template and does not edit an existing character's saved progress.

| Section | Available operations |
| --- | --- |
| Character / Currency | Name, level, experience, balances, stats, unspent points, HP/MP recovery, pet level, GM status, account ban |
| Inventory | Search by name/code, show owned items, grant or remove quantities |
| Card collection | Search and adjust cards, up to 255 per card |
| Skills | Set grades 0–5; zero removes the skill |
| Quests | Grant scroll quests, activate, complete objectives, delete; objective completion does not directly grant rewards |
| Apartment / Shop wishlist | Inspect saved furniture and wishlist entries |
| Dungeon progress | Inspect managed clear records; does not overwrite native dungeon saves |
| Mentors | Inspect recruitment records |
| Friends / Friend requests | Inspect and delete relationships or requests with confirmation |
| Card exchange | Inspect auction records |
| Connections | Inspect live sessions and disconnect a selected session |
| Native dungeons | Inspect worker isolation keys, ports, membership, and extra workers |
| Sky arena / Parties / Trades | Inspect live rooms and groups |
| Audit log | Inspect the latest 200 character, stock, and skill changes |

Character, stock, and skill edits require the character to be offline. Database
transactions check this again before committing. Level changes set the matching
starting experience; maximum HP/MP are derived from level and attributes.
Character names must fit 14 GBK bytes. Native dungeon consumables have a 255-item
capacity; grants beyond it roll back. Equipped items and tutorial pets cannot be
removed down to zero.

Live snapshots use a local named pipe restricted to the current Windows user.
They require the server to be running. Offline save inspection and editing do not.
These tools do not change the native damage, projectile, boss, or drop algorithms.

Channel editing, bot configuration, auction cancellation, complete mentor actions,
and direct native stage-progress editing are not all exposed in this UI.

The server integration checks cover GM persistence, stock limits, online-edit
rejection, audit records, and named pipes. `server/tools/test-gm-ui.ps1 -ProcessId
<pid>` checks the 18 views after the GM tab is selected; it does not grant items,
delete records, or disconnect real players.
