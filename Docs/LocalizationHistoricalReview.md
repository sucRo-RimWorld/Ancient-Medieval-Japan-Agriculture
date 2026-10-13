# AMJGrains 日本語説明文・翻訳監査（2026-10-08）

**状態: 2026-10-08に§2の15件の日本語説明・作業文が作者承認済み。日本語DefInjectedと英語Def原文へ反映済み。既存Plantの追加史実監査と実ゲームUI表示は未完了。**

本書はAMJGrains固有の承認済み説明と根拠の正本。共通規則は Project `Docs/HistoricalDescriptionGuidelines.md`。§2の日本語候補は今回の承認により確定し、§6の英訳と共に本番へ反映した。DefName・既存labelは変更せず、同時代比較が未監査の既存植物説明については別途継続する。

## 1. 実装・和訳の監査結果

| 対象 | 現状 | 対応 |
|---|---|---|
| アワ・ヒエ・キビ・ソバ・大麦、収穫物・穀粒 | Plant5件の史実・地域情報は§7で承認済み、収穫物・穀粒には既存の機能説明を維持 | Plant5件の説明を日英で本番反映。**Baseで研究不要の大麦**という実装に反する旧記載も訂正済み |
| `AMJC_Wheat` と `AMJC_Barley` | 前者はMO醸造だけを説明、後者は未実装の麦茶・味噌を予告 | Base/MO共通の実装済み食材用途だけに訂正 |
| 共通加工Recipe（雑穀・ソバ・大麦） | 日本語descriptionに「藁を得る」とあり、Baseでは藁を生成しない | Base/MO共通の「殻付き穀物を得る」説明に訂正 |
| 小麦脱穀Recipe | BaseとMOで同じDefNameだがMOでのみ藁生成 | Base翻訳のみ訂正し、MO翻訳は維持 |
| `Plant_Rice` / `RawRice` | Vanilla米を陸稲化し、稲束・籾から加工して既存`RawRice`を得る | 新たな可食米Def/Plantは作らず、水田栽培は別Modの責務 |
| 粉 `AMJC_BuckwheatFlour` / `AMJC_MilletFlour` | ThingDef本体description空、和訳labelのみ | 承認済み、日本語・英語へ反映 |
| 料理 `AMJC_Houtou` / `AMJC_Sobagaki` / `AMJC_MilletDumplings` | ThingDef本体description空、和訳labelのみ | 承認済み、日本語・英語へ反映 |
| 共有の製粉・料理Recipe5件 | description空、和訳labelのみ、英語jobStringが残る | 説明文と作業表示の承認済み、日本語・英語へ反映 |
| 非MOの小麦・小麦束・小麦粉・手動石臼（4 ThingDefs）、小麦製粉Recipe | description空、和訳labelのみ | 承認済み、日本語・英語へ反映 |
| MOの既存小麦粉・石臼 | 外部MO所有Defを条件付きで再利用 | グラフィック・歴史説明の本格的日本化はMO Japanizationの所有。Grainsは自Modの互換説明のみ |

## 2. 承認済み日本語説明文（本番XMLに反映）

### 粉・料理

| DefName | 採用済み日本語description |
|---|---|
| `AMJC_BuckwheatFlour` | **蕎麦粉（そばこ）。** 殻を取った蕎麦の実を挽いた粉。湯で練る蕎麦掻きなど、麺にしない粉食にも利用される。 |
| `AMJC_MilletFlour` | **雑穀粉（ざっこくこ）。** 粟・稗・黍から得た可食穀粒を挽いた粉。AMJGrainsでは三種の雑穀を共通の粉にまとめ、団子の材料にする。 |
| `AMJC_Houtou` | **餺飥（はくたく）。** 小麦粉を練って加熱する古い粉食を表現したもの。平安期の史料に見える餺飥と現在の山梨のほうとうには関係が指摘されるが、料理として同一だったとは断定できない。ここでは現代の具だくさんの味噌煮込みを再現せず、簡素な小麦粉食として扱う。 |
| `AMJC_Sobagaki` | **蕎麦掻き（そばがき）。** 蕎麦粉を湯で練って食べる、麺にしない粉食。蕎麦切りが広く普及する以前にも、蕎麦の実を炊くほか粉を練って食べる方法があった。 |
| `AMJC_MilletDumplings` | **雑穀団子（ざっこくだんご）。** 粟・稗・黍などの粉を水で練って加熱する団子状の食物を、一品として抽象化したもの。近世以降の甘味菓子そのものを再現する料理ではない。 |

**表現上の留保:** 餺飥の存在は文献で知られる一方、平安期の餺飥と現代山梨のほうとうが同一の料理であるとの確証はない。麺を打つ工程もAMJGrainsには個別実装しない。雑穀団子も「特定時代の特定レシピ」を忠実再現したとは主張しない。

### 非MOの小麦・石臼

