# Grains MO依存監査（2026-10-07初回、2026-10-10必須解除反映）

**2026-10-26依存方針更新（現行）:** 以前の2026-10-10のMO必須解除は今回の作者判断で覆り、Agricultureは `DankPyon.Medieval.Overhaul` **必須**とする。理由はMOの小麦・粉・石臼・研究、作物・加工パッチを本来の中世生産基盤として利用し、MOなしの同等経路の維持に開発資源を使わないため。実装 `About/About.xml`／`About/Manifest.xml` に依存を宣言する。下記のMOなし経路・4プロファイル・旧テスト記録は**当時の実装／検証履歴**として保持するが、今後のVanilla単独リリース保証の承認条件ではない。削除／移植はDef・セーブ・外部参照を再監査するまで保留。最新の設計は `Docs/Design.md` 冒頭に従う。

対象はGrains main `644c2c9c60e7fa695d4b50f4aee16eae58b9835c` のProduction XMLと画像参照。所有境界の正本は [`Design.md`](Design.md)、実機検証の正本は [`GrainsProfileTesting.md`](GrainsProfileTesting.md)。Scenariosの静的検証・CIに問題が検出されていないため、作者の指示により追加のセーブ実行アダプター作業を保留し、Grains監査へ戻った。

## 監査時点の結論（以下の実装追記で更新）

**初回監査時点の結論（現在は一部解消済み）:** BaseからMOの識別子・クラス参照は分離した。旧MO画像4パスの問題は2026-10-08以降AMJ/Vanilla仮画像参照へ分離済み。**2026-10-10、作者指示により本番About.xmlからMO必須依存を削除した。** packageId `sucro.ancientmedievaljapan.core` と既存AMJC DefNameは維持する。画像完成・本番実機完了を意味しない。旧セーブ移行は対象外。

| 対象 | 確認結果 | 残る条件 |
|---|---|---|
| Base XMLのMO識別子・クラス | `DankPyon_` / `MedievalOverhaul.` の要素・属性参照なし | Vanilla継承・全Def解決は実ゲーム未検証 |
| MO互換 | 条件付きルートへ分離。旧38明示Def契約を保持 | 実ゲームのPatch・Bill・描画・ERRORゲート |
| 小麦・粉・石臼の供給 | MO不在のフォールバックとMO時の既存供給元を静的検証 | 4構成の実際のロード・工程実行 |
| Straw・研究・材料 | BaseのMO参照を除去、MO互換のみで再接続 | 新規開始での実際の継承値 |
| 画像 | 下表のMO提供4パスを共有Defから無条件参照 | Grains所有画像への置換と全表示状態の検証 |
| About（2026-10-10） | **MO必須依存なし**。MOとCCTOの`loadAfter`は任意の順序ヒントとして維持 | 新規開始の実機・配布・完成画像を検証 |
| シナリオ | Scenariosが完全所有。Grains旧コピーなし | Scenariosが所有する新規開始の実機検証 |

## 監査時点のMO画像の残存依存

確認資料は作者提供 `02-3219596926.zip`。そのAboutはMO **1.6.2.2**、ZIP SHA-256は `6ed379d7c400db43b3af2a9a7fd6203f7f7e99ca1a670d36b0f73fd1adccc9e3`。以下のパスの画像がMOアーカイブ内に存在し、GrainsのTexturesには同じパスの供給がない。Vanilla全画像の在庫は今回与えられておらず、全プロバイダーの網羅証明ではない。

| Grains Def / 要素 | 参照パス（`Textures/`以下） | MO内で確認した状態 |
|---|---|---|
| `AMJC_Plant_Barley` / `graphicData/texPath` | `Things/Plants/FullGrown/WheatPlant` | `PlantWheat_Mature.png` |
| `AMJC_Plant_Barley` / `plant/immatureGraphicPath` | `Things/Plants/Immature/WheatPlant` | `PlantWheat_Immature.png` |
| `AMJC_GrainProcessingSpot` / `graphicData/texPath` | `Things/Building/Production/StonecuttingSpot` | north/east/south/west画像と各マスク |
| `AMJC_GrainProcessingTable` / `graphicData/texPath` | `Things/Building/Production/Millstone` | east/south画像と各マスク |

参照元は `Defs/ThingDefs_Plants/Plants_StageA.xml` と `Defs/ThingDefs_Buildings/Buildings_GrainProcessing.xml`。いずれも共有ルートにあり、MO不在時にも読み込まれる。`DankPyon_` を含まない汎用パスなので、Def識別子の検査では検出できない。従来の「MO仮画像3件」は**3Def**として正しいが、植物の成熟・未成熟を分けると**4パス**になる。

MO画像の存在だけから再配布権限を推定しない。MO画像をコピーして依存解消とはしない。今後はGrains所有パスへ差し替え、植物の成熟・未成熟、設備の方向・マスクを実際のGraphic設定に合わせて用意する。新規画像生成は今回実施していない。

