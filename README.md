# Ancient & Medieval Japan - Grains（中世日本 - 穀類）

**RimWorld 1.6 — Development build**

A standalone grain-growing and food-processing Mod inspired by agriculture in pre-Edo Japan. Formerly AMJ Core; the historical `sucro.ancientmedievaljapan.core` packageId and existing AMJC DefNames remain unchanged to retain stable identifiers for cross-Mod compatibility.

## Features

- Awa, Hie and Kibi, with different growing seasons, soil fertility requirements and temperature preferences.
- Soba, barley, wheat and upland rice (using Vanilla `Plant_Rice`).
- Integrated crop growth balancing for six Vanilla crops and seven optional MO crops (formerly the separate Crop Growth Balance Mod). MO lentils require Plants skill 2; all seven inherited crop growth periods retain their previous CGB values.
- Harvested sheaves, threshing and hulling, grain milling, and basic flour foods.
- Vanilla-only wheat, wheat flour and manual millstone when Medieval Overhaul is absent.
- MO wheat/RawWheat/flour/millstone reuse and thresh-time Straw integration when MO is present, without duplicate default wheat or milling routes. AMJG-owned balance and processing settings take priority.

The balancing model, including the inherited Vanilla/MO growth periods and provisional MO lentil skill 2, is specified in [Design](Docs/Design.md) and [cultivation balance](Docs/Balance/Crops/Millet_Cultivation_Balance.md).

## V1 food options

The same harvested grains can be cooked as grain meals or porridge, or milled into flour. Common dishes can combine leftover cereals; flour-specific dishes provide a small mood and preservation advantage. Brown and white rice also have separate meal and porridge recipes.

| Dish | Accepted ingredients | Cooking work (1 serving) | Room-temperature spoilage | Mood on eating |
|---|---|---:|---:|---|
| Common grain meal | Six edible grain item types, including brown and white rice (mixing allowed) | 240 | 3 days | No extra bonus |
| Common grain porridge | The same edible grains (mixing allowed) | 200 | 1.5 days | No extra bonus |
| Common flour food | Four grain flours (mixing allowed) | 300 | 2 days | No extra bonus |
| Four flour-specific dishes | One designated flour per dish | 300 | 2.5 days | +2 for 0.5 days |
| Rice meal / white rice meal | Brown rice / white rice, respectively | 220 | 3.5 days | None / +3 for 0.5 days |
| Rice porridge / white rice porridge | Brown rice / white rice, respectively | 180 | 1.75 days | None / +1 for 0.5 days |

There are **11 normally producible dishes with 22 cooking Bills** (one- and four-serving recipes). Every dish uses 0.5 ingredient nutrition for a serving worth 0.9 nutrition; four-serving Bills use four times the ingredients and cooking work. The four flour-specific dishes are *hakutaku (houtou), sobagaki, millet dumplings and barley-flour gruel*. The active flours are wheat, buckwheat, millet and barley; Medieval Overhaul supplies its own wheat flour when installed. Grains does not automatically prioritize ingredients by remaining shelf life, so Bill ingredient filters remain available for leftovers.

Paddy rice has separate processing Bills: 10 portions of paddy produce 10 brown rice (Work 80) or 8 white rice (Work 240). The common grain dishes remain available alongside rice-only meals and porridges.

### Q.米粉は？

米粉は古代・中世日本にも存在した。ただし、**玄米粉が中世庶民の日常的な食材として使われていたとする十分な根拠は確認できない**。米粉を独立した製粉品・備蓄品として増やしても、現状のGrainsの遊びに必要な選択が増えるとはいえない。そのため、通常の米粉製粉と米粉団子の調理を除外した。玄米・白米、およびそれぞれの飯・粥は残している。旧米粉・米粉団子の定義は互換性のため保持し、史料に基づく儀礼食・菓子を扱う場合に米粉の採用を再検討する。

### Q. What about rice flour?

Rice flour did exist in ancient and medieval Japan. However, there is insufficient evidence to treat flour made from brown rice as an everyday ingredient of medieval commoners. A separate rice-flour production and stockpiling loop also lacks a clear gameplay purpose in Grains. Rice flour milling and rice-flour dumpling cooking are therefore unavailable in normal production; brown and white rice, and their respective meals and porridges, remain available. Legacy definitions are retained for compatibility. Rice flour may be reconsidered for historically supported ceremonial foods or confections.

These values reflect the current source definitions. Current-revision loaded-game verification of cooking Bills and ingredient selection is still outstanding; this development build is not a publicly verified release.

## Dependencies and compatibility

**Required:** RimWorld 1.6. **No mandatory additional Mods or DLC.**

**Strongly recommended (optional):** [Adaptive Primitive Storage](https://steamcommunity.com/sharedfiles/filedetails/?id=3400037215). Its existing granary can store Grains' grain sheaves, kernels and flours, including MO wheat. The original granary and woven baskets retain their Hay construction requirements. With both MO and APS active, Grains additionally offers straw-built variants in the construction menu: select either the Hay original or a variant using the same amount of MO Straw. No building requires both.

**Other optional integrations:** Medieval Overhaul (MO) and [Crop Cold Tolerance Overhaul](https://steamcommunity.com/sharedfiles/filedetails/?id=3812412548) (CCTO).

**The former Crop Growth Balance Mod has been merged into Grains and is no longer needed or released separately.** Do not enable an old standalone CGB copy alongside the integrated Grains build.

The `loadAfter` entry for MO is an **optional ordering hint, not a hard dependency**. MO-specific definitions and patches load only when MO is active through `loadFolders.xml`; `BaseWithoutMO` supplies the fallback path otherwise. CCTO adds explicit cold-death support when installed.

New Village and its starting definitions are provided exclusively by the separate AMJ Scenarios Mod. Grains does not include a scenario or legacy scenario compatibility copy.

## Development status

The author confirmed on 2026-10-10 that Grains has not been installed or used. **There are no existing Grains saves to migrate.** Historical save migration, enabling/disabling mid-save and removing MO from old saves are **out of scope**, not release blockers.

The mandatory MO dependency has been removed. Previous fresh-start Vanilla / MO × CCTO tests passed on their tested revisions; current-revision game loading and native Bills, final graphics, UI/language and gameplay balance remain to be verified. This development build has not been publicly released.

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
