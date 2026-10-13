# 稲の共通収穫後加工 — Vanilla米との接続設計

**Status:** 加工必須化のXML・日英翻訳に加え、実機Pickle四構成各6/6・ERROR 0を確認（2026-10-08）。**旧セーブ移行・季節別播種・最終画像・公開互換性は未検証**。
**Owner:** AMJGrains（共通穀物の収穫後加工）。将来のRice Cultivationは同じ `Plant_Rice` の水田栽培・水管理と任意の稲架掛けを担当する。

## なぜ見直すか

従来のVanilla `Plant_Rice` 陸稲Patchは `plant.harvestedThingDef` を上書きせず、可食状態の `RawRice` が畑から直接出ていた。これは**Vanilla食材・料理との互換性を保つ実装上の近道**であり、脱穀や籾摺りが不要という史実でも、他の畑作穀物と比較して無償の加工優位を与える積極的な設計判断でもない。

従来 `Docs/Design.md §2.4` の稲作共通工程では「稲→籾→米」を想定していた。陸稲統合でそれを引き継がなかったのは設計と実装の不整合。ユーザーの2026-10-08指摘を受け、**陸稲も収穫後の加工を必要とする経路へ変更する方針**とする。

## 目標の入出力

```text
Vanilla Plant_Rice（Grainsの陸稲設定）
  ↓ 収穫
AMJC_RiceSheaf（稲束・未脱穀）
  ↓ 脱穀（同一Grains加工場所/加工台）
AMJC_RiceInHull（籾・籾殻付き）
  ↓ 籾摺り（同一Grains加工場所/加工台）
RawRice（既存Vanillaの食用米）
  ↓
Vanilla CookMealSimple・既存の米を使うRecipe
```

- 追加するThingDefは**非可食中間物2件**。新しい可食米Def（`AMJC_Rice` 等）は**作らない**。`RawRice` の既存Def・料理フィルタ・交易・開始物資を無条件に差し替えない。
- `Plant_Rice` を複製せず、収穫先だけ `RawRice` → `AMJC_RiceSheaf` とする。畑の稲を収穫したときには加工が必要になる。一方、既に所持している `RawRice` や他Modが直接生成する `RawRice` は引き続き使用できる（アイテム自体の全世界的な精米強制ではない）。
- これによって、現行の「米作だけは脱穀・殻取りの追加労働0」というバランス上の例外は解消する。ただし実際の農業・加工労働差は作業速度、播種、収穫、搬送、栽培条件を含む実ゲームで再評価する。

## Recipeと数値の実装基準（実プレイ調整前）

| 実装Recipe | 入出力 | 単品workAmount | 10個一括workAmount |
|---|---|---:|---:|
| `AMJC_ThreshRice` / `AMJC_ThreshRiceBulk` | 稲束1/10 → 籾1/10 | 15 | 120 |
| `AMJC_HullRice` / `AMJC_HullRiceBulk` | 籾1/10 → Vanilla米1/10 | 10 | 80 |

- 実装workAmountは既存の雑穀・蕎麦・大麦の2段階加工と揃えた初期値で、**最終バランス値としては未確定**。1:1歩留まり、10個の一括処理、研究不要の加工場所/加工台は初期検証条件。稲束・籾の腐敗開始は各120日、双方食用不可とした。専用設備は追加せず、既存のGrains加工場所／加工台で扱う。
- MO使用時の稲藁は既存穀物の脱穀と同様に `DankPyon_Straw` を1:1生成する。将来Rice Cultivation側の任意乾燥による副産物と二重発生させない。MOなしに存在しないStraw Defは参照しない。
- `RawRice` のNutrition、腐敗期間、食事適性はVanilla側の値を維持。精米米/玄米/米糠を個別ThingDefとして増やすことを本変更の必須条件にしない。籾摺りから日常用の米へ抽象化する。
- **米粉への後続加工（2026-10-13の作者確定方針、本番Def/Recipeソース実装済み・実機未検証）:** Grainsが追加する米粉は、うるち米由来ともち米（糯米）由来の粉を一つの穀粉ThingDefへ抽象化する。既存の`RawRice`を製粉原料に用い、もち米専用Plant/穀粒/製粉設備を増やさない。稲束→籾→`RawRice`の既定ループとVanilla米食材互換は維持する。これは実際のうるち米・もち米の性質や搗き餅・粉食の製法を同一視する歴史記述ではない。米粉のThingDef・製粉Recipe・米粉団子Recipeは既にソース実装済みだが、実機のBill動作・史実説明の最終校閲・先行Modとの詳細互換性は未検証である。
- **画像（ソース導入済み・実描画未検証）:** 籾 `AMJC_RiceInHull` の専用画像を追加し、Vanillaの食用米 `RawRice` もGrainsがAMJ画風へリテクスチャしている。既存 `RawRice` Defを複製せず、画像参照だけをAMJ固有パスへ置換済み。ロード後のgraphic stateと通常ズーム表示は別途確認する。料理互換性・DefName・バランスは変更しない。
- 史実では杵と臼を用いて脱穀と籾摺りを連続作業で行った事例もある。2種類のRecipeはプレイヤーに加工負担・保存中間物を示す**ゲーム内抽象化**であり、古代に全地域・全時代で二つの専用設備が必要だったとの主張ではない。
- 陸稲の従来 `growDays 5 / harvestYield 11 / fertilityMin 0.7 / fertilitySensitivity 0.8` は直ちには変更せず、七穀の比較を**加工労働込み**で再監査する。従来の「陸稲の最大収量6セル、単独首位なし」は収穫段階の解析であり、最終食材の効率を保証しない。

