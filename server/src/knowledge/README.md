# Nanaimo protocol knowledge base

Research notes on client packets, state transitions, resource formats, and native
code addresses. These notes describe observed behavior and implementation evidence;
they are not the current application's installation guide.

| Reference | Subject |
| --- | --- |
| [Framing and connections](authority/01-framing-and-connections.md) | Frames, checksums, connections, state lifetimes |
| [Login, village, and characters](authority/02-login-village-and-characters.md) | Login, pages, character objects, pet followers, apartment entry |
| [Profiles and inventory](authority/03-profiles-and-inventory.md) | Profile state, experience, item instances, shops, cards, quick slots, furniture |
| [Pets and combat](authority/04-pets-and-combat.md) | Pets, skills, damage, projectile lifetimes |
| [Rooms and multiplayer](authority/05-rooms-and-multiplayer.md) | Room loading, peers, routes, scene coordinates |
| [Targets, drops, and bosses](authority/06-targets-drops-and-bosses.md) | Target identity, resource selection, health, chests, drops, boss lifecycle |
| [Pickup and settlement](authority/07-pickup-settlement-and-rewards.md) | Pickup, currency, experience, reward policies |
| [Static addresses](authority/08-static-addresses-and-call-chains.md) | Functions, offsets, call chains, field access |
| [Configuration and checks](authority/09-runtime-configuration-and-checks.md) | Historical release layout and verification boundaries |

Interpret a packet using the connection, dispatcher, client state, opcode, and
payload length together. An opcode alone does not identify one universal layout.
Keep static analysis, observed client behavior, host tests, replay, and assumptions
distinct. Adapter policy is not evidence of an original server algorithm.
Static addresses apply only to the corresponding client binary layout.
