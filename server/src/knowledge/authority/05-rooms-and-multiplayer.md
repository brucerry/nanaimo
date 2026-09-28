# 05 Rooms and multiplayer

> Translation draft: numeric evidence and code references are retained, but the prose still needs technical review. Use the [workspace guide](../../../../README.md) for current setup. Original research documents are preserved in the workspace archive `.work/original-source.zip`.

## Phase model

```text
CF09 Transfer
-> Owner/Room Creation
-> CF70/CF71 List of members and CF7E Ready/Teamshot
-> CFEB/CFEC Room information
-> CFD3/CFD4 and CFD5/CFD6 Peer Connectors
-> Preload CF80/CFEC
-> Every oneclient CF7F
-> Every oneclient Eventually CF80
-> Real time forwarding
-> Fight and settle
-> Leave/Break/Village recovery
```

Unrequested subgroups in a successful front stage do not authorize a later stage.

## Channel Selector Login andGSReentry

Channel Selector uses a new login connection10Yes`271B/12 -> 271C/176`, And then close the old oneGS0/login10, Reconnect selectedIPv4And what's savedGSPort asGS0& Send`C351/24 -> C352`.The retention of the same logical slot still requires completion of the old agent/Once destroyed at the back end, there is only one clear sourceIPMatch to unlock; character re-entry remains request-driven after re-entry.

Initialize the frame for every new login: S2C Sequence from0starting;first complete request fixes the receiver `checksum_seed=low5|(low5<<8)`; Low response to each copy trigger request5bits. The adaptor tectonic verification covers the first low09/16/0E, `0,1..100,1` Sequence rings, follow-up low-level copy, UID21Reuse andC355Consistency of information; these results do not constituteclientRun acceptance and acceptance. See you at the call address08.

## Room phase C47D and the reconstruction of villages

C47D/144 It's for village equipment/The shortcut changes are also used to prepare the room for editing `C47E -> C368 -> C47F -> C379 -> C3CC -> C44C`, To rebuild the appearance and follow up PET.In the room/In readiness, send only `C47E -> C379 -> C3CC -> C44C`: C368 Call scene destruction, clear shared_scene+0x24, and the initialization of the pre-controller shall be invalidated; therefore, C47F It'll be suppressed. It won't change CF80/Start building or falsifying a phase manager.

Village returns independently of request-driven.Selector4 From SuperBossPage18No operational observation returnedC36C; Corresponding processing inC367SendC368After that, allow the first timeCB21TriggerC47FAttached. Other Selector in FirstCB21Request still before attachC36C.Adaptor communication records verified to cover two branches, clientRendering effects not verified.

## The Raminos Village Forward Progress

Fifth village (hd4) There's an independent one64bit precondition field in**C355Full Frame+0x80..0x87**.The average first four villages show a stage of progress and compression that no byte replaces it. Its local movement word needs**Two in each front pair**; NormalCheckDungeonThe path is separated. Under zero mask conditionsclientIn progress, every pair is receivingC355This corresponds to the reported “challenge completion level before moving” borders/Writeer/Words/Message/Send address, page map and local map small parts abnormally attributable to08.

