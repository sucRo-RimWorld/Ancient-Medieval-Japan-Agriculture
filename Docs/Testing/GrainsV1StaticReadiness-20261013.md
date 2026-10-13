# Grains V1 six-step source audit / 一括実機前の静的監査

**Date:** 2026-10-13. **Repository:** `Ancient-Medieval-Japan-Grains`. **Scope:** present source and GitHub tree; production runtime and private Steam installation are not asserted by any result in this document. Author decision: defer the actual real-game launch until a consolidated native test, do not modify approved crop/sheaf graphics, and do not publish.

## 1. Four-profile real common-food Bill test implementation

**Existing evidence:** A previous-source RimWorld 1.7 four-profile run passed six scenarios in each profile (24/24, no ERROR), but the prior sixth scenario tested named flour dishes and Vanilla Simple meals, not genuinely mixed 2-source common-food Bills. See `Docs/GrainsProfileTesting.md` and `Testing/Native17-20261013.json`.

**New C# source:** `GrainsSimulationSteps.CommonFoodJobs` introduces a separate seventh `@quickstart:AmjStageAQuickstart @timeout:270` feature scenario per profile (Vanilla, Vanilla+CCTO, MO, MO+CCTO). It runs **3 foods × 2 batch sizes × 2 material modes = 12 real `WorkGiver_DoBill` jobs per profile**, for 48 job attempts under the complete four-profile native matrix. Accepted conditions:

| Food & DefName | Batch | Work | Ingredient Nutrition | Ingredient mode | Output |
|---|---|---:|---:|---|---:|
| 飯 `AMJC_MixedGrainMeal` | 1 / 4 | 240 / 960 | 0.5 / 2.0 | millet alone, or millet + barley | 1 / 4 |
| 粥 `AMJC_MixedGrainPorridge` | 1 / 4 | 200 / 800 | 0.5 / 2.0 | millet alone, or millet + barley | 1 / 4 |
| 粉食 `AMJC_MixedFlourFood` | 1 / 4 | 300 / 1200 | 0.5 / 2.0 | millet flour alone, or millet flour + Base AMJC/MO DankPyon wheat flour | 1 / 4 |

Every test uses actual loaded ThingDefs, checks each staged input Nutrition=0.05, preconditions on accepted recipe filters and WorkGivers, real bill completion, exactly expected product counts, and consumption of the entire allowed staged amount of each material. **This new scenario creates disposable input stacks with `ThingMaker`; it does not claim these were harvested.** The existing sixth scenario still tests the native actual harvest→thresh/hull→mill→cook chain. Separate maps are cleaned up on error. Loaded JP/EN Info panel and real ingestion/Thought/graphic visual approval are still outstanding and are not covered by the source wiring alone. The previous 24/24 must not be reused as a new 28/28 PASS.

**Static coverage:** `Tests/Production/test_grains_mixed_meals.py` guards each profile's seventh scenario, the three foods, batch/mixing combinations, Native DoBill job path, exact stock/product tests, cleanup, and Base/MO flour switch. A CI PASS verifies source wiring only, not C# compilation or actual game performance. The bounded 240s internal fixture deadline is unchanged; timed out jobs fail rather than weakening the limit.

## 2. Vanilla/Grains meal balance (confirmed XML, unchanged)

Vanilla comparison *item names*: **簡単な食事 (Mood 0)** → Grains **飯・粥・粉食 (Mood 0)** → Grains **5 flour-specific dishes (Mood +2)** → Vanilla **手の込んだ食事 (+5)** → **豪華な食事 (+12)**. +2 is not a new food tier named 「美味しい食事」 and not a Vanilla Fine meal.

All eight foods consume **0.5** ingredient Nutrition per serving and yield **0.9** food Nutrition; four-serving Bills consume **2.0**, yield 4, and require four times the individual cooking Work. Common grain meal Work240, 3 days rot; porridge Work200, 1.5 days rot; common flour dish Work300, 2 days rot. Each specific flour dish Work300, 2.5 days rot, taste Mood+2 for 0.5 days (nonstacking). Common meals MarketValue10 and specific meals MarketValue12. Therefore the specific foods are only +0.5 rot days / +2 taste / +2 value versus common flour, with a single-flour requirement and the same cooking Work. Milling adds Work to both kinds of flour meal. **No balance numbers were modified.** Canonical balance table: `Docs/Balance/Crops/SevenGrainFoodChainAudit.md §3`. Total harvest/labor feel, spoilage logistics and true meal selection remain unverified.

## 3. Threshing, hulling, milling, and workstations (source-consistent)

- **Common thresh/hull:** four common families (millet, buckwheat, barley, rice) × 2 stages × x1/x10 = **16 RecipeDefs**. Per single batch thresh 15 Work, hull 10 Work; ten-at-once thresh 120, hull 80, preserving 1:1 item output. Base-only wheat has x1/x10 threshing (15/120). The owner WorkGiver binds two AMJ processing stations; conditional Vanilla fueled/electric stove processing remains enabled for Base and MO, with the production code under `Defs/RecipeDefs/Recipes_GrainProcessing.xml`, `BaseWithoutMO/Defs/Recipes_Wheat.xml` and relevant compatibility stoves patches.
- **Milling Base:** five recipes, input 10→powder 10, Work300; all list `AMJC_ManualMillstone` (player label 「石臼」), `FueledStove`, `ElectricStove`. The underlying ID must remain unchanged.
- **Milling MO:** the four AMJ-owned powder recipes have recipeUsers changed to `DankPyon_Millstone` only; MO wheat/flour comes from MO-native Recipes, with bulk Work800 and no Grain-owned change to MO research/throughput. Flour-food (both dedicated hakutaku and shared mixed flour) swaps wheat-flour input to `DankPyon_Flour`; no duplicate Base wheat mill is enabled.
- **Remaining loaded-game checks:** actual stove-processing `DoBill` for each selected equipment, MO WorkGiver usage, optional CCTO with exact provider labels, no Hay leakage, full 1.6 provider and source-hash matrix. Static conditional XML proves intended routing but not actual engine job availability.

