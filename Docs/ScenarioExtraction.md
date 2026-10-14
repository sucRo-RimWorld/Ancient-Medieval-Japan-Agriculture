# Starting Scenario 所有境界（Grains / Scenarios）

2026-10-14 決定：非公開開発段階のため、旧 Core / Grains の開始シナリオ・セーブ移行互換は維持しない。旧版の互換コピーやロード条件は Grains から撤去する。これは Scenarios 専用リポジトリへの完全分離であり、Grains 単独で `New Village` は表示しない。

## 所有者

- **AMJ Grains** (`sucro.ancientmedievaljapan.core`)：穀物・栽培・加工・料理を所有する。ScenarioDef、開始用 FactionDef / PawnKindDef、初期物資・開始研究・開始ダイアログを提供しない。
- **AMJ Scenarios** (`sucro.ancientmedievaljapan.scenarios`)：`AMJC_NewVillage`、`AMJC_PlayerVillage`、`AMJC_Villager`、対応する日本語・英語、開始用 MO / Grains 差分を唯一所有する。Vanilla 単独で開始でき、MO と Grains は任意。
- Grains の `loadFolders.xml` は `/`、MO 互換、非 MO 補完の3領域のみ。旧 `LegacyStartingScenarios/` は本番に存在しない。

Scenarios 正本： [Ancient-Medieval-Japan-Scenarios](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Scenarios) の `Defs/`、`Compatibility/`、`Docs/RuntimeTesting.md`。新しい開始条件の仕様変更・テストはそちらで行う。Grains 側に移行用 Def を再作成しない。

## 静的・実機回帰

1. Grains Vanilla / MO の明示 XML は `ScenarioDef:AMJC_NewVillage`、`FactionDef:AMJC_PlayerVillage`、`PawnKindDef:AMJC_Villager` をいずれも持たない。
2. Grains の配布対象ファイルに `LegacyStartingScenarios/` や旧開始用パッチ・ダイアログを含めない。
3. Grains だけを有効にした新規開始画面に `New Village` は存在しない。Scenarios を有効にした場合だけ Scenarios が開始 Def を提供し、重複定義が発生しない。
4. 既存の穀物・MO・CCTO プロファイルと処理 Bills のテストは維持し、開始人数・研究・物資のテストは Scenarios が所有する。実ゲームでの検証は別途必要。

公開・既存セーブは存在しないため、旧セーブ読込／保存／削除時の互換ゲートや暫定コピーを、現行版の前提条件として扱わない。開発環境の古い展開先に手動で残ったファイルは、最新ソースのロード挙動と区別して確認する。
