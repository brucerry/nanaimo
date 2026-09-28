# 07 Pickup, settlement, and rewards

> Translation draft: numeric evidence and code references are retained, but the prose still needs technical review. Use the [workspace guide](../../../../README.md) for current setup. Original research documents are preserved in the workspace archive `.work/original-source.zip`.

## Veterans Terminal Hans

Static call chain proof, abort D012 DWORD+0x1C Independent D00E Into the original Hans Construct a function. The adaptor custom policy will be selected Boss Modes of information definition HP Plus complete total HP, Request HP>=200, Only once in the first final termination `floor(total_hp/50)`, and write the same number D012+0x1C; Duplicate and non-termination reports carried0.4000-HP Monomer Boss Host testing only occurs once score4000 and Hans80.Just Boss HP Less than200♪ Only when ♪ D012+0x20 The card fields are clean. These values are part of the visible adapter custom policy/Track movement, card availability and account numbers/Re-entry Consistency still requires a new round of operation validation.

## D034/D035

```text
D034 +0x08 WORD  Category: 20 Card, 40 Combat effects; independent star manager other category30
D034 +0x0A WORD  Site items UID
D034 +0x0C DWORD category20 Card code; category40 Type: 1 Power, 2 HP, 3 MP, 4 Speed
D035 +0x08/+0x0A Local Roles UID
D035 +0x0C WORD  Recursive Category
D035 +0x0E WORD  Showback scene items UID
D035 +0x10 DWORD Echo Card Code/Effect Type
```

Local UID Equivalence is necessary for the local effect path.8/8tectonic sample not satisfied`0x006F0A0E`It's purifiedUIDcomparing;current-by-dateclientResponding to the use of actual collectorsUID.Share Response Maintained 24 Bytes; category20 It's through itself `+0x13` Consumption, while other branches use retained tails.

Yescategory20, Non-zeroD035 `DWORD +0x10` It's original**This fight gets the card result carrier**.clientCreate16Bytes`{collector UID, card code, prev, next}`Node and attach it to the combat controller`+0x970`.CF88Then we'll clear the queue, and we'llUIDMatches three of the player's result blocks (at`+0x64C + 0xC0*i`) , And record most of them14A different card code; a duplicate code adds a byte count.`WORD +0x0E`Remove the scene card object independently and cannot replace this result carrier.

The tectonic function to be inherited will D034 Medium `+0x08..+0x0F` Copy to D035 Medium `+0x0C..+0x13`, And then it's covered with zero category20 Medium `DWORD +0x10`.This explains why the operation is successful, and the result page is still empty. Only the overwhelming operation is removed. Repeat category40/category20 Pick-up is still of the same kind: first accepted pick-up is sent/Radio D035; The double pick-up won't get the second one ACK Or a durable increment 2026 Year 9 Month 13 Oh, shitclientRunning, category20 Medium codes13000028, 13000094 and 13000139 Granted and `t=278s/335s/336s` _Other Organiser D035 Back, next CF88 _Other Organiser `t=433s`, Every last count has increased 1, The list of cards obtained after the user confirmed the cleanup is visible **End-to-end acceptance of key result list behaviour satisfied**.No screenshot to save the exact localized name/order;same code count2, Uncarded, re-logged-in retention and multi-person collector unique attributions remain unaccepted.

## CF87/CF88

CF88 It's a settlement/Character information status. Record steps are as follows:`0x34`, No, it's not`0x38`.

```text
record +0x04 BYTE  Upgrade Door Control
record +0x07 BYTE  Copy Level After Settlement/Title Index
record +0x0A BYTE  After Settlement Player Level
record +0x0C DWORD Increased empirical value of this settlement
record +0x10 DWORD Absolute Cumulative/Current experience value
record +0x14 DWORD Threshold threshold for current level
record +0x18 DWORD Next Level Threshold
```

CF88 For locals, you can also get a copy of the award UID Records, `record+0x07` Achieved `0x006FA36E -> sub_40AAF6 -> sub_A72530` and directly replace the account copy level; clientThe grade is not calculated on the basis of fractions, experience, level or difficulty. The same value must be passed later 271A/C355/CF71 To recover.

The value is provided by character record. For static/Delivery milestones distributed at running time `hd0/episode15/dungeon2/difficulty2`, stage0 Reward floor level23, stage1 It's been standardized into the same stage0 Front line, so superBossThe settlement does not add a second level2/st1 Organisation23/st0, and keep a higher level. Unknown characters only keep older dictionaries more strict than+1 The rules are compatible. Death is still called CF88, Progress disabled; combat-cycle heavy `settlement_sent` Prevents duplicated submission. The global metronology of the adapter to the grade map is still unknown.

StaticHUDThe arithmetic operation limits the current value to the next threshold and calculates the unsigned`(current - lower) / (next - lower) * 100`.It won't be repaired`current < lower`or`next <= lower`; Need for a consistent adapter threshold.clientNo new grade or threshold curves extrapolated: all values above are provided by adapters.`record+0x0A`Pass`0x006FA34E -> sub_40FEE8 -> sub_A72570`Writing character information level; non-zero`record+0x04`Yes`0x006FA73B`. This process function does not recalculateHP/MP.

