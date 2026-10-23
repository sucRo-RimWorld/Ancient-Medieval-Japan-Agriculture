# MO 1.6.2.2 cultivated-crop and connected-food audit — 2026-10-23

**Owner:** Agriculture (repository renamed from Grains; existing in-game name/packageId retained). **Evidence:** user-supplied actual MO Steam package ID `3219596926`, 1.6 source, `Defs/ThingDefs_Plants/Plants_Cultivated_Farm.xml` and 1.6 recipe/item/storage XML (not MO wiki/other versions). Exactly **17 named cultivated non-alchemy crop ThingDefs** have a `plant/harvestedThingDef`; four additional MO alchemy garden crops are excluded from this decision entirely. Search covers 1.6 MO `Defs/` and `Patches/` actual XML; external optional Mods' final loaded state, legacy saves, and 1.7 not certified.

**Interpret status precisely:** `REINTERPRET (partial)` = the approved MO fibre carrier is being adapted through Agriculture plus required-for-integration Japanization, with no claim of completed texture/biological accuracy. `EXCLUDE` = the present PR implements a **bounded** exclusion of normal sow/cooking/ordinary trade/known MO container entries while keeping all Defs; `EXCLUDED` = prior source excludes the same bounded routes. `KEEP` = do not remove current source; other quantitative/cooking choices not automatically approved. `HOLD` = no edit now. The end-state ideal of a fully MO-free Agriculture is not a prerequisite: existing useful processes stay. Species names are not enough to approve unnamed cultivars or modern cuisine.

| MO cultivated plant | Harvested Def | Status | Source-based rationale and connected flows |
|---|---|---|---|
| `DankPyon_Plant_Onions` | `DankPyon_RawOnions` | **EXCLUDE** | Bulb onion food cultivation in Japan belongs to Meiji; 6 remaining regular MO meal Bills and two old tomato-omelette Bills use it |
| `DankPyon_Plant_Lentils` | `DankPyon_RawLentils` | **HOLD** | MO lentil skill/growth is currently AMJ-owned; four Western-leaning food Bills and Japanese actual cultivation history need a joint decision |
| `DankPyon_Plant_Cabbages` | `DankPyon_RawCabbages` | **EXCLUDE** | MO description and plant indicate headed cabbage; food cabbage introduced late Edo/Meiji; only Hunter's Stew x4/x8 use it |
| `DankPyon_Plant_Garlic` | `DankPyon_RawGarlic` | **KEEP** | Japanese records from ancient texts, historically attested; culinary/medicinal context still to rebalance |
| `DankPyon_Plant_Mushrooms` | `RawFungus` | **HOLD** | Specific cultivation technique and fungus type undefined; fungal food exists but generic simple field cultivation not automatically validated |
| `DankPyon_Plant_Wheat` | `DankPyon_RawWheat` | **KEEP** | Historical grain and current Grains/MO flour/stone milling reuse; no change |
| `DankPyon_Plant_Flax` | `DankPyon_RawFlax` | **REINTERPRET (partial)** | Retain Def carrier for approved karamushi/ramie textile route: Agriculture sets PlantPurpose.Misc and raw-fibre category Textiles; Japanization maps crop/raw/linen labels and textile research, preserving MO clothChain ON/OFF. Plant appearance and lifecycle need separate visual/numerical acceptance; not removal or fully validated Japanese textile production. |
| `DankPyon_Plant_Sugarcane` | `DankPyon_Sugarcane` | **EXCLUDED** | AMJ mainland production excludes Ryukyu sugarcane cultivation; raw cane is removed from ordinary traders, and three grinding Bills are disabled. Finished `DankPyon_Sugar` remains a potentially imported good, with actual trader stock availability still OPEN. |
| `DankPyon_Plant_Carrots` | `DankPyon_RawCarrots` | **HOLD** | Eastern carrot entered Japan around 16th/17th century; MO visual variety and early/late-era gating unreviewed |
| `DankPyon_Plant_Herb` | `DankPyon_RawHerb` | **HOLD** | Generic culinary herb species unspecified; do not confuse with 4 separately frozen MO medicinal herbs |
| `DankPyon_Plant_Tomatoes` | `DankPyon_RawTomatoes` | **EXCLUDED** | Already excluded by Agriculture PR235; this batch also closes remaining tomato-omelette trader stock |
| `DankPyon_Plant_Grape` | `DankPyon_RawGrapes` | **HOLD** | Japanese local grapes vs imported cultivar and European wine press/fermentation are separate historical/production decisions |
| `DankPyon_Plant_Pumpkins` | `DankPyon_RawPumpkins` | **HOLD** | New World crop reached Japan around late Sengoku / early Edo; era gating and intended route need a source-based decision |
| `DankPyon_Tree_Apple` | `DankPyon_RawApples` | **HOLD** | Japanese historical small apple vs later Western orchard apple, cider, flour pie and storage require cultivar/industry audit |
| `DankPyon_Tree_Lemon` | `DankPyon_RawLemons` | **EXCLUDE** | Japan cultivated lemon from early Meiji; 3 lemon-cake Bills plus 3 already-excluded GriffonberryPie Bills |
| `DankPyon_Tree_Mulberry` | `DankPyon_RawMulberry` | **KEEP** | Mulberry and sericulture have ancient Japanese roles; fruit+wine side route not automatically accepted |
| `DankPyon_Tree_GriffonBerry` | `DankPyon_GriffonBerry` | **EXCLUDED** | Fictional crop chain already excluded by Agriculture PR236; research/fantasy animal handled by Japanization owner |

