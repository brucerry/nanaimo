# 08 Static addresses and call chains

> Translation draft: numeric evidence and code references are retained, but the prose still needs technical review. Use the [workspace guide](../../../../README.md) for current setup. Original research documents are preserved in the workspace archive `.work/original-source.zip`.

## Address & Call Relationship

The theme is read to the field by function, distributor and status record address. The address is not semantic; the same operation code is connected, controller orclientMay be consumed by different processors in the state.

```text
Construct Functions -> Send Call Point -> Connection
Response Assigner -> case -> Process Functions
Read All Offsets -> Writing/Call
Maximum Reading Offset -> Minimum length
```

Debug string for location code. Field conclusions are based on command and call relationships. Static analysis, clientThe operational observation and adaptor self-defined strategy is described separately and cannot be replaced by each other; protocol rules are set out below01–07, I'll see you in the structure09.

## Login, village and curriculum

| Border | Address/Call Chain | Validation Use |
|---|---|---|
| C368 Process Functions | `0x00531BD0`, case50024 | Local Page Character/Information Build |
| C355 Parser | `0x00531DA3 -> 0x0041646E -> 0x00530C90` | Village data analysis |
| C47F Process Functions | `sub_643310` | Character List UID Find and Attach |
| C47F UID Comparison | `0x00643349..0x00643357` | WORD `+0x08` Character Match |
| C47F Apply | `0x00643384..0x0064338C`, `0x00643478..0x00643484` | Look/PET/Effects Application |

- 271A Assigner: `sub_4E34D0`, Here's the thing `0x004E3588/0x004E37A8`.
- 271A Process Functions: `sub_4E3830`; Package `+0x0E` Read from `0x004E38AE`, Character Info bytes `+3` Writing `0x004E38B1`; Minimal complete character information length is 68.
- C352 Assigner: `sub_4DF710`, `0x004DF766..0x004DF83D`.
- C352 Process Functions: `sub_4DFAA0`; Status `+0x08==100`, Glossary `+0x0A` to `sub_40B744`, Local stage becomes5.
- I don't think so5Stage door control conditions: `sub_4DF1E0`, `0x004DF52B..0x004DF550`; Zero Select Status12, Non-zero selection status1.

### C355 The couple's profile and the original expression next page

C355Consuming chain by `0x00531DA3 -> 0x0041646E -> 0x00530C90`; Couple's name from the bag`+0xDF`To the original`0xA76170`, By non-empty writing`manager+0x4F6C`.The ring`WORD+0xF0 -> 0x4DDA30 -> manager+0x1028`Independently from the Bull logo.

NetworkCross-border fill in failed samples to make name first byteFF, `manager+0x4F6C=1`, And the ring suffix is still0; The next page is missing CI Records `43000000` Find, and `0x5FB67A` Extract NULL Source Address `0x260`.From the fault sample Windows Anomalous `0xC0000005`, Module Offset `0x001FB67A`, & Base `0x400000` Correspond. Current response maintains an empty name (03) , Native `0xA76170` Will `manager+0x4F6C` As false, **Close door control before next page and missing records are searched**; The ring remains0; Empty name to maintain primary door control through adapter response.

Segregation original-x86 Compare execution with original name/The ring setter & Next Page gate, CRT bounded-copy, strlen, memset It's a model. Missing records lookup Press NULL Modelling. Cross-border name construction oncelookup/predicateCalled and not validly read at above address; empty nameNetworkAnd twoStand_AloneThe construction is zerolookup/predicate, `closed-before-page2`.It's a hypothetical quarantine, not all of itclient UI accepted and accepted; construction output is not real time wire.

### Channel Switching Call Chain

_Other OrganiserclientMap resolution; addresses of different maps are not interchangeable.`CChangeChannelWindow`It'sRTTIAnd complete consumption chains are the basis for attribution of this field.

| Border | Exact Address | Proven acts |
|---|---|---|
| Window Loop/Update | `0x00A6A150 -> 0x00A69620` | Apply state23; By Login type10 Connect adapter IP:11005; Visible Connect Fail Branch |
| Channel List Request | Construct Functions `0x004E0810`; Sender `0x00A6A000` | `271B/12` Yes connection10 Let's go |
| Channel List Assign/Process Functions | `0x00A699C0 -> 0x00A69A70` | `271C`; Lines from0x0AStart calculation, line position from0x0CAction (`population WORD+0`, `index BYTE+2`, `IPv4 DWORD+4`) , Capacity from0xACbeginning;minimum initialization into single-line frames176 |
| Click/Destruction/Connection | `0x004DE530` | Destroy the connectionGS0, Close Login10, Use savedGSPort Connection SelectedIPv4As Connection0 |
| GS Reacceptance of request | Construct Functions `0x004DEA50` | `C351/24`, Send as soon as connected |
| Re-admissibility complete | `0x00A699C0 -> 0x00A69C70` | `C352 BYTE+0x08==100`; Install `WORD+0x0A`, exit selection;failure to return selection interface |

`0x271D` In Protocol DollardataOnly once in the register, position is `0x0091A904`; No tectonic function from the closed channel window chain/Send side. The static result is only authorized in the old-GSRe-entry of adaptor after destruction to the same logical slot, excluding character creation during destruction andclientReceiving and Inspection.

Adapter does not add extradataPackage orclient. Initialize each login packageS2CSequence is0; Local connection before assignment`login_header_seeded`Door control lowers the first complete frame5Bit Call`reset_header_tuple(low5)`, Generate checksum torrents`low5|(low5<<8)`; `select_reply_control0(request_control0)`Still running on request. The adaptor structure is low09/16/0Eand serial rings;unmodifiedclientOperation and acceptance not completed.

## Player Experience Carrier andHUDStatic Call Chain Closed

EXPField close-up basis307Bar Parser/HUDCommand.0x6EBAC0Full Assigner has no available inverted translation and is relevantcaseByIDAThe anti-compilation is closed and cannot be described as a complete back-compilation. The protocol policy and value source are available03/07.

- C354 Construct Functions0x54DCD0 (thunk0x41F9A6) , Status1Sender0x52ACB0/0x52ACCD ->0xADA3E0 (socket0) ; C355 Character Assigner0x531DA3 ->0x41646E ->0x530C90, All length728 (FinallyWORD+0x2D6) .From+0x28/+0x2C/+0x30Read Current/Lower/Next state, and read level+0x23.C355+0x80/+0x84Keep a separate part of the copy premise, which does not containEXP.
- CF70/12Construct Functions0x708400Passthunk0x404165Achieved; controller0x6E94C0Yes0x6E9506, which willcontroller+0x7CCopy torequest+8, Send operation in0x6E951BPlace, point0x40A821/0x6E90C0 ->0xADA3E0 (Console Index+0x78) .The controller has its own0x40A96Bdoor control conditions;not introduced without requestCF71.
- CF71 Yes0x6EBAC0Medium case53105.Local UID Branch0x6F4FD0..0x6F4FF5Use and C355/CF88 Same setting function, will DWORD+0x40Writing next, DWORD+0x44Writing current, Do Not Write lower.level+0x48Right next to you grade+0x49Apply before. Fullness of reservations184Byte Response Overwrite Existing Roles/Skills/Easter tail; no basis for conversion to a shorter, experienced, exclusive package.
- CF87/12 Construct Functions0x7087F0 to0x40FDC1, Sender0x6EAA00 ->0x40A821; CF88 Record Start Location+0x0C, Step length0x34, Min12+52*count.Local Results Record+0x14 Lower ->0x6FA399/0x41A7B7; +0x10 Current ->0x6FA3B8/0x41587A; +0x18 Next ->0x6FA3D7/0x415ACD.Level up door control condition+4, Current Level+0x0A Yes07; NoneHP/MP Recalculations.
- Shared Settings: 0x41A7B7 -> 0x4D7BC0 Writing Character+0xFC8 Down; 0x41587A -> 0x4D7BE0 Writing+0xFCC Current and temporary benefits removed+0xFD0; 0x415ACD -> 0x4D7C10 Writing+0xFD4 Next. Level 0x40FEE8 -> 0xA72570 Set level fields only, no thresholds.
- HUD0x8E1620 From Address0x41BDC9/0x833FE0, 0x4061CC/0x834000, 0x40AD8A/0x834020 Read lower, current, next Information and deposit it in separate files HUD+0x448, HUD+0x44C, HUD+0x450.Mode0x40A3E4/0xA74410 Select whether the current value contains temporary gains (by address)0x409133/0xA76240, It's a separate reward model0, 1, 2) .It's decorating without a symbol/Limit the current value to the next value before dividing it; the percentage formatter is0x8E1D24 ->0xB47637.Close100%The result is not a valid progress state at the time of the spill.

## LumineosLocal prefix door control conditions

- C354/8: Construct Functions `0x41F9A6 ->0x54DCD0`; Status1Sender `0x52ACB0`, socket0Send `0x52ACCD ->0xADA3E0`.C355/728: Assigner `0x52C100`, Copy728And `0x52C2BB ->0x4197A4 ->0x544000`; The single character-handling function above is by WORD+0x2D6 Consumption, minimum728.
- C355 DWORD+0x80/+0x84: Read Point As `0x5441CC..0x5441DC`, Call `0x5441E8 ->0x404692 ->0x54EDD0`, Yes `0x54EDDD/0x54EDE3` Write first progress object+0x48/+0x4C.The object pointer is manager+0x26A0, **No, it's not** manager+0x26A4. Normal bytes+0x3C..0x77I'm sorry0x54406F/0x54409A/0x54ED70Writing manager+0x2EB5And the first object+4; +0x78..0x7BI'm sorry0x54EDA0Writing manager+0x2EF1And the first object+0x40.Independent+0x88..0xC3/+0xC4..0xC7/+0xC8..0xDELeveldataI'm sorry0x4D0F80/0x4D0FB0Writing second object+0/+0x3C/+0x40.Level display is not equal to pre-level completion.
- `0x5000A0` Move Call Normal `0x416CA7 ->0x4FF190` Yes `0x500650` location; general gate control only allowed 0..15.hd Fetcher `0x411379 ->0xA75250` Read Manager `+0xD34`; Level Retrieval `0x416E23 ->0xA731E0` Return Pointer `+0x5DC`.With PropertiesclientIt's a quick shot hd Yes 4, Level as 100, So it's all alone hd 4 The branch is decisive: hd Read `0x500666`, Comparison `4` Yes `0x50066B` Check `0x500688` to `0x500ABF`.
- `0x4183D6 ->0x509EC0(k)` Test first object QWORD+0x48 It's **Two** bits bits2*k and2*k+1; Call0x509EDD/0x509F06 ->0x40296E ->0x50D440 ->0xB4D2F0 Hold64Consistency of position, crossing bit31/32.22 individual `k:destination-page` Correspond `0:7,1:6,2:11,3:16,4:17,5:18,6:19,7:14,8:9,9:4,10:3,11:2,12:1,13:0,14:5,15:10,16:15,17:20,18:21,19:22,20:23,21:24`.I don't think so8/12/13 It's not in this special door condition. It's not proven to be exempt from all local rules.
- False -> `0x40ACAE ->0x506050`, Type of dialogue9/Message59 Pass0x408F5D -> 0x515CE0 Cases0x3B; Localizeresources9061 Load in0x51A508.Operating observation provides matching localized text; resourcesThe text itself is not independently decoded. The failure returns before the page request: C367/16 Construct Functions0x41D66F -> 0x4D79B0, Successfully sent0x500BD0 -> 0xADA3E0 (Connection0) ; DWORD+8 Page, WORD+0xC/+0xE Coordinates. No reasonable unrequested response bypasses.
- `0x41C328 ->0x9000D0` minimap Visualization Settings slot0, And then from predecessor0 Present predecessor21 Settings slot1 Present slot22, Position in 0x900110.**0x90012F Force Alone slot6=0**; slot23 and slot24 Set As1.The meaning is now closed: Construct function 0x8F5A50 From String 0xCAD0B0 (`images/interface/000290-qz_minimap_v05_d07.im3`) Build slot6 drawable items, offset +0xBE0; Next door 0xCAD080/0xCAD0E0 Separately named d06 and d08.Renderer 0x8FBAE0 Yes 0x8FBD9B Site Test Marks +0xB64 + 4*N, And 0x8FBD9B Place Testable Drawable Items +0xBC8 + 4*N, So even with a complete progress mask,, d07 Skipped. Refresh Call 0x53106D/0x8FABA7 re-enacting it;constructing functions 0x8F5E73 And reset 0x900187 Clears the sign. This sign array and move first progress object +0x48 Different. The range is in 0x8F0000 Present 0x901000 The scans found these tectonic functions/Reset/Initializer/Rendering the consumer, but not unwritten proof of the whole process. The mandatory storage isd07keeping hidden; removing quarantine experiments showing storage does not prove physical roads exist orclientRun through.
- ZeroclientSnapshot: Manager0x00191F64, First Object0x032B9B98Offset+0x48=0, Second Object0x032B9C08With Display0x55; ReceivedC355Yes+0x80..0x87Also zero. Current adapter policy05Possession. Hostage check/3190Isolatedx86It's not a caseclientReceiving and Inspection.

### The Village High and Highd07Show borders

