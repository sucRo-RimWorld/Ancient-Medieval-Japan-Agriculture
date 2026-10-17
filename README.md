# Ancient & Medieval Japan - Grains（中世日本 - 穀類）

**RimWorld 1.6 — Development build**

A standalone grain-growing and food-processing Mod inspired by agriculture in pre-Edo Japan. Formerly AMJ Core; the historical `sucro.ancientmedievaljapan.core` packageId and existing AMJC DefNames remain unchanged to retain stable identifiers for cross-Mod compatibility.

## Features

- Awa, Hie and Kibi, with different growing seasons, soil fertility requirements and temperature preferences.
- Soba, barley, wheat and upland rice (using Vanilla `Plant_Rice`).
- Harvested sheaves, threshing and hulling, grain milling, and basic flour foods.
- Vanilla-only wheat, wheat flour and manual millstone when Medieval Overhaul is absent.
- MO wheat/RawWheat/flour/millstone reuse and thresh-time Straw integration when MO is present, without duplicate default wheat or milling routes. AMJG-owned balance and processing settings take priority.

The balancing model and crops are specified in [Design](Docs/Design.md) and [cultivation balance](Docs/Balance/Crops/Millet_Cultivation_Balance.md).

## V1 food options

The same harvested grains can be used in mixed meals or processed into flour. Mixed dishes remain available for combining leftover cereals even after ingredient-specific recipes are expanded; recipes requiring a particular flour have a small advantage.

| Dish | Accepted ingredients | Cooking work (1 serving) | Room-temperature spoilage | Mood on eating |
|---|---|---:|---:|---|
| Mixed grain meal | 5 edible grain types (mixing allowed) | 240 | 3 days | No mood bonus |
| Mixed grain porridge | 5 edible grain types (mixing allowed) | 200 | 1.5 days | No mood bonus |
| Mixed flour food | 5 cereal flours (mixing allowed) | 300 | 2 days | No mood bonus |
| 5 ingredient-specific flour dishes | Each recipe's designated flour | 300 | 2.5 days | Mood +2 after eating |

All eight dishes consume 0.5 ingredient nutrition to make one meal worth 0.9 nutrition. Each also has a four-serving cooking Bill, with four times the inputs and work. The five specific flour dishes are *hakutaku (houtou), sobagaki, millet dumplings, barley-flour gruel and rice-flour dumplings*. Grains does not automatically prioritize ingredients by time remaining before spoilage; players can use Bill ingredient filters to select available leftovers.

These values are implemented in the current Defs. Four-profile loaded-game validation of actual mixed-ingredient cooking Bills is still outstanding; this development build is not a publicly verified release.

## Dependencies and compatibility

**Required:** RimWorld 1.6. **No mandatory additional Mods or DLC.**

**Strongly recommended (optional):** [Adaptive Primitive Storage](https://steamcommunity.com/sharedfiles/filedetails/?id=3400037215). Its existing granary can store Grains' grain sheaves, kernels and flours, including MO wheat. When both MO and Adaptive Primitive Storage are active, Grains' compatibility patch replaces Hay with MO Straw as a construction ingredient for the granary and woven baskets.

**Other optional integrations:** Medieval Overhaul (MO) and [Crop Cold Tolerance Overhaul](https://steamcommunity.com/sharedfiles/filedetails/?id=3812412548) (CCTO).

The `loadAfter` entry for MO is an **optional ordering hint, not a hard dependency**. MO-specific definitions and patches load only when MO is active through `loadFolders.xml`; `BaseWithoutMO` supplies the fallback path otherwise. CCTO adds explicit cold-death support when installed.

New Village and its starting definitions are provided exclusively by the separate AMJ Scenarios Mod. Grains does not include a scenario or legacy scenario compatibility copy.

## Development status

The author confirmed on 2026-10-10 that Grains has not been installed or used. **There are no existing Grains saves to migrate.** Historical save migration, enabling/disabling mid-save and removing MO from old saves are **out of scope**, not release blockers.

The mandatory MO dependency has been removed. Previous fresh-start Vanilla / MO × CCTO tests passed on their tested revisions; current-revision game loading and native Bills, final graphics, UI/language and gameplay balance remain to be verified. This development build has not been publicly released.

## Planned improvements

- Refine Grains' sheaf, grain and flour item graphics with thicker outer outlines so they remain legible at normal game zoom and visually consistent alongside Vanilla and Medieval Overhaul items. This is a future art update; existing accepted images remain unchanged for now.

## Documentation

- [Design and ownership](Docs/Design.md)
- [MO dependency audit](Docs/GrainsDependencyAudit.md)
- [Four-profile testing and open runtime gates](Docs/GrainsProfileTesting.md)
- [Crop cold tolerance](Docs/Balance/Crops/ColdTolerance.md)
- [Development tools](Docs/DevelopmentTools.md)