Base小麦と粉類が以前参照していた既存作物・雑穀画像、および手動石臼・粉食の仮画像は当時の開発用素材である。2026-10-11の画像参照更新で、現行の小麦粉（Base/MO）・雑穀粉・蕎麦粉は新たに作者が追加した専用画像を使用する。PNG構造検査の合格は、texPathの解決や方向・表示状態、画風の完成を証明しない。

## 検証の読み方と次の単位

`Tests/validate_grains_base.py` はBase XMLの識別子・クラス参照、条件付き互換、旧38契約を検証する。出力を「MO identifier/class references 0」へ限定し、画像と実ゲームは対象外と明記した。`test_grains_chain.py`、`test_grains_environment.py`、シナリオの新規開始テストは Scenarios 側の責務であり、Grains は開始Defの非所有を静的回帰で検証する。

本監査で選定した単位は上記3Defの画像依存解消（下記の仮参照分離を実装済み）。専用画像制作・レビュー後、4構成（Vanilla / MO / CCTO / MO+CCTO）をGrainsの6穀物ケースでコンパイル・実行し、描画・Bill実行・ERRORを確認する。旧セーブ移行・MO取り外しは導入実績がないため不要。Scenariosの新規開始実機テストはScenarios所有とする。

このクラウド環境ではRimWorld本体・Managed assembly・実セーブがないため、C#コンパイル、ゲーム実行、保存移行は検証していない。当初は静的検証だけでProductionの依存宣言を変更していなかった。2026-10-10、作者判断でMO必須指定を解除し、旧セーブ移行は導入実績なしのため対象外とした。現行版の実機検証は引き続き必要。

## 現行方針：MO併用時もAMJGの所有設定を優先（2026-10-09）

**MO有無にかかわらず、AMJG所有Defの画像・研究・加工台建材はGrainsの確定設定を維持する。** 大麦の成熟・未成熟画像は既存AMJアワの仮画像、脱穀場所・加工台はAMJG側が選択したVanilla石切台の仮画像。大麦は研究不要、`AMJC_GrainProcessingTable` は研究不要かつ Steel 30。これらのMO側旧画像・`DankPyon_BasicAgriculture`・`DankPyon_IronIngot` への復元Patchは廃止した。完成画像や実機表示のPASSを意味しない。

MO提供の小麦Plant／RawWheat／Flour／MillstoneはMO併用時の実Def供給元として引き続き利用するが、Grainsが責務を持つ生育バランス・脱穀工程・副産物はGrainsの条件付きPatchを優先する。MO石臼の研究制限除去、MOの素材カテゴリ追加、脱穀時のStraw接続など、重複機能を増やさないための互換は維持する。MO提供の小麦Plantが元から持つ農業研究条件はそのままとし、AMJG所有の大麦には伝播させない。

旧38件の固定履歴fixtureは書き換えない。静的検証では作者承認済みの **4画像パス＋3旧MO研究・建材上書き** のみ検証用コピーへ過去値を復元し、その他すべての履歴契約を照合する。現在のBase/MO両projectionは7項目でAMJG優先を直接検証し、旧MO上書きを再導入する負例テストを実装する。履歴上の旧MO優先表と旧PatchはGit履歴に保管し、現行仕様として参照しない。

**2026-10-10：MO必須指定解除。** Production `About/About.xml` の `modDependencies` からMOを削除し、MO/CCTOの`loadAfter`は任意の順序制約として保持する。`loadFolders.xml` はMO存在時だけ `Compatibility/MedievalOverhaul`、不在時だけ `BaseWithoutMO` を読み込む。`About/Manifest.xml` の `<dependencies />` は変更不要。旧セーブや途中MO撤去の移行試験は不要。実RimWorldでの新規ロード・Patch競合・描画・完成画像は別途確認する。

## 2026-10-28 Designから移設した失効方針・旧構成履歴

この節は旧依存解除・旧Vanilla供給と検証マトリクス・過去のBase/MO比較表の保存記録であり、**現行の設計・公開条件・テスト必須条件・実装指示ではない**。現行方針は [Design.md](Design.md) 冒頭と現行MO必須構成の各節、および本ファイル冒頭の2026-10-26方針更新を優先する。次の記録は既存Defや過去の検証経緯を失わないため原文のまま移設した。

**2026-10-26 作者方針 — 自分のMO併用プレイを開発の第一目的にする（依存必須化以前の判断経緯。依存部分は上記後続決定を優先）:** Agricultureの当面の開発・バランス判断は、作者が実際に遊ぶMO＋関連AMJ構成において、日本の中世農耕として自然に機能し、栽培・収穫・加工・料理の選択と作業負担が実用的であることを優先する。Workshop・2game等への公開は、作者自身のプレイで使える成果のお裾分けであり、公開範囲拡大・一般利用者向けの網羅的互換・告知や体裁の整備を製品機能より先行させない。VPE／More Plantsとの専用互換は後回しとし、未使用の組み合わせを成立させるためだけの追加Patch・代替工程・テストは実装しない。ただし既存Mod比較の事前監査と、実際に利用する構成で発生する不具合への対応は別とする。これは**作業優先順位のみの決定**であり、2026-10-25のMO任意・Vanilla既存機能の最小動作保証、現行DefName・セーブ保護、数値、作物除外、Grains分離／Agriculture統合の所有範囲、公開要件を自動変更しない。責務・必須依存の再変更は実際のプレイ上の問題、最小変更案、移行コストを比較してから別途決める。Project正本 `Docs/Architecture.md`／`Docs/ImplementationPriorities.md`を参照。