| DefName | 採用済み日本語description |
|---|---|
| `AMJC_Plant_Wheat` | **小麦（こむぎ）。** 日本へは弥生期に伝わり、奈良時代にも栽培が確認される穀物。小麦の穀粒は食事の材料に使え、石臼で挽けば小麦粉として粉食にも利用できる。AMJGrainsでは刈り取った小麦束を脱穀して使う。 |
| `AMJC_RawWheat` | **小麦束（こむぎたば）。** 穂と茎を付けたまま収穫した小麦。未脱穀のままでは食べられず、脱穀によって小麦穀粒になる。 |
| `AMJC_WheatFlour` | **小麦粉（こむぎこ）。** 小麦穀粒を石臼で挽いた粉。AMJGrainsでは餺飥を作るために使う。 |
| `AMJC_ManualMillstone` | **石臼（いしうす）。** 手で穀粒を挽いて粉にする石製の道具。AMJGrainsでは小麦・蕎麦・雑穀の製粉に使用する。 |

### 加工・粉食Recipe（入出力を現行XMLと一致させる案）

| Recipe | 採用済みdescription | 採用済みjobString |
|---|---|---|
| `AMJC_MillWheat` | 脱穀した小麦穀粒10個を挽いて、小麦粉10個にする。 | 小麦を製粉している。 |
| `AMJC_MillBuckwheat` | 殻を取ったソバ穀粒10個を挽いて、蕎麦粉10個にする。 | 蕎麦を製粉している。 |
| `AMJC_MillMillet` | 雑穀10個を挽いて、雑穀粉10個にする。 | 雑穀を製粉している。 |
| `AMJC_CookHoutou` | 小麦粉から餺飥を1個調理する。 | 餺飥を調理している。 |
| `AMJC_CookSobagaki` | 蕎麦粉からそばがきを1個調理する。 | そばがきを調理している。 |
| `AMJC_CookMilletDumplings` | 雑穀粉から雑穀団子を1個調理する。 | 雑穀団子を調理している。 |

粉食Recipeは**Nutrition 0.5相当の粉**を要求する（粉1個のNutritionは0.05）。上表の製粉数量と混同しない。

### 既存Plant和訳の歴史説明（§7で承認・反映済み）

- アワ・ヒエ・キビ: 「粟（あわ）」「稗（ひえ）」「黍（きび）」の名形を確認。史実の気候耐性とゲームの収量・枯死値は分けて説明する。
- ソバ・大麦: 「蕎麦（そば）」「大麦（おおむぎ）」の名形を確認。江戸期の蕎麦切り普及を中世の標準食としない。
- 陸稲: 「陸稲（おかぼ）」の読みと栽培形態を確認。水田・苗代の機能をGrainsが提供すると書かない。
- 中間物: 雑穀・ソバ・大麦の「未脱穀→殻付き→可食」を維持し、同一ThingDefへ集約されるアワ/ヒエ/キビを別々の食材として誤記しない。

## 3. 主な典拠（史実の日本語確認用）

- 小麦/大麦の伝来と奈良時代の栽培: 農林水産省「特集1 麦(1)」 https://www.maff.go.jp/j/pr/aff/1602/spe1_01.html
- 餺飥と現在のほうとうの概要: 農林水産省「ほうとう」 https://www.maff.go.jp/j/keikaku/syokubunka/traditional-foods/menu/hoto.html
- 平安期の「はくたく」と現代ほうとうの直接的なつながりが未確定: 国土交通省「ほうとう」 https://www.mlit.go.jp/tagengo-db/R1-00166.html
- そばがき・蕎麦切りの歴史的な違い: 農林水産省「特集2 新そば」 https://www.maff.go.jp/j/pr/aff/1811/characterinformation.html
- 団子状の粉食に関する長い歴史: 農林水産省「和菓子の歴史」 https://www.maff.go.jp/j/pr/aff/2002/spe2_01.html
- 地域ごとの雑穀食: 文化庁「にし阿波地域の雑穀食」 https://www.bunka.go.jp/seisaku/bunkazai/joseishien/syokubunka_story/93727709.html

これらは歴史説明を起案する根拠であり、特定地域の近世以降の習俗をすべて中世へ遡らせる根拠ではない。

## 4. 反映済み・未完了ゲート

1. §2の**日本語15件が作者承認済み**。共通/非MO各フォルダのThingDef/RecipeDef日本語descriptionと6件のjobStringへ反映した。Grainsの名称を表示文中では **AMJGrains** に統一し、小麦の「実は」の両義性も修正した。
2. §6の英文は承認済み日本語から翻訳し、各ThingDef/RecipeDefの英語原文のdescriptionおよびjobStringへ反映した。日本語だけにない新しい歴史断定を追加しない。既存label・DefName・packageId・料理内容は変更しない。
3. `Tests/test_grains_localization.py` は§2/§6の承認済み文面・ローカライズキー・読み込み対象ファイルと本番XMLの一致を静的検証し、将来の一方だけの文面改変を検出する。
4. §7の六作物の歴史説明は追加承認後に日英へ反映済み。**実ゲーム4構成での表示・言語切替・ERROR 0** は別ゲートのまま。静的検証を実機成功と扱わない。

## 5. 旧MO翻訳Fixtureとの整合（2026-10-08 CI回帰）

`Tests/Fixtures/MO_PreSplit_Contracts.json` は過去のMO必須版を固定する履歴データであり、今後のBase/MO両立を妨げる目的ではない。共通日本語RecipeDefの脱穀説明6件を、Baseでも事実になる「殻付き穀物を得る」記述へ修正したため、旧FixtureにあるMO限定の藁記述から意図的に差分が生じた。

