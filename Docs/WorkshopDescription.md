# Grains — Steam Workshop description

The single authoritative paste-ready description is `Docs/SteamWorkshopDescription.txt`. Paste **its entire English-then-Japanese body into Steam's English description field only**; do not paste it a second time into Japanese. `Docs/SteamWorkshopDescription-ja.txt` is the Japanese drafting source. No image is reused separately per language; any future images must appear exactly once with English/Japanese captions. Keep the full combined body under 8,000 UTF-8 bytes, omit license/AI/donation disclosures, preserve dependencies and current development-status warnings. This repository draft does not mean the Mod is ready to publish.

Title and About name: `Ancient & Medieval Japan - Grains（中世日本 - 穀類）`. The current `packageId` remains `sucro.ancientmedievaljapan.core`. New Village belongs to the separate Scenarios Mod. Base wheat/processing and MO conditional integration are included; exact-current-runtime and final-art release gates remain independently open; Grains has no preexisting deployed saves.

## Approved Grains V1 food comparison image

- Immutable approved 1672×941 PNG master: `Art/Sources/Workshop/GrainsV1FoodComparison_EN-JA.png`.
- Byte-identical web-facing image: `Docs/Media/GrainsV1FoodComparison_EN-JA.png`. Both Git blobs must equal `18957f3c0d7f4283ab6b3cafd8a50891087db466`; source SHA-256 `6f68e2ff9206b4c904861af4e13acb2a151ef48c7a842a092cb2f50a266959c3`.
- The combined Workshop draft contains one `[img]https://raw.githubusercontent.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/main/Docs/Media/GrainsV1FoodComparison_EN-JA.png[/img]` after both language sections. Do not repeat the comparison rows in text or show a second copy of the image. Captions are bilingual within the image.
- Author-controlled Steam publication is separate: GitHub file presence, draft BBCode and CI do not establish that the live Workshop page has been edited or rendered correctly. Confirm rendering after manual update. Neither Art nor Docs enters the subscriber's in-game Workshop payload.

## Language divider and optional MO ordering

Place one Steam `[hr][/hr]` after the complete English description and before the Japanese section, and a second `[hr][/hr]` before the single shared bilingual comparison image placed after the Japanese section. The Production `About.xml` has **no hard MO dependency** and retains MO and CCTO in optional `loadAfter`; `loadFolders.xml` selects MO compatibility only when present. This metadata decision does not certify final graphics or completed fresh-start public-release testing. Historical-save/provider-removal tests are out of scope because no Grains saves exist. Preserve the stable packageId.
