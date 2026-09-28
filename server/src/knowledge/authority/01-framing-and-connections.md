# 01 Framing and connections

> Translation draft: numeric evidence and code references are retained, but the prose still needs technical review. Use the [workspace guide](../../../../README.md) for current setup. Original research documents are preserved in the workspace archive `.work/original-source.zip`.

## Level of evidence

| Category | What's there to prove | It doesn't prove anything alone |
|---|---|---|
| Original communications records | Precise bytes and order of original interactions | Missing field semantics supported by static call chain |
| Operational observation | Conduct attributable when interacting with local adapter | Original adapter policy |
| Static Analysis | Commands, Offsets, Branches, Constructive Functions and Process Functions | Run acceptance and inspection |
| Host self-measurement | Bytes, sequence, route, request door control and reset | clientRender |
| Playback/Simulation | Conduct under a clear presumption | Conduct of complete procedures and protected pathways |
| Custom Policy | Accompanying behaviour of the adaptor selection | Original adapter algorithm |
| Assumptions | An explanation that could be rejected by a follow-up experiment | The facts are confirmed |

The package conclusion uses the following record structure to distinguish the scope of application: 

```text
Connection | Assigner/controller | Status | Operating code (decimal)/Hexadecimal)  | Length of declaration/Length received
Trigger condition | Response | Request low position | S2C Serial number | Original Frame | Close Orientation
```

## S2C Pack head

- High-serial cycle: first response is 0, And then 1..100, Again 1 Action.
- Low response response to requests driven by requests 5 bits.
- Validation and torrents are independently taken from the first GS C351 Low 5 bits: `low | (low << 8)`.
- Radio, Boss, The room is reset and the movement path is recycled with the same serial number.
- The response length must overwrite the maximum deviation actually read in the processing function and all read bytes are initialized.

## Protocol bar groups

The semantics of the same operating code are limited by the following range:: 

```text
(connection, dispatcher/controller, client state, opcode, length)
```

The conclusion cannot be moved directly between different groups; the tectonic function, the processing function and the state door determine their scope of application, respectively C351/C352, C367/C368, CF70/CF71, CFEB/CFEC, CF7F/CF80, CFD3/CFD4, CF9C, CF88 with D012.

## Connection Type

Login, GS/Front end, room/Combats, data registration and UDP/P2P It must be recorded separately, even if they repeat the operation code or loop transfer. The actual source end point must be separated from the local backend loop.

### Inner game switch channels

Toggle Channel with New **Type 10 Login Connection**Implementation `271B/12 -> 271C/176`.Close old before choosing the channel **GS Connection 0** Connect to Login 10, We'll rebuild it now GS0 & Send `C351/24`.`C352 BYTE+0x08==100` It's a re-entry condition; `WORD+0x0A` Install a new scene/Session Host Value.`0x271D` Registered in protocol dollarsdata, But verified `CChangeChannelWindow` The chain won't send it out.

The adaptor cannot be immediately reconnected while the old backend is being written off GS Consider as a non-relative player and allocate the first empty slot. The current strategy is only in the initial frame of the new login package (`stage0`) Yes `271B` timemarks to switch intent; there must be a live connection and source IP Match, wait until the old agent and the back end are fully released before reverting to the slot, UID . Pages, rooms, combat status are still open for new interlinking internal cycles; no injections are made during the closure period C368/C47F.Same IP When matching is not the only one, no identity can be merged.

## Status Period (epoch) 

| Border | Meaning | Reset Check |
|---|---|---|
| CF09 | Early Transfer/Room boundary | Lists, character snapshots, one-time tags, expired transmission status |
| CF6C | Maps/Room identification confirmed | General objectives HP, Static/Run-time Selector, Box, Boss, Settlement |
| CF7F | Fight/Eventually Load Borders | Battle cycle, Boss Requesting count, real-time communication door control |
| Channel Selection | Refresh Login Connection 10, Close and reconnect GS0 | Keep the logical slot until the old agent and backend are released/information;replacement of pages, rooms and combat status |
| C365 | World/Page Destruction | It is forbidden to re-establish local roles here |
| C367/C368 | Create page and local roles | Character identity, information, equipment, HP/MP |

Even TCP A second entry copy is also part of the new room cycle, without disconnection.

## Operational attribution

Operating attribution requires silent baselines, single clock operational markers, and correspondence between new frames, payloads and local status writing. Heart beats, telemetry, automatic saving and timers are background flows that need to be distinguished.

Only if a caller or status writer bound to the operation is found can the request be considered to have been triggered by a click.

Requests that appear before and after the click should be considered as background traffic until the controlled differential experiment proves their causality.

## Anticipated requests did not arise

Expected request status setting function, status writing point, busy/The containment state, the conditions of completion, the control life cycle and the main cycle sequence are bound together.

## Breakpoint attribution

The interface text does not specify which end to close the patch. Evidence of the direction of closure includes: FIN/RST, `recv`/`send` Returns values, patch bugs, process survival and frame timetamps. Decoding text is not a substitute for original log or frame byte.

Saved Super Boss Returning to observation, CF73/CF1D YesclientRequest and before CF1E; This sequence cannot be extended to a generic breakout process CF99 It is another applied state boundary: even if the transmission connects to health, it may set conditions for local return to the village.

## A stable sealed family

```text
C351/C352  GS Seeds/Checksum border
CF09/CF0A  Transfer Period
CF6C/CF6D  Maps/Room cycle
CF70/CF71  Prepare room information/List
CFEB/CFEC  Room information/Level Load
CF7F/CF80  Preload/Final Load
CF87/CF88  Settlement
CF99/CF9A  Report of the abuse of surrender for long periods of time/Confirm
D00D/D00E  General objectives/Source termination state
D011/D012  Boss Overall status
D034/D035  Scene collections
```
