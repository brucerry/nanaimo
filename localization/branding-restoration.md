# Logo removal

The game keeps its localized text name, but displays no replacement wordmark.
The original publisher/game logos and the rejected green badge were removed from
18 chapter and loading backgrounds. [branding.json](branding.json) lists every
installed image, template, edited rectangle, and generated-output checksum.

Background repairs use the built-in image-editing tool. Only the recorded logo
rectangle is composited back into each IM3 template, with a narrow blended edge.
An exact decoded-pixel comparison verifies that everything outside that rectangle
is unchanged. The normal locale compiler preserves frame metadata and restores
the separately managed interface text.

The final assets are stored in `localization/templates/` and installed at the
`client/` paths listed in `branding.json`. No generated-image cache path is needed
to build or run the game. `verify_branding.py` checks installed checksums, rejects
wordmark text bindings, and verifies the native window title.

## Editing prompts

For the first chapter, the built-in edit removed the entire old game logo at
`x=563..778, y=345..541`, reconstructing the hillside and white cloud artwork.
The prompt required the original 4:3 composition, unchanged characters, buildings,
borders, chapter text and loading interface, with no replacement logo, badge,
lettering, watermark, blur patch or geometric fill.

For chapters 2 through 16, the prompt rectangle was `x=563, y=370, w=215,
h=196`. The village and arena rectangles match their manifest entries. Chapter
composites extend slightly beyond the prompt rectangle to blend into the right
edge; `branding.json` records the final edited bounds.

The remaining backgrounds used this prompt with those rectangles:

> Precisely edit this 800x600 game background. Remove the entire green Chinese
> wordmark badge and all its decorative strokes inside the specified rectangle.
> It replaced an unwanted game logo; the user wants no wordmark or logo at all.
> Naturally reconstruct the underlying scenery, matching the existing whimsical
> 3D game artwork, light, materials and perspective. Do not leave a blur patch,
> plain fill, geometrical smear, rectangle or any lettering. Keep the characters,
> all UI panels, central loading frame, chapter text, borders and everything
> outside that region unchanged. Exact 4:3 composition. Background restoration only.