It's from the adapterEXPAwards/The curve is still unsolved in value. The adaptor uses a defined policy of explicit certainty: a successful request drivenCF87/CF88The character of the final settlement reward for the request100 EXP; Death settlement bonus0; From LevelLThe cost of the upgrade is`L * 100`; LevelLAccumulated lower limit`50 * (L-1) * L`; The upper limit of the grade is99.Sets if one or more thresholds are crossed`record+0x04=1`And send the result level and a consistent current/Lower limit/Next value. Every character/Up to one settlement for the combat state cycle and atomized until successful values are released.

Sustainable identity, seeds, atom substitution and villages/I'll see you in the room03.Request DrivenCF88Still the only clearance incentive submitted; duplicatedCF87We can't repeat the reward. Death increases0, AndCF8B/CF7FReactivate a unique battle cycle instead of second submission in the old cycle. Hosts build to holdLV1/EXP0 -> LV2/EXP100, With a sign1and thresholds100/300; Restart RestoreLV2, Second clearance bonusEXP200/Signs0.The current cross-border test also verified the next copyCF8B/CF8C/CF70, It's not syntheticC355, CF73/CF1DFollow the request after returningC355, Repeated re-entry inhibition, level37 EXP66700/66600/70300 (2.7027%) , Level88 EXP382900/382800/391600 (1.1364%) , Near-threshold status and ceiling99.Death/The resurrection and the double character test is an independent host return test. There's no new oneclientAnimation/Progress Bar/Re-entry of adopted statements.

88.8%/100%Showing inconsistent failed samples, CF71Will Current/Next threshold is200/225, C355Provision0/0/100, AndCF88Provides three-tier groups of books of accounts0Time200/225=88.8889%; High LevelCF88After, CF71It's200/225Combination with higher lower limit causes unsigned spills and proximity100%Display, C355It's0/0/100It covers the real state. The phenomenon is not consistent with the carrier values and cannot be determined as a low settlement percentageEXPLost. Current carriers use consistent and durable account status; this is not proof of the original incentive formula, nor is it authorized to discard the earned status.

## Player Death Test and Clear Borders

clientDeath failure sample, adaptort=518Second local logD010TerminalHP0, t=529Roger thatCF95/8, t=537Roger thatCF87/12; WhenCF95There's no equivalent. DeathCF87StillBoss-finalWhen the door is controlled, the process does not advance properly. This is proof of failure and does not constitute a retest as described below/Death treatmentclientReceiving and Inspection.

Static Call Chain ProofCF95 `REQ_FLYSHOOTING_RETRY` By `sub_708900` Build as Loadless8Bytesdataand by `sub_6EAD40` Send; CF96 Distribution of cases `0x6FBF91` Do not read any load, so its minimum security build is8Bytes.CF87 From `sub_7087F0`/`sub_6EAA00` Start with length12Bytes; WORD+8andWORD+0x0A It's not proven dead/Clear Marks.CF88 From+0x0C Start recording and by0x34♪ The steps go on, need ♪ `12+52*record_count`, So a player needs64Bytes.

By local authority during the combat cycle HP From positive to zero recognition of death.CF95 Request received CF96/8; Restoration of authority for a possible re-test HP, But the death lock marks remain in place D00D/D00F/D011 or CF87 Come. Reactivate fighting will clear the mark. In the same cycle, after death, CF87 Copy that immediately CF88/64, Not To Boss Mark as defeated/Cleared and not issued Boss Hans Or a pass-through bonus CF87 It's still real Boss/resourcesFinal state door control. Host self-inspection covers two branches. Original adaptor is dedicated to the death ranking/The results are still unknown, and new retests, results rendered and operational acceptances for return to villages are still pending.

## Result/Rank Vector

D012The status and ranking is by slot number, and each active slot is recorded in doubleclientThe sample observed`SUCCESS_CLEAR/S`; The sample is not replacedreleaseOperation and acceptance.

Scores, results status, rankings and reward vehicles are separated. VisibleSRating does not prove the current formula or the grade threshold.

## Source of generic scoring figures

clientThere's no score field for every monster in the target's ledger.D00E Three absolute scores received from non-ends DWORD, Separated in +0x08/+0x0C/+0x10; D010 Non-end received absolute score and is located in +0x0C, Pass 0x748990 Writing matching rooms-Slot Line; CF88 Total received settlement/Rank status. Separately with a symbol star value 10/50/100/200/500 and category 30 The collections are star-class/The ranking manager, not the normal monster-kill score sheet. Therefore, the adaptor self-defined strategy keeps the score constant for each non-end attack and adds the maximum initial status of the target at a time on the first terminal, which is being converted to zero HP.Normal D00D, Type of binding 20 The same rules are used for collisions and meat target terminals HP Sum. Duplicate, superfluous injury, invalid and failed start-up reports are divided into zero: HP90 Terminal -> Score90; HP1363 After two attacks, remain unchanged, then add1363; Chief HP4000 Add to final terminal only4000.This is not an original adaptor formula.

