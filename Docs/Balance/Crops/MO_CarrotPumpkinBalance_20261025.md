# MO Carrot and Pumpkin: cultivar and gameplay-balance decision — 2026-10-25

**Owner:** Agriculture. **Status:** historic type and gameplay role selected for the AMJ pre-Edo crop roster; **existing numbers retained for source implementation**, with storage realism/late-period presentation/numerical native acceptance explicitly OPEN. The user's 2026-10-25 continuation is authorization to progress this design review; it is **not** permission to invent unreviewed coefficients. No new rimworld XML, images or research gate are introduced in this decision.

## 1. Source and Japanese historic type

- MO original user-supplied Steam Workshop `3219596926.zip`, exact `3219596926/1.6/Defs/ThingDefs_Plants/Plants_Cultivated_Farm.xml` and `1.6/Defs/ThingDefs_Items/Items_Resource_RawPlant.xml`; Agriculture `Compatibility/MedievalOverhaul/Patches/MO_ExistingCropGrowthBalance.xml` and `loadFolders.xml` (only when MO is loaded). Japanese MO translation source Workshop 2665554648, `1.6/Common/Languages/Japanese/DefInjected/ThingDef/Plants_Cultivated_Farm.xml`.
- **Pumpkin: Japanese pumpkin, `Cucurbita moschata`, is the admitted historical target.** It reached Japan in the 16th century, making it a **late Muromachi/Sengoku** introduction; this does not mean it was cultivated in every earlier region or era. This is not Western `Cucurbita maxima`/modern sweet-floury pumpkin. MO's English description and existing Japanese translation describe a giant bright-orange fruit, so **the upstream presentation has not yet been certified as the Japanese type**. Graphics remain deliberately untouched under the author's image-skip instruction.
- **Carrot: Eastern/Asian carrots, `Daucus carota` Eastern group, are the only potential historical target.** Primary public references place introduction around late 16th–early 17th century, with the MAFF children's overview using early Edo. The crop is **late-Sengoku-boundary, not an ordinary ancient/medieval Japanese staple**. No identification with a specific named modern cultivar (e.g. today's Kintoki form), no claim that the original orange taproot asset depicts Eastern carrots. Ordinary availability currently remains as MO defined; the advanced historical era-gating mechanic has **not** been approved or implemented.
- Gameplay treatment: preserve both upstream plant and raw-food `DefName` and allow existing later-stage sow research to distinguish them from established ancient grains. This is **not** a dated unlock or historically precise distribution rule: MO Intermediate/AdvancedAgriculture are game research tiers, not calendar centuries. If exact era availability becomes necessary, review it jointly with the overall historical-era system, never with an invented special crop exception.

**Sources:** MAFF on carrots (early Edo Eastern type) https://www.maff.go.jp/j/kids/agri/roots/region03.html ; ALIC carrot history (late 16th to 17th) https://www.alic.go.jp/content/000084679.pdf ; NDL reference bibliography (16th-century account) https://crd.ndl.go.jp/reference/entry/reference/show?id=1000307854 ; MAFF pumpkin history https://www.maff.go.jp/j/kids/agri/roots/region06.html ; ALIC 2026-10 pumpkin https://alic.go.jp/koho/kikaku03_001814.html ; MAFF pumpkin cultivar comparison https://www.maff.go.jp/j/heya/kodomo_sodan/0304/02.html .

## 2. Actual current source values (MO 1.6.2.2 with Agriculture)

| Property | Eastern carrot candidate | Japanese pumpkin candidate |
|---|---:|---:|
| MO DefName | `DankPyon_Plant_Carrots` | `DankPyon_Plant_Pumpkins` |
| Raw harvest | `DankPyon_RawCarrots` | `DankPyon_RawPumpkins` |
| MO original growDays | 3.25 | 8 |
| **Effective Agriculture+MO growDays** | **6** (existing former CGB override) | **8** |
| Harvest units per crop cell | 6 | 20 |
| Raw nutrition per unit | 0.05 | 0.05 |
| Plant raw nutrition/full mature harvest | 0.30 | 1.00 |
| Sowing Plants skill | 4 | 8 |
| Research | IntermediateAgriculture | AdvancedAgriculture |
| Fertility minimum (locally explicit) | **not stated** (old 0.6 is inside an XML comment, NOT active) | 0.7 |
| Fertility sensitivity | 0.6 | 0.9 |
| Sow tags | Ground, Hydroponics (upstream, NOT historical endorsement) | Ground |
| Harvest raw rot start | 20 RimWorld days | 15 RimWorld days |
| Raw market value per item | 1.1 | 1.1 |

Note: Pumpkin's `vacuumResistant` XML and carrot Hydroponics support are upstream game abstractions. Neither establishes a historical grow method. Don't invent actual field-crop precision or delete an otherwise referenced legacy field without a separate compatibility decision.

## 3. Independent bounded scenario comparison (NOT real-game acceptance)

