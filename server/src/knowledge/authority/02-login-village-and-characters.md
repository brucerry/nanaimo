# 02 Login, villages, and characters

> Translation draft: numeric evidence and code references are retained, but the prose still needs technical review. Use the [workspace guide](../../../../README.md) for current setup. Original research documents are preserved in the workspace archive `.work/original-source.zip`.

## Login and GS Access

| Operating Code | Length | Character |
|---|---:|---|
| 271A | 68 | Login name, grade, action door control, appearance |
| C351/C352 | 24/12 | GS Seeds, response lows and validation and initialization |
| C354/C355 | 8/728 | Information on village roles, HP, Copy opendata |

Network with Stand_Alone It's a stand-alone startup mode.Network Initial appearance can delay application D6 Effect, with reservation D7 pets; later requested driven C379 Restoring effects Network The delay is extended to Stand_Alone.

## Channel Selection and Reentry

```text
New Login Connection10  271B/12 -> 271C/176
Select Channel        Close Old GS0 Connect to Login10; Use Selection IPv4/Saved GS Port Reconnect GS0
Re-entry        C351/24 -> C352; Conditions of success BYTE+8=100, scene/Session Host Value WORD+0x0A
Reconstruction            Follow-up is request-driven C367/C368; Closing the path does not recreate the character
```

It's different from the normal initial login `2719/271A -> 2732 -> 271B` Order. Re-entry is based on the initial frame that the new login package solves271B (`stage0`) .Only when a live connection is matched within the source endpoint and the old agent and backend registration is fully released can the adaptor retain the original logical slot, UID The new connection replaces the village character readiness, room membership, combat status and one-time marking IP Multiple sessions remain independent when there is disagreement, and do not merge directly by address.

## Page Life Cycle

```text
C365/C366  Villages/World Transmission and Destruction
C367/C368  Create Page and Local Roles
C36C       Normal Page Completion
CB21       Move/Activity, and partial delay of door control
```

C365 Ban the creation or re-establishment of local roles during destruction, which remains request-driven C367/C368 Construct.selector4 Not confirmed C36C: Other C367/C368 After the border, both the single machine and the network path directly activates the first CB21 Trigger C47F”. Other choosers keep C36C -> First CB21 The order of that. Super Boss Returning failed sample, selector4/page18 It's C367/C368 After that CB21, But I didn't C36C/C47F.The sample describes the impact of a lack of door control and does not support the voluntary addition of character envelopes during destruction.

## C368 Character Information Carrier

```text
+0x34 hp_max      u16
+0x36 mp_max      u16
+0x38 hp_current  u16
+0x3A mp_current  u16
```

These fields can't be transposed directly to 271A, C355, C47F, C377, CF88 or CF8A Other structures.`0x006FC343..0x006FC3A4` The static call chain indicates that the package directly calls character information HP/MP Sets the function, there is no hierarchical query or a hierarchically extrapolated operation271A/CF71/CF88 Synchronization;for experience value strategy see07.

## Remote Character C36A/C36B

```text
C36A Length       112
+0x3C WORD      character key carrier;validated security test value as80
+0x3E WORD      Checked for character call chain UID << 4 Carrier
+0x58 DWORD     ((Y & 0x3FF) << 12) | ((X & 0x3FF) << 2)
+0x6E WORD      Character UID
C36B Length       12
```

C36A The structure is effective and does not represent a visible character/Page Attendance, scene insert, resourcesReady, visible tags and time series for execution.

## C47F Attach

C47F/52 For specific roles. Process function `sub_643310` Walk Through Character List, Package WORD `+0x08` And the character WORD `+0x08` Match.

```text
UID Comparison        0x00643349..0x00643357
Apply look        0x00643384..0x0064338C
                0x00643478..0x00643484
```

Current Structure at Remote C36A And then immediately the same UID Send C47F, Carrying pets Current/Maximum age, D0..D5 Equipment/Decorations, D6 Effects, D7 pets and D8 Foundation/Gender Block.`sub_643310` Only `actor+0x3410` Implementation of full pet phase non-zero/Global branch. So the adapter records the match between the character and the target, three different transfers in the character CB21 Try again when the package arrives C47F, Three times; not three times by the timersrcYes `release/components/channel_reentry/current_runtime.inc`, Attached to the focus of the test branch consists of immediate dispatch and three moves to trigger retry.srcis not equal toclientVisible effects learned.

