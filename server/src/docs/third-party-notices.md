# Third-party notices

The project source is licensed under the GNU Affero General Public License
version 3. The canonical license is [LICENSE](../../../LICENSE) at the workspace
root. Third-party components retain their own licenses and copyright notices.

The original project describes non-commercial, local research into Nanaimo client
behavior. Nanaimo executables, libraries, game data, artwork, and audio belong to
their respective rights holders. The source license does not grant redistribution
rights for those assets or for the original installer archive.

Keep each component's original license and copyright material:

- TinyCC: the supplied toolchain under `../../tools/tcc/`, including its
  `COPYING`, `COPYING.LIB`, headers, libraries, and other notices.
- .NET and Microsoft.Data.Sqlite: their upstream license and NuGet package notices.
- Python and diagnostic packages: their distribution licenses, if installed.
- Bot name material: `VillageBotNameCatalog.cs` credits the Apache-2.0
  [Chinese-Names-Corpus](https://github.com/wainshine/Chinese-Names-Corpus).
- The optional localization tool uses OpenCC's conversion dictionaries through
  `opencc-python-reimplemented` and PyCryptodome. Preserve their upstream licenses
  when distributing those tools or dependencies.

The English documentation replaces obsolete claims that the whole workspace was
source-only: this supplied workspace also contains proprietary client binaries and assets.
