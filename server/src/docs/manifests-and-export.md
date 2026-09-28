# Manifests and historical exports

The current merged build writes `client/server/server-merged/build-manifest.json`
and `build-manifest-win7.json`, recording published file sizes and SHA-256 hashes.
These are runtime build inventories, not licenses or a public release allowlist.

The retained `scripts/export_patch.py` supports the historical native adapter
distribution. It previews by default and exports only reviewed allowlisted files.
It does not install a game, build the merged application, patch a client, or grant
redistribution rights for game assets.

From `server/src/`:

```powershell
python -B scripts/export_patch.py
python -B scripts/export_patch.py --dry-run --layers source,knowledge
python -B scripts/export_patch.py --help
```

| Layer | Historical contents |
| --- | --- |
| runtime | Launch scripts, native adapter, runtime helpers |
| source | Reachable C/include closure and build/verification scripts |
| knowledge | Protocol research |
| docs | Project guides |
| python / tcc | Optional, separately reviewed external tool records |
| testports | Isolated test-port adapter |

The default layers are runtime, source, and knowledge. Dependencies expand
automatically; testports is not selected by default. A source-only export can
verify its include closure without being a complete runnable game or test suite.

`manifest/source_closure.json` records reachable quoted includes for the native
adapter entry points. `scripts/refresh_source_manifest.py` refreshes this inventory;
it retains previously reviewed binary output contracts when binaries are absent.
`manifest/patch_allowlist.json` records the historical export candidates.
Use the export script's review/status mechanisms when changing an allowlist;
refreshing a hash alone is not a distribution review.

Client executable baselines and game-resource hashes must remain separate from
source inventories. Localization changes resource hashes legitimately, but does
not justify silently accepting a different executable baseline. Consult
[third-party notices](third-party-notices.md) before preparing any distribution.