Character Orientation and `C36A -> immediate C47F -> three distinct CB21/retry C47F pairs` (C36A, Immediately C47F, Three different CB21 Trigger one at a time C47F Retrying) order is supported by static base and host self-measurement. No original remotes for high-grade pets are maintained C47F Communications records, so long-distance pets/Visible restoration of effects is still to be run.

Local villages are subject to the use of independent gate control C47D Send it after success C368, C47F.selector4 _Other Organiser C367 Send on Request C368, And then for the first time CB21 Send once C47F, Not waiting for the option to be omitted C36C.Fitr Bytes Overrided `C365 -> C366/C379/C389`, `C367 -> C368`, `CB21 -> C47F`; These results are not provenclientVisible Effects.

## View data and social routers

- C376/C377 for viewing target character information; remote C377 Shouldn't trigger unrelated locals C379/C389 Send.
- CB22 Normal socialization/Emoticons are restricted by page range.
- CB23 Source of use UID, Name and Filtered GBK text;denial of invalid or empty text.
- Active connection use dynamics UID21/22 . . . . . ..do not synthesize UID1 As the current multiplayer character.

## Identity book

```text
Storage Location/Read Functions | Packet Offset | Equal Comparison/Attribution | Numerical Source | Effective connection/Status
```

Synthetic values meet local equivalents, but do not therefore become original accounts UID.

## Visibility scope of evidence

Existing evidence does not yet cover the full visibility of room preparation and village roles when homeowners enter first and other members join, nor do they cover two-way, long-range, high-level pets, D6 Effects & Remote CF9C Transformation/Full presentation of the start-up effect.C36A/C47F Keeps the request driven and is on the correct page; C365 No re-establishment of character during destruction.

## Prepare the room boundary

Prepare room appearances, CF71/CF72/CFDA Construct, Progress and Escort We'll see the difference, the location of the different modes05; I'll see you at the address08.The presence of a line in the list of members does not mean that there is a character in the eyes.

## A start-up tutorial door control

Cardinal access determination to login fields controlled by adaptor: 

```text
271A byte +0x0E
  -> sub_4E3830
  -> Character Info Object byte +3
C352 Success
  -> sub_4DFAA0 Local phase5
sub_4DF1E0 phase5
  byte +3 == 0  -> Global state12 (Bishop Cheng) 
  byte +3 != 0  -> Global state1  (Skip the tutorial) 
```

Network Mode uses character information parameters `skip_tutorial` Controls the tutorial entrance. Skipping path is still requested-driven: C351 Just got it C352, ThenclientDirectly C354; Adapter returns C355, Re-apply the character information door. Send it back EA61 scene0.This path does not send a tutorial EA61, It's not synthetic C353.

clientOperation and observation logs `C351 -> C352 -> C354`, Nothing C353, And then it came up `C355 -> EA61(scene0) -> C367/C368`, And you can see that you enter the game directly. This observation only shows the range of the recorded operations, not the current one release Re-admissibility.

## Apartment/OzVill Portal Call Chain

The world of apartments is independent GS scene boundary.`C358/8 ENTER_OZVILL` Only contains the header.`C38D/28 REQ_MOVE_MINIROOM` Yes WORD+8 Carry `how_move`, Yes +0x0A Carry 16 Byte Owner ID; The last two bytes of the declaration are not initialized and must be ignored.`C38E` The condition for success is BYTE+8=10, Read range to +0x6F, Shortest112bytes;including mobile type, owner/Character setdata, Access count and two fixed BoardNWall Records.

Operation observation in t=16s Copy that C358, Yes t=17..31s Eight frames C38D, The sample therefore does not support the explanation for the 'request not to reach the adapter' C38E The home owner bytes are taken from the configured character information without replaying an abnormal format in the request/Home main field contaminated with stack contents. This is a source and adapter custom policy and does not prove the original account number UID.

Room object loading is independent C392/C393 Boundary, see03, Not on your own C424.The results of the host's construction do not support the full presentation of the character of the own apartment in visible access, refurbishment and remote visitors C40A See also route03.
