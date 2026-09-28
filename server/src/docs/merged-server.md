# Merged server architecture

The deployed package is `client/server/`, reached through `client/Start-Game.exe`.
The active source is `server/src/`; `serverMode` is `merged`. Older instructions
for a separate native server or a `runtime` directory no longer apply.

## Components

- `managed/`: gameplay rules, binary protocols, accounts, inventory, quests,
  shops, pets, cards, apartments, trading, auctions, parties, relationships,
  arena/leisure modes, and SQLite persistence.
- `managed-host/`: command-line host, shutdown, recovery, native worker processes,
  and integration checks. No WPF application or web administration server is launched.
- `adapter/nanaimo_gameplay_bridge.c` and `release/components/`: the retained
  native dungeon implementation. Native code continues to calculate dungeon
  damage, boss behavior, projectiles, drops, and settlement.
- `../../../launcher/`: the Windows Forms manager and GM tools.

The original bundle credits the native dungeon framework to Wo Ai Luo. The
managed database retains some imported models and historical fields for compatibility.

## Local account flow

The launcher accepts 1 to 64 non-control characters as an account name. The
loopback account endpoint opens or creates that account without a password.
Each account has one character. New accounts enter the original character creation
screen; existing characters resume saved progress. New character names must fit
the native 14-byte GBK limit. Tutorial completion or skipping is persisted.

`launcher/local-account.json` remembers the last account. `config/profile.ini`
supplies native compatibility defaults and is not an authoritative replacement
for a character already stored in SQLite.

## Native dungeon handoff

C# handles village/world state. The world connection's `CF09` transition starts
a native dungeon session; relevant CF/D0 combat and room packets are forwarded
to the native worker. `CF1D` returns control to the managed village handler.
Arena and leisure connections remain in the managed implementation.

The internal bridge uses a fixed 5,120-byte little-endian state structure.
`F100` imports state, `F101` queries it, `F102` returns it, and `F103` reports
native-validated hits for quest progress. Client packets cannot invoke these
internal state operations through the public world handler.

| Byte offsets | State |
| --- | --- |
| 0–47 | Version, character ID, level/experience, HP/MP, two 64-bit balances |
| 48–95 | Skill points/slots, revivals, quick-slot expiry, pet, combat values, name length, gender |
| 96–159 | GBK name, equipped appearance, time base, pet gems |
| 160–271 | 16 skill grades, six quick slots, native instance handles |
| 272–1951 | Quantities for 420 native cards |
| 1952–3995 | Item count and up to 255 code/quantity entries |
| 4000–5023 | 256 native instance handles |
| 5024–5051 | Dungeon grade and highest cleared stages |

Experience uses `50 * (level - 1) * level` at the native handoff boundary.
Coins, cash, cards, and items are persisted as transactional deltas so concurrent
changes are retained. Commit IDs prevent duplicate rewards during journal recovery.
Startup recovers `data/native-journal/` before resetting offline state; an
unrecoverable journal blocks startup and is retained for investigation.

Parties have isolated native processes; members of a party share the native
six-session room. Additional workers allocate loopback port blocks within
53000–53998. Windows Job Objects terminate workers if the host exits unexpectedly.
The native quick-slot representation supports only 255 instance handles; oversized
inputs are rejected, rather than silently truncated.

## Verification boundaries

`scripts/build_merged.ps1 -Test` exercises state round trips, login/world handshakes,
inventory, pets, cards, quest and auction lists, wishlists, dungeon entry/return,
duplicate settlement, rollback, save persistence, and GM operations in isolated data.
These checks do not validate every original gameplay feature or all client visuals.
Multiplayer combat, all bosses/stages, relationship flows, external friend bridges,
and every arena/leisure mode still need client-level acceptance tests.

There is no supported switch back to the historical standalone native server in
the current portable package. Back up the entire save directory before experimenting.
