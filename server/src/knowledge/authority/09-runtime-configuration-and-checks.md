# 09 Runtime configuration and checks

> Translation draft: numeric evidence and code references are retained, but the prose still needs technical review. Use the [workspace guide](../../../../README.md) for current setup. Original research documents are preserved in the workspace archive `.work/original-source.zip`.

This theme note release The protocol field and the static call chain are available01–08; Installation, construction and tool preparation [Run dependent](../../docs/runtime-dependencies.md) with [Build and Validate](../../docs/build-and-test.md).

## File Structure

The following paths are relative to the repository root directory:: 

| Component | Path |
|---|---|
| Activate the entrance | `start_nanaimo_launcher.bat` |
| Launcher | `gui_launcher/nanaimo_launcher.ps1` |
| clientConnector | `gui_launcher/client_connect.ps1` |
| Protocol adapter | `adapter/nanaimo_adapter.exe` |
| Adaptor Source Entry | `adapter/nanaimo_adapter.c` |
| srcComponent | `release/components/<Function Name>/` |
| srcReliance List | `manifest/source_closure.json` |
| Operational list of documents | `manifest/open_release_manifest.json` |

srcRepatriation by List Recording include Closed and build output protocols; running list of documents to record file sizes SHA-256.Document identification does not amount to a protocol semantic orclientShow effect validation.

## Connect to character information

Launcher Usage Network Mode Connection `127.0.0.1`; The adaptor listening address is: loopback.Login, data registration, game entrances and game sessions are different connections, field meanings are connected andclientStatus distinction, see [Protocol frame & & connection](01-framing-and-connections.md).

Processes for entering the game include keeping selected character information and inventory, loading local adapter, waiting to listen, registering character information, and then startingclient.Standalone Constructor with Network The protocol for the constructor is different [Login of villages and roles](02-login-village-and-characters.md), Field layout should not be mixed.

## Endurance and Status Carrier

- Levels and experiences use a character-based step record. Configure parameters `level` Initialization of missing records only; existing roles used in a durable account book.
- C355 Village information, CF71 Room information and CF88 Clearing from the level of the corresponding character/State of experience; information source views [Character information and backpacks](03-profiles-and-inventory.md), See reward and settlement fields [Picking up settlements and rewards](07-pickup-settlement-and-rewards.md).
- HP/MP, Attack/Defense strategy, selection of pets and equipment properties have their own carrier and calculation rules and are not directly derived from grade values.
- Enduring File Name, dataFormat version and protocol identifiers are the implementation interface. The loaded status in memory is not automatically synchronized with the disk file.

## Evidence and realization of the boundary

Static analysis to determine function calls, field deviations and read boundaries; adaptor tectonic validation to check text sections, sequences and state doors; clientOperating observations are used to describe the actual behaviour of a given input. Three types of evidence are not interchangeable.

Numerical policy in local adapter is not equal to the original adapter algorithm.SSTG, SMMO, PON And the roadresourcesI'll see the selection rules05, 06, 08; Same familyresourcesRetaliation is a compatible strategy, and it doesn't mean that the missing originals are restoredresources.
