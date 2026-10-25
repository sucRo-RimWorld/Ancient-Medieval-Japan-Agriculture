# AMJ Grains Coordination

## AGRI-MO-MULBERRY-HARVEST055-20261025 — approved mulberry-only repeated harvest correction

**Owner:** Agriculture. Author approved MO `DankPyon_Tree_Mulberry` harvest threshold inherited0.40→explicit **0.55**; implemented only in MO-optional `Compatibility/MedievalOverhaul/Patches/MO_MulberryHarvestBalance.xml` via conditional match/nomatch for original inherited and future explicit fields. Preserve reset0.30, growDays19.5, yield6, silkworm3/chance1, Cocoa research, 6-day SilkBed/25 capacity, ProcessDef efficiency5, original grape crop and wine exclusions; no new Vanilla dependency or silk-bed change. Independent ideal 13h/day 60-day opportunity counts: soil70% 10→4, 100% 13→5, 140% 18→7; a 30-day 100% season remains 2 after change. Processor Framework public source shows **capacity25 is max, not minimum**, 1 insect can be loaded and processed; 3 worms→15 silk in 6 ideal days. Actual harvest C# side drop, normal job/season, CCTO, 1.6/1.7 runtime and old saves **not yet game tested**; native costly runs not triggered. Canonical rationale `Docs/Design.md` + `Docs/Research/MO_CultivatedCropAudit_20261023.md`. No image work, release or other coefficient changes.

## AGRI-MO-WHEAT-MILL600-20261025 — author-approved MO wheat bulk grinding adjustment

**Owner/production:** Agriculture main optional MO folder `Compatibility/MedievalOverhaul/Patches/MedievalOverhaul_StageA_Wheat.xml` adjusts only `DankPyon_CraftFlourBulk.workAmount` from MO original **800 to author-approved 600**. Exactly 10 `AMJC_Wheat`→10 `DankPyon_Flour`; MO 1.6 `CookSpeed` and the single (Work100)/manual (Work300) recipes are unchanged. Grain 10 thresh Work120, meal cooking Work300, other grain milling Work300, actual post-AMJ flour rot60 all remain. Source input `3219596926.zip` 1.6, independently checked 0.75/1/1.5/2.0 CookSpeed with GeneralLaborSpeed1; 28-unit one-harvest residue case. Ratio 10 singles versus new batch40% saved, but other crop milling Work is `GeneralLaborSpeed`, so simple total Work1020 is *not elapsed time*. Formal decision and model `Docs/Design.md` and `Docs/Balance/Crops/SevenGrainFoodChainAudit.md`. **Status:** XML+owner design, static limited audit; **changed production has no native 1.6/1.7/runtime-load/work-balance confirmation**, no publication or images. MO optional, Vanilla production unchanged. Another Mod deliberately reusing the exact MO grinding Recipe may inherit its new 600. Old coordination/test PASS predates change and is not validation of revised work.

## AGRI-MO-WARINGO-HARVEST-055-20261025 — approved one-field correction

**Owner:** Agriculture. Author's direct 「ｙ」 approved the proposed MO waringo `harvestMinGrowth=0.55` as the single crop coefficient change. Production `Compatibility/MedievalOverhaul/Patches/MO_WaringoHarvestBalance.xml` sets just `DankPyon_Tree_Apple` threshold from native observed 0.40 to 0.55; `growDays=33`, `harvestAfterGrowth=0.30`, fruit yield20, Nutrition0.05, rot15, research and the Western-pie/cider exclusions remain. 60 ideal 13h-growth calendar days: 100% soil opportunities 6→2; 140% 9→3; 70% 4→1, with no work/weather/actual season modeling. Formal new approved rule and bounded premises: `Docs/Design.md` and `Docs/Research/MO_AppleWaringoAudit_20261024.md`. **Not yet native-verified** for changed XML (1.6/1.7), ordinary pawn harvests, new no-Japanization integration or old saves; no new native jobs or art. Sugar import3–6/price5, MO herbs and other fruit values untouched.

## AGRI-MOJ-COMPONENT-MIGRATION-20261025 — former Japanization agriculture patches now owned/implemented here