## Historical references and confidence boundaries

- Onion bulb, Meiji commercial cultivation: https://www.meiji.co.jp/foods/soup/marugotoyasai/dictionary/onion.html
- Cabbage, late Edo/Meiji for food: https://www.maff.go.jp/tohoku/monosiritai/syokutaku/kyabetu.html
- Lemon, Meiji first import (NDL reference bibliographies): https://crd.ndl.go.jp/reference/entry/index.php?id=1000184920&page=ref_view
- Garlic ancient texts and spices: https://www.maff.go.jp/j/pr/aff/2108/spe1_01.html
- Eastern carrot in 16th-17th Japan: https://vegetable.alic.go.jp/yasaijoho/senmon/2105_chosa3.html
- Flax fiber crop Meiji introduction: https://www.hkd.mlit.go.jp/ky/ns/nou_sin/jtfkjs00000082w4.html

**High-confidence excluded current batch:** MO bulb onions, headed cabbages, and lemon (not Japanese native citrus). Do **not** substitute Japanese `negi`, `na`/leafy brassicas, `yuzu` or other species silently by only relabeling the same western crop Def without independent plant traits/art/uses. The entire dish path is removed from ordinary Bills where its recipe requires removed inputs.

**Precise MO 1.6.2.2 reference audit (current batch):**
- Sowing: 3 `plant/sowTags` -> empty (keep Defs; do not change Vanilla/CCTO temperature or growth values).
- Ordinary trader stock: 3 raw edible ThingDefs + 4 discontinued cooked products -> `tradeability=None`. In addition, prior tomato omelette product `DankPyon_MealTomatoOmelette` is corrected in its **own** `Patches/MO_CropExclusion_Tomato.xml` to close the previously overlooked general-trader route.
- Cooking Bills: 9 original `RecipeDef/recipeUsers` replaced with empty lists: BoiledBeansAndOnion 2, HuntersStew 2, SteakWithWine 2, LemonCake 3. The earlier TomatoOmelette 2 and GriffonBerryPie 3 are **already disabled**; their source ingredient references remain intentionally for old-save and third-party Def stability. Changing core ingredients or inventing replacement recipes is not part of this narrow compatibility slice.
- Hideout loot: 4 actual `MedievalOverhaul.BuildingLootableExtension/randomItems` leaf entries, two onions and two cabbages, are removed from MO container generation. No lemon-specific hideout entries were present in this MO source audit.
- Total new crop-family patch: **21 operations** matching **23 upstream leaf/Thing/Recipe XPath targets** (three sow + three raw sale + nine recipe + four finished-meal sale + two multi-match loot operations). Existing MO and AMJ references stay resolvable, including saved objects; direct spawns from other Mods/scenarios, existing placed resources/ongoing Bills, special barter inventories and all recipe outputs from debug actions are **not** covered.
- MO alchemy crops and **the four MO medicinal herbs and their unique drug production are expressly frozen**: Mindwort, Poppy, Fleawort, Fly agaric; no patches or decision changes here.
- Upstream 1.6 `Plants_Cultivated_Farm.xml` also contains text/patch nodes besides those 17 ThingDefs; counts are of real named cultivated plants, not raw top-level XML nodes.

