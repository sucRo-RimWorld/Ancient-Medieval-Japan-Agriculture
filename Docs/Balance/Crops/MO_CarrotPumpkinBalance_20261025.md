# MO Carrot and Pumpkin: cultivar and gameplay-balance decision — 2026-10-25

**Owner:** Agriculture. **Status:** historic type and gameplay role selected for the AMJ pre-Edo crop roster; **existing numbers retained for source implementation**, with the bounded storage concern resolved by source comparison; Japanization's historical Japanese presentation and existing source crop values are now loaded-verified in RimWorld 1.6, while whole-farm seasonal/economic acceptance remains OPEN. The user's 2026-10-25 continuation is authorization to progress this design review; it is **not** permission to invent unreviewed coefficients. No new rimworld XML, images or research gate are introduced in this decision.

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
| **Effective fertility minimum** | **0.7 from Core `PlantBase` inheritance** (MO's 0.6 exists only as an inactive comment) | **0.7 locally explicit** |
| Fertility sensitivity | 0.6 | 0.9 |
| Sow tags | Ground, Hydroponics (upstream, NOT historical endorsement) | Ground |
| Harvest raw rot start | 20 RimWorld days | 15 RimWorld days |
| Raw market value per item | 1.1 | 1.1 |

Note: Pumpkin's `vacuumResistant` XML and carrot Hydroponics support are upstream game abstractions. Neither establishes a historical grow method. Don't invent actual field-crop precision or delete an otherwise referenced legacy field without a separate compatibility decision.

### 2026-10-25 source closure: inherited carrot fertility

**Actual original XML:** In the user-provided MO 1.6.2.2 `Plants_Cultivated_Farm.xml`, `DankPyon_Plant_Carrots` inherits `PlantBase` and supplies no active `plant/fertilityMin`. The commented `<fertilityMin>0.6</fertilityMin>` is inert. `fertilitySensitivity=0.6` controls growth speed, **not sow eligibility**. `DankPyon_Plant_Pumpkins` explicitly specifies `fertilityMin=0.7`.

**Core 1.6 inheritance:** `PlantBaseNonEdible` defines `plant/fertilityMin=0.7` and `PlantBase` inherits it without an override. Therefore the original carrot Def resolves to a **minimum of 70%**. The Core 1.6.4871 extracted parent hierarchy is visible in https://rimworld.huijiwiki.com/wiki/Data:ThingDef_Plant_Psilocap.json (`meta.parents.PlantBaseNonEdible.plant`, `meta.parents.PlantBase`); the older independent Core XML at https://github.com/RimWorld-zh/RimWorld-Core/blob/master/Core/Defs/ThingDefs_Plants/Plants_Bases.xml also confirms 0.7. The Agriculture MO conditional growth patch only changes carrot `growDays` to 6, not `fertilityMin`, and the source MO 1.6 Patch XML does not target either carrot/pumpkin plant. This is a **source-level conclusion**; a combined third-party mod could modify final loaded Defs.

**Player impact:** Both crops can be sown on suitably tagged 70% stony soil, 100% normal soil and 140% rich soil, and **neither is eligible on Environment 50% Thin Soil**. At fertility70% the growth multipliers (on eligible soil) are carrot `1+(0.7-1)*0.6=0.82` and pumpkin `1+(0.7-1)*0.9=0.73`. The prior 70/100/140% tables remain valid. Even though growth formulae yield finite values at fertility50% (0.70/0.55), no sowing occurs there because the min fertility check fails. Shorter growth, lower skill/research, and weaker fertility penalty **within the shared sowable soil range** remain carrot advantages; access to poor 50% soil is not.

**Decision:** Preserve the original `fertilityMin=0.7` for both crops. Giving carrot a new 50% requirement would be a **new numerical balance change**, creating competition with deliberately 40–50% tolerant millet/buckwheat; no approval or independent cross-crop justification exists. No plant XML or images were changed. Only original 1.6 source and independent core parent definition were verified, **not** an actual native RimWorld 1.6/1.7 final loaded farming test.

## 3. Independent bounded scenario comparison (NOT real-game acceptance)

**Predeclared expected player decisions:** (A) in a short growing window on land meeting the shared 70% minimum, a faster/lower-skill crop should remain available; (B) in sustained good conditions a high-tier pumpkin should justify advanced research, Plants8, and fertile ground via high food yield and fewer harvesting actions; (C) storage and ordinary trade should not create an effortless year-round food stock without crop labor; (D) no crop from late Sengoku should masquerade as an early ancient commonplace.

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

At 70%/10 units the carrot finishes once while pumpkin finishes zero times. At 100%/20, carrot needs three sow+harvest cycles to yield 0.90 raw nutrition; pumpkin needs two cycles for 2.00. At 140%/30, pumpkin yields 5.00 raw nutrition in five cycles versus carrot 1.80 in six. Pumpkin requires Plants8 and AdvancedAgriculture versus carrot Plants4 and IntermediateAgriculture; **both need at least 70% fertile soil**, but carrots have lower fertility sensitivity (0.6 versus 0.9) and shorter growth. **Different short-season and research/skill tradeoffs survive; neither “pumpkin always wins” nor “carrot should be deleted” is supported.** Neither crop can be sown on Environment Thin Soil 50% under the present original MO/Core definitions.

**Economy, labor and storage countercheck:** Raw produce both have MarketValue1.1 per item, so at one full harvest the undiscounted nominal values are carrot 6.6 versus pumpkin22 (not net income or actual merchant offer). One harvest+one sow job per completed cycle is only a labor **count proxy**, not pawn ticks, actual haul time or total nutrition in meals. 20- versus 15-day rotting intervals make pumpkin *shorter-lasting than carrots in the game*. **The former assertion that this contradicts real-world storage is withdrawn**: uncut pumpkin can be stored 1–2 months (ALIC; with curing), or around 2–3 months in suitable cool/dry stores; properly topped mature carrots can last about 4–5 months under cooler, high-humidity storage (CSU/Iowa State). Those are **different storage environments**, not evidence that pumpkins always spoil faster in the same room. RimWorld uses one temperature-based rotting mechanic (no crop-specific storage-humidity requirement). Across a 60-day RimWorld year, 15 days = one 15-day quadrum (~one quarter-year) and 20 days = one third-year, so the *rough relative duration* is defensible as an abstraction, not a literal translation from real months. Extending the 15-day pumpkin value to 20 would improve easy high-yield stocking and is not required for historical realism; evaluate concrete scenario bounds below rather than inventing a new coefficient.

### Storage and stock economy acceptance — 2026-10-25 correction

**Historical source separation:**
- ALIC on whole Japanese-market pumpkins: summer harvest, ~two weeks postharvest curing, and **1–2 months** of storage for intact fruit; this is an explanation of commercially prepared pumpkins, not direct evidence for 16th-century storage-house practices. https://www.alic.go.jp/koho/kikaku03_000266.html
- Chiba Prefecture describes **2–3 months** for an uncut pumpkin in a cool dark location. It separately notes that modern Western pumpkin is now prevalent, so this estimate is **not measured specifically for premodern Japanese `C. moschata` landraces**. https://www.pref.chiba.lg.jp/ryuhan/pbmgm/zukan/yasai/kabocha.html
- Colorado State University Extension lists **2–3 months** for pumpkins at ~10–13°C with dry air, **4–5 months** for carrots close to 0°C with very moist air. Iowa State Extension separately estimates properly stored topped carrots at **4–6 months**. The relative ordering *depends on different optimal storage setups* and cannot be generalized to a common unsheltered room. https://extension.colostate.edu/routt/resource/home-storage-of-foods/ ; https://yardandgarden.extension.iastate.edu/how-to/storing-winter-storage-vegetables
- RimWorld mechanics and calendar: food rots at full speed at ≥10°C, half speed at 5°C and stops at ≤0°C; a game year is 60 days with 4 quadrums (15 days each). Real-world above-freezing pumpkin storage would not actually be improved indefinitely by freezing; this is an existing Vanilla abstraction we do **not** rewrite. https://rimworldwiki.com/wiki/Temperature ; https://rimworldwiki.com/wiki/Time

**Prespecified bounded game outcomes:** retain risk of losing a large, skill8/advanced-research pumpkin harvest in uncooled storage; avoid making individual food stocks last effortlessly throughout the year; retain the less demanding carrot's shorter-yield but slightly longer-lived buffer. The counterfactual increase from pumpkin15 to20 days must show a clear player benefit and a demonstrated factual issue *before* an XML coefficient change. Compare normal indoor 15°C, cool store 5°C, and frozen 0°C; then a single overproduction case of twenty simultaneously harvested plots, using **abstract daily raw-nutrition demand=1.00**, no new harvests, no processing/trade, no rot progress at harvest, no alternate food, and full immediate consumption. This is not a real pawn food/plant model or historical refrigeration.

| Storage temperature | Pumpkin current rot life | Carrot current rot life | Effect |
|---|---:|---:|---|
| 15°C (indoor, uncooled) | 15 game days | 20 game days | High-yield storage risk present |
| 5°C (cool) | 30 game days | 40 game days | Both last longer; no extra patch needed |
| ≤0°C (freezing) | indefinitely while frozen | indefinitely while frozen | Vanilla storage abstraction dominates crop-specific value |

**Twenty full-harvest plots in an idealized one-time batch:** pumpkin20 items/plot ×20 plots ×0.05 nutrition/item = **20.00 raw nutrition**, and carrot6×20×0.05 = **6.00**. At a constant demand=1.00 raw nutrition/game day in an uncooled 15°C room, only **15.00 of pumpkin20.00** can be eaten before rot begins, so **5.00 is at risk**; all 6.00 carrots can be consumed before their 20-day rot threshold. If pumpkin were raised to20 days with all else unchanged, the model removes that **5.00** overproduction loss; at 5°C the existing pumpkin15 baseline already extends to30 days and permits all20.00 to be consumed under this toy demand. Actual stacks harvested on different days, temperature cycles, food poisoning/meal input efficiency, pawn hauling/storage, trader transactions, and ordinary harvest failures change outcomes. These computations establish **gameplay cost of a proposed increase**, not a biological lifetime or numerical PASS.

**Resolution:** Correct the previous mistaken historical presumption. **KEEP `DankPyon_RawPumpkins` daysToRotStart=15 and `DankPyon_RawCarrots`=20 unchanged**; the 15/20 distinction is a workable medium-term storage and overproduction constraint at existing high pumpkin harvest yield. No curing job, separate storage humidity/temperature mechanic, special preservation bonus, or new refrigerator is justified by this limited question. Do not elevate modern controlled-storage months to a 1:1 absolute game time scale. A future observed concrete gameplay problem or verified cultivar-specific source may reopen this case, but there is no outstanding storage-only XML task.

## 4. Decision and implementation boundary

1. **Botanical target and historical caveat established:** Japanese pumpkin, Eastern carrot (late chronology), rather than the upstream generic Western-style depictions. Preserve crop DefNames and historical/period uncertainty in the source. Avoid a falsely specific cultivar named Kintoki or “original Japanese pumpkin sprite completed.”
2. **Keep current numeric XML for now:** carrot 6/6/Plants4/Intermediate and pumpkin 8/20/Plants8/Advanced/MinFert0.7, along with existing nutrition/rot/price/fertility. These have meaningful short-season versus high-yield roles in the bounded game model; this is an **interim game-design acceptance of retaining existing settings**, not an independent empirical agricultural calibration or final gameplay balance PASS.
3. **Resolved in bounded source/gameplay comparisons:** pumpkin15 vs carrot20 storage ordering is not inherently historically wrong; preserve both without a numeric patch. Core XML inheritance fixes the carrot's absent local fertilityMin at **0.7**, equal to pumpkin. **Open, distinct issues:** crop appearance/Japanese English description and other-Mod Japanese translated description. Agriculture owns crop/numeric acceptance, Japanization owns MO `DankPyon_` descriptions/translation/retexture, and the author explicitly skipped image work. Do not change English/Japanese player-facing historical prose or textures without the relevant owner acceptance. In-world exact century-limited sowing requires its own cross-crop design and is not implied by old research DefNames.
4. **No production XML changes, no native game or release action in this review.** Actual loaded RimWorld 1.6/1.7, real seasonal harvest and whole-farm economy are unverified. Retain all earlier 10 Western dish exclusions, original raw crop and generic cooking, and the four frozen MO medicinal crop/drug lines.

## 5. Actual original game 1.6 integrated source acceptance — 2026-10-25

**Latest evidence (supersedes narrower source-only/unverified notes above for exactly these properties):** Japanization accepted descriptions [PR #32](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Medieval-Overhaul-Japanization/pull/32) main `a31fbde4ea42ca4d8f28c5caf4fc12f69b4bc8f6`; its existing one-case Japanese original-game Pickle gained the cropped acceptance in [PR #33](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Medieval-Overhaul-Japanization/pull/33) main `f4019d4ca63dc8d855be368c47d2d166dcf9553c`. The Project's manually triggered, source-pinned full-suite 1.6 acceptance [PR #1022](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/1022) main `0a69eb05478cb419e90b2dccf4d8f5fc2e36646d` pinned **this Agriculture main `425c6399bb2a9e206969facee2d964cff2037dae`** and the above Japanization main. **[Actions #40168425002](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/40168425002): original RimWorldLinux `1.6.4871 rev598`, Japanese full-suite+MO+VEF+SYR, Pickle 1/1 passed, Unity ERROR0, process exit0, timed_out false, native_pass true.** The original licensed game was isolated; two known MO/SYR MP3 assets were converted to WAV **in a temporary clone only**, and original files/voices were preserved.

**Actual checked loaded premises, not standalone simulation:** carrot `DankPyon_Plant_Carrots` has `growDays=6` (not upstream 3.25), `harvestYield=6`, `sowMinSkill=4`, and inherited **`fertilityMin=0.7`** with `harvestedThingDef=DankPyon_RawCarrots`; pumpkin `DankPyon_Plant_Pumpkins` has `8/20/8/0.7` and raw `DankPyon_RawPumpkins`. Four Japanese ThingDef descriptions contain approved Eastern carrot / Japanese pumpkin terminology; 10 original Western-dish recipes (carrot cake3, pumpkin pie3, fritters2, milk soup2) have **no recipeUsers**. The same Pickle scenario retained the earlier waringo loaded-source harvest checks. **1/1 is one consolidated scenario, not ten recipe case launches.**

**Still unverified:** actual sow-menu visibility and planting attempt in 50% Thin Soil; NPC seasonal sow/work/harvest on maps or across seasons; difficulty multipliers under other storyteller settings; raw crop/meal merchant actual inventories, site loot, unmodified external mod recipes, existing saved Bills/stock, 1.7, English profile on the same source head, optional Japanese MO translation `Lucifel.MedievalOH.JPT` overwrite order, UI pixel rendering/visual cultivar correctness, and whole-farm long-cycle balance. The scope supports **loaded crop source contracts**, not quantitative field balance or historical cultivar accuracy of existing images. No Agriculture production XML, numeric coefficient, image, Steam or package dependency modified by this acceptance record.