**Predeclared expected player decisions:** (A) in short/less fertile growing opportunity, an earlier/less-demanding crop should remain available; (B) in sustained good conditions a high-tier pumpkin should justify advanced research, Plants8, and fertile ground via high food yield and fewer harvesting actions; (C) storage and ordinary trade should not create an effortless year-round food stock without crop labor; (D) no crop from late Sengoku should masquerade as an early ancient commonplace.

**Source-specified model, not self-validating values:** `fertilityFactor = 1 + sensitivity × (fertility - 1)`, `fullCycles = floor(effectiveGrowingUnits × fertilityFactor / growDays)`. Each cycle assumes full growth, undamaged harvest, grower/difficulty factor1, prompt reseed/harvest without travel, shared ideal light/temperature, and no crop death, blight or processing. `effectiveGrowingUnits` are **not calendar days**; 5/10 are short/edge, 20 is representative steady opportunity, and 30 is long/multi-cycle. Direct raw nutrition = cycles × raw output × 0.05. No harvested-while-unripe 65% cases, seasonal temperature, CCTO or storyteller-specific setting are proven.

| Soil fertility | Effective growth units | Carrot cycles → raw nutrition | Pumpkin cycles → raw nutrition |
|---|---:|---:|---:|
| 70% | 5 | 0 → 0 | 0 → 0 |
| 70% | 10 | 1 → 0.30 | 0 → 0 |
| 70% | 20 | 2 → 0.60 | 1 → 1.00 |
| 70% | 30 | 4 → 1.20 | 2 → 2.00 |
| 100% | 5 | 0 → 0 | 0 → 0 |
| 100% | 10 | 1 → 0.30 | 1 → 1.00 |
| 100% | 20 | 3 → 0.90 | 2 → 2.00 |
| 100% | 30 | 5 → 1.50 | 3 → 3.00 |
| 140% | 5 | 1 → 0.30 | 0 → 0 |
| 140% | 10 | 2 → 0.60 | 1 → 1.00 |
| 140% | 20 | 4 → 1.20 | 3 → 3.00 |
| 140% | 30 | 6 → 1.80 | 5 → 5.00 |

At 70%/10 units the carrot finishes once while pumpkin finishes zero times. At 100%/20, carrot needs three sow+harvest cycles to yield 0.90 raw nutrition; pumpkin needs two cycles for 2.00. At 140%/30, pumpkin yields 5.00 raw nutrition in five cycles versus carrot 1.80 in six. Pumpkin also needs Plants8, AdvancedAgriculture and at least 70% soil, versus carrot Plants4 and IntermediateAgriculture. **Different tradeoffs are established in this limited model; neither “pumpkin always wins” nor “carrot should be deleted” is supported.** Carrot's possible Thin Soil 50% sowing requires final inherited `fertilityMin` verification: do not present an unverified minimum as a tested fact.

**Economy, labor and storage countercheck:** Raw produce both have MarketValue1.1 per item, so at one full harvest the undiscounted nominal values are carrot 6.6 versus pumpkin22 (not net income or actual merchant offer). One harvest+one sow job per completed cycle is only a labor **count proxy**, not pawn ticks, actual haul time or total nutrition in meals. 20- versus 15-day rotting intervals make pumpkin **less** storable than carrot in current MO, contrary to the direction expected of traditionally stored mature pumpkin under suitable real-world conditions; this is a material realism/balance review item, **not** license to auto-select a new rot value. A longer pumpkin shelf life could greatly increase usable high-yield stored food over the 60-day RimWorld year, so test one-harvest versus accumulated/multi-cycle inventory before selecting a coefficient. Japanese pumpkin/western pumpkin horticulture and actual storehouses are not equivalent; do not map historical months 1:1 to a RimWorld day.

## 4. Decision and implementation boundary

1. **Botanical target and historical caveat established:** Japanese pumpkin, Eastern carrot (late chronology), rather than the upstream generic Western-style depictions. Preserve crop DefNames and historical/period uncertainty in the source. Avoid a falsely specific cultivar named Kintoki or “original Japanese pumpkin sprite completed.”
2. **Keep current numeric XML for now:** carrot 6/6/Plants4/Intermediate and pumpkin 8/20/Plants8/Advanced/MinFert0.7, along with existing nutrition/rot/price/fertility. These have meaningful short-season versus high-yield roles in the bounded game model; this is an **interim game-design acceptance of retaining existing settings**, not an independent empirical agricultural calibration or final gameplay balance PASS.
3. **Open, narrowly scoped:** pumpkin storage (15 vs carrot20) for historical plausibility and long-term economy; final inherited carrot fertility; crop appearance/Japanese English description and other-Mod Japanese translated description. Agriculture owns crop/numeric acceptance, Japanization owns MO `DankPyon_` descriptions/translation/retexture, and the author explicitly skipped image work. Do not change English/Japanese player-facing historical prose or textures without the relevant owner acceptance. In-world exact century-limited sowing requires its own cross-crop design and is not implied by old research DefNames.
4. **No production XML changes, no native game or release action in this review.** Actual loaded RimWorld 1.6/1.7, real seasonal harvest and whole-farm economy are unverified. Retain all earlier 10 Western dish exclusions, original raw crop and generic cooking, and the four frozen MO medicinal crop/drug lines.