- VILName `Village_map_Image\%02d%04d.vil`: formatter0x50EAE0, string0xC394DC; Villages+1/Page -> Numeric fifth-village record50000+Page distribution0x50EA00Call Map Fake+8 `0x40BD16 ->0x9DBA60`.Pack Reader Positioning Entry Offset+4, Read16Head bytes, four dimensionsDWORDand264fixed bytes, then index+260byte texture, with-1terminated; grid reading is everyxColumns36*Height. Current wrapping head is9BytesNANA_PACK, Count230Yes+9, (id,offset) DWORDYeah+13; This is the fifth village bricks16x16, Grid as50x36.
- `0x9DCE30`/read0x9DCE88 Return with symbolWORDUnits+12+2*Type, stepband36.Type1/+14 It's a collision: `0x5039A0 ->0x4157CB`, ♪ By the In0x5033E0/0x503BE0. Type4/+20 Yes0x5000A0in0x500292Type of destination field that was previously consumed7/+26 It's an action trigger; 0x5000A0Situation in progress160..182 Call0x404A52, Import0..22, Then enter the room. Village sign is not enough to build a shootinghd/Set/resourcesMetagroup.
- The planned copy page order is `8,7,6,11,16,17,18,19,14,9,4,3,2,1,0,5,10,15,20,21,22,23,24`.Only the first six pages have a corresponding trigger (160..165) .In the original package retained, page17The only destination is16; does not exist17Present18The side. The rest17The planned phase page has the same grid layout, with no exit or action trigger. This closes the delivery reportresourcesThe physical breakpoint of the layer; it's not justd07Draws signs. Near0x4FFA30/0x4FF890/0x506280I didn't offer the missing exits.
- d07Show the quarantine experiment offsets the file0x4FF52F, VA 0x90012FIt's purified10Byte Writing Command`C7807C0B000000000000`Replace With10individualNOP, Follow-up address0x900139no change; slots6So keep the prefix right5, instead of mandatory at zero progress1.546An example of an initial bio-inclusion check box6The change, reset, stack and non-receivable register is maintained. This only shows the local effect of the storage and does notreleaseUse this change, realclientRun-through or village roads rebuilt.
- Overleaf location and export independence: `0x500AE4..0x500B15` Willtype5and the source page to the target map `0x40C586 ->0x50A090` (Target Find As `0x410492 ->0x50D770`, `0x40D6CF ->0x50DDE0`) .Mark as**cell+22It's got a symbolWORD**.Scanner `0x50A090` Press firstYPress againXAll over the border `0x416FFE ->0x50A180`, First Match Read Point As0x50A0FE; Return when no match is available(-1,-1).`0x416202 ->0x50A1E0` Area size and object origin+0x18/+0x1CConvert grid point coordinates (constructive function)0x9DADA0Initialize original point as0) .17↔18Move the wheelX=9999Replace with TagX, Keep InY; Corresponding Vertical Paths0x500B26..0x500B57ReplaceY.The missing mark cannot be considered a birthplace0.
- Existing17↔18Roads and the seventh entrance166is a custom map content: Page18Source17Tagsx2,y14..20Generate(32,224), Page17Source18Tagsx47,y14..20Generate(752,224).Access166It's at the passable gridx22..27,y3..4, This does not represent the recovery of original portal images228Notes, Graphics/Object List and Entry165/Back16No change; full range of existing roads is shown below.900Isolationx86Field assertion overtook two112PixelsYBring the corner, 108SynthC367/C368Requesting a tectonic check, but unable to prove full character collision or visible game behavior.page18/index6/ep22It'sclientLoad & Prepre-create/ListUISee you on the black screen05; index6resourcesMap and battle tectonics06.Village roads don't add to other battle triggers and don't change monsters/SSTG/SMMO/BossMap.
- **Copy returns a different type of markup than cross-page arrival.**`sub_538EE0` Test Old Game Locations2Yes0x539588, And it's recordedOLD_SHOOTING_DUNGEONON_GAMEYes0x539591.For Copy6/I don't think so100Set, 0x53C7E8/0x53C7FBSelect Branch; 0x53C80CType of push8, 0x53C80ESend Tags169, 0x53C823Call0x40C586 -> 0x50A090.Type8Read with SymbolsWORD **cell+28**, . . . ., instead of the source pagecell+22Or the access triggercell+26.And the result is0x53C828..0x53C830Copying to village character creation. The first six entries sent are marked163..168; Their access trigger is160..165.Missing returned marked pages18Sample supply portal166, But he didn't return169, And soclientIt's an independent choice(-1,-1), ♪ Though ♪C368It's coming back(400,96).
- The return mark for the seventh entrance is located atpage18Cells(25,6)+28, Value as169, Not-1.Original Results(400,96)At the non-trigger entrance/Accessible units for export and two roads and entrances are connected0x50A090/0x9DCE30, the mark is missing(-1,-1), Tags169Got it(400,96), Overleaf Generating Belts and Remaining MapsdataNo change. Return location byclientThe character creation path to read map tags is determined, not by the adaptor plier or by unrequested repositioning of the character;/Leave/Actual return after clearance is still lackingclientReceiving and Inspection.
- Existing road includes16Group two-way connection: 18↔19↔14↔9↔4↔3↔2↔1↔0↔5↔10↔15↔20↔21↔22↔23↔24.Together with the first seven regions,23Planning Village Pages, 22Group two-way link, no23Playable copy. Road differences count as17It's on the record1700Bytes/1070individualWORDentry, only+14/+20/+22; The rest213Notes, Graphics/Object sheet, action field and type8Back169No change. Horizontal tagsx2/47WillXAs32/752, ReservationsY224..335; Vertical Tagsy2/33WillYAs32/528, ReservationsX352..447.The border is closed without connection and source tags are not exported/Overlap.3360It's an isolated place, 20160An assertion and320SynthC367/C368Requesting a structural check, full character/controller/Rendering history unverified.
- Ready to create/List Border: entry166Existing settings are called0x402EF0 -> 0x42FBA0, Status as9; 0x42FBA0Before storage/Current status. Capture room/Creates a string that includes unquoted torrents without a black background loader or active drawingsUISee you at the observation range05; No battlefield images or minimap The mark's inference to seal it off.

## Room character, loading, map and continuation door control