**Author change and actual production:** old centralized Japanization no longer owns crop/fruit/fibre/Straw MO patches. Agriculture [PR #258](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Agriculture/pull/258), main `ced45d030c39ac1273d71a83fd4111d1941700da`, moved 4 full MO patch XML files (waringo/Cocoa fruit research text; Eastern carrot/Japanese pumpkin English descriptions; flax→ramie/cloth/recipes; removal of Hay→Straw from both MO drying racks) and 5 Japanese DefInjected files into **existing MO-only Compatibility/MedievalOverhaul/**. `About` optional `loadAfter Lucifel.MedievalOH.JPT`; original DefNames, crop stats, textures, research/ProcessDef identities stay. Japanization [PR #35](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Medieval-Overhaul-Japanization/pull/35), main `ec616c5140034e41398cbfc6605ff9a360b1a2b4`, deleted same 9 old owner duplicates, retaining its other 3 legacy production patches and rain-drying DLL until a safe future owner. Agriculture does NOT own that DLL, Griffon animal generation/trade or new general drying system by implication. Owner specification `Docs/Design.md`; retexture ownership latest in Project `Docs/RetextureImplementationGuidelines.md`. Previous coordination implying J owns agricultural text/fibre no longer applies. 2026-10-25 old original full suite 1.6 PASS included Japanization; **distributed MO+Agriculture without it is not runtime-verified** and saves/optional Japanese translation overlay have not been tested. No Steam or new image adoption.


## GRAINS-KOGASHI-CONFECTIONERY-GATE-20261021 — intermediate deferred until useful common ingredient

**Owner decisions confirmed 2026-10-21:** Tea's existing rice/optional-barley → kogashi-yu stays one direct Cooking Bill; do not add standalone `こがし`, roasted-grain intermediates, or separate roast/grind Bills for the low-effect drink. Grains alone owns future edible kogashi and confectionery. At a future sweets design, evaluate a shared kogashi ingredient **only if multiple meaningful uses justify increased labor, Bills, storage, and compatibility costs**; a lone sweet does not force a split. If later warranted, roasting+grinding remain one Grains processing step, Tea retains drink ownership and standalone rice fallback, optional Grains-enabled path avoids duplicate drink Bills, and migration is separately audited. Confirmed specification: [Grains Design](Docs/Design.md), [Tea Design](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Culture-Tea/blob/main/Docs/Design.md). **No initial nine-food scope/production XML, existing recipes, or native test state changed.**


## GRAINS-GRAINPROCESSING-WORKROLES-20261020 — independent processing work and optional WorkRoles grouping

**State:** 1.6 standalone & MO+WorkRoles real-pawn tests and one original old-save migration have prior PASS evidence. Remaining OPEN: Farmer-specific normal job ordering, 1.7, exact distributed WorkRoles DLL and full-colony throughput. Source/evidence: `Docs/Design.md`, `Docs/GrainsProfileTesting.md`, [Project 1.6 #39497770576](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39497770576) (10/10 Pickle, original two external FMOD errors remain strict FAIL), [two-process save #39543914641](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39543914641). Full former chronology archived in `Docs/History/Coordination-2026-10.md`.

## GRAINS-V1-NATIVE-20261020 — latest nine-food acceptance and MO patch outcome

**State:** Nine active foods/eighteen Bills. 1.6 Vanilla/CCTO [#39403725666](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39403725666) 20/20 and ERROR0. Original MO 1.6 functional evidence [#39413523334](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39413523334) 10/10, with two external FMOD errors, so strictly not ERROR0. 1.7, current MO migration and full release remain OPEN. Exact fixture SHA and runs in `Docs/GrainsProfileTesting.md`; chronology archived.

## GRAINS-CROP-UI-DRAFT-WITHDRAWN-20261020 — unvalidated display design not approved

**State: prototype REJECTED as premature; PR [#192](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/192) CLOSED UNMERGED (source head `f937b2dee5f683a542d36e66f2e63213abf93281`).** The agent invented a 15/30/60-calendar-day total-yield and harvest-cycle comparator before establishing the actual player decision and without author approval for the UI. That source was never built into the distributed `Assemblies/AMJGrains.dll` and its UI has no native verification. Do not revive or implement it by assumption. The later agent proposal to replace total yield with throughput is **likewise unapproved**, not a final design or new instruction.

**Next work (evaluation, not permission to change values/UI):** First use the existing Grains crop data/food processing to distinguish real decision situations: short-season survival, poor soil and cool temperature viability, fertile-soil productive use, actual labor/handling, and rice's exceptional processed-food value. Determine which crop choice is meaningfully different and whether the already-present plant selection information lets a player understand it. Example: at 140% fertility, 20 ideal effective growing days rice 55 harvest-units/five cycles vs awa 54/two cycles, so merely adding a total yield column obscures the labor tradeoff; this is a theoretical example not a calendar forecast. For cases where no meaningful choice exists, address crop values before UI. Only after a demonstrated player-visible information gap and specific author agreement, decide on minimal additional display. **No 15/30/60 horizon, total-harvest ranking, work proxy, or unrelated crop number is an agreed requirement.** Existing fertility-growth mechanism stays in place. No production crop XML or compiled DLL change.

## GRAINS-FERTILITY-GROWTH-REVIEW-20261020 — author holds yield-only conversion

**Latest author decision (2026-10-20):** Do **not** move fertility effects exclusively to per-harvest yield yet. Preserve the existing soil-fertility growth-rate mechanism and assess whether `growDays`, `harvestYield`, `fertilitySensitivity`, `fertilityMin`, skills, post-harvest nutrition/work and finite seasons can create meaningful crop differentiation before considering a new mechanic. Grains [PR #190](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/190), merge `538cc8fc525faf8620094e5b1e55427afaba2fcb`, documents a 9-crop cross-balance analysis in `Docs/Balance/Crops/FertilityGrowthModelReview.md` and supersedes §4.3.1's prior implementation direction. In source-only arithmetic, 45/45 20-effective-growth-day cells matched expected values, not native test PASS. 140% soil/20 effective days: rice 55 units with 5 harvest cycles versus awa 54 with 2; MO lentils 100% 20 days 20 units/2 harvests vs onions 26/2, both raw food Nutrition .05. This shows both meaningful differentiation and specific deficiencies. No growDays, harvestYield, fertilizer values, XML patch, DLL or Steam modifications. **Earlier 200% ceiling applies only if a new direct-yield multiplier is revived; it is NOT a current Vanilla growth-speed cap.** Follow-on: focus on rice/awa and MO lentils/onions and soba/kibi crop-level changes before deciding to implement any yield framework. Do not initiate native tests automatically.


## GRAINS-CGB-MERGER-20261019 — author-approved unified crop balancing

**MERGED into Grains main:** [PR #183](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/183) `89a71ea1b223d46bcab2dec46eadb8f2189c70cf`. Grains now ships 6 Vanilla + 7 optional MO former CGB crop `growDays` values, and MO lentil sowMinSkill=2 formerly in Japanization. Rice skill4, seven-grain crop values, CCTO cold rules and MO optionality unchanged. Definitive source: `Docs/Design.md §4.2.1a` and the two ExistingCropGrowthBalance XML files; source checks 15/15 combined ownership/inventory PASS; native loaded Def/sow/harvest on this exact main NOT RUN. Standalone CGB retired, not an additional runtime dependency. Japanization removed both its CGB dependency and duplicate lentil patch. Next: seven-grain + Vanilla/MO crop **yield/skill/fertility overall rebalance**, including lentil growDays8/harvest10/sow2 now provisional; future 200% capped yield system remains unimplemented; do not silently change numbers based on the merger.


## GRAINS-RICE-FLOUR-RETIREMENT-20261019 — production change merged

**Owner:** Grains. **State:** SOURCE IMPLEMENTED; TARGETED STATIC SOURCE CHECK PASS; NATIVE UNVERIFIED. Grains [PR #175](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/175), main `8ffcd5515041c1ea695e8106167a815202aaaba9`, implements the author's decision **not** to require rice flour as a routine ancient/medieval staple. The old `AMJC_RiceFlour`, `AMJC_RiceFlourDumplings` ThingDefs and `AMJC_MillRice` / `AMJC_CookRiceFlourDumplings` (+Bulk) RecipeDefs are retained for saved references only: all three Bills have no recipeUsers and MO no longer restores the rice milling Bill. Shared flour food 1/4 recipes exclude rice flour; MO flour substitution moved to ingredient slot 4. Active flour paths: wheat (MO if present), buckwheat, millet, barley (four); active cooked food paths: common three, specified flour four, specified brown/white rice four (11 foods/22 Bills). Do not delete accepted art or treat legacy IDs as active production. Formal source: `Docs/Design.md`, `Docs/Balance/Crops/SevenGrainFoodChainAudit.md`, actual XML/conditional MO patch. Static XML mapping assertions examined; native Bill menu/load, MO and old-save compatibility remain OPEN. No Steam upload. **2026-10-19 scoped follow-up:** Current-source static source assertions 30/30 PASS; found and corrected a 5/4/4-length bug in `Tests/E2E/GrainsProfileSteps.cs` (the retired `RawRice` milling slot), and added a focused test guard. Existing Python validators and actual loaded game have NOT been executed for the corrected source; formal evidence/boundary in `Docs/GrainsProfileTesting.md`. **Public-copy synchronization (2026-10-19):** README and EN/JA Workshop draft now explain the rice-flour exclusion in a Q&A and use the active 4-flour/11-dish inventory; the superseded comparison image is retained in Git but held out of the current Workshop draft. GitHub copy only; no Steam publication.

## GRAINS-RICE-WHITE-20261019 — separated brown/white rice and meals

**Owner:** Grains processing/cooking and graphics. **Status:** XML and documentation integrated on main in [PR #163](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/163), merge `7a166f3425777d319035f0ec0f2478c54b55fe37`; native execution/UI/approved distinct images **OPEN**. Brown rice retains Vanilla `RawRice`; white rice is `AMJC_WhiteRice`. Existing paddy10→brown10 Work80 Bill stays; new paddy10→white8 Work240 Bill. **Later 2026-10-20 decisions supersede this four-meal scope:** separate brown-rice-only meal/porridge Bills retired (legacy ThingDef/RecipeDef remain with `recipeUsers` removed), and the active rice-specific dishes are white meal and white porridge only (single/bulk: four Bills). White meal Mood+3 and porridge Mood+1 for 0.5 day; processed paddy10→brown10 Work80 and paddy10→white8 Work240 remain. Current first-release inventory **9 dishes/18 cooking Bills**. Preserve mixed meal/porridge; the later PR #175 supersedes its former rice-flour production connection. Formal source: `Docs/Design.md §5.3`, `Docs/Balance/Crops/RicePostHarvestProcessing.md`, exact Def/Recipe/Thought/XML.

**Cross-owner decision:** Do **not** apply “dislike non-white rice” universally to pawns with wealthy backgrounds. Evaluate limited learned refined-food expectations only for specifically fitting character backgrounds. Eligible backgrounds belong to Culture Characters; conditional meal-Thought gameplay belongs to Culture, not to Grains or the Characters build. This is a proposal, not an implemented mood penalty. Avoid treating character repository's unresolved paired 1.6 native cutover as proof the effect exists.

## AGRI-MO-CARROT-PUMPKIN-NATIVE16-20261025 — loaded source contract PASS

**Owner/evidence:** Agriculture main `425c6399bb2a9e206969facee2d964cff2037dae`; Japanization text source main `f4019d4ca63dc8d855be368c47d2d166dcf9553c`; Project manually pinned full-suite Japanese original 1.6 [Actions #40168425002](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/40168425002). **Pickle 1/1, Unity ERROR0, process exit0, native_pass true.** Carrot loaded 6/6/Plants4/fertilityMin0.7, pumpkin 8/20/Plants8/fertilityMin0.7, raw harvest item Def links intact, four Japanese crop ThingDef descriptions correct, all 10 Western carrot/pumpkin specific cooking Bills disconnected; prior Waringo check in same case. Evidence and remaining boundaries at `Docs/Balance/Crops/MO_CarrotPumpkinBalance_20261025.md`. **Native accepted only for these final Def/recipeUser properties**, not season/actual map sow, crop economic balance, trade loot, 1.7, optional translation precedence, original cultivar sprite or old saves. No gameplay XML/image/Steam modification made in this documentation closure. Older source-only unverified entries below are historical for this precise scope.

## AGRI-MO-CARROT-PUMPKIN-BALANCE-20261025 — cultivar targets and current values provisionally retained

**Agriculture formal sources:** `Docs/Design.md` and `Docs/Balance/Crops/MO_CarrotPumpkinBalance_20261025.md`, original MO 1.6.2.2 user ZIP and active `MO_ExistingCropGrowthBalance.xml`. Botanically historic targets: 16th-century Japanese pumpkin (`C. moschata`), and late-16th/17th-century Eastern carrot only, **not modern Western varieties or ancient ubiquity**. MO Japanese/English descriptions and original artwork are not yet true representations; image generation/replacement remains explicitly skipped. Limited independent 5/10/20/30-effective-growing-unit × fertility70/100/140% comparison preserves early carrot/late high-yield pumpkin distinction using **existing** carrot growDays6/yield6/skill4 and pumpkin8/yield20/skill8/minFert70%. Storage concern resolved in 2026-10-25 report update: pumpkin rot15 vs carrot20 is not historically inverted under properly differentiated storage conditions, and extending pumpkin life would reduce high-yield overproduction risk; **retain both without XML changes**; **no new number, research/era lock, label, XML or production image**. Authoritative numeric decisions remain Agriculture, presentation/translation of MO-owned Defs belongs to Japanization; do not make author a courier. **2026-10-25 source inheritance resolved:** MO carrot has no active explicit fertilityMin; Core 1.6 PlantBaseNonEdible supplies 0.7 through PlantBase, same as explicitly-set pumpkin 0.7. Both cannot be sown on Environment Thin Soil50%. Carrot's short-season/skill advantage remains but there is no 50% soil advantage. No numeric patch. Original old-save and actual final-loaded 1.6/1.7 seasonal/merchant economy remain OPEN. See formal balance document for premises and source links.

## AGRI-MO-PUMPKIN-FOOD-EXCLUSION-20261025 — approved Western recipes disconnected on Agriculture

**Latest owner decision:** author approved disabling original MO pumpkin fritter 2 and milk-soup 2 cooking Bills and ordinary finished-food trade 2, in addition to the prior pie3. Agriculture production source: `Patches/MO_PumpkinFritterMilkSoupExclusion.xml`; canonical rationale/evidence: `Docs/Design.md`, `Docs/Research/MO_CultivatedCropAudit_20261023.md`. Six MO-only `MayRequire` XPath operations (four recipeUsers Replace, two tradeability None Add) match all six original MO 1.6 targets; no crops, original raw pumpkin trade/loot, generic workstations, Def identity, medicine or assets edited. **Pumpkin and carrot cultivar/growth/era decisions remain HOLD.** User original 1.6 XML targeted static apply 6/6 PASS; original RimWorld 1.6/1.7 loaded gameplay and saved jobs still unverified. No new test launch, release or Steam publication. The previous `AGRI-MO-PUMPKIN-RECIPE-AUDIT-20261025` candidate-only status below is historical and superseded for these four Bills/two finished foods.

## AGRI-MO-PUMPKIN-RECIPE-AUDIT-20261025 — four Bills reviewed, change not authorized

**Source audit:** Original MO 1.6.2.2 two PumpkinFritters and two PumpkinSoup cooking Bills, both finished meals, and carrot/pumpkin crop paths inspected. Fritter recipe has no oil or sugar even though finished item describes a glazed sweet biscuit; soup description implies spices but actual recipe is pumpkin+milk. Both meals have their own mood/taste effects and ordinary trade remains possible. **Recommended but not author-approved:** disconnect the four Bills and ordinary trade for the two finished foods while preserving original Defs/other cuisine/crops. No gameplay source changed. Original MO carrot `growDays=3.25` is overridden to **6** by Agriculture's actual `Compatibility/MedievalOverhaul/Patches/MO_ExistingCropGrowthBalance.xml`; pumpkin stays `growDays=8`. Both crop/cultivar balances remain HOLD; original 1.6/1.7 native unverified; no image/test/Steam work. Formal evidence, four Bill IDs, historical references and next decision are in `Docs/Research/MO_CultivatedCropAudit_20261023.md`. Do not treat a recommended exclusion as implemented or ask author to relay other-owner decisions.

## AGRI-MO-CAKE-PIE-20261025 — ordinary Western cakes/pies disconnected; raw produce preserved

**Owner/source:** Agriculture `Patches/MO_WesternCarrotCakePumpkinPieExclusion.xml`, formal `Docs/Design.md` and `Docs/Research/MO_CultivatedCropAudit_20261023.md`. Latest author-approved scope: remove normal creation Bills for carrot cake x3/6/18 and pumpkin pie x3/6/18 (6 explicit recipeUsers) and finished-meal ordinary trader stock (2 ThingDefs, tradeability None). **Carrot/pumpkin sowing, original plants/harvest items and trade, pumpkin fritters/soup, existing Def identities, and four MO medicinal herbs stay unchanged.** Do not treat a crop HOLD as crop removal or restore the two Western food chains in Japanization/cuisine changes. Original MO 1.6.2.2 ZIP 8/8 exact XPath targets, static only. Original RimWorld 1.6/1.7 final loaded bills, third-party recipes/stock and old saves remain OPEN; no native run or Steam action.

This file is the authoritative coordination surface for separate chats, agents, and workstreams working on **Ancient & Medieval Japan Grains**.

Use it instead of asking the user to manually relay messages between workstreams.

## Working rule

At the start of AMJ Grains work:

1. Read `AGENTS.md`.
2. Read this file from `main`.
3. Check whether there are OPEN / IN PROGRESS items relevant to the current workstream.
4. Perform the work directly when possible.
5. Update this file when an item's status, owner, blocker, or result materially changes.
6. Put confirmed specifications and implementation decisions into the actual source-of-truth files as well.

This file is for coordination only. It is **not** the final design document.

## Source of truth

Current primary design source:

`Docs/Design.md`

Confirmed implementation details must also be reflected in the relevant C#/XML/Defs/Patches/localization files.

## Repository boundaries

AMJ-related standalone mods may have their own repositories and their own authoritative coordination logs.

When work belongs to another repository, create/update the handoff in that repository's:

`main:Docs/Coordination.md`

Do not use the user as the transport layer between repositories.

## Status vocabulary

- **OPEN** — needs work
- **IN PROGRESS** — currently being investigated or implemented
- **BLOCKED** — waiting on a specific prerequisite
- **DONE** — completed and reflected in the proper source of truth
- **ARCHIVED** — retained for history but no longer active

## Suggested workstream labels

Use whichever label best fits the task:

- **Core/design**
- **Agriculture/XML**
- **Food/cooking**
- **Buildings/furniture**
- **Research/progression**
- **Compatibility**
- **C#/framework**
- **Art/graphics**
- **Localization**
- **Testing/release**

## Current coordination items

### GRAINS-STORAGE-CATEGORY-20261019 — Foods直下のGrains穀物へMO穀物・小麦粉を一本化

**Owner:** Grains XML/UI compatibility. **Status:** SOURCE IMPLEMENTED, structural/source validation only; latest loaded-game storage tree, MO milling/brewing and Processor Framework runtime test OPEN. **2026-10-19 follow-up:** Move MO processed wheat `AMJC_Wheat` exclusively to `AMJC_GrainKernels` (MO raw wheat sheaf remains `AMJC_GrainSheaves`), while keeping `DankPyon_Cereal` as an empty compatibility category Def. Replace MO 1.6 grinding, ale wort, windmill/watermill and legacy stockpile category filters with direct `AMJC_Wheat` authorization; never route sheaves or all grains into generic MO processing. Previously integrated: `AMJC_Grains` under `Foods`, MO `DankPyon_Flour` only under `AMJC_GrainFlour`, explicit flour filter bridges. Canonical source: `Docs/Design.md §2.11/§6.5`, `Compatibility/MedievalOverhaul/Patches/ZZ_GrainCategories_MO.xml`, and test assertions. No fresh actual game run, no Steam action. Third-party mods adding to obsolete MO cereal category may require separate compatibility.

### GRAINS-PLAYTHROUGH-BEFORE-17-20261017 — flour-food play first

**Owner:** Grains gameplay/acceptance. **Status:** HOLD RimWorld 1.7 new testing pending ordinary-game flour-food confirmation. **Author decision (2026-10-17):** Verify the existing crop → harvest → thresh → hull where applicable → mill → cooked/eaten flour-food loop in real gameplay first, resolve concrete gameplay faults, then run a proportionate consolidated current-source 1.7 compatibility test. The past source-scoped 1.7 24/24 is retained but does not cover later changes. Do not auto-start native tests, add another verification matrix, declare 1.7 supported, or release as a result of this note. Canonical acceptance sequence: `Docs/GrainsProfileTesting.md` § “Author-ordered validation sequence”. No runtime tests or production changes performed by this scheduling update.

### GRAINS-FLOUR-SCOPE-20261013 — food chains / remaining native game and visual review

**State:** V1 nine active foods and four flours are production-source implemented, with earlier 1.6 tests as scoped in `Docs/GrainsProfileTesting.md`. Remaining: after-change MO and 1.7 loaded results, real ordinary-colony cooking/Work/mood and visibility. Current specs `Docs/Design.md` and `Docs/Balance/Crops/SevenGrainFoodChainAudit.md`; detailed historical status archived.

### AMJ-011 — Barley post-harvest grain graphics

**Owner:** Art/graphics. **Status:** PRODUCTION WIRED / loaded-game visual and high-resolution source review OPEN (2026-10-19).

The 2026-10-10 author-approved hulled-barley source is preserved at `Art/Sources/Things/Item/Resource/AMJC_Barley/BarleyInHull/BarleyInHull.png` (Git blob `2cb0f5eb972e3b0f574ae61c7b4150336c5adbff`); the then-current three identical 256px derivatives (blob `b047aaccdbbc3f31c3a0be3eb57f4077eb42c3d4`) are historical. The 2026-10-11 author-pushed 128×128 single-`_a` grain sprites include dedicated edible barley `AMJC_Barley`; PR #40 wired the edible-barley texPath. **Do not list edible barley as having no production image.** The corresponding image's high-resolution source/final visual acceptance and ordinary-zoom game display remain to be confirmed, without changing gameplay, DefNames, or accepted crop/sheaf art.

### AMJ-017 — Grain-processing equipment imagery / manual millstone stone materials

**State: IMPLEMENTED on Grains main** via [PR #186](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/186), squash commit `65aec174d29aaef0af8f7c5c351abc79bece002f` (2026-10-20). Only non-MO `AMJC_ManualMillstone`: larger 1×1 256px image identical N/E/S/W, Stony material30 + fixed WoodLog20, `CutoutComplex` stone-only tint masks, unchanged brown handle. MO continues using `DankPyon_Millstone`. Author provisionally accepted the actual game appearance; do not regenerate it speculatively. **Exact 1254×1254 source is now preserved** at `Art/Sources/Things/Building/Production/AMJC_ManualMillstone/ManualMillstone_TrimCandidate1254.png`, blob `8235ec7d4ca298c1a9b7e30cbdd3524e22d20777`; earlier `ManualMillstone_Reference1254.png` stays separate.

**Native evidence:** First [#39273905572](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39273905572) demonstrated four stone-material colors with fixed brown handle in a real Vanilla EN screenshot; the historical Japanese startup SIGSEGV made that bilingual job FAIL. Later [#39303924920](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39303924920) Vanilla/CCTO V1 gameplay **20/20 Pickle PASS** had unrelated common-food texture errors. Latest [#39391591589](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39391591589) following main food-image path fix is **4/4 native EN/JA × Vanilla/CCTO UI PASS, raw Unity ERROR0, exit0**; former texture ERRORs and Japanese crash did not recur. Not a MO, 1.7, Steam or full Bill acceptance. No need to rerun unchanged stone visuals. Formal source `Docs/GrainsProfileTesting.md` and mill XML/artwork.

### AMJ-013 — Wheat grain graphics

**Owner:** Art/graphics. **Status:** PRODUCTION WIRED / loaded-game visual and high-resolution source review OPEN (2026-10-19).

`AMJC_Wheat` now uses the dedicated 128×128 author-pushed single-`_a` wheat grain icon wired by the 2026-10-11 PR #40 (main `73b1ee429fb5690a333abbcef2857dba69d47456`). The older note that it reuses the edible-millet stack sprite describes the **pre-2026-10-11** interim state, not current production. Do not create a second wheat-grain texture merely because that note was stale. Final in-game zoom, source provenance/archiving, and style acceptance remain OPEN. MO wheat sheaves still use the conditional MO-owned `DankPyon_RawWheat` provider with Grains graphical overrides. No balance or recipe change.

### AMJ-019 — Millet hulled / edible grain production artwork

**Owner:** Grains Art/graphics. **Status:** BOTH PRODUCTION WIRED / edible-millet master provenance and loaded-game visual check OPEN (2026-10-19).

**Current source:** The author-pushed 128×128 `Textures/Things/Item/Resource/AMJC_Millet/Millet/Millet_a.png` (with `Millet_a.dds`) exists on `main` as of commit `5111980e13983125fc50b1368b4800fa61429090` (2026-10-11). `Defs/ThingDefs_Items/Items_StageA_Grains.xml` references `Things/Item/Resource/AMJC_Millet/Millet` for `AMJC_Millet`; `Graphic_StackCount` uses the single `_a` appearance, as confirmed in `Docs/Design.md §12.1.4`. The images are shared by Awa/Hie/Kibi after hulling. **The previous OPEN request to make a new edible-millet image is superseded; do not regenerate it.** `Art/Sources/Things/Item/Resource/AMJC_Millet/Millet/` currently contains only `.gitkeep`, so a verified high-resolution original remains to be recovered, if it exists. A 128px derivative must not be misrepresented as an original. Normal-zoom actual-game appearance and final visual acceptance are not established by the presence of a PNG/DDS.

**Accepted in-hull history:** On 2026-10-10 the author approved the boxed `AMJC_MilletInHull` PNG; PR #31 preserved its 1429×1100 source at `Art/Sources/Things/Item/Resource/AMJC_Millet/MilletInHull/MilletInHull.png` (Git blob `61d6720c7354107424400dd9abf0edc0a118b859`) and replaced the old three-stack derivatives (then blob `039c6f11bbe5426d169b011a706cfcb3e6802c2f`, merge `604ac338ac344981c101d3fbeb186e2c239bbc69`). The later 2026-10-11 main commit `5111980e13983125fc50b1368b4800fa61429090` superseded those three production slots with one 128×128 `_a`; PR #38 (main `a27c19544e86375e7aa6ac108b63ef9129ce4a85`) updated tests to allow one `_a`, with optional contiguous `_b/_c`. Accepted 14 crop states and 5 sheaf images remain unchanged.

**Next:** Use the already-loaded grain-icon Pickle checks and a proportionate regular-zoom private-play inspection when Grains' existing author-ordered gameplay review runs. Resolve only concrete defects. Archive a genuine edible-millet high-resolution master only if its identity can be demonstrated, not by upscaling the current PNG. New art remains needed for the five specific dishes, dedicated processing table/non-MO stone mill, and flour quantity variants; those are distinct workstreams.

### AMJ-018 — Current private-play and eventual public readiness

**State:** OPEN for normal-zoom visual acceptance, original/master provenance and release gates; previously completed tests do not certify later production. This numeric harvest-threshold change does not touch art or distribution. Current `Docs/Design.md`, `Docs/GrainsProfileTesting.md` and `Docs/WorkshopPackaging.md`; former progress and artifact details archived.

### TEST-005 — Core + Environment 自動ゲームプレイ評価

**Requested by:** author (2026-10-05 JST)  
**Owner:** Core gameplay balance / cross-mod testing  
**Status:** IN PROGRESS — test implementation added; combined RimWorld runtime PASS pending

「Core + EnvironmentだけでAMJ固有の遊びが成立するか」について、機械判定できる部分を手動プレイへ残さず自動化する。

実装済み:
- Pickle Stage Aへ `Stage A crops preserve distinct gameplay roles` を追加し、短期作・収量・寒冷適応・痩せ地適応・研究ゲートの役割差を関係式として回帰検証する。
- Stage Aサマリ契約を7シナリオから8シナリオへ更新した。
- 最新GitHub Stage A validationは commit `4c77664f6a1cb09e1d89349cda0717c2d4413e48` でGreen。
- Environment側に実マップ土壌 + Core作物の統合Quickstartを追加し、Thin Soilの播種差、地図加重の肥沃度成長倍率、気候勾配を自動評価する。

手動へ残すのは、見た目、UI自然さ、テンポ、煩雑さ、「差が存在する」ことを超えた面白さのみとする。

Durable design source: `Docs/Design.md` section **Core + Environment 自動ゲームプレイ評価方針**, commit `9eace339ba63e7e23d60d4a13c46727229d2be41`.

**Next action:** ローカルRimWorldでCore `run-e2e.bat` の新8/8ゲートと、Environment `run-runtime-tests.bat` のCore+Environment統合プロファイルを実行し、実行時PASS/ERRORゼロを確定する。合格後は数値条件の手動再確認を要求しない。

### ART-SOURCE-ARCHIVE-023 — Original-source inventory

**Owner:** Art/graphics. **Status:** IN PROGRESS for the historical 2026-10-07 archive only. The previous **19 historical roles / 12 archived / 7 pending** are a *legacy-source recovery snapshot*, not the current crop-family count. Current 7 mature + 7 immature + 5 sheaf production states are mapped separately in `Docs/References/GrainsCropSourceManifest.json`. In this commit the 3 current existing mature source files are reused unchanged and 16 previously unarchived high-resolution crop/sheaf source inputs are added verbatim. Earlier accepted Awa/Hie/Kibi immature originals are preserved; their latest outlined versions have distinct source filenames. Image production and art approval are not release or game-rendering evidence.

**Next:** recover only genuine missing *historical* accepted masters when author-approved identity can be proven; never replace an original with a 256px derivative.

### ARCH-MODULAR-001 — Grains modularity and optional MO dependency

**Owner:** Grains agriculture/compatibility, Scenarios owner for starting scenarios. **Status:** IN PROGRESS for current-revision fresh-start real-game verification; mandatory MO dependency removal implemented. Old-save migration is out of scope.

Canonical design: `Docs/Design.md §2.8` (Base/MO ownership boundary), `Docs/GrainsDependencyAudit.md`, `Docs/GrainsProfileTesting.md`, `Docs/ScenarioExtraction.md` and production XML. Vanilla + Grains has independent wheat, grain processing and minimum flour-food paths; MO presence selects `Compatibility/MedievalOverhaul` and MO absence selects `BaseWithoutMO`. No duplicate default wheat/flour/millstone providers on MO profile. **2026-10-14 correction:** Pre-publication old-save compatibility is not applicable. Grains has no legacy New Village provider, scenario loader conditions, or scenario-only fixture; Scenarios exclusively owns the three starting Defs and new-start tests. Grains base/MO projections must contain zero such Defs; its runtime profile scope remains grain-only. Canonical contract: Docs/ScenarioExtraction.md and Docs/Design.md. No runtime/Steam completion is inferred.

**2026-10-10 author decision:** The old mandatory MO declaration is obsolete. Remove MO from Grains `About/About.xml` hard dependencies and retain optional `loadAfter` ordering; align README, Workshop/2game/metadata and static/profile regressions. Preserve current production `packageId=sucro.ancientmedievaljapan.core`, AMJC DefNames, MO-only wheat/flour/millstone/Straw interoperability, and CCTO optionality. This is a metadata/compatibility-contract change, **not** a claim that MO can safely be removed from a running historic Core/Grains save.

**Evidence boundaries / next work:** Four-profile 24/24 fresh native-job Pickle and ERROR 0 existed on earlier source; later climate/sow diagnostics and this dependency-metadata change have no new identical exact-head real-game matrix evidence. **2026-10-10 author clarification:** Grains has never been installed or used; no Grains saves need migration. Old-Core save load/re-save, mid-save add/remove and removing MO from hypothetical existing saves are removed from the release gate. The synthetic tools and historic notes remain archive-only, not active tasks. Graphics/normal UI, approved text, seasonal growth, fresh-start four-profile checks and publication decisions remain OPEN. No Steam upload or author-visible game test is claimed. Obsolete intermediate migration and PR step history is retained in Git history; formal current sources above supersede its prior MO-required instructions.

### ARCH-UPLAND-RICE-001 — One rice Plant, cultivation environments differ

**State:** Accepted: reuse Vanilla `Plant_Rice` across upland and future paddy, preserving shared grain post-harvest, without creating a second PlantDef. Rice Cultivation/Waterworks owns paddy/water effects. Existing implementation in `Patches/UplandRice.xml`, `Docs/Design.md §4.2.2`, `Docs/Balance/Crops/RicePostHarvestProcessing.md`. Former experiment and open test chronology archived; current in-game seasonal behavior and paddy integration OPEN.

### AMJGRAINS-LOC-APPROVAL-20261008 — accepted Japanese, paired English

**Owner:** Grains descriptions/localization. **Status:** JP approved and paired EN strings committed to source; static parity regression added; runtime text load/ERROR-0 not executed.

The author approved the existing 15 Japanese historical description/Recipe work-string candidates except two precise wording adjustments: use **AMJGrains** rather than Grains in player-facing prose, and replace the ambiguous wheat phrase `実は食材になるほか挽いて粉食に利用できる` with `小麦の穀粒は食事の材料に使え、石臼で挽けば小麦粉として粉食にも利用できる`. Apply the other approved Japanese text without extra historical claims. `Docs/LocalizationHistoricalReview.md` §2/§6 now records exact Japanese/English pairs. Source scopes: 5 shared grain-flour/food ThingDefs, 5 shared RecipeDefs, 4 non-MO wheat/millstone ThingDefs, one non-MO wheat milling Recipe. Japanese DefInjected describes all 15 and jobStrings for six Recipes; default English XML carries matching descriptions/jobStrings. Product labels, names/DefNames, gameplay XML, MO provider ownership and historical MO fixture unchanged. `Tests/test_grains_localization.py` checks exact docs–Japanese–English correspondence. Remaining: game-loaded language switching and four-provider Pickle/ERROR-0, extra plant prose review and images.

### GRAINS-SEVEN-CROP-BALANCE-AUDIT — release synchronization open (2026-10-15)

**State:** Existing seven-grain values and fertility-speed model remain as source. MO Lentil/Onion historical comparisons were superseded by Agriculture crop exclusion (2026-10-24). Current MO reference balance, wheat processing labour and fruit harvests OPEN; ownership and numeric premises in `Docs/Balance/Crops/SevenGrainFoodChainAudit.md` and `Docs/Balance/Crops/FertilityGrowthModelReview.md`. Historical status archived.

### GRAINS-RICE-POSTHARVEST-IMPLEMENTATION-20261008

**State:** Source implements rice sheaf→paddy→brown/white rice via existing DefNames; white rice batch 10→8 and legacy Bills preserved. Food/Work/old-save and 1.7 acceptances are distinct; current scope in `Docs/Design.md`, `Docs/Balance/Crops/RicePostHarvestProcessing.md`, `Docs/GrainsProfileTesting.md`. Detailed chronology archived.

### GRAINS-GENERATED-ART-20261010 — crops and sheaves

**Owner:** Grains art. **Status:** SOURCE INTEGRATED / STATIC VERIFIED; fresh-game rendering OPEN.

PR #20 (`cd52a3d9e15467d2f6ebc1bdaac63cda84f98cbd`) wired seven immature and seven mature crop paths, five bundle families, Vanilla upland rice and MO wheat conditional graphical override. Latest immature outline is #4D4E3C with upright millet. The 2026-10-10 source archive preserves or maps all 19 source states via the hash-locked manifest (16 new sources, 3 preexisting originals). The author's crop image checks are acknowledged; no re-generation or repeat approval request. Static PNG / Stage A / Workshop tests are distinct from loaded-game visual acceptance.

**Next:** fresh-start screen/render inspection across actual Vanilla and MO providers, address only concrete sprite/UI failures; independently complete postharvest **millet in-hull and edible grain** (AMJ-019), barley/wheat item images and processing station art. No Steam release or real-game PASS claimed.

### GRAINS-NATIVE17-20261013

**Owner:** Grains runtime/CCTO compatibility. **State:** AUTHOR-APPROVED CCTO DETECTION FIX; 1.7 FOUR-PROFILE NATIVE PASS. Production base `2f5b8132eb1a32bc3cfa01625f895dc045f70fc8` plus six FindMod title selectors: Vanilla/Vanilla+CCTO/MO/MO+CCTO each 6/6, total 24/24, native ERROR0, complete PID logs and no skipped/flaky cases. The prior 21/24 extension failures are superseded only for this corrected source; original logs/hashes remain in the evidence. Old shared-zone recovery issue was already fixed by #77.

Author approved only two production XMLs accepting legacy English and current bilingual CCTO names; all values/recipes/art/metadata and 1.6 test paths retained. Meaningful regression rejects either lost title across all six selectors. Canonical sources: `Docs/Balance/Crops/ColdTolerance.md`, `Docs/GrainsProfileTesting.md`, existing `Docs/Testing/Native17-20261013.json` correction field. No user relay. Native frost/death, natural climate, current 1.6 runtime, old binary-title provider, legacy saves, every stove variant, visuals/balance and release remain OPEN.

### GRAINS-NATIVE16-20261015 — current V1 four-profile native functional acceptance

**Owner:** Grains runtime + Project isolated host. **Status:** SAME CURRENT GRAINS SOURCE `f339d5fe98a56aba5cae9652c2546c0162b79cc3` **FUNCTIONAL 40/40 PASS** on actual Linux RimWorld 1.6.4871 rev600; **ORIGINAL MO/SYR MP3 STRICT ERROR0 NOT PASSED; RELEASE HOLD**.

- [Project Actions #38682563490](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38682563490): Vanilla 10/10 + Vanilla+CCTO 10/10, unmodified providers, native ERROR0, no skipped/flaky, exit0.
- [Project Actions #38682422683](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38682422683): MO 10/10 + MO+CCTO 10/10, native ERROR0, no skipped/flaky, exit0 **only in a disposable cloned game with two external provider MP3 sounds converted to WAV**. Original SYR/MO media, all production Mods, native jobs and source assertions are unchanged; original-media FMOD release acceptance remains OPEN.
- The ten-scenario contract includes actual harvest/processing/flour food Bills, 12 common-food Bill combinations, alternate bench/stove jobs, wrong-ingredient rejection and pawn Thought/ingestion. Exact premises, exclusions, earlier source-scoped Vanilla 20/20 and MO 9/10 evidence, and separate WAV/original-provider limits: `Docs/GrainsProfileTesting.md`.
- **2026-10-15 UI update:** [native #38686645068](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38686645068) on Grains `46eb7f8a468e2387880742fb4c0c4a4e98799a59` achieved Vanilla/CCTO × EN/JA **4/4 actual loaded-language assertions**, ERROR0 and **16 screenshot files**. However, real Japanese screenshots show **missing kana/kanji** despite loaded translations; Japanese **visual UI is FAIL / host-font diagnosis OPEN**. English InfoCard displays plausibly. Formal contract and evidence boundary: `Docs/GrainsProfileTesting.md`. Remaining separate gates: missing Japanese glyphs, ordinary-map-zoom art, MO-language profiles/original-media ERROR0, private play, current 1.7 and Steam publisher/subscriber verification. Neither native UI runs nor these notes alter any Grains gameplay/art.

### GRAINS-MO-JP-INFOCARD-20261016 — private WAV font UI 4/4; original MP3 strict FAIL

**Owners:** Grains developer-only loaded bilingual UI fixture; Project isolated Linux 1.6 test host and existing temporary MO/SYR audio clone. Grains source `e49717489333a1367500c37feec1ab3a8408d566`. Original MO media UI [#38783420759](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38783420759) reached mo-en Pickle 1/1, exit0 and four PNGs but logged the **two known third-party FMOD Unsupported file** Unity ERRORs; original strict full MO matrix **FAIL**, the other three profiles not run. **Separate** verified one-off `/amj grains-ui-mo-wav-font-16` converted only temporary complete-game copies of the original pinned SYR/MO MP3s to PCM16 WAV and used the same approved child-only hash-verified Noto JP font. Actual [#38784551164](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38784551164), artifact 11998061567: **MO EN/JA + MO+CCTO EN/JA UI 4/4 Pickle, 0 Unity ERROR, game exit0, 16 original InfoCard PNGs**, Japanese 12/14/21 style assignment and non-placeholder 稲21×21, あ19×19, 中20×21 verified; original Japanese names/descriptions and section separators visibly legible. Project test-only PR #385 `05c4f53c`, WAV lane PR #388 `4c32ebdb`; no Grains product XML/texture/translation edits, Steam, real game or original-provider mutation. **Do not promote WAV-clone success to original-MP3 audio or release approval.** Exact owner evidence: `Docs/GrainsProfileTesting.md`; original cross-workstream audio correction remains Project/FMOD-owned.

### GRAINS-LOADED-JP-RENDER-20261016 — private native UI font display restored
**Final gate:** [#38767986561](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38767986561) validates actual Unity-visible Noto, 15 legacy Verse font slots, non-placeholder 稲/あ/中 21×21/19×19/20×21 px; 4/4 native UI PASS/ERROR0 and 16/16 PNGs byte-identical to the visually checked repaired #38766667191. This final validation is automatically required for Japanese private-lane tests and remains distinct from stock baseline and production/Windows acceptance.


**Owner:** Project native Linux host for rendering implementation, Grains owns unchanged loaded-InfoCard UI scenario. **Status: visually legible in the isolated *opt-in* `/amj grains-ui-font-16` lane; ordinary `/amj grains-ui-16` is not implicitly updated.** Actual [Project #38766667191](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38766667191) used Grains test source `10cca5cba89eb2bed01d3a904996b4844843f545`: original EN/JA × Vanilla/CCTO loaded-InfoCard Pickle 4/4 PASS, Unity ERROR0, exit0, 16 screenshots. Project's rootless child-only OS-font mount exposes verified temporary official Noto Sans JP to Unity: Japanese live font list grows 16→17 including Noto and all sampled 稲/あ/中 native quads have 19–21px dimensions, compared with 2×2 previous placeholders. Direct actual Japanese InfoCard screens for 稲・稲束・籾・米 display Kanji/Kana; English PNGs 8/8 byte-identical to previous, Japanese PNGs 8/8 changed. Strict regression source gate belongs to Project; old fontconfig/HOME and direct filename methods remain rejected. Formal Grains result: `Docs/GrainsProfileTesting.md`; formal Project host route: `Docs/GoldenPaths/PersistentHostIntegrations.md`. **No** Grains production XML/art/recipe, original game binaries, persistent system font, player save, Windows or Steam change; other loaded UI layout/clipping/art acceptance remains open. Previous failure entries are historical, not current status for this private rendering lane.