## Currency borders

CF8A Only settled/UIStatus, balance setup not called.

```text
coin/Hans   group1 u64
nana/Cash   group2 u64
```

The resulting target book is fully recorded and shared471011Okay/2642individualresources, Include onlyreward_kind0 (466340Line) orreward_kind5 (4671All right. All right121295Lines belong1046It's named after a monsterMMOresourcesBothreward_kind0; reward_kind5It's a verified piece-random effect path, not aHans.An ordinary monsterHansSupplyed by Consumable AdapterD00E/D012 DWORD+0x1C, AndclientresourcesProvide only1/10/100/500The display unit. So, there's no one for every monsterHansThe normal coin strategy, capacity, difficulty selection, fraction/Rank/Level thresholds and experience curves remain part of the adapter ' s custom-defined policy and do not contain the original adapter ' s numerical closure.

## SuperBossIt'sEXPAbnormal Boundaries

TwoclientThe sample is in `0xB4AB10` There's been an anomaly. We're here `%.2f` DualFormatizer: 

```text
0x008E1D24 -> 0x00B47637 -> sub_B4AA94
```

It's not onePON/Base of the shooting game`%s`resourcesNam Formatting Tool.

There's no consistency in sharing high-level failureEXPState causes an unmarked spill/Deleting Zero Error.D3DIM700Invalid Read is an independent crash signature.

## Consistency of results

Here's the deal/Status constraints, not all throughclientStatement of acceptance and acceptance.

- Result/Rank by active slot; 
- CF88 Levels remain specific; 
- EXPCurrent values, floors and next thresholds are consistent; 

- The settlement should not cover editableZ/XStatus of skills; 
- Repeated pick-up and capacity failure remain safe; 
- Customized scoring formula does not equal the original adapter behaviour.

## SuperBossWar and warclient_Other Organiser

ep7/dg2Failed sample: 

```text
D013 Hardcoding child1 -> D012 final -> CF87/CF88 -> CF15/CF16
CF8B real1/show0/mode1 -> CF8C real1/show0/diff1/dg2
client CF73 -> CF1D -> Adapter CF1E/200
```

clientYesCF8CAnd then start leaving; CF1EIt's not the first reason.CF8CLength is48, And consumed fields`+0x28/+0x29/+0x2C/+0x2E`It's complete. Matches the current difficulty/Copy. The strongest share invalid status is previousBossLife cycle: child0No recyclingresourcesHP♪ Beyond the limits, and ♪child1And inresourcesHPForced recovery below.

Current Build UsagedataDriverd sub-object recovery instead of special use. Host builder supports settlement and contains `CF8B -> CF8C -> C587 -> CF70 -> CFEB stage1`; JustclientRun-time can proveclientNow choose the path instead of the path CF73/CF1D.Don't make it difficult1Force as Difficult2.

## Native Hans and Enduring Account Policy

Native PvE D00E `status200/subtype20` Will `DWORD+0x1C` Read As Hans Number, pass0x689FA0/0x6D2E10Construct `ef_money_hans_*.eff`, And immediately add that amount to the local battle HUD (`0x748950`, Fields+0x284) .Native Animation by Character UID tracking movements;verified updates/Remove chain not sent D034, No second account reward. Visual completion and permanence are two separate borders.

Subtype30 It's the card path (`0x685810`), Not that subtype20.Single D035 Process Functions **It does category30**, Scope 0x6F1B7D Present 0x6F1D2F, I've been through it `ssky_pvp_star` The manager also switchs the group status, D034/D035 Include Only category20/40 The general assertion has been withdrawn. This host certificate does not establish an ordinary one Hans Nor can you authorize a false or unrequested request D035.Existing category20/40 The semantics of the request remain valid.

Minimum life value for initialization of target entries for the adapter ' s custom incentive policy (HP) .If HP<200, The first terminal will not be sent Hans And don't send a card HP>=200, Reservations1%; if the card is drawn, use the subtype30 and replace Hans The look, or else subtype20 The number is exactly the same `floor(max_hp/50)`.There's no random number rating and money tree multipliers that change this value, HP1363 Host Terminal Carried/Reservations27; HP90 Carry0.Type of binding20The same general reward path now shared with the meat target terminal.Boss D012 Carrying Alone at First Final `floor(total Boss profile HP/50)`, For example:4000→80.These are local self-defined choices, not original probabilities `shopping_state_save`, Spill/♪ Lose to roll, fight ♪/Maintaining the terminal before it is generated and before it is recorded remains mandatory.clientAny absolute amount can be broken down to the base amount; no new inventions50Currency orCF80Cursor.

Changes in balances in running samples99,999,999 -> 100,000,159Just prove itsubtype0Auto-record, no animations.subtype10The Star Extension Experiment isn't original eitherHansReceiving and Inspection. Visible Generating/Home, HUD/Account number/Back/Re-entry Consistency and Connection Stability is incompleteclientauthentication;multipleclientThe distribution and settlement records are also not closed.

See static construction functions and package fields08; Target eligibility and terminal rules are available06.