| Border | Address/Call Chain | Validation Use |
|---|---|---|
| CF71 Status4 | `0x006F4E01` | Prepare the character information for the room/Character Object Fields |
| CF72 Cases | `0x006F594E..0x006F5E07` | Dynamic RolesUIDFind, fully apply character information, PetLevel/Starter Refresh |
| Remote Location CF71 | `0x006F561E -> sub_40F8B2/sub_6D85E0`; `0x006F562E..0x006F5647 -> sub_41C256/sub_6D8650` | X=-300 Hide Generating Point; Y=151+100*Room slot |
| CFDARoom entrance | case53210 `0x006FBFFC`; UIDFind `0x006FC032..0x006FC0B7`; Carrier `0x006FC124..0x006FC145` | Activate character and enter `(-300,151+100*slot) -> (77,151+100*slot)` |
| 0x0578 Progress | Assigned `0x006FCEEC/0x006FCEF8 -> 0x006FD76B` | Slot loading progress record; no escort |
| CF8C SuperBOSS Reset | Situation `0x006FBD28..0x006FBF61` | Show actual bytes `+0x28/+0x29`, Difficulty WORD `+0x2C`; Whenresourcescycle less than16time, copy WORD `+0x2E` read-only;the minimum value of this branch is48, Remagnation is not allowed Lumineos |
| CF99 Abandoner Request | Construct Functions `sub_7089F0`; Sender `sub_6EAE60`; Call Point `0x0066C190/0x0066C6C9` | Command `0xCF99`, Length8; The long-term controller applies bytes to settings+7Send Before |
| Apply Object+7Door control | Set Functions `sub_66E5E0`; Read Functions `sub_66E600`; CF8CDecision `0x006FBE40..0x006FBE61` | Non-zero room selection/controllerstate3; continue normal reloading when zero |
| CF6D Clear Path | case53101 `0x006F7949`; status10 Call `0x006F7973 -> sub_7084F0` | Settings application+6, Zero application+8 and application+7 |
| CF9A Confirm | case53146 `0x006FC1DE..0x006FC21E` | Logs and release frames; unmarked clearance or load consumption |
| CF71Local Member Test | `0x6F4EC6 -> sub_419BBE -> 0x6F4ECD sub_40A34E` (=Thunk `sub_677470`, LocalUID) Relations with members`WORD +0x1A`Yes`0x6F4EDC`Comparison of places; `0x6F4EDE jnz loc_6F54E5` | Cases53105Split into local and remote branches according to each member entry |
| CF71 Local Hide Generate | `0x6F4FFA push -300 -> sub_417954` (`sub_66E8A0` Lazy single case) `-> sub_4179BD` (`sub_4D8CF0` = `*(singleton+0x14)`) `-> 0x6F500D sub_40F8B2 -> sub_6D85E0 -> sub_40E5A7 -> sub_6E73A0(actor,-300)` | **Unconditional** X=-300 For local members; Y = 110+100*`BYTE+0x56` (`0x6F5044`) or 252+100*`BYTE+0x56` (`0x6F506D`); There is no Character Existence test in this branch |
| Remotely Create CF71 | `0x6F55B3 cmp var_DAC,0` Door control conditions; `sub_40D9E5` (`sub_66EB80`) + `sub_40B7AD`; X=-300 `0x6F561E`; Y=151+100*slot `0x6F5647`; Current Character Path `loc_6F564E` (`sub_40CDDD`) | “The rule only applies to remote members when they are missing; the remote branch will never reposition the existing character |
| CFDA Local applicability | Scenario 53210 `0x6FBFFC`: Include onlydataPackage field `DWORD+0x08`, UID Comparison `0x6FC08A/0x6FC090` with `sub_4021E9(actor)`; Ownerless/Local/Mode Exclude | CFDA Any character applicable to frame naming, including local roles |
| CF84 Site Placement (Closed))  | Cases 53124 `0x6F5E20`: `+0x08` No symbol (only) `0x14` and `0x3C` Active, other values skip the whole body) , `+0x0A` No symbol character object UID, `+0x10`/`+0x14` Double → `[actor+0x7BD8]`/`[actor+0x7BDC]`, Minimum length `0x18`; Subject `sub_415820(actor,3)` (CFDA Use 1) → Carrier `sub_403F0D(-300,300,300)` `0x6F6005..0x6F601F` → X=-200 `0x6F602F` → Y=300 `0x6F603F` → Locally Only UID Blocks `0x6F6051..0x6F606C` | clientMy own **scene** Place path: It uses a scene line Y=300, And the room is used Y=110/151+100*Slot, and carrier ends on 77/119; This is a closed scene routing path, which does not confirm that the general access coordinates are correctX=353, Failure to reach entry point; releaseDo not send the placement frame. Static field with14Read-only running memory anchors are consistent. |
| Local Character Location Report `0x044C` (1100) | Construct Functions `0x6AE840` (Length `0x28`, Operating Code `0x044C`), Indirect Functions `0x41333B`, Unique Call Point `0x6ACC63` Yes `sub_6ACBF0` Internal | Load `+0x0A` = **Character X**, Load `+0x0C` = **Character Y** (The two are separate `fistp` Convert `[this+0xD0]+0x110C`/`+0x1110` two floating points;types `0x34`/`0x46` Use constant 400/300 Cover them; payload `+0x14` = **Character Clock** (`0x6ACD5E -> 0x417544`, The measurement value is 59.4-60.0 Times/sec — A counter, not coordinates; load `+0x06` = Sender Parameters1 (Construct function default `0xFFFF`) ; Load `+0x08` Read in Every Frame 21 (Local UID) , Even though neither the construction function nor the sender wrote it — Consider it unwritten until it is confirmed by live reading; frame passes `[0xD8CE78]` (`0xADA650`/`0xADA630`) Sending approximation per second 2 Times |
| D00F kind20 builder | block `0x6EA060` (kind `0x14` at `0x6EA07B`, `retn 18h`), thunk `0x405308`, constructor `sub_7086A0` (len `0x1C`, op `0xD00F`) | **Frame relative calibration**: `+0x08` kind, `+0x0A` WORD = Constructor's first true field to be included in, `+0x14..+0x18` Five bytes (broadcast bytes) 0) ; **Unwritten Only `+0x0C..+0x13` Eight bytes** (Two pins with one word on each frame `0x0018747C`/`0x00187408`) .**payload Relative caliber** (payload = Frame +8) : `+0x00` kind, `+0x02` The WORD, `+0x0C..+0x10` Five Bytes Writing, `+0x04..+0x0B` Eight bytes not initialized——The two sentences are the same |
| D00F Family / thunks | `0x6EA320`/`0x4189CB` (kind10) , `0x6EA060`/`0x405308` (20) , `0x6EA230`/`0x40DC56` (30) , `0x6EA130`/`0x41E164` (40) , `0x6EA2A0`/`0x41D06B` (50) , `0x6EA390`/`0x40E45D` (60) , `0x6EA1B0`/`0x419FEC` (70) , `0x6EA0D0`/`0x41955B` (80)  | A tectonic function of each type, each of which is in its own `jmp` thunk Back |
| D00F Source of Total Collision Types | `sub_670E10`: `0x671E7A` Read `[collision_object+0x1A0]`, `0x671E80` Push it to `0x671E88 ->0x40DD7D -> sub_6E9FB0`; Construct Functions `sub_7086A0` | Author Object Class to Frame `WORD+0x08`; Full carrier type observed60/80, And the Zero-Species70Event is a separate compatible shape. No numerically impaired field |
| D014 Collision with victim carrier | `0x6EA3F0`/`0x41E105` (Type10) , `0x6EA440`/`0x41BB8F` (20) , `0x6EA490`/`0x406B31` (30) ; Construct Functions `sub_7086D0`; Call Point `0x672AF5`/`0x672C07`/`0x672E05` Yes `sub_670E10` Internal, category comparison `[obj+0x1A0]==3` Yes `0x672B58`, `==4` Yes `0x672C6A` | Carrier exists and is controlled by category, but is in operationclientZero in common monster contact D014 (53268)  |

- CF71 Cases53105: `0x006F4E01..0x006F5930`; Remotely Create `0x006F55C0..0x006F5647`.
- Room Character Construct Function Chain: `sub_66EB80 -> sub_6D3B10 -> sub_4EF380`; LookD8Low byte branchon0/1.

- CF71 Remote creation intentionally outside the screen: X=-300, Y=`151+100*slot`.
- Run-time UID22 It's X=-300/Y251, Room slot `actor+0x7A4C=1`, Non-zero model pointer in `actor+0x7A0C`, And there are visible door conditions `actor+0x7D88=1`.
- CFDA Cases53210 It's character-activation/Carried boundary: dataPackage DWORD `+0x08` Select Character DWORD `+0x04`; `sub_415820(1)` Activate it; `sub_403F0D` Start Default X=-300 Present X=77 Carrier.
- 0x0578 Case read the package progress `+0x0C` And the character room slot `+0x7A4C`, And then it's through `sub_6A3840` Writes a schedule for each slot. Early 0x0578 The escort belongs to a perjury.
- **CFDAFields: ** case53210Consumption only`DWORD +0x08` (UIDComparison`0x006FC08A/0x006FC090`, List Through`actor+0x8130`) ; Minimum length12.Both modes of escort are instant constants——XStart Value`0x006FC112/0x006FC13A push 0xFFFFFED4` (-300) , XTarget value`0x006FC0FE push 0x77` (119) or`0x006FC124 push 0x4D` (77) , YValue from`sub_41CCD8 -> sub_66E740` (Character`+0x7A4C`) Fetching, `=110+100*slot`or`=151+100*slot`; Call`sub_403F0D -> sub_6E0DF0`.Mode Selector isclientLocal state, notdataBytes: `0x006FC0EB sub_419BBE -> sub_6658E0` (Delayed Initialization0x1FB30byte single instance) then`0x006FC0F2 sub_40A2D1 -> sub_664F00`Back`singleton+3`Bytes, Test`0x006FC0FA/0x006FC0FC`.Re-entry Path`0x006FC14C..0x006FC1B5`Repeats the same constantCF71constant`X=-300`, **CF71andCFDAI don't want to carry a free startXFields**.

- CF77 Construct Functions `sub_7085A0`: opcode `0xCF77`, Length16.
- CF77 op100 Construct Functions `sub_6E9C20` (Placeholder `sub_408067`) : From Retriever `sub_403D2D/sub_41DF7A/sub_41D1B5/sub_411ACC` Access `+0x0A..+0x0D`.
- 0x044C Shared Source Character Selection: `0x006FCE26..0x006FCEBC` UsedataPackage DWORD `+0x08`; Subtype assignments maintained `0x006FCF30 -> 0x006FD207`.

- CF77 Construct Functions `0x007085A0`; Construct Functions `0x006E9C20`; Caller `sub_736DB0`, Call Point `0x0073704A/0x00737328`.
- CF77Fields `+0x0A..+0x0D` Read objects `+0x1E530/+0x1E548/+0x1E54C/+0x1E528`.
- Write the author directly: `sub_50A280/sub_73AFE0/sub_708DA0/sub_50A260`.

- `sub_7089F0` Build CF99/8; `sub_6EAE60` Send it.
- Yes `0x66C190/0x66C6C9` Where, the controls that are running for a long time exceed their counter `0xE10` Organisation CF99; And `0x66C19E/0x66C6D7` Set application bytes+7.
- CF8C From `0x6FBE40` Read application bytes+7; If value is not zero, then state is reached3 `0x6FBE53..0x6FBE61`, And the value of zero follows the normal reload path `0x6FBE66`.
- CF9A I didn't clear the sign.CF6D Status10 Reaching Closed Clearing Assistive Functions `sub_7084F0` in `0x6F7973`.

CF71Locally Write andCFDAmode is the instant constant; do not convert their fields to freely chosen local coordinatesCF71Rearmed on every match frameX=-300, This is not the case for the remote creation of location only. This static fact is not forced to be retainedCF80Previous LocalCF71; Save the current active sequence in03/09Medium.CFDARe-escort andCF84The location cannot be valid as proof of the location of the copy entry.

D00F kind20 +0x0A Writes from the first stack parameter. Its only call point0x673BB4 inVMPVariable area, which allows the full caller to attribute and match each contact instance to remain openD00DComparing does not constitute a valid rebuttal; collision-only exposure may not occurD00D.The frame relative deviation and load relative deviation in the table must not be interchangeable.

## Backpacks, pets, shops and shortcuts

| Border | Address/Call Chain | Validation Use |
|---|---|---|
| C44F Construct Functions | `0x82CC60`, length24 | `REQ_CHANGE_PETITEM`; action1 Jewelry upgrades/action2 Upgrade processing |
| C450Process Functions | `sub_8005F0` Regular cases`0xD7` | WORD+8Status; BYTE+0x0AOperation; BYTE+0x0BDetails |
| C44C PET Records | Cases`0xD3`, Start Offset+0x0C Step length0x24 | Upgrade+0x0D; Gems+0x14/+0x18/+0x1C |
| C475 Construct Functions | `0x82C840`, Length20 | Separated mold operation;not duplicated gemstone request |
| CF94 Numbering Major Classes21 Solutions | `0x6F1E3C..0x6F1E78` | `sub_406B13 -> sub_40CD97 -> sub_9C2AE0` |
| CF94 Numbering Major Classes21 Effects | `0x6F21FB..0x6F2577` | Definitions WORD+0x1FC to `sub_41127F -> sub_9C1090` |
| Loaded Curse Definition | Object`0x18FEA408`, Code+0x10=21000012, Type+0x1FC=2 | Card13000009: Defense-50%Curse |
| Loaded Shield Definition | Object`0x18FEB3A8`, Code+0x10=21000001, Type+0x1FC=9 | Card13000017: 7The second shield |
| Loaded accelerator definition | Object`0x18FEB790`, Code+0x10=21000002, Type+0x1FC=6 | Card13000019: 25Second accelerator |
| CF71 Door Control Condition Carrier | `0x6F51D8..0x6F51F1` | Read `WORD +0x7A`, Call `sub_40CDFB` |
| CF72 Door Control Carrier | `0x6F5AC6..0x6F5AD7` | Use to match local character reading `WORD +0x56`, Call `sub_40CDFB` |
| Character Object Door Control Conditions Seter | `sub_40CDFB -> sub_6DCCB0` | Writing Combat Roles `+0x80EC` |
| HUDDraw/Destruction | `sub_6DCD10` | Slot0-2Unconditional; door control conditions0Destroy Slot3-5, Door control conditions1Draw them |
| HUDRender | `sub_6DE170` | Only if it's a character `+0x80EC != 0` Rendering slots when they exist3-5 |
| Slot Input/Use | `sub_6DE350` | Just a part `+0x80EC != 0` time, slot index3-5It was accepted |
| Six code construction function | `sub_41F28A -> sub_6DCF50` | Construct six objects independently of door control conditions |
| C431 Construct Functions | `sub_7C6B10` Pass `sub_412DF0` | opcodeC431, Length16; Active field type/General Category/Number/Code |
| C432 Assigned | `0x5F71ED -> sub_40E584 -> sub_5F83C0` | Based on BYTE+8 Branch; 10 Successes;reading NaNa+0x58 and Hans+0x60 |
| C470 Assigned | `0x5F71CB -> sub_41C215 -> sub_5F7B10` | Same result selector and balanced layout |
| Special Gems Action Constructive Function | `sub_805FC0` | 18000001->action3,18000002->action4; WritingPETThe handle+0x0A, Slot+0x0B, Identity of items+0x0C |
| C450 Action Application | `sub_8005F0` Cases0xD7 | Status WORD+8=2000; Actions BYTE+0x0A; Details BYTE+0x0B |
| Current Action3/4Local application | `sub_4073F1 -> sub_807200` | Delete the selected zero base slot and compress the remaining slots; actions3Keep returned gem objects, actions4Destroy it |
| PALoader | `0x960B90` | 24Line of a field defined432bytes;types+395to397, Percentage+399to401, Value+404/+408/+412 |
| Type8 resourcesSemantics | `PA._D9` Okay17000014/17000444 | Life Value Restored0.3%/1.2%, Type8Percentage3/12 |
| SelectedPETDigital Bridge | AdapterPETProperties Sum | Properties & Overwrite Type1/2/3; Type8See the numerical policy04 |
| Source of injury | D00D Actual `old_hp-new_hp`; D011 `boss_hp_sync_result.applied_damage` | Just by asking/Nominal attacks do not restore life value |
| HPApply | D010Assigner`0x6EBAC0`, Character Missing`0x6EE985`, HPWriting`0x6EEA28 ->0x406E74 ->0x6E3000`, Score`0x6EF06B` | Full36Byte Response: Current Character+8, Score+0x0C, AbsolutelyHP+0x10, Hurt+0x12, Initialize+0x1D/+0x22; Fixed scene type60/80Reuse this to close the process function |
| CF71 X Permissions | `0x6F53D3..0x6F53E7 -> sub_412913 -> sub_57DCA0` | Bytes `+0xA9` Writing Manager DWORD `+0x55F4` |
| CF71 Z/X Okay | `0x6F5403..0x6F548D` | DWORD `+0xAC/+0xB0` Code; BYTE `+0xAA/+0xAB` Level |
| CF72 X Permissions | `0x6F5BBA..0x6F5BCB` | Match bytes `+0x72` Write to the same manager door conditions |
| X Enter/Render door control conditions | `sub_416FBD -> sub_57DCC0`; `sub_878340` | Read Manager `+0x55F4`; The equipment cycle is indexed1Zero break |
| C3E8Expired door control | `0x574C13..0x574C6F`; Mirror `0x8645F2..0x864669` | CurrentDWORD `+0x4C`, ExpiryDWORD `+0x84`; Effective only when current non-zero expires |
| Special Gem Structure Functions | `sub_805FC0`; C44F Construct Functions `sub_82CC60` | action3/4; PET Process Functions `+0x0A`, Zero base slot `+0x0B`, Special Process Functions `+0x0C` |
| Special Gem Local Application | `sub_4073F1 -> sub_807200` | Excludes the selected old index and adopts the pair `sub_40EAED(..., new_index++)` Reinsert survivors |

- C475Construct a function to `sub_82C840`, Length20.Operation request confirmedBYTE+8Mode, BYTE+9Operation, WORD+0x0ANumber, DWORD+0x0CLocal Tags andDWORD+0x10codes; these field positions are from running observation.
- C476Correspond `sub_8005F0` When it's consolidatedcase0xFD.WORD+8Zero means no; status3/4PressWORD+0x0AQuantity given from all overDWORD+0x0CStart Code.
- C475The above field locations are from operational observations and the results of the current static analysis are not sufficient to add field conclusions.

D00EThe identifier extension experiment involvesVA `0x006EC328`, File Offset`0x2EB728`It's16Byte area. Experiments maintainedPEThe layout is unchanged; it is not unmodifiedclientThe originalHansThe following address is only applicable to the corresponding code layout and cannot be extrapolated directly to the other map.

- C430 Normalization cases 0xB7 Yes `sub_8005F0`; Line Build on `0x804622 -> sub_40949E -> sub_7C8430`.Head consumption BYTE+8 Status, BYTE+9 Modes and WORD+0x0A counting;lines from+0x0C Let's go, let's go 8, Organisation DWORD Code, WORD Identification number, BYTE Selected Status and BYTE Keep status. The security length is `12+8*N`.Object Code From+0x10 Start with the ID number from+0x14 _Other Organiser+0x24 or +0x28 Start. There are no numbers.
- C44F/24 Construct Functions `sub_82CC60`, Sender `sub_7F4FB0`, Organisation `0x7F5012`; action3/4 Use PET Process+0x0A, Emerald slot+0x0B and special item identification+0x0C.C450 Situation0xD7 Yes `sub_8005F0` Consumed successfully WORD+8=2000 and action/detail+0x0A/+0x0B.`0x805D59/0x805DE3 -> sub_4073F1 -> sub_807200 -> sub_406447 -> sub_7F7EE0` Remove specific identity to verify and compress gemstones.
- C46D/16 Call `sub_7F49F0`, Construct Functions `sub_41DA3E -> sub_82CBD0`: DWORD code+8 and DWORD identity+0x0C.C46E Assigned `0x8006EA/0x8006F4 -> sub_416B85 -> sub_81D440`; Minimum failed value12, Minimal of success20, Use DWORD status1+8, code+0x0C and identity+0x10.Passed after success `sub_406447 -> sub_7F7EE0` Remove identity and implement local family logic.
- CF93/12 Construct Functions `sub_6E76B0` Just carry it WORD Slot+8.CF94 Process Functions `0x6F1D9F` I've consumed the character UID+8, Slot+0x0A, Code+0x0C, HP/MP Change value+0x10/+0x12 And the tail after initializationWORD+0x16; It doesn't have any remaining count carriers.
- CF95/8Construct Functions `sub_708900` And the sender `sub_6EAD40` Do not carry any load.CF96Distributionor status `0x6FBF91` Do not read any load; 8Bytes are safe.
- CF87/12 Construct Functions `sub_7087F0`, Sender `sub_6EAA00`; No confirmed signs of death.CF88 Process Functions Start `0x6F97F7`, Record Offset+0x0C Let's go, let's go 0x34, The security length is `12+52*N`.

- `gi._D6`:413760byte. Loader0x9C1D10Read121Flying18field line, then read2497It's a food19fields rows;food defined size516bytes, code+0x1D8, HP/MPYesWORD+0x1FC/+0x200.
- `sub_F07688`It's2001The line memory map is just a partial snapshot, **Not really**Proofresourcesor protected loader generally required2001.Full head/Can not open message03.
- CF94 Cases53140 Yes0x6F1D9F Consumption WORD+0x10 Life values and WORD+0x12 magic values;numbering categories21 Parsing precise local definitions separately.

### Resuscitation sugar static call chains and borders

`MI._D22` ProvidedclientLocal Credit Sheet: `48000001=5`, `48000002=10`, `48000003=30`, `48000004=1`, `48000005=5`, `48000006=11`, `48000007=32`; resourcesOkay, no C430 . The complete static call chain is C475 Allocation -> C476 Result -> C430 `(code,identity)` Examples -> C46D Use -> C46E Success.`sub_81D440` The specific markings were removed and triggered domain48 It's event35, Pass `sub_4011B3 -> sub_7090B0` (`this[5504]` (Byte Offset `+0x5600`)) Read local counters, addresourcesValue and pass `sub_408A53 -> sub_54E430` (`this[5504]` (Byte Offset `+0x5600`)) Writing.

Death is a different group: D010 The absolute value of life is zero; CF95 Yes8Byte retest request, CF96 Yes8No load response byte; CF87 Yes12Byte result request, CF88 One to consume12At the end of the byte add0x34Byte Records.`sub_73F3A0` and `sub_73FF50` Read Retry/Result UI . The subsequent carrier is closed in `0x5310B2 -> sub_408A53` It's proven C355 `BYTE+0xF2`, And `0x6F53B8 -> sub_41A2CB(value,0) -> sub_6E2F40 -> sub_408A53` Local CF71 `BYTE+0xA8` Writing Manager `+0x5600`.CF84 Process Function Variables `WORD+8==60` Yes `0x6F5F33..0x6F5F53` Read the current value through `sub_4011B3`, Less1and write `sub_41A2CB(value,1) -> sub_408A53`; It also applies the package coordinates to the target CF95 And after a successful durable adapter reduces the amount, it passes through an orderly process `CF96 -> CF84 variant60 -> CF72` Drive this original reduction. This closes the byte/Order and Host Build, not VisibleclientControl restored.

## Card synthesis, key and learning line door control conditions

| Border | Address/Call Chain | Validation Use |
|---|---|---|
| C3E8 Page Processing Functions | `sub_5E9280`; Room UI `sub_7996F0` Situation30 | Seven lines of clear learning are recorded in `+0x60/+0x7C` |
| C3E9 Construct Functions | `sub_57A550` (`0x57A55A` Command Code, `0x57A563` Length8) | Synthetic Page Entry Request |
| C3EA/C3EEAnother controller | `sub_83CA40` | C3EAConsumption of prefixes for the controller; C3EE `DWORD +8==400` Send C3E7/C3E9 and write the results to be validated `+0x528` |
| Correlation of results | `sub_40281A -> sub_83DC40` | If the controller is `+0x528!=0`, Creates the result panel582,363& Empty Fields |
| Second package controller | `sub_85A270` | case50154 Assigned C3EA; case50158 Assigned C3EE |
| Second C3EA Process Functions | `sub_40F15A -> sub_859840` | Guide to `BYTE +8`; Count to `+9/+0A/+0B`; Expiry/Current Status To `DWORD +0x0C/+0x10`; Minimum length20 |
| Second C3EE Processor | `sub_41E6AA -> sub_8599D0` | answer400 Read `DWORD +0x0C`, Store the result code in the controller `+992`, Set completion status `+964=1`, Clear Busy Status `+961` |
| Second result dialogue box | `sub_8564A0 -> sub_41F2FD/sub_8515A0 -> sub_40ED13/sub_851910` | FromC3EEOutput code build result dialogue and display reward; domain15TargetedPETSpecial treatment |
| C3ED Construct Functions | `sub_7BEC40` Pass `sub_413BBF` | Fixed length28 |
| Activated C3ED Call Point | `0x84DDA0/0x84E396/0x84E896` Yes `sub_84D660` Medium | Writing `WORD +8=10`, `DWORD +0x0C=token`; Do Not Initialize `WORD +0x0A` |
| Spectrum Search | `sub_9B8010`; Terminator `sub_9BC500 -> node+0x10` | Sorted recipe map returns accurate output and communication records |
| Optional End | C3EF Construct Functions `sub_84F5E0`; C3F0Another processing function in `sub_83CA40` | The requested driver 's optional path does not exist in some successful groups |
| Activated C3E8 Copy | `sub_7996F0`, `0x79B406..0x79B424` | Exact Copy 0x8C To controller state |
| Setup for Counting | `0x79B427..0x79B474 -> sub_406DD9 -> sub_86D8C0` | Every one C3E8 Read `BYTE +0x40+i` And write four DWORD Inn |
| Free Setup | `0x79B476..0x79B494 -> sub_41D769 -> sub_57DCE0` | Every one C3E8 Will `WORD +0x88 !=0` Convert to Permission Bytes |
| C42F Construct Functions | `sub_82C9B0` (`0x82C9BA`, length8) | Viewed Game List Request |
| C473 Construct Functions | `sub_82C950` Pass `sub_41EADD` | length12; Caller Writing `WORD +8=mode`, `WORD +0x0A=page` |
| C473 Mode1 Caller | `sub_7F3C30` (`0x7F3C67`) | Send mode with requested page1 |
| C473 Mode10 Caller | `sub_7F3CB0` (`0x7F3CD7`) | Send mode with requested page10 |
| C474 Process Functions | `sub_8005F0`, Standardised lowercase0xFB | Mode `BYTE +0x0A`, Count `BYTE +0x0B`, Line is in `+0x0C` |
| C474 Mode1 & Clear List | case0xFB -> `sub_41B3F6` | Empty list before an iterative count; `count=0,len12` It's safe |
| C46D Construct Functions | `sub_82CBD0` Pass `sub_41DA3E` | length16; `DWORD +8=code`, `DWORD +0x0C=selected row value` |
| C46D Encoding Field47 Caller | `sub_7F49F0`, Cases47 | Generic project transfer/using request;not composite key selection |
| C46E Assigned | `sub_8005F0` Standardized cases0xF5 -> `sub_416B85 -> sub_81D440` | Result Process Functions |
| C46E Success | `sub_81D440`, `a2[2]==1` | Consumption `DWORD +8 status`, `DWORD +0x0C code`, `DWORD +0x10 echoed row value`; Minimum length20 |
| C46EFault | `sub_81D440`, Outcome Selection | Status in `DWORD +8`; Minimum length12 |
| C46E Status2 | `sub_81D440 case2 -> sub_409507(18,0)` | Ignoring non-fault related to coin-restrictive tips/Events18; It's not a common definition—Use chain results |
| C46E Successful number42 | `sub_81D440 case1/v41=42` | Primary corner branch, precise removal and offsetting `sub_406447(code,0,row,1,1,0)` |
| C46EMajor categories of success48 | `sub_81D440 case1/v41=48` | Exact removal, event35Update with original resuscitation value |
| C475 Distribute Constructive Functions | `sub_82C840`, length20 | `BYTE +8=mode`, `BYTE +9=operation`, `WORD +0x0A=quantity`, `DWORD +0x0C=local token`, `DWORD +0x10=code` |
| C476 Distribution results | `sub_8005F0` Standardized cases`0xFD` | `WORD +8` status; status3/4 It'll consume`+0x0A` And from`+0x0C` Retrieved DWORD Code |
| Resuscitator for former livelihoods | `sub_4011B3 -> sub_7090B0` | Read `this[5504]` (Byte Offset `+0x5600`) ; By the `sub_73F3A0`, `sub_73FF50`, `sub_7F49F0`, `sub_81D440` Call |
| Resuscitator for former livelihoods | `sub_408A53 -> sub_54E430` | Writing `this[5504]` (Byte Offset `+0x5600`) ; C46EArea48In Event35, and then add byMIDefined bound credit |
| C46E Numbering48 Credit | `sub_81D440`, `a2[2]==1`, `a2[3]/1000000==48` | Exact Remove `(code,identity)`, `sub_409507(35,0)`, Read the current counter, add `sub_4194C5(resource)`, Writing counter |
| Copy Retry Request | `sub_708900` / `sub_6EAD40 -> sub_40A821` | CF95, The exact length8, No load; `0x006FBF91` It's purified CF96 Situation does not consume loads |
| Copy Result Request/Response | `sub_7087F0` / `sub_6EAA00`; `0x006F97F7` | CF87 Length12; CF88 Pack head+0x34Byte Record, Single Record Secure Length64 |
| ResuscitationUIDoor control conditions | `sub_73F3A0` / `sub_73FF50` | Yes `0x73F6A9/0x73F7EC/0x73FA9A/0x73FBD1` and `0x74017B/0x740208/0x740338/0x7403C3/0x740622/0x7406AF` It's purifiedgettercalling;zero/Non-zero-controlled retest/The result is presented |
| CF93 Construct Functions | `sub_6E76B0` (`0x6E76BA` Command Code) | Length is12request; caller inWORD+8Provide shortcut slots |
| CF94 Assigner | `sub_6EBAC0`, case53140Yes`0x6F1D9F` | Character Object WORD+8, Slot WORD+0x0A, Code DWORD+0x0C |
| CF94 Numbering14 Life Value | `0x6F1FC5..0x6F20AD` | Read WORD+0x10, Limits/Apply Character Life Values |
| CF94 Numbering14 MP | `0x6F20E4..0x6F21BF` | Read WORD+0x12, Limits/Apply Roles MP |
| C3EAGuide partitioner | `sub_859840 -> sub_4167B1 -> sub_85A8D0` | `BYTE +0x08` It's a lead step; values0/1/2 Builds a lead state, and persisted3 It avoids the lead path |
| C3EA Count key control small widgets | `sub_859840 -> sub_4032B5 -> sub_85AAA0` | `BYTE +0x09/+0x0A/+0x0B` Passed to three separate small widgets settings |
| key mode0 Settings/Render | `sub_40DDFA -> sub_85E810`; `sub_86E2E0 mode0` | In Control `+0x10` Storage/Render C3EA `+0x09` |
| Free Date Mode1 | `sub_40B7D5 -> sub_86DD00`; `sub_86E2E0 mode1` | Rendering existing expired/Current Timetamps Correct |
| key mode2 Settings/Render | `sub_40FDB7 -> sub_86E7C0`; `sub_86E2E0 mode2` | In Control `+0x14` Storage/Render C3EA `+0x0A` |
| key mode3 Settings/Render | `sub_411B44 -> sub_86E810`; `sub_86E2E0 mode3` | In Control `+0x18` Storage/Render C3EA `+0x0B` |
| C3FB Construct Functions | `sub_845E20` Pass thunk `sub_4187D7` | opcodeC3FB, Length12 |
| C3FBField writing/Send | `sub_844D10` | LocalWORDFields in Package`+8/+0A`is the current/Update guidance steps; controller bytes are set when sent`this+0x23=1` |
| C3FCProcess Functions | PassThunk `sub_41C602` Call `sub_845180` | Inspectionopcode C3FC; `WORD +8==1` denote success; always clear controller bytes `this+0x23` And releasedatapackages; minimum length10 |
| Guide call points | `sub_85A8D0`, `sub_85A3F0`, thunk `sub_408AB2` | C3EALead steps to the card/Guide objects for synthetic controllers |
| C46D Numbering Major Classes47 Request | `sub_7F49F0 case47 -> sub_41DA3E/sub_82CBD0` | The second line value of the selected item is copied to the request `DWORD +0x0C`; No multiplication or consumption algorithms |
| C46E Numbering47 Apply | `sub_81D440 case success/domain47 -> sub_406447 -> sub_7F7EE0` | Echo line values used to find and remove matching inventory objects |
| Capacity base | `sub_7F49F0 case47 -> sub_417E4F(item definition)` | Capacity is independent of the requested line value to add a defined value; supports whole line activation instead of`quantity`Multiplication |
| Second controller active object | Run-time false tables `0xC978E0` (`0xDCC8E0` IDA dataLayout), Reopened Object `0x1EA2BF80` | The object of the reconstruction shows `busy+961=0`, `completion+964=0`, `result+992=0`; It's a re-opening state, not a temporary failure |
| General Incentive Vehicle | C42F `sub_82C9B0` -> C430 `sub_8005F0` | Request-driven inventory of game items; sustainable general incentive lines to be combined |

- C3E8 Type40: `0x0079B614..0x0079B821`; Seven Record Cycle `0x0079B6D5..0x0079B762`.

Observed C3ED `WORD +0x0A=20` is a stack fill. It does not confirm any key selection syntax.

For the second controller, it workedC3EEBytes`+0x0F`consumption;the minimum security length is16.

```text
DWORD +8    400
DWORD +0x0C Accurate synthetic product code
```

It's only sent12bytes. Therefore, the result code for its second controller is read beyond the initialised package range.`sub_83CA40`the local fact that no output field is consumed and cannot be extended to the control group.

`sub_8005F0` Less50041.And so, `case0xFB` Yes50292/C474, `case0xF5` Yes50286/C46E.OriginalC46DCash list andC46EList response statement withdrawn.

C474 Mode1Line Use8Byte long, but only line code is consumed when an object is constructed; clientProvide local sentry `2100123100`.Mode4/5PagesUIPath explicitly avoids normal use of terminal page door control conditions, so empty pages clear content, but do not automatically stop page breaks.

Run-time bytes `C3EA +8..+0B=0 -> C3FB current0/update3 -> no C3FC` Now with an accurate static busy seter/The cleaner is connected. The sample provesclientBusy status unsolved, not proof that current processing has been processed and accepted.

## Skills, injuries and clean-up

| Border | Address/Call Chain | Validation Use |
|---|---|---|
| CF9B Skills requests | Construct Functions `sub_708700`; Build Functions `sub_6EA670`; Send `sub_40A821` | Operating Code `0xCF9B`, Length12, Request DWORD `+0x08` |
| CF9C Action | `0x006F1D57 -> sub_6EA6A0` | Character Search, Status/Level/Code Application |
| CF9D Cleaning | `0x006F1D7B -> sub_6EA7D0 -> sub_40773E -> sub_6E2D90` | Character Object DWORD `+0x08`; I want you to do the double-cleaning right now/Reset, Minimum Length12 |
| CF9C Skills Root Node | `sub_6E2890` | Main skills `actor+0x80F0`, Secondary skills `actor+0x80F4`, Activate Signs `+0x80F8` |
| Naturally complete the skill roots | `sub_6D9510`, `0x006DAA6B..0x006DABEB` | Root0Status2Destruction/Clear`+0x80F0`and set up`+0x80FC`; Secondary state2No full reset`+0x80F0..+0x8110`Group |
| D014/D015static field carrier; regular collision attribution withdrawn | D014Construct Functions `sub_7086D0`; Send Point `0x672AF5 -> 0x6EA3F0`, `0x672C07 -> 0x6EA440`, `0x672E05 -> 0x6EA490`; D015 case `0x6EF56B` | D014Length16, kind10/20/30; D015Source/WoundedBYTE `+0x0E/+0x0F`, AbsolutelyHP WORD `+0x10`, TerminationBYTE `+0x12`, HurtWORD `+0x14/+0x16`; Length of the security architecture24.9Month8No desolation of direct collisions by dayD014. |
| Character attacks/Gain | Fetcher `sub_6B67B0/sub_6B67F0/sub_6B6810/sub_6B6830` -> `+0xA38/+0xA3C/+0xA40/+0xA44`; CFEC `+0x2E0 -> 0x6F641C -> sub_412F3F/sub_708E80`; `sub_677A20` | SelectPON/Source attack plus current supply from adapter/Other Character Modifyer (Current)ID `+0x1E554` It's `manager+0x1FAF8`)  |
| Power-charging line collector | `0x006AE470` | PET Electricity line visits |
| Power recorder | `0x006D7300` | PET/Starter Mutation |
| Skills Activator | PassThunk `0x0041C378` Access `sub_6E2F00` | Return Character Object `+0x80F8` |
| Skills Variant Acquirer | PassThunk `0x00417571` Access `sub_6E2F20` | Return Character Object `+0x8100` |
| Post-attack/Collision door control conditions | `sub_674A00`, Call `0x00674C99/0x00674CA8` and `0x00674D2F/0x00674D3E` | When active and non-zero variant equals1Time, Skip `sub_41B5CC`, While continuing to deal with collisions,/Ammunition recovery |
| Full Skills Reset | `sub_6E2D90`, Command `0x006E2D9A..0x006E2E38` | Destroying two root nodes; Zero Active/Time state, reset variant/Sentry; +0x810C/+0x8110♪ Turn into1, No, it's not0 |

- `sub_6E2890` It's set `actor+0x80F8=1`, Yes `+0x80F0` Creates a primary node and can be used `+0x80F4` _Other Organiser.
- `sub_6D9510` In Status2; main completion will empty `+0x80F0` and set up `+0x80FC`, But the block didn't clean up the full character skills group.
- `sub_6E2D90` Destroy the two root nodes immediately `+0x80F0/+0x80F4` As 0, Settings `+0x80F8/+0x80FC/+0x8104/+0x8108=0`, `+0x8100=-1` and `+0x810C/+0x8110=1`; The security of the updateer depends on active=0.
- Follow upCF9BSuccess can't be proven aloneCF9DInsecurity;onlystate2Nor is it sufficient to prove that the character has been cleared/On the state cycle boundaryCF9DFor the removal of residual locks, the trigger range is to be independently defined.

Naturally complete the call chain: 

```text
0x006DAA6B..0x006DAAC7  Update root0; state2 -> Destruction, root0=0, gate2=1
0x006DAAD0..0x006DABEB  gate2 Update on foundation root1; state2 Call not to reset full character state
```

ReservationsclientMemory evidence includes1260A sample, with`root0=0`, `root1=0`, `active=1`, Prove that the residue is accessible at the time of operation.

- D00F Construct Functions0x7086A0 -> Indirect Jump0x41E3B2 -> Construct Functions0x6EA320 -> Send0x40A821: Length28, Type10YesWORD+8It's not a roleUID.
- D00F kind40 builder Pass thunk entry 0x41E164 ♪ Started in 0x6EA130 It's on. It's written WORD+8=40, WORD+0x0A, BYTE+0x10/+0x11, WORD+0x12 and BYTE+0x14..+0x18.It's not initialized DWORD+0x0C or BYTE+0x19..+0x1B.These bytes are residues. They can't be a target selector.
- YesclientRuns a sample, not initialized DWORD+0x0C Yes alternates2/0x0018747C . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .  WORD+0x12 carry0/1/2.fields have been staticly confirmed; will1/2Explains that the exhaustive white list is only a self-defined filter policy for adapters, not a static semantic for the field.
- D010 Assigner0x6EBAC0, case0x6EE8A6: WORD+8 Select Local/Remote Character; 0x6EE985 Lack of character leads to skipping HP Update.WORD+0x10 ->0x6EEA28 ->0x406E74 ->0x6E3000 Double after the plier HP Store in actor+0x7BC0 Location; getter0x664F60 Return value to right after encoding1bits.
- D010 Non-end Path 0x6EF06B Read DWORD+0x0C.Character Retrieving Function thunk0x41CCD8 ->0x66E740 Return Character+0x7A4C; 0x410DED ->0x746A20 Select Score UI, And 0x403323 ->0x748990 Writes one of the three rows that matches and rebuilds the decimal text that it displays D010+0x0C=0 Direct score panel reset, not harmless fill.
- D010 Yes0x6EF09FLocation, BYTE+0x1DUnconditionally read;in0x6EF241/0x6EF4E5Location, condition branch consumptionWORD+0x22.Original to zero36Bytes cover the full closed processor. Legacy28Bytes Not Type10Echo is independent of characterUIDThe correctness is too short.
- CF9C Definition Type1 Branch0x6E2A38..0x6E2A53 Keep secondary root nodes and settings8110, If the timer is greater than0; Type0 Allocation of secondary root nodes and clearance8110.Definition Type (+0x20) And the character variant (+0x8100, Copy Custom Type+0x98) Different. The memory map sample will be meat52000011 Map to Type0/Variables1, Will projectile52000007 Map to Type1/Variables0; ♪ Can't putmeatUnittype1.
- CF9D0x6EA7D0 ->0x6E2D90 Destroy root nodes, active=0, variant=-1, 810C=1, 8110=1, And0x688C30 (Global bytes=0) .Don't say that every sign is zero. Independent, impermeable0x6E0C10 Search arrays+0x7DD8/+0x7EB8 and list+0x7F7C.

- Direct local impact door condition: 0x674C99..0x674CF1 and 0x674D2F..0x674D87 Use 0x6E2F00/0x6E2F20; If active Not equal to 0 and variant equals 1, Skip 0x41B5CC -> 0x6DC8A0.With global ballistic gate control conditions 8110 Different.
- D00ERecovery0x6C9B20Remarked target+0x198Compared with the given location, rather than making an equivalent comparison.Loader0x6C8D9Ato0x6C8DA0Copy Source+0x334; 0x6CD770Execute Terminal/Status2Effect. ShareMMONot a link certificate.
- Root Completed0x41582A ->0xAB6DD0 Read root pointer only from a read-only recorder/Mark, do not call it.
- Skills transfer chain andclientMemory correspondence: meat residue in body residue samplesactive1/variant1/root1Hold, 8110=0, Global00; New ballistics retentionroot1But the timer600Overwrite As300.Target observation and static analysis are independent evidence and cannot be replaced with body residue samples.

- Redscreen Path: 0x6DC9D3 Storage 0x64FF0000, 0x6DC9F1 -> 0x41341C -> 0x743220 Configure Effects; 0x743290 Update and 0x743320 Render it. Activate/The vertigating the door condition bypassed the entire auxiliary function, but not the whole D010 HP Writing.
- Keep root object visibility: 0x6DAB7C..0x6DABC9 Calculator duration8108-(actor_frame7A50-start8104), Use final120 The beat count is in810C Toggle; 0x6DB43E..0x6DB467 Right root1Apply alpha255/0; The new ballistic calculator will reveal the retained root1; 0x6E2D90 Destroy them later.

- `sub_670E10` The intersectional branch can be called `sub_41B5CC` and D014 Constructive function: Collision object `+0x1A0` Category2, 3, When other, generate separatelykind10, 20, 30.
- D014 Request field is `+0x08` Type WORD, Source Key `+0x0A`, Collision Index `+0x0C`, And the source of the pack/Victim character bytes `+0x0E/+0x0F`.
- D015 Universal intake path usage `+0x0F` Choose the victim from `+0x10` Write absolute life values, use `+0x0A/+0x0C` Deletes the matching source line in `+0x12=200` Select the terminal, and in `+0x14` Terminal or `+0x16` Non-end consumption injury. An initial zero24The byte construction overwrites the closed path.
- Original static call chain reserved ,  and .These residues only prove the length of the tectonic, field and treatment functions.
- Direct collisionsclientThe sample contains zero resolution `pkt type=53268(0xD014)` On the contrary, it contains repeated inactivity D00F/28 Type20 Report. The sample does not support common monsters/Body/Box collisions usedD014attribution to; D014 The exact ownership of the scene is still pending.

Activate authorized10000msCleaning/12000msThe rest of the collision window is determined by the adaptor policy, not byclientAddress fact; its evidence and precise current rules04Medium.CF9D+4000msOr the continued locking of the whole battle is no substitute for the above-mentioned door control.

## NativeL8Text and packingUIRecords

GS._D20 Read From0x40E53E -> 0xAD7AE0 -> 0x41038E/0xAD7200.AD7200It's embeddedAES128Key0123456789ABCDEF123456789ABCDEF0and zero initial vectors;useCBC/PKCS7Decoded and got itNANA_TEXT.Parser0x434820AuthenticationNANA_TEXTInstall after#ID#Value pair.GSKey2346/2347It's a list of two sweet monsters, 2348YesMastlock, 2397YesMastlockCollector Title, 2419It's sweet and dangerous. Constructing functions0x6659D0Medium, 0x667132Yes+0x1CD0Writing24; 0x667196/0x6671A1Yes+0x1CE8Loading2419, Size52.0x668A1F/0x668A2AYes+0x972ELoading2346; 0x668A3B/0x668A46Yes+0x97BELoading2347, Each size is52.These writings provide descriptive strings, not monster models or production records. Two separate text samples differ in secret, but the five relevant strings are identical.

NANA_PACKHeader file size is9Bytes, DWORDCount from the first9Byte starts, then from No13Byte start countIDandrecord_offsetThe pair, the steps are long8bytes. Record byDWORDLoad Size Start; IM3The payload is followed. Current interface.packRecords291It'spayloadOffset14246215/Size1160Correspond40x30 D08Mini-Twat (exeFilename000291-qz_minimap_v05_d08.im3) ; Records1267Yes26x26Level24/R8Emblems. These exact bytes, plus the neighboring inspection/Base logs, matching the installation program logs; different from the package (1202 vs 1174These are the villages/LevelUIGraphics, not battlefields. Content boundaries06; Level Namespace03.

## RamiñosresourcesSelection

- Index to Independent Entry: movement0x5000A0, Situation166/167Call0x404A52 (6/7) ; New`0x404A52 ->0xA74450`Changedmanager+0x5CCbytes, getter `0x403D19 ->0xA743B0`Reads the same bytes. Combined belowepisode100Conversion, entrance7/Index6Selectionresourcesep22; Access8/Index7Selectionep23; Final planned entrance23/Index22Selectionep38.`0x66EC30`Through constant0xC5F374 `hd%d_ep%02d_dg%02d_st%02d.sstg`Formatting ShootingresourcesIt'sgetter.It's the entrance to the attribution7Request for firehd0, Not the villagehd4.NewC/ASM/PELocal coverage of constants and complete cloning/The missing limit belongs to06.

- Initialization of the room `sub_72F0F0` Separately save chapter sentry and in **0x72F239** with100Compare. Special Branches0x72F23E..0x72F265Pass0x403D19Get copy bytes, in0x72F24CAdd16, Call again0x40E2AA ->0x72EFC0 (0x72EFCDWriting Shooter Manager+0x1E534) , And then0x408CAB ->0x708D20Import0 (resourcesCopy Fields+0x1E538) .Normal Branch0x72F26C..0x72F2BCDo not replace. It'sresourcesName change, not change the room/Basis for transfer of the package.
- resourcesAccessor0x403D2D -> 0x677290 (+0x1E530 hd),0x418499 -> 0x6772B0 (+0x1E534 episode),0x40EE08 -> 0x6772D0 (+0x1E538 dungeon),0x418AF2 -> 0x6772F0 (+0x1E540 stage) Yes0x66EC30It'sSSTGFormatter Providesresources.Single casesThunk 0x419BBE -> 0x6658E0 Returns the shooting manager. SelectedSMMOClose0x66F1A0/0x69D510/0x69D680/0x69FB10 It's the same.
- Retroactiveraw CF6C/52: Full Screen+0x22 hd0, +0x23 episode100, +0x24 dungeon5, +0x25 stage0, WORD+0x26 difficulty0; clientDebug Open`hd0_ep21_dg00_st00.sstg`And`hd0_ep21_dg00_st00_L_00.smmo`.Existing generated table contains matchingep16toep22Combat number category. This does not fill the top village map exit/The absence of a trigger does not prove that other planned categories of combat numbers do not exist in all possible archives.
- Supported768Origin of cases-x86The experiment carried out actual comparisons/Add/Zero and local settings; linkage to declaration provides only a single419BBEAnd a copygetter403D19.It validates the value of each byte copyep100Branches, stacks, and99/101No conversion anywhere.resourcesThe availability is limited to the attached onesep16..22table. There is no byte fixable for execution, which is not completeclientReceiving and Inspection.resourcesMap policy, operational differences, and tectonic validation range06.

### SuperBossReset and NamresourcesLoad

- Request Builder0x708760 WritingCF8B/12; Mode1Sender0x6EA920 Write Actual/ShowBYTE+8/+9And modeWORD+0xA=1, and adopted0x40A821Send. Responder0x6EBAC0 It'scase53132Yes0x6FBD28..0x6FBF61.Existing applications+7/CF99Door control conditions remain valid; failure provided follows normal reloading branch.
- Normal BranchCF8CIt's0x6FBE66..0x6FBF5CPartially adopted0x4013A7 ->0x708D40 (manager+0x1E540) Writes the actual stage, difficulty and level, respectively0x6FBEEALocation, 0x418499 ->0x6772B0ReadresourcesLevel+0x1E534; Yes0x6FBEEFComparisons, 16with0x6FBEF2_Other Organiser0x6FBF22, As Level>=16. Only related cards<16♪ Time, in ♪0x6FBEFA/+0x6FBF11RenderingWORD+0x2E, And call0x408CAB ->0x708D20 (resource dungeon+0x1E538) and0x41B8B0 ->0x708DA0 (wire dungeon+0x1E54C) .So, the level22Reset ignores response level fields.A55-caseIsolated local-x86Only one case is used for execution419BBEhooks and perform actual acquisition/Comparison/setting operations; it keeps fields0/1/6/255/65535and Levels0/15/16..23/100The border. It's not completeclientRun.
-0x66EC30 WillresourcesFetcher Formatting As `hd%d_ep%02d_dg%02d_st%02d.sstg`; 0x41F2D5 ->0x69BAD0 Yes `flying/` It's open down, test stream `is_open` Pass 0x40763F ->0x43E0E0 ->0x40752C ->0x4414B0 (filebuf+0x4C Non-zero), and parse within the successful branch. File opens 0x4162A2 ->0x43E100 Spreading the zero result to stream failure; CF8B Send 0x40A821 ->0x6E90C0 ->0xADA3E0 Use connecting objects+0x78.`CShootingStage::Open ... Closed` The bottom will be executed even if it fails.SSTG Byte pass XOR0x28 Processed; it passed 0x40FBFA ->0x6A0620 To Phase Object+0x21C+260*Nine for the slot260Bytes SMMO Name.0x69D510 Read the selected name instead of the regenerated dg00 Filename.ep22 Option's first name is in the file0x5AA0, The next eight are on the scale0x20C; Keep Name instead of Rename All SMMO.resourcesFor coverage and compatibility, see06.
- SSTG The logical identifier is separate from its file name: 0x69BAD0 Read the last four XOR0x28 DWORDs ->0x40D5F8/0x669F00 Storage manager+0x1FB0C..0x1FB18; 0x403490/0x669F30 Copy them to0x70DC10.Compare with hidden retriever0x403D2D, Logical level0x41DF7A/0x7085D0 (+0x1E548), Logical Copy0x41D1B5/0x7085F0 (+0x1E54C), and stages0x418AF2/0x6772F0 (+0x1E540).Yes0x70DDC5 location, storage phase compared to the acquirer; mismatch for formatting constant0xC70854 `Open St Data File Error_%d_%d` Pass0x70DDDB/0x70DDEC In storage/Request order. This pattern is not an operating system error. It is borrowed ep22 Super script0/100/6/1 Unable to meet entrance8 It's0/100/7/0, Even if renamedresources ep23/dg00/st00 The same thing. The identity consistency experiment changes the file103116/103120, resourcesNo change in name. The failed sample does not match only the reporting stage, and no independent copy does not match the model observation; the content is in the range of operational evidence06.

### The monster's herePONPreload & OriginalresourcesDecoding

- MMO Launcher0x6C27A0 Open the requested path; version200 Use thunk0x40C7BB ->0x6C9310 For Fixed64Byte Strings and72Byte Event RecordedXOR0xABProcess. The second event count is stored in the target+0x108 (264) and Event Pointer+0x10C (268) _Other Organiser+8Place. Sixth/End Effect Event Record Places Name in Record+0location;no re-use of projectiles+8.Yes196_01/196_03 MMO, ThreePONFilename From File0x14E/0x196/0x1DEStart; corresponding records are advanced8Byte Start.
- Preload0x6CAE80Throughtarget+264/+268, Yes72*j+8Read filenames and initialize tags on target+1260Preset, Pass0x40F907/0x41B027It's going to match the ballisticsresourcesJoin the queue. Original diskPONLoader0x6B8F90Yes0x6B9020Callfopen (Accurate compilation retained) ; 0x6B903BEmpty file pointer branch entry0x403148 ->0xAAE330andFILE ERROR/Quit Path. Do Not With0x9E4B50Another pack/Script resolver, orBMOOther than thatresourcesFormat Fuzzy.
- PON105: First of all,4Byte Version Number, 260Byte bytesXOR0xABProcess the primary effect name, and read every entry by the number of rows of the effects quoted1612BytesXORRecord0/280/540/800/1060/1340String fields of the location260bytes, for firstNULis the boundary. After recording, the original loader reads24+4bytes, final reading260BytesXOREffect name. Checked196The same family file13Article records, 21508bytes. The number of files should readEFF2+16Cross-checking with the exact length cannot be considered as a hypothetical mode of playPONVersion to be verified separately.
- 12Independent originalx86Check execution0x6C9310or circular, overwrite64Byte File Slots and6Article72Byte Shooting Record; ReadFileandCRTCopy provided by Model.ReadFile IATSlot0xD91F74UnscheduledUC_ERR_READ_UNMAPPED, The result was a failure to isolate the environment, not a failureclientresourcesFailed; results after binding model only prove local resolution, noclientRun acceptance and acceptance. Compatibility policy and origin02resourcesSee you at the missing range06.

## Ordinary targets, source attacks and selectionresources

| Border | Address/Call Chain | Validation Use |
|---|---|---|
| D00DOrdinary attack | Construct Functions `sub_708640`; Construct Functions `sub_6E9D40/sub_6E9E20`; Call Point `0x006713FC/0x0067148E/0x00671856` | Command `0xD00D`, Length20; Owner WORD `+0x0C`, Source BYTE `+0x0E`, Effects BYTE `+0x0F`, Selector DWORD `+0x10`; No injuries/Fatal injuries |
| D00E Assigned | `0x006EBC50..0x006EBC66 -> 0x006EBE05` | Opcode53262 Receiving Borders |
| Terminal Selector D00E | `0x006EBEB1..0x006EBEC8`; `0x006EBEE9..0x006EBEF0 -> sub_40C0C2 -> sub_6C9B20` | Status `+0x1A=200`, Selector WORD `+0x18`, Terminals in matching scene lines/Present |
| D00E Rating only | `0x006EC884..0x006EC8E1` | Status Not200; Update Rating DWORDs `+0x08/+0x0C/+0x10`, No Selector Terminal Call |
| D00E Preload pool back | `0x006EC5F8..0x006EC61D -> sub_6A0720` (`0x006A0720..0x006A07C8`) | After the current line is missing, go through the stage manager `+2924`, Pass `sub_4100E6` Compare the same chooser, return the matching object `BYTE+0x1A` And call `sub_40C0C2`; Zero means no back matching |
| D00E Selector Missing Warning | `0x006EC786..0x006EC850`; Value `0x006EC864..0x006EC87C` | Local project type outside1/2/3/4/5/10, Especially0, Can printCP949Preloading pool/ItemUIDWarning; `NpcUid=WORD+0x18`, `iItemType` It's a local search output, noD035Fields |

```text
sub_69FB10 Read Current SMMO Slot Index at stage+0xB49
sub_69F830(stage, slot) Returns percentage bytes stage+0xB40+slot
sub_419961 -> sub_6C27A0 Percentage received
resourcesTarget records +0x10C -> Runtime Target +0x08 Original Local HP
sub_41E5D3 -> sub_6CA6C0
0x006CA898..0x006CA8D4: target+0x08 = trunc(target+0x08 * percentage * 0.01)
PE double 0x00C59EB8 = 0.01
sub_6CA6C0 Also initialize target running time state; selector writing position as target+0x2B0
sub_670E10 Network D00D Call Point 0x6713FC/0x67148E/0x671856
sub_67A320 Local/The curriculum control equipment is different HP Zero Call Chain
sub_6CD770 Process local terminations/Performance Status
```

This will close the current oneSMMOGeneral initialization formula on the path: generated`nominal_hp`YesclientInitial LocalHP.Yes`raw1459, slot110`, clientInitialize As1604.So, an originalHPStopping the boundary does not prove original initialization; it requires damage/Modification or life-cycle analysis.

Local display of unproven removal or wave completion.

Selection of pet beads and room-to-room rights are defined separately—Use Chain: 

- CF71/CF72 Character/Application of information from withindataPackage field `+0x7C/+0x80/+0x84` Present `0x6F5277/0x6F5832 -> sub_40D3DC -> sub_6D78A0` Sending three gem codes; bro information path `0x6F5B91` Use `+0x58/+0x5C/+0x60`.PA Find `sub_40CFB8` In Definition `+395..+412` Retrieval Type/Value/%. Type1Plane/Percentage of attacks targetedPETDefines the value to be capped and treated by `0x6D8585..0x6D858C -> sub_41FA9B -> sub_6E7310` Writing to Character `+0xD8`.`sub_7CE970` Update separatelyPETA valid attack on page.
- CFEC Cases53228 `0x6F6292..0x6F6F1E` From Address `0x6F6408..0x6F641C` Read `DWORD` `+0x2E0`, and adopted `sub_412F3F -> sub_708E80` Install to Manager `+0x1FAF8`.Two of them16byte rows `+0x2E4..+0x303` Call `sub_417067 -> sub_708EA0` And in the manager `+0x1FAFC/+0x1FB00` Middle Store Remote RolesID/Modifyor pairs. A single sample of zero values in the field cannot prove a generic original extraction value.

The adaptor constructs a response by this layout: `attack`Write asDWORD `+0x2E0`; Remote per LinedataFirst they're cleared, then they're in line`+0`FillUID WORD, Okay`+0x0C`Fill ModifyerDWORD (`+0x2F0`/`+0x300`) .The character of more than two remote rooms cannot be represented by this fixed carrier and is recorded as omitted/BOSSProofreading mirror sender configuration changer becauseD00D/D011Do not carry the numeric result.`defense`Only for adapters, and inD010/D015It's not a new discoveryclientFields.

```text
sub_670E10 Hit the ballistics/Source subtype selection base attack
0x671150 / 0x6715F4 / 0x671BAC / 0x6720AC
    -> sub_410357 -> sub_677A20(actor_id, base_attack)
Manager's Current Character ID  +0x1E554
Manager 's Current fixes  +0x1FAF8
Results when character matches     base_attack + Current Amended
Results when roles are not matched  base_attack + The amount of correction in the corresponding character map
Subsequent application of source multipliers, as required/Reset -> Deduction of target baseline -> Limit RemainingHP
```

Add Operation By `sub_40A2D1()==0` Door control; each branch also contains a special `sub_409052()==1` Resets to the unadjusted base, so the changer does not apply to each source subtype. The complete enforceable code offset scan is written directly only `0x708E8D` (`sub_708E80`, CFEC Install) And read directly `0x677A50` (`sub_677A20`) The manager was found `+0x1FAF8`.Remote Fields `+0x1FAFC/+0x1FB00` It's also limited to `sub_708EA0` Writing and `sub_677A80` Read.clientWriting Not With Roles `+0xD8` or C377 The control panel is associated to these slots. The difference can be assumedflat23Explain, but without memory during combat, the village speed reading is0, And soflat23It's not a valid factual conclusion.

### Presentation of personal data battle statistics C376/C377

Visible personal information panel has a third large status class.`sub_82E4B0` (`0x0082E4B0..0x008301DD`) Yes `0x0082E537..0x0082E54B` Accept C377/50039.In Local Mode, HP/MP From `sub_4089C7 -> sub_54ECC0` and `sub_412DD2 -> sub_A75850` formatting;reading in remote mode C377 WORD `+0x2A/+0x2C`.Both of them `0x0082EE1F..0x0082EE39` / `0x0082F03E..0x0082F058` Rendering C377 WORD `+0x2E` To Panel Buffer `+0x428`, and show unpublished and unchangedclientValue333Defense333.

The attack wasn't a direct mirror.`sub_A74790` First of all, from across the board `+0x6A4` Copy Local 0x24 bytes character records; copying fields `+0x1C` Through Global `+0xB0` The manager selects a record. The byte of the record `+0x40D` Select a row table, global bytes `+0x1035` Provides normal counts, and `sub_9F4830` Point to the table `+4+16*i` Medium 16 Byte Line.`sub_82E4B0` For every line `DWORD+0 * DWORD+8` (a) To make peace;dataBytes `+0x86` When set, it will be recorded `+0x40C` Writing Selector3, If the count is different from the selection, the extra line will be multiplied by a multiple C377 Word `+0x84`.And then it'll be `0x82EF91..0x82EF9E` / `0x82F1B0..0x82F1BD` & Add C377 Word `+0x38`.

As a result, local stacks only contain writing operations `0x82EF87/0x82EF9E` And the only reading operation `0x82EFA4`; Remote totals only include writing operations `0x82F1A6/0x82F1BD` And the only reading operation `0x82F1C3`.Each reading operation is pushed directly to the variable formatr `sub_B475A2`, The formatr is on `0x82EFBC` / `0x82F1DB` Place character buffer `+0x438` Write and As NUL character terminates. The search failed directly on `0x82EFDF` / `0x82F1FE` Format Service As C377 `+0x38`.Therefore, the total number of panels calculated is in the character, shot manager, bulletball ordataNo numerical receiver in package state and not involved in closed normal/BOSS The bulletball injury chain. The probe444Render attack451, Proves the local contribution of the operation7; This structure is closed and not a selected record/The semantic name of the author for which the row is assigned.

C377Fields do not amount to a complete combat attack strategy: a tectonic sample`multiplayer_send_remote_c377`Yes`+0x2A/+0x2C/+0x2E/+0x38`Write5, CorrespondclientAttack12/Defense5, Attack with adaptor9000/Defense10000inconsistent. This contrast only indicates the carrier boundary and does not define the default attributeC376The request is28Bytes, C377Response is136Bytes, Construct Functions/Sending call point is not static closed.C377The basis of the attack is:u16, Can not directly carry0..1000000The range of the adaptor policy.

### For importkind10IdentityPONBase attack

- PONLoader `sub_6B8F90` A fixed record to decode DWORD+0x114 Map toresourcesOkay+0x1A8 Yes `0x6B9458..0x6B9464`, `0x6B9ACB..0x6B9AD7`, `0x6BA13C..0x6BA148` and `0x6BA873..0x6BA87F` Cross-cutting reservations104/105/106 Branch.
- Same loader in `0x6B98B0..0x6B98C4`, `0x6BA658..0x6BA66C` Read four bytes in his brother's file PON End to `owner+0x140`; Read in order of source lines `owner+0x350`, Associated objects/Count in `+0x34C`.
- `sub_6B58F0`, `0x6B5955..0x6B5991`, Will do+0x1A8 Copy to Runtime Source+0xA38/+0xA3C/+0xA40/+0xA44.`sub_6B6700` extrapolates the last three variants; capturers are0x6B67B0/0x6B67F0/0x6B6810/0x6B6830.
- `sub_674A00` The same list +0x34C/+0x350 Line Numeric.Kind10 Call `sub_410587 -> sub_677B70` It's an expression owner+0x140 and source array loop index; builder0x6EA320 Write them D00F WORD+0x0A/+0x0C.It's done Kind10 Precision PON Basic search, not original character injury calculation.
- The disk count, it's representative1255individualPONThe key finds a precise tail key for each file and generates prefix protocols. Each file contains an extra fixed record; the solver must overwrite the full line numbering field and cannot replace zero or beyond with the adjacent positive. Full representation field: 10509Okay, 3427Positive, 438A different value, 20to5955.In Generating`release/components/combat_economy/incoming_pon_attack_data.inc`This precise rule has been achieved; the resulting character-damage conversion and reserve remains04Possession, not additional static proof.
- Kind20 Construct Functions0x6EA060 Lack of this owner/Line dimension group, with impact target selector; closedMMOLine mode hasHP/Base Type/Incentives, but no contact.Kinds60/80 Also provide object onlydata.Currency/The negative boundary of the fraction is07.

DefenseresourcesThere will be no increase in the existence of this bodyclientLocal Harm Formula.PA._D9 Type4Yeah8,530A coded defence definition, andava._D1 Type4It's a costume definition1,367One, but relevantPAConsumers`sub_6D78A0`, `sub_7CE970`, `sub_7F7570`, `sub_7F7EE0`and`sub_807200`Deal with the attack/HP/MPor assistive type, not involving type4Less profit from fighting.D00F/28Send type/Source/Select the identity of the device and the absence of defensive information;`0x6EE8A6`It's purifiedD010Situation from`+0x10`Present`sub_6E3000`PassWORDWriting Absolute CurrentHP.It'sD00F/D010It's still off the borderclientDefense gains, not constructionD010Previous use of adapter.

`sub_66F1A0 -> sub_40284C -> sub_69D510` Bytes for the selection phase+0xB49; Filename is+0x21C+260*Slot.`sub_410BD6 -> sub_69D680` Reset/Parsing the slot list; `sub_69FB10` Read/Walks through the slot only once. The generated paragraph column represents the alternative to the slot; no cumulative substitute list can be used to select the slot7Open on Runtime st01_H_01, Not that st01_H_00.

## The record of the old soldier, the injuries and the results

| Border | Address/Call Chain | Validation Use |
|---|---|---|
| LocalBossAlterationD011 | `sub_670E10`; Accurate comparison/Subtract/Clip Window`0x00671C34..0x00671C80`and`0x00672134..0x00672180` | It creates ballistics and ordinary owners/Source reporting sharing`attack-basis`, Limit to remaining life value, update sub-object/Group`+0x214`; No arithmeticCF9BDoor control conditions |
| D011 Send Call Point | `0x00671E88`, `0x00671F25`, `0x00672362`; Construct Functions `sub_6E9FB0/sub_6E9F00` | D011/32 Owner/Source/Mode/Sub Objects/Serial number/signs;no numeric local damage |
| D012BossHP | Basis `0x006EC8F6`; Yes `0x006EC904` Read `packet+0x28` | Records `BossNowEnergy`, Separated from local energy of the scene object |
| D013 Sub-object Recovery | Situation `0x006EE212`; Owner Match `0x006EE27C..0x006EE288`; Record sub-object matching `0x006EE34C..0x006EE363`; Terminal fan to `0x006EE856..0x006EE869` | Select one or moreBosssub-object;counting `+0x08`, Owner `+0x0A`, Sub-object Record `+0x0C+4*n`, Energy `+0x5C` |

```text
Boss/Group +0x318 Holds a scene object (0x224) 
Sub Objects +0x210 Total/Local Energy
Sub Objects +0x214 Current/Local Energy
D012 Record Object +0x18 Total HP
```

There's no universal formula to connect each subpoach toD012RecordsHP.For Classifications`hd0/ep4/dg2/st0/diff2`Shells+Ice popping. Read only memory records`+0x18`It's equal to the body object's current`+0x214`, And the auxiliary object energy remains independent.D012Target field recovery component; final completion is a separate adapter door control condition. For observed`hd0/ep0/dg1/st0/diff2/slot6`Run, preciseD011Owner/SourcePONAttack420/554Less base19We can recreate locals401/535Damage21-21.

- D011Identity field remains as `+0x0C/+0x0E` It's `owner/source`, and `+0x12/+0x13/+0x14` It's `mode/child/ordinal`; No new package fields extrapolated.
- The adaptor's only used the exact matchPONOwner line and effective positive source slot; not substitute for accurate attribution from adjacent sources.
- clientAssociation: owner834Attack420andowner837Attack554, Map toBossFoundation19, Local Harm401/535, Exact21Yeah21Sequence Match.

- The serial number of the same sub-object updates the sub-object/Local energy of the group; unserialized deaths will not be verified.
- D013 is the end mechanism at the sub-object level, and D012 There's a difference in the aggregation record.
- Reservationsep7/dg2resources/Run-time association attests acceptance of sub-objects0More harm than harm70992, And then forced recovery is still a positive sub-object1.

- D012 Situation `0x6EC8F6`: DWORD+0x28 Records/Read `0x6EC901..0x6EC914`; owner recording Writing `0x6EC96F..0x6EC979 -> sub_6BF200`; BYTE+0x19 Sub-object Comparison `0x6EC9E7..0x6EC9F4`; WORD+0x1A Component comparison `0x6ED0FE..0x6ED11B`; Unconditional matching sub-object fan out `0x6ED5F8..0x6ED60C -> sub_40C0C2 -> sub_6C9B20`.`sub_6C9B20` Select the indexer component vector immediately and terminate it/Association, therefore neutral should occur at a sub-object+0x19, Not a serial number+0x1A.The sub-object loops in `0x6ED62A` We'll leave, then we'll deal with it HP0/end state; therefore sub-object255And carry the final one safely D012 Status. Real sub-object, with a terminal byte+0x1A/+0x1B=`00 04`, Form WORD1024 It's not a safe way to recycle. Just keep the real object D012 For non-final component head terminalsdataPackage, with a valid serial number; D013 Responsible for sub-object level recovery.
- D011Local Path `sub_670E10`: type3Attack Read in `0x671B4F..0x671B72`; Hurt/Accumulation of limits and sub-objects+0x214Writing in `0x671C31..0x671C80`; D011Construct the point at `0x671E88/0x671F25/0x672362`.Do not transmit numerically injured or lethal fields.2026Year9Month10Day Run will147ArticleD011with147Local variation association: originalPONAttack less the frame of the component, and after termination of the limit, corresponds to the amount of each local variation; adaptor repeats and inserts the pet into the force143There's more than one normal deduction10.

### Winter EnduranceresourcesClose to recycling

I'll see you at the top, at the front and at the border06.`0x69FB10` Type3Phaseresources -> `0x4123AA/0x6BF2F0` BMOloader; mode-SondataUseXOR0x38.For the relevantBMO, count2It's a file+0x15C; Subpath from+0x160/+0x230Here we go. Record the steps0xD0.Loader Call`0x6BFFE8 ->0x419961/0x6C27A0`; MMOVersion200Let's go`0x6C2BA6`.Local fields read directly; `0x40C7BB/0x6C9310` UseXOR0xAB Decoding64Byte Strings and72byte array records. Two checkedMMOFlow to exactEOF.`0x41E5D3/0x6CA6C0` Targets+8Zoom by selected percentage; clientLocal shared energy independently confirms the sum of observations observed.

D012 Subobject Match `0x6EC9E7..0x6EC9F4`, Serial number `WORD` Read `0x6ED5F8..0x6ED5FF`, and call `0x6ED60C ->0x40C0C2/0x6C9B20` It's a karma removal chain.`0x6C9B20` The selected component and its components were terminated +0x198 Line equal to its serial number; version200 Read the association directly and keep the obsolete converter at0x6C8D9A Copy that source+0x334.`0x412B75/0x6CD770` Runs a custom termination effect, then `0x401C3A/0x6CB490` Settings state2 (+0x3C0).Nothing HP It's protecting against real children D012 , and then click the.

NormalD012The payload is60B; Inherited top-sender contains a line name section+0x3C..+0x3FTime, first time/Repeat final state frame as64B, clientYes0x6EDC14/0x6EDF10Read these bytes from adjacent branches.child255Skip widget selection, do not skip the subsequent final state branch.D013ReadWORD+8Number, WORD+0xA owner, WORD+0xC+4*n childandDWORD+0x5Cenergy;initiated single records96BFrame Overwrite to+0x5F.Final distribution `0x6EE856..0x6EE869` Use the same recovery springboard.D01132Construct Functions `0x6E9F00/0x6E9FB0` -> `0x40A821/0x6E90C0` Still assigned to existing game controller units, unchangedC/ASM/Query source and accuracyresourcesI'll see you in the ledger .Partimon Full Function Invert BackNone; This conclusion is based on a collection of instructions and is not based on false or false informationCOutput.

## Original Hans, cards, stars and pick-up displays

### SecretEpi4 PET/The card display excluded

- Phase I Public `sub_66EC30` Format `hd%d_ep%02d_dg%02d_st%02d.sstg`; Hide to get closed `0x403D2D ->0x677290`, Playset Retriever in Path `0x418499`, Copy Fetcher `0x40EE08`, Phase Retriever `0x418AF2 ->0x6772F0`.Hide1/Story3belong to `hd1_ep03` Epi4Family.
- D00E subtype30 Achieved `0x6EC007..0x6EC039 -> sub_685810`; Boss D012 The cards are the same `sub_685810`.Construct Functions `sub_6B49D0` Yes +0x00 Storage scene UID, And +0x11C Store the supplied code.
- `sub_6B49D0` Branch: When the code exceeds50,000,000, and pass the event effects; numbering12Passing skills card effects;numbering22Transmit mysterious effects; otherwise, `sub_41B65D -> sub_A74290` Get Photo Cards, `sub_40CA5E -> sub_9BED50` Find Code.`sub_9BED50` Support number12And precise picture card range13000001..13000420.All four ancient technical numbers15Code returns0.
- In Common Blocks `0x6B4D99..0x6B4E38` No zeros were found before, so the object+0x114No storage subeffects, no runningX/YFiction Settingser, no sub-activation call has occurred `0x6B4E2D -> sub_40C568(1)`.
- Tucard records `WORD+0xBC==9` Yes `0x6B4D31..0x6B4D6E` Select a pet card visual effect; fixed image string to `game\shootinggamebasic\ef_ddakg_pet.eff` and `_ef.eff`.This only means the presentation of the card category. It doesn't mean anythingdomain15Patient ownership. Decoded `ddakg._D4` Medium, 100A pet call cardfield7Both9, There are no production fields equal to15009109/15009241/15009245/15009249.
- `EDdakgi._D19` Okay50000031to50000040It's secretEpi4Event card and follow event effects branch>50,000,000.PETDefinitions/Ownership is separate: `pi._D7` It contains four lines of ancient technology; C44C Category15♪ Rebuilding from`0x80382E` Start with the record from+0x0CLet's go, let's go0x24, and adoptedPIMap solves defined identity. No command level is attached directly to a secret phase terminal to a new oneC44COkay.

- C431 Construct Functions `0x7C6B10`, UI Send `0x7C5242..0x7C5268`; C432 Assigner `0x5F71ED -> 0x40E584 -> 0x5F83C0`.C432 Cash/NaNa Yes `0x5F85C3/0x5F85FB/0x5F8611` Use+0x58; Hans/coin Yes `0x5F85A4/0x5F85E6/0x5F8627` Use+0x60.C470 `0x5F71CB -> 0x41C215 -> 0x5F7B10` Use the same offset.

| Border | Address/Call Chain | Validation Use |
|---|---|---|
| CFEChash profile | case53228 `0x6F6292..0x6F6F1E`; Numeric body `0x6F6504..0x6F6C08` | Min0x328, Highest bytes+0x327; NeitherCF80Not really subtype20 Number |
| D00E Terminal/Hansen Gate Control | 0x6EBE1A/0x6EBE1E;0x6EBEF8/0x6EBEFC | BYTE+0x1A=200; BYTE+0x1B=20 |
| Number of Hanses/Generate | 0x6EBF4A..0x6EBF55 -> 0x689FA0 | DWORD+0x1C amount; Match TargetX/Y; RoleUID |
| Hans face value | 0x689FA0 ->0x41E6E6 ->0x6D2E10 | 1/10/100/500, `ef_money_hans_*.eff`, `clash_hans.crs` |
| Local Character Door Control Conditions | 0x6EBF17 -> 0x677780 | Character DWORD+0x7D88 Non-zero value requirement |
| Local HansHUD | 0x6EBF68 -> 0x748950 -> 0x748810 | Yes+0x284& Adddatanumber of packages; showing separate accounts/Limit door control conditions |
| Tracking character UID | 0x66F6B5..0x66F734 ->0x6D3190 ->0x6B3EF0 | Find Roles UID, Update Campaign Destination X/Y |
| Hans Update/Remove | 0x66F74E -> 0x68A380 -> 0x6D31B0 | The action will be disconnected/deleting;none in this chain D034 |
| Card identifier | 0x6EC002/0x6EC039 -> 0x685810 | Subtype30, Card code+0x1C |
| Card Generation Call Chain | `0x00685810..0x00685859`; `sub_B479CC(296)`; `sub_419BBE -> sub_6658E0`; `sub_41D435 -> sub_6838A0`; `sub_40470F -> sub_6B49D0`; `sub_4182D7 -> sub_685DE0` | Allocationresources, Get a Drop ManagerID, Construct and**Insert a card into the manager intrusive Listing** (Head `+4`, End `+8`, Count `+0`; Object Link `+288/+292`) .`sub_685DE0` _Other Organiser `sub_685810` and `sub_6858C0`.|
| D034 Construct Functions | `sub_708850` (Closed `sub_405731`) ; `0x0070885A mov word ptr [eax+6],0D034h`; `0x00708863 mov word ptr [ecx+4],10h` | Command D034, Length16, No tectonic sideload |
| D034Send Functions | `sub_6EAB00` cat10/`sub_6EAB80` cat20/`sub_6EABC0` cat30/`sub_6EAB40` cat40, Use each of them `+0x08 WORD`, General `sub_40A821` | There are four categories; themes07It's“category=40”Cover only one of them.cat30Here we are `sub_4160AE <- sub_6746A0`; cat20Jumper `0x41F7BC` Indirect calls (no cross-references) so upstream calls remain closed |
| D035 Category20 Assigned | `0x6F06AF..0x6F06BC`; dataPath `0x6F06D5..0x6F06F1` and `0x6F073C..0x6F0758` | `WORD+0x0C=20`; `WORD+0x0A` Collector/Player UID Non-zero `DWORD+0x10` Card code provides a Queue of Obtained Card Results |
| D035 Getting cards in line | `sub_762810`, Exact `0x762838..0x76289F` | returns without code; otherwise assigned16Bytes, StorageUID/Code, add to combat controller `+0x970` |
| D035Scene Card Remove | `0x6F0791..0x6F0819` -> Card Manager `sub_685520` | `WORD+0x0E` Search independently of the settlement code/Remove scene object |
| CF88 Consumption of cards obtained | `0x6F9C20 -> sub_75C210`; `0x6F9C27 -> sub_7628C0`; Precise consumers `0x7628CA..0x762A13` | Consumer controller `+0x970`, Match UID To Result Block `+0x64C+0xC0*i`, Pass Node `+4` Card code, unlink/Release |
| Every player's card result barrel | `sub_76BA80`, `0x76BA80..0x76BB1A` | barrel `+8` Most of them14different codes;in byte arrays `+0xB0+i` Number of duplicates in |
| D012 Hans | 0x6ECAAF/0x6ECB4F -> 0x689FA0 | Number of chief terminals+0x1C, Separate from the card+0x20 |
| D012 Card | 0x6ECB83 ->0x6ECB87 `sub_40242D` (->`sub_685520`) ->0x6ECB8E `sub_4118A6` (`jmp sub_685810`) | First terminal card code `DWORD+0x20`; `sub_4118A6` Yes **Unique** It's `CodeRefsTo(0x685810)`, That's D012 and D00E Subtype30 sharing a card tectonic function; no `BYTE+0x1B` Compare it |
| resourcesIncentive bytes | 0x6C84EB/0x6C84F1;clone0x6C17B0 | source+0x118 -> target+0x1A; It's not a subtype condition20 |
| Star)  | 0x6EC328 -> 0x68D5C0 -> 0x6E8290 | reward10; subtype10Star Extension Experiment; `ssky_pvp_star*.eff` |
| Star Rating Collection | 0x67E180 -> 0x6E8710;0x74D990/0x74DB10 | Overlap/Rating/Rank, not track |
| D035 Category30 | 0x6F1B7D..0x6F1D2F | WORD+0x0C=30; StarUID+0x0E; DWORD+0x10 Score |

Original Call Chain byIDACommand & CorrespondPECommand check, overwrite871Bytes.dataLibrary string cannot be independently provenresourcesor path semantics;dataLibrarydataParagraph should read0xCC.

Room GamedataThe package is **CFEC**, Not thatCF80 (Confirmed by distribution table static call chain) : CFEC Situation53228Entry`0x6F6292..0x6F6F1E`Bytes`+0x327`Consumption, minimum frame`0x328`; `0x6F6504..0x6F6C08`In that case.CF80 Situation53120Entry`0x6F747C..0x6F768F`Read Only`+0x6C`/`+0x8130`, Never read these arrays, so in opcode0xCF80 Down 0x304 The byte load is invalid: 150 It's all right +0x0A/+0x0C, 20 individual UID Byte in +0x262, 50 I'll take a snag +0x276, 50 There's a symbol number of stars in the code +0x2A8, and stages/Presentation/Slot byte in +0x2DA/+0x2DB/+0x2DC (Match `send_cfec_room_profile` It's 0x328 Byte frames and theirs +0x2DA/+0x2DB/+0x2DC Writing) .`0x669870/0x669960` Map 0 Present 9 Yes 10/50/100/200/500/-10/-50/-100/-200/-500.**Hans type20 Do not consume the value cursor**; Its motion uses a separate dispersive auxiliary function.`0x68D660` It does not match the target star's progress path, not Hans' construction function.

