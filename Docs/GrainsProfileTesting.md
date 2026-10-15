# Grains dependency-migration test profiles

**Historical RimWorld 1.6 seasonal-test evidence (2026-10-08 JST):** `automated-gates(8).log` confirms the post-sky-fix **vanilla 6/6, runtime ERROR 0**. The other three profiles on that corrected source were not re-run as part of that 2026-10-08 evidence. Earlier `automated-gates(4).log` established four-profile fresh smoke **24/24, ERROR 0**, before seasonal-test changes; it does not certify the current 40-scenario matrix. Natural seasonal weather, visual approval and release-readiness remain separate gates. There are no deployed Grains saves, so legacy migration is out of scope. Production MO hard dependency was removed on 2026-10-10; previous run results apply only to the revisions actually tested. The fresh-start runtime/art/public-release gates remain open; old-save/provider-removal is not a gate.

The four-profile harness was established before changing production dependency metadata. The production `About.xml` now declares **no mandatory MO or other Mod**, while `loadAfter` retains an optional MO ordering hint. The MO conditional and BaseWithoutMO fallback paths remain exclusive. A successful metadata/static tooling test is not a real-game test or public-release clearance.

## Author-deferred batch: loaded-game localization and rendering (2026-10-13)

**Decision:** Do not perform one-off native/game-UI checks after each rice-text edit. Defer actual RimWorld launches and screenshot inspection until the next consolidated Grains native/visual regression session. This is scheduling scope only, **not** a waiver or a PASS. Static XML/document/locale parity tests may continue independently; none can substitute for loaded-game evidence. Do not start a new runtime host or retry incomplete game installation merely for this wording revision.

In a single reviewed-source run, inspect these **four existing items** in both Japanese and English and verify the indicated specification:

| Loaded item | Expected description and paragraph layout |
|---|---|
| `Plant_Rice` (稲 / rice plant) | **2 paragraphs**: Yayoi rice cultivation, upland versus paddy rice as one Plant, then AMJ's shared crop and current growing/processing behavior; no Edo/medical history here |
| `AMJC_RiceSheaf` (稲束 / rice sheaf) | **2 paragraphs**: harvested unthreshed sheaf, then ancient/medieval taxes, estate rent and commodity-money role |
| `AMJC_RiceInHull` (籾 / rice in hull) | **2 paragraphs**: grain in husk, then the distinction between hulling into brown rice and further polishing to white rice, including vitamin-B1 loss |
| `RawRice` (米 / rice) | **4 paragraphs**: edible rice in a single Vanilla Def, Edo white-rice diet and beriberi, Takaki's Meiji Navy investigation, and Suzuki's oryzanin discovery |

Batch checks: (1) resolve each item by its **actual loaded DefName**; confirm that English default description patches and Japanese DefInjected win without duplicate keys or missing/empty strings, including `RawRice` despite its reused Vanilla identity; (2) confirm literal XML `\n\n` becomes visible paragraph breaks, no raw backslash text, clipping, missing kanji or broken line layout in the Info tab at ordinary UI scale; (3) inspect the existing crop/item icons and normal-zoom states for missing texture/BadTex without altering approved artwork; (4) in each relevant native profile, collect game version, exact source state, provider manifest, named scenario results, complete Player.log and zero-ERROR evidence, reusing the standard Grains matrix rather than launching a separate ad-hoc game.

**Scope and results:** Run with the next consolidated Grains 1.6/1.7-appropriate native matrix, not immediately in this wording workstream. A test on a prior commit or solely a successful GitHub Action does not verify these latest descriptions. Existing older native 1.6 PASS evidence and the documented 1.7 CCTO failures remain as recorded; they are not overwritten or silently reclassified. Actual JP/EN UI and rendering are **DEFERRED / NOT RUN** on this source; final publication eligibility remains OPEN.

## Isolated bilingual loaded-UI and material inspection fixture (2026-10-15)

**Scope: TEST SOURCE ONLY, no native result yet.** The original four-profile 10-scenario V1 owner features and their recorded source-scoped functional 40/40 remain unchanged. `Tests/E2E/GrainsUiSteps.cs` and `Tests/E2E/Profiles/grains-ui.feature` add a *separate* one-scenario Pickle/Quickstarts fixture, to run via the same isolated Linux 1.6 Grains host in two truly boot-selected languages rather than asserting a translation file is equivalent to loaded game language. It is not part of the 40/40 and may not inherit its runtime PASS.

Explicit premises: resolve the actual `Plant_Rice`, `AMJC_RiceSheaf`, `AMJC_RiceInHull` and `RawRice` ThingDefs with JP loaded labels 稲/稲束/籾/米(生) (the Vanilla 1.6 RawRice label) versus EN rice plant/rice sheaf/rice in hull/rice; require respectively 2/2/2/4 nonempty paragraphs with **actual newlines** and no literal `\\n` escapes in the loaded Def descriptions. Resolve JP 飯/粥/粉食 versus EN cooked grains/porridge/flour dish. Require full, non-BadTex `MatSingle` for each current grain/food icon and all available stack-count variants, plus resolvable planted-state graphics without overwriting any accepted crop, sheaf or grain source. Open a real `Dialog_InfoCard` for each of the four rice items in an Xvfb-rendered native game at 1280×720 and preserve PNG screenshots as review **evidence**; a screenshot file existing is not a human visual/semantic acceptance.

Runner must create independent `Prefs.xml` with an explicit `langFolderName` per boot, verify `Prefs.LangFolderName` in game, set a test-owned screenshot destination, record the source SHA / named scenario / actual rendering / complete original Unity log / zero Unity ERROR / normal exit, and refuse missing or unreadable screenshots. Existing 40 owner scenarios, their original assertions and functional evidence are not relaxed. This visual batch does **not** claim pixel-level typography/line clipping, ordinary-map crop-zoom aesthetics, final graphic approval, original-MP3 MO audio ERROR0 or Workshop readiness. A failed native compilation or runtime finding remains FAILED until corrected and re-run.

### Direct-font-object native trial and next glyph-geometry check (2026-10-15)

