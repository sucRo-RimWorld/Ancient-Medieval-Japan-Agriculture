# MO 1.6.2.2 cultivated-crop and connected-food audit — 2026-10-23

**Owner:** Agriculture (repository renamed from Grains; existing in-game name/packageId retained). **Evidence:** user-supplied actual MO Steam package ID `3219596926`, 1.6 source, `Defs/ThingDefs_Plants/Plants_Cultivated_Farm.xml` and 1.6 recipe/item/storage XML (not MO wiki/other versions). Exactly **17 named cultivated non-alchemy crop ThingDefs** have a `plant/harvestedThingDef`; four additional MO alchemy garden crops are excluded from this decision entirely. Search covers 1.6 MO `Defs/` and `Patches/` actual XML; external optional Mods' final loaded state, legacy saves, and 1.7 not certified.

**Interpret status precisely:** `EXCLUDE` = the present PR implements a **bounded** exclusion of normal sow/cooking/ordinary trade/known MO container entries while keeping all Defs; `EXCLUDED` = prior source excludes the same bounded routes. `KEEP` = do not remove current source; other quantitative/cooking choices not automatically approved. `HOLD` = no edit now. The end-state ideal of a fully MO-free Agriculture is not a prerequisite: existing useful processes stay. Species names are not enough to approve unnamed cultivars or modern cuisine.

| MO cultivated plant | Harvested Def | Status | Source-based rationale and connected flows |
|---|---|---|---|
| `DankPyon_Plant_Onions` | `DankPyon_RawOnions` | **EXCLUDE** | Bulb onion food cultivation in Japan belongs to Meiji; 6 remaining regular MO meal Bills and two old tomato-omelette Bills use it |
| `DankPyon_Plant_Lentils` | `DankPyon_RawLentils` | **HOLD** | MO lentil skill/growth is currently AMJ-owned; four Western-leaning food Bills and Japanese actual cultivation history need a joint decision |
| `DankPyon_Plant_Cabbages` | `DankPyon_RawCabbages` | **EXCLUDE** | MO description and plant indicate headed cabbage; food cabbage introduced late Edo/Meiji; only Hunter's Stew x4/x8 use it |
| `DankPyon_Plant_Garlic` | `DankPyon_RawGarlic` | **KEEP** | Japanese records from ancient texts, historically attested; culinary/medicinal context still to rebalance |
| `DankPyon_Plant_Mushrooms` | `RawFungus` | **HOLD** | Specific cultivation technique and fungus type undefined; fungal food exists but generic simple field cultivation not automatically validated |
| `DankPyon_Plant_Wheat` | `DankPyon_RawWheat` | **KEEP** | Historical grain and current Grains/MO flour/stone milling reuse; no change |
| `DankPyon_Plant_Flax` | `DankPyon_RawFlax` | **HOLD** | Modern Japan flax-fiber crop dated Meiji; upstream linen spinning and loom/stored fiber must be rerouted with Clothing/textiles before farm is disabled |
| `DankPyon_Plant_Sugarcane` | `DankPyon_Sugarcane` | **HOLD** | Ryukyu/island sugar/cultivation and sugar grinding must be separated by geographic/era and owner supply routes |
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

## Next work requiring a separate impact audit

1. Flax and linen/fiber chain: MO `DankPyon_RawFlax` feeds `DankPyon_SpinFlax` and two `DankPyon_MakeLinen` Bills, plus flax storage containers. Do not delete before alternative hemp/ramie input and MO/Culture textile responsibility are approved. A Japanese fiber is not mechanically equivalent to European linen by label change.
2. Lentils and their four food Bills: imported grain-crop status not resolved, plus existing Agriculture-owned MO lentil growDays/sowMinSkill2; don't remove from sow menu prematurely.
3. Sugarcane, apple/wine, grape/wine, pumpkin, carrot cultivar, generic herb, fungus cultivation: separate historical era and ordinary-food or industrial supply contracts from raw crop availability; keep as provisionally loaded while review proceeds. Vanilla corn/potatoes/hops and other Vanilla crops need a **separate** period-fit audit with their own Def/research/food paths; do not misrepresent a MO-only count as global completion.
4. The 1.6 original game final patched Defs, farm menu, kitchen Bills, generated merchant stock, MO hideout loot, and old save are unverified. Run smallest consolidated manual test only after the bounded source implementation ends, not on every PR.