resourcesSource+0x118 -> Objective+0x1A Yes0x6C84EB/0x6C84F1Still static; object cloning function0x6C17B0Copy the byte as well, 471011Line Awards0/5It'sresourcesStatistics can't deny the originalHansIt's working for ordinary monsters.

Falling/Generate Location Ring: `sub_6696D0` (xRing`this+31122`, yRing`this+31272`, Copy`+31426/+31576`, Cursor`+31422/+31423/+31726/+31727`, Count`+31424/+31425/+31728/+31729`) WillCFECDispersion pair installed toclientFalling manager, and only cursor retriever`sub_6699C0/sub_669A20` (Main cursor) and`sub_669A80/sub_669AE0` (Read. Their only consumer is the Decompressor`sub_6B1E50`, `sub_6849B0`, `sub_684D50`, `sub_68CFE0`, `sub_68D660`, These decompressors will take the money/Star currency denomination (500/200/100/50/10/1) Split into one generator position per unit; two fromD00ETerminal access (`0x6EC63A`, `0x6EC849`) .The ring has never been read on the character entry path (CF71Cases53105Access0x6F4E01, CFDACases53210Access0x6FBFFC) , So it doesn't have a character starter or a video camera.

## Clearing and character data anchor

| Border | Address/Call Chain | Validation Use |
|---|---|---|
| CF88 Cases | `0x006F97F7` | Settlement record cycle, stepband0x34and level/Experience |
| CF88 Level Settings | `0x006FA34E -> sub_40FEE8 -> sub_A72570` | Records+0x0A Writing Character Information WORD+2 |
| CF88Copy Level Settings | `0x006FA36E -> sub_40AAF6 -> sub_A72530` | Records+0x07 Writing Character Information WORD+0; First frame of record+0x13 |
| CF88 Percentage of experience | `0x006F9D88..0x006F9E3A` | `(record+0x10 - record+0x14) / (record+0x18 - record+0x14) * 100`, Upper limit100 |
| CF88 Level Up Door | `0x006FA73B` | Non-zero records+0x04 Access to level-up interface; final level record-keeping+0x0A |
| Write directlyC368CurrentHP/MP | `0x006FC343..0x006FC3A4` | +0x38CurrentHP, +0x3ACurrentMP, +0x34MaxHP, +0x36MaxMP; No grade calculation |
| D035 LocalUID | `0x006F0A0E` | Collector/Local Effect Equivalence |
| SuperBOSSFormat | `0x008E1D24 -> 0x00B47637 -> sub_B4AA94` | `%.2f` Double Path |

Copy level and player level adjacent but independent.`sub_41E0D8 -> sub_A72510` Read Copy Level `WORD [*(manager+0x5C4)+0]`; `sub_40AAF6 -> sub_A72530` Write it. The player level is used accordingly `+2` Access/Sets right. Carriers write including: 271A case30 `+0x0D`; C355 `0x00530F90..0x00530F9B` From `+0x24`; CF71 case53105 `0x006F4FC0..0x006F4FCB` From `+0x49`; CF0E case53006 `0x006F8938..0x006F8943` From `+0x0B`; CF88 case53128 Records `+0x07`; CF8A case53130 Records `+0x06`.C355 `+0x25` Pass `sub_40CA04 -> sub_54E410` Writing Manager `+0x4F70`; Its only acquisition path `sub_402BD0 -> sub_540350` Show/Clear Dialog52, So it's not the title state.

Title Rendering in `sub_82E4B0` End of field: Local level in `0x0082E584` Read, examined remote data provides frame `+0x29`, `sub_B475A2` Use constant `0xC9211C` `interface\%06d-qz_inter_lv_icon%d.im3` Formatting IMAGE Records `1243+grade`; Later switch Override level 0..42, Use TEXT Keys 2038..2058/2394..2412 Pass 0x410E1F/0x434710.Level 24 Images 1267 It's packed R8 tags;text 2397 Yes, it is Mastlock title 39..42 Shared Keys 2412 is empty text, not shared image. Record 1283..1285 Not included in two check interface packages; selection of semantics for phase labels03.The level of the selected phase is a separate application DWORD `+0x1E534` (`sub_418499 -> sub_6772B0`, Writeer `sub_40E2AA -> sub_72EFC0`).`sub_75F530` Local account level may be raised to the selected value, but CF6C Construct Functions `sub_708570` Organisation hd/episode/dungeon/stage/difficulty It's 52 bytes, not including levels; CF77/16 and CF87/12 It is also omitted. So, the durable reward and the sequence to grade map is the adapter ep15/dg2/diff2 Run a read-only real-time check to find account level 2 and selected stage levels 23; Together CF88 Lines and v1 Status, which supports a milestone map and attributes the error of the sample to an incentive strategy rather than to subsequent carrier coverage.

SuperBossBefore the battleresourcesExists, not deleted: `ssky_ending_bigboss_intro_ep15.eff` Quote Basis, ep15 Mechanical bear, title and VS IM3 Images, approaching the first50End of frame. CurrentdataNot in the library SSTG, SMMO, SM2, BMO, MMO or PON resourcesQuote the name, so the selection is globalclientUICode.C355 `+0x25` Turning off to manager instead `+0x4F70`, `sub_540350`, Specific type of modular frame (type)9, dialogue box52, Localizeresources9024) , This is not about the current problem. The initial file name template exists, but it's at the momentdataThere is no reference to attribution in the library. After exitclientDebugging log failed to load open path (only general black background included)EFFMessage) but the channel is not yet complete; trigger, construction function/Play life cycle and completion/Enter unlocking is still unresolved. Do not setC355, And don't fake itCF80Or forced to callEFF.