`Tests/validate_grains_base.py` はこの6件だけを**現行の中立表現へ完全一致**で検査し、その後歴史Fixtureの旧文言へ検証用メモリ上で戻して、残りの翻訳・ゲーム仕様・MO互換の履歴ハッシュを照合する。旧Fixtureファイルは一文字も変更しない。未承認の翻訳・加工成果物の変更が紛れても通過しないよう、固定翻訳と中立化6件の負例テストを追加した。


## 6. English localization (translated from approved Japanese, 2026-10-08)

The following descriptions and jobStrings are the approved Japanese-first implementation translations. They are the English Def original strings; do not add unsupported historical claims to this table without revisiting the Japanese source.

| DefName | English description | English jobString, if Recipe |
|---|---|---|
| `AMJC_BuckwheatFlour` | Buckwheat flour (sobako). Flour made from buckwheat grains after their hulls have been removed. It is used in flour-based foods such as sobagaki, made by mixing the flour with hot water rather than forming noodles. | — |
| `AMJC_MilletFlour` | Millet flour (zakkokuko). Flour ground from edible grains of awa, hie, and kibi. In AMJGrains, these three millets share one flour item used to prepare dumplings. | — |
| `AMJC_Houtou` | Hakutaku (餺飥). A dish representing an old kind of wheat-flour food prepared by kneading flour and heating it. Hakutaku in Heian-period records has been linked to present-day Yamanashi hoto, but the two cannot be assumed to be the same dish. Here it represents a simple wheat-flour food, not a modern miso-simmered dish with many ingredients. | — |
| `AMJC_Sobagaki` | Sobagaki (蕎麦掻き). A flour-based dish made by stirring buckwheat flour into hot water instead of shaping it into noodles. Before soba noodles became widespread, buckwheat was also eaten as cooked grains or kneaded flour. | — |
| `AMJC_MilletDumplings` | Millet dumplings (雑穀団子). A single game dish representing dumpling-like foods made by mixing flour from awa, hie, or kibi with water and cooking it. It does not recreate the sweet confections of the early modern period or later. | — |
| `AMJC_Plant_Wheat` | Wheat (komugi). A cereal introduced to Japan in the Yayoi period and known to have been cultivated during the Nara period. Wheat grains can be used in meals or ground into flour for flour-based dishes. In AMJGrains, harvested wheat sheaves must be threshed before use. | — |
| `AMJC_RawWheat` | Wheat sheaf (komugitaba). Harvested wheat with the heads and stalks still attached. It cannot be eaten before threshing, which produces edible wheat grains. | — |
| `AMJC_WheatFlour` | Wheat flour (komugiko). Flour ground from wheat grains using a stone mill. In AMJGrains, it is used to prepare hakutaku. | — |
| `AMJC_ManualMillstone` | Stone mill (ishi-usu). A stone tool for grinding grains into flour by hand. In AMJGrains, it is used to mill wheat, buckwheat, and millet. | — |
| `AMJC_MillWheat` | Grind 10 threshed wheat grains into 10 units of wheat flour. | Milling wheat. |
| `AMJC_MillBuckwheat` | Grind 10 dehulled buckwheat grains into 10 units of buckwheat flour. | Milling buckwheat. |
| `AMJC_MillMillet` | Grind 10 units of edible millet into 10 units of millet flour. | Milling millet. |
| `AMJC_CookHoutou` | Prepare one serving of hakutaku using wheat flour. | Cooking hakutaku. |
| `AMJC_CookSobagaki` | Prepare one serving of sobagaki using buckwheat flour. | Cooking sobagaki. |
| `AMJC_CookMilletDumplings` | Prepare one serving of millet dumplings using millet flour. | Cooking millet dumplings. |

## 7. 既存六作物の承認済み歴史説明（2026-10-08）

**本節の六作物は2026-10-08に日本語説明を作者承認済みです。** ただし`Plant_Rice`のみ2026-10-13の作者指定によりV1表示名を「陸稲」から「稲」に改め、陸稲・水稲の品種／栽培法の違いとAMJでの統合方針を明示した。§7の日本語を正本として日本語DefInjectedへ、対応する英語を§8と本番Patchへ反映する。他5作物の承認済み文言、ゲーム挙動・栽培数値・Recipe・DefNameは変更しない。

