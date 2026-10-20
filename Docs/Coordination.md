# AMJ Grains Coordination

## GRAINS-KOGASHI-CONFECTIONERY-GATE-20261021 — intermediate deferred until useful common ingredient

**Owner decisions confirmed 2026-10-21:** Tea's existing rice/optional-barley → kogashi-yu stays one direct Cooking Bill; do not add standalone `こがし`, roasted-grain intermediates, or separate roast/grind Bills for the low-effect drink. Grains alone owns future edible kogashi and confectionery. At a future sweets design, evaluate a shared kogashi ingredient **only if multiple meaningful uses justify increased labor, Bills, storage, and compatibility costs**; a lone sweet does not force a split. If later warranted, roasting+grinding remain one Grains processing step, Tea retains drink ownership and standalone rice fallback, optional Grains-enabled path avoids duplicate drink Bills, and migration is separately audited. Confirmed specification: [Grains Design](Docs/Design.md), [Tea Design](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Culture-Tea/blob/main/Docs/Design.md). **No initial nine-food scope/production XML, existing recipes, or native test state changed.**


## GRAINS-GRAINPROCESSING-WORKROLES-20261020 — independent processing work and optional WorkRoles grouping

**Owner:** Grains independent processing WorkType and optional WorkRoles integration. **State:** implementation main [#220](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/220), 1.6 Vanilla/CCTO native Grains 20/20 ERROR0 [Project #39447209377](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39447209377), and **actual original-source WorkRoles + Vanilla 1.6 native 10/10 PASS, original Unity ERROR0, game exit0** [Project #39453821019](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39453821019), artifact `12280365057`. Exact Grains E2E source head `51f17c4b2e07afededecd15bf613437bfd4a2a0a`, WorkRoles upstream `e5d29302c3922a60a324f57a3c2ecd5cc02d87ed`; loaded WorkRoles Farmer/Grower RoleDefs and actual seeded RoleStore each contained `AMJC_GrainProcessing`, real alternate grain processing/cooking Bills succeeded. Grains test [#224](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/224) merged to `f9ea1ee05a01588330aea218e59a64531a3c1bbe`. WorkRoles 1.6 actual source-built DLL, **not Workshop-distributed binary**. The first Project [#39452670705](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39452670705) **failed**, caused by the isolated provider staging omitting `0MultiplayerAPI.dll`, the test evaluating seeded roles too early, and preexisting log classifier reference bug. Those were corrected by Project [#871](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/871) and Grains #224; original failure evidence is preserved, not retroactively PASS.

**Additional actual 2-pawn work AI acceptance (2026-10-21):** [Project #39460052749](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39460052749), artifact `12282906174`, exact Grains head `e34db95300f07d9d101fb1da9089fc450cf51c92`, upstream WorkRoles `e5d29302c3922a60a324f57a3c2ecd5cc02d87ed`: original RimWorld 1.6.4871 rev600, WorkRoles activated, **10/10 native Pickle PASS, game exit0, Unity ERROR0, skipped0, flaky0**. A newly added opt-in assertion put two capable pawns on Grower roles, supplied two separate 10-unit stacks and two `AMJC_ThreshMilletBulk` Bills on separate AMJ processing tables (full bench and spot), advanced the ordinary pawn tick loop **without `StartJob` or `playerForced`**, and observed the two pawns simultaneously holding **different** real `DoBill` jobs. The previous WorkRoles RoleDef+seeded-store checks and all Grains jobs passed in the same run. [Grains PR #226](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/226) merged main `94ccc6f16334b25f3942f02bf17ef3087bfe0238`. No recipes, yields, work times, or production assets changed.

**Remaining OPEN:** existing WorkRoles **old saves** with player-customized role migration; autonomous job selection under actual crop-growing competition (the fixture intentionally cleared planted crops and used Grower roles); WorkRoles Farmer-role autonomous work (role source/seed membership passed); MO-owned stone/Cooking on this WorkType build; 1.7 and installed Workshop WorkRoles DLL. The parallel test verifies **simultaneous assignment to independent nonforced jobs**, not two completed long-running Bills or sustained factory throughput. Source of truth: `Docs/Design.md`, XML; evidence `Docs/GrainsProfileTesting.md`. No Steam publication or recurring test authorized.


## GRAINS-V1-NATIVE-20261020 — latest nine-food acceptance and MO patch outcome

**Status:** RimWorld 1.6 9料理/18Bill `e3ec85d7` Vanilla/CCTO **20/20 native PASS, ERROR0** ([Project #39403725666](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39403725666)); MO+CCTO **10/10 gameplay PASS, Projectの元ログ厳密ERROR0はFAIL** ([Project #39404691834](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39404691834)), because original external MO/SYR FMOD audio produces 2 errors. **2026-10-20 作者最新判断: Grainsが変更していない既知の外部FMOD2件はGrains初回公開の阻害要因から除外。修正・音声だけの再テストは不要。** ただし歴史的なProject実行FAILは保持し、Grains本体のXML・Recipe・画像・実喫食ERRORや新種のERRORは引き続き修正対象。確定した例外の正本は `Docs/Design.md`、SHA別証拠は `Docs/GrainsProfileTesting.md`。 Prior Grains MO XML XPath errors 6 were corrected by Grains [PR #203](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/203) and verified absent in both corrected-source MO+CCTO runs. The optional category XPath gate in Project native error classification was strengthened in [Project PR #838](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/838). **Newer actual source verification:** Grains [PR #205](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/205) added the white-porridge 1x/4x native Bills (Vanilla/CCTO 20/20 [#39403704297](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39403704297)). Newer Grains `7e1c96b1...` with that test: [MO+CCTO #39412070824](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39412070824) **10/10 Pickle PASS, exit0; only external FMOD2, strict Project FAIL**. [MO alone #39412620388](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39412620388) original **9/10 Pickle FAIL**, Houtou actual mood count0, plus FMOD2; not exempt. Narrow Grains test-fixture completion fix [PR #214](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/214) main `b99ad22f12a8ef494663ca71968fbb35547da422` waits for real game-awarded tasteThought before interrupting Ingest; subsequent [MO #39413523334](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39413523334) exact branch head `32fd4f838b5841add4c7cf9f4b8e97d89f6dcc39` **10/10 native Pickle PASS, exit0, only external FMOD2, strict Project FAIL**. Production recipes, moods and XML unmodified. The two profile successes are **not one same-head 20/20 run**; do not relabel Project outcomes or add audio-only tests. Canonical premises in `Docs/GrainsProfileTesting.md`. First MO+CCTO attempt [#39404491728](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39404491728) failed slot3 preflight snapshot; separate real run succeeded functionally. **No Steam publication, native 1.7 certification, or automatic game launches.** Precise individual premises and source SHA in `Docs/GrainsProfileTesting.md`. The older coordination 11-food counts are historical and superseded by main `Docs/Design.md` 9-food scope.

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

**Owner:** Grains Food/processing. **Status:** V1 implemented; current native **Vanilla/CCTO 1.6 food-loop PASS**, MO/1.7 and normal-map visuals remain OPEN. Current production: four flours; four flour-specific dishes, three common foods and **two white-rice dishes (9 foods / 18 normal cooking Bills)**. Retired brown-rice-only RecipeDefs/ThingDefs retain their identities but no cooking-station recipeUsers. Legacy rice flour/dumpling Defs survive for save references, not active production. Canonical production: `Docs/Design.md §5`, `Docs/Balance/Crops/SevenGrainFoodChainAudit.md`, `Docs/LocalizationHistoricalReview.md`, relevant Defs/Recipes. Current accepted JP/EN text and graphics paths are there; original grains and placeholder-specific-meal images remain protected from unapproved changes.

**2026-10-20 resolved common-food loading:** [Grains PR #198](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/198), main `d4fb9eca74f1bc710570eceeeb68f36bd0cced11`, corrected `Graphic_StackCount` `texPath` directory for the three shared meals and four brown/white-rice meal variants. Original accepted PNG/master images, Graphic class, recipes and thoughts remain unchanged. [Project native #39391591589](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39391591589) at `da4e2392e635ee29a91fb4e695c922f5d7e5cdc3`: RimWorld 1.6 Vanilla/CCTO × EN/JA **4/4 UI Pickle PASS, raw Unity ERROR0, exit0**, seven food materials non-BadTex. Previous seven `Collection cannot init` errors resolved; the DDS idea was rejected and leaves no assets. The earlier Japanese Mono signal11 from #39273905572 was **not reproduced** in this fixed-source run; older failure remains historical. Formal spec/evidence: `Docs/Design.md §5`, `Docs/GrainsProfileTesting.md`.

**2026-10-20 current-source actual food-loop automation:** [PR #205](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/205) integrated in main `06d55102253e397ba2b376a78d44bf4b7f03c991`; its test source `46de2d731db4b4661a13af14b56c96765c7fb11e` completed [Project native 1.6 #39403704297](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39403704297), artifact `12255373418`: **Vanilla 10/10 + Vanilla+CCTO 10/10 = 20/20 actual Pickle PASS; both process exit0 and raw Unity `[ERROR]` 0.** Test premises: currently offered 9 food/18 recipe scope; separate harvested grain/sheaf→hull→polished rice→white cooked meal Bill; 12 one/two-input common-food actual Bills; added white porridge single (white rice10→1 dish) and bulk (white rice40→4 dishes) actual DoBills with **test-staged white-rice inputs**, not harvested; actual ingest Mood +3/+1 white meal/porridge and +2 flour-specialty; wrong-input rejection. Loaded legacy brown-only recipes have no `recipeUsers`. Specific recipe-premise/scope in `Docs/GrainsProfileTesting.md`.

**Still OPEN:** MO/MO+CCTO exact current-source native tests; RimWorld 1.7 remains author-held until ordinary-play flour loop review; normal-zoom cooked-food aesthetic assessment, exhaustive execution of all 18 recipe variants, and Steam release. Do not rerun unchanged Vanilla/CCTO merely to grow PASS counts.

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

**Owner:** Testing/release + Art/graphics. **Status:** IN PROGRESS for artwork; private-play source preparation, loaded rendering and release gates remain OPEN. Author prefers private Workshop upload and normal play before public release.

Canonical graphics/state policy: `Docs/Design.md §12.1.2–12.1.4`; current crop/sheaf sources: `Docs/References/GrainsCropSourceManifest.json`; source inventory: `Art/Sources/Inventory.md`.

**2026-10-13 author priority correction — crop and sheaf art deferred:** Keep the adopted seven immature + seven mature planted-state images and all five sheaf images as they are. Do not retouch, regenerate, replace or create new sheaf stack-count quantity variants now. These aesthetic improvements remain future work without a set date, not current prerequisites for other Grains implementation, tests or private play. Retain path/PNG integrity and real-game rendering checks; do not treat aesthetic deferral as permission to ignore confirmed technical defects. Grain, flour, finished food and equipment art are outside this deferral. No production PNG, Def, Recipe or runtime result is changed by this decision.

**2026-10-11 current source state:** Seven immature and seven mature crop images are integrated at ordinary `AMJC_<Crop>` paths. Five adopted sheaf families now use `MilletSheaf`, `BuckwheatSheaf`, `BarleySheaf`, `WheatSheaf`, `RiceSheaf` in both `Textures` and `Art/Sources`. `Dense` was a revision label, not a separate state. Def paths, MO wheat override, source manifest, tool references and existing path validators follow these canonical names. Accepted image bytes are unchanged. Removed 19 redundant production PNGs: ten identical sheaf `_b/_c` copies and nine unused `RawMillet` / `MixedMilletSheaf` / `RawBuckwheat` aliases. Each sheaf currently has one `_a`; genuinely distinct low/medium/high appearances remain pending. Low/Medium source-work folders remain with their families. Historical candidate directories and generation records retain their names; do not rerun historical install scripts to restore superseded paths.

Author-pushed 128px single `_a` grain icons are integrated for millet, buckwheat, barley, wheat, rice-in-hull and edible rice. Dedicated barley/wheat/rice-in-hull Def paths and a visual-only Vanilla `RawRice` patch were integrated in PR #40, main `73b1ee429fb5690a333abbcef2857dba69d47456`. No recipe, nutrient, crop value or gameplay identifier changed.

**2026-10-11 graphics runtime test preparation:** The existing real-provider primary-loop Pickle scenario now checks all nine grain icons on the Unity main thread, including loaded `RawRice` and low/middle/high `Graphic_StackCount` materials. This is a real loaded-game test added to the source, not a Python simulation; exact-head game build, four profiles, ERROR-zero and normal-zoom visuals remain unverified. The six-scenario count and existing runner are unchanged.

**2026-10-11 flour texture wiring:** Five author-pushed `_a` flour images exist. AMJC_MilletFlour and AMJC_BuckwheatFlour now use their dedicated sprites in both profiles; Base AMJC_WheatFlour and MO-owned DankPyon_Flour use the dedicated wheat-flour sprite without creating a duplicate MO flour Def. BarleyFlour/RiceFlour artwork remains unreferenced because production ThingDefs/Recipes are missing. The author's 2026-10-13 correction confirms both are within Grains' five-flour target scope; image-only availability is not implementation. No game balance, new flour items, or artwork bytes changed. Runtime visual/load verification and future quantity variants remain OPEN.

**Remaining work:**
- Vanilla/MO normal-zoom review of the 14 crop stages, five sheaves and grain icons; no current loaded-game graphics PASS is claimed.
- **DEFERRED — future only:** aesthetic improvements to the 14 planted crop states and five sheaves, including distinct low/middle/high stack appearances. Keep current accepted assets unchanged. **Still OPEN separately:** three flour quantity appearances; preserve accepted flour artwork. MO wheat remains `DankPyon_RawWheat`, with Grains graphics; MO flour remains `DankPyon_Flour`.
- High-resolution sources for the newly author-pushed grain icons: **temporarily deferred by the author**; do not relabel production derivatives as original masters or block independent loaded-texture tests on this archival task.
- Five specific cooked foods (`AMJC_Houtou`, `AMJC_Sobagaki`, `AMJC_MilletDumplings`, `AMJC_BarleyFlourGruel`, `AMJC_RiceFlourDumplings`) still need dedicated sprites. Common 飯・粥・粉食 have distinct provisionally accepted and wired game textures (2026-10-19; loaded visuals OPEN). The processing spot, processing table and non-MO manual millstone now have dedicated provisional game textures, each identical across four directions; loaded-game visual verification remains OPEN. Do not treat accepted spot or common foods as still lacking source images.
- Source/runtime/UI/release evidence must retain its actual revision and profile scope. Old-save migration is out of scope because Grains was never installed.

**Verification for sheaf cleanup:** PNG integrity, existing Base/MO reference and historical-contract checks, upland rice, Stage A, source mapping and subscriber-payload checks passed locally. Exact-head PR and push checks are separate merge evidence. No image generation or RimWorld runtime was performed.

**Test audit retained:** PR #41 (`6b6a7a13fb8f55aa4b6249616237fef7ae191d93`) removed image freezes and source-string checks. Source hashes remain provenance only; PNG/reference/XML/provider checks and Pickle remain. Do not restore the deleted archive or art-wording tests.

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

**Requested by:** author (2026-10-08 JST)  
**Owner:** Grains design / Agriculture XML / Testing  
**Status:** Common Plant and processing source IMPLEMENTED; future paddy behavior and latest-version loaded-runtime checks remain OPEN

**2026-10-13 author correction:** The upland/paddy distinction is a cultivation environment, not a second rice species/PlantDef, consistent with the existing unification of uruchi/mochi rice. Grains owns the common `Plant_Rice`, sheaf/hull/`RawRice` and flour; Rice Cultivation will apply paddy-field and water-management modifiers to the same Plant. No new rice PlantDef, edible rice or rice flour. Waterworks remains optional. Canonical updated specifications: `Docs/Design.md §4.2.2/§6.3/§8.1` and `Docs/Balance/Crops/RicePostHarvestProcessing.md`. Existing V1 production XML, crop balance and author-approved description text remain unchanged. Future paddy code, water/yield balance, loaded-game and compatibility checks remain OPEN.

Audit result: the current design still assigned water field, rice plant, paddy rice, edible rice and first-stage rice processing wholesale to Rice Cultivation, while runtime/test balance was fixed to six grains. Those statements are superseded by the updated Design boundary. CCTO already patches Vanilla `Plant_Rice`, so Grains should reuse that Def and must not add a duplicate cold-tolerance extension. Current six-grain environment and Pickle gates remain valid historical/current-implementation regressions but are insufficient for final release after this scope expansion.

**2026-10-13 author decision — batch native UI validation:** Defer the newly revised JP/EN rice-item Info-tab text and paragraph/texture checks until a future **consolidated Grains native regression**. Do not run isolated game launches for each localization revision and do not provision an additional temporary game host for this. Keep loaded-Def JP/EN and normal-zoom visual evidence OPEN, along with the independently recorded 1.7 CCTO test failures. Formal four-item expected layout, provider profiles, evidence and exclusion rules: `Docs/GrainsProfileTesting.md`, 'Author-deferred batch' section. Static-only validation may proceed; this is not acceptance or release approval.

**Current boundary / next work (2026-10-13 audit):** `Plant_Rice` V1 field-growing Patch, seven-crop static tests, sheaf/hull processing and five flour recipes exist in production source; `About` MO hard dependency was already removed. Do not repeat those completed source steps. The shared Plant stays Grains-owned; Rice Cultivation must only extend its paddy growing conditions. **2026-10-13 author-directed V1 localization update:** player label becomes Japanese `稲` / English `rice plant`; the historical text distinguishes upland and paddy growing and clarifies that this mod unifies them as one Plant, with paddy gameplay not yet included. Canonical Japanese/English text and sources: `Docs/LocalizationHistoricalReview.md §7–8`; static parity must pass before merging, while actual game-language UI remains unverified. **2026-10-13 rice prose allocation update:** Plant_Rice (rice-growing history), AMJC_RiceSheaf (medieval tax/rent/commodity money), AMJC_RiceInHull (hulling, brown/white rice and B1 loss), and Vanilla RawRice (Edo diet and beriberi, Takaki's Meiji naval study, Suzuki's oryzanin) each explain their own stage, with short semantic paragraphs. RawRice description is now patched without altering its DefName, nutrition or item identity. Japanese-first and English canonical text: `Docs/LocalizationHistoricalReview.md §7–8/§10`; formal contract: `Docs/Design.md §4.2.2`. Native language UI and save/compatibility checks remain OPEN. Revalidate loaded-game latest-source sow/harvest/processing and optional providers; future paddy source must undergo the independent prior-art and native regression gates. Formal ownership/balance lives in `Docs/Design.md` and `Docs/Balance/Crops/RicePostHarvestProcessing.md`.


**ARCH-UPLAND-RICE-001 implementation slice (2026-10-08):** Vanilla `Plant_Rice` upland-rice patch and Japanese DefInjected label/description, Ground-only sow tag, 5-day/11-yield/0.7 minimum fertility/0.8 sensitivity, 10–42°C growth and 18–32°C optimal values. Existing `RawRice` is preserved, no new rice Def or CCTO extension. New static seven-crop matrix regression runs beside the historical six-crop fixture. Game-loaded seven-crop Pickle, sow/harvest, visuals, four profiles and save gates remain open. Do not claim standalone or remove MO dependency.


**ARCH-UPLAND-RICE-001 E2E follow-up (2026-10-08):** Four real-provider Pickle profiles retain six scenarios but now include `Plant_Rice` in seven-crop environment comparison and native harvest jobs. Loaded checks cover Vanilla `RawRice`, cooking eligibility, Ground-only sowTags, exact rice balance, and CCTO -1°C / extension cardinality. A temporary growing zone validates soil-sow eligibility; actual season-controlled sow remains pending. PowerShell expected scenarios changed with feature names. Rice XML additions now use conditional replace-or-add to avoid duplication with CCTO. Runtime C# compilation, four-profile ERROR 0, art and save checks remain pending.


**Grains complete-food E2E gate (2026-10-08):** Native `CookMealSimple` jobs are now exercised for `AMJC_Millet`, `AMJC_Buckwheat`, `AMJC_Barley`, `AMJC_Wheat`, `RawRice` with real harvested/threshed/hulled inputs in all four-profile features; five extra meal Bills confirm per-grain input consumed and Vanilla `MealSimple` output. Explicit Pickle scenario timeout 270s, internal watchdog 240s and isolated runner 420s are set, retaining six scenarios per profile and strict ERROR gate. Static harness-contract regression added; real game/C# run, images and save compatibility are not yet validated. Next: run the four profiles with installed game in noninteractive rendering-preserving mode; triage failures before creating production art. Do not remove MO dependency.


**ARCH-MODULAR-001 MO1.6 provider archive preflight (2026-10-08):** The supplied `3219596926.zip` yielded 263 parseable XML files in loaded `1.6/Defs`, including the MO wheat/RawWheat/Flour/Millstone/1×+10× grinding Recipe and native Cooking `DankPyon_DoBillsMillstone` WorkGiver. Enhanced the Grains static provider validator to read packaged ZIPs as well as extracted folders and reject mismatched worker/recipe sources, with a synthetic ZIP negative regression. This is real MO **source** evidence but no RimWorld executable/Assembly-CSharp was available to launch four profiles or compile C#; renderer/ERROR/save gates remain OPEN.


**Grains runtime timeout preflight (2026-10-08):** Found Pickle global `-pickle-run-timeout=4` (240s) below the seven-crop/meal `@timeout:270` scenario budget. Parameterized the shared launcher global timeout with legacy default 4min; Grains alone now uses global 7min (420s) and outer 540s process watchdog. Static Windows tooling checks the four feature tags, global cap and process/matrix budgets. The private-desktop matrix still allows 40min, and per-job internal watchdog stays 240s. No game was launched; need real 4-profile Pickle, C# compile, runtime ERROR 0 before art.

### AMJGRAINS-LOC-APPROVAL-20261008 — accepted Japanese, paired English

**Owner:** Grains descriptions/localization. **Status:** JP approved and paired EN strings committed to source; static parity regression added; runtime text load/ERROR-0 not executed.

The author approved the existing 15 Japanese historical description/Recipe work-string candidates except two precise wording adjustments: use **AMJGrains** rather than Grains in player-facing prose, and replace the ambiguous wheat phrase `実は食材になるほか挽いて粉食に利用できる` with `小麦の穀粒は食事の材料に使え、石臼で挽けば小麦粉として粉食にも利用できる`. Apply the other approved Japanese text without extra historical claims. `Docs/LocalizationHistoricalReview.md` §2/§6 now records exact Japanese/English pairs. Source scopes: 5 shared grain-flour/food ThingDefs, 5 shared RecipeDefs, 4 non-MO wheat/millstone ThingDefs, one non-MO wheat milling Recipe. Japanese DefInjected describes all 15 and jobStrings for six Recipes; default English XML carries matching descriptions/jobStrings. Product labels, names/DefNames, gameplay XML, MO provider ownership and historical MO fixture unchanged. `Tests/test_grains_localization.py` checks exact docs–Japanese–English correspondence. Remaining: game-loaded language switching and four-provider Pickle/ERROR-0, extra plant prose review and images.

### GRAINS-SEVEN-CROP-BALANCE-AUDIT — release synchronization open (2026-10-15)

**Owner:** Grains owns seven grains/rice/conditional MO wheat; separate optional [Crop Growth Balance](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Crop-Growth-Balance) owns Vanilla 6 + optional MO 7 existing crop `growDays`. Source extraction and regression protections completed in Grains [PR #101](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/101); never restore those 13 patches to Grains.

**Author decision:** CGB is **specifically a Grains relative growth-period compatibility/alignment Mod**, not a CCTO-style cold-tolerance or independent all-crop real-calendar/yield/labor rebalance. Grains independent values unchanged. The two Mods remain technically independent, but **public release of CGB must coincide with Grains**, not occur earlier. Grains personal playtesting focuses on Grains crops; no separate long-form CGB field play required. The CGB native 1.6 functional Pickle and simultaneous Grains Def checks are documented in CGB `Docs/Testing/Native16-CropGrowth-20261014.md`; MO original-provider FMOD error2 per profile means strict ERROR0 only 2/4. Do not claim all-profile zero errors or revise Grains' own native processing/UI/art release requirements.

**2026-10-19 author-requested millet revision — MERGED main [PR #166](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/166), commit `27bacafa08669d7ffe50164bb6db619334c320ec`; native OPEN:** キビ4日/9、ヒエ7日/18、アワ9日/27（旧5/11・6/12・6/13）。今後の実プレイ/作物比較の候補値。作物Def、設計/分析、歴史Fixture差し替えと静的・実機テスト前提を更新。ソバ・大麦・小麦・稲、収穫後の脱穀30→120日保存、料理やCCTO低温値は維持。24/27生育可能・六穀全種に代表環境は**静的モデルに限定**し、未だ現行ソースのゲームネイティブPASSではない。CGBの既存13作物値はそのまま維持し、比較基準は同Repoの[PR #35](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Crop-Growth-Balance/pull/35)、コミット`adeb10d6ed7517dfed288db2069da1bbde24d631`で同期済み。正本: `Docs/Design.md §4.2.1`、`Docs/Balance/Crops/Millet_Cultivation_Balance.md`、本番XML/Tests。Steam公開未承認。

**2026-10-15 publisher migration:** Author now directs **every future AMJ Workshop upload** via GitHub Actions, including Grains and Crop Growth Balance; YADA is no longer used. Grains' old `.rimignore` and legacy YADA-style validator are not the Actions upload selector. Owner's GitHub Actions explicitly stages runtime/legal files (preflight only), with a separate protected manual SteamCMD/Steamworks uploader pending; CCTO for VPE's existing publisher proof cannot be generalized as a finished Grains uploader. No Steam/2game publication or visibility change authorized. Original Grains gameplay/runtime/release conditions and exact existing workshop identity remain unchanged.

**Open:** Grains release completion and actual jointly scheduled publication; CGB owns its own final package/copy and 1.6 source alignment. CGB yearly nutrition, 10/20/30-day seasons, equalized harvest productivity and Pawn labor studies are no longer prerequisites for joint first release. Do not publish either Mod as a side effect of this handoff.

### GRAINS-RICE-POSTHARVEST-IMPLEMENTATION-20261008

**Owner:** Grains production / E2E / localization. **Status:** Source implementation; full Python CI, C# compile, four-profile Pickle/ERROR-0, save migration and rendering pending.

Implemented Vanilla `Plant_Rice → AMJC_RiceSheaf → AMJC_RiceInHull → RawRice` with the shared processing benches and both individual/bulk Bills; initial work values 15/120 and 10/80, 1:1 conservation, intermediates 120-day/non-edible. Existing `RawRice` remains untouched. MO-only threshing generates `DankPyon_Straw`, Base cannot reference it. Approved Japanese copy is carried into new JP DefInjected and paired English defaults; rice crop description is narrowly aligned. Four-provider Pickle source covers native harvest, real bulk thresh, real bulk hull, Vanilla simple meal and CCTO coexistence without altering profile scenario count. Existing AMJ millet art is temporary; dedicated rice art waits for runtime validation. The historical pre-split MO fixture is not modified. Canonical design/balance: `Docs/Balance/Crops/RicePostHarvestProcessing.md`, `Docs/Balance/Crops/SevenGrainFoodChainAudit.md`. Do not remove About MO dependency or claim game validation/release until runtime gates pass.


**GRAINS-RICE-POSTHARVEST-QA-20261008:** Additional protection for the shipped rice processing source: §10 exact Japanese/English localization parity for two ThingDefs and four RecipeDefs (including four bilingual jobStrings), plus negative drift tests; Pickle loaded contracts now require the precise MO Straw Def and 1×/10× counts rather than accepting any second coproduct. Historical 38 MO contracts remain immutable. Full CI and four-profile C#/Pickle still separate; no gameplay balance/art/dependency changes.


**GRAINS-AUTOMATED-GATES-FIX-20261008:** Prepared targeted remediation for uploaded `automated-gates(2).log` errors: mood ThoughtStage empty description, invalid meal texture/stack graphic class, inherited double rottable (retain four standard meal comps and 2.5-day expiry), and overly strict empty 15×15 fixture search. CCTO-only duplicate local/Workshop installation is resolved by choosing a local copy in the test manifest, without modifying either installed copy or player ModsConfig; any other duplicate remains an error. Deterministic/negative Python and synthetic four-profile PowerShell tests expanded. This is a source correction, **not** a claim that the real 4×6 Pickle/ERROR-zero gate has passed.


**CI follow-up on automated-gates fixes (2026-10-08):** GitHub Actions run 37733027245 confirmed two failing negative tests were not guarded because the refreshed chain validator omitted the meal-comp and graphic assertions; restored those assertions and fixed the now-changed 2.5-day XPath in an existing negative test. Windows synthetic duplicate CCTO provider passed source resolution, but its manifest assertion hit PowerShell cast/member precedence (`[string]$cctoRecords[0].Root`); parenthesized the `Root` access before conversion. Confirmed by job logs before this targeted correction; the original loaded-game four-profile gate is still pending.

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