Player Level Retriever `sub_41BE41 -> sub_A72550` We've got it164A call point. It's in `sub_670E10` And precise D011 Attack on the subtractive algorithm window `0x00671C34..0x00671C80` / `0x00672134..0x00672180` It doesn't exist0x604C26/0x615AFCMiddle end of the local sampleclientReader Comparison Item/recording requirements, and0x642A4D/0x65154D/0x6EB43DComparative character level20, For Status/UIRoutes; none of these calls alter the attack, target base, life or magic values.CF88 `0x006F97F7..0x006FAD0F` Not four of themC368Life Value/Call by Magic Value Settings.

## Apartment and interior layout

- C358 Construct Functions `0x94E040`; Send `0x94BEF0 -> thunk 0x40E2C8` (`ENTER_OZVILL`).
- C38D Construct Functions `0x509B90`; Organisation `0x94B870 -> thunk 0x40C699`; C38E Process Functions `0x94C540` and `0x531BD0`, Situation50062.
- C409 Construct Functions `0x566300`; Room/Internal Send `0x595040/0x563330 -> thunk 0x4160EA`.The normal backpack was sent to C430 Completed `0x804999 -> thunk0x4114BE -> sub_7F4690`; Yes `0x7F46C4` It writes DWORD+8=10, And then `0x7F46D7 -> sub_ADA3E0(0,packet)`.Construct Functions Writing WORD+4=12 and WORD+6=C409.Inventory+0x9A It's a request to complete the door control condition.
- C40A Consumers are independent: villages `sub_531BD0` It's case50186 Yes `0x534BA9`, Call `0x534BB6 -> thunk0x41A663 -> sub_541B30`; Apartment scene `sub_59F660` It's case50186 Yes `0x5A2BB8`; Internal controls `0x561070`; General inventory `sub_8005F0` It's case50186 Yes `0x8049A3`; Special List `0x5D2610` Accept30, `0x5D90C0` Clear request30.At normal completion, inventory+0x9A Writing `0x805300`; +10==6 An extra DWORD+0x400 Read `0x804A1E` (Length/Field engagement03) .
- Village Processing Functions `0x541B51/0x541B62` InspectionBYTE+8Whether it is30/10, On Match `0x541C00` returns;other values writtencontroller+0x3EC, Subsequent decitationcontroller+0x3C4.Yes `0x541BB3` Implementation `mov dword ptr [edx+0xB4],0` (EDXYes `0x541BAD` Fromcontroller+0x3C4Loading;other articlecontroller+0x3C0==2Path in `0x541B8E` Do the same unsafe writingWindows APPCRASHReportC0000005and module offset0x141BB3; CombinedPEBase0x400000, This is exactly the same order `(RoomState)` Debug string is not sufficient to determine the processing function without an abnormal address.
- Preview Life Cycle: `sub_523890` Clear controller+0x3C4 Yes `0x523AAD`; `sub_543B70` Only active panel62/65andMyInfo+0x5604==1Distribution in CasesCRoomPost(size0xD4, Construct Functions`0x9DFB70`), ♪ And installed on `0x543CA1/0x543ECE`, And then load itBasic.rom/Floor/Wall. This means that there is no need for a normal backpack. There is no record of a collapseEDXValue; static empty initialization paths and isolated empty pointer experiments re-emergenceWER EIP, Not a live memory.
- C40B Construct Functions `0x5B2630` By CRoomState Construct Functions `0x58DC40` Organisation; buy Send `0x5949F0`; C40C Situation50188 Pass `0x40CF4F -> 0x5AA7B0`.
- Construct Functions C411 `0x5B2960`; Parsing/Builder `0x593E10 -> thunk 0x41A0C3`; C412 Situation50194 Yes `0x59F660`.
- C423 Construct Functions `0x54D960`; Initial request `0x5A8D30 -> thunk 0x403846`; C424 Situation50212 Yes `0x59F660`, Maximum consumption bytes+0x413.

Apartment field capture per se does not prove that the normal village path is protected; the consumer ' s character is determined by a matching command, anomalous location and an auxiliary branch experiment.

- Examples of furniture written in chains: roomsC40ACall Point0x5A3101 -> thunk0x41C251 ->0x5C65D0(code,TakeOn,Index); Yes0x5C67A2and in the ten category branches, after0x405E5C ->0x5CEFC0, Yes0x5CEFCDWritingnode+0x33C.The item code is uniquely locatednode+0x338 (Set Functions0x5CF470/Read Functions0x5CF490) .ExistingIndexRead Chain0x41DB6F ->0x5CEFE0Yes0x5CEFEARead+0x33C.
- Selected0x5C90D0 Use controller+0x3DC As selected index. Right0x41235F the fifteen comparison calls correspond to0x5C9B89/0x5C9C33/0x5C9CF8/0x5C9DC6/0x5C9E94/0x5C9F62/0x5CA030/0x5CA929/0x5CABA0/0x5CAE2E/0x5CB095/0x5CB30C/0x5CB583/0x5CB7FA/0x5CBA61; Brother floor/The wall comparison has been made using a native indexer. Select has been completed0x413C9B ->0x5C6130 General/Queue under Index List; C411 It's a prerequisite for a later visible submission, not for a furniture click.
- Protected Index getter The current chain of evidence is `0x41235F -> 0x5CE700 -> 0x100F646 -> lookup 0x1011A51`.It's a residual protection PE Quick, head pointer `0x16A3E80`, Isolated snapshots in `0x1011A51` Read `[0x16A3E80+4] = [0x16A3E84]` Times Failed (4bytes; this means reading the address, not restoring the pointer content.WindowsAnomalous sample `0xC0000005` Module Offset `0x00C11A51` corresponds to it. This local search precedes the adapter line field consumption, C411 Submit later than furniture selection; 03As stated mode20/consumer0/11The original log of the line cannot be rewritten to be old mode10 Route failed.
- This conclusion is based on and corresponds toPEUnanimously258ArticleIDACommand: `0x5CE700`(3), `0x100F646`(109), `0x1010140`(14), `0x1010850`(52), `0x10112D0`(15), `0x1011A30`(6), `0x1011A40`(47), `0x1011F70`(12), Plus quarantine stale-heap lookup.The entrance instructions don't matchdataThe library can not be compiled as evidence of that protected routelive stack, Storage or completeness crash dump; We can't keep static/The speed path is used as a full call pad for the catch, and it is not possible to release the full adaptor package route and access status. The pure adaptor complete furniture functionality has not yet been validated; furniture removed, fakedC411Or a mistakeIndexNone of the functions can be proven Application Records.
- C392Construct Functions0x5B2810/Jumper0x411B8FWriting12/C392.Initialise Send Point0x590011Control request0x592099Writinginfo1000, Yes0x5920B7I'm sorry0xADA3E0Send.C393 case50067in0x59F660, from0x5A0FF0Start: NumberWORD+8, info WORD+10, Normal records+12/Step length12; 6000Into the relay branch0x5A167A, Otherwise, it is considered the final snapshot2000Values belong to policy.type4Definitions+1036==2♪ Time, in ♪scene+0x4CCDistribute rectangles toscene+0x56CIndex, range40A pointer. The protocol/Layout upper bounds and scope of application only03Maintenance.

Protocol Size/Lines and pricing/I'll see you at the end of the day02/03.C38ESafe coverage of all consumed bytes, notclientVisible effects acceptance.

### C47D Destruction in the preparatory phase

- Send them unconditionally in the roomC47E -> C368 -> C47F -> C379 -> C3CC -> C44CIt triggers the destruction of the scene.C368 Assigner0x006FC343 Achieved0x006FC3A9/0x006FC3B0 -> sub_4188C7 -> sub_6754B0, This releases the shared scene from the shared scene manager+0x24.Readiness20x0071B7A0 Keep the controller+0x14==1, Skip Distributor0x0066EC30, And0x0066F1A0 Will NULL Passed to0x0069D510; 0x0069D51D Will EAX0 Writing `[eax+0xB49]`.WER Offset0x0029D51D Matches. Multiple people`room_active`Depression when effectiveC368/C47F, Stock confirmation vehicle remains unchanged. The failure of the apartment to run indicates that the state cannot rely solely on a copy sign: C38D/C38E Internal set up, but copy `room_active` Still0, And so C47D Send C368/C47F ♪ And make andclientSend it now C367 Page135 `(388,220)`.The adaptor independently maintains the local apartment status cycle; it is based on the above-mentioned operational phenomena and theC368Static chain, not involving new package offset conclusions.
- Selector4LocalPETReturns with ClosedC47FRole target processing function. Terminal window containsCF1D/CF1E, Character Update, C367/C368Page18andCB21Move, noC36C/C47F.Selector4 C367Processing allowed for first timeCB21TriggerC47FBack up; C365No character added in destructiondataPackage.

