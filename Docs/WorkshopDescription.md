# Grains — Steam Workshop description

The single authoritative paste-ready description is `Docs/SteamWorkshopDescription.txt`. Paste **its entire English-then-Japanese body into Steam's English description field only**; do not paste it a second time into Japanese. `Docs/SteamWorkshopDescription-ja.txt` is the Japanese drafting source. No image is reused separately per language; any future images must appear exactly once with English/Japanese captions. Keep the full combined body under 8,000 UTF-8 bytes, omit license/AI/donation disclosures, preserve dependencies and current development-status warnings. This repository draft does not mean the Mod is ready to publish.

Title and About name: `Ancient & Medieval Japan - Grains（中世日本 - 穀類）`. The current `packageId` remains `sucro.ancientmedievaljapan.core`. The unpublished Crop Growth Balance module is integrated here (six Vanilla and seven optional MO crop growth timings, plus MO lentils skill 2); do not publish/install an independent CGB payload. New Village belongs to the separate Scenarios Mod. Base wheat/processing and MO conditional integration are included; exact-current-runtime and final-art release gates remain independently open; Grains has no preexisting deployed saves.

## Preserved historical Grains V1 comparison image (not in the current draft)

- Immutable approved 1672×941 PNG master: `Art/Sources/Workshop/GrainsV1FoodComparison_EN-JA.png`.
- Byte-identical web-facing image: `Docs/Media/GrainsV1FoodComparison_EN-JA.png`. Both Git blobs must equal `18957f3c0d7f4283ab6b3cafd8a50891087db466`; source SHA-256 `6f68e2ff9206b4c904861af4e13acb2a151ef48c7a842a092cb2f50a266959c3`.
- The comparison depicts the superseded five-flour/five-flour-dish inventory and must not appear in the **current** Workshop draft. Both accepted PNG copies are preserved unchanged. Do not quietly replace or revise the artwork: a new current-spec comparison requires separate author acceptance before inclusion.
- Author-controlled Steam publication is separate: GitHub file presence, draft BBCode and CI do not establish that the live Workshop page has been edited or rendered correctly. Confirm rendering after manual update. Neither Art nor Docs enters the subscriber's in-game Workshop payload.

## Language divider and optional MO ordering

Place one Steam `[hr][/hr]` after the complete English description and before the Japanese section. The current draft does not display the superseded comparison image; an additional divider is needed only if a new, author-accepted shared image is later appended. The Production `About.xml` has **no hard MO dependency** and retains MO and CCTO in optional `loadAfter` (including Adaptive Primitive Storage). Adaptive Primitive Storage is **strongly recommended but not a dependency** for granary storage, keeping APS's original Hay-cost buildings and, with MO, presenting separate Straw-cost building choices; `loadFolders.xml` selects MO compatibility only when present. This metadata decision does not certify final graphics or completed fresh-start public-release testing. Historical-save/provider-removal tests are out of scope because no Grains saves exist. Preserve the stable packageId.

## V1非公開配布の準備・残ゲート（2026-10-13）

**2026-10-13 tracked-content filter audit (static, not installed payload):** `main` tracked inventory had 462 blobs; `.rimignore` basename rules select **93 subscriber-oriented files** (About 4, BaseWithoutMO 12, Compatibility 9, Defs 10, LICENSE 1, Languages 9, LegacyStartingScenarios 9, Patches 5, Textures 33, `loadFolders.xml` 1), and exclude 369 including all Art/Docs/Tests. No non-runtime leakage or missing listed required metadata/loader/conditional Recipe XML was found by the independent path check. The approved Workshop comparison image is **not** a packaged Texture; Art source and Docs/Media Git blobs are identical and remain source-only. `Tests/Release/validate_workshop_payload.py` remains the authoritative CI payload filter gate; 93 is an inventory snapshot, not an actual YADA zip install or Steam upload. About+Manifest both retain `0.1.0-dev`, without MO hard dependency; optional `loadAfter` and `loadFolders.xml` are preserved. Final local installation, subscriber re-download, rendered BBCode and private-play results remain OPEN.


- 説明本文には共通の畑地「稲」、7穀、4種の穀粉、共通3料理・指定粉食4料理・白米専用2料理（合計9料理/18調理Bill）、調理台を使う暫定加工を正確に記載。水田機能やMO必須依存は宣伝しない。
- 英語欄に日英併記を一度だけ掲載する。旧仕様の比較PNGは原本を保存し、現行本文には掲載しない。画像・キャプションの内容や元ファイルは変更しない。
- `About/About.xml` と `About/Manifest.xml` のバージョンは `0.1.0-dev` のまま。作者管理の `About/Changelog.txt`・既存 `PublishedFileId.txt` を勝手に置換しない。
- 実際に作者が非公開アップロードを行う前に、Workshop用のローカルインストール場所・加入者向けフィルタ・About/Manifest/Changelog・MO条件付き`loadFolders.xml`・承認済み画像・Workshop IDを確認する。正規の `Tests/Release/validate_workshop_payload.py` と `Tests/Release/validate_add_changenote.py` を使用する。
- **2026-10-20 現状:** 現行9料理/18 BillのRimWorld 1.6 Vanilla・CCTO実ジョブ各10/10、元Unity ERROR0。MO+CCTOは同じ9料理の実ジョブ10/10合格・Grains固有XML ERROR0だが、既知の外部MO/SYR音声FMOD2件で元のProject厳密ERROR0はFAIL。作者の決定により**Grainsが変更していない既知音声2件は初回公開の阻害要因としない**。その後追加した白粥単品・4食実BillはVanilla/CCTOで合格、MOでの追加分再実行は未実施。証拠・対象SHAは `Docs/GrainsProfileTesting.md` を参照。**引き続きHOLD:** 通常プレイの操作感・穀物バランス判断・通常ズーム画像、現行配布候補のローカル導入・Workshop実アップロードと再ダウンロード、公開承認。過去の93ファイルのフィルタ監査は当時の記録であり現行ファイル数ではない。作者の公開指示なしに版数やWorkshop IDを確定せず、Steam公開もしない。