## 将来の水田・Rice Cultivationとの接続（2026-10-13 作者確定）

- **稲は一つの `Plant_Rice` とする。** 陸稲・水稲、うるち・もちを別の植物Def、可食米、米粉には分割しない。V1の畑地栽培設定と承認済みの説明文は維持する。
- **Rice Cultivationは水田・田植え・給排水・水分管理を所有する。** 同じ稲を水田に植える場合の生育・収量補正はここで実装する。水田での増産と水不足の不利は、適切な管理に対する報酬として表現する。肥沃度による成長速度と株当たり収穫数は別扱いとし、後者や水分履歴は必要なら追加ロジックで表現する。詳細値・仕事・条件は先行Mod監査と実機検証後に決める。
- **Grains併用時**は畑・水田とも `Plant_Rice → AMJC_RiceSheaf → AMJC_RiceInHull → RawRice` を共用し、収穫物・脱穀・籾摺り・製粉・可食米を二重定義しない。
- **Rice Cultivation単独時**はVanilla `Plant_Rice` / `RawRice` で独立に遊べる最低限の経路を用意し、Grainsの中間ThingDefに無条件依存しない。両Mod併用時はロード順、播種可能地形、CCTO・MO・Waterworksの任意性、実収量・水管理、旧セーブを検証する。
- Waterworksは任意の水利供給元であり、Grainsの畑地稲→籾→米ループの必須依存ではない。

## 実装ゲートと互換性（実機スモークPASS／残余ゲート未完）

1. **稲束／籾／脱穀／籾摺り** の確認済み日本語説明と対応する英文を実装済み。史実説明は新たな断定を加えず、ユーザー承認案を維持。
2. `Patches/UplandRice.xml` の収穫先を稲束に置換。共通Def・Recipe単品/一括、MO時のStraw、日英説明と既存AMJ仮画像を追加済み。実描画は未確認。
3. `Tests/test_upland_rice.py`、`Tests/test_grains_balance_chain.py`、`Tests/validate_stage_a.py`、`Tests/E2E/GrainsProfileSteps.cs`、`Tests/E2E/GrainsSimulationSteps.cs` を更新。**稲の実収穫→稲束→実Bill脱穀→実Bill籾摺り→RawRice→CookMealSimple** は4構成の実機Pickleで各6/6・ERROR 0を確認済み（旧セーブ互換は別検証）。
4. CCTO低温・MOあり/なし・旧セーブ中の `RawRice` ・Vanilla料理・直接Riceを入手する交易・他Modの米参照は維持し、実機ログ ERROR 0 と言語表示、テクスチャBadTexを検証する。
5. 旧監査の「陸稲加工労働0」は旧XMLの履歴値。新Recipeの加工合計は10原料あたり200。実機・保存・描画ゲートを経るまでリリース可能とは報告しない。

## 史料

- 農林水産省「お米の産地銘柄とブレンド米の進化」：https://www.maff.go.jp/j/syouan/keikaku/soukatu/okome_summary/01/type03.html （脱穀→籾乾燥→籾摺り→玄米）
- 山梨県埋蔵文化財センター「油田遺跡」：https://www.pref.yamanashi.jp/maizou-bnk/topics/101-200/0144.html （弥生期の竪杵と脱穀）
- クボタ「臼を使った籾摺り」：https://www.kubota.co.jp/kubotatanbo/history/tools/hulling.html （杵臼による処理、時代による工程の変化）


### 2026-10-08 実機スモークの合格範囲

`automated-gates(4).log` により、Vanilla / Vanilla+CCTO / MO / MO+CCTOで各6/6のPickleと隔離ランタイムERROR 0を確認した。新規隔離テスト上で稲束→籾→Vanilla米の実Billを含む既定のシナリオが完了した。料理用仮PNGの読込障害も解消。これにより四構成実機スモークのゲートを完了とするが、実ユーザーの古いセーブ（農地、既存の `RawRice`、途中のBill、既存設備等）の移行、途中導入/削除の安全性と、季節別・低温下での実播種は未確認。最終専用画像の制作と目視も未完了。既存数値とRecipeを変更せず、今後の検証は `Docs/GrainsProfileTesting.md` に記録する。


### 旧米保存互換性の次ゲート（2026-10-08）

旧Coreで取得済みの `RawRice` を新しい `AMJC_RiceSheaf` に改名・書換えしない。既存の `RawRice`、`Plant_Rice`、`AMJC_` 保存アイテム、穀物加工設備と保存済みAMJC加工Billについて、RimWorld 1.6実機の停止時ロード→再保存前後でDefName、保存ID、数量、Billを失わないことを確認する。Grains側の読取専用XML比較器と負例テストは `Scripts/grains_save_contract.py` / `Tests/test_grains_save_contract.py`、詳細は `Docs/GrainsProfileTesting.md`。これらはXML保存契約の準備であり、実機エンジンでの旧セーブ読込・保存互換性はまだ未合格。Scenarios/Faction/PawnKindの移行はScenarios側を正本とする。
