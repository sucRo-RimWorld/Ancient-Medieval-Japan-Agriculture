# Ancient & Medieval Japan - Grains（中世日本 - 穀類）

**RimWorld 1.6 — 0.1.0 first-release candidate (not yet published)**

The current published-name candidate is a standalone grain-growing and food-processing Mod inspired by pre-Edo Japan; development scope is expanding into **Agriculture**, which also owns historical selection of MO/Vanilla cultivable crops and their related food and processing chains. The display name is still Grains until the separately reviewed product renaming. Formerly AMJ Core; the historical `sucro.ancientmedievaljapan.core` packageId and existing AMJC DefNames remain unchanged to retain cross-Mod identifiers.

## Features

- Awa, Hie and Kibi, with different growing seasons, soil fertility requirements and temperature preferences.
- Soba, barley, wheat and upland rice (using Vanilla `Plant_Rice`).
- Integrated source adjustments formerly owned by Crop Growth Balance: six Vanilla crop growth settings and seven optional MO plant definitions. Some historically unsuitable MO crops (including lentils) are now **excluded from ordinary sowing** despite their compatibility-preserved growth Def fields. This does **not** make them available crops.
- Harvested sheaves, threshing and hulling, grain milling, and basic flour foods.
- Vanilla-only wheat, wheat flour and manual millstone when Medieval Overhaul is absent.
- MO wheat/RawWheat/flour/millstone reuse and thresh-time Straw integration when MO is present, without duplicate default wheat or milling routes. AMJG-owned balance and processing settings take priority.

The balancing model, including the inherited Vanilla/MO growth periods and provisional MO lentil skill 2, is specified in [Design](Docs/Design.md) and [cultivation balance](Docs/Balance/Crops/Millet_Cultivation_Balance.md).

## Agriculture transition — optional MO crop and food selection (not part of a released V1)

Agriculture's development source now owns crop-to-food historical filtering when Medieval Overhaul is installed. This is **not** a claim that the present Grains 0.1.0 release candidate has already become the renamed Agriculture release, or that the entire new crop integration has passed native gameplay checks.

- **Excluded from ordinary sowing:** MO onion, headed cabbage, tomato, lemon, lentil, sugarcane and griffonberry. Farm mushroom sowing is also disabled while natural `RawFungus` gathering and the ordinary mushroom-soup ingredients remain available. The original DefNames remain for compatibility.
- **Retained with independent processing uses:** wheat (grain/flour/foods), ramie reusing MO flax identifiers (textile processing), and mulberry (silkworm/silk as the primary historical rationale). Historical and supply-balance audits remain open where noted in [the crop-use audit](Docs/Research/MO_CropActiveUseAudit_20261026.md).
- **Still sowable, but crop utility is not yet accepted:** grapes, Eastern-type carrots, Japanese-type pumpkins and waringo. The original MO Western-style fruit wine, pies, pancake and several other inappropriate food recipes have been disconnected. The resulting fruit/vegetable items can still serve as generic food ingredients, but **dedicated active MO recipes are absent**. Retaining a Def for saved games is not a decision to keep farming it permanently.
- **Other open crop paths:** garlic remains usable as food and in a MO soup; culinary herbs can be processed into spices. The historical fit and player benefit of those remaining dedicated processing paths have not yet been approved.
- **Imported sugar:** original domestic sugarcane processing is disabled, and its 11 original MO sweet-dish Bills are currently disconnected. The formerly approved Agriculture-added trader stock of 3–6 sugar has been **suspended** because no approved domestic cooking Bill currently consumes it. The sugar ThingDef and its market value 5 remain, as do any original MO or third-party import/trade routes; there is **no longer a custom Agriculture ArtPatron sugar stock generator**. **No speculative confection is authorized** to manufacture demand.

A future decision on sowing, produce supply, or imported sugar needs both a historically supported purpose and a meaningful gameplay role; neither a workbench recipe nor a crop will be invented simply to preserve an otherwise orphaned supply line. The specific technical state, uncertainties and revision boundaries are in [the crop-use audit](Docs/Research/MO_CropActiveUseAudit_20261026.md).

