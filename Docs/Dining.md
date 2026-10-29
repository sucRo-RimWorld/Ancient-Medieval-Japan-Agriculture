# 献立・配膳の移管（2026-10-30）

一膳化・配膳・喫食・Daily Menu互換処理の正本は[AMJ Dining](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Dining/blob/main/Docs/Design.md)へ移管した。AgricultureはDiningを必須とし、料理・役割指定 `Patches/DailyMenu_MealRoles.xml` と汎用簡単な食事の新規Bill停止を所有する。

DiningはCulture系カテゴリだがCulture本体を要求しない。AgricultureやMOへの逆依存もない。今回の移管ではDaily Menu依存と既存取餐挙動を維持する。主菜・添え物の独立枠、膳UI、盛り付け・品数再設計は後続作業。

従来のDLLはDiningだけが配布する。両Modを対応版へ更新してDiningを有効にする必要がある。旧AgricultureのDLLとの二重ロードは禁止。ゲームの有効Mod設定・既存セーブは自動変更していない。

[従来の実行証拠と限界](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Dining/blob/main/Docs/Dining.md)。移管後のMO併用一膳化は[限定実ゲーム確認](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Dining/blob/main/Docs/ExtractionValidation.md)で合格。実ユーザー旧セーブ移行は未検証。