| DefName | 承認済み日本語description | 設計値の照合 |
|---|---|---|
| `AMJC_Plant_FoxtailMillet_Awa` | **粟（あわ）。** 弥生時代の遺跡でも栽培が確認される畑作の雑穀で、米だけに頼らない食料の一つだった。AMJGrainsではキビより生育に時間がかかるが、一度の収穫量が多い。収穫後は脱穀と殻取りを行う。 | 6日／収量13。遺跡資料は吉野ヶ里歴史公園。 |
| `AMJC_Plant_BarnyardMillet_Hie` | **稗（ひえ）。** 古くから栽培され、地域によっては米よりも日常的に食べられた雑穀。山間部で稗をふだんの主食として利用した記録も残る。AMJGrainsではアワやキビより低温で生育できるが、霜への強さまで保証するものではない。収穫後は脱穀と殻取りを行う。 | 成長最低5℃（アワ・キビ8℃）。日常食は農水省の宮崎県北部事例であり全国一律の歴史ではない。 |
| `AMJC_Plant_ProsoMillet_Kibi` | **黍（きび）。** 米・麦・粟などと並び、「五穀」の一つに数えられることもある雑穀。AMJGrainsではアワ・ヒエより早く成熟し、肥沃度の影響も比較的小さい一方、一度の収穫量は少ない。収穫後は脱穀と殻取りを行う。 | 5日／収量11／肥沃度感応0.3。「五穀」には黍に代えて稗を含める異説あり。 |
| `AMJC_Plant_Buckwheat_Soba` | **蕎麦（そば）。** 古代には凶作に備える作物として栽培を勧められ、山間部や痩せた土地でも育てられてきた。AMJGrainsでは短期栽培に向くが霜には弱く、収穫後に脱穀・殻取りを行う。古い蕎麦の食べ方と、江戸期に広まる麺状の蕎麦切りは区別する。 | 4日／収量8／最低肥沃度0.4。農水省の救荒作物・蕎麦切り史。 |
| `AMJC_Plant_Barley` | **大麦（おおむぎ）。** 弥生時代に日本へ伝わり、奈良時代にも栽培が確認される麦類。米とともに食生活を支えてきた。AMJGrainsでは冷涼な気温での栽培に向き、雑穀より生育期間は長いが収量が多い。収穫後は脱穀と殻取りを行う。 | 10日／収量22／成長最低0℃。農水省「特集1 麦(1)」。 |
| `Plant_Rice` | **稲（いね）。**日本列島では弥生時代に水田稲作が広まった。水田で育てる水稲（すいとう）と畑で育てる陸稲（おかぼ・りくとう）は同じイネだが、栽培環境に適した品種に違いがある。\n\nAMJGrainsでは陸稲と水稲を分けず、共通の「稲」として扱う。温暖な気温と一定以上の肥沃度を必要とし、収穫後は脱穀と籾摺りを経て米になる。 | Vanilla `Plant_Rice` Patch：5日／収量11／最低肥沃度0.7／成長10～42℃。陸稲の古代の普及割合は確定していない。水田栽培はV1未実装。 |

### 根拠・留保

- 粟・稗の弥生期遺跡：https://www.yoshinogari.jp/ym/episode04/foods01.html
- 稗を日常食とした宮崎県北部の事例：https://www.maff.go.jp/j/keikaku/syokubunka/k_ryouri/search_menu/menu/hie_zushii_miyazaki.html
- 黍と稗をめぐる「五穀」の異説：https://www.maff.go.jp/j/keikaku/syokubunka/traditional-foods/bunrui/mamerui-kakou.html
- 蕎麦の救荒作物としての歴史と蕎麦切り：https://www.maff.go.jp/j/pr/aff/1811/characterinformation.html
- 大麦の日本への伝来と奈良時代の栽培：https://www.maff.go.jp/j/pr/aff/1602/spe1_01.html
- 陸稲・水稲の歴史と1955年頃以降の陸稲減少：https://www.maff.go.jp/j/syouan/keikaku/soukatu/okome_summary/07/environ_ment_03.html
- 米・絹・布が銭貨の代わりに用いられた11～12世紀の商品貨幣（日本銀行貨幣博物館『日本貨幣史』）：https://www.imes.boj.or.jp/cm/history/content/index.html
- 米・布・綿などの現物貨幣には地域差があり、食用だけでない経済的役割があった（東京大学文学部・三上喜孝『日本古代貨幣流通史の研究』）：https://www.l.u-tokyo.ac.jp/postgraduate/database/2001/235.html
- 江戸の都市部に白米食が広がる一方、農村部で麦・雑穀等を混ぜた「かて飯」を常食した経緯（農林水産省「脚気の発生」）：https://www.maff.go.jp/j/meiji150/eiyo/01.html
- 江戸患いの原因：白米の精米でビタミンB1が減り、副食の少ない食事では脚気を招く（農林水産省「脚気の発生」）：https://www.maff.go.jp/j/meiji150/eiyo/01.html
- 高木兼寛の海軍兵食調査・龍驤／筑波の航海比較、後の戦時脚気被害の陸海軍差（農林水産省「脚気撲滅への挑戦」「栄養の重要性を発見」）：https://www.maff.go.jp/j/meiji150/eiyo/02.html 、 https://www.maff.go.jp/j/syokuiku/wpaper/h29/h29_h/book/part2/chap6/b2_c6_6_00.html
- 1884年の筑波航海と1885年の麦飯支給、陸海軍の医学制度・意思決定がもたらした差（内閣府経済社会総合研究所『経済分析』第203号、松井・村上2021）：https://www.esri.cao.go.jp/jp/esri/archive/bun/bun203/bun203i.pdf
- 1910年の鈴木梅太郎による米ぬか由来オリザニンの発見（理化学研究所）：https://www.riken.jp/press/2015/20150731_3/
- 鈴木梅太郎の研究業績、当初アベリ酸、後にオリザニンという命名（国立国会図書館）：https://www.ndl.go.jp/portrait/datas/6356/

