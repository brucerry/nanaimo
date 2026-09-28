# Nanaimo server source

The active server combines managed C# gameplay and SQLite persistence with the
original native C dungeon worker. The Windows Forms manager is in `../../launcher`.
Start with the [workspace README](../../README.md) for the player and build workflow.

- [Architecture and scope](docs/merged-server.md)
- [Startup and source map](docs/startup-and-source.md)
- [Build and verification](docs/build-and-test.md)
- [Runtime dependencies](docs/runtime-dependencies.md)
- [GM tools](docs/gm-guide.md)
- [Manifests and exports](docs/manifests-and-export.md)
- [Protocol knowledge base](knowledge/README.md)
- [Third-party notices](docs/third-party-notices.md)

The historical `gui_launcher/` and native adapter scripts are retained for
research. Their original launch workflow and client hash baseline do not describe
the current merged portable package. The current build entry point is
`scripts/build_merged.ps1`; `serverMode` must remain `merged`.

The original project describes local, non-commercial client-behavior research.
Source visibility does not establish redistribution rights for the game client
or assets. Preserve the project [LICENSE](../../LICENSE) and third-party notices.
