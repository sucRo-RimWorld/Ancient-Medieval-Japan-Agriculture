# AMJ Grains Coordination

## GRAINS-CGB-MERGER-20261019 — author-approved unified crop balancing

**MERGED into Grains main:** [PR #183](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/183) `89a71ea1b223d46bcab2dec46eadb8f2189c70cf`. Grains now ships 6 Vanilla + 7 optional MO former CGB crop `growDays` values, and MO lentil sowMinSkill=2 formerly in Japanization. Rice skill4, seven-grain crop values, CCTO cold rules and MO optionality unchanged. Definitive source: `Docs/Design.md §4.2.1a` and the two ExistingCropGrowthBalance XML files; source checks 15/15 combined ownership/inventory PASS; native loaded Def/sow/harvest on this exact main NOT RUN. Standalone CGB retired, not an additional runtime dependency. Japanization removed both its CGB dependency and duplicate lentil patch. Next: seven-grain + Vanilla/MO crop **yield/skill/fertility overall rebalance**, including lentil growDays8/harvest10/sow2 now provisional; future 200% capped yield system remains unimplemented; do not silently change numbers based on the merger.


## GRAINS-RICE-FLOUR-RETIREMENT-20261019 — production change merged

**Owner:** Grains. **State:** SOURCE IMPLEMENTED; TARGETED STATIC SOURCE CHECK PASS; NATIVE UNVERIFIED. Grains [PR #175](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/175), main `8ffcd5515041c1ea695e8106167a815202aaaba9`, implements the author's decision **not** to require rice flour as a routine ancient/medieval staple. The old `AMJC_RiceFlour`, `AMJC_RiceFlourDumplings` ThingDefs and `AMJC_MillRice` / `AMJC_CookRiceFlourDumplings` (+Bulk) RecipeDefs are retained for saved references only: all three Bills have no recipeUsers and MO no longer restores the rice milling Bill. Shared flour food 1/4 recipes exclude rice flour; MO flour substitution moved to ingredient slot 4. Active flour paths: wheat (MO if present), buckwheat, millet, barley (four); active cooked food paths: common three, specified flour four, specified brown/white rice four (11 foods/22 Bills). Do not delete accepted art or treat legacy IDs as active production. Formal source: `Docs/Design.md`, `Docs/Balance/Crops/SevenGrainFoodChainAudit.md`, actual XML/conditional MO patch. Static XML mapping assertions examined; native Bill menu/load, MO and old-save compatibility remain OPEN. No Steam upload. **2026-10-19 scoped follow-up:** Current-source static source assertions 30/30 PASS; found and corrected a 5/4/4-length bug in `Tests/E2E/GrainsProfileSteps.cs` (the retired `RawRice` milling slot), and added a focused test guard. Existing Python validators and actual loaded game have NOT been executed for the corrected source; formal evidence/boundary in `Docs/GrainsProfileTesting.md`. **Public-copy synchronization (2026-10-19):** README and EN/JA Workshop draft now explain the rice-flour exclusion in a Q&A and use the active 4-flour/11-dish inventory; the superseded comparison image is retained in Git but held out of the current Workshop draft. GitHub copy only; no Steam publication.

## GRAINS-RICE-WHITE-20261019 — separated brown/white rice and meals

**Owner:** Grains processing/cooking and graphics. **Status:** XML and documentation integrated on main in [PR #163](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/163), merge `7a166f3425777d319035f0ec0f2478c54b55fe37`; native execution/UI/approved distinct images **OPEN**. Brown rice retains Vanilla `RawRice`; white rice is `AMJC_WhiteRice`. Existing paddy10→brown10 Work80 Bill stays; new paddy10→white8 Work240 Bill. Four brown/white rice-specific meals (rice meal, white meal, rice porridge, white porridge) with eight 1/4-food Bills; white meal Mood+3 and white porridge Mood+1 (0.5 days), both provisional game-balance values. Preserve mixed meal/porridge; the later PR #175 supersedes its former rice-flour production connection. Formal source: `Docs/Design.md §5.3`, `Docs/Balance/Crops/RicePostHarvestProcessing.md`, exact Def/Recipe/Thought/XML.

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

**Owner:** Grains Food/processing. **Status:** V1 source complete, current game/UI acceptance OPEN. Current production: four flours, four flour-specific dishes, three common foods and four brown/white rice dishes (11 foods / 22 cooking Bills). Legacy rice flour/dumpling Defs survive for save references, not active production. Canonical production: `Docs/Design.md §5`, `Docs/Balance/Crops/SevenGrainFoodChainAudit.md`, `Docs/LocalizationHistoricalReview.md`, relevant Defs/Recipes. Current accepted JP/EN text and graphics paths are there; original grains and placeholder-specific-meal images remain protected from unapproved changes.

**Remaining:** native-current-source end-to-end flour and meal Bills including cooking input selection/Thought, bilingual actual loaded UI, and normal-zoom evaluation of the provisionally adopted common 飯・粥・粉食 images and specific-food placeholders. Steam publisher and actual distribution remain separate; historical static/native PASS logs do not certify current source. Earlier process snapshots and individual native font experiments are retained in Git history and `Docs/GrainsProfileTesting.md` / Project test records, not repeated in this coordination queue.


### AMJ-011 — Barley post-harvest grain graphics

**Owner:** Art/graphics. **Status:** PRODUCTION WIRED / loaded-game visual and high-resolution source review OPEN (2026-10-19).

The 2026-10-10 author-approved hulled-barley source is preserved at `Art/Sources/Things/Item/Resource/AMJC_Barley/BarleyInHull/BarleyInHull.png` (Git blob `2cb0f5eb972e3b0f574ae61c7b4150336c5adbff`); the then-current three identical 256px derivatives (blob `b047aaccdbbc3f31c3a0be3eb57f4077eb42c3d4`) are historical. The 2026-10-11 author-pushed 128×128 single-`_a` grain sprites include dedicated edible barley `AMJC_Barley`; PR #40 wired the edible-barley texPath. **Do not list edible barley as having no production image.** The corresponding image's high-resolution source/final visual acceptance and ordinary-zoom game display remain to be confirmed, without changing gameplay, DefNames, or accepted crop/sheaf art.

### AMJ-017 — Grain-processing equipment imagery / manual millstone stone materials

**Owner:** Grains graphics / BaseWithoutMO. **State (2026-10-20):** Base production already includes approved 1×1 processing spot/table and earlier gray manual millstone. New **candidate only** [Grains PR #186](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/186) changes the non-MO millstone to a larger 1×1 sprite with stone-only Stony material mask (selected stone 30 + WoodLog 20; same 256px sprite/mask copied to all N/E/S/W, wooden handle fixed). MO continues to use `DankPyon_Millstone`. Author inspected the new game screenshot 2026-10-20 and judged the appearance **provisionally acceptable**; do not regenerate it speculatively or call this final source/art acceptance.

**Actual current candidate image evidence:** [Project Actions #39273905572](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39273905572), artifact `12196806528`, exact Grains candidate `31f08687521540364ec397863f63dbf3e8ba8fba`: RimWorld 1.6 Vanilla EN UI Pickle **1/1 PASS, ERROR0, exit0**. Real 12-building map screenshot `vanilla-en/screenshots/equipment-map-vanilla-en.png` shows granite/marble/sandstone/slate millstones colored differently with brown handles. Same UI dispatch overall **FAIL** from a later Japanese Mono/GC SIGSEGV exit139 before Pickle report. Separate Vanilla V1 [#39273199178](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39273199178) **9/10**, failed fixture `WorkGiver_GrowerSow.JobOnCell` NullReferenceException; do not conflate with stone rendering. MO/CCTO/1.7 and new-build Bill behavior are not covered by this screenshot test.

**Open:** PR #186 not merged. New high-resolution trimmed original `ManualMillstone_TrimCandidate1254.png` exists in the candidate ZIP outside GitHub but **not yet byte-preserved** under `Art/Sources/Things/Building/Production/AMJC_ManualMillstone/`; the old `ManualMillstone_Reference1254.png` is different artwork and is not an acceptable substitute. Finish source provenance/validation and review the above separate failures before closeout; Steam untouched. Formal spec and verified coverage live in candidate `Docs/Design.md §12.1.2` and `Docs/GrainsProfileTesting.md`, and production XML/PNGs. Historical earlier 1.6 Base/CCTO four-profile materials and first 12-building map screenshot: [Project Actions #39236537085](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39236537085), source `47b312ce2c1ab83206889f34329be365fbd747bb`; older gray mill, not material-selected art.

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