**年代・因果の留保（2026-10-13）：** 高木の遠洋航海比較は1884年の明治期であり、日清・日露戦争中に実施した研究ではない。海軍の予防成果はビタミンB1の物質的同定より前で、高木自身は当時B1を特定していない。脚気は白米そのものの毒性ではなく、精米によるB1減少と、副食も含めた栄養不足が問題。鈴木の1910年の米ぬか研究は後にビタミンB1に結び付くオリザニンの発見であり、「純品のB1構造を1910年に決定した」とは書かない。ゲームの稲に医療効果や病気の仕組みを新設した意味でもない。
- 農村の米飯はかつてハレの日が中心で、白米への憧れは戦後の経済成長まで続いた（農林水産省「和食としての米」、内閣府広報「米が育んだ日本の歴史と文化」）：https://www.maff.go.jp/j/syouan/keikaku/soukatu/okome_summary/07/environ_ment_03.html 、 https://www.gov-online.go.jp/eng/publicity/book/hlj/html/202011/202011_01_jp.html
- 編集上の留保：庶民が戦後になって初めて白米を食べたとは言わない。江戸など都市部では戦前に白米食が存在し、地域・階層で普及時期に差がある。「古代の米は食料より貨幣」という量的な優劣は断定せず、食料に加え税・年貢・商品貨幣として重要だったと記述する。
- 弥生期の水田稲作の広がり（国立歴史民俗博物館「第1展示室」）：https://www.rekihaku.ac.jp/exhibitions/room1/
- 中世西日本と東国の年貢品目の違い（農林水産省・関東農政局「武蔵七党の活躍」）：https://www.maff.go.jp/kanto/nouson/sekkei/kokuei/kanna/rekishi/02.html
- 1185年の史料にみる荘園の年貢米（国立歴史民俗博物館「元暦二年七月日某注進状」）：https://khirin.rekihaku.ac.jp/pid/nmjh_collection/H-73-31.html
- 畑の陸稲と水田の水稲の定義（農研機構）：https://www.naro.go.jp/laboratory/tarc/rice_faq/growing/025127.html
- 栽培環境に適したイネの品種差（農研機構）：https://www.naro.go.jp/laboratory/tarc/rice_faq/kind/025069.html
- 陸稲と水稲の品種・形態的な差（茨城県農業総合センター研究報告第17号）：https://www.pref.ibaraki.jp/nourinsuisan/nosose/cont/img/seikouken_kennkyu_17.pdf

**分類上の補足（2026-10-13）：** 陸稲と水稲は生物学的に別種ではなく、栽培形態や環境適応に差のある稲の品種群。ゲーム内説明では両者の実際の違いとAMJの抽象化のみを記し、V1などの版数、機能の未実装、別Modの担当は説明しない。現行V1の畑作限定と水田の開発予定は設計書で管理する。

**史実・ゲーム値の線引き:** ゲームで設定した最低生育温度を史実の絶対的な耐霜性とは解釈しません。地域の郷土食の記録を古代から全国一律の慣習とすることも避けます。アワ・ヒエ等の表示名変更は本節では決めず現行labelを維持します。ただし稲の表示名は2026-10-13の作者指定によりV1から「稲」へ変更します。追加機能（稲の田植えや酒造等）も予告しません。


## 8. English descriptions translated from §7 approved Japanese (2026-10-08)

These six English strings follow the Japanese §7 descriptions; only Plant_Rice was revised under the 2026-10-13 author instruction. The five shared plants use `Defs/ThingDefs_Plants/Plants_StageA.xml`; the reused Vanilla `Plant_Rice` is patched by `Patches/UplandRice.xml`. Earlier §2/§6 approved food, milling, and non-MO wheat descriptions remain unchanged.

| DefName | English description |
|---|---|
| `AMJC_Plant_FoxtailMillet_Awa` | Foxtail millet (awa). A dry-field millet found at archaeological sites from the Yayoi period, and one of the grain foods eaten alongside rice. In AMJGrains it takes longer to grow than kibi but yields more grain per harvest. Harvested grain must be threshed and dehulled. |
| `AMJC_Plant_BarnyardMillet_Hie` | Japanese barnyard millet (hie). A grain cultivated since ancient times and eaten more routinely than rice in some regions. Records from mountain communities also describe its use as an everyday staple. In AMJGrains it grows at lower temperatures than awa or kibi, but this does not guarantee greater resistance to frost. Harvested grain must be threshed and dehulled. |
| `AMJC_Plant_ProsoMillet_Kibi` | Proso millet (kibi). A millet sometimes counted among the traditional five grains, together with rice, mugi and awa. In AMJGrains it matures sooner than awa or hie and is relatively less affected by soil fertility, but yields less grain per harvest. It must be threshed and dehulled after harvest. |
| `AMJC_Plant_Buckwheat_Soba` | Buckwheat (soba). In ancient Japan it was recommended as a crop to guard against poor harvests, and it has been grown in mountain regions and on less fertile land. In AMJGrains it grows quickly but is vulnerable to frost, and requires threshing and dehulling. Older ways of eating buckwheat are distinct from cut soba noodles, which became widespread in the Edo period. |
| `AMJC_Plant_Barley` | Barley (omugi). A cereal introduced to Japan in the Yayoi period and known to have been cultivated during the Nara period. It has long contributed to diets alongside rice. In AMJGrains it grows well in cooler conditions, takes longer to mature than the millets, and yields a larger harvest. Harvested grain must be threshed and dehulled. |
| `Plant_Rice` | Rice (ine). Paddy-field rice cultivation spread across the Japanese archipelago during the Yayoi period. Paddy rice (suito) is grown in flooded fields, whereas upland rice (okabo, rikuto) grows in dry fields. They belong to the same rice species, though varieties adapted to different growing conditions differ.\n\nAMJGrains represents upland and paddy rice as a single rice crop. It requires warm temperatures and sufficiently fertile soil. After harvest, threshing and hulling yield rice. |