---

**2026-10-25 当時の方針（依存部分は2026-10-26に失効）— 数値バランスの主基準はMO＋関連AMJ、当時MOは任意:** 本Modの作物・加工・食品経済に関する **主たる数値調整・比較・受入環境をMO併用のAMJ統合環境**とする。MO原本の `growDays`／`harvestYield`／工数・保存性・栄養値は比較用の実データであり、値をそのまま採用する義務はない。史実妥当性とAMJ全体での食料入手量／栽培回数／技能／肥沃度／脱穀・製粉・調理の総労働／保存・心情の帰結を一体で優先して決める。**Vanilla単独は既存の播種→収穫→加工→調理→喫食が成立し、参照欠落・進行不能にならない基本互換性だけを保証**する。Vanilla用の現行製粉・加工設備やRecipeは維持し、MO設備・副産物・研究・工程との差を埋めるためだけの個別の収量・工数・効率バランスや代替設備は開発しない。同じAMJ独自作物の主要な共通値は可能な範囲で共通利用するが、MO原本の作業台・小麦加工などとの生産性がVanilla経路で一致する必要はない。**MO必須化、Vanilla既存機能削除、実際に使えないBill・不足素材・プレイ不能を許す方針ではない。** CCTO V3未承認の35 HP trial、Environment側の土壌・天候、水田側の水管理効果の数値をこの方針だけで確定しない。検証は主環境の経済・栽培収支を中心に、Vanilla単独は変更に応じて最小限の動作・進行回帰を確認する。以前のVanilla／MO完全同等性を求める分析・テスト案は最新方針に劣後するが、既存の実行済み証拠・テストは無断削除しない。根拠：[Project Architecture](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Architecture.md#2026-10-25-author-approved-amj-numeric-balance-and-support-profiles)。本決定は**設計・検証の優先順位の変更**であり、今回作物・料理XML、栽培値、MO互換・依存メタデータは変更しない。

---

#### 2026-10-07 Core / MO依存監査結果（初回分類・履歴）

`ARCH-MODULAR-001` の初回実装監査では、現行CoreのStage A / New VillageはMOなしではロード・進行できない直接参照を複数持つ一方、農業ループ自体はMO固有機能を必要としないと判断した。**この初回分類のうちGrains再編で変更された所有判断は、直後の「Grains再編後の最終 Base / MO 所有境界」を優先する。**

監査時点の実装事実:
- 本体には `1.6/` フォルダ、製品C#、製品DLLは存在せず、実行時実装は主にXML Def / Patchで構成される。C#はE2Eテスト側にのみ存在する。
- Production DefのParentNameはVanilla側の `PlantBase` / `PlantFoodRawBase` / `RoughPlantBase` / `BenchBase` 等で、MO ParentDef継承はない。
- MO向け `MayRequire` / `PatchOperationFindMod` は未実装で、`Patches/MedievalOverhaul_StageA_Wheat.xml` はMO存在を前提に無条件で読み込まれる。CCTO互換だけは既に `PatchOperationFindMod` で条件化されている。
- Base側の主な直接MO参照は、`DankPyon_Straw`、`DankPyon_BasicAgriculture`、`DankPyon_IronIngot`、`DankPyon_RawWood`、`DankPyon_Cereal`、MO小麦/RawWheat、New VillageのMO研究・初期物資、`DankPyon_Peasant` apparel tag、MO由来の仮テクスチャである。
- Paper / Paper Press、Salt、MO Drying Rack、Processor Frameworkは現行Stage AのProduction XMLから直接参照されていない。これらは設計上の将来連携であり、Agricultureのハード依存理由にはしない。

依存の分類と所有方針:

| 依存領域 | 分類 | Vanilla / Agriculture側 | MO併用時 |
|---|---|---|---|
| MO小麦そのものへの調整、MO RawWheat/Flour/製粉Recipeの変更、日本語ラベル上書き | **A — MO拡張そのもの** | 読み込まない | 専用互換Patchとして維持 |
| Awa/Hie/Kibi/Soba/Barleyの栽培・脱穀・殻取り | **B — 自己完結可能** | AMJ既存Def/Recipeを正本とする | 同じAMJ Defを利用 |
| Barley / 加工台の `DankPyon_BasicAgriculture` 前提 | **B — 自己完結可能** | AMJ所有の農業進行へ置換するか、研究なしの初期経路として成立させる。正確な研究Def/コストは実装前バランスで確定 | MO研究ツリーへの接続・置換は互換Patch |
| `DankPyon_Straw` を全穀物の脱穀副産物として直接出力 | **C — MO時だけ互換可能** | Strawを主要農業ループの必須出力にしない。Agriculture自身に用途がない段階では重複Straw Defを作らない | MO互換Patchから `DankPyon_Straw` を副産物として追加 |
| `DankPyon_RawWood` StuffCategory | **C — MO時だけ互換可能** | Vanilla `Woody` で成立 | MO時だけ許可素材へ追加 |
| 加工台の `DankPyon_IronIngot` cost | **B — 自己完結可能** | Vanilla資源で建設可能にする。専用AMJ金属Defは不要 | 必要ならMO互換でiron ingot costへ寄せる |
| `DankPyon_Cereal` ThingCategory | **C — MO時だけ互換可能** | Vanilla食事で利用できる食材属性を正本とする | MO汎用製粉・醸造へ接続する対象だけ条件付き登録 |
| MO小麦Plant/RawWheatをStage A小麦そのものとして利用 | **B — 自己完結可能** | 小麦をVanillaプロファイルでも提供するならAMJ所有のフォールバック小麦Plant/収穫物が必要。既存 `AMJC_Wheat` のDefNameは維持 | AMJフォールバック小麦を重複表示・栽培させず、MO小麦を公式互換から利用 |
| MO Flour / 製粉設備 | **C — MO時だけ互換可能**（依存解除の最低条件ではない） | Vanilla料理へ穀粒として消費できれば主要ループ成立。粉食を正式提供する段階でAMJ所有の粉/Recipeを追加。専用新規設備は必須ではない | MO Flour / Millstone / mill系経路を優先再利用 |
| MO仮テクスチャ（Barley wheat art、StonecuttingSpot、Millstone） | **B — 自己完結可能** | Production用AMJ画像へ置換 | 同じAMJ画像を利用してよい |
| New VillageのMO研究・ration/raw wood/iron ingot・knife stuff | **B — 自己完結可能** | Vanilla/AMJ所有要素だけでStartを成立させる | MOらしい研究・物資差分が必要なら条件付きPatch |
| `DankPyon_Peasant` apparel tag | **C — MO時だけ互換可能** | Vanilla `Neolithic` 等だけで成立 | MO時だけtagを追加 |
| MO Paper / Paper Press | **C — MO時だけ互換可能** | Agriculture本体では所有しない。植物/繊維原料までを必要に応じて所有 | MO製紙への原料接続は互換Patch。Vanilla製紙が必要になれば製紙側機能の責務 |
| MO Drying Rack / Processor Framework | **C — MO時だけ互換可能** | Agricultureの依存にしない | MO設備へ接続する機能だけ条件付き利用。別AddonがPFを必要とする場合はMOの推移的依存に頼らず直接依存を宣言 |
| MO Salt | **C — MO時だけ互換可能** | Agriculture本体では所有しない | 塩を必要とするPreservation / Coastal系等の責務側で条件付き接続 |

この監査では、**A分類の存在はCoreをMO必須にする理由にならない**。Aは「MOがある時だけ意味を持つ互換機能」なので、Baseから隔離・条件化する。

---

##### 識別子と2026-10-10当時のMO依存解除・新規導入監査（依存解除部分は履歴）

Grainsは作者確認時点でまだどこにも導入されておらず、旧Core/Grainsセーブが存在しない。**旧セーブの移行、途中導入・削除、既存セーブからのMO取り外しは開発・公開の必須ゲートから除外する。** 過去の移行検査器・記録は歴史的診断用として保持し、現行の必須作業とは扱わない。

`packageId=sucro.ancientmedievaljapan.core`、`AMJC_` prefix、既存DefNameはAMJモジュール間・外部Patchとの参照安定性のため保持する。当時はMOなしでGrains独自の小麦・粉・石臼、MOありでMO提供資産を条件付きで利用していた。重複Def/Recipeや意図しない上書きを防ぐ契約は維持する。**当時のVanilla / MO × CCTO四構成テストは履歴**で、現行のMO必須Agricultureには新たなVanilla単独公開ゲートを課さない。

##### MO必須解除の実装順序（移行履歴。現行結果は文書冒頭を優先）

1. **テストを先に分岐**し、Production About.xmlを変更しなくてもMOなしのBase XMLをロードできるテスト用Vanillaプロファイルを用意する。
2. BaseのMO直接参照を除去する。Straw / Cereal / RawWood等はMO互換へ移し、建設材料・研究・ScenarioはVanilla/AMJ側で自己完結させる。
3. Barley/設備のMO仮画像をAMJ-owned Production画像へ置換する。
4. Stage Aで小麦をVanillaプロファイルにも提供する場合は、AMJフォールバック小麦を追加し、MO時の重複を抑止する。粉食は必要な用途が確定した範囲だけ追加する。
5. `Patches/MedievalOverhaul_StageA_Wheat.xml`、MOラベル、MOカテゴリ、MO Scenario/素材差分を明示的な条件付き互換へ隔離する。
6. Vanilla / MO × CCTO有無の自動マトリクスをすべて通し、runtime ERROR 0を確認する。
7. MOの必須指定をAbout.xmlと説明から解除し、MO用`loadAfter`・条件付き互換は残す。各テスト・説明の整合を確認する（2026-10-10反映）。

**2026-10-10当時の状態記録（現行リリースゲートではない）:** `Plant_Rice` 陸稲化・七穀環境回帰は実装され、MOなしの実ゲームSmokeにも過去の合格実績がある。作者指示によりAboutのMO必須宣言を先行解除した。当時は全構成の実播種・実収穫・季節・ロードを公開判断の要件としていた。**現行AgricultureはMO必須**であり、旧4構成ゲートは適用しない。旧PASSを変更後のMO必須構成の検証へ転用しない。

##### 旧MO依存解除時の自動テスト・サポートマトリクス（履歴）

以下は2026-10-10の4構成検証契約であり、**現行AgricultureのVanilla単独サポート・恒久4構成再実行義務ではない**。現在のMO必須構成の受入は必要な変更範囲に限定する。

| プロファイル | 主な自動確認 |
|---|---|
| Vanilla + Grains | 未解決MO Def/class 0、Stage A作物＋陸稲、一次加工、製粉・最低限粉食、Vanilla simple meal受入、runtime ERROR 0 |
| Vanilla + Grains + CCTO | 上記 + AMJC crop cold-tolerance extension |
| MO + Grains | MO小麦/Flour/Cereal/Straw等の互換Patch、重複小麦・小麦粉・石臼経路なし、陸稲のAMJ設定維持、runtime ERROR 0 |
| MO + Grains + CCTO | MO互換とCCTO互換の同時適用、Stage A回帰、runtime ERROR 0 |

**当時の4構成テスト方針:** 静的XML検証を前段に置き、Pickleで実ロード後のDef・Recipe・Scenario・Vanilla meal受入・MO互換・runtime ERRORを確認する。RimTest Reduxは、今後C#の移行補助や独立ロジックを追加した場合の単体/ロジック試験を優先し、現行のXML中心実装で無理に代替しない。人間の手動確認は、Production画像/UI/操作感など自動化できない項目に限定する。

Fermentation / Brewingは、GrainsやMOをハード依存にしない設計を採る場合、それぞれ **Vanilla単独の主要ループ** と **Grains併用時の追加原料接続** を最低限の自動ケースとする。MO設備/原料を正式互換する場合だけMO併用ケースを追加し、CCTOは発酵・酒造側が作物温度Defを直接変更しない限り直積マトリクスへ含めない。Fermentation + Brewing相互のケースも、一方の出力を他方が公式に消費する設計が確定した場合だけ追加する。

##### 2026-10-07 Grains再編時の Base / MO 所有境界（履歴）

以下のBase/MO表は**当時の実装・依存解除計画と保存済みフォールバック構造の記録**であり、2026-10-26のMO必須化と食用作物／非食用繊維の所有境界を覆さない。旧表の「最終」「MO任意」「Base必須」は現行のリリース条件・機能追加指示ではない。実際の確定数値・Def識別子・既存加工経路は変更しない。

| 領域 | Base（Vanilla + Grains） | MO併用時 | 最終所有判断 |
|---|---|---|---|
| アワ・ヒエ・キビ・ソバ・大麦 | AMJ既存Plant・収穫物・一次加工を使用（β期間は調理台でも脱穀・殻取り可） | 同じAMJ Defを使用（β期間は調理台も使用可） | **Grains** |
| 小麦Plant / 小麦束 | Grains所有の非MO小麦Plant・穀束を供給 | MOの DankPyon_Plant_Wheat / DankPyon_RawWheat を可視の供給元として使い、AMJフォールバックは重複表示しない | **概念・バランスはGrains、MO時の実Def供給元はMO** |
| 脱穀後の小麦穀粒 | 既存 AMJC_Wheat | MO小麦束をGrains脱穀工程へ通し、同じ AMJC_Wheat へ合流 | **Grains**。既存DefName維持 |
| 小麦粉 | Grains所有のフォールバック小麦粉 | DankPyon_Flour を唯一の標準小麦粉として利用し、AMJ小麦粉を二重表示しない | **Base概念はGrains、MO時の実Def供給元はMO** |
| 蕎麦粉 / 雑穀粉 / 大麦粉 | Grains所有 | Grains所有の専用粉とし、MO汎用小麦粉へ自動統合しない | **Grains** |
| 製粉Recipe | 4種の穀粉を対象とし、GrainsがVanilla側の各Recipeを所有 | 小麦はMO既存経路を再利用。他3種はGrainsのRecipeをMO石臼へ追加。米粉は休止し通常のBillから外す | **工程仕様はGrains** |
| 石臼 / 最低限の製粉設備 | Grains単体で使えるAMJ所有の石臼を持つ（表示名：石臼 / stone mill。内部DefName `AMJC_ManualMillstone` は互換維持）。最低限経路はMO研究なしで成立 | MOの DankPyon_Millstone を標準設備として再利用し、AMJ石臼は重複表示しない | **BaseはGrains、MO時の実Def供給元はMO** |
| DankPyon_Cereal | 存在しない前提。Base主要ループに使わない | MO既存製粉/醸造へ参加させる対象だけ登録。未脱穀束・ソバ・雑穀を一律登録しない | **MO互換専用** |
| Straw | **AMJ Straw ThingDefを作らない。** Base脱穀では茎葉残渣を非アイテム化 | 脱穀時だけ DankPyon_Straw を副産物として追加。収穫時Hay・製粉時Hay/Strawは出さない | **MO時のみMO所有資産を利用** |
| 作物・加工研究 | Grains所有の大麦・一次加工台は初期利用可能。手動製粉をMO研究で止めない | MO併用時もGrains所有の大麦・一次加工台にMO研究条件を追加しない。MO所有小麦Plant側の研究条件は維持 | **Grains所有Defの解禁はGrains優先** |
| 建築材料 | Vanilla / Grains所有のStuff・Thingだけで建設可能 | Grains加工台はMO併用でもSteel 30を維持。MO原木カテゴリ等は互換として追加可能だがGrains側の確定コストを置換しない | **Grains設備の確定コストはGrains優先** |
| New Village Scenario | **独立Scenarios Modだけが所有**。Grainsの旧定義・互換コピーは廃止 | ScenariosがGrains/MO導入時の物資・研究差分を任意互換として所有 | **Grainsから完全除去**。Scenariosの実ゲーム開始は別途検証 |
| CCTO | なくてもGrains作物は成立 | CCTO存在時だけ既存互換Patch | **任意互換** |
| 共通の稲 / `RawRice` | Vanilla `Plant_Rice` / `RawRice` を再利用し、V1では畑地向けに調整 | 共通Plantを保持し、将来の水田環境で生育・収量を補正 | **Grains**。陸稲・水稲・うるち・もち米の別Plant/食材/米粉ThingDefは増やさない |
| 水田・水管理 | 所有しない | 所有しない | **Rice Cultivation**。同じ `Plant_Rice` の水田対応を所有し、Waterworksは任意の供給基盤 |
| 豆類 | Agriculture更新段階で栽培・収穫を所有する（V1未実装） | MOの品種・料理は歴史監査後に個別選別 | **Agriculture（2026-10-23決定）** |
| 繊維作物の栽培・収穫原料／紡績・織布・製紙 | 旧V1では対象外。Agricultureで繊維植物の栽培・農産原料を所有し、加工産業は他Owner | 現在のMO `DankPyon_Plant_Flax`と`DankPyon_RawFlax`を`Patches/MO_FibreCropPurpose.xml`で繊維作物/原料へ分類（MO任意） | **栽培・一次農産原料＝Agriculture／既存MO紡績・麻布・研究・画像＝MO Japanization、独立産業の新規実装＝別途決定** |
| 根菜・一般野菜 | V1対象外、Agriculture将来更新の栽培・収穫・食材化 | 不適切なMO作物の通常取得・料理を歴史選別で除外 | **Agriculture（2026-10-23決定）** |

---

**小麦の旧Base/MO実装境界（旧MOなし供給は保存済み履歴、現行はMO必須）**

現行コードの残存MO依存と検証範囲は [`GrainsDependencyAudit.md`](GrainsDependencyAudit.md) を参照する。Def識別子の分離完了を、画像・実ゲームを含む独立動作の完了と解釈しない。

- MOなし: Grains小麦Plant → Grains小麦束 → 脱穀 → AMJC_Wheat → Grains小麦粉。
- MOあり: MO小麦Plant → DankPyon_RawWheat（小麦束）→ 脱穀 → AMJC_Wheat → DankPyon_Flour。
- 同一プロファイルでAMJ小麦PlantとMO小麦Plant、AMJ小麦粉とMO小麦粉を標準経路として並存させない。
- 既存MO+Coreセーブで参照されているMO小麦/RawWheatと AMJC_Wheat の関係は維持する。Core更新と同時にMOを既存セーブから外す移行は別ケースとして扱う。
- MO小麦の成長・肥沃度等はGrainsの穀物バランスに合わせて条件付きPatchし、供給元がMOであることを理由に未調整値へ戻さない。

**Strawの生産・副産物境界（旧Vanilla単独の理由は履歴）**
- Grains単体ではStraw ThingDefを所有しない。
- Vanilla + Grainsの主要ループで独立した需要先を持たず、依存解除のためだけに追加すると用途の薄い在庫を増やすためである。
- MO併用時にはMO Strawの既存需要があるので、穀束の脱穀時だけ DankPyon_Straw を追加する。
- MO Strawの価値が作物選択を過度に歪めないこともMOプロファイルの回帰テスト対象とする。
- 将来、MOなしでも藁を主要資源として消費するAMJ機能が成立した場合は、その自然な所有Modを決めてGrainsと条件付き互換する。

**研究・材料・Scenarioの分離条件**

Base側から次の無条件参照を0にする。
- DankPyon_BasicAgriculture とNew VillageのMO研究
- DankPyon_IronIngot / DankPyon_RawWood
- DankPyon_Peasant
- DankPyon_MealRations 等、ScenarioのMO専用初期物資
- DankPyon_Cereal / DankPyon_Straw
- MO小麦 / RawWheat / Flour / MillstoneへのBase直接参照
- MO由来のProduction仮テクスチャ

Scenarioの旧具体値表は移行中のMOプロファイル互換契約とする。Baseの具体値はCore標準Scenario節に併記する。step 2では同package内でBaseとMO差分を分離済み。2026-10-07の作者決定により、最終所有先は開始シナリオ専用の独立Modとする。物理移行は下記の移行条件を満たしてから実施し、現行Defの存在をGrainsの恒久責務と解釈しない。

---

**2026-10-07～10-10依存解除の移行順序（実施履歴。現在の作業指示ではない）**
1. About.xmlを変えず、MOなしBase XMLをロードできるテスト用プロファイルを先に作る。
2. Base XMLからMO研究・素材・カテゴリ・Straw・Scenario参照を除去する。
3. 非MO小麦Plant/穀束、手動石臼、最低限粉食と穀粉の初期3種（小麦粉・蕎麦粉・雑穀粉）をBaseへ追加する。大麦粉も本番Def/Recipeで提供する。米粉・米粉団子の旧実装は2026-10-19に通常生産を停止し互換保持のみに変更した。
4. MO互換を条件化し、MO小麦/RawWheat/Flour/Millstone/Strawへ上表どおり収束させる。
5. MO仮テクスチャをGrains所有Production画像へ置換する。2026-10-08の作者指示により、MO有無にかかわらずAMJ側の共有画像設定を優先する。現在は既存AMJ/Vanilla仮参照で、MO時の旧画像復元は廃止した。専用Production画像・実描画は未完了（`GrainsDependencyAudit.md`）。
6. 当時のVanilla + Grains / Vanilla + Grains + CCTO / MO + Grains / MO + Grains + CCTOマトリクスに環境別穀物選択回帰を追加する計画だった（**現行の恒久実行条件ではない**）。
7. About.xmlの必須依存を解除し、任意`loadAfter`とREADME/Workshop等の公開説明を同期する（2026-10-10反映）。

**当時の次工程**はテストハーネス分離だった。現在の新規作業指示ではない。

**2026-10-07 ハーネス分離の実装:** `run-grains-tests.bat` と
`Docs/GrainsProfileTesting.md` に、実MO / 実CCTOの有無を選ぶ4プロファイルを追加した。
Production About.xmlを変えずにruntimeファイルをそのままテスト専用コピーへ読み込ませる。
旧MO/CCTO fixtureの8シナリオとは別に、各構成の読み込み・一次加工・任意CCTOを
確認する移行用smokeを置く。step 2でNew Village実開始、step 3で製粉・粉食のロード後契約を追加したが実機未実行であり、陸稲を含む七穀選択を含むリリース条件全件を満たした扱いにはしない。
**2026-10-07 Base分離 step 2:** Base Defs/翻訳からMO研究・素材・カテゴリ・Straw・Scenario・apparel参照を除去した。大麦と加工台はBaseでは研究不要、加工台の金属はSteel 30、簡易加工場はWoodyのみとする。MO併用時は `loadFolders.xml` の IfModActive により `Compatibility/MedievalOverhaul` を読み込み、旧38件の明示AMJC Def/抽象Def契約を保持する。MO小麦脱穀、外部MO小麦PatchとRawWheat翻訳もこの条件付きフォルダへ移した。step 2時点ではBase小麦の脱穀Recipeを非公開とし、次のstep 3で復帰した。

4構成のPickle featureにNew Villageのロード後設定と実開始を追加した（各5シナリオ）。静的projectionはAMJ所有XMLとBase差分Add/Replaceだけを検証し、RimWorldの継承・全Patch・Def解決・描画の代替ではない。step 2時点では小麦・製粉・粉食は未実装だった（次のstep 3でXMLを追加）。MO仮画像3件の置換、実機4構成、セーブ移行は未完了。Production About.xmlのMO必須依存は維持する。

**2026-10-07 step 3 小麦・製粉・最低限粉食の初期実装:**

- `loadFolders.xml` はrootと、MO時の `Compatibility/MedievalOverhaul`、非MO時の `BaseWithoutMO` を排他的に読み込む。IfModNotActiveは添付MO 1.6 LoadFolders.xmlの実使用を確認した。Base専用小麦/小麦束/小麦粉/石臼/小麦製粉がMO時に重複しない。
- 非MO小麦は `AMJC_Plant_Wheat` → `AMJC_RawWheat` →既存 `AMJC_ThreshWheat` / Bulk →既存 `AMJC_Wheat`。12日・収量28・肥沃度最低0.7/感応度0.9、研究なし、束30日、脱穀1:1・Strawなし。最低成長0℃、高温側58℃、最適10～42℃をBaseの明示初期値とする（Vanilla標準値との実機比較は未実施）。CCTO時だけ-6℃固定枯死拡張を付ける。
- 製粉は `AMJC_MillWheat` / `AMJC_MillBuckwheat` / `AMJC_MillMillet`。可食穀粒10→粉10、各0.05栄養で総栄養0.5を保存、workAmount 300、当初Crafting（2026-10-17以降はCooking）。粉は60日保存・直接摂食不可。小麦粉 `AMJC_WheatFlour` はBaseだけ、`AMJC_BuckwheatFlour` / `AMJC_MilletFlour` は両プロファイルで所有する。
- Base設備は `AMJC_ManualMillstone`（2026-10-20の暫定採用版では建築時に選ぶ石材 `Stony` 30 / WoodLog 20）。MO時は既存 `DankPyon_CraftFlour` でAMJC_Wheat→DankPyon_Flourへ進み、蕎麦/雑穀製粉を `DankPyon_Millstone` に接続する。最低限製粉を研究で止めないためMO石臼のresearchPrerequisitesを条件付きで除去する。MO小麦の既存研究と旧38件のAMJC契約は維持する。MO側の研究除去Patchは実ソースに対する静的参照確認であり、他Modとの実ロード優先順位は未検証。
- 最低限粉食の初期値は以下。各Recipeは研究・Cooking最低技能なし、Campfire / ElectricStove / FueledStoveで利用可能、workAmount 300、Nutrition換算入力0.5→料理1個0.9、2.5日保存、`AMJC_AteFlourFood` のMood +2（0.5日、stackLimit 1）。MO時だけ餺飥の入力をDankPyon_Flourへ置換する。

| 料理 | ThingDef | RecipeDef | Base入力 | MO入力 |
|---|---|---|---|---|
| 餺飥 | `AMJC_Houtou` | `AMJC_CookHoutou` | `AMJC_WheatFlour` | `DankPyon_Flour` |
| そばがき | `AMJC_Sobagaki` | `AMJC_CookSobagaki` | `AMJC_BuckwheatFlour` | 同左 |
| 雑穀団子 | `AMJC_MilletDumplings` | `AMJC_CookMilletDumplings` | `AMJC_MilletFlour` | 同左 |

新規説明文は日本語先行レビュー前のため空欄とし、未承認の歴史的説明を英訳・公開していない。旧小麦Recipeの説明/翻訳は保持する。新規画像は生成せず、Base小麦は既存AMJアワ、束・粉は既存AMJ雑穀、石臼・料理はVanillaの開発用画像参照を使う（step 3当時の記録。2026-10-08の実機画像ロード障害により、現行の粉食3品はAMJ同梱雑穀の仮画像へ変更した。詳細は本書の実機回帰注記を参照）。**これらは完成画ではなく、固有画像・日本語説明レビュー→英訳、実機の継承/参照/Graphic/Bill実行、環境別選択・既存セーブ回帰をリリース前に完了する必要がある。** Production About.xmlのMO必須依存はまだ変更しない。本段落はstep 3時点の記録であり、現行の開始シナリオ互換コピーは `LegacyStartingScenarios` へ移転済みである（`Docs/ScenarioExtraction.md`参照）。

`Tests/validate_grains_chain.py` と変異回帰は入力/出力、栄養保存、研究不要、保存/Mood、MO差替え/非重複を静的確認する。旧38件はそのまま照合し、新規11件の共有Defだけ追加許可する。4構成のPickleに製粉/粉食のロード後契約を加え、各6シナリオとした。ゲーム内でBillを完了した証拠ではなく、実機実行自体も未実施。

---

旧Base列は保存済み非MO実装の接続記録であり、**現行の公開受入はMO必須構成の列を基準**とする。既存Defや旧参照を削除しない。

| 食材 | 旧Grains Base（履歴） | 現行MO必須構成 |
|---|---|---|
| 雑穀 | 既存Vanilla食事へ使用可。雑穀粉へ製粉 | 同左。MO generic Cerealへ自動登録しない |
| 大麦 | 既存食事に利用。大麦粉を製粉し麦粉粥を作る経路をソース追加 | 同左。MO汎用Flour/Aleへ自動参加しない |
| 小麦 | AMJC_Wheatを既存食事・Grains製粉へ | AMJC_WheatをMO製粉へ接続し DankPyon_Flour を得る |
| ソバ | 可食穀粒として既存食事へ。蕎麦粉へ製粉 | 同左。MO generic flourへ統合しない |
| 米（陸稲） | Vanilla `RawRice` の玄米と `AMJC_WhiteRice` の白米を飯・粥・Vanilla食事に使う。米粉には製粉しない | 同左 |
| 雑穀粉 | Grains粉食へ | 同左 |
| 蕎麦粉 | そばがき等のGrains粉食へ | 同左 |
| 小麦粉 | Grains所有のフォールバック粉 | DankPyon_Flour を標準小麦粉として利用 |
| 大麦粉 | `AMJC_BarleyFlour` と `AMJC_CookBarleyFlourGruel` ソース追加（実機未検証） | 同左。MO汎用小麦粉と区別する |
| 米粉（旧実装） | 製粉Billは無効、旧 `AMJC_RiceFlour` と米粉団子のDefNameを保存 | 同左。MO石臼への再登録を禁止 |