The current local custom policy is set only low44bits (22(a) To meet22Logical progress check while retaining final cleanup22And all higher places.**It doesn't work23A physical copy entry became available.** C355+0x78..0x7F No change; general table0x0F and stage display status1/0x55 The precondition fields are kept unchangedC355Other fields of the build path write after finished setting.**C354 Still DrivenC355; No Unrequested AddC367/C368, Re-establishment of the character orclientPatch.** We have to log in/Refreshing information; changing adaptor executable cannot update openedclientCache mask.

Preconditioned10Inspections and3190Isolationx86The case only validates the construction and does not show visible effects.clientRead-only snapshots record zero-covered and all22Operation observation involves fighting after entering the sixth copyresourcesFailed to find (06) , And from6Present7/8It's not just a little map.d07Superseding suppress (08) Incomplete border with pristine physical village is independent; additionalC355Filling or removing displays do not create missing map exports.

### Entity village routes and map content

The shipped fifth-village map package connects only the first six dungeon routes: page8, 7, 6, 11, 16, 17, Trigger as160to165.Page17 (Access6) One returned to page16Exports, **Not returned to page18 (Access7) Exports**.Remaining17Copies of planned pages with the same griddata, There is no page exit or a copy entry trigger. Therefore, light the small map slot8to23The corresponding area cannot be shown to be useful08.

The road map of the existing village is in the first place18Pages24The remaining planned route of the page contains16group two-way connections;all23Connected grid-point paths in each planned village area,17/18Page at the seventh entrance166Return with birth169Reservations. Road modifications are limited to17Collisions, targets and sources of arrival recordedWORD, Do Not Add167..182Copy trigger or alternative to combat. Accurate grid, direction and structure/I'll see you at the border08.OriginalresourcesThe sixth entrance to the package should be understood separately from the road map.clientIt's been observedpage18/index6/ep22The entrance, but the whole road/Collision and return to the seventh entrance/Re-entry not yet accepted18East End Connect18↔19, The unconnected border and final endpoint remain closedep23..38The battlefieldresources (06) .

**Blackscreen observation range: ** The black screen of the report is in the creation before entering the preparatory room/ListUI, No. No. No22Close the battleground and open the battleground of the sceneSSTG/SM2/IM3Existence or auxiliary image decode does not support thisUINormal rendering.Entry166I'm sorry0x402EF0 -> 0x42FBA0 (08) Switch to scene status9; Create/The specific loading or rendering failure point of the list background has not been verified.

clientIt'sd07Forced concealment of signs and village roads to become independent; the existence of roads does not eliminate the appearance/Get to the mark. SyntheticC367/C368Exchanges do not mean that the player movesVILUnit cannot independently prove all images or battlesresourcesMissing; place of playability depends on fightingresourcesMetagroups, Toptops, Images andclientActual conduct.

## Map Identities and Continue

Lumineos episode100Retain communications/Room ID; resourcesFind conversions and seven battlesresourcesSee the scope06, clientSee you at the selected address08.Seven battlesresourcesScope and originalresourcesThere is no contradiction in only six accessible accesses; the existing road map maintains the seventh entry path and returns signage, extending only to village roads.

CF77 It's oneresourcesHint, CF6C It's the entire chamber of authority, CFEB Provide Initial/Reload information at the stage, and CF8B Provide subsequent conversions.CF7F There's a consensus `hd/episode/dungeon/stage/difficulty` Stay closed until the metagroup.`absolute_slot=difficulty*3+stage_index`; BOSS, General objectives and boxes/The faller uses the same snapshot. It works CF77/CFEB Paths can be available CF6C ; hint is not a permission for a hybrid group.

The current private continuity strategy willCF8BMode2Consider the next copy only as attachedHP/resourcesLibrary contains the current level/Set/Hard`dungeon+1/stage0`Time.CF8CAnd the identity is being updatedCFEB/CF7FBefore. Host Build Certificatedg1→dg2; clientOriginal mapping and skills acceptance remain open onceclientRunback`{real=0,show=0,mode=2}`To Copy1, And the second fight didn't take placeCF9B; Resetting only the adaptor state cannot be explained or verifiedclientLocal gate control conditions.

I don't think so7The entrance's super Boss mode1 Reset has been locatedresourcesName and search anomaly; different from mode2 . See the failure and acceptance range06, Conditional CF8C See copy setting functions08.No changes to the section of the room/Copy Section bypasses the problem.

## List of members, identity and appearance

- The owner can send it before the members enter CF70, And initially only self-receiving CF71.
- When members become active in the room, the synchronised owner receives an exact drop-off remoteCF71Send.
- Members CF70 No duplicates.
- A new oneCF09/The room has been rearmed with the bookkeeping.
- Synthetic Network UID1 CF72 It's still suppressed.

Many people running samples for useUID21/22; These numbers are sample identities, not generic accountsIDRule.

CF71Field policy corresponds to open realization `release/components/protocol/protocol_state_sync_base.inc`: 

```text
+0x34 DWORD = 0 In the group where the visible observation is maintained
+0x38 DWORD = Selected pets
+0x3C DWORD = Sex of data & 1 (Basic model D8) 
+0x4A/+0x4C/+0x4E/+0x50 = Validity of character information HP/MP
+0x52 WORD = 0 (Room phase)
+0x56 BYTE = Room slot
+0x57 BYTE = Sex of data & 1
+0x58 DWORD = Selected pets
+0xB4 WORD = 1
```

`sub_4EF380` Only initialize lowWORDYes0/1Basic model; `+0x3C=0x2000`Not as Model Value.CF72FoundationD8Yes+0x34Use samegender0/1, Not that0x4000.CF71It's+0x57/+0x58The gender of the data and the selection of the pets are taken separately, and the effect cannot be uniformly zero, PET, HPThere's a package layout. No cross-ports.

Each member list entry will be the sameUIDSendCF71And thenCF72, **Including remote character objects**.CF72 case53106I'll find with itWORD+8Current character; WORD+0x64=1You'll apply the full character information, andBYTE+0x66It's refreshingPetLevel/Starter. SynthUID1The inhibition is not the same as the inhibition of a remote character objectCF80_Other OrganiserCF72 (03/09) .

## Entry, progress and visibility

CF71 Yes X=-300 Create an offscreen character everywhere.CFDA Cases 53210 WilldataWord+8 Match RoledataWord+4, Activate it and activate a model-related standby room guard; 0x0578 Updates only loading progress. Default mode is as `(-300,151+100*slot) -> (77,151+100*slot)`; Use peer for alternative mode119 Lines `110+100*slot`.Exact Local/Remote command path in 08 . Do not extrapolate generic room locations from a mode.

Every oneCFD9/8Request for one for each active room in order in the roomCFDA/12.No advance requestCFDA; The battle barrier remains unchanged. The response of the requesting party alone is not sufficient to achieve long-range activationclientOperation observation confirmed the entry of two character subjects, but could not prove that the current two machines were automatic/Visibility of positioning effects, late accession or repeat status cycles.CFDAIt's the entrance frame, **It's not a city stage correction** (See phase entry analysis for this theme) .

## Load and forward

If members preload waiting ownersCF7F, One two-CF80It's still sequencable.

Current Build: 

```text
Owner CFEB -> Owner CFEC -> Members Preload CF80 -> Members CFEC
Members CF7F -> Members Eventually CF80
Owner CF7F  -> Owner Eventually CF80, This is the only response
All of it CF7F All in -> Enable real time forwarding
```

TwoCF80dataThe package cannot be certified and loadedCFEBConduct a global time check and require visible progress line behavior.

The numbering categories currently transmitted by the adaptor include moving/Activities, 0x03E8/0x044C The game, the effect of success CF9C/CF9D Character Effects, Normal Terminal State, BOSS Life Value/Scores, and removal of pick-up items. Shooting room chats are not included in this forward list.

Every forwarding keeps its source identity, room/Pages, connecting life cycle, requesting priority and order rules.

## Reciprocal groups and chats

CFD4We have to decode remoteUID, IPandUDPPort, avoid port contaminationUIDbytes. Remotely in the running recordUID/IP/UDP3534Correct, but this does not provide a separate proof that remote character models are visible.

VillagesCB23A two-way communication observation is available by the adapter router/Game chat is an independent peer channel, room alert port3534.Connector provides admin privilegesTCP/UDP3534Firewall option; whether the firewall leads to an anomaly of conversation without two machine capture certificates.

## Segregation by character data and results

```text
Name, class, pet, equipment/Effects
Skills/Card Status
Picker UID
CF71/CF7E Room slot
CF88 Records
D012 Result/Rating
C47F Target character
CF9C Target character
```

D012Result/Rating vector and doubleclientI'll see you on the run07; There's no substitute for a samplereleaseEnd-to-end acceptance.

## Multiple-manufacturing Borders

The available evidence does not yet fully cover the owner before joining, pre- and post-readiness character visibility, and loads, and automatic two-way/Return to nest ammunition, long rangePET/Effect attached toCF9CConversion, and multi-state cycle departures/Break/Village recovery.clientThe attribution of the phenomenon depends on the sourceUID, Frame order, request low and sequence; firewall causality unproven.

## Stage entrance position calibration and scope of evidence

Three timesclientRuns calibration only for **hd0/ep0/dg1/st0**.CF80After, `0x044C`The relationship between the report and the integer position of the character is: **report X = actor X + 53, report Y = actor Y + 5**.In the record`payload+0x0A`YesX, `payload+0x0C`YesY; `payload+0x14`It's Joe59.4–60Times/The second character clock, not the coordinates.

1. **X, YIt has to be the sameclientLocal mode pairing.** CFDAwithCF71The pattern-related constants are shown below08.XScope130–172From Room End77/119Add53, But that's not the basis for a separate determination of location: 
   - Mode bytes=0: `(X,Y)=(130,156)`, That's the room`(77,151)`Add`(53,5)`.
   - Mode byte non-zero: `(X,Y)=(172,115)`, That's the room`(119,110)`Add`(53,5)`.
   - in the sample`(172,115)`Matching match(s); uniform requirementsY≈156You'll miscalculate the non-zero mode.`X<0`or`X>=300`It's not in accordance with phase entry, 300YesCF84Endpoint.
2. **YThe path used to write a position-specific position is not a simple vertical tolerance.** Y≈305CorrespondCF84Path, not in line with entry; Y≈257CorrespondCF71Constant252is still written. It is not a valid entry or room boundary`X=-247/Y=257`Over and above6Seconds, indicate the adaptor triggersclientThe placement values can remain constant and cannot be attributed solely to the suspension.
3. **Freezing location is not determined through the entrance.** It's a single sample39Reports`X=-247`.`0x044C`It's only coming from the level, so we can't use the room to the level“ΔX”Comparative calibration.

The three same-stage samples above do not constituterelease. Simplify the use of the load relative tag, and whether the calibration contains the eight byte bytes header has not been independently confirmed; measured(+53,+5)And the relationship between mode and mode cannot be pushed to another stage.