**Static verification:** `Tests/test_grains_localization.py` checks approved Japanese/English text against loaded-source XML/DefInjected and verifies the `Plant_Rice` description Patch. Game-loaded language switching, UI readability and four-profile ERROR 0 are still pending.


## 9. MO履歴Fixtureとの整合（2026-10-08）

旧MO必須Coreの履歴Fixtureには、今回承認した5つの穀物PlantDefの旧英語descriptionもハッシュ計算の対象として含まれている。履歴Fixture自体は変更しない。 `Tests/validate_grains_base.py` は、現行の説明文を本書§8の承認済み英文と完全一致で検証してから、ハッシュ比較用のコピーに限って旧説明文へ戻す。これにより、説明文だけの承認済み変更を認めながら、作物数値・研究・製粉・画像参照など既存契約の検証は維持する。 Vanillaの陸稲 `Plant_Rice` は旧AMJC履歴ハッシュ対象外のため、翻訳テストと `Patches/UplandRice.xml` の照合で確認する。

`Tests/test_grains_base.py` には実装英文と§8の改変をそれぞれ検出する負例を追加した。実機での翻訳ロード・表示確認は未実行。

## 10. 陸稲の新加工語彙（2026-10-08）

承認済みの稲束／籾／脱穀／籾摺りを基礎とし、2026-10-13の工程別解説方針により、栽培・陸稲／水稲の区別は§7/8、古代中世の租税・年貢・貨幣は稲束、籾摺り・玄米・白米・ビタミンB1の違いは籾、江戸患い・白米食・高木兼寛・鈴木梅太郎は食用米 `RawRice` に分ける。英語は日本語の意味に対応させ、改行はRimWorldの `\n\n` を使用する。`RawRice`のDefName／ゲーム値は維持し、説明のみ英語Patchと日本語DefInjectedへ追加する。ゲーム内文言にはV1・未実装機能・別Modの担当を記さない。

**収穫物**

- `AMJC_RiceSheaf`：**稲束**。収穫した稲を束ねたもの。穂にはまだ籾が付いており、食料にするには脱穀と籾摺りが必要となる。\n\n古代から中世の米は食料だけでなく、租税や荘園の年貢として納められ、平安時代後期には絹や布とともに貨幣の役割も果たした。 English: **rice sheaf** — Harvested rice stalks and ears tied into sheaves. Threshing and hulling are needed before this rice can be eaten.\n\nIn ancient and medieval Japan, rice was not only food: it was paid as taxes and estate rent. In the late Heian period it also served alongside silk and cloth as a form of commodity money.
- `AMJC_RiceInHull`：**籾**。稲を脱穀して得られる、籾殻に包まれた米粒。籾殻を取り除くことで食用の米になる。\n\n籾摺りで得られる玄米には糠層や胚芽が残り、さらに精白すると白米になる。精白ではビタミンB1を含む米ぬかや胚芽が減る。 English: **rice in hull** — Rice grains after threshing, still enclosed in their husks. Remove the husks to obtain edible rice.\n\nHulling yields brown rice with its bran and germ intact; further polishing yields white rice. Polishing removes much of the vitamin B1 in the bran and germ.

**食用米**

- `RawRice`：**米**。米（こめ）。籾殻を除いた食用米。AMJGrainsでは玄米と白米を別の食材に分けず、料理や製粉に利用する。\n\n江戸の都市では白米食が広まったが、副食の乏しい白米中心の食事から脚気が流行し、「江戸患い」と呼ばれた。農村では麦や雑穀を混ぜた飯が多く、白米を十分に食べられる暮らしが広く実現したのは戦後の経済成長を経てからだった。\n\n明治時代、海軍軍医の高木兼寛は軍艦「龍驤」と「筑波」の航海を比較し、食事改善で脚気が減ることを示した。原因物質が未解明の段階で食事と疾病の関係を検証した、疫学・栄養学上の成果だった。後の日清・日露戦争では陸軍と海軍の脚気被害に大きな差があった。\n\n1910年、鈴木梅太郎は米ぬかから脚気予防に有効な「オリザニン」（後のビタミンB1）を発見した。 English: **rice** — Rice (kome). Edible rice after the husks have been removed. AMJGrains uses one ingredient for both brown and white rice in cooking and milling.\n\nWhite rice became popular in Edo's urban diets, but white-rice-heavy diets with too few side dishes led to beriberi, called "Edo wazurai" (Edo sickness). Rural households often mixed rice with barley or other grains. Eating plentiful white rice became widely attainable only after postwar economic growth.\n\nIn the Meiji period, naval physician Kanehiro Takaki compared the voyages of the warships Ryūjō and Tsukuba and showed that improved diets reduced beriberi. This was an important epidemiological and nutritional finding before the cause was identified. The Army and Navy later suffered greatly different levels of beriberi during the Sino-Japanese and Russo-Japanese wars.\n\nIn 1910, Umetaro Suzuki discovered oryzanin, a beriberi-preventing substance from rice bran later linked to vitamin B1.