## V1 food options

The same harvested grains can be cooked as grain meals or porridge, or milled into flour. Common dishes can combine leftover cereals; flour-specific dishes provide a small mood and preservation advantage. Brown rice is cooked with the common grain meal or porridge Bills; polished white rice also has two special dishes.

| Dish | Accepted ingredients | Cooking work (1 serving) | Room-temperature spoilage | Mood on eating |
|---|---|---:|---:|---|
| Common grain meal | Six edible grain item types, including brown and white rice (mixing allowed) | 240 | 3 days | No extra bonus |
| Common grain porridge | The same edible grains (mixing allowed) | 200 | 1.5 days | No extra bonus |
| Common flour food | Four grain flours (mixing allowed) | 300 | 2 days | No extra bonus |
| Four flour-specific dishes | One designated flour per dish | 300 | 2.5 days | +2 for 0.5 days |
| White rice meal | Polished white rice only | 220 | 3.5 days | +3 for 0.5 days |
| White rice porridge | Polished white rice only | 180 | 1.75 days | +1 for 0.5 days |

There are **9 normally producible dishes with 18 cooking Bills** (one- and four-serving recipes). Every dish uses 0.5 ingredient nutrition for a serving worth 0.9 nutrition; four-serving Bills use four times the ingredients and cooking work. The four flour-specific dishes are *hakutaku (houtou), sobagaki, millet dumplings and barley-flour gruel*. The active flours are wheat, buckwheat, millet and barley; Medieval Overhaul supplies its own wheat flour when installed. Grains does not automatically prioritize ingredients by remaining shelf life, so Bill ingredient filters remain available for leftovers.

Paddy rice has separate processing Bills: 10 portions of paddy produce 10 brown rice (Work 80) or 8 white rice (Work 240). Brown rice goes into the common grain meal/porridge recipes, while polished white rice unlocks the dedicated white meal and porridge. The former brown-rice-only dish Defs and cooking recipe IDs are retained for existing saved references, but their Bills are no longer offered at cook stations.

### Q.米粉は？

米粉は古代・中世日本にも存在した。ただし、**玄米粉が中世庶民の日常的な食材として使われていたとする十分な根拠は確認できない**。米粉を独立した製粉品・備蓄品として増やしても、現状のGrainsの遊びに必要な選択が増えるとはいえない。そのため、通常の米粉製粉と米粉団子の調理を除外した。玄米・白米は残し、玄米は共通の飯・粥で利用できる。白飯・白粥は白米専用料理として残す。旧米粉・米粉団子の定義は互換性のため保持し、史料に基づく儀礼食・菓子を扱う場合に米粉の採用を再検討する。

### Q. What about rice flour?

Rice flour did exist in ancient and medieval Japan. However, there is insufficient evidence to treat flour made from brown rice as an everyday ingredient of medieval commoners. A separate rice-flour production and stockpiling loop also lacks a clear gameplay purpose in Grains. Rice flour milling and rice-flour dumpling cooking are therefore unavailable in normal production; brown and white rice remain available, with brown rice used in common meals/porridge and dedicated white rice meal/porridge retained. Legacy definitions are retained for compatibility. Rice flour may be reconsidered for historically supported ceremonial foods or confections.

These values reflect the current source definitions. The current nine-dish food loop and selected cooking Bills have been checked in native RimWorld 1.6 Vanilla/CCTO gameplay; MO+CCTO gameplay also completed its ten scenarios on the nine-dish source. The unchanged external MO/SYR FMOD audio errors remain in the original logs and are not Grains release blockers. These checks do not establish ordinary-play balance, final zoom-level appearance or the actual Workshop subscriber build.

## Dependencies and compatibility

**Required:** RimWorld 1.6. **No mandatory additional Mods or DLC.**

