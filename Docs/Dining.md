# 献立・配膳の移管（2026-10-30）

> **履歴資料（2026-11-01新設計により将来方針として失効）。** 以下は旧Diningの実装・移管・実ゲーム検証の記録であり、現行ソースの旧依存関係を説明する。新設計は[Diningの新設計正本](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Dining/blob/main/Docs/Design.md)に従い、5段階料理Bill・材料ロール・栄養・料理製品の実装担当をAgricultureとする。Dining実行Modの存廃、旧DLL・Def・セーブ・依存関係の移行は未決定であり、以下の旧方式を新設計の承認として扱わない。

一膳化・配膳・喫食の正本は[AMJ Dining](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Dining/blob/main/Docs/Design.md)へ移管した。AgricultureはDiningを必須とし、料理・役割指定 `Patches/DailyMenu_MealRoles.xml` と汎用簡単な食事の新規Bill停止を所有する。

DiningはCulture系カテゴリだがCulture本体を要求しない。AgricultureやMOへの逆依存もない。2026-10-31: DiningはDaily Menu依存を除去し、Harmonyのみを必須とする。主菜・添え物の独立枠、膳UI、盛り付け・品数再設計は後続作業。

従来のDLLはDiningだけが配布する。両Modを対応版へ更新してDiningを有効にする必要がある。旧AgricultureのDLLとの二重ロードは禁止。ゲームの有効Mod設定・既存セーブは自動変更していない。

[従来の実行証拠と限界](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Dining/blob/main/Docs/Dining.md)。移管後のMO併用一膳化は[限定実ゲーム確認](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Dining/blob/main/Docs/ExtractionValidation.md)で合格。実ユーザー旧セーブ移行は未検証。

Daily Menuを外した現行版の[2026-10-31実ゲーム検証](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Dining/blob/main/Docs/IndependentValidation.md)は合格（喫食・運搬・保存・病人への給餌、ログERROR 0）。更新時はDaily Menuを無効にする。同時使用は非対応。