**加工Recipe**

- `AMJC_ThreshRice`：稲を脱穀する。収穫した稲束から籾を分離する。English: **thresh rice** — Separate rice grains enclosed in their husks from the harvested sheaves.
- `AMJC_ThreshRiceBulk`：稲を脱穀する x10。稲束10個をまとめて脱穀し、籾10個にする。English: **thresh rice x10** — Thresh ten rice sheaves to obtain ten portions of rice in husk.
- `AMJC_HullRice`：籾摺りをする。籾から籾殻を取り除き、食用の米に加工する。English: **hull rice** — Remove the husks from threshed rice to obtain edible Vanilla rice.
- `AMJC_HullRiceBulk`：籾摺りをする x10。籾10個をまとめて籾摺りし、食用の米10個にする。English: **hull rice x10** — Remove the husks from ten portions of threshed rice to obtain edible Vanilla rice.

**加工中表示（日本語 / English）**

- `AMJC_ThreshRice.jobString`：稲を脱穀している。 / Threshing rice.
- `AMJC_ThreshRiceBulk.jobString`：稲をまとめて脱穀している。 / Threshing rice in bulk.
- `AMJC_HullRice.jobString`：籾摺りをしている。 / Hulling rice.
- `AMJC_HullRiceBulk.jobString`：籾をまとめて籾摺りしている。 / Hulling rice in bulk.


## 11. 粉食心情の実機エラー修正（2026-10-08）

`AMJC_AteFlourFood` の心情付与は+2（0.5日、重複1回）のまま維持する。実機ログでStageの説明が空であるため、英語 `I ate a dish made from ground grain.`、日本語ラベル「粉食を食べた」、日本語説明「挽いた穀物から作った料理を食べた。」を追加。歴史的な新説や未実装の調理法を主張せず、実装済みの粉食心情だけを説明する。


## 12. 大麦粉・米粉の追加食用途候補（2026-10-13、実装前の史料調査記録）

本節は**候補比較を行った時点の履歴**である。後続の§13に記録されたとおり、大麦粉・米粉と麦粉粥・米粉団子は**材料・方法を表す機能的総称としてソース実装済み**。以下の「未承認・不採用」はその時点の**史実上の固有料理名としての採否**を指し、現在のThingDef・RecipeDefの実装状況ではない。§2／§6／§7／§8／§10の承認済み日本語と英語は維持する。

| 候補 | 史料で確認できる点 | ゲーム実装での留保 | 判断 |
|---|---|---|---|
| **米粉団子** | 農林水産省「米粉」は奈良時代から米粉を菓子に利用したと説明。「串だんご」は米の粉を丸めて茹でる団子を説明し、串団子については室町まで遡るとする | 砂糖・串・甘味タレ・後世の製品名を基本レシピに導入しない。うるち・もち米粉は既定どおり `RawRice` 由来の単一米粉で抽象化。粒を蒸して搗く餅とは別製法 | **有力候補、未承認** |
| **はったい（麦こがし系）** | 大麦食品資料が『和名類聚抄』の米麦乾燥・焙煎・製粉して湯水に溶く記述を紹介。現代の「はったい粉」は煎った大麦を挽いたもの | **焙煎は必須の製法差。** 単に生の `AMJC_Barley` → `AMJC_BarleyFlour` を石臼で挽いた工程だけで、生成物を「はったい粉」と呼ぶのは不正確。乾煎りを料理工程側で表現する案などを比較する必要がある | **条件付き候補、未承認** |
| **一般の粉食・麦粉粥** | 日本の大麦に粥食の歴史はあるが、確認した説明は主として挽き割り大麦を粥にする方法 | 挽き割り大麦の粥を、細挽きの大麦粉の料理と無条件に同一視できない | **典拠不足、現時点では不採用** |

資料：
- 農林水産省「米粉」 https://www.maff.go.jp/j/keikaku/syokubunka/traditional-foods/menu/komeko.html
- 農林水産省「串だんご」 https://www.maff.go.jp/j/keikaku/syokubunka/k_ryouri/search_menu/menu/34_29_tokyo.html
- 農林水産省「米粉の利用」 https://www.maff.go.jp/j/syouan/keikaku/soukatu/okome_summary/08/rice_flour_charac_teristic_04.html
- はくばく「知られざる大麦の歴史」 https://www.hakubaku.co.jp/omugi-lab/hyakka/oomugi-history/
- 日本粉体工業技術協会「はったい粉」 https://appie.or.jp/column/%E3%81%AF%E3%81%A3%E3%81%9F%E3%81%84%E7%B2%89/

