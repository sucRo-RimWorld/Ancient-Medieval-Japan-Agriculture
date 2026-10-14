# Vanilla/MO cultivated-plant growth-time audit (2026-10-14)

## Scope and source

Grains changes only the `plant/growDays` of already existing sowable PlantDefs, and only while Grains is loaded. The objective is to use the same agronomy-informed, gameplay-compressed relative scale that its own seven grains use; it is **not** a direct conversion of Gregorian days into RimWorld `growDays`. The historical Grains crop set uses 4-12 growth days. At ordinary outdoor light, `growDays` 3 represents at least ~5.54 calendar days before other growth modifiers; a player observed MO lentils becoming harvestable in about seven elapsed days.

- Existing Vanilla PlantDefs: `RimWorld-zh/RimWorld-Core`, `Core/Defs/ThingDefs_Plants/Plants_Cultivated_Farm.xml` (public Vanilla source mirror; game-version-specific loaded verification pending), https://github.com/RimWorld-zh/RimWorld-Core/blob/master/Core/Defs/ThingDefs_Plants/Plants_Cultivated_Farm.xml
- Installed/source-provided Medieval Overhaul 1.6: `3219596926.zip`, `3219596926/1.6/Defs/ThingDefs_Plants/Plants_Cultivated_Farm.xml`. This audit does not assume its 1.7 behavior.
- Agronomy: NDSU lentil maturity trials (~85-93 days), https://www.ndsu.edu/agriculture/sites/default/files/2023-01/2022%20CREC%20Annual%20Report.pdf ; UMN Extension vegetables (carrot 55-75 days, potato 50-100 days, garlic overwinters), https://extension.umn.edu/vegetable-growing-guides-farmers/crop-and-field-planning-tools-vegetable-farmers ; UMN cabbage 60-100 days, https://extension.umn.edu/garden-and-home/yard-and-garden/gardening-in-minnesota/growing-cabbage . Exact day-range comparisons to other species depend on cultivar/region and are not claims that the game uses physical calendar conversion.
- Growing time concept: https://rimworldwiki.com/wiki/Crop ; 13 hours of plant growth opportunity per game day, yielding a nominal `60,000 / 32,500` elapsed-day multiplier without growth penalties.

## Existing-Mod audit and ownership decision

Checked 2026-10-14, RimWorld 1.6 and provided MO 1.6 source. Search topics: `RimWorld realistic crop growth time balance`, `Vanilla Plants Expanded growing period`, `Progression Agriculture MO`, `Medieval Overhaul lentils maturity`, `RimWorld growth time patch`. VE candidate *Vanilla Plants Expanded* (https://steamcommunity.com/sharedfiles/filedetails/?id=2134308522) adds more crops and plowing but explicitly balances around Vanilla, not around Grains' period-informed values. Non-VE *Progression: Agriculture* (https://steamcommunity.com/sharedfiles/filedetails/?id=3418017359) adds seed/unlock mechanics and MO/VPE crop deduplication; neither feature corrects base Vanilla+MO growth-time mismatch without dependency/extra system. Source XML for these candidates was not obtained; their Workshop descriptions and feature scopes, not a proven nonexistence claim, support the narrower decision.

**Decision:** narrow existing-Grains crop balance patch (not a new Mod, not a global framework) targeting named Vanilla and MO `growDays` only. Preserve original defs, yields, harvested items, sow conditions, temperature, CCTO extensions, research, graphical paths, trees and 1.7 version boundary. Do not require VE/Progression Agriculture. If both third-party providers alter the same XPath, last patch wins and loaded profile needs separate verification.

## Adopted changed growth days (old => new)

| Provider | PlantDef | Old | New | Agronomic/gameplay rationale |
|---|---|---:|---:|---|
| Vanilla | Plant_Potato | 5.8 | 6 | fairly early edible tuber |
| Vanilla | Plant_Corn | 11.3 | 13 | fertile long-season grain; avoid easier harvest outperforming processed millet |
| Vanilla | Plant_Strawberry | 4.6 | 6 | moderate production interval; not instant early staple |
| Vanilla | Plant_Cotton | 8 | 10 | long-season fiber crop |
| Vanilla | Plant_Hops | 5 | 7 | not an exceptionally fast agricultural output |
| Vanilla | Plant_Smokeleaf | 7.5 | 9 | maintain lengthy non-staple crop |
| MO | DankPyon_Plant_Onions | 4 | 8 | mature bulb crop, not comparable to rapid greens |
| MO | DankPyon_Plant_Lentils | 3 | 8 | ~85-93-day grain legume cannot undercut AMJ short-cycle millet so strongly |
| MO | DankPyon_Plant_Garlic | 4 | 10 | overwintering bulb crop compressed for game seasons |
| MO | DankPyon_Plant_Flax | 7 | 9 | field-grown fiber crop |
| MO | DankPyon_Plant_Sugarcane | 5.8 | 12 | very long warm-season grass, compressed for playability |
| MO | DankPyon_Plant_Carrots | 3.25 | 6 | early root crop (~55-75 days), but not 3 growth days |
| MO | DankPyon_Plant_Tomatoes | 4.25 | 7 | fruit-bearing annual, compressed to a single-harvest crop |

These are **initial balance numbers**, not empirical proofs of per-tile seasonal equilibrium. No unreviewed change to per-harvest count or nutrition: after growDays adjustment, the old 10 lentils/3 growth-days changes to 10/8. Other unchanged existing Vanilla/MO crops remain intentionally in the overall comparison rather than being forcibly normalized by applying arbitrary multipliers.

## Intentionally unchanged (not missed)

Vanilla rice stays `5` (Grains V1 upland/paddy shared Plant). Vanilla haygrass 7, healroot 7, psychoid 9, genetically engineered devilstrand 22.5 and cocoa tree 16 retain their existing cycles. MO cabbage 8, pumpkin 8, wheat 12 (already Grains-owned wheat compatibility), mushroom 3, herb 5.6, grape 19.5 and apple/lemon/mulberry/fictional griffon berry trees retain existing values for now. Fungus, medicinal unspecified herbs, perennial fruit trees and fictional plants cannot be assigned a seed-to-one-off-harvest period by the same annual-field-crop rule. Changing them requires a crop-cycle-specific model, not a blanket days multiplier. Cocoa and trees are perennial. MO crop list checked against all 17 source ThingDefs.

## Test assumptions, exclusions and open acceptance

Static: all 13 changed paths target known preexisting `plant/growDays` elements; MO names and old values match the supplied 1.6 XML, Vanilla names and baseline values match the named mirror; no changes touch `Plant_Rice`, MO wheat, own 7 crops, `harvestYield`, CCTO temperature values or production outputs. Runtime: Vanilla, Vanilla+CCTO, MO, MO+CCTO need loaded-Def exact days, sow/harvest and seasonal/nutrition-per-day comparison, complete ERROR-zero log. Native 1.6 and potential 1.7 must be assessed separately. Existing player save crop growth fractions are not rewritten; new crop cohorts use new growing speed. Existing native test PASS predating this change is not evidence for this patch. Workshop/public publication is out of scope.
