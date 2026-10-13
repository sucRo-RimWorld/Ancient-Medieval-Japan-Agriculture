# AMJ Grains Coordination

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

### GRAINS-V1-PHASE-20261013 — simplified first release

**Owner:** Grains design/processing, equipment, and testing. **Status:** DONE — V1 scope policy integrated in Docs/Design.md; the equipment-specialization work remains OPEN for later releases.

**Author decision (2026-10-13):** Keep Grains V1 focused on a working harvest→thresh→hull→mill→food loop using available benches, rather than completing all originally planned purpose-built worktables before the first version. Specialized processing-station/millstone assignments and final building art are deferred to later Grains versions, without dropping intermediate grain-processing stages or relaxing load/native-Bill/ERROR-0 checks. 'V1' refers to the Grains release phase, not RimWorld game version or a promised numerical semver.

**Implementation distinction:** Both Base and MO already allow thresh/hull at FueledStove and ElectricStove (main PR #66, commit 9c323595ea1919f3986318a0b627bfbecc0b94c4), while keeping existing AMJ processing benches. The V1 non-MO stove milling fallback is implemented separately from draft PR #62: all five Base recipes list `AMJC_ManualMillstone`, `FueledStove`, `ElectricStove`; MO-only milling still replaces users with `DankPyon_Millstone`, and MO wheat keeps its native recipe. Real stove-Bill execution is not yet verified. Draft PR #62 is still open for other pending review material; avoid integrating its unapproved additions by treating it as a release gate for this standalone fallback. Existing production Defs, recipe yields/ingredients, WorkGivers, art and release metadata were not changed by this design-only decision.

**Later work:** Once equipment and images are designed, deliberately reallocate recipeUsers/WorkGivers with Base/MO compatibility, saved bills and existing DefName protections and native jobs tests; do not remove fallback benches by inference. Formal source: Docs/Design.md §1, §5, and Grains implementation roadmap.
### GRAINS-FLOUR-SCOPE-20261013 — Five flours, special and shared meals

**Owner:** Grains Food/processing. **Status:** In progress — all five flour and five strict-ingredient food chains are implemented; V1 mixed-meal source extension is on a feature branch; four-profile real RimWorld jobs, ERROR 0 and old-save checks remain OPEN.

**Current gameplay ownership:** Base uses `AMJC_WheatFlour`; MO uses `DankPyon_Flour`; four other flour providers are Grains-owned. Rice flour intentionally merges sticky/non-sticky flour conceptual use but not pounded mochi. Existing specified five flour foods and x1/x4 cooking Bills keep their DefNames, strict ingredient filters, +2 Mood, 2.5-day rot. Raw five grain providers enter mixed grain meal/porridge; five flour providers enter mixed flour food. New three shared foods have x1/x4 Bills and permit mixing only within their raw/flour families; the author requires generic leftover-use recipes to remain permanently alongside higher-performing specific dishes. New textures not created, accepted art untouched.

**Sources/gates:** `Docs/Design.md §5.3`, `Docs/Research/GrainMealRoadmap.md`, `Docs/Research/GrainsMixedMealsPriorArt-20261013.md` and adjacent decision JSON. Earlier research and four-serving batch details remain in `Docs/LocalizationHistoricalReview.md §12–13` and PR history (#64, #65, #70). Base and MO already allow thresh/hull at stoves; non-MO milling also has stove fallback, while MO milling stays with MO equipment. Runtime loaded Def, actual mixed Bill, harvested-source chain, rendering, save migration, and author historical wording acceptance are separate gates. No actual RimWorld runtime result established by static CI.

### AMJ-011 — Barley post-harvest grain graphics

**Owner:** Art/graphics. **Status:** IN PROGRESS — hulled barley accepted and integrated; edible barley remains OPEN (2026-10-10 JST). Immature/mature crop and sheaf art were integrated on PR #20; do not reopen those assets. The author accepted the hulled barley image. Preserve its exact 1429×1100 source at `Art/Sources/Things/Item/Resource/AMJC_Barley/BarleyInHull/BarleyInHull.png` (Git blob `2cb0f5eb972e3b0f574ae61c7b4150336c5adbff`) and three identical 256px textures at `Textures/Things/Item/Resource/AMJC_Barley/BarleyInHull/BarleyInHull_{a,b,c}.png` (Git blob `b047aaccdbbc3f31c3a0be3eb57f4077eb42c3d4`). `AMJC_BarleyInHull` now uses the dedicated texPath; its gameplay/recipe/nutrition values are unchanged. Dedicated edible barley artwork and real-game normal-zoom appearance checks are still pending. Crop/sheaf source provenance remains in `Docs/References/GrainsCropSourceManifest.json`.

### AMJ-017 — Grain-processing station graphics

**Requested by:** Alpha visual-completion audit  \
**Owner:** Art/graphics  \
**Status:** OPEN

The AMJ-specific `AMJC_GrainProcessingSpot` and `AMJC_GrainProcessingTable` are functionally validated but still use Medieval Overhaul StonecuttingSpot / Millstone graphics as development placeholders. Because these are AMJ-owned BuildingDefs normally visible during Stage A play, `Docs/Design.md §12.1.2` requires production graphics before the public Alpha.

The two buildings should remain visually related but clearly communicate the existing gameplay distinction:
- simple spot: improvised/manual, 1x1, slow;
- processing table: purpose-built, 1x1, normal speed;
- no gameplay/XML balance changes are part of the art task.

**Next action:** Art/graphics should create production textures for both grain-processing buildings after the crop-art backlog, then perform the normal in-game size/readability check.

**Result / references:** `Defs/ThingDefs_Buildings/Buildings_GrainProcessing.xml`; current temporary paths are `Things/Building/Production/StonecuttingSpot` and `Things/Building/Production/Millstone`.

### AMJ-013 — Wheat grain graphics

**Requested by:** Agriculture/XML  \
**Owner:** Art/graphics  \
**Status:** OPEN

`AMJC_Wheat` is a new processed grain state introduced by AMJ-012. The data slice temporarily reuses the accepted AMJ edible-millet stack texture. Because `AMJC_Wheat` is a distinct player-visible ThingDef rather than a deliberately shared post-harvest state, `Docs/Design.md §12.1.2` requires a dedicated production texture before the public Alpha. This does not block the already validated wheat data slice.

**Next action:** Art/graphics should provide a dedicated wheat-grain texture before the public Alpha and wire only the graphic path; do not change the validated wheat processing/balance.

**Result / references:** target DefName is `AMJC_Wheat`; harvested `DankPyon_RawWheat` remains the MO wheat-sheaf asset/display override.

### AMJ-019 — Millet hulled / edible grain production artwork

**Owner:** Grains Art/graphics. **Status:** IN PROGRESS — author-accepted in-hull image integrated; edible image and in-game review OPEN (2026-10-10 JST).

`AMJC_MilletInHull`（殻付き雑穀）は作者が新たに提示・承認した**枡入り**PNGへPR #31で差し替え済み。原画（1429×1100）を `Art/Sources/Things/Item/Resource/AMJC_Millet/MilletInHull/MilletInHull.png` に無加工で保存し、既存 `texPath` の `Graphic_StackCount` 用 `MilletInHull_a/b/c.png` を256×256、3枚同一データで置換した。新原画Git blob `61d6720c7354107424400dd9abf0edc0a118b859`、出力Git blob `039c6f11bbe5426d169b011a706cfcb3e6802c2f`。旧採用原画はGit履歴に保持。定量・レシピ・DefName・既存texPathは変更なし。PR #31の4チェックはPASS、main反映コミット `604ac338ac344981c101d3fbeb186e2c239bbc69`。実ゲームの見た目は未検証。

`AMJC_Millet`（殻なし・可食雑穀）は旧テクスチャのままで、作者採用済みの新画像がないため引き続きOPEN。アワ・ヒエ・キビの加工後状態は同一ThingDefを共有し、種別ごとの重複画像は不要。19種の栽培・束画像とは別計上する。正本: `Docs/Design.md §12.1.3`。

**2026-10-11 current status:** Main commit `5111980e13983125fc50b1368b4800fa61429090` replaced the hulled-millet production family with a single 128×128 `_a.png` and removed its duplicate `_b/_c` PNGs; this supersedes the earlier PR #31 three-file inventory, not the accepted high-resolution source/master. PR #38 (main merge `a27c19544e86375e7aa6ac108b63ef9129ce4a85`) updated the art/processing tests and `Docs/Design.md §12.1.4` so one `_a` is valid; optional `_b/_c` must be contiguous and valid and may differ. Main push checks all passed. No new grain image was authored by PR #38.

**Next:** 殻なし雑穀の採用・原画保存・実際に必要な数量別PNGの書き出し後、両方の通常ズームでの実ゲーム表示を確認。

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
- Three finished foods (`AMJC_Houtou`, `AMJC_Sobagaki`, `AMJC_MilletDumplings`), and Grains processing spot/table/non-MO manual millstone imagery. Source-work folders are placeholders only, not completed art.
- Source/runtime/UI/release evidence must retain its actual revision and profile scope. Old-save migration is out of scope because Grains was never installed.

**Verification for sheaf cleanup:** PNG integrity, existing Base/MO reference and historical-contract checks, upland rice, Stage A, source mapping and subscriber-payload checks passed locally. Exact-head PR and push checks are separate merge evidence. No image generation or RimWorld runtime was performed.

**Test audit retained:** PR #41 (`6b6a7a13fb8f55aa4b6249616237fef7ae191d93`) removed image freezes and source-string checks. Source hashes remain provenance only; PNG/reference/XML/provider checks and Pickle remain. Do not restore the deleted archive or art-wording tests.

### ART-TEMPLATE-001 — Pixel-exact shared component policy

**Owner:** shared art/tooling
**Status:** DONE (policy/tooling); OPEN (per-family template registration at next derivative)

Author requested pixel-identical reused parts across AMJ image families. Canonical policy: `Docs/GoldenPaths/FixedImageTemplates.md`. AGENTS, ArtStyle, texture Golden Path and cover workflow now require a hashed lossless master, binary editable mask, deterministic compositing, and zero protected RGBA differences. `Scripts/Art/fixed_template.py` implements compositing/validation; its regression test is in CI. Visual-reference editing alone is no longer sufficient.

No image was generated or replaced in ART-TEMPLATE-001 itself. The masu registration gap identified here is closed by ART-TEMPLATE-004 below. Covers are registered separately in ART-TEMPLATE-003. Do not claim unrelated asset families are pixel-locked unless they have their own master/mask/manifest registration. Species style references remain references rather than identical-species templates. Pending species proposals remain pending.

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

### TEST-RENDERED-PUBLISH — Isolated rendered runtime tooling (2026-10-05 JST)

**Owner:** Testing/tooling
**Status:** DONE (tooling publication); latest-main runtime regression pending

The earlier local snapshot passed Core 7/7, Environment six base Quickstarts
(58/57/54/52/3/3 assertions) and CCTO 70/70, with eight clean runtime logs
and combined exit 0. Game windows were enumerated on an independent non-visible
WinSta0 desktop while Direct3D rendering stayed enabled. The first hidden run
exposed worker-thread texture/map assertions; Core now posts those two steps
through PickleDriver and checks Unity main-thread execution. New Village Python
text reads explicitly use UTF-8. The saved launcher rerun passed after a separate
Haimatsu-review game released its DLL lock. No unrelated process was terminated.

Only this task's tooling/docs are published. Original working folders and other
local art/Def/review changes are preserved. These changes were transplanted onto
latest GitHub main, retaining its newer Core eight-scenario suite and Environment
Core-profile tests. Those newer runtime gates were not executed in the recorded
run and remain pending. No production art/Def/config change is included.

**Next action:** run the saved isolated desktop launcher against updated local
repositories to exercise the newer suites; human visual acceptance stays separate.

**Procedure:** Core Docs/IntegratedRuntimeTesting.md and
Scripts/IntegratedRuntimeDesktop/Run-AMJ-IsolatedDesktop.ps1.

### ART-SOURCE-ARCHIVE-023 — Original-source inventory

**Owner:** Art/graphics. **Status:** IN PROGRESS for the historical 2026-10-07 archive only. The previous **19 historical roles / 12 archived / 7 pending** are a *legacy-source recovery snapshot*, not the current crop-family count. Current 7 mature + 7 immature + 5 sheaf production states are mapped separately in `Docs/References/GrainsCropSourceManifest.json`. In this commit the 3 current existing mature source files are reused unchanged and 16 previously unarchived high-resolution crop/sheaf source inputs are added verbatim. Earlier accepted Awa/Hie/Kibi immature originals are preserved; their latest outlined versions have distinct source filenames. Image production and art approval are not release or game-rendering evidence.

**Next:** recover only genuine missing *historical* accepted masters when author-approved identity can be proven; never replace an original with a 256px derivative.

### ARCH-MODULAR-001 — Grains modularity and optional MO dependency

**Owner:** Grains agriculture/compatibility, Scenarios owner for starting scenarios. **Status:** IN PROGRESS for current-revision fresh-start real-game verification; mandatory MO dependency removal implemented. Old-save migration is out of scope.

Canonical design: `Docs/Design.md §2.8` (Base/MO ownership boundary), `Docs/GrainsDependencyAudit.md`, `Docs/GrainsProfileTesting.md`, `Docs/ScenarioExtraction.md` and production XML. Vanilla + Grains has independent wheat, grain processing and minimum flour-food paths; MO presence selects `Compatibility/MedievalOverhaul` and MO absence selects `BaseWithoutMO`. No duplicate default wheat/flour/millstone providers on MO profile. Legacy New Village definitions stay conditional when separate Scenarios is not present; separate Scenarios owns its own runtime and save handoff.

**2026-10-10 author decision:** The old mandatory MO declaration is obsolete. Remove MO from Grains `About/About.xml` hard dependencies and retain optional `loadAfter` ordering; align README, Workshop/2game/metadata and static/profile regressions. Preserve current production `packageId=sucro.ancientmedievaljapan.core`, AMJC DefNames, MO-only wheat/flour/millstone/Straw interoperability, and CCTO optionality. This is a metadata/compatibility-contract change, **not** a claim that MO can safely be removed from a running historic Core/Grains save.

**Evidence boundaries / next work:** Four-profile 24/24 fresh native-job Pickle and ERROR 0 existed on earlier source; later climate/sow diagnostics and this dependency-metadata change have no new identical exact-head real-game matrix evidence. **2026-10-10 author clarification:** Grains has never been installed or used; no Grains saves need migration. Old-Core save load/re-save, mid-save add/remove and removing MO from hypothetical existing saves are removed from the release gate. The synthetic tools and historic notes remain archive-only, not active tasks. Graphics/normal UI, approved text, seasonal growth, fresh-start four-profile checks and publication decisions remain OPEN. No Steam upload or author-visible game test is claimed. Obsolete intermediate migration and PR step history is retained in Git history; formal current sources above supersede its prior MO-required instructions.

### ARCH-UPLAND-RICE-001 — One rice Plant, cultivation environments differ

**Requested by:** author (2026-10-08 JST)  
**Owner:** Grains design / Agriculture XML / Testing  
**Status:** Common Plant and processing source IMPLEMENTED; future paddy behavior and latest-version loaded-runtime checks remain OPEN

**2026-10-13 author correction:** The upland/paddy distinction is a cultivation environment, not a second rice species/PlantDef, consistent with the existing unification of uruchi/mochi rice. Grains owns the common `Plant_Rice`, sheaf/hull/`RawRice` and flour; Rice Cultivation will apply paddy-field and water-management modifiers to the same Plant. No new rice PlantDef, edible rice or rice flour. Waterworks remains optional. Canonical updated specifications: `Docs/Design.md §4.2.2/§6.3/§8.1` and `Docs/Balance/Crops/RicePostHarvestProcessing.md`. Existing V1 production XML, crop balance and author-approved description text remain unchanged. Future paddy code, water/yield balance, loaded-game and compatibility checks remain OPEN.

Audit result: the current design still assigned water field, rice plant, paddy rice, edible rice and first-stage rice processing wholesale to Rice Cultivation, while runtime/test balance was fixed to six grains. Those statements are superseded by the updated Design boundary. CCTO already patches Vanilla `Plant_Rice`, so Grains should reuse that Def and must not add a duplicate cold-tolerance extension. Current six-grain environment and Pickle gates remain valid historical/current-implementation regressions but are insufficient for final release after this scope expansion.

**Current boundary / next work (2026-10-13 audit):** `Plant_Rice` V1 field-growing Patch, seven-crop static tests, sheaf/hull processing and five flour recipes exist in production source; `About` MO hard dependency was already removed. Do not repeat those completed source steps. The shared Plant stays Grains-owned; Rice Cultivation must only extend its paddy growing conditions. Preserve author-approved V1 upland-rice localization until the combined-field wording receives historical review. Revalidate loaded-game latest-source sow/harvest/processing and optional providers; future paddy source must undergo the independent prior-art and native regression gates. Formal ownership/balance lives in `Docs/Design.md` and `Docs/Balance/Crops/RicePostHarvestProcessing.md`.


**ARCH-UPLAND-RICE-001 implementation slice (2026-10-08):** Vanilla `Plant_Rice` upland-rice patch and Japanese DefInjected label/description, Ground-only sow tag, 5-day/11-yield/0.7 minimum fertility/0.8 sensitivity, 10–42°C growth and 18–32°C optimal values. Existing `RawRice` is preserved, no new rice Def or CCTO extension. New static seven-crop matrix regression runs beside the historical six-crop fixture. Game-loaded seven-crop Pickle, sow/harvest, visuals, four profiles and save gates remain open. Do not claim standalone or remove MO dependency.


**ARCH-UPLAND-RICE-001 E2E follow-up (2026-10-08):** Four real-provider Pickle profiles retain six scenarios but now include `Plant_Rice` in seven-crop environment comparison and native harvest jobs. Loaded checks cover Vanilla `RawRice`, cooking eligibility, Ground-only sowTags, exact rice balance, and CCTO -1°C / extension cardinality. A temporary growing zone validates soil-sow eligibility; actual season-controlled sow remains pending. PowerShell expected scenarios changed with feature names. Rice XML additions now use conditional replace-or-add to avoid duplication with CCTO. Runtime C# compilation, four-profile ERROR 0, art and save checks remain pending.


**Grains complete-food E2E gate (2026-10-08):** Native `CookMealSimple` jobs are now exercised for `AMJC_Millet`, `AMJC_Buckwheat`, `AMJC_Barley`, `AMJC_Wheat`, `RawRice` with real harvested/threshed/hulled inputs in all four-profile features; five extra meal Bills confirm per-grain input consumed and Vanilla `MealSimple` output. Explicit Pickle scenario timeout 270s, internal watchdog 240s and isolated runner 420s are set, retaining six scenarios per profile and strict ERROR gate. Static harness-contract regression added; real game/C# run, images and save compatibility are not yet validated. Next: run the four profiles with installed game in noninteractive rendering-preserving mode; triage failures before creating production art. Do not remove MO dependency.


**ARCH-MODULAR-001 MO1.6 provider archive preflight (2026-10-08):** The supplied `3219596926.zip` yielded 263 parseable XML files in loaded `1.6/Defs`, including the MO wheat/RawWheat/Flour/Millstone/1×+10× grinding Recipe and native Cooking `DankPyon_DoBillsMillstone` WorkGiver. Enhanced the Grains static provider validator to read packaged ZIPs as well as extracted folders and reject mismatched worker/recipe sources, with a synthetic ZIP negative regression. This is real MO **source** evidence but no RimWorld executable/Assembly-CSharp was available to launch four profiles or compile C#; renderer/ERROR/save gates remain OPEN.


**Grains runtime timeout preflight (2026-10-08):** Found Pickle global `-pickle-run-timeout=4` (240s) below the seven-crop/meal `@timeout:270` scenario budget. Parameterized the shared launcher global timeout with legacy default 4min; Grains alone now uses global 7min (420s) and outer 540s process watchdog. Static Windows tooling checks the four feature tags, global cap and process/matrix budgets. The private-desktop matrix still allows 40min, and per-job internal watchdog stays 240s. No game was launched; need real 4-profile Pickle, C# compile, runtime ERROR 0 before art.

### AMJGRAINS-LOC-APPROVAL-20261008 — accepted Japanese, paired English

**Owner:** Grains descriptions/localization. **Status:** JP approved and paired EN strings committed to source; static parity regression added; runtime text load/ERROR-0 not executed.

The author approved the existing 15 Japanese historical description/Recipe work-string candidates except two precise wording adjustments: use **AMJGrains** rather than Grains in player-facing prose, and replace the ambiguous wheat phrase `実は食材になるほか挽いて粉食に利用できる` with `小麦の穀粒は食事の材料に使え、石臼で挽けば小麦粉として粉食にも利用できる`. Apply the other approved Japanese text without extra historical claims. `Docs/LocalizationHistoricalReview.md` §2/§6 now records exact Japanese/English pairs. Source scopes: 5 shared grain-flour/food ThingDefs, 5 shared RecipeDefs, 4 non-MO wheat/millstone ThingDefs, one non-MO wheat milling Recipe. Japanese DefInjected describes all 15 and jobStrings for six Recipes; default English XML carries matching descriptions/jobStrings. Product labels, names/DefNames, gameplay XML, MO provider ownership and historical MO fixture unchanged. `Tests/test_grains_localization.py` checks exact docs–Japanese–English correspondence. Remaining: game-loaded language switching and four-provider Pickle/ERROR-0, extra plant prose review and images.

### GRAINS-SEVEN-CROP-BALANCE-AUDIT-20261008

**Owner:** Grains cultivation/food-chain balance. **Status:** Static full chain audit added, no gameplay retune; CI validation pending. Runtime and player-time balance remain open.

The authoritative `Docs/Balance/Crops/SevenGrainFoodChainAudit.md` now compares 7 crop choices, 10-unit thresh/hull outputs, 1:1 retention, normalized workAmount, 60/90-day flour storage, 0.5 nutrition flour input to 0.9 food, +2 Mood, and the original MO 1.6 source grinding work costs. The 27-cell seven-crop projection has all seven tied-or-unique maxima but `Plant_Rice` is only tied in six and uniquely best in zero; earlier unqualified niche phrasing is corrected in the GrainsEnvironment design source. MO wheat bulk flour costs 800 nominal work versus 300 Base; no MO-global grinding work changes have been imposed. A new `Tests/test_grains_balance_chain.py` and Stage A workflow step guard source XML/recipe/food and exact current max-count signature. `validate_grains_chain.py --mo-root` additionally verifies 300/100/800 workAmount on 1.6 source; synthetic ZIP fixture gains a negative work drift case. All tests here are static. Verify Stage A/Workshop CI and then four-profile actual Pickle, C# compile, ERROR 0, season sow, grain storage behavior, artwork, old saves before release.

### GRAINS-RICE-POSTHARVEST-IMPLEMENTATION-20261008

**Owner:** Grains production / E2E / localization. **Status:** Source implementation; full Python CI, C# compile, four-profile Pickle/ERROR-0, save migration and rendering pending.

Implemented Vanilla `Plant_Rice → AMJC_RiceSheaf → AMJC_RiceInHull → RawRice` with the shared processing benches and both individual/bulk Bills; initial work values 15/120 and 10/80, 1:1 conservation, intermediates 120-day/non-edible. Existing `RawRice` remains untouched. MO-only threshing generates `DankPyon_Straw`, Base cannot reference it. Approved Japanese copy is carried into new JP DefInjected and paired English defaults; rice crop description is narrowly aligned. Four-provider Pickle source covers native harvest, real bulk thresh, real bulk hull, Vanilla simple meal and CCTO coexistence without altering profile scenario count. Existing AMJ millet art is temporary; dedicated rice art waits for runtime validation. The historical pre-split MO fixture is not modified. Canonical design/balance: `Docs/Balance/Crops/RicePostHarvestProcessing.md`, `Docs/Balance/Crops/SevenGrainFoodChainAudit.md`. Do not remove About MO dependency or claim game validation/release until runtime gates pass.


**GRAINS-RICE-POSTHARVEST-QA-20261008:** Additional protection for the shipped rice processing source: §10 exact Japanese/English localization parity for two ThingDefs and four RecipeDefs (including four bilingual jobStrings), plus negative drift tests; Pickle loaded contracts now require the precise MO Straw Def and 1×/10× counts rather than accepting any second coproduct. Historical 38 MO contracts remain immutable. Full CI and four-profile C#/Pickle still separate; no gameplay balance/art/dependency changes.


**GRAINS-AUTOMATED-GATES-FIX-20261008:** Prepared targeted remediation for uploaded `automated-gates(2).log` errors: mood ThoughtStage empty description, invalid meal texture/stack graphic class, inherited double rottable (retain four standard meal comps and 2.5-day expiry), and overly strict empty 15×15 fixture search. CCTO-only duplicate local/Workshop installation is resolved by choosing a local copy in the test manifest, without modifying either installed copy or player ModsConfig; any other duplicate remains an error. Deterministic/negative Python and synthetic four-profile PowerShell tests expanded. This is a source correction, **not** a claim that the real 4×6 Pickle/ERROR-zero gate has passed.


**CI follow-up on automated-gates fixes (2026-10-08):** GitHub Actions run 37733027245 confirmed two failing negative tests were not guarded because the refreshed chain validator omitted the meal-comp and graphic assertions; restored those assertions and fixed the now-changed 2.5-day XPath in an existing negative test. Windows synthetic duplicate CCTO provider passed source resolution, but its manifest assertion hit PowerShell cast/member precedence (`[string]$cctoRecords[0].Root`); parenthesized the `Root` access before conversion. Confirmed by job logs before this targeted correction; the original loaded-game four-profile gate is still pending.

### ART-GRAINS-GENERATOR-20261009 — Grains-specific grain/crop image generation entry point

**Requested by:** author (2026-10-09 JST)  
**Owner:** Art/tooling / Grains  
**Status:** CORRECTED SOURCE IMPLEMENTATION — paid API path removed; no image accepted by this work

The initial implementation called the OpenAI image API and therefore would have created separate API usage/cost outside the ChatGPT subscription. The author rejected that design. It is superseded.

Current source keeps `Scripts/Art/grains_image_generator.py` as a deterministic **prepare/review** entry point only. `prepare` resolves and hashes the registered accepted AMJ references, caller-supplied subject identity and applicable real Medieval Overhaul reference, then writes `prompt.txt`, an exact-byte reference bundle, `manifest.json`, and `generation-request.json`. The actual image is generated once with ChatGPT's built-in image generation outside the Python process. `review` then stages that PNG byte-for-byte, runs existing generated-asset QA plus relative 64 px complexity comparison, creates the review sheet, and hands boxed contents to the existing boxed-resource preparer. The script contains no `OPENAI_API_KEY`, OpenAI API endpoint, image-API request, or automatic retry/batch loop.

Because in-chat image generation displays its result immediately, this no-extra-API-cost route cannot claim private pre-display screening. The generated image remains an immediately visible, not-yet-vetted draft until `review` and semantic comparison complete, per the shared Project texture pipeline. Candidate/work output remains blocked from `Textures/`, `Art/Sources/`, and `Docs/References/`. No gameplay XML, Def paths, production PNGs, dependency metadata, Workshop payload, or publication state changed.


### GRAINS-GENERATED-ART-20261010 — crops and sheaves

**Owner:** Grains art. **Status:** SOURCE INTEGRATED / STATIC VERIFIED; fresh-game rendering OPEN.

PR #20 (`cd52a3d9e15467d2f6ebc1bdaac63cda84f98cbd`) wired seven immature and seven mature crop paths, five bundle families, Vanilla upland rice and MO wheat conditional graphical override. Latest immature outline is #4D4E3C with upright millet. The 2026-10-10 source archive preserves or maps all 19 source states via the hash-locked manifest (16 new sources, 3 preexisting originals). The author's crop image checks are acknowledged; no re-generation or repeat approval request. Static PNG / Stage A / Workshop tests are distinct from loaded-game visual acceptance.

**Next:** fresh-start screen/render inspection across actual Vanilla and MO providers, address only concrete sprite/UI failures; independently complete postharvest **millet in-hull and edible grain** (AMJ-019), barley/wheat item images and processing station art. No Steam release or real-game PASS claimed.

### GRAINS-NATIVE17-20261013

**Owner:** Grains runtime/CCTO compatibility. **State:** TEST FIX VERIFIED; PRODUCTION CCTO BLOCKER REQUIRES SCOPE DECISION. Latest base `d456ebb86ae580eb3fa74982a1a1fc5edc345c77` tested on 1.7.4976 rev1043: Vanilla 6/6 ERROR0, Vanilla+CCTO 4/6 ERROR4, MO 6/6 ERROR0, MO+CCTO 5/6 ERROR2; complete PID logs, no skipped/flaky scenarios. Actual harvest/processing/new meals and controlled seasonal recovery pass all four. Shared grow-zone replacement caused the old rice recovery failure; test zones now remain separate. Strict failed extension checks are retained.

Production `CCTO_StageA.xml` (five operations) and Base `CCTO_Wheat.xml` (one) search an English display name; installed CCTO uses its bilingual title. Actual 1.7 FindMod checks active display names, so required extensions are missing. This task authorizes test-code fixes only: production XML not changed. Next decision: bounded name compatibility in those two XMLs, retaining old English title and all numeric values, then affected runtime reruns. Canonical evidence/premises: `Docs/GrainsProfileTesting.md`, `Docs/Testing/Native17-20261013.json`. Natural frost/climate, legacy saves, stove variants, appearance and release remain OPEN. This item supersedes earlier seasonal/NRE/source-identity runtime correction chains; historical evidence remains in the testing document/Git history.