既存Mod比較（2026-10-13、限定調査）：VEの `VanillaCookingExpanded` 1.6 `Items_RawPlant.xml` は `VCE_Flour` という小麦粉を所有し、`Recipes_Bakes.xml` の製菓Recipeはそれを明示指定。VE `VanillaPlantsExpanded-MorePlants` 1.6では複数の別穀類栽培があるが、そのことだけでAMJの大麦粉・米粉の供給を置換できない。非VEではMO 1.6の `DankPyon_Flour` が小麦粉として既にGrainsの条件付き供給元。さらに Progression: Agriculture の公開説明ではMO・VCE双方の粉を統合し、VPE More Plantsの大麦等を一般穀物へ接続する互換がある。ただし同Modの1.6実ソースは今回未取得・未検証のため、全Modとの最終互換を証明しない。

**残ゲート:** 追加2種の粉を単に作れる状態で完了扱いしない。工程・用途・画像パス・ローカライズ、VE・非VEの具体的な競合Patch、Vanilla/MO×CCTO四構成での実ゲームBills／ERROR 0を揃える。料理候補の正式採否と歴史説明文の承認は別途必要であり、未承認の本番文字列を作らない。


## 13. 五粉への機能追加（2026-10-13、史実校閲・実機未完）

§12の調査を踏まえ、米粉を練って加熱する米粉団子と、大麦粉を湯で煮る汎用的な麦粉粥を追加。特定の中世郷土料理名が史料で確認されたという意味ではなく、材料と基本操作を表すゲーム内の説明名。「はったい」は麦を**先に炒ってから挽く**製法のため、先に通常製粉する麦粉粥を「はったい」とは呼ばない。史実・表示名称の最終校閲は未了。

- `AMJC_BarleyFlour` — 大麦粉、`AMJC_Barley` から10→10。既存の `BarleyFlour_a` を参照
- `AMJC_RiceFlour` — 米粉、Vanilla `RawRice` から10→10。うるち・もち米を単一粉で抽象化し、既存の `RiceFlour_a` を参照
- `AMJC_MillBarley` / `AMJC_MillRice` — Base手動石臼／MO既存石臼に条件付きで割当
- `AMJC_BarleyFlourGruel` — 麦粉粥、既存大麦の穀粒画像を一時流用
- `AMJC_RiceFlourDumplings` — 米粉団子、既存米の穀粒画像を一時流用
- `AMJC_CookBarleyFlourGruel` / `AMJC_CookRiceFlourDumplings` — 粉Nutrition 0.5相当から料理1個へ

この8件の日本語・英語は現在の機能操作を説明するものであり、未承認の特定時代・地域の由緒を追加しない。元の§2/§6/§7/§8/§10承認済み文言と料理3品は変更しない。画像制作・RimWorld実機での四プロファイル実ジョブPASSは別ゲートのまま。

## 14. V1史実・日英の静的校閲（2026-10-13）

**対象:** 既存の稲／稲束／籾／食用米の承認済み段階別本文、追加された大麦粉・米粉と麦粉粥・米粉団子の本番ThingDef/RecipeDefの機能説明。承認済みの日本語と英語、およびDefName・Recipe・数値は今回変更しない。

- **米粉団子:** `AMJC_RiceFlourDumplings` は米粉を水と練って加熱する一般的な調理法の**ゲーム上の総称**。農林水産省「米粉」（https://www.maff.go.jp/j/keikaku/syokubunka/traditional-foods/menu/komeko.html）および「米粉の種類と用途」（https://www.maff.go.jp/tokai/seisan/shinko/komeko/type.html）は米粉と団子の利用を記す。しかし、特定のAMJ Recipeが中世日本において単一の正式料理名として確立していたことまでは証明しない。
- **うるち・もちの抽象化:** `AMJC_RiceFlour` は一つのアイテムとして両原料の粉を代表するが、澱粉特性や用途は実際には異なる。農水省「米粉の種類と特性」（https://www.maff.go.jp/j/syouan/keikaku/soukatu/okome_summary/08/rice_flour_charac_teristic_01.html）参照。白玉粉など個別の製法、搗き餅、菓子の時代差は本Recipeに付与しない。
- **麦粉粥:** `AMJC_BarleyFlourGruel` は大麦粉を湯で煮る**材料・調理法に基づく一般名**。古代・中世日本の全地域で同名の定型料理があったと断定しない。挽き割り大麦の粥を細挽き大麦粉の粥と同一視しない。麦こがし／はったい粉は穀粒を**先に焙煎してから製粉**するため、通常の製粉から煮る現在のRecipeとは異なる（https://appie.or.jp/column/%E3%81%AF%E3%81%A3%E3%81%9F%E3%81%84%E7%B2%89/）。
- **史実上の未承認箇所:** 追加2食品の歴史的固有名称・時代への結び付けの**作者による日本語最終校閲は未了**。今回の監査は既存の材料・方法説明の範囲を確認しただけで、新しい日本語史実叙述を採用・英訳・公開したものではない。
- **表示確認の延期:** 稲 `Plant_Rice`、`AMJC_RiceSheaf`、`AMJC_RiceInHull`、Vanilla `RawRice` の日英4組と2/2/2/4段落、他の食品のUI表示、パッチ競合・料理Billは `Docs/GrainsProfileTesting.md` の一括実機検証へ含める。CIの文字列整合は実表示の代替ではない。