## Sugarcane and imported sugar — author decisions, 2026-10-23 and 2026-10-24

**Production owner:** Agriculture. The mainland colony cannot grow/manufacture sugarcane sugar; Ryukyu cultivation/refining belongs outside the AMJ mainland farm. Finished sugar is an imported luxury, distinct from rice syrup or ama-zura.

**Exclusion (PR #239):** `Patches/MO_CropExclusion_Sugarcane.xml` leaves original DefNames in place, but disables sowing `DankPyon_Plant_Sugarcane`, ordinary trade of raw `DankPyon_Sugarcane`, and the three original sugar grinding Bills (`DankPyon_CraftSugar_Manual`, `DankPyon_CraftSugar`, `DankPyon_CraftSugarBulk`). Sugar and all existing consumer recipes remain referenceable. Existing save objects, direct third-party spawns, and saved Bills are not erased.

**Imported sugar (PR #240, fixed by the 2026-10-24 approved follow-up):** In original MO 1.6.2.2 there was no explicit Sugar stock generator; `Patches/MO_SugarImportTrade.xml` connects `DankPyon_Sugar` to existing noble ArtPatron trader `DankPyon_Caravan_Medieval_ArtPatron`. Its first version contained no quantity and returned 0 items (see follow-up source audit below). **Author-approved correction** is to set the sugar `StockGenerator_MultiDef`'s **`countRange=3~6`**. Existing Chocolate generator is untouched. `Patches/MO_SugarImportPricing.xml` sets **Sugar `MarketValue=5.0`** (original `2.2`). These are approved *gameplay* values, not a historical currency conversion. The same trader's `commonality=1` remains unchanged and is not a guaranteed encounter rate; absence of a NobleHouse/ArtPatron trader can still prevent imports.

**Evidence boundary:** original 1.6.2.2 user-supplied MO XML: 5 crop/process targets, 1 merchant target, and 1 Sugar market-value target each uniquely match. Static application leaves DefNames, Sugar's food dependencies, Chocolate stock generator, and the other merchant generators unchanged. Original Sugar `thingCategories` is `DankPyon_Condiments`; tradeability and other ThingDef data are not edited. Static source verification is **NOT** native 1.6/1.7 merchant stock, loaded Def, old-save or trade-window acceptance. No optional-MO hard dependency, new merchant/faction, or Japanese syrup production was added. MO medicinal herbs and drug chains remain frozen.

## Follow-up source audit: imported sugar stock zero, price, and next crops — 2026-10-23

### Source defect discovered and corrected after PR #240

The original MO ArtPatron Chocolate `StockGenerator_MultiDef` had neither `countRange` nor `totalPriceRange`. The initial Sugar `MultiDef` from Agriculture PR #240 copied this shape. In RimWorld's `StockGenerator.RandomCountOf`, those zero defaults return **0 items**; `TryMakeForStockSingle` does not produce stock when the count is zero. This was a concrete production defect in the intermediate `main`, not just an unverified-game limitation.

The **2026-10-24 author-approved correction** is Sugar-only `countRange=3~6`. Existing Chocolate behavior is outside this amendment and unchanged. Cross-check source (third-party decompilation; target RimWorld 1.6 native profile remains to be verified):
- https://github.com/Dyyrlysh/RimworldDecompile/blob/79ca2aa15c9e116bd43d448692b62636467f6c46/RimWorld/StockGenerator.cs
- https://github.com/Dyyrlysh/RimworldDecompile/blob/79ca2aa15c9e116bd43d448692b62636467f6c46/RimWorld/StockGenerator_MultiDef.cs
- https://github.com/Dyyrlysh/RimworldDecompile/blob/79ca2aa15c9e116bd43d448692b62636467f6c46/RimWorld/StockGeneratorUtility.cs

### Historical scope and economic comparisons

Historical evidence supports mainland Japan's **imported luxury** sugar, initially a medicinal/gift item and increasingly a confection ingredient in medieval and late-Sengoku Japan. The Sugar `ThingDef` remains an imported food ingredient, but it must not be assumed common on day 1 or available at every trader. Sources:
- https://www.alic.go.jp/joho-s/joho07_000217.html — Zheng trade, medieval imported sweets and late-Sengoku Portuguese arrivals.
- https://www.alic.go.jp/koho/kikaku03_000078.html — import history and early-Edo Ryukyu local sugar processing, which is outside AMJ's mainland cultivation.
- https://www.seitokogyokai.com/knowledge/propagation/ — early elite/medicinal use and later commercial sweets.

Actual MO 1.6.2.2 unchanged per-unit `MarketValue`: Salt **1.0**; Flour **1.5**; Sugar **2.2**; Spices **2.5** (all `Items_Resource_Ingredients.xml`). Sugar's sale value is currently below MO spices, which conflicts with its newly exclusive imported-luxury role unless other supply constraints create scarcity. These are *in-game currency values, not historical monetary conversions*. The currently active sugar-consuming original MO meal families are Apple Pie ×3 and Sweet Pancakes ×2; Lemon Cake ×3 and Griffon Berry Pie ×3 were already disconnected by Agriculture. Sugar's `Nutrition=0.02`, and nutrition-based recipe ingredient counts are 0.04 for one Apple Pie / 0.05 for one Sweet Pancake, theoretically corresponding to ~2 / ~3 sugar units; 5x recipes require 0.20 / 0.25 nutrition (about 10 / 13 sugar). The Apple Pie/Sweet Pancake cultural suitability is **separately HOLD** rather than approval of their new local source or imported ingredients.

### Author-approved 3–6-stock / market-value-5.0 model — bounded acceptance assumptions

**Author decision (2026-10-24):** 3–6 finished sugar per ArtPatron stock generation and Sugar base `MarketValue=5.0`; this replaces the prior unapproved draft. Both values remain subject to actual game economy inspection, but do **not** require additional author approval merely to commit the already-approved coefficients. Base MO comparisons remain Salt 1.0, Flour 1.5, Spices 2.5, Sugar *previously* 2.2. Game values are not historical coin exchange rates.

| Scenario | Intended player outcome | Predicted with approved values | Evidence limitation |
|---|---|---|---|
| Typical one ArtPatron visit | A small luxury purchase enables modest confectionery, not everyday staples | 3–6 sugar at pre-trade base value 15–30; one apple pie (~2 sugar) or sweet pancake (~3) | Trade adjustment / generated stock counts await native validation |
| Edge: no MO NobleHouse/ArtPatron trader | Never force unsupported trader encounters or foreign factions | No sugar from this specific route | Alternative imports remain outside the approved change |
| Three eligible trader encounters | Imports remain episodic rather than equivalent to a crop | 9–18 sugar before any consumption (arithmetic only) | Frequency of merchant encounters unknown |
| Price differentiation | Finished imported sugar costlier than local salt/spices | 5.0 vs salt1.0, spices2.5; estimated pie's sugar base value ~10 | Final buying/selling modifiers and overall economics not verified |
| Five-serving bulk Bill | Accumulating trade inputs is meaningful | 5 apple pies require ~10 sugar, larger than one 3–6-stock shipment | Runtime RecipeDef ingredient counting still unverified |

**Implementation status:** numeric values are **AUTHOR-APPROVED, source-patched**. The above scenarios are prospective behavioral expectations, **not native or empirical economic-balance PASS**. Actual merchant generation, recipes, availability after Factions/Japanization redesign, 1.6/1.7 and existing saves remain OPEN.

### Other MO crop interactions reviewed from the user-provided 1.6.2.2 XML

- **Mushrooms — cultivation HOLD, connected recipe audit complete:** `DankPyon_Plant_Mushrooms` is a dark-required `CavePlantBase` with `diesToLight=true`, Ground/Hydroponics sowTags, BasicAgriculture research, growDays3, harvestYield6, sowMinSkill3. It harvests *Vanilla-referenced* `RawFungus`; `DankPyon_CookMealMushroomSoup` and its Bulk version directly require `RawFungus`. No explicit wild-spawn source for the *MO mushroom plant* is present in the inspected 1.6 MO XML. **Do not just empty sowTags:** otherwise the two soup Bills may be inaccessible in a non-Ideology profile unless another `RawFungus` acquisition source is established. Dark-cave planting is not automatically a historically documented medieval mushroom-farming technique. Verify the actual loaded non-Ideology `RawFungus` supply/harvest before any exclusion, rehoming or recipe edit. Historical source for later Japanese shiitake cultivation: https://kotobank.jp/word/%E3%81%97%E3%81%84%E3%81%9F%E3%81%91-3153759.
- **Grapes — retain crop for now; wine pathway HOLD:** `DankPyon_Plant_Grape` requires `DankPyon_VinePost` sowTag, AdvancedAgriculture, growDays19.5, sowMinSkill5, and harvestYield12. `DankPyon_MakeMustWine` turns 25 fruit into 5 `DankPyon_MustWine` at Brewery; associated barrel ProcessDefs turn must into wine. The fruit/food cultivation decision must be separate from European winemaking.
- **Mulberry — KEEP for silkworm function; wine pathway HOLD:** `DankPyon_Tree_Mulberry` has `MedievalOverhaul.Plant_SecondaryDrop`, producing `DankPyon_Silkworm` alongside fruit. Original `sowResearchPrerequisites` is `Cocoa` (historically unrelated). `DankPyon_MakeMustMulberry` uses 25 berries to yield 5 `DankPyon_MustWine_Mulberry`; disabling this western-style wine loop would not justify disabling mulberry/silkworm cultivation. Research ownership and silk-source consequences need MO Japanization design review before edits.
- **Lentils — HOLD:** `DankPyon_CookMealWinterLentilSoup` ×2 still active; `BoiledBeansAndOnion` ×2 already disabled with onions. Existing Agriculture lentil growDays8/sowMinSkill2 remain approved historical implementation values, not permission to prune without reconciling food Bills and crop-balance statements.

All four MO medicinal herb/medicine production chains remain outside this review; no native runtime tests or source changes to other crops were performed.


## Next work requiring a separate impact audit

1. Fibre production: the previously approved MO `DankPyon_Plant_Flax`→苧麻/カラムシ and `DankPyon_Linen`→麻布 reinterpretation now has bounded source patches in Agriculture/Japanization. Retain plant and cloth Def identity; retexture/plant regrowth and actual MO `clothChain` ON/OFF gameplay remain unresolved, and no additional independent textile Mod is forced.
2. Lentils and their four food Bills: imported grain-crop status not resolved, plus existing Agriculture-owned MO lentil growDays/sowMinSkill2; don't remove from sow menu prematurely.
3. ArtPatron sugar supply's actual generated stock, merchant availability and historical price/rarity, plus apple/wine, grape/wine, pumpkin, carrot cultivar, generic herb and fungus cultivation: review independently. Sugarcane sowing/raw trade/grinding remain source-excluded. A supported Sugar trader-generator source exists but actual loaded trade and total economy remain unverified. Vanilla corn/potatoes/hops and other Vanilla crops need a **separate** period-fit audit with their own Def/research/food paths; do not misrepresent a MO-only count as global completion.
4. The 1.6 original game final patched Defs, farm menu, kitchen Bills, generated merchant stock, MO hideout loot, and old save are unverified. Run smallest consolidated manual test only after the bounded source implementation ends, not on every PR.