## 4. Japanese/English proofreading

The present Japanese DefInjected inventory contains **78 localized Def keys** across **13 Japanese XML files** with **no duplicate `label`/`description`/`jobString` keys** in that inventory. A focused audit of 37 player-facing crop/processing/meal/building ThingDefs found a Japanese label + description for 36; **Vanilla `RawRice` intentionally retains its existing Japanese label「米」** and Grains patches only the `RawRice.description`, so that missing local override is not an error. All **8 prepared foods have one JA label+description** each; all **16 food Bills have one JA label+description+jobString** each. Existing approved long explanations and the shared three food names are preserved.

**Correction implemented:** The ThingDef named `AMJC_Houtou` is the approved Japanese 「餺飥（はくたく）」, and its description explicitly distinguishes the early historical usage from later hoto. The previous English label `houtou` and one-serving recipe label `cook houtou` contradicted existing English descriptions and bulk Bill labels referring to `hakutaku`. Player-visible English strings are now **`hakutaku`** and **`cook hakutaku`**, in agreement with the existing `cook four hakutaku`; DefNames, Japanese labels, gameplay and citations remain untouched. The invariant is in `Tests/Production/test_grains_localization.py`; canonical approved source in `Docs/LocalizationHistoricalReview.md §17`. Current-game localization loading, long-info text wrapping and historical proper-name decisions for future *new* recipes still require distinct approval/evidence.

## 5. Textures and references (no art modification)

Source tree contains exactly **33 production PNG files** under `Textures/`: 7 immature crops, 7 mature crops, 5 harvested sheaves, 9 hulled/edible grain states, and 5 flours. Current AMJ-owned `texPath` prefixes in production ThingDefs have a tracked PNG target; **no missing owned image path detected in this static inspection**. `Graphic_StackCount` families for the grain/flour/sheaf states intentionally have many `_a`-only textures and are guarded by the existing source/stack graphics validators. All eight cooked-food ThingDefs currently **reuse existing grain PNGs as temporary placeholders**; none has its own finished dish graphic. The three AMJ grain-processing/millstone buildings use the **Vanilla stonecutter-table graphic**; local textures are not yet designed. Visual appropriateness is therefore **OPEN even if no BadTex is detected**. The 14 crop-state and five sheaf images (and new sheaf-count variants) are author-deferred: retain exact previously approved image bytes without retouching them. Only separate future approved art work may replace placeholder food/building paths.

## 6. Workshop subscriber payload and release readiness

The tracked `main` source inventory at this audit had **462 blob paths**. Applying `.rimignore` case-insensitive basename rules yielded **93 subscriber-oriented paths** (About 4; BaseWithoutMO 12; Compatibility 9; Defs 10; Languages 9; LegacyStartingScenarios 9; Patches 5; Textures 33; `LICENSE` 1; `loadFolders.xml` 1) and excluded **369**. None of `Art/`, `Docs/` or `Tests/` leaked through the static inclusion check. Exact live packaged bytes, actual YADA output, Steam publisher behavior and re-downloaded Workshop payload are **not** verified by this count.

`About/About.xml` and `About/Manifest.xml` both use **0.1.0-dev**; Medieval Overhaul and CCTO remain optional. Versioned `About/Changelog.txt`, conditional `loadFolders.xml` and local/future MO providers remain included. The author-approved bilingual food-comparison source `Art/Sources/Workshop/GrainsV1FoodComparison_EN-JA.png` and display `Docs/Media/GrainsV1FoodComparison_EN-JA.png` have identical Git blob `18957f3c0d7f4283ab6b3cafd8a50891087db466`, and Workshop English+Japanese BBCode references the image **once**; neither image enters the subscriber's runtime Texture folder. The actual private upload and eventual publication are author-owned and **not performed**. `Tests/Release/validate_workshop_payload.py`, `Tests/Release/validate_add_changenote.py`, and Stage A CI remain mandatory static package gates.

## Gates still open / no PASS inferred

1. C# **actual native compile** and fresh exact-source **four-profile × seven Pickle scenarios** on an appropriate 1.6 game: all 28/28, no skips/flakes, ERROR 0, real two-grain/two-flour Bill completion and job budgets.
2. Runner host service/label/online and true remote provider staging. The earlier 1.6 Vanilla process/log smoke of AMJRuntime is not a Grains E2E PASS.
3. Current-source 1.6/1.7 loaded JP+EN text, RawRice, native spoilage/taste/ingestion checks, vanilla vs MO stove/equipment jobs, normal-zoom artwork and visual judgment.
4. Private play feel and current Workshop subscriber/publisher verification. No public release approval.

The report reflects source static analysis. The new C# tests themselves are **implemented but not run**; CI verifies Python/PowerShell/asset/packaging contracts, not actual Unity execution.