Real [Project Actions #38705251696](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38705251696), owner `eba098ab7eb14abf44f641cafbe0a03577159106`: EN/JA × Vanilla/CCTO real native 4/4 owner UI scenarios, Unity ERROR0, all 16 screenshots. In Japanese runs the test created `AMJ_Test_NotoSansJP` from a temporary verified Noto Sans JP using `Font.CreateDynamicFontFromOSFont`, changed **15 references** across five `Verse.Text` arrays, and verified live assignment. Comparing screenshot bytes/pixels against prior controlled fontNames-only [#38692895907](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38692895907): all eight English captures were byte-identical; the eight Japanese screenshots changed pixels but **still displayed no Kana/Han**, including Vanilla UI. Hence replacing selected Unity font objects does not by itself prove visible Japanese graphics. The corresponding `runtime-textgeometry.txt` reported `probe_status=WRONG_FONT_COUNT`, because its sampler included only the original three objects despite requiring a count of four in the font-injection case. This was **diagnostic incompleteness**, not a failed 4/4 owner UI scenario.

**Next bounded diagnostic:** include the actually created `AMJ_Test_NotoSansJP` font in the existing optional `TextGenerator` geometry probe, preserving per-language 20px `UIVertex` quad coordinates/UVs/alpha for `A`, `稲`, `あ`, `中`. Compare **new font's actual glyph quad dimensions** with old 2×2 placeholders, the unchanged screenshot evidence and original fonts; do not infer rasterization success from a font-object name or successful assignment. Only diagnostic C# and isolated host source pin may change; owner translations/art/gameplay/Steam and installed third-party font binaries remain untouched.

### Explicit Japanese font-object selection after degenerate Unity glyph quads (test-only, 2026-10-15)

Actual [native #38703813653](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38703813653), Grains `0b2e398494427a172c14430944bf24fbe516602b`, EN/JA × Vanilla/CCTO **4/4 owner UI scenarios PASS, native ERROR0**, 16 InfoCard screenshots. Real 20px Unity TextGenerator `UIVertex` metrics on all three legacy fonts: Latin `A` yields a **13–17px-wide/15–17px-high glyph quad**, Japanese `稲`, `あ`, `中` each yield **2×2px degenerate placeholders**, with all vertex alpha 255 and small UV rectangles. This is **strong runtime evidence that the selected Unity Font objects do not supply renderable Japanese glyph outlines on this Linux host**, even though `HasCharacter` and `GetCharacterInfo` returned true and four vertices existed. All 16 UI PNGs remain byte-identical to the previous atlas-only test; the missing Japanese pixels are not fixed. This is not proof that original Windows RimWorld lacks glyphs or that the embedded Unity non-English resource contains Chinese-only glyphs.

The next bounded **distinct test-only remediation experiment** uses the already verified temporary Noto Sans JP placed in Japanese profiles' disposable HOME, calls `UnityEngine.Font.CreateDynamicFontFromOSFont("Noto Sans JP",20)` to instantiate an actual Unity Font rather than editing `fontNames` on the unusable old Font, and assigns that instance to the **existing** `Verse.Text.fonts`, `fontStyles`, `textFieldStyles`, `textAreaStyles`, `textAreaReadOnlyStyles` slots *in the isolated test process only*. The English profiles remain original controls, prior `fontNames` injection must be disabled, and an exact assignment record is mandatory. The same four original real InfoCards/screenshots and zero ERROR constraints are preserved. After test, inspect Japanese glyph quad dimensions **and actual pixels**; a C# setter returning successfully is not a visual PASS. No new font is bundled in Grains, no original publisher bytes or user OS/font installation changes, and no Grains production locale/gameplay changes. **Not tested yet**.

### Unity generated quad UV/color follow-up (2026-10-15, test-only)

Native [#38702862526](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38702862526) on owner test source `9c6d4d885dd0c1cd97575ed8787e21906bb55051` **4/4 bilingual × Vanilla/CCTO loaded-UI scenarios**, Unity ERROR0, 16 native InfoCard screenshots, four TextGenerator reports. With all three actual GUI fonts at 20 px, `TextGenerator.Populate` returned true and `characterCountVisible=1`/`vertexCount=4` for Latin A, Japanese 稲, kana あ and common Han 中; for four mixed characters it returned 4 visible characters and 16 vertices. English and Japanese results were identical. Comparing all 16 PNG files against original preceding game run #38698858976 showed **byte-for-byte identical images**, including absent Japanese glyphs on the isolated Linux/Xvfb host. **Four vertices do not certify glyph shape, valid UV coordinates, nonzero alpha or visible pixels.**

To distinguish degenerate quads or blank atlas references from a later renderer failure, next disposable-only test records each of the first four generated `UIVertex` fields (`position`, `uv0`, `color`) for each single-character TextGenerator sample. The exact same original English/Japanese screenshot and strict loaded-Def/Pickle/Unity ERROR gates stay in place. Do not infer active InfoCard shader output from probe-generated geometry; compare both languages before attributing any issue to font atlas vs GPU compositing. No original bundled font/binary, user fonts, production images or translations are altered. **New quad-field native collection pending**.

### Unity IMGUI text geometry diagnostic (2026-10-15; test-only)

The original Linux 1.6 game's font metadata and glyph request probes have already been executed. Existing `HasCharacter`, `GetCharacterInfo`, and direct `fontNames` changes **did not establish visible Japanese rendering**. The next bounded diagnostic instruments the **same original one-scenario bilingual native game test**, but only if the isolated runner sets `RIMWORLD_GRAINS_UI_TEXT_GEOMETRY_REPORT`. While the original `Dialog_InfoCard` is displayed, its test-only C# code uses the actual loaded Unity legacy `TextGenerator.Populate` API with selected `Calibri_tiny`, `Arial_small`, `Arial_medium`, 20 px generation settings and 700×350 clipping extents to inspect geometry produced for Latin `A`, Japanese `稲`, kana `あ`, common Han `中` and `A稲あ中`. Records success/failure, `vertexCount`, visible character count, and accessible `verts.Count` (or reflective API error) in a per-profile text report. It does **not** change original `Verse.Text` styles, engine assets, installed fonts, or production Grains content; generated temporary text vertices and any atlas effects exist only in the disposable process.

This checks **whether the Unity text generator can produce per-character vertices**, not whether RimWorld's *actual* InfoCard GUIStyle used identical settings or whether those vertices were rendered to the screen. It is not glyph shape analysis and cannot prove original Windows behavior. Preserve the existing 4 boot-selected English/Japanese × Vanilla/CCTO loaded-text checks, screenshots, Unity ERROR0, normal process exit, and original font/glyph reports. Any absent collector report is a diagnostic FAIL; any `Populate=false` or vertex-count mismatch is a finding for interpretation, not permission to overwrite approved Japanese XML. **Native game test not executed on this collector source yet.**

### Unity legacy glyph atlas diagnosis — actual native result (2026-10-15)

**Runtime:** exact owner source Grains `53246ddf581872f93b8530d4f788197178fd84e1`, Project Linux RimWorld **1.6.4871**, original unmodified Vanilla/CCTO and default Unity font names, [Actions #38698858976](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38698858976), artifact `amj-integrations-38698858976` (ID `11955485609`). The original bilingual owner loaded-Def UI Pickle scenario ran independently `vanilla-en`, `vanilla-ja`, `vanilla-ccto-en`, `vanilla-ccto-ja`: **4/4 scenario PASS, Unity ERROR0, game exit0, zero failed/skipped/flaky, 16 original 1280×720 InfoCard screenshots**; all four `runtime-fonts.txt` and new `runtime-glyphs.txt` files were collected.

**Actual legacy engine font evidence:** The active `Verse.Text` fonts `Calibri_tiny`, `Arial_small`, `Arial_medium` are **Unity dynamic fonts**, each with **256×256 Texture2D atlas** and `GUI/Text Shader`, original family `Calibri,Arial` / `Arial`; no Noto font or modified `fontNames` was used. For each font at 16px and 22px, `GetCharacterInfo` returned **false before a requested glyph registration** and **true afterward** for Latin `A`, Japanese-specific Han `稲`, kana `あ` and common Han `中`, in **both** English and Japanese profiles. All 24 bounded `RequestCharactersInTexture` calls completed without exception; advance metric was not exposed by the reflection check (`NA`). The pre-request false does **not** by itself mean game glyphs are missing: the explicit 16/22px sizes may differ from Unity's active rendered atlas sizes; a post-request true proves only that the API returned character metadata, not visible pixels, accurate glyph shapes or an actually used GUI layout.

**Screen evidence:** Against preceding Noto-first A/B [#38692895907](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38692895907), all **16/16 final screenshot PNGs were byte-identical**, covering Vanilla/CCTO × EN/JA × four rice items. Thus glyph requests did **not** change the UI output. The Japanese images still visibly omit Kana/Kanji **including base RimWorld UI**, while ASCII and numerals remain; e.g. Ja RawRice PNG SHA-256 `b1ed73544a30ca5947161786e3be0e026a11c4399019441845d2a30082d5752c` in both runs. The fault is **not attributable to Grains' Japanese XML**, which is loaded correctly, or to a simple failure to request these glyphs; the isolated Linux/Xvfb Unity UI rasterization/layout is unresolved. **Japanese graphical PASS is NOT established; original Windows UI behaviour and actual bundle font binary identities are untested.** Do not repeat fontconfig/Unity `fontNames`/glyph-request-only trials. Project native host owns the next actual text-generator/IMGUI rasterization or original game font asset investigation; Grains production XML, imagery and Steam payload remain unchanged.


### Actual Unity font inventory and scoped fallback-name A/B — 2026-10-15

[Project native 1.6 #38691883786](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38691883786), original Grains source `9fe11dc8b5cc20929b9475cc673bf3aeb0478d2a`: four original Vanilla/CCTO × English/Japanese UI scenarios **4/4 PASS**, 0 Unity ERROR, 16 valid screenshots, and four collected real `runtime-fonts.txt` reports (`amj-integrations-38691883786` artifact `11952916279`). Actual runtime `Verse.Text` GUIStyle references are `Calibri_tiny` (families Calibri,Arial), `Arial_small` and `Arial_medium` (families Arial); `LegacyRuntime` also loaded. All four loaded `UnityEngine.Font.HasCharacter` probes report true for Latin, Kana, shared Han and Japanese-specific 稲/籾. There is **no Chinese-named font in the four reported loaded font objects**, but this does not prove no bundled Chinese fonts elsewhere or that `HasCharacter` implies pixel rendering. Unity `GUI.skin` itself cannot be queried outside `OnGUI`; that diagnostic yielded a nonfatal exception and the actual Verse style fields are available. Japanese glyphs **remain visibly missing** in the original Ja screenshots, while English labels render. **No locale or production XML change is justified.**

**A/B execution and visual verdict (2026-10-15):** [Project native Actions #38692895907](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38692895907), Grains source `b67e495dc60edc1ebcbe352330839fbb61d23087`, four independent Vanilla/CCTO × English/Japanese profiles: **4/4 existing loaded-UI scenario PASS, 0 Unity ERROR, exit0, 16 valid screenshots and four nonempty font inventories** (artifact `grains-ui-jpfont-native-38692895907`, ID `11953271349`). Only Japanese disposable HOME contains the verified temporary Noto Sans JP font. Its `runtime-fonts.txt` explicitly shows `diagnostic_font_override=Noto Sans JP`; `Arial_small`, `Arial_medium`, and `Calibri_tiny` now each have `Noto Sans JP` **first** in the live `fontNames` array; English controls retain original names. The real Japanese InfoCard screenshots still **omit Japanese characters**. Independently compared each of eight Japanese screenshot PNGs against [the earlier fontconfig-only run #38690304819](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38690304819), 4 rice Defs × Vanilla/CCTO: **8/8 files are byte-for-byte identical and 0 pixels differ** (1280×720); e.g. Japanese `RawRice` SHA-256 in both runs is `b1ed73544a30ca5947161786e3be0e026a11c4399019441845d2a30082d5752c`. This specifically **rejects the proposed fontconfig-plus-fontNames remedy**, not the loaded Japanese translations, not the presence of glyphs in some other engine font asset. `UnityEngine.Font.HasCharacter=true` does not prove the text was rasterized into the InfoCard. **Japanese visual UI remains FAIL on the isolated Linux/Xvfb host.** The current native experiments cannot establish whether original on-screen Windows game fonts have the same defect; do not treat host artifacts as production Grains graphic defects. Stop repeating this exact fallback-only experiment; any next investigation must inspect Unity's actual glyph rasterization/atlas and original bundled font assets before modifying host or production state. No owner production language, art, gameplay or Steam files were modified.

### Internal Unity font inspection (test-only source, runtime evidence above)

After the Japanese InfoCard screenshots remained blank for Kana/Kanji even with a **temporary fontconfig-verified Noto Sans JP**, the next diagnostic reads fonts **inside the running RimWorld 1.6 Unity process**, not from the Linux host. On the first open real InfoCard of each requested language profile, `Tests/E2E/GrainsUiSteps.cs` writes a bounded `runtime-fonts.txt` only when the isolated Project runner sets `RIMWORLD_GRAINS_UI_FONT_REPORT`. This report includes loaded `UnityEngine.Font` names/families and `HasCharacter` results for Latin, Kana, and both Japan-specific and shared Han characters (稲/籾/米/飯/あ/漢/中/国); the active IMGUI skin styles and internal `Verse.Text` font/style fields are recorded separately. Reflection allows unavailable Unity font modules/properties to be reported rather than fabricated. A loaded font is **not automatically the active one**. The result is an evidence collection, not a claim that every `HasCharacter` outcome is a release criterion.

The Project private isolated native game must preserve this nonempty text report **together with** the unchanged per-language InfoCard screenshot, source revision, original Unity log, and normal native Pickle/ERROR0 checks. No production translations, visuals, original game assets, upstream font, personal saves, or Steam files may be modified to gather it. **Native tested:** runtime font inventory and active Verse.Text styles were collected in #38691883786 and confirmed again in #38692895907; their interpretation and the failed fallback experiment are recorded above. The visually missing Japanese glyphs remain an OPEN/FAIL rendering gate despite prior loaded string assertions.

### Real bilingual loaded-UI and screenshot evidence — 2026-10-15

**Source / runtime:** exact owner Grains `46eb7f8a468e2387880742fb4c0c4a4e98799a59`, isolated original Linux RimWorld **1.6.4871 rev600**, [Project native Actions #38686645068](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38686645068), retained Actions artifact `amj-integrations-38686645068` (ID 11950487058). Separate actual booted profiles `vanilla-en`, `vanilla-ja`, `vanilla-ccto-en`, `vanilla-ccto-ja`, each **one source-owned UI scenario PASS**, no skipped/flaky tests, exit0, **Unity ERROR0**; 4 InfoCard screenshots per profile, **16 nonempty 1280×720 PNGs**. These tests are independent of the previously confirmed 4×10 V1 gameplay functionality.

**What is verified:** both languages' loaded rice labels and descriptions for `Plant_Rice`, `AMJC_RiceSheaf`, `AMJC_RiceInHull`, `RawRice`; the real Japanese 1.6 Vanilla item `RawRice.label` is **米(生)**, not shorthand 米 (no owner label override permitted), and its four descriptions retain expected semantic paragraph divisions. Actual 2/2/2/4 loaded paragraph counts without literal escaped newline text; the three localized shared-food labels; CCTO-on/off; game-material and stack-graphic non-null/BadTex checks for current grain, flour, cooked-food placeholders and crop graphic existence. English InfoCard screenshots were **visually inspected**: all four descriptions and expected paragraph separation visibly render at 1280×720, without apparent description clipping or missing glyphs.

**IMPORTANT: loaded-language PASS is NOT Japanese visual PASS.** Independent visual inspection of the Japanese InfoCard PNGs shows **missing Kanji/Kana glyph rendering** despite fully resolved Japanese Def strings and native ERROR0: Latin characters and numbers remain visible, while Japanese labels and description characters disappear. This affects at least the Japanese `RawRice` screenshot and requires review of all four InfoCard captures. Treat JP graphical localization **FAIL / unresolved**. A read-only fontconfig / host Japanese glyph availability investigation belongs to the Project native host; no evidence yet that Grains XML or assets are the cause. Do not replace approved historical Japanese text or accept blank rendered screenshots simply because a C# Def assertion passed. Native host-font and Unity fallback diagnosis must precede any host-only remediation.

**Still open:** Japanese glyph/actual InfoCard text visibility, real normal-map-zoom crop and item composition (not merely `MatSingle`), loaded MO/MO+CCTO bilingual UI under original/controlled media, final manual graphic quality and private-play release gate. No text, recipe, gameplay, image, save, original MO media or Steam Workshop source was modified in this test.

### Actual Unity OS-font resolver / embedded resource evidence (2026-10-15)

**Grains owner test-only source:** `fa1f78838a3c3261ea259c1472d249c3d520903a`, `Tests/E2E/GrainsUiSteps.cs`. Production localizations, Defs and textures are unchanged. The original four bilingual UI profiles (English/Japanese × Vanilla/CCTO) each run one real `Dialog_InfoCard` loaded-Def/graphics scenario with four InfoCard PNGs, strict Unity ERROR0 and natural game exit0; they are **not** the V1 40-scenario functional matrix.

- [Project original XDG font run #38733538628](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38733538628): **4/4 owner Pickle PASS**, exit0 and Unity ERROR0, 16 PNG captures, four new `runtime-fonts.txt` resolver reports. The disposable Japanese HOME's verified Noto Sans JP `~/.local/share/fonts` file was selected by `fc-match`; actual Unity `Font.GetOSInstalledFontNames()` returned **16 OS fonts and did not list Noto Sans JP** in either Japanese run. Runtime `Resources.Load` successfully located original three legacy Arial/Calibri Fonts, `LiberationSans SDF`, `LiberationSans SDF misc non-english` TMP assets and `misc non-english characters` TextAsset. Their presence is **not** proof of font usage by the legacy InfoCard or CJK glyph availability.
- [Project legacy HOME path A/B #38734480929](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38734480929): same four actual scenarios, ERROR0/exit0. The identical checksum-verified private Noto was placed only in disposable `~/.fonts` (not system-wide). Fontconfig again selected it, while Unity's own enumeration remained **16 and Noto absent**. **All sixteen corresponding PNG files were byte-for-byte identical** to the XDG trial, confirming neither HOME placement corrects missing Japanese text.

**Interpretation:** Grains' Japanese data is loaded by the original native tests, but its text and base RimWorld Kana/Han remain **visually FAIL** on the isolated Linux 1.6/Xvfb host; these tests do not certify readable Japanese or the author's Windows UI. No production Grains changes, licensed game asset edits, user saves, persistent fonts or Steam state changes. Neither XDG nor legacy HOME placement, `fontNames`, Unity Font-object replacement or glyph-request metadata is a proven fix. Future native glyph/font discovery belongs to Project test-host routing via `Docs/GoldenPaths/PersistentHostIntegrations.md` and `main:Docs/Coordination.md`; use read-only engine/native-resolver evidence rather than repeating the same external-font trials. Retain actual screenshot pixels as the acceptance criterion.

### Unity actual font-file paths and direct-font-by-path A/B (2026-10-15)

**Grains owner diagnostic only; Japanese visual remains FAIL.**
Read-only font-path collector added in source
`e76fbc8a2c5496a96e92c8eebe984298769023e9`
and executed in the original isolated Project
[Actions #38738213368](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38738213368),
real Linux RimWorld 1.6.4871. Its first native attempt crashed with
Mono GC SIGSEGV, exit139 and no Pickle summary; the *one* identical native
retry completed all four original EN/JA × Vanilla/CCTO owner loaded-UI
scenarios **4/4 Pickle PASS, Unity ERROR0, natural exit0**, 16 InfoCard
PNGs, and four complete font inventories. Actual Unity installed-font
API enumerated **16 fonts (DejaVu and Ubuntu only)** and 30 paths all
under `/usr`, omitting the previously verified private Noto JP OTF even
though Japanese disposable HOME's fontconfig selected that exact file.
Runtime `/proc/self/maps` query returned no matching font-related mapped
library, which is not evidence that Unity contains no linked or transient
glyph renderer. All 16 screen captures were byte-identical with previous
family-name Noto experiment; no visible Kana/Han.

A distinct test-only file-path constructor from source
`d5d243661473205c6a53a450d037d4f8fdaf756f` was then executed
[Project #38739733584](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38739733584).
In Japanese only, the same exact official hash-verified 4,533,028-byte
OTF from the disposable HOME was provided as the **full file path** to
`UnityEngine.Font(string)`, not to
`Font.CreateDynamicFontFromOSFont(familyName)`. The created test object
`AMJ_Test_NotoPath` was assigned to 15 live `Verse.Text` font/style
slots, with explicit test-only provenance. **All four** original owner
loaded UI scenarios were again **Pickle PASS, Unity ERROR0, exit0**;
all 16 screenshots were saved. But the actual TextGenerator
`UIVertex` result for **both Latin A and Japanese 稲/あ/中** in the new
object was **0×0 quad size with all UV coordinates zero**. All 8 Japanese
screenshots changed from the earlier Noto family-name trial by losing
even remaining text; all 8 English controls stayed byte-identical.
Thus the constructor's successful return, assigned styles and loaded
translations do **not** certify rendered glyphs. The path-based mode
is **rejected**, and Project runner restores its prior isolated
diagnostic default; test-only code remains dormant. No game, source
graphics, translation XML, user fonts/saves or Steam publication
changes, no Windows-client inference. The next genuine diagnosis
must isolate how the original Unity legacy IMGUI font backend supplies
CJK outlines rather than retrying HOME paths, `fontNames`, family-name
or file-path constructors. Full host evidence and coordination:
Project `Docs/GoldenPaths/PersistentHostIntegrations.md`,
`main:Docs/Coordination.md`.

### Bundled TMP character tables and actual Verse UI font ownership (2026-10-16)

**Result:** original Linux RimWorld 1.6 test host Japanese graphics still FAIL. A failed first run [Project Actions #38759854204](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38759854204) had only test-C# compilation error CS0246, because the optional Unity Font module was referenced as a concrete type; it never ran the game. Grains test-only [PR #133](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/133), merged source `10cca5cba89eb2bed01d3a904996b4844843f545`, fixed that reference through existing Unity type reflection; no production Def or translator source changed.

Actual original Linux 1.6 [Actions #38760513224](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38760513224), Grains exact source `10cca5cba89eb2bed01d3a904996b4844843f545`, **4/4 real Vanilla/CCTO × EN/JA UI scenarios, Unity ERROR0, exit0, 16 PNG screenshots**, archived artifact **11986020363**. All four `runtime-fonts.txt` reports collected and agreed:

- Original bundled TMP `LiberationSans SDF`: 250 serialized character entries and 250 glyphs, initialized runtime lookup 259 entries, serialized and lookup codepoint `A U+0041` **present**; `稲 U+7A32`, `あ U+3042`, `中 U+4E2D`, `籾 U+7C7E` all **absent**.
- Original `LiberationSans SDF misc non-english`: 116 serialized character entries/glyphs and runtime lookup 128 entries; `A` and all four sampled Japanese characters **absent**. These are bounded character lookups, not an exhaustive complete CJK font inventory. The primary asset has two fallback references; their actual full charset remains separately unexamined.
- The **actual** 15 font/style slots of `Verse.Text` during the original InfoCard were **15/15 legacy `UnityEngine.Font`, 0/15 TMP**. Thus neither of the two loaded TMP font assets is the direct font selected by this legacy UI.
- All **16 corresponding InfoCard PNGs byte-for-byte matched** the preceding [#38745487194](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38745487194); no repaired Japanese pixels. Previous TMP `HasCharacter=false` for even Latin A was an insufficient diagnostic; the new raw lookup proves that the base TMP asset *does* contain A but the active legacy InfoCard uses a different font pipeline.

**Explicit test assumptions / exclusions:** Four profiles contain exactly one unchanged UI scenario each; original 2/2/2/4 loaded rice descriptions, Japanese/English labels, required graph/material resolution, four InfoCards/screenshots per profile, original Unity ERROR0 and exit0. No Japanese visual acceptance, current 40-case Grains V1 gameplay rerun, Windows UI proof, TMP fallback-chain completion, font installation, provider/game modification, or Steam publishing is claimed. The remaining graphics issue belongs to the Project isolated legacy Unity font backend, not to Grains localization or TMP asset replacement. Canonical Project host details are in `Docs/GoldenPaths/PersistentHostIntegrations.md`.

### Japanese original InfoCard glyphs restored on private isolated Linux 1.6 host (2026-10-16)

**Actual original native [Project #38766667191](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38766667191), artifact 11988508473, Grains source pinned `10cca5cba89eb2bed01d3a904996b4844843f545`.** The four unchanged Grains original owner UI scenarios (English/Japanese × Vanilla/CCTO) **4/4 Pickle PASS, Unity ERROR0, natural exit0**, all 16 PNGs collected. **Real Japanese glyph graphics, previously FAIL on this host, now render** in the private `/amj grains-ui-font-16` test path. Project's Linux test runner tests a verified, nonshipping Noto Sans JP font in the process's **own rootless Linux mount namespace**, exposing a temporary copy of host OS font files plus Noto at the game's `/usr/share/fonts` view. The underlying system, licensed game and provider media, normal author fonts, Grains Defs/translation/graphics, save files and Steam remain unchanged.

Actual Unity OS font list is **17** in both Japanese child profiles, including Noto Sans JP (previously 16 without it), and **31** paths (previously 30). The actual selected temporary Noto font's `TextGenerator` Kanji 稲 quad is **21×21 px** versus old **2×2 placeholder**; Hiragana あ **19×19 px**, Kanji 中 **20×21 px**. Direct visual examination of the original InfoCards for `Plant_Rice` / 稲, `AMJC_RiceSheaf` / 稲束, `AMJC_RiceInHull` / 籾 and `RawRice` / 米 showed readable Japanese labels/descriptions; all **8 Japanese screenshots differ** from the previous missing-glyph run, while all **8 English screenshots are byte-identical** to baseline. The original loaded-localization source and unchanged 4-profile Grains owner assertions were not altered. Two Japanese InfoCards have a potential `コンテンツソース` heading/divider overlap; this is a separate *layout* matter, not failed font rendering.

**Strict regression rerun:** [Project native #38767986561](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38767986561), artifact **11989468555**, ran the same four profiles **4/4 PASS, Unity ERROR0, natural exit0** with merged Project PR #362's mandatory tests. Each English process must **not** see the private Noto; each Japanese process must enumerate the exact mounted Noto OTF, use the Noto font in the actual Verse style and generate **non-placeholder ≥12px** quads for 稲/あ/中. Both Japanese runs passed with `21×21`, `19×19`, `20×21` px respectively; all **16 corresponding PNGs are byte-identical** to the independently visually reviewed prior successful #38766667191 (8 unchanged EN, 8 repaired Japanese). No font binaries in the archived results. A future 2×2 Unicode replacement therefore **fails the private native test gate**, rather than allowing Pickle-only false acceptance.

**Boundary:** This is a native rendering fix for the Project's *private, opt-in* `/amj grains-ui-font-16` test lane, **not** an alteration of the regular `/amj grains-ui-16` default profile, product font pack, Mod or Windows player. A strict non-placeholder Japanese TextGenerator/Unity font visibility gate is also owned by Project; source/string PASS alone never implies visual acceptance. Other Grains UI clipping/paragraph content, 1.7 native profiles, MO original media, final art and private player/release decisions are **not** newly approved by this result. Formal host implementation and outcome: Project `Docs/GoldenPaths/PersistentHostIntegrations.md` and current `main:Docs/Coordination.md`.

### Native Japanese InfoCard typography matched to original fonts (2026-10-16)

**Grains owner developer-test change only; production source and text are unchanged.** The prior private Linux 1.6 Noto font pipeline made Japanese glyphs readable but applied one **20px** face to all fifteen Verse.Text font/style slots, enlarging the section headings and overlapping the built-in `コンテンツソース` divider for 稲束 and 籾. An original owner native [#38774478869](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38774478869) measurement logged `Calibri_tiny 12px`, `Arial_small 14px`, `Arial_medium 21px` and three GUIStyle `fontSize=0` (the original font size is inherited). That initial run had **3/4 original scenario PASS** then Linux process exit139 before the last original game Pickle report, so it is **not a 4/4 test PASS**.

Grains [PR #137](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/137), merged exact `e49717489333a1367500c37feec1ab3a8408d566`, derives the actual original sizes at runtime and uses three verified temporary Noto OS-font objects (**Tiny12, Small14, Medium21**) at corresponding original indices in all five Verse.Text font/style arrays (3×5=15). TextGenerator collects three original fonts plus three temporary Noto faces, with actual Unicode and quad evidence. The old direct-file constructor remains disabled, and no font is added to Grains' install/Steam payload.

Actual original Linux 1.6 UI [Project Actions #38776287374](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38776287374), artifact **11994475764**, immutable exact owner source `e49717489333a1367500c37feec1ab3a8408d566`: **4/4 original EN/JA × Vanilla/CCTO loaded-InfoCard Pickle scenarios PASS, Unity ERROR0, normal exit0**, and all **16 PNG captures** present. Reinspection of the four original Japanese InfoCards (稲・稲束・籾・米) shows legible Japanese at the original UI scale; `コンテンツソース` no longer overlaps its section divider on 稲束 and 籾. All **8 English PNG bytes** match previous [#38767986561](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38767986561), whereas all **8 Japanese PNGs changed** due to corrected font sizes. The Project exact CJK glyph gate and new original-slot size/object checks passed in both Japanese profiles.

**Scope:** This corrects the Project-host **opt-in** `/amj grains-ui-font-16` test-only typography, not regular `/amj grains-ui-16` or the author's original Windows UI, game language assets, Grains XML, item graphics or Steam distribution. Other UI text/paragraph review, MO/provider original-media and RimWorld 1.7 tests remain separate. Canonical host implementation/constraints: Project `Docs/GoldenPaths/PersistentHostIntegrations.md`; Project main Coordination tracks the cross-owner handoff.

### MO/MO+CCTO bilingual InfoCards using verified temporary font and isolated PCM audio (2026-10-16)

**Test-only Grains owner revision:** exact `e49717489333a1367500c37feec1ab3a8408d566`, one existing `grains-ui.feature` original loaded Def/material/label/description InfoCard scenario per profile; the production source, language text and sprites are **unchanged**. Project [PR #385](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/385), merged `05c4f53c188420eac64a79b6a08961b925971202`, adds an on-demand **original-media strict** `/amj grains-ui-mo-font-16` operation that runs actual staged MO / MO+CCTO × English / Japanese, reusing the officially verified temporary Noto font and child-only `/usr/share/fonts` namespace. The **first actual original-media run [#38783420759](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38783420759), artifact 11997150432** evaluated `mo-en`: original owner Pickle **1/1 PASS**, game exit0, four InfoCard screenshots captured, but two authentic upstream SYR/MO `FMOD::Sound Unsupported file or audio format` **Unity ERRORs**. Hence strict **FAIL**, stopping before the other three profiles. This is a known independent original-MP3 Linux 1.6 issue, not a failed Grains English label, Japanese glyph, or MO provider preflight; **do not report original-media four-profile PASS** or waive those errors.

The separate Project [PR #388](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/388), merged `4c32ebdbfac742bef6a146aba3f74051dc236f09`, reuses the established `mo_wav_owner16` **disposable complete game clone**, exact original provider/package provenance, hash-verified official Noto Sans JP 4,533,028-byte OTF and hash-pinned imageio-ffmpeg 0.6.0 Linux wheel. Inside **that clone only**, it transcodes upstream SYR `UI/DebugGoose/Honk.mp3` and MO `Sounds/DankPyon_FlaskBreak.mp3` into 44,100Hz stereo 16-bit PCM WAV, deleting the MP3 *copies* there. The original staged MO/SYR media hashes remain unchanged. Its manual private command is `/amj grains-ui-mo-wav-font-16`, a **temporary-audio functional UI** lane, not original MP3 release acceptance.

**Actual Linux RimWorld 1.6.4871 [#38784551164](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38784551164), artifact 11998061567**, ran the unchanged owner UI scenario separately as `mo-en`, `mo-ja`, `mo-ccto-en`, `mo-ccto-ja`: **4/4 Pickle PASS, Unity ERROR0, natural exit0, zero failed/skipped/flaky**, four original rice InfoCards per boot = **16 PNG screenshots**. Japanese live Unity Noto font detection and actual Verse Tiny12/Small14/Medium21 slot mapping passed; generated Kanji 稲 **21×21px**, kana あ **19×19px**, Han 中 **20×21px** (not 2×2 replacements) for each Japanese profile. Actual screenshots for 稲・稲束・籾・米 show readable Japanese and `コンテンツソース` divider no longer intersects the heading; MO and MO+CCTO differ in expected loaded provider/UI context. In English controls, Noto is not loaded. The test did **not** change production translations, XML Defs, game fonts, original MO/SYR source, Steam, saves or globally installed fonts.

**Acceptance distinctions:** the *font* and original loaded Japanese InfoCard functionality have native evidence for all four Base/CCTO and all four MO/MO+CCTO language combinations using their respective opt-in font routes. This is **not** eight independent V1 gameplay matrices: each here is one owner UI scenario. Current-source MO 10-case gameplay, original MO audio ERROR0, RimWorld 1.7 JP font support, physical audio audibility, full game-map crop textures, Windows visuals and release approval are independent. Preserve original-media strict MO **FAIL** even when the WAV-clone UI function succeeds. Canonical reusable host implementation: Project `Docs/GoldenPaths/PersistentHostIntegrations.md`; cross-repo handoff: Project `main:Docs/Coordination.md`.

## Next consolidated native food-Bill matrix: missing execution coverage (2026-10-13)

**2026-10-13 implementation update — SOURCE ONLY:** `Tests/E2E/GrainsSimulationSteps.cs` now includes an independent `CommonFoodJobs` / `CommonFoodBill` scenario in every real-provider profile. It stages exactly the declared 0.5/2.0 Nutrition in **one input Def** or **two different permitted input Defs** and has the *actual* `WorkGiver_DoBill` produce 1/4 food servings. For each of 飯、粥、粉食, both batch sizes and both input modes run once: **12 native Bill cases per profile**. The fixture uses `ThingMaker` to create **test-only input stacks** rather than harvesting them; the separate unchanged harvest/processing E2E scenario supplies end-to-end crop evidence. Product and per-ingredient counts must match, and setup/teardown must restore its isolated map. At the time of this seven-case expansion, the four profile lists expected **7 scenarios per profile (28/28 across four profiles)** rather than the preceding historical six; the current ten-case V1 suite is specified in the reconciliation below. The 1.7 runner prints the dynamic configured scenario count. `Tests/Production/test_grains_mixed_meals.py` guards scenario wiring in CI; it **does not compile C# or prove native gameplay**. A new exact-head 1.6/1.7 runtime, ERROR 0, job timing and UI remain OPEN. The prior 24/24 real 1.7 evidence is historical, not a current 40/40 result.

**Current evidence versus requirement.** A real four-profile run on RimWorld 1.7.4976 rev1043 (recorded in `Testing/Native17-20261013.json`, with production CCTO title fix) passed 24/24 named scenarios and had zero runtime ERROR entries **for that recorded source revision**. The existing `GrainsProfileSteps.cs` checks the three common-food Recipes' **loaded definitions**, 1x/4x output/count/allowed input families and `allowMixingIngredients`. In the earlier recorded revision, `GrainsSimulationSteps.cs` executed flour-specific Bills and standard `CookMealSimple` on harvested grains but did **not** execute two-input common-food Bills. The seventh native C# scenario was subsequently added, yet its **runtime outcome remains UNTESTED** on the current revision; neither the older 24/24 nor source-only wiring counts as gameplay PASS. Localized source/labels changed again after some captured native evidence; no current-main loaded JP/EN UI PASS is recorded.

**Add these assertions to the existing *consolidated* Grains suite** (native code before the next full run, not an independent launch for each text change):

| Test premise / actual-game operation | Expected result |
|---|---|
| Base profiles `vanilla` and `vanilla-ccto`, MO profiles `mo` and `mo-ccto`; RimWorld 1.6 current and optional 1.7 test build | Real providers, no MO fixture; one recorded exact source fingerprint and game version per profile; no missing/duplicate key or runtime ERROR |
| `AMJC_CookMixedGrainMeal` and `...Bulk` via actual pawn `DoBill` | Same loaded `AMJC_MixedGrainMeal` product, exactly 1/4 servings; 0.5/2.0 total ingredient Nutrition from eligible **hulled edible grain**; Work 240/960, 3-day rot; a *single* grain is sufficient; at least one run must actually consume **two different grain Defs** |
| `AMJC_CookMixedGrainPorridge` and `...Bulk` via actual `DoBill` | Same loaded `AMJC_MixedGrainPorridge` product, exactly 1/4 servings; total 0.5/2.0 Nutrition; Work 200/800, 1.5-day rot; both single-grain and two-grain use verified |
| `AMJC_CookMixedFlourFood` and `...Bulk` via actual `DoBill` | Same loaded `AMJC_MixedFlourFood` product, exactly 1/4 servings; total 0.5/2.0 Nutrition; Work 300/1200, 2-day rot; both single-flour and two-flour use verified; Base wheat flour is AMJ-owned, MO wheat flour is `DankPyon_Flour` |
| Negative recipe-family and dedicated-meal guards | Raw kernels must not satisfy flour-food recipes, flour must not satisfy grain meal/porridge recipes, and the five named flour dishes must reject the *wrong* flour. Common 3 recipes may combine only within the accepted family, not cross categories |
| Real ingestion after cooking, where native test tools can observe it | Common 3 foods carry **no extra Mood thought**; five dedicated flour dishes use `AMJC_AteFlourFood` with +2, 0.5-day, nonstacking. Vanilla `簡単な食事` 0 / `手の込んだ食事` +5 / `豪華な食事` +12 are **comparison benchmarks**, not new Grains dishes, cloned ThoughtDefs, or extra skill gates |
| Japanese and English loaded `ThingDef`/Info and 1x/4x Bill labels | JP `飯` / `粥` / `粉食` and matching English `cooked grains` / `porridge` / `flour dish`; approved crop/rice descriptions and paragraph layout from the 'Author-deferred batch' above, no missing translations or raw `\\n` escapes |
| Runtime rendering and reports | Existing accepted PNGs untouched; inspect BadTex / graphic stack material and ordinary-scale appearance, capture actual `Player.log`, Pickle summary, outputs, version/provider manifest, and zero ERROR; include individual expected test items/status, not only overall PASS |

**Test data integrity:** Mixed-Bill jobs must prove a *real* `WorkGiver_DoBill` consumed measured quantities of each different permitted ingredient and completed the product count. Checking only `allowMixingIngredients` or artificial RecipeDef projections is insufficient. Prefer ingredients obtained through native harvest→thresh→hull→mill in isolated staging; when controlling stack quantities, record any test-only arrangement separately and do not alter production Recipes/Defs. If expanding the six existing Pickle scenarios, adjust the feature names, summaries, watchdog budgets, assertions and reproducible source-state witnesses together; avoid prolonging an already 240-second internal native job deadline without evidence. Unfinished/new scenarios do not count as PASS. Distinguish 1.6 from unstable 1.7; CCTO 1.7 success is not a substitute for current 1.6.

**Execution readiness:** The shared AMJRuntime WSL2 host has a successful **Vanilla-only 1.6 native process/log smoke** on author-provided evidence, but Project `amj-rimworld` self-hosted runner service/label/online readiness is not established by that result. Do not submit a native runner job until those conditions and actual provider staging are confirmed. Preserve the user's deferred one-batch native policy. This paragraph records the older acceptance checkpoint before the subsequent C# scenarios were added. Host execution, native-game PASS, art or production code changes are **not** claimed by this documentation.

## Linux 1.6 native V1 food Thought defect (2026-10-14)

The actual [Project Linux 1.6 host run #38506566985](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38506566985) compiled and loaded this Grains source at `b11cf8085758d10289d26878e392c9a61611e3a9` with original 10-scenario Pickle feature. **Vanilla 9/10 PASS**, no skipped or Unity ERROR. The native pawns' ingestion check **failed** for `AMJC_MixedGrainMeal tasteThought differs`; source code also showed all three generic foods inherited `MealFineBase`, whose Vanilla `AteFineMeal` was not suppressed by omitting the child `tasteThought` element. This would award an unintended Fine Meal thought despite the approved generic MealSimple-equivalent Mood 0. The three generic food ThingDefs therefore use their ancestor **`MealCookedIngredientless`**, retaining all existing explicit ingestible preferability, nutrition, rottable comps, ingredient/graphic Defs and recipes, but removing inherited Fine Meal tasteThought. The five specific flour foods retain `MealFineBase` with explicit `AMJC_AteFlourFood` (+2/half-day/nonstacking) untouched. The existing real pawn Ingest test remains strict; static XML tests now guard parent selection. **The original #38506566985 is FAIL**, not 10/10; the CCTO profile was not run. The initial failing run remains FAIL; it is superseded only for the fixed source by the separate native result below. MO/MO-CCTO stay untested.

### Verified corrected V1 native acceptance — source-locked Linux 1.6 (2026-10-14)

Actual [Project Actions #38507556257](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38507556257) compiled and loaded the **real owner source revision** `9f8bc72eb16faacfd1219f18e91b489294af224c` (Grains [PR #95](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/95) merged production correction). On dedicated isolated **Linux RimWorld 1.6.4871 rev600** with actual CCTO provider and official Pickle/Quickstarts, `grains-vanilla.feature` **10/10 PASS** and `grains-vanilla-ccto.feature` **10/10 PASS**, **20/20 total**, each exact named scenario executed once, 0 failed/skipped/flaky, `exitReason=passed`, native game process exit 0, no timeout and classified Unity ERROR 0. Individual native scenarios cover:

| Current V1 owner scenario premise | Actual tested operation and expectation |
| --- | --- |
| Provider isolation | Only real Vanilla/CCTO as selected, no MO, API fixture or simultaneous old production Core |
| Primary loop and crop/recipe metadata | Crops resolve harvested ThingDefs; usable inputs/products; beta processing bench/stove and rice thresh/hull conditions |
| Optional cold tolerance | CCTO extensions absent in Vanilla, present/consistent in CCTO |
| Wheat and grain flour/food contracts | Actual loaded non-MO wheat, milling, recipe, product, nutrition and food definitions |
| Seven-grain niches | Controlled 3×3×3 fertility/temperature/growing-season utility projection; each grain retains some tied-or-better crop role (not natural-year balance) |
| Native harvest, processing and specialty flour food | Actual pawn jobs including harvest→thresh→hull→mill→cook (not just recipe XML) |
| Single- and mixed-input common foods | Three food families × 1/4 servings × single/two ingredient choices = **12 actual DoBill combinations** |
| Alternate benches | Real fueled-stove/processing-spot thresh/hull, Base stove milling, loaded electric stove recipe exposure; no electric-stove job claim |
| Wrong ingredient rejection | Actual `WorkGiver_DoBill` must reject incorrect raw/flour ingredients for tested recipes |
| Pawn ingestion thought | All three generic foods award no bonus Food Thought, and tested two flour-specific dishes award `AMJC_AteFlourFood` +2, 0.5 day and nonstacking. Remaining three specialty foods retain structural/static contracts, not all five native pawn ingestion jobs |

The first [failed #38506566985](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38506566985) is retained as original **9/10** source defect evidence. The successful **20/20** is exclusively the corrected source. The two remaining owner-required profiles **MO and MO+CCTO = NOT RUN** (real MO provider absent), so the complete four-profile requirement **40/40 remains OPEN**. Latest JP/EN Info UI, actual graphics aesthetics, save/long play, natural seasonal weather, arbitrary modpack interoperability, installed Steam Workshop payload and publication are independently **NOT verified**. The Project private Issue #39 `/amj grains-v1-16` command is the canonical reproducible execution path; no PR-event automatic launch is enabled.

## V1 test-inventory reconciliation (2026-10-13)

The ten current real-provider scenarios are: provider load/isolation; primary harvest/processing/food Def contracts and nine grain graphics; conditional CCTO; wheat/flour/food Def contracts; seven-crop environmental niches; actual harvested-source production Bills, seasonal growth and five Vanilla simple-meal inputs; actual single/two-ingredient common-food Bills (3 families × 1/4 servings × 1/2 input kinds = 12 native cases); alternate bench native jobs; wrong-ingredient native rejection; and pawn ingestion thoughts. Four configurations are mandatory: Vanilla, Vanilla+CCTO, MO, MO+CCTO. **40/40 is the current required count, not a recorded current-main runtime PASS.**

Review of coverage and scope:

| Requirement | Current automated check | Gap / action |
|---|---|---|
| Vanilla independence, MO provider reuse and optional CCTO | Four real-provider profiles and XML projection; no fixture allowed in the real suite | Exact-head 1.6/1.7 loaded-provider and ERROR-0 result outstanding |
| Seven crops, upland rice, climate/soil choice | Environmental-niche and native harvest/seasonal checks | Natural season/long-play balance is not simulated by the 27-cell controlled grid |
| Thresh → hull → mill → cook, five flour-specific foods | Native harvested-source production scenario, loaded Recipe contracts | The source now includes stove/processing-spot thresh/hull DoBills and Base stove milling. Powered ElectricStove DoBill and complete current-head four-profile results remain unverified; no loaded job PASS is claimed |
| Eight foods, 16 Bills, shared-family mixing | Static ingredient, product, nutrition, cooking work and rot contracts; native common-food scenario handles 12 single/mixed Bill cases | Exact-head native result outstanding. Eight wrong-only ingredient cases now have native WorkGiver assertions in source; their execution is unverified |
| Mood rules and food preference | Static common foods no tasteThought, five specialties +2 definition and Vanilla baseline comparison | Five native pawn Ingest jobs and shared Thought/stacking assertions are implemented in source, but live execution remains unverified; Vanilla meal tiers are comparison baselines, not Grains recipes |
| JP/EN item and 1×/4× Bill descriptions | Source parity and loaded-UI requirements | Full latest bilingual Info/paragraph/clipping checks require consolidated runtime/visual review |
| Production graphics | PNG integrity/static path validation plus loaded texture and StackCount checks | Normal-zoom aesthetics are separate; deferred 14 plant and five sheaf *redesigns* are not test blockers |
| Save migration | No deployed Grains saves | **Out of scope**; do not restore obsolete Core/provider-removal migration or scenario ownership gates |
| Equipment specialization, water paddies, future recipes | Post-V1 development owned elsewhere | **Out of V1 scope**; do not require purpose-built bench artwork, paddy simulation or future meal recipes for these functional tests |

Do not count duplicated source-XML assertions as independent gameplay evidence. The old eight-case fixture was reduced to six grain cases; it is not another required ten-scenario native profile. Keep current-source build, full native logs, distribution and author visual judgment as separate results.

**Additional source-only cases:** The 8th performs four native fueled-stove/processing-spot thresh/hull jobs per profile and one extra Base-only stove mill Bill; it checks ElectricStove loaded recipe exposure but not powered electric jobs. The 9th rejects wrong-only ingredients for 3 common and 5 specialty food Bills via native WorkGiver. The 10th uses real pawn Ingest jobs for 3 common meals and 2 specialty flour foods and verifies a single +2/half-day nonstacking memory. Inputs and ready-made meals in these tests are test-only staged items, not harvested evidence. Exact-head C# compilation, 40/40 native jobs, and zero-ERROR reports are still NOT RUN.

## Loaded grain-icon check (2026-10-11)

The existing real-provider primary-loop Pickle scenario checks the **nine current grain states**: hulled/edible millet, hulled/edible buckwheat, hulled/edible barley, edible wheat, rice in hull and Vanilla `RawRice`. For all four existing profiles (Vanilla, Vanilla+CCTO, MO, MO+CCTO), it resolves the loaded `ThingDef` graphic path/class, checks each `_a` PNG with Unity `ContentFinder<Texture2D>`, and resolves `Graphic_StackCount.SubGraphicForStackCount` materials for low, middle and high counts. It rejects missing/null/BadTex assets. This runs inside the existing Unity-main-thread `RuntimeThread.Run` entry point, retains the primary-loop check within the current ten-scenario suite, and does not pin production PNG bytes or require redundant `_b/_c` PNGs.

A successful C# compile and installed-game four-profile Pickle run with complete ERROR-zero logs are required to call this a runtime PASS. The check confirms loaded texture/material selection, **not** visual quality or framing at normal zoom. The author has temporarily deferred high-resolution source archival; no source-master or normal-zoom acceptance is implied.

## Windows entry point

From the development checkout, with Steam running and the test helpers installed:

```bat
run-grains-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"
run-grains-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld" vanilla
```

The default runs all four profiles sequentially and records every result even if
an earlier profile fails. Optional second argument: `vanilla`, `vanilla-ccto`,
`mo`, `mo-ccto`, or `all`. The normal entry point uses the existing Windows
private-desktop launcher without switching the input desktop. Unity rendering
stays enabled; do not replace it with `-nographics`.

| Profile | MO provider | CCTO provider | API fixtures |
|---|---|---|---|
| vanilla | absent | absent | absent |
| vanilla-ccto | absent | installed CCTO | absent |
| mo | installed MO | absent | absent |
| mo-ccto | installed MO | installed CCTO | absent |

All profiles use installed Harmony, RimLogging, Pickle and Quickstarts, a staged
AMJ target, and the separate AMJ test mod. Only declared hard dependencies are
added recursively; active loadAfter/forceLoadAfter constraints are then ordered.
For example, current MO additionally requires VEF and Processor Framework.
Installed optional mods and DLC are not enabled just because they are installed
or enabled in the player's normal configuration. Missing dependencies, ambiguous
duplicate package installations and dependency/load-order cycles fail setup.

## Staging boundary

`build-e2e.bat <RimWorldRoot> <profile>` uses
`Scripts/Stage-GrainsTestProfile.ps1` for the four real-provider profiles.
The disposable `AncientMedievalJapanCore.E2ETarget` has the existing test packageId,
no hard MO dependency, and profile-specific provider load order. Its Defs,
Patches, Textures, Languages, Compatibility and BaseWithoutMO are copied unchanged. Assemblies, versioned 1.6
content and loadFolders are copied if present. Production Core is not activated
alongside the test copy. The test-mod assembly build is shared with the legacy
runner; the real profiles neither build nor load the MO/CCTO API fixtures.

Real profiles do not apply `E2E_Graphics.xml`, remove MO references, substitute
recipes, or manufacture missing provider Defs. Base MO Def references were isolated in step 2. Vanilla
now has initial wheat/flour/milling/food XML, with pending runtime/art/prose verification.
Those gaps must remain visible until runtime migration resolves them.

`build-e2e.bat <RimWorldRoot>` and the existing `run-e2e.bat` / `run-tests.bat`
rewrite only the staged loader MO condition to the fixture packageId and
retain the historical MO+CCTO lightweight-fixture suite and eight assertions.
That suite remains useful for narrow XML contracts; its PASS does not prove
real MO/CCTO integration or Vanilla independence. Do not run the legacy and
Grains runners concurrently: both rebuild the same staged test-mod directories.
Generated test mods must remain disabled during ordinary play. `clean-e2e.bat`
removes the generated target, test mod and both fixtures when no test is running.

## Evidence and failure gates

Each profile gets fresh isolated SaveData and Pickle output under
`TestResults/Grains/<profile>/`. `matrix.json` records launch state, exit code,
setup diagnostics and report paths. The report includes:

- `Player.log` and Pickle `summary.json`;
- the profile manifest and resolved installed package/root list;
- source HEAD, existing source fingerprints, and SHA-256 hashes of the staged
  runtime target, compiled test assemblies/features and non-Vanilla providers.

The current profile suite requires exactly ten grain-only named scenarios, 10/10 passes and
zero skips (40/40 across four profiles). Historical 6/6 and 24/24 results apply only to their recorded revisions. Assertions check actual provider presence, fixture/production-Core
absence, resolved primary crop/recipe/simple-meal contracts, loaded wheat/flour/milling/food contracts, and presence/absence of AMJ cold-tolerance extensions. Runtime log checking
fails on **any ERROR**, including provider errors. Both summary and log gates
run after a launched game even if the process or scenarios already failed.
Missing fresh output is a failure. The Grains game-process watchdog is nine minutes per profile;
Pickle has a separate **seven-minute run-wide limit**, exceeding the production scenario's
270-second allowance. The private-desktop matrix watchdog remains forty minutes and
terminates its own tree. The legacy fixture suite retains its four-minute Pickle default.

These are **migration smoke tests**, not the completed release matrix. Subsequent
implementation must prove actual harvest/Bill cooking behavior, six-grain environmental-choice
regression, and save compatibility cases. None is inferred from loaded-contract smoke
scenarios or from the old fixture suite.

## Tooling regression

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File Tests/test_grains_profiles.ps1
```

This launches no game. It stages all four profiles in a temporary directory,
verifies exact runtime byte preservation and production/player-config immutability,
tests provider isolation and ordering, rejects wrong-suite summaries, and checks
missing/forbidden dependencies and cycles. GitHub's Windows PowerShell 5.1 job
also parses the scripts and runs this regression. It does not compile the game
test assemblies or claim runtime success.

## Conditional runtime folders and static contracts

AMJG-owned-field precedence (2026-10-09): Base and MO loaded profiles must both retain research-free `AMJC_Plant_Barley`, research-free `AMJC_GrainProcessingTable` costing exactly 30 Steel, and the four shared AMJ-chosen placeholder graphic paths. The earlier MO-specific research and IronIngot substitutions for those AMJG Defs are removed. MO-specific wheat/flour providers and thresh-time Straw integration remain unchanged. The XML projection and negative regression tests enforce these conditions; only an installed-game Pickle run establishes real loaded-Def behavior. Production hard MO dependency is now removed; old-save provider removal is out of scope because no Grains saves exist.

Production `loadFolders.xml` loads `/`, activates
`Compatibility/MedievalOverhaul` only for `DankPyon.Medieval.Overhaul`, and activates
`BaseWithoutMO` only when that provider is absent.
Real-profile staging preserves the loader and conditional files byte for byte.
Only the legacy fixture stage changes its disposable loader conditions via
`Prepare-FixtureLoadFolders.ps1`; production XML never names a fixture.
Workshop archives must include the loader and all conditional runtime files.

`Tests/validate_grains_base.py` verifies zero MO identifiers in Base Defs/localization
and compares all 38 pre-split explicit AMJC contracts to their MO projection.
The golden fixture records source commit and hashes. `amj_profile_xml.py` and
`AmjProfileXml.ps1` project only AMJ-owned Defs and the Base-difference Add/Replace
patches. They do not simulate external MO wheat patches, XML inheritance, loaded
Def references, localization resolution or texture loading. Existing wheat-patch
checks remain separate; actual game tests are required for those boundaries.

## Scenarios ownership boundary (2026-10-14)

Scenarios is the sole owner of `New Village`, its player Faction/PawnKind, starting equipment/research, language files and MO/Grains start compatibility. **Grains does not ship or load the former legacy scenario even when Scenarios is absent.** There are no deployed legacy saves to migrate. Formal Grains owner boundary: `Docs/ScenarioExtraction.md`.

The current real-provider Grains matrix consists of **ten grain-only Pickle scenarios per Vanilla/CCTO/MO/MO+CCTO profile**, not village-start tests. The former separate eight-case fixture is reduced to its six grain Stage A cases; village assertions, legacy Scenario quickstart and dedicated scenario fixture are removed. Starting-scenario runtime tests remain Scenarios-owned. Before declaring native success require the actual source revision, all cases, isolated logs and ERROR-0 evidence. A static CI pass is not runtime evidence.

## Step 3 chain validation and outstanding release gates

`python Tests/validate_stage_a.py` includes Base/MO chain validation.
`python Tests/test_grains_chain.py` rejects nutrition multiplication, extra milling
products, bread-like storage, cooking research gates, broken MO flour routing and
duplicate fallback defs. Shared new AMJ contracts are allowed explicitly; the
original 38-contract hashes are still compared unchanged.

The sixth Pickle scenario checks loaded fallback/provider isolation, wheat
harvest/thresh definitions, flour nutrition and recipe users, the standard mill
research gate, CCTO wheat extension, and food input/output/storage/Mood. This is
loaded-contract coverage, not completed growing/harvesting/milling/cooking Bills.
C# compilation and real-profile execution remain pending in an installed game.

MO integration removes the standard millstone's research prerequisites so the
minimum Grains flour loop is research-free. This external patch must be audited
against actual MO source and the final loaded Def; it is excluded from the
AMJ-only static projection. New descriptions are blank pending Japanese author
review before English translation. All new graphics are documented development
references (existing AMJ or Vanilla); no final image was generated. Environmental
choice, save migration, provider/parent inheritance and graphics require runtime
verification before claiming current-revision runtime success or public-release readiness (About.xml's hard MO requirement was separately removed by author instruction on 2026-10-10).

Repeat the actual-provider XML audit with `python Tests/validate_grains_chain.py --mo-root <MO-root>` against the installed or supplied MO 1.6 sources. This checks source targets/nutrition, not the resolved runtime Patch result.


## Environmental and production-job regressions (2026-10-07)

`Tests/validate_grains_environment.py` and `Tests/Fixtures/Grains_Environment.json`
fix 27 representative cells per Base/MO profile: fertility 0.5/1.0/1.4,
temperature 10/20/30°C, and 5/10/20 **effective growing days**. See the owning
balance specification in `Docs/Balance/Crops/GrainsEnvironment.md` for assumptions
and source provenance. This is an analytical finite-season model, not actual
weather, resting or frost survival. Regression mutations cover fertility exclusion,
season length, high-temperature suitability and one-grain dominance.

The seventh Pickle scenario uses loaded crops and the actual
`PlantUtility.GrowthRateFactorFor_Fertility` / `_Temperature` utilities over the
same cells. Every grain must win at least one cell; no grain may win >=2/3 of viable
cells, counting ties as wins. It does not advance a growing plant through a season.

The sixth grain-only scenario requests `AmjStageAQuickstart`, independently of New Village.
`GrainsSimulationSteps.cs` creates mature plants for all six grains (an extra Soba
plant supplies its x10 processing chain), one capable Plants/Crafting/Cooking-20
worker, processing table, fueled Vanilla stove, profile-owned mill and a fuelled Campfire. Existing
Quickstart pawns are temporarily despawned; only this disposable map is prepared.
The test never creates harvested ingredients or recipe products. Native designated
harvest jobs create the sheaves. Eleven native `DoBill` jobs then thresh/hull,
mill and cook millet, buckwheat and wheat into all three minimum foods. The MO
wheat milling step uses `DankPyon_CraftFlourBulk` on the actual MO millstone.

Bills come from the loaded recipes, ingredient selection comes from loaded
`WorkGiver_DoBill` workers, and ingredients/products are counted across map stacks
and the test worker's carried stack. Each Bill must finish one iteration, consume
the expected ten ingredient units and produce exactly its declared output counts.
Rice bulk threshing and rice bulk hulling are also executed as real Bills on a
fueled Vanilla stove in Base and MO; other processing Bills still use the AMJ
processing table. All 18 single/bulk grain-processing recipe users are checked
in loaded Defs for both stoves and both AMJ processing stations.
For V1 non-MO milling, the same test fixture also runs the AMJC_MillMillet
Bill at the FueledStove through the loaded Cooking WorkGiver; the other four
AMJC milling Recipes retain the existing AMJ manual millstone route. Loaded
Def assertions require exactly three bench users (AMJ stone, FueledStove and
ElectricStove) for all five Base milling Recipes, while all four AMJ MO
milling Recipes remain MO-millstone-only. MO wheat uses its upstream native
flour Recipe. Real installed-game and ERROR-0 verification of the new stove
milling job remains pending.
Harvest and milling/cooking must add no Straw/Hay; MO threshing's declared Straw
is allowed. The test checks available table recipes and rejects jobs that end
without their outputs. It does not call product-generation helpers itself.

To avoid reliance on normal game speed or Pickle pause state, the test pauses the
fresh map and posts batches of at most 64 `TickManager.DoSingleTick()` calls to the
Unity main thread, yielding between batches. Each job is limited to 20,000 ticks;
the production scope has a 150-second wall limit within the existing 300-second
profile process timeout. Cleanup ends the worker's job, removes setup objects,
restores terrain, parked pawns and the prior speed. Rendering, private desktop,
provider isolation, source hashes, strict summary and ERROR gate remain required.

**Verification boundary:** source/tooling/static checks can run here. This workspace
has no installed game/assemblies: the new C# has not been compiled or run against
RimWorld. Only an actual six-scenario 6/6, zero-skips, runtime ERROR 0 run of each
real profile establishes that these job tests work. Mature-plant setup does not
prove sowing, calendar growth, cold death, mood ingestion or save migration.


Production-job source review also found missing work routing for AMJ's two
processing benches and Base manual mill. Shared `AMJC_DoGrainProcessing` and
Base-only `AMJC_DoGrainsMilling` now connect those benches to native
`WorkGiver_DoBill` under Crafting. MO mill routing remains MO-owned. Static
positive/negative contracts cover giver class and fixed targets; actual jobs are
still uncompiled/unrun here. The step source preserves asynchronous cleanup while
remaining compatible with the repository's Framework C# 5 compiler syntax.


## Complete scenario extraction

The former `LegacyStartingScenarios/` provider, its two loader conditions and extraction-prototype scripts/tests were removed from Grains (2026-10-14). Grains now has zero start-def providers; Scenarios is optional and supplies its own content. Static Base/MO checks assert that no `AMJC_NewVillage`, `AMJC_PlayerVillage` or `AMJC_Villager` Def is owned by Grains, and the subscriber filter does not allow legacy content.

Keep the existing grain-only profile, Bill and CCTO tests. Scenarios owns testing of a new village start across its four independent profiles. No save migration testing is needed for unpublished Grains/Core starts.

## Upland rice scope extension (2026-10-08 audit)

Grains owns Vanilla `Plant_Rice` as **upland rice (陸稲)** through the committed Production `Patches/UplandRice.xml`, retaining `RawRice` without a duplicate item Def. XML and seven-crop analytical regression exist; loaded-Def, sow/harvest and real-game success remain unverified.

The real-provider suite retains **six named scenarios per profile**, but its C# Pickle source and static analysis now cover **seven crops**. No four-profile game execution has occurred. These tests must pass at runtime before standalone release:

- the loaded `Plant_Rice` is the single Grains rice Plant in both Vanilla and MO profiles, with the V1 field-growing settings; the Japanese label is `稲` and the English label is `rice plant` (verify both UI languages separately, not merely the underlying static strings);
- upland rice is sowable on normal ground with the implemented `Ground`-only sow tag and without `Hydroponic`;
- a native harvest produces `AMJC_RiceSheaf`; real threshing/hulling Bills yield Vanilla `RawRice`, which remains accepted by ordinary meals;
- CCTO profiles retain CCTO's existing `Plant_Rice` minimum-growth / cold-death behavior without a duplicate Grains cold-tolerance extension;
- the analytical and loaded-Def environment regressions are extended from six crops to seven and still reject a single crop dominating the representative cells;
- the real-profile matrix exercises at least one native upland-rice sow/harvest path and retains the strict runtime ERROR gate.

Future Rice Cultivation integration is not part of the Grains standalone matrix. **It must reuse the same Vanilla `Plant_Rice` rather than define a separate water-rice crop.** Its owner must test normal-field and paddy-field sowability, field-water growth/yield modifiers, absence of duplicate rice Plant/food/flour Defs, and retention of the Grains `AMJC_RiceSheaf → AMJC_RiceInHull → RawRice` chain when Grains is present; standalone Rice Cultivation must use Vanilla `Plant_Rice`/`RawRice` without Grains intermediate dependencies. Include MO/CCTO/Waterworks optional-provider and save/load regressions.


### Seven-crop Pickle regression (2026-10-08)

The four real-provider profiles retain **six named scenarios each**, now covering **seven crops** including Vanilla `Plant_Rice` in the environmental-niche and native designated-harvest checks. Loaded runtime assertions verify the upland rice growth/fertility/temperature balance, Ground-only sow tags, `RawRice` meal compatibility, and exactly one CCTO extension with a -1°C threshold only when CCTO is present. A temporary normal-soil growing zone checks engine sow eligibility with `PlantUtility.CanSowOnGrower` and `CanNowPlantAt`, and the native harvest job produces actual `AMJC_RiceSheaf`. New real rice thresh/hull Bills must yield `RawRice`; the preexisting 11 processing/cooking Bills are retained. The PowerShell expected-name registry matches the new feature files.

The upland rice XML now uses conditional replace-or-add for all six PlantProperties fields that may already be present (including CCTO's minimum growth temperature). The static test validates match and nomatch values and guards against duplicate operations. The historical six-crop analytical fixture is not rewritten.

**Still unverified:** C# compilation, actual four-profile Pickle 6/6 and ERROR 0, a calendar-controlled native Sow job, graphical rendering, old-save compatibility, and the standalone MO-dependency release gate.


### Real simple-meal Bill extension (2026-10-08)

The quickstart production scenario includes **five real Vanilla `CookMealSimple` Bills** after native harvest and the eleven preexisting grain Bills, plus two new bulk rice processing Bills. Inputs must come from additional native thresh/hull Bills using the already-harvested millet, buckwheat, barley and wheat sheaves; two additional real Soba harvests provide enough sheaves for the second bulk route. Vanilla `RawRice` is obtained via two native bulk rice thresh/hull Bills from the newly harvested `AMJC_RiceSheaf`. The five edible ingredient Defs are `AMJC_Millet` (shared by Awa/Hie/Kibi), `AMJC_Buckwheat`, `AMJC_Barley`, `AMJC_Wheat`, and `RawRice`. Each meal Bill locks its ingredient filter to one raw grain and verifies its actual consumed stack count, recipe completion and additional `MealSimple` output. No harvested or cooked ingredient is manufactured by the fixture.

A new static regression in `Tests/test_grains_chain.py` protects the E2E harness wiring and the identical four-profile feature tags. The long `@quickstart:AmjStageAQuickstart` scenario uses Pickle `@timeout:270`; the production helper's wall-clock watchdog is 240 seconds, the isolated Pickle run-wide limit is 420 seconds, and the game-process watchdog is 540 seconds. At the time of this historical smoke-harness entry, the suite expected six scenarios per profile. The current V1 matrix requires ten per profile with strict scenario/ERROR-0 gating. **This is source/test harness implementation only, not a claim of successful game execution or C# compilation.** Dedicated production art, true native sow job, four-profile runtime, graphical checks, migration and release gating remain open.


## MO 1.6 source/provider preflight (2026-10-08)

The provided packaged MO archive `3219596926.zip` was checked as source evidence. All 263 XML files under the loaded `1.6/Defs` root parsed. It contains `DankPyon_Plant_Wheat` (12-day grow period, yield 28, `DankPyon_RawWheat`), `DankPyon_Flour` (nutrition 0.05), the researched `DankPyon_Millstone`, both 1× and 10× flour recipes, and `DankPyon_DoBillsMillstone` (`WorkGiver_DoBill` on Cooking, fixed to the MO millstone). This **does not prove loaded Defs, C# compilation or runtime job success**.

The existing `Tests/validate_grains_chain.py --mo-root` source validator now accepts an extracted Medieval Overhaul installation **or** a Workshop ZIP directly. In ZIP mode it reads only version 1.6 XML and checks the native MO WorkGiver, both flour recipes and wheat provider. The static regression creates a synthetic versioned ZIP and confirms that an incompatible MO WorkGiver fails before the game is run. Example: `python Tests/validate_grains_chain.py --mo-root 3219596926.zip`. Game-run four-profile Pickle, strict runtime ERROR 0, actual native sow, art and migration gates remain pending.


### Pickle run-wide timeout audit (2026-10-08)

Pickle's `-pickle-run-timeout` is measured in **minutes** and applies to the entire feature run, whereas `@timeout:270` allows one long production scenario 270 **seconds**. The former shared 4-minute (240-second) global cap could terminate this scenario prematurely. The shared launcher now accepts `-PickleRunTimeoutMinutes` (default `4` for legacy tests); Grains' four-profile runner uses `7` minutes with a separate `540`-second process watchdog. The Windows static tooling verifies `scenario < Pickle run < process` and the 40-minute four-profile watchdog boundary. This is runner verification, not proof of game runtime or C# compilation.


### MO millstone Hay byproduct regression (2026-10-08)

The **unpatched MO 1.6 provider source** declares `Hay` alongside `DankPyon_Flour` in its three milling Recipes (manual, regular and bulk). Grains' MO-conditional `MedievalOverhaul_StageA_Wheat.xml` **removes `products/Hay` from all three Recipes**, so the expected loaded Grains+MO grinding output is flour only, not Hay. The native-Bill helper compares all *loaded* Recipe product counts, rejecting undeclared Hay/Straw; the loaded-MO Pickle contract additionally asserts all three Recipes are flour-only. The upstream-MO archive validator intentionally checks the provider's original Hay before AMJ patches apply, while the Grains static validator enforces the three removal operations. Neither static check substitutes for a real RimWorld test.


### Upland rice translation and loaded Straw test hardening (2026-10-08)

`Tests/test_grains_localization.py` now enforces the six §10 author-approved Japanese/English item/recipe labels and descriptions and the four bilingual jobStrings against actual Production DefInjected/Def XML. Negative mutations of the source, localization and approved document must fail. The four-profile loaded-Def Pickle step checks the **exact** MO rice-thresh products and quantities (`AMJC_RiceInHull` plus `DankPyon_Straw`) and requires the Base thresh and both hull Recipes to have only the specified main output. Static verification is not equivalent to C# compilation, game loading or ERROR-0 success.


### Uploaded four-profile runtime failure remediation (2026-10-08; real rerun pending)

The `automated-gates(2).log` recorded Vanilla and MO Pickle failures, including missing mood description, missing `SimpleMeal` texture, a `Single()` exception and a test-only empty 15×15 search; two CCTO profiles stopped before launch on duplicate installed providers. Fixes preserve the approved 2.5-day meal rot time and standard meal Forbiddable/Ingredients/FoodPoisonable comps while avoiding a second inherited rottable; reference Vanilla `Things/Item/Meal/Simple` as `Graphic_Single`; add English/Japanese mood text; and prepare the isolated map's 15×15 central area deterministically instead of requiring untouched land to be empty. The CCTO duplicate resolver selects one local Mods copy in its **test-only provider manifest** rather than erroring when both local and Workshop CCTO are installed; duplicates of unrelated required providers remain errors. The manifest explicitly records the selected root; installed files and player configuration are untouched. Actual provider identity in game, four-profile 6/6, ERROR 0, loaded rottable comps and textures must still be verified by the real rerun.


### Real-game four-profile Pickle 6/6; temporary meal-graphic correction (2026-10-08)

The author-provided `automated-gates(3).log` confirms the exact six expected Pickle scenarios passed without skips in **all four profiles** (vanilla / vanilla-ccto / mo / mo-ccto); C# build, CCTO local-first selection and real work-Bill behavior no longer report scenario failures. However the **ERROR-0 gate still fails in all four profiles**, solely on three flour-meal graphics: `AMJC_Houtou`, `AMJC_Sobagaki` and `AMJC_MilletDumplings` use `Things/Item/Meal/Simple` with `Graphic_Single`, a path which the installed game reports cannot resolve, plus follow-on `MatFrom with null sourceTex`. Previous `Things/Item/Meal/SimpleMeal` was also missing. Do not infer texture availability from old Vanilla Def XML or the Python source projection.

As a **temporary, unapproved visual placeholder only**, these three food Defs now reuse the Grains-owned, already shipped Millet/Buckwheat three-variant PNG sets and matching `Graphic_StackCount` class. Houtou/dumplings use Millet and sobagaki uses Buckwheat. This intentionally is not a claim that the food art depicts prepared dishes; replace with proper approved individual food artwork during the post-runtime art phase. The chain validator and negative tests confirm actual `Textures/` PNG presence for all three `_a/_b/_c` variants, matching Def paths and class, so regressions to unprovided Vanilla art should be caught in CI. No PNG bytes, numbers, Recipes, thoughts, names, package IDs, load order, MO dependency, or historical copy changed. **Re-run real four-profile 6/6 and runtime ERROR 0** before marking the release gate passed. Visual approval and old-save migration remain separate.


### 四構成の実機スモーク確定結果（2026-10-08 JST）

作者アップロードの `automated-gates(4).log` により、以下の**実ゲーム**実行結果を確認した。GitHub CI上の仮想summaryではなく、各構成でQuickstarts、Pickle C# assembly、実ゲームの起動が行われたテスト結果である。

| Profile | Pickle | 隔離ログERROR | 判定 |
| --- | ---: | ---: | --- |
| `vanilla` | 6/6 | 0 | PASS |
| `vanilla-ccto` | 6/6 | 0 | PASS |
| `mo` | 6/6 | 0 | PASS |
| `mo-ccto` | 6/6 | 0 | PASS |

実行スクリプトは六つの期待シナリオ名と失敗/skip 0を検証し、ERRORレベルのログがないことも確認した。全構成でテスト対象Def/Patch/Textureのバイト同一ステージが成功した。CCTOがローカル・Workshopに重複していても、テスト用manifestにローカル側を選択して正常に実行した。粉食3品をAMJ同梱穀物の仮画像へ切り替えた `6d0c791bca3424a8e97c73fb48f906beded01ed7` の後、以前の `Texture2D` / `MatFrom` のERRORは再発していない。C#コンパイルは成功、`CS1684` は非致命警告のみ。

**この範囲のゲートはDONE：** 4構成のロード済み契約と、E2Eに組み込んだ作物収穫・加工/製粉/料理の実Bill、6シナリオ×4、隔離ランタイムERROR 0。**リリース前の残件：** 季節別のネイティブ播種/経時低温検証、日本語/英語の実UI表示、完成料理/作物/設備の専用画像と通常ズームの目視、プレイ時間・バランス。本番のMO必須指定は2026-10-10に解除した（この旧実機PASSとは別の改訂）。既存節に残る「実機未実施」の文言は当時の作業記録であり、最新の合否は本節を正本とする。


### 旧Coreの穀物セーブ保存契約：保管する履歴診断（2026-10-08作成、10-10対象外化）

**現行判断:** Grainsの導入実績・既存セーブはゼロ（作者確認）。この節の実セーブ作成/再保存と旧MOの削除試験は今回の開発・公開ゲートから除外する。検査器は過去の診断資料として残すが実施は要求しない。

Grains固有の存続確認は新規の `Scripts/grains_save_contract.py` が担当する。独立Scenariosの `Scripts/scenario_save_contract.py` は開始Scenario/Faction/PawnKindの所有者切替を担当し、Grains側で重複実装しない。Grainsの検査器はRimWorld 1.6の UTF-8 XML形式の **production-ID** `.rws` に限定し、旧Core `sucro.ancientmedievaljapan.core` とMOが保存元/更新先の双方にある場合にだけ適用する。名前の改変や保存データの修復は一切実行しない。

旧Core環境で作成した実際の旧セーブを**コピー**して、検証に使用する。元セーブは読み取り専用。作物・加工品・設備などの `AMJC_` Things と、`Plant_Rice` / `RawRice` の保存ID・Def名・数量、AMJC Recipeの保存Bill、ゲームtick、GameVersion、Mod ID集合を更新前と更新後の再保存されたXMLで比較する。必須Grains項目とVanilla米を含まないセーブは、この検証用fixtureの入力として拒否する。Id・数量・Bill等が変化した場合も不合格。時刻停止状態でのエンジンload→re-saveを比較前提とし、作物の自然成長や生産による数量変化を許容しない。

ローカル使用例（入力のSHAは作業時の実際の40文字のcommitを指定。旧版ソースとセーブの来歴は別途記録する）。

```powershell
python Scripts/grains_save_contract.py prepare --save "D:\\AMJ-TestFixtures\\old-core-grains.rws" --output "D:\\AMJ-TestResults\\grains-migration" --legacy-commit <OLD_FULL_SHA> --updated-commit <NEW_FULL_SHA>
python Scripts/grains_save_contract.py compare --before "D:\\AMJ-TestResults\\grains-migration\\baseline.rws" --after "D:\\AMJ-TestResults\\grains-migration\\same-provider-upgrade\\resaved.rws"
```

`prepare` は新しい隔離フォルダ内に元XMLのSHA256、元セーブとバイト同一な `baseline.rws`、プロバイダ/実機検証要件を `plan.json` へ保存する。既存フォルダを上書きしない。`compare` が通っても `runtimeVerified=false` と明示する。**これは実ゲーム起動器ではなく、実機で旧セーブを更新版Grainsへロード・再保存する処理は未実装**。XML要素が一致しても未解決Def、ロード後の参照、ログERRORやUIは証明されない。Scenarios追加/削除やMO解除はこの契約の外側であり、Scenarios専用ゲートまたは別のリリーステストが必要。

`Tests/test_grains_save_contract.py` はsynthetic保存XMLで数量の欠落、`RawRice` の再解釈、加工Bill差分、tick変更、Mod構成欠落、別名テストMod、破損XML、既存結果への上書きを負例として検出する。GitHub CIではこのfixtureテストのみ実行し、実旧セーブをリポジトリへアップロードしない。現在の4×6 Pickle/ERROR-0 PASSは変わらず、**旧セーブ移行の実機ゲートは依然OPEN**。


### 暦移動＋制御温度による実播種・低温成長の追加検査（2026-10-08）

既存の4構成×6シナリオの名称・件数と本番XMLを維持したまま、実Bill/収穫/調理が完了した最後に、`GrainsSimulationSteps.ProductionScope.NativeSeasonalSowAndGrowth` を追加した。Quickstartの隔離された土壌セルを使い、テスト専用の暦と屋外温度を設定して、以下の**ゲーム実装の経路**を検証する：

1. **適温25℃：** 栽培区画で陸稲を選択し、実際の `WorkGiver_GrowerSow.JobOnCell` が `JobDefOf.Sow` を提供することを確認。ジョブを動かして `Plant_Rice` の実播種を完了。
2. **暦を1 quadrum（15日）進めて5℃：** 新たな区画で陸稲の播種ジョブが出ないこと、既に播かれた陸稲の成長率が0になり、実際の `DoSingleTick` 2,200tickで成長が停止することを確認。
3. **同じ5℃：** 区画を大麦に切替え、実際の播種ジョブで `AMJC_Plant_Barley` を播き、2,200tickで成長が進むことを確認。
4. **25℃へ戻す：** 稲の成長が2,200tickで再開することを確認。区画・テスト植物・積雪深・暦tick・温度Overrideを成功/失敗時ともに復元する。

**このテストの限界：** `BiomeDef.constantOutdoorTemperature` をテスト中だけ25/5℃に設定するため、RimWorldの native sow/growth/long-tick 経路は通るが、地形・実際の季節曲線・降雪・天候変化を通じた自然発生の温度変化を検証するものではない。また固定枯死温度の**実際の枯死処理**は今回の条件（5℃）では試験していない。CCTOなし/あり、MOなし/ありの4構成での実機結果は追加変更後**未実行**。旧 `automated-gates(4).log` の6/6・ERROR 0は変更前の版の合格実績であり、新しい播種テストの実行結果として再使用しない。

今回追加したのはテストソースと回帰検証のみ。必要なゲーム本体・Pickle・QuickstartsがないCIではC#ビルドと実ゲーム動作は判断できない。 `Tests/test_grains_chain.py` は Job/生育tick/保存復元の接続が失われないことを静的に確認する。新しい実機確認でも元の `run-grains-tests.bat` を使い、4構成の6/6とERROR 0を必須にする。


### automated-gates(5).log の実機結果と再現修正（2026-10-08 JST）

2026-10-08の作者提供ログにより、新しい暦＋5/25℃実播種/成長検証で `vanilla`, `vanilla-ccto`, `mo-ccto` は各Pickle **6/6・ERROR 0**、`mo` のみ **5/6・ERRORあり**だった。失敗したシナリオ名は `Grains mo real harvest and flour food Bills complete` で、末尾に追加された温度復帰検証の `Native TickLong must resume rice growth after warming.` で失敗した。4構成すべてC#ビルド成功、CS1684は非致命の警告のみ。**この実行を4構成完全合格とは扱わない。**

RimWorld 1.6参照実装の `Plant.Resting` は現地日内割合が `<0.25` または `>0.8` の場合にtrueとなり、`Plant.GrowthPerTick` は `Resting` 時に**温度にかかわらず0**となる。旧テストは最初の昼固定後、実ジョブ時間と15日暦ジャンプ・2,200tickを重ねながら、復温直前には日内時刻を昼へ戻していなかった。MO単独での成長停止が日没/休息によるものだった可能性が高いが、ログには最後の時刻・光量がないため断定しない。

修正では、**最後の復温直前だけローカル時刻を次の正午へ進める**（`GenLocalDate.HourOfDay` と `GenDate.TicksPerHour` を使用）。テスト用の元tickは終了時に復元する。 `map.skyManager.SkyManagerUpdate()` で実スカイライトを刷新し、日内休息外・実際の光量・成長率・生育段階と25℃の生育季節を確認してから、以前と同じ`DoSingleTick` 2,200tickによる成長増加を検査する。失敗時は光量/温度係数/日内割合/成長段階/初期・最終成長量を出力し、原因を判別できるようにした。夜間休息を無視するPlantパッチ、架空の太陽光追加、成長量の強制設定、期待値緩和は行っていない。

対象はE2Eソースと静的回帰のみ。**この修正後の実機4構成＋ERROR 0は未実行**であり、旧(4)・(5)ログから合格を流用しない。本番穀物XMLや栽培数値・CCTO・MO互換挙動は不変。未完の旧セーブ、自然気候の経時検証、実際のCCTO凍害枯死、専用画像の検証は引き続き独立のゲート。


### automated-gates(6).log：MO単独再検証PASS（2026-10-08 JST）

作者提供の修正後 `mo` 単独プロファイル実行ログで、**C#ビルド成功、Pickle 6/6、隔離実行時ERROR 0** を確認した。`automated-gates(5).log` で失敗した `Native TickLong must resume rice growth after warming.` の再発はこの実行で認められなかった。修正対象は `b127e162d91403281a01c7903ac6ed64752657c2` の、最終復温フェーズでローカル正午・天空光・成長率を確認するテストである。データ保持のため隔離SaveDataを作成し、通常 `ModsConfig.xml` は変更なし。CS1684（`Span`/`ReadOnlySpan`）は引き続き非致命のコンパイラ警告。

| 証拠ログ | 構成 | 新しい暦・播種・経時成長を含むPickle | 隔離ERROR | 判定 |
|---|---|---:|---:|---|
| `automated-gates(5).log` | vanilla | 6/6 | 0 | PASS（復温修正前） |
| `automated-gates(5).log` | vanilla-ccto | 6/6 | 0 | PASS（復温修正前） |
| `automated-gates(6).log` | mo | 6/6 | 0 | PASS（復温修正後） |
| `automated-gates(5).log` | mo-ccto | 6/6 | 0 | PASS（復温修正前） |

新しい季節別播種・制御温度下の成長処理について、**各プロバイダ構成の実機合格実績は揃った**。ただしこの表の3構成は修正前、MO単独だけが修正後のコードで実行されているため、**同一コミットの修正版を4構成で一括実行したことにはならない**。最終の4構成再実行を残す。日本語/英語の実UI・自然季節気象・CCTO閾値を跨ぐ生死・画像/プレイバランスの各リリースゲートは別途OPEN（旧セーブ移行は対象外）。今回の結果は既存の制御温度テストの実機確認であり、自然季節曲線・霜の影響の実証ではない。


### automated-gates(7).log — 4構成最終回帰のNREとテスト専用照明同期修正（2026-10-08）

`automated-gates(7).log` は同一修正版4構成一括テストで、**`vanilla`、`vanilla-ccto`、`mo` はいずれも6/6不達・ERRORあり**、`mo-ccto` のみ6/6・ERROR 0。3構成の失敗シナリオはすべて `real harvest and flour food Bills complete`、表示例外は `Object reference not set to an instance of an object` で、スタックトレースなし。コンパイル全件成功、CS1684は非致命。**この回帰はFAILED**。以前の(6)のMO 6/6/ERROR0をこの同一コミット4構成のPASSへ流用しない。

`b127e16` で追加した `map.skyManager.SkyManagerUpdate()` のテスト途中・強制的な呼び出しが今回の共通原因候補。RimWorld 1.6の実装では当該メソッドは天候と画面・シェーダー・陰影まで処理し、CCTO/MO非依存の単純な日照値更新APIではない。ログから発生行は確定できないため、この因果関係は**仮説**として扱う。テストの修正では直接の `SkyManagerUpdate()` 呼び出しを廃止し、ローカル正午時点のゲーム本来の `GenCelestial.CurCelestialSunGlow(map)` 値を検証して `map.skyManager.ForceSetCurSkyGlow(solarGlow)` で**隔離Quickstartのキャッシュ照度だけ**を合わせる。元の照度も保存・復元する。稲の`TickLong`成長判定、休息時間外と温度/光量係数の条件、および2,200tickの実ゲーム進行は維持。例外には工程名と元の例外を内包させ、スタックが得られなくても原因の絞り込みが可能なようにする。

**テスト本体の新しい実機実行は未完了**。3構成のNREが完全に解消されたことや、自然季節・降雪・CCTO固定枯死を検証したとは主張しない。ゲーム本番XML/Def、依存関係、レシピ、温度値、画像を変更していない。続く実機ではまず`vanilla`単独を診断し、6/6・ERROR 0なら残る構成を確認する（未失敗条件の機械的な繰り返しは避ける）。


### automated-gates(8).log — 照明同期修正後のVanilla合格（2026-10-08 JST）

作者提供ログ `automated-gates(8).log`（原本SHA256: `967ec9afcac0915103f3213d33ac1af91b53312fa884021dd85dbeb1886dad06`）では、`vanilla` 単独のC#ビルド、Pickle **6/6**、隔離実行時 **ERROR 0** を確認した。通常のModsConfigは変更されず、CS1684は非致命警告だった。(7)のNullReferenceExceptionはこの実行では再発していない。ログ本文にコミット番号やsource-stateの内容は含まれないため、厳密なコミット/全ソースhashの対応付けまでは主張しない。

残る `vanilla-ccto` / `mo` / `mo-ccto` の照明修正後版は未確認。以前の(5)〜(7)の別版合格を転用しない。今回の合格は制御温度と暦移動を伴うネイティブ播種/成長であり、自然気象、CCTO枯死、旧セーブ、画像、公開可否の合格を意味しない。

### 4構成のソース同一性を保つ実行手順

`Scripts/GrainsSourceState.ps1` はAbout・本番Defs/Patches・BaseWithoutMO・MO互換・旧シナリオ・全翻訳・全Textures・E2Eソース/全4feature・Scripts、およびloader/build/通常ランナーのバイトと相対パスをSHA256へまとめる。絶対パス、時刻、git HEAD、設計書、TestResults、Pythonキャッシュは計算に含めない。同じバイトを別ディレクトリへ移しただけなら同じ値になる。ファイル追加/削除/改名も対象で、稲パッチや翻訳/画像を旧固定リストから漏らさない。

通常の `run-grains-tests.bat` は引き続き非表示デスクトップ・実描画経路で実行する。ランナーは開始時の全体hashをコンソール、`TestResults/Grains/matrix-source-state.txt`、各 `Pickle/source-state.txt` と `matrix.json` の `SourceHash` に記録する。各構成のbuild前と結果確定前に同じhashを要求し、途中の編集やsource-stateの不一致は終了コード2にする。構成別の6シナリオと実行時ERROR 0も従来どおり必要で、ソース同一性だけでは合格にしない。

最終4構成ゲートは、変更を終えた一つのソースで引数なし（または `all`）を実行して確認する。単独構成を診断した場合はその構成だけの証拠として扱う。旧ログ(8)のVanillaはゲーム処理修正後の診断合格として保持し、この新しいランナーの実機合格へ読み替えない。

`Tests/test_grains_profiles.ps1` はWindows PowerShell 5.1のsyntheticテストで移設/時刻/報告書に対する同値性、稲/翻訳/PNG/C#/feature/runner/Aboutの変更、追加・削除・改名、4構成共通の記録hashを検証する。このテストは実ゲーム合格を代替しない。


**CI verified (2026-10-08 JST):** commit `a055c5b0e4f76ce7d69b57c89ed08850d2e52e30` passed [Stage A run 37761723079](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/actions/runs/37761723079), including Windows PowerShell 5.1 parsing and the expanded synthetic four-profile/source-drift tests, plus [Workshop payload run 37761722979](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/actions/runs/37761722979). Source-identity tooling validation is DONE. This is not an installed-game execution of the new runner; corrected real-provider matrix remains OPEN.


### GRAINS-SEASONAL-NRE-DIAGNOSTICS-20261008

Uploaded automated-gates(9).log reports source SHA256 fd08db39254d0272c369a781d03f43911b23f6829ec1eea3b6fe501ca20a7563. Vanilla passed 6/6 with ERROR 0; vanilla-ccto, mo and mo-ccto failed the seasonal warm-recovery setup with NullReferenceException. The excerpt does not provide individual pass counts for failed profiles or final matrix/source-drift completion. The earlier sky-update hypothesis has not resolved the failure and is not a proven cause.

Diagnostic-only E2E change: report Exception.ToString() in the outer message so Pickle's message-only output includes the underlying stack, and label individual calendar/celestial/cache/temperature/growth operations. Explicitly check rice survival and sky-manager availability without skipping or relaxing any native sow/growth assertion. Production XML, crop values, recipes, textures and dependencies unchanged. Root cause and actual-game fix remain OPEN; next installed-game execution is a focused failed-profile diagnosis, not another speculative full matrix.

## RimWorld 1.7 unstable — test-only real-provider matrix

Invoke `powershell -NoProfile -ExecutionPolicy Bypass -File Scripts/Run-17Tests.ps1 -GameRoot "D:\SteamLibrary\steamapps\common\RimWorld" -Profile all` for a local **1.7 unstable** game. The existing `run-grains-tests.bat` / `Run-GrainsProfiles.ps1` are still the 1.6 gates. This new entry does **not** change production C#/XML/Def values or `About/About.xml` and does not declare 1.7 release support.

The new 1.7 entry identifies actual `Version.txt` / `RimWorld.exe`, selects installed 1.7 Pickle/Quickstarts providers from their `LoadFolders.xml`, stages only the established disposable profile Mods, and compiles the existing Quickstart/Pickle test sources using Roslyn and the game's root assemblies. Only the **staged test target**, never source `loadFolders.xml`, gains a `v1.7` node copied from the approved `v1.6` conditional routing; test-only About files gain 1.7. Normal player ModsConfig/Prefs and repository production files are hash-preserved. Every selected real-provider profile runs on its own hidden rendered 1.7 process with native PID-checked Player.log, no ERROR, and the existing strict six-scenario `Validate-GrainsPickleSummary.ps1` gate. The test path does not use fake MO/CCTO API fixtures or silently replace actual provider Defs.

Four independent profiles (default): `vanilla`, `vanilla-ccto`, `mo`, `mo-ccto`; each must pass its six named assertions: real provider identity, primary grain loop, optional cold tolerance contract, wheat/flour/minimum meal, seven-grain environmental distinction, and native harvest/processing Bills. This is **24 profile-scenarios**, not 24 new checks. Test results stay under `TestResults/Grains17-*/` and are not Workshop content. CI validates only PowerShell parsing, native-log rejection, game version and provider selection; the real loaded-game 1.7 matrix is not yet executed by this GitHub-only change.

## Independently inspected Windows 1.7 run — 2026-10-13

Production base `d456ebb86ae580eb3fa74982a1a1fc5edc345c77` plus this test-only fixture/dispatcher/XML-reader correction was executed in four real-provider profiles on `1.7.4976 rev1043`. Newly merged flour/mixed-meal source is retained. [Exact named outcomes, source/provider DLL and complete PID-log hashes](Testing/Native17-20261013.json). Local matrix: `TestResults/Grains17-20261013-194427-800/matrix.json`.

| Profile | Passed / total | Failed | Native ERROR entries | Gate |
|---|---:|---:|---:|---|
| Vanilla | 6/6 | 0 | 0 | PASS |
| Vanilla+CCTO | 4/6 | 2 | 4 | FAIL |
| MO | 6/6 | 0 | 0 | PASS |
| MO+CCTO | 5/6 | 1 | 2 | FAIL |

Six scenario premises per profile:
1. `loads the requested real providers`: exact source and actual optional providers are active, never API fixture substitutes. PASS in all four.
2. `primary grain loop resolves`: actual harvested products, processing recipes and optional MO straw/output routing match the owner chain. PASS in all four.
3. `optional cold tolerance resolves`: each custom crop has exactly one CCTO extension with owner values when CCTO is active and none otherwise. FAIL in both CCTO profiles; PASS without CCTO.
4. `wheat flour and minimum food resolve`: Base wheat/flour or native MO wheat/flour and the strict owner foods match; Base CCTO fallback wheat must carry its expected extension. FAIL only in Vanilla+CCTO.
5. `seven grains retain environmental niches`: loaded temperature/fertility/environment contracts remain distinct. PASS in all four; not a proof of gameplay enjoyment or an exclusive winning niche for every grain.
6. `real harvest and flour food Bills complete`: actual pawn harvest and thresh/hull/mill/cook Bills, including latest flour/mixed recipes, followed by controlled 25℃ rice sow, +quadrum/5℃ rice denial and barley sow/growth, and 25℃ rice recovery. PASS in all four. This does not independently run every V1 stove fallback/recipeUsers variant.

The old seasonal fixture put rice and barley in one grow-zone and changed that whole zone to barley: native growers removed the earlier rice. Separate rice/barley zones now preserve the actual sow/growth assertions, with cleanup and crop/temperature diagnostics. Native 1.7 test compilation resolves the installed Pickle 1.6 dispatcher signature by reflection only under `AMJ_TEST_WAYLITH`; the 1.6 direct-call path and actual main-thread dispatch stay intact. XML reads honor declarations/BOM; synthetic provider metadata reads explicitly use UTF-8. No scenario/ERROR gate was relaxed.

**Confirmed production blocker, not repaired in this test-only patch:** all five operations in `Patches/Compatibility/CCTO_StageA.xml` and one in `BaseWithoutMO/Patches/CCTO_Wheat.xml` match the English display name `Crop Cold Tolerance Overhaul`. The actual active provider is named `Crop Cold Tolerance Overhaul（作物耐寒性オーバーホール）` with the expected packageId. RimWorld 1.7's actual `PatchOperationFindMod.ApplyWorker` calls `ModLister.HasActiveModWithName`; these operations therefore do not attach the extensions. Native failures specifically report missing custom-crop and fallback-wheat extensions. A separate author-approved production change must accept the actual provider name while preserving old English-name compatibility and every crop value. Until then the full 24-scenario matrix is **FAILED (21/24)**; do not mark CCTO coexistence or release support PASS.

Offline validation: native harness contracts passed in Windows PowerShell 5.1/PowerShell 7; source chain 44 tests and latest mixed-meal six tests passed (the hash-locked audit uses original Git bytes; Windows autocrlf must not reinterpret them); synthetic four-profile validation passed. Prior 1.6 compile proof is separate from a 1.6 game rerun. Production XML/Defs/art/public metadata and normal ModsConfig/Prefs remain unchanged.

Still unverified: natural weather/climate, actual threshold-crossing CCTO frost death, actual old colony load/re-save, every stove/equipment setting variant, accepted visual/UI states, balance, downloaded Workshop payload and publication.


### Author-approved CCTO name correction — current result (2026-10-13)

The author approved the bounded production-XML proposal after reviewing the preceding failed matrix. Both `Patches/Compatibility/CCTO_StageA.xml` (five operations) and Base `CCTO_Wheat.xml` (one) now accept **both** the legacy English title and `Crop Cold Tolerance Overhaul（作物耐寒性オーバーホール）`. This only restores optional extension attachment; all crop/threshold/recipe/ingredient/output values, C# production language, art, public metadata and the 1.6 paths remain unchanged. Prior failures above are historical diagnosis, superseded for this explicitly corrected source.

Latest production base `2f5b8132eb1a32bc3cfa01625f895dc045f70fc8` plus these six title selectors passed **Vanilla 6/6, Vanilla+CCTO 6/6, MO 6/6, MO+CCTO 6/6 (24/24)** on actual `1.7.4976 rev1043`. Every profile has zero failed/skipped/flaky scenarios, complete native PID/log closure and **ERROR 0**. Real provider identity, primary loop, CCTO extension/value presence/absence, wheat/flour/food chain, seven-grain environmental contracts and native harvest/processing/latest meals plus controlled seasonal recovery all passed. Local matrix: `TestResults/Grains17-20261013-201330-725/matrix.json`. Existing [evidence JSON](Testing/Native17-20261013.json) retains old failure hashes and adds `approved_ccto_name_correction` with every exact name, result, source hash and corrected XML/report/log hash.

CI regression now rejects losing either provider title in any of the six operations; the existing Stage A gate recognizes the accepted two-title list. The source-chain suite (45 tests) preserves numeric/production-chain regressions. Windows `core.autocrlf` can change hash-locked legacy XML/audit bytes: offline byte-hash checks use the exact unchanged Git blobs, without rewriting expected hashes or changing production content. Native source hash recorded before that offline-only restoration is retained in the evidence.

This establishes fresh-profile 1.7 runtime of the corrected optional compatibility; it does not establish native frost death, natural climate, actual legacy save migrations, every stove/equipment variant, accepted UI/visuals, balance, installed Workshop bytes or publication. Legacy-title compatibility is structurally guarded; an old English-title CCTO binary and current 1.6 runtime were not separately launched in this correction.


## Independent sibling crop growth balance (2026-10-14 split)

Grains **does not** own Vanilla potato/corn/strawberry/cotton/hops/smokeleaf or MO lentil/onion/garlic/flax/sugarcane/carrot/tomato `growDays` patches. They moved to independent optional **AMJ Crop Growth Balance**. Do **not** assert their replacement values in the Grains-only four-profile acceptance tests. Native Grains acceptance retains its own seven grains, Vanilla `Plant_Rice=5`, and optional MO wheat `growDays=12` with the existing processing and CCTO constraints.

For combined-play compatibility, include separate profiles with Crop Growth Balance installed and require Grains seven-grain unchanged values, no duplicated crop growth overrides, correct MO conditional availability, and native ERROR0. Crop Growth Balance's own 6+7 loaded-Def assertions, field work, season-yield economic comparison and 1.6/1.7 verification belong to its own `Docs/Design.md` and tests rather than this Grains release gate. Previous post-PR#99 native validation was not performed; no game PASS is inherited by moving the patch.


## Linux 1.6 MO audio-isolated functional test (2026-10-14)

This is separate from the strict release/runtime gate for the **unmodified**
Medieval Overhaul and SYR audio files. Project [native run #38566373498](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38566373498)
ran source-pinned Grains `9f8bc72eb16faacfd1219f18e91b489294af224c`
with the actual staged VEF/SYR/MO providers inside a disposable game
copy. Only two verified problematic third-party sound MP3s were
transcoded inside that game copy to 44.1-kHz stereo PCM WAV; no owner
production XML, original third-party source or strict ERROR threshold
was changed. Unity **ERROR 0, FMOD 0**; Pickle `mo` **9/10 FAIL**,
natural game exit code 1. Exactly one of the ten required scenarios,
`Grains mo common food single and mixed Bills complete`, failed in
the native test fixture with `Harvest failure must be excluded by worker
skill.`; all nine other scenarios passed, including real harvest, flour,
processing, recipe rejection and ingestion checks. `mo-ccto` was not
executed in this same run after the failure.

Owner test-only [PR #103](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/pull/103),
main `6a3c55d58754e9127f7b92bba116fd384dadf314`,
now sets a candidate's Plants/Crafting/Cooking skills to 20 and verifies
**actual** `PlantHarvestYield >= 1.0` before selecting it, rather than
assuming all random pawns at level 20 meet the original requirement.
The strict post-spawn >=1.0 assertion, all original scenario names and
production data remain unchanged. Stage A/Workshop CI passed.
Project source repin [PR #225](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/225)
targets this exact owner source; a real rerun is required before the
failure can be considered resolved. Converted-media test success cannot
satisfy the original-media ERROR0 release gate or prove audible sound.

## Verified current Grains main native V1 results (2026-10-15)

**Actual-source revision:** `f339d5fe98a56aba5cae9652c2546c0162b79cc3` (Grains main, including all 33 newly committed DDS production derivatives). The Project native runner source pin was updated by [Project PR #261](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/261), merge `1e080f5a0bd16c2bf3a169d968bdb39a83fda3a0`, before the following **two separately completed real Linux RimWorld 1.6.4871 rev600** runs. Unlike earlier 6a3c55d / 9f8bc72e results, these runs use the same current Grains source.

| Profile | Native evidence | Real Pickle outcome | Audio/provider condition |
| --- | --- | --- | --- |
| Vanilla | [Project Actions #38682563490](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38682563490) | **10/10 PASS**, failed/skipped/flaky 0, Unity ERROR 0, game exit 0 | Original Vanilla providers; no audio substitution |
| Vanilla+CCTO | Same [#38682563490](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38682563490) | **10/10 PASS**, failed/skipped/flaky 0, Unity ERROR 0, game exit 0 | Original CCTO and Vanilla providers; no audio substitution |
| MO | [Project Actions #38682422683](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38682422683) | **10/10 PASS**, failed/skipped/flaky 0, Unity ERROR 0, game exit 0 | Actual MO/VEF/SYR providers in **disposable isolated WAV-converted audio clone** |
| MO+CCTO | Same [#38682422683](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38682422683) | **10/10 PASS**, failed/skipped/flaky 0, Unity ERROR 0, game exit 0 | Actual MO/VEF/SYR/CCTO providers in the **same type of disposable WAV-converted audio clone** |

**Acceptance premises (unchanged):** For each profile the ten *original owner* scenarios assert real requested provider isolation; seven crop/processing/food Def contracts; CCTO conditions; Base-vs-MO wheat/flour providers; seven-grain controlled environment niches; actual pawn harvest→thresh/hull→mill→cook; three common foods × single/four servings × single/two eligible raw/flour inputs (12 native Bills); alternate processing-site/fueled-stove native Bills and recipe exposure on ElectricStove (not a powered electric execution claim); eight wrong-only cooking inputs rejected; and pawn Ingest/Thought effects including no generic Fine Meal bonus and the tested flour-specific +2 nonstacking Thought. All named scenarios ran once without skip or flaky outcome. These premises do **not** certify actual whole-season crop balance, powered ElectricStove Bills, all five specialty foods ingested by real pawns, screenshot appearance or JP/EN loaded Info-panel layout.

**Critical separation of evidence:** The complete **40/40 is a current-source native *functional* matrix**, **not** 40/40 strict acceptance of unmodified MO audio. The two MO profiles converted only the copied external `UI/DebugGoose/Honk.mp3` and `MO_MedievalOverhaul/Sounds/DankPyon_FlaskBreak.mp3` to 44.1 kHz stereo PCM WAV inside a temporary game clone, keeping the real original providers, scripts, scenarios, assertions, zero-ERROR threshold and source XML unchanged. Original external MP3s were not edited in their provider locations. The MO run explicitly records `original_mp3_strict_acceptance=NOT_PASSED_BY_THIS_LANE`; prior original-media MO FMOD errors therefore **remain an independent strict release blocker** until an unchanged-original-media run proves otherwise. Zero audio errors in the clone do not prove audible playback.

**Still OPEN:** loaded Japanese and English Info-tab text/paragraph/clipping and ordinary zoom/sprite judgment, final food/flour/equipment art acceptance, private long play and Steam subscriber/publisher/re-download verification; current-owner 1.7 native tests; strict MO original-media ERROR0. No Grains production Def, recipe, balance, artwork, game Mod installation or Steam publication was modified by these test runs.

## MO byproduct TargetCount acceptance — 2026-10-15

For all 10 threshing Recipes (five crops, single/x10): Base and Base+CCTO keep one grain product and the standard counter. MO and MO+CCTO keep grain as products[0] and Straw as products[1], with `AncientMedievalJapan.Grains.RecipeWorkerCounter_PrimaryGrain` loaded from a real compiled `Assemblies/AMJGrains.dll` and `CanCountProducts == true`. Count only primary-grain stocks while observing Vanilla stockpile/zone/quality restrictions and the existing 1/10 target-count increments. In actual native `DoBill`, verify starting below X, stopping at or above X, not stopping on Straw stocks, and unchanged Straw yields. The existing four-profile native matrix and ERROR-0 gates remain mandatory. Current native evidence for the TargetCount fix (2026-10-15, Grains source `87ca0255df74b0be44eeb5c039a66450d9532a40`): the production C# DLL exists under `Assemblies/AMJGrains.dll` (blob `1dc251ec59272aae5ad0ccff9d6f818427fb6dfa`); source-pinned Linux 1.6 production builds succeed. Vanilla and Vanilla+CCTO real Pickle scenarios each **10/10 PASS, Unity ERROR0** in [Project run #38749268990](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38749268990). MO original-source **10/10 native functional PASS**, including the actual pawn's grain-producing TargetCount Bill stopping at the counted **stockpiled** primary-grain threshold and permitting next-stage hull, in [Project run #38748683576](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38748683576); inherited original-provider FMOD **2 Unity ERROR** means strict MO **FAIL**. The early native test incorrectly assumed loose floor products were included by RimWorld's stockpile resource counter and attempted an unattached Bill in loaded-Def checks; both test premises were corrected without changing recipes or output quantities. MO+CCTO **not separately run on this source**, as the original-media MO strict failure short-circuits the current two-profile poller. Previously validated WAV-converted external-audio clone results on other Grains source revisions are not this repair's original-media strict evidence. **Still OPEN**: exact final subscriber build/payload, current-source MO+CCTO native TargetCount, strict original-provider MO and MO+CCTO ERROR0, native save/load/UI tests, and Steam publication. Do not mark the draft PR/release ready based on source-only checks or MO functional PASS.


### 2026-10-15 TargetCount integration: exact four-profile source verification

**Accepted scope for this one Grains PR only:** the author explicitly authorized a merge-decision exception for the already reproduced, unchanged original MO/SYR FMOD audio-error pair. This is not a global Unity ERROR0 waiver, does not reclassify the original strict MO pipeline as PASS, and does not extend to other errors, Mods or subsequent content changes. No original MO/SYR sound assets, suppression logic or test assertions were altered.

| Real provider profile | Exact source and evidence | Gameplay scenarios | Unity runtime errors | Merge-decision status |
| --- | --- | --- | --- | --- |
| Vanilla | [Project run #38749268990](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38749268990), core source `87ca0255df74b0be44eeb5c039a66450d9532a40` | 10/10 | 0 | PASS |
| Vanilla+CCTO | Same #38749268990 | 10/10 | 0 | PASS |
| MO | [Project run #38748683576](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38748683576), same gameplay source | 10/10 | 2 known original-provider FMOD | Function PASS, strict ERROR0 FAIL; explicit scoped exception |
| MO+CCTO | [Project run #38756557050](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38756557050), current feature + diagnostics at `dfb89ae15cea3d6b01532b3a9228b30706be289c` | 10/10 | same 2 known original-provider FMOD | Function PASS, strict ERROR0 FAIL; explicit scoped exception |

The four native 10-scenario matrices run real game/Pickle, preserve recipe products including MO Straw and verify map-bound actual production TargetCount at a stockpile and following hull; only the diagnostic C# messages changed between gameplay source `87ca0255` and `dfb89ae`. The native MO+CCTO archive `amj-grains-pr16-38756557050` records `exit_reason=passed`, `game_exit_code=0`, ten unique passed scenarios, zero failed/skipped/flaky and exactly the two known FMOD errors. The diagnostic compile fix after `613d909` does not change gameplay or assertions. The strictly evaluated overall MO+MO+CCTO outcome remains **FAIL** for original-media audio, independently from the author-approved one-PR merge decision. This verification does not establish visual/UI author acceptance, Steam upload or long-term private-play sign-off.