**Strongly recommended (optional):** [Adaptive Primitive Storage](https://steamcommunity.com/sharedfiles/filedetails/?id=3400037215). Its existing granary can store Grains' grain sheaves, kernels and flours, including MO wheat. The original granary and woven baskets retain their Hay construction requirements. With both MO and APS active, Grains additionally offers straw-built variants in the construction menu: select either the Hay original or a variant using the same amount of MO Straw. No building requires both.

**Other optional integrations:** Medieval Overhaul (MO) and [Crop Cold Tolerance Overhaul](https://steamcommunity.com/sharedfiles/filedetails/?id=3812412548) (CCTO).

**The former Crop Growth Balance Mod has been merged into Grains and is no longer needed or released separately.** Do not enable an old standalone CGB copy alongside the integrated Grains build.

The `loadAfter` entry for MO is an **optional ordering hint, not a hard dependency**. MO-specific definitions and patches load only when MO is active through `loadFolders.xml`; `BaseWithoutMO` supplies the fallback path otherwise. CCTO adds explicit cold-death support when installed.

New Village and its starting definitions are provided exclusively by the separate AMJ Scenarios Mod. Grains does not include a scenario or legacy scenario compatibility copy.

## Initial release status

The author confirmed on 2026-10-10 that Grains has not been installed or used. **There are no existing Grains saves to migrate.** Historical save migration, enabling/disabling mid-save and removing MO from old saves are **out of scope**, not release blockers.

The mandatory MO dependency has been removed. The RimWorld 1.6 nine-dish Grains source passed 10/10 native gameplay scenarios in Vanilla and 10/10 with CCTO, with no Unity errors ([results](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39403725666)). MO+CCTO passed its 10 gameplay scenarios on that nine-dish source ([results](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39404691834)); the original strict test run is marked failed solely because of two pre-existing external MO/SYR FMOD audio errors, which the author excludes from Grains release judgment. A later native Vanilla/CCTO check also passed both white-rice porridge single/bulk cooking jobs ([results](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39403704297)); the added test was not independently rerun with MO. Ordinary-play usability, visual appearance at normal zoom, crop balance, the exact installed payload and the author's publication decision remain open. This initial-release candidate has not been publicly released.

## First release scope / 初回公開範囲

The first release is the existing **RimWorld 1.6** Vanilla-capable grain loop: seven grains (including Vanilla rice), harvesting/threshing/hulling, four active flours, brown/white rice processing, and the **9 existing dishes / 18 single- and four-serving cooking Bills** listed above. Current processing spots, tables, the Vanilla-only manual millstone and the approved temporary cooking-bench paths are included; MO, CCTO and APS integrations remain optional. This describes **included content, not completion of final gameplay/release verification**.

**Not in the first release:** sobakiri (future late-Sengoku research unlock), hoshii/dried cooked rice, new grain dishes or new production steps, paddy-water farming, rice flour/dumpling production, newly redesigned production facilities, extra quantity-based artwork and RimWorld 1.7 support. Future grain-based dishes remain within Grains, but their gameplay specifications and publication dates are not yet fixed. No new crop-yield UI or soil map is required for the first release.

## Planned improvements

- **Revise existing graphics:** Improve outer outlines, silhouettes and readability at normal game zoom across crop, sheaf, grain and flour art, with a consistent visual style alongside Medieval Overhaul. The already accepted 14 crop states and five sheaf graphics remain unchanged for now; their aesthetic revisions are deferred.
- **Complete and review food/equipment graphics:** Finish remaining grain and rice sprites, replace provisional meal artwork where necessary and review the existing processing spot, table and Base manual millstone graphics at normal game zoom. The three equipment images are already integrated; final visual acceptance remains open.
- **Add stack-size variants:** Eventually provide distinct small, medium and large sheaf and flour sprites instead of the current single-appearance fallback. Sheaf quantity variants remain deferred; flour variants are not part of that deferral.

## Documentation

- [Design and ownership](Docs/Design.md)
- [MO dependency audit](Docs/GrainsDependencyAudit.md)
- [Four-profile testing and open runtime gates](Docs/GrainsProfileTesting.md)
- [Crop cold tolerance](Docs/Balance/Crops/ColdTolerance.md)
- [Development tools](Docs/DevelopmentTools.md)
