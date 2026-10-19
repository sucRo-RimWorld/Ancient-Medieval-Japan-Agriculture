# Ancient & Medieval Japan - Grains（中世日本 - 穀類） — 設計書

**文書状態:** Core→Grains再編実装中。MO必須依存解除（2026-10-10）。新規開始の実機・画像・UI・バランス検証を継続。旧セーブ移行は対象外  
**対象:** RimWorld 1.6  
**公開表示名（名称方針）:** Ancient & Medieval Japan - Grains（中世日本 - 穀類）  
**機能位置づけ:** 独立した穀類Mod。表示名はGrains、既存`packageId`・DefNameは互換維持。MO必須依存は解除、MO互換は条件付きで継続  
**2026-10-19 作者最新方針 — 米粉の新規生産を廃止:** 中世日本の庶民の通常食として玄米粉／米粉を必須工程にする根拠は不十分。玄米・白米、米飯・白飯・米粥・白粥は維持する。一方 `AMJC_MillRice`、`AMJC_CookRiceFlourDumplings`、同BulkのBillを設備から外し、共通粉食の材料から米粉を除外する。旧ThingDefとRecipeDefのDefName・原画像は参照保護のため残すが、現行料理数・製粉種類・新規生産品に数えない。本文中の2026-10-13の五粉・五品に関する履歴表現は本決定に劣後する。

**基本方針:** GrainsはAMJ全体の必須Coreではなく、乾田穀物の栽培・一次加工・最低限の粉食を自己完結して提供する。Vanilla + Grainsを基本経路とし、Medieval Overhaul（MO）は任意の公式互換先。MOありではMO固有の小麦・石臼等を条件付きで再利用し、Grains所有の設定を優先する。旧セーブからのMO削除・移行は、導入実績と対象セーブがないため今回の対象外。

---

## 1. 目的

現Coreを再編したGrainsは、RimWorldの乾田農業に、**土地・気温・生育期間によって主食穀物を植え分ける判断**を追加する独立コンテンツModとする。

中心領域:
- アワ・ヒエ・キビ・ソバ・大麦・小麦の乾田栽培
- 穀束、脱穀、殻取り等の一次加工
- 雑穀粉・蕎麦粉・大麦粉・小麦粉の4種を対象とする製粉経路。米粉は2026-10-19の判断で通常の生産対象から外し、既存アイテム・料理・BillのDefNameだけ互換維持する。
- 粉を無用途にしないための研究不要・低設備の最低限粉食
- 作物ごとの肥沃度・温度・生育日数・収量・加工価値の差
- MO導入時の小麦・小麦粉・石臼・Straw等の公式互換

Grainsは、水田・給排水・水管理、豆類、繊維、根菜、一般採集、発酵、酒造、保存、建築等を「共通基盤だから」という理由では所有しない。畑地・水田に共通する稲Plantと稲束・籾・米の加工はGrainsが所有する。これらは実際の主要ゲームループに最も自然なModへ分け、Grainsとは必要時だけ条件付き互換する。

最終的な対象時代は新石器相当から戦国末期までとするが、Grains自体はその全生活文化を網羅しない。

**Grains V1の段階的実装方針（2026-10-13 作者確定）:** V1は専用作業台の完成より、Vanilla単独／MO併用で「栽培→収穫→脱穀→殻取り（小麦は不要）→製粉→料理」の基本ループを成立させ、通常プレイで検証可能にすることを優先する。

既存設備・調理台を用いた暫定加工はV1で許容する。当初想定した工程別の専用作業台への再振り分け、その設備の完成画像や設備固有のバランス調整はV1以降に段階的に実施する。

V1の加工工程・品目・材料・歩留まり・研究条件・MO互換・既存DefNameを安易に省略・変更しない。実ゲームのBill実行、画像読込、ERROR 0等の検証と公開判断は継続する。

「V1」はGrainsの機能段階であり、RimWorld 1.6/1.7対応やModの具体的なsemverを指定しない。具体的な設備境界は§5.1–5.2と下記ロードマップで管理する。

### 1.1 名称・サブタイトル方針

シリーズ英語名 **Ancient & Medieval Japan（AMJ）** は維持し、公開日本語シリーズ名は **中世日本** とする。表示名は **Ancient & Medieval Japan - Grains（中世日本 - 穀類）** に統一する。ここでの日本語名は公開上の略称であり、設計書中の古代から中世にわたる史実上の対象範囲を変更しない。

一方、英語圏では `Medieval Japan` だけだと、侍・城・合戦・戦国武将などの華やかな軍事文化を主題とするModと受け取られやすいため、公開時のサブタイトル・短い紹介文では**江戸以前の一般の人々の生活が主題であること**を明示する。

方針:
- **`pre-Edo Japan`** を、時代感を直感的に伝える有力な表現として使用する
- `rural life`、`village life`、`ordinary people`、`farming and survival` 等を組み合わせ、生活・生産側のModであることを示す
- `samurai warfare` や `Sengoku warfare` を主題と誤解させる表現は避ける
- 固定サブタイトルは公開準備時に決定する。現段階では **“pre-Edo rural/village life”** を中核イメージとする
- 日本語でも「戦国の華やかさ」より、**古代～中世の一般の人々が新しい土地で暮らしを成立させる**ことを前面に出す

---

## 2. 設計思想

### 2.1 農村を基準にする

都市・寺院・上流階級の食文化ではなく、RimWorldの小規模コロニーに近い、

- 農村
- 山村
- 湿地・河川沿いの集落
- 小規模な自給生活

を基準とする。

「京都や江戸で食べられたもの」ではなく、**地方集落が自力で確保・加工できたもの**を優先する。

AMJが主役として想定するのは、武将・大名・上流階級ではなく、**農民・職人・猟師・漁師・流民・下級武士等を含む一般の人々**である。戦国末期までを対象に含むが、「戦国の華やかさ」や武将文化を中心テーマにはしない。

RimWorld / MOのコロニーは、様々な事情で既存共同体を離れた人々が新しい土地で小規模な共同体を作る遊びとして捉える。AMJも、
- 何を植えるか
- どこに水田を作るか
- 冬までに何を蓄えるか
- 木・土・藁・紙等をどう使うか
- 食料をどう加工・保存するか
- 限られた人手をどこへ割くか

といった**生活の成立そのもの**を中心に置く。

米、とくに精製された白米は「最初から当たり前に食べる高性能主食」ではなく、雑穀・麦・芋等で日常を支えながら、土地・水・労働力・加工設備へ余裕ができた時に安定供給できる、**共同体の豊かさを示す食料**として位置づける。

### 2.2 Coreでは各資源に主用途を1つ与える

史実上ひとつの作物・資源に複数用途があっても、Core本体では原則として**主用途を1つだけ実装する**。

目的:
- Coreの役割を「農作物・原材料の基盤」に限定する
- 1つの資源から料理・薬・油・繊維などへ機能が枝分かれし、Coreが肥大化するのを防ぐ
- 中間素材や専用加工設備を、用途のない段階で先行実装しない
- 後続Addonが同じCore資源を拡張しやすくする

例:
- 荏胡麻 → Core採用は保留。採用する場合も種実食材までとし、搾油はCore外
- 葛 → Coreでは葛根として薬草
- 大豆 → Coreでは豆類食材
- 米 → Grainsが共通の稲Plant・穀物加工を所有し、Rice Cultivationが同じ稲の水田栽培を拡張する
- 栗 → Coreでは食料資源（木そのものは通常の樹木として木材利用可能）

油・発酵・料理・繊維加工・酒造などの追加用途は、それぞれ対応するAddon側で実装する。特に搾油はCoreの進行要件から外し、油を実際に利用する機能側で必要になった時点まで後回しにする。

搾油は、それ単独で十分な規模や独立したゲームループを持たない限り、専用Addonを作らない。
- 食用油としての用途が主なら、**その油を実際に使う保存・発酵・調理等の機能側**で用途とセットで追加する
- 灯火・塗装・生活素材としての用途が主なら Materials / Architecture系Addon
- 複数用途が増え、独立した設備・研究・加工体系が必要になった場合のみ専用Addon化を再検討する

**史実上の多用途性そのものは捨てず、Mod群全体で段階的に解放する。**

### 2.3 史実に根拠を置いたゲーム的抽象化

Coreは古代～中世日本を厳密に再現する歴史シミュレーターではなく、**史実に根拠を置きつつ、RimWorld上で意味のある選択を残すゲーム的抽象化**を行う。

実装判断では「史実に存在したか」だけでなく、**ゲーム内で別の判断を生むか**を重視する。さらに、その工程・設備・資源管理が**UXとして実際に楽しいか、判断や工夫につながるか**を最優先の採用基準とする。史実的に正確でも、単なるクリック数・待ち時間・在庫分裂・作業キュー増加にしかならない要素は省略・統合・自動化してよい。

削ってよい例:

- 実際には複数段階ある加工工程を、ゲーム上意味が薄い場合は1工程にまとめる
- 地域差・時代差の細部を、代表的な仕様へ整理する
- 同じ役割の作物・野草・道具をすべて実装しない
- 歴史上存在していても、既存要素とゲーム上ほぼ同じ役割なら省略または互換で済ませる
- 季節差・栽培差・保存差を、成長速度・耐寒性・保存期間など少数のパラメータへ抽象化する

残すべき例:

- 作物ごとの寒冷・乾燥・痩せ地への適性差
- 水田整備・水管理による米の生産性改善
- 大豆などの多用途性（荏胡麻・油料作物はCore採用自体を保留し、必要な用途が成立した段階で再検討）
- 日本側で早期利用できる繊維資源
- 保存・加工に必要な手間
- MOの西欧基準と日本史の時代感がずれる部分

判断基準は、**それを削ることで古代～中世日本らしい選択が失われるか**とする。

### 2.4 歴史的に非必須な技術は「使うと得」にする

史実上、常に必須ではなかった工程・設備・技術は、ゲーム上でも原則として必須化しない。

代わりに、

- 作業時間短縮
- 収量改善
- 保存性改善
- 天候への安定性向上
- 失敗率低下
- 副産物回収効率向上

などの利点を与え、**使えば有利だが、使わなくても生活は成立する**設計を優先する。

稲作ではこの原則を次のように扱う。

- **共通の必須加工**は `稲束 → 籾 → 米` とする。Grainsは共通稲Plantの収穫先を未脱穀の稲束とし、既存の穀物加工設備で脱穀・籾摺りを提供する。最終食材はVanilla `RawRice` を再利用する
- 「籾」は独立した中間物。**籾摺りBillで玄米（Vanilla `RawRice`）または白米（`AMJC_WhiteRice`）を選択**する。玄米は籾10→10・作業量80、白米は籾10→8・作業量240とし、後者に明確な労力差・歩留まり損失を設ける。**Grains現行XMLは稲束を収穫し、2段階のRecipeでRawRiceを得る。実機検証は未完了**
- 稲架掛け、乾燥した稲、乾燥の歩留まり差等は**Rice Cultivation側の水田栽培向け任意追加工程**とする。MO Drying Rack／Processor Frameworkを必須依存とはしない。通常の収穫と必須加工を経るだけで食事に利用でき、乾燥工程は任意
- MO Strawは条件付きの副産物として検討するが、BaseにMO Defを参照させない。稲藁の発生箇所と任意乾燥での二重発生を防ぐ

**共通の稲Plant・稲束・籾・RawRiceの加工経路と畑地での基準栽培値はGrainsが所有する。水田環境による生育・収量補正と水管理、稲架掛け等の拡張はRice Cultivationが決める。** 実装候補とゲームテストの関係は [RicePostHarvestProcessing.md](Balance/Crops/RicePostHarvestProcessing.md) を正本とする。

この原則は、史実上地域差・時代差が大きい設備や加工方法にも適用する。

### 2.5 料理バフの設計基準

Core本体では完成料理を大量追加しない。この項目は、**各Addonで必要に応じて追加する専用料理・既存料理Mod互換・将来の料理機能**に適用する。

**独立した Cuisine / Cooking Addon は現時点では計画しない。** 料理名・レシピ数を増やすこと自体にはゲーム上の価値を置かず、同じ性能・同じ材料役割の料理を多数追加して調理Billやメニューの一覧性を悪化させない。新しい料理Recipeは、材料選択・保存性・加工工程・季節性・設備・効果等に**他の料理と区別できるゲーム上の意味がある場合だけ**追加する。

料理の表現は、個別の歴史料理を大量に並べるより、**性能と投入コストを基準に少数の抽象料理へまとめる**ことを優先する。代表例として、上位加工品カテゴリの食材を一定数要求して「豪華な和食」を作り、Vanilla/MOの豪華な食事相当の食事価値にMoodと短時間の健康系バフを加える、といった粗い抽象化を許容する。現時点の目安は**上位カテゴリ加工品4個 → 豪華な和食**だが、必要個数・栄養・Mood・Hediff値は実装時のバランス調整で確定する。

一方で、MOの目玉焼き・狩人のスープ・カボチャのフリットのように、**特定材料を要求すること自体がゲーム上の選択になる料理**は個別Recipeとして厳選してよい。個別料理を追加する基準は、専用材料の確保・材料構成・解禁Tier・保存性・効果・季節性等によって、他の料理と明確に異なる生産判断を生むこととする。単なる料理名違い・見た目違い・史実上の別名だけでは追加しない。個別料理名は原則として説明文やフレーバーへ寄せ、**材料指定そのものに意味がある少数の例外だけRecipe化する。**

個別Recipeの有力候補は、**祝い・祭礼・宴席など日常食ではない特別な機会に作られた料理**とする。日常的に常食する料理より、希少材料・複数加工品・追加作業を要求しやすく、完成時に高いMoodや一時的な健康/能力効果を持たせる理由も作りやすい。したがって、個別料理の追加可否は「歴史的に有名か」より、**特別な場面のために備蓄・加工・調理するというゲーム上の目的が成立するか**を優先して判断する。

料理の一時バフは、**Medieval Overhaul（MO）の料理バランスを基準に設計する**。

基本原則:
- 料理Tierだけで、バフの有無や強さを決めない
- 低Tier料理でも、材料・工程・供給条件に相応のコストがあるなら一時バフを付けてよい
- 高Tier料理でも、研究Tierだけを理由に強いバフを付けない
- 材料側の研究Tier・栽培難度・入手性も料理価値に含める
- 保存性、Mood、食事カテゴリ、栄養、作業量も料理価値の一部として扱い、すべてを能力バフで表現しない
- バフは原則として一時的なHediff・能力補正として実装し、恒常的な能力上昇や恒常的な健康改善は付けない

バフ設計で評価する要素:
- 材料の入手難度
- 材料の供給安定性
- 卵・乳など畜産資源の要求
- 複数系統の材料要求
- 材料側の研究・栽培Tier
- 加工工程と作業量
- 専用設備の必要性
- 保存性そのものが持つ価値
- 食事カテゴリとMood
- 既存MO料理との相対バランス

MOで確認済みの参考例:
- 目玉焼き: 初級料理でも専用材料である卵を要求し、短時間の能力バフを持つ
- マッシュドポテト: 初級料理でもジャガイモに加えて乳製品を要求し、短時間の能力バフを持つ
- カボチャのフリット: 料理研究は初級だが、材料のカボチャ自体は上級農業で解禁される
- 中級料理: 「手の込んだ食事」相当の食事カテゴリ価値を持つ例がある
- 上級料理: 「豪華な食事」相当の食事カテゴリ価値を持つ例がある
- パン: 研究解禁料理でも特別な能力バフを持たない例
- 燻製肉: 保存性が主価値で、付加価値はMood +2程度に留まる例

許容する一時効果の例:
- 意識
- 運動能力
- 指の機能
- 作業速度
- 空腹率
- 消化・免疫・臓器機能などへの短時間補正
- Mood

禁止・抑制するもの:
- 永続的な能力上昇
- 永続的な健康改善
- 料理だけで重い疾病や負傷を治療する効果
- MO既存料理との比較で明らかに過剰なバフ

> **料理バフはTier基準ではなく、材料・手間・研究・保存価値を含む総コストに対して付与する。**

米は畑でも栽培可能で、水田の整備・管理は将来導入される任意の生産コストである。米専用料理の価値は**米という名称や水田必須との誤認ではなく、実際の栽培条件・期間・一次加工・調理負担に応じて**決める。同程度の料理工程でも、投入資源と労働が実際に重い場合は米系料理をやや高価値にすることを許容する。ただし「米だから無条件に強い」のではなく、栽培・加工を含む総コスト差を根拠に調整する。

また、**酒などの嗜好品は日常食より工程・研究を複雑にしてよい**。生存に必須な主食や日常料理は管理負荷を抑える一方、任意の高付加価値品は、追加精製・選別・発酵・濾過・温度管理・複数研究段階などをゲーム上の遊びとして許容する。複雑さは嗜好品としての価値・希少性・強い一時効果へつながる場合に採用し、単なる作業数の水増しにはしない。

### 2.6 日本・中国・朝鮮半島を一括りにしない

竹・紙・大豆など、共有される素材・技術はあるが、文化そのものは統合しない。

- 日本Coreは日本の生活基盤として作る
- 中国系Modは中国文化として扱う
- 朝鮮半島系Modが存在する場合も別文化として扱う
- 共通素材のみ互換カテゴリで接続する

「East Asian Culture」的な文化混同は避ける。

### 2.7 DLC非依存

CoreはDLCなしで成立させる。

- Ideology前提の宗教・禁忌は扱わない
- 肉食禁忌・精進・寺院制度はCore外
- 神祇信仰などもシステム化せず、必要ならフレーバー程度

### 2.8 依存・基準環境

AMJ各Modは、**AMJシリーズ内の別Modを理由なく必須依存にしない**。各Mod自身の主要ゲームループが成立するために不可欠な前提だけを必須依存とし、他のAMJ Modや外部Modとの接続は原則として任意互換とする。

長期的な構成目標は、中央の「Core」を全Addonが必ず通る依存木ではなく、**独立したAMJ Mod群 + 公式相互連携**である。全要素を一体で遊びたい場合はAMJ一式を導入するが、個別Modだけ、またはVanilla / MO等との一部組み合わせでも、そのMod自身のゲームループが成立する構成を優先する。

依存判断の基準:
- 「その素材・機能を使うと内容が増える」だけなら必須依存にしない
- 「その前提がないとModの主要ゲームループ自体が成立しない」場合のみ必須依存を認める
- AMJ Mod同士の追加レシピ・追加原料・追加Def接続は、両方が存在する場合だけ有効になる互換Patchを基本とする
- 任意互換が増えること自体は避ける理由にしない。RimTest Redux / Pickle / 静的検証等で主要な組み合わせを自動化し、互換マトリクスとして維持する
- 依存関係を減らすために同一概念のDefを各Modへ無秩序に複製しない。所有Modを明確にし、必要なら条件付きPatchで参照する

#### Medieval Overhaulとの関係

作者の標準プレイ環境・主要比較対象としてMOを重視する方針は維持するが、**作者がMO前提で遊ぶことと、GrainsがMOを必須依存にすることは分ける。**

Grains移行後はVanilla + Grainsで、乾田穀物の栽培 → 一次加工 → 製粉 → 最低限の粉食まで成立させる。MO導入時は同じ概念のMO小麦・小麦粉・石臼・Straw等を条件付き互換から実Def供給元として再利用し、同等のAMJ資産を二重表示しない。

Base XMLにはMO DefNameへの無条件参照を残さない。MO固有PatchはMO存在時だけ適用する。**本番About.xmlのMO必須依存は解除済み**。任意の`loadAfter`は互換Patchの適用順を守るため残す。公開可否は新規導入での実機ロード・製粉・描画等で判断する。旧セーブ移行は対象外。

`AMJ - Medieval Overhaul Japanization` はMO本体を古代～中世日本向けに再構成するPatch + Retextureレイヤーであり、責務上MO必須を維持する。World Tech Levelは中世限定世界を作るための強い推奨Modとし、Japanization自身では包括的なTech Level制限を再実装しない。

#### AMJ Mod間の依存方針

現時点の方向性:
- Grains / 現Core: Vanilla単体で乾田穀物・一次加工・製粉・最低限粉食を成立させる。MOは推奨・公式任意互換
- Fermentation: Grainsを必須依存にしない。単独の発酵ループを持たせ、Grains導入時に雑穀・共通米等を追加接続し、Rice Cultivation導入時には水田に固有の機能だけを条件付きで扱う
- Brewing: Grains / Waterworks / Rice Cultivationのいずれも自動的な必須依存にせず、存在時だけ対応原料・工程を追加接続する
- Ironmaking: Vanilla単体で鉄器加工・砂鉄供給・古代～中世製鉄を成立させる。MO / Environment / Waterworksは公式任意互換。詳細は [IronmakingDesign.md](IronmakingDesign.md)
- Waterworks / Rice Cultivation / Hot Springs / Environment / Events / Factions / Backgrounds等: 責務上必要な前提だけを持ち、Grainsを共通必須基盤にはしない。Waterworksは水利、Rice Cultivationは水田・水管理と**共通の稲Plant**に対する栽培条件の拡張を所有し、稲Plant本体・共通穀物加工・製粉はGrainsが所有する

#### Hot Springs / 温泉 Addon

GrainsはHot Springsを所有しない。専用リポジトリ作成前の詳細設計・互換方針はProjectの [HotSpringsCandidate.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/HotSpringsCandidate.md) を正本とする。Grains側には、Grainsを必須依存にしないという所有境界だけを残す。
#### 大豆の所有と外部大豆Mod

大豆はGrainsの所有外。未所属の大豆・加工候補と外部Mod比較はProjectの [DeferredPlantsAndProcessing.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/DeferredPlantsAndProcessing.md) と [ExistingModAudit.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/ExistingModAudit.md) を参照する。Grainsは将来Addon向けの大豆Defを先行所有しない。
#### Core実装識別子

実装開始時点の識別子は以下で固定する。packageIdやDefName prefixは外部Patch・セーブ・互換Modから参照されるため、公開後は原則変更しない。

- packageId: `sucro.ancientmedievaljapan.core`
- Core所有Defのprefix: `AMJC_`
- 最初のStage A栽培スライス: `AMJC_Plant_FoxtailMillet_Awa`
- アワ・ヒエ・キビ共通の未脱穀収穫物: `AMJC_RawMillet`
- CCTOは任意依存のままとし、導入時のみAMJ側互換PatchからCCTOのDefModExtensionを付与する
- AMJC固有作物の耐寒数値・設計表・候補範囲・Def対応・互換XMLはAMJCが所有し、CCTO側には置かない。CCTOはフレームワークとして利用する

#### MO併用プロファイルとしての統合

MO導入時は、既存の農業・研究・設備・素材を可能な限り再利用し、日本側で必要な差分だけAMJが所有する。これは**MOをAMJ全体の必須プラットフォームとする意味ではなく、MO併用時の公式互換設計**である。

再利用の優先順位:
1. MOに同じ概念のDef・設備・素材・研究・カテゴリがある場合は、MO併用Patchからそれを利用する
2. 概念は同じだがAMJ側の数値・研究位置が合わない場合は、MO併用時だけ既存DefをPatchで調整する
3. MOに存在しない日本固有要素、または既存Defではゲーム上の意味が異なる要素はAMJ独自Defとして所有する
4. MOなしの主要ゲームループに必要なDefを、MO側だけに所有させない。逆に、MO互換だけのためにAMJ側へ同一資産を無秩序に複製しない

技術方針:
- MO固有参照・Patchは可能な限り専用ファイル/ディレクトリへ隔離し、MOなしでは読み込まれない条件付き互換として扱う
- base側のPlantDef / ThingDef / Research / Recipe / C#からMO DefNameへの無条件参照を残さないことを、必須解除の移行条件とする
- `MayRequire`、`PatchOperationFindMod` 等は、XML構造に適した方式を選んでMO存在時だけ適用する
- MO更新時はMO互換プロファイルを自動テスト対象として維持する
- Food Drying、DBH、Edo等の他の任意Modも同じく条件付き互換として扱う

`AMJ - Medieval Overhaul Japanization` は例外で、MO本体の研究・Def・Recipe・資産を日本化すること自体が存在理由なのでMO必須を維持する。Grains等の独立AMJ ModはJapanizationを必須依存にしない。

用語:
- **Vanillaプロファイル** — RimWorld + Grains（MO依存解除後の目標）
- **MO併用プロファイル** — RimWorld + MO + Grains
- **現行公開Coreプロファイル** — 移行完了までの RimWorld + MO + AMJ Core。公開版の実装事実を示す語であり、長期依存原則ではない

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

##### Agriculture / Waterworks / Rice Cultivation境界

- Grainsは、畑作穀物に加えてVanilla `Plant_Rice` をV1では**畑地向けの共通稲Plant**として再定義し、穀物の収穫後加工・製粉までを所有する。水田・畑でPlantDefは分けず、`RawRice` はVanilla Defを再利用する。
- WaterworksはGrains非依存のまま、自然取水・開渠・暗渠・分水・温泉引湯等の**水利基盤だけ**を自己完結して所有する。
- 将来のRice Cultivationは、水田・水管理・田植え・共通 `Plant_Rice` の水田栽培条件を所有する。水稲専用Plantを増やさず、Grains併用時は同一Plantから共通の稲束・籾・米／製粉経路へ接続する。
- Waterworks + Rice Cultivation併用時だけ、水田をWaterworksの有効な水路ネットワークへ接続する公式互換を行う。GrainsはWaterworksを要求しない。
- Rice Cultivationの単独時フォールバック、稲架掛け等の水田向け任意工程、Waterworksとの接続詳細は、専用所有先ができるまではProject側で管理し、Grainsに重複設計を置かない。

##### 紙・塩・Processor Framework

- Paper / Paper PressはAgricultureの主要ループではないため、Core/Agriculture本体へVanilla代替を抱え込まない。Agricultureが靭皮繊維等を所有する場合も、製紙そのものはMO互換または将来の製紙/Materials側機能へ接続する。
- SaltもAgriculture本体の主要ループではない。既存設計の「海水採取 → `DankPyon_Salt`」はMO併用時だけ成立する互換案として扱い、BaseにMO Saltへの直接出力を置かない。
- 現行CoreはProcessor Frameworkのclass / ProcessDefを直接利用していない。MOがPFを依存に含めていることを理由に、将来AddonがPFを暗黙利用してはならない。Fermentation等がPFを主要ループに必要と判断した場合は、そのAddon自身が直接依存を宣言する。

##### 識別子と新規導入の互換方針（2026-10-10確定）

Grainsは作者確認時点でまだどこにも導入されておらず、旧Core/Grainsセーブが存在しない。**旧セーブの移行、途中導入・削除、既存セーブからのMO取り外しは開発・公開の必須ゲートから除外する。** 過去の移行検査器・記録は歴史的診断用として保持し、現行の必須作業とは扱わない。

`packageId=sucro.ancientmedievaljapan.core`、`AMJC_` prefix、既存DefNameはAMJモジュール間・外部Patchとの参照安定性のため保持する。MOなしではGrains独自の小麦・粉・石臼、MOありではMO提供の資産を条件付きで再利用し、重複Def/Recipeや意図しないMOによる上書きを許さない。新規開始のVanilla / MO × CCTO 4構成を検証する。

##### MO必須解除の実装順序（移行履歴。現行結果は文書冒頭を優先）

1. **テストを先に分岐**し、Production About.xmlを変更しなくてもMOなしのBase XMLをロードできるテスト用Vanillaプロファイルを用意する。
2. BaseのMO直接参照を除去する。Straw / Cereal / RawWood等はMO互換へ移し、建設材料・研究・ScenarioはVanilla/AMJ側で自己完結させる。
3. Barley/設備のMO仮画像をAMJ-owned Production画像へ置換する。
4. Stage Aで小麦をVanillaプロファイルにも提供する場合は、AMJフォールバック小麦を追加し、MO時の重複を抑止する。粉食は必要な用途が確定した範囲だけ追加する。
5. `Patches/MedievalOverhaul_StageA_Wheat.xml`、MOラベル、MOカテゴリ、MO Scenario/素材差分を明示的な条件付き互換へ隔離する。
6. Vanilla / MO × CCTO有無の自動マトリクスをすべて通し、runtime ERROR 0を確認する。
7. MOの必須指定をAbout.xmlと説明から解除し、MO用`loadAfter`・条件付き互換は残す。各テスト・説明の整合を確認する（2026-10-10反映）。

**2026-10-10の状態修正:** `Plant_Rice` 陸稲化・七穀環境回帰は実装され、MOなしの実ゲームSmokeにも過去の合格実績がある。作者指示によりAboutのMO必須宣言を先行解除した。現在の改訂と全構成の実播種・実収穫・季節・ロード契約等の検証は引き続き公開判断の要件であり、以前のPASSを転用しない。

##### 自動テスト・サポートマトリクス

最低限、依存解除実装前に次の4プロファイルを常設する。

| プロファイル | 主な自動確認 |
|---|---|
| Vanilla + Grains | 未解決MO Def/class 0、Stage A作物＋陸稲、一次加工、製粉・最低限粉食、Vanilla simple meal受入、runtime ERROR 0 |
| Vanilla + Grains + CCTO | 上記 + AMJC crop cold-tolerance extension |
| MO + Grains | MO小麦/Flour/Cereal/Straw等の互換Patch、重複小麦・小麦粉・石臼経路なし、陸稲のAMJ設定維持、runtime ERROR 0 |
| MO + Grains + CCTO | MO互換とCCTO互換の同時適用、Stage A回帰、runtime ERROR 0 |

静的XML検証は全プロファイルの前段に置き、Pickleで実ロード後のDef・Recipe・Scenario・Vanilla meal受入・MO互換・runtime ERRORを確認する。RimTest Reduxは、今後C#の移行補助や独立ロジックを追加した場合の単体/ロジック試験を優先し、現行のXML中心実装で無理に代替しない。人間の手動確認は、Production画像/UI/操作感など自動化できない項目に限定する。

Fermentation / Brewingは、GrainsやMOをハード依存にしない設計を採る場合、それぞれ **Vanilla単独の主要ループ** と **Grains併用時の追加原料接続** を最低限の自動ケースとする。MO設備/原料を正式互換する場合だけMO併用ケースを追加し、CCTOは発酵・酒造側が作物温度Defを直接変更しない限り直積マトリクスへ含めない。Fermentation + Brewing相互のケースも、一方の出力を他方が公式に消費する設計が確定した場合だけ追加する。

##### Grains再編後の最終 Base / MO 所有境界（2026-10-07）

この節は ARCH-MODULAR-001 のGrains再編確定版である。上記の旧Agriculture監査、後段の旧Stage B〜Dロードマップ、MO前提の一次加工記述と矛盾する場合は、Grains移行ターゲットについてはこの節を優先する。旧MO必須版の具体値はMO条件付き互換で保持する。下記のBase/MO表と実装進捗に従う。

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
| 豆類 | 所有しない | 所有しない | **Grains外。将来所有先を用途と合わせて決定** |
| 繊維・紡績・製紙 | 所有しない | Grains-MO互換にも置かない | **Grains外。将来のMaterials/Textiles等で決定** |
| 根菜・一般野菜 | 所有しない | 所有しない | **Grains外。将来所有先を決定** |

**脱穀「Xまで実行」の修正仕様（2026-10-15）:** MO併用時の脱穀10レシピは主製品（穀粒・籾）と副産物 `DankPyon_Straw` を出力する。`workerCounterClass` はGrains専用 `AncientMedievalJapan.Grains.RecipeWorkerCounter_PrimaryGrain` とし、先頭の主製品だけをカウントする。ゾーン・品質・在庫条件はRimWorld標準 `CountProducts` を継承する。従来の藁、材料、歩留まり、作業量、DefName、`targetCountAdjustment` を変更しない。Vanillaは標準カウンターのままとする。MO向け配布前に `Assemblies/AMJGrains.dll` を公式ゲーム参照に対してビルドし、ロード・Billの実行・配布物への同梱を確認する。C#ソースのみの変更を実機PASSやリリース完成とはみなさない。

**β暫定脱穀・殻取り経路（2026-10-13 作者確定）:** AMJ専用加工設備の完成まで、**Vanilla・MO両構成**で、既存の簡易加工場 `AMJC_GrainProcessingSpot` と加工台 `AMJC_GrainProcessingTable` に加え、Vanilla燃料式調理台 `FueledStove` と電気式調理台 `ElectricStove` にも脱穀・殻取りRecipeを登録する。雑穀・蕎麦・大麦・米の脱穀・殻取り各単品/10個、および小麦脱穀の単品/10個、計18Recipeを対象とし、共通加工親RecipeとMO有無別の小麦親Recipeにだけ設備を追加する。既存の材料・加工量・作業量（脱穀1個15／10個120、殻取り1個10／10個80）・研究条件は変更しない。作業スキルは2026-10-17にCookingへ変更した。MO導入時のみ従来の脱穀Strawを維持し、VanillaにはStrawを追加しない。独立WorkGiverや新しい設備Defは作らない。AMJ設備完成時に調理台経路を撤去検討する。**製粉のMO石臼専用経路はこの変更に含めない。** 実ゲームで両構成の調理台Billを実行し、ERROR 0を確認するまでは動作確認済みとしない。

**2026-10-17 作者決定 — 穀類一次加工の料理仕事への統一:** Vanilla/MOを問わず、Grains所有の雑穀・ソバ・大麦・米・小麦の脱穀、殻取り／籾摺り、および五種の製粉Recipeの `workSkill` をすべて `Cooking` にする。`AMJC_DoGrainProcessing` とBase限定 `AMJC_DoGrainsMilling` の `WorkGiverDef.workType` も `Cooking` に統一する。MO既存の小麦製粉RecipeとMO石臼WorkGiverは既に料理扱いであり、MO所有Defを無用に上書きしない。加工場所・加工台・Vanilla調理台・Base石臼・MO石臼の登録先は現行どおり保持する。`workAmount`・`workSpeedStat`（Grains手作業の`GeneralLaborSpeed`）・習得率・投入／産出量・研究条件・副産物・DefName・Bill保存IDは変えない。料理仕事へ移すのは担当WorkTypeと経験／要求スキルであり、加工速度や歩留まりの再調整は別判断とする。変更後の実ゲーム実行は未検証であり、既定の通常プレイ確認と併せて受け入れる。

**小麦統合の固定ルール**

現行コードの残存MO依存と検証範囲は [`GrainsDependencyAudit.md`](GrainsDependencyAudit.md) を参照する。Def識別子の分離完了を、画像・実ゲームを含む独立動作の完了と解釈しない。

- MOなし: Grains小麦Plant → Grains小麦束 → 脱穀 → AMJC_Wheat → Grains小麦粉。
- MOあり: MO小麦Plant → DankPyon_RawWheat（小麦束）→ 脱穀 → AMJC_Wheat → DankPyon_Flour。
- 同一プロファイルでAMJ小麦PlantとMO小麦Plant、AMJ小麦粉とMO小麦粉を標準経路として並存させない。
- 既存MO+Coreセーブで参照されているMO小麦/RawWheatと AMJC_Wheat の関係は維持する。Core更新と同時にMOを既存セーブから外す移行は別ケースとして扱う。
- MO小麦の成長・肥沃度等はGrainsの穀物バランスに合わせて条件付きPatchし、供給元がMOであることを理由に未調整値へ戻さない。

**Strawの固定ルール**
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

**Grainsの存在意義を守る恒久回帰**

最低限、痩せ地/標準/肥沃地、寒冷/温暖、短期/標準/長期の代表環境を組み合わせて評価する。収量・growDays・加工価値に加え、CCTO併用時は固定枯死温度/休眠、MO併用時はStraw等の追加価値も含める。

期待する役割は、ソバ/キビ=短期・痩せ地、ヒエ/大麦=寒冷側、小麦=肥沃地・長期・粉食、アワ=標準〜肥沃地で収穫回数を抑える主力とする。単一穀物が代表条件のほぼ全てで最適になる状態を回帰失敗とする。Environmentは差を強調する推奨姉妹Modだが、**Vanilla + Grainsでもこの選択が消えないこと**をリリース条件とする。

**旧Stage B〜Dとの整合**
- 旧Stage B（豆類）、Stage C（繊維）、Stage D（根菜）はGrainsロードマップから外す。
- これらの歴史的調査・候補記述はAMJ全体の将来候補として参照してよいが、「Grainsが所有する予定」という意味では失効する。
- 大豆をFermentationが使う、繊維を製紙/織物が使う、根菜を将来の食生活機能が使う等の接続は、実際の所有Mod確定後に任意互換で行う。

**移行順序**
1. About.xmlを変えず、MOなしBase XMLをロードできるテスト用プロファイルを先に作る。
2. Base XMLからMO研究・素材・カテゴリ・Straw・Scenario参照を除去する。
3. 非MO小麦Plant/穀束、手動石臼、最低限粉食と穀粉の初期3種（小麦粉・蕎麦粉・雑穀粉）をBaseへ追加する。大麦粉も本番Def/Recipeで提供する。米粉・米粉団子の旧実装は2026-10-19に通常生産を停止し互換保持のみに変更した。
4. MO互換を条件化し、MO小麦/RawWheat/Flour/Millstone/Strawへ上表どおり収束させる。
5. MO仮テクスチャをGrains所有Production画像へ置換する。2026-10-08の作者指示により、MO有無にかかわらずAMJ側の共有画像設定を優先する。現在は既存AMJ/Vanilla仮参照で、MO時の旧画像復元は廃止した。専用Production画像・実描画は未完了（`GrainsDependencyAudit.md`）。
6. Vanilla + Grains / Vanilla + Grains + CCTO / MO + Grains / MO + Grains + CCTO の恒久マトリクスに、環境別穀物選択回帰を加えて通す。
7. About.xmlの必須依存を解除し、任意`loadAfter`とREADME/Workshop等の公開説明を同期する（2026-10-10反映）。

この所有境界確定後の最初のruntime作業は、**テストハーネス分離**とする。

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
- Base設備は `AMJC_ManualMillstone`（BlocksGranite 30 / WoodLog 20）。MO時は既存 `DankPyon_CraftFlour` でAMJC_Wheat→DankPyon_Flourへ進み、蕎麦/雑穀製粉を `DankPyon_Millstone` に接続する。最低限製粉を研究で止めないためMO石臼のresearchPrerequisitesを条件付きで除去する。MO小麦の既存研究と旧38件のAMJC契約は維持する。MO側の研究除去Patchは実ソースに対する静的参照確認であり、他Modとの実ロード優先順位は未検証。
- 最低限粉食の初期値は以下。各Recipeは研究・Cooking最低技能なし、Campfire / ElectricStove / FueledStoveで利用可能、workAmount 300、Nutrition換算入力0.5→料理1個0.9、2.5日保存、`AMJC_AteFlourFood` のMood +2（0.5日、stackLimit 1）。MO時だけ餺飥の入力をDankPyon_Flourへ置換する。

| 料理 | ThingDef | RecipeDef | Base入力 | MO入力 |
|---|---|---|---|---|
| 餺飥 | `AMJC_Houtou` | `AMJC_CookHoutou` | `AMJC_WheatFlour` | `DankPyon_Flour` |
| そばがき | `AMJC_Sobagaki` | `AMJC_CookSobagaki` | `AMJC_BuckwheatFlour` | 同左 |
| 雑穀団子 | `AMJC_MilletDumplings` | `AMJC_CookMilletDumplings` | `AMJC_MilletFlour` | 同左 |

新規説明文は日本語先行レビュー前のため空欄とし、未承認の歴史的説明を英訳・公開していない。旧小麦Recipeの説明/翻訳は保持する。新規画像は生成せず、Base小麦は既存AMJアワ、束・粉は既存AMJ雑穀、石臼・料理はVanillaの開発用画像参照を使う（step 3当時の記録。2026-10-08の実機画像ロード障害により、現行の粉食3品はAMJ同梱雑穀の仮画像へ変更した。詳細は本書の実機回帰注記を参照）。**これらは完成画ではなく、固有画像・日本語説明レビュー→英訳、実機の継承/参照/Graphic/Bill実行、環境別選択・既存セーブ回帰をリリース前に完了する必要がある。** Production About.xmlのMO必須依存はまだ変更しない。本段落はstep 3時点の記録であり、現行の開始シナリオ互換コピーは `LegacyStartingScenarios` へ移転済みである（`Docs/ScenarioExtraction.md`参照）。

`Tests/validate_grains_chain.py` と変異回帰は入力/出力、栄養保存、研究不要、保存/Mood、MO差替え/非重複を静的確認する。旧38件はそのまま照合し、新規11件の共有Defだけ追加許可する。4構成のPickleに製粉/粉食のロード後契約を加え、各6シナリオとした。ゲーム内でBillを完了した証拠ではなく、実機実行自体も未実施。

#### 外部Modとの競合優先順位

AMJを導入している場合、**AMJが責務を持つ機能・資源・バランスについてはAMJの設計を最終優先する。**

- 外部Modと競合しない要素は可能な限り共存させる
- 同じ作物・設備・研究・加工経路・栽培条件等を双方が変更する場合は、AMJ側の定義・数値・研究位置・加工設計へ統一する。具体的には大麦のMO研究条件復元、およびAMJG加工台のMO鉄材・研究条件への置換を行わない
- 外部Modの仕様へAMJを合わせるために、AMJ内で確定した肥沃度感応度・温度特性・収量・加工工程等を崩さない
- これは他Modを文化的理由だけで削除する方針ではなく、**AMJが扱う領域の整合性を守るための競合解決規則**である
- 外部Modにしか存在しない非重複要素は、そのまま利用できることを優先する

**Vanilla Plants Expanded - More Plants（VPE More Plants）**
- 必須依存にはしない。Vanilla基準で水田・追加作物を実装した先行例として参考にする
- アワ・ヒエ・大麦等、AMJと役割が重なる作物が併用環境に存在する場合は、AMJ側で重複非表示・統合・数値上書き等を行い、AMJ設計を優先する
- AMJで確定した growDays、収量、肥沃度感応度、温度特性、研究、一次加工等をVPE値へ合わせない
- VPEの水生栽培が肥沃度を無視する設計はAMJ水田へ採用しない。AMJ作物では水田でも作物固有の肥沃度感応度を維持し、土地条件を農業上の選択として残す
- VPE水田とAMJ水田を併用する場合の具体的なDef統合・栽培可否は、Rice Cultivation実装時に互換Patchとして決める

**種システム系Modは要望ベース互換とする。**
- AMJ Core標準では、種アイテム・播種時の種消費・種在庫管理を導入しない
- Progression: Agriculture、SeedsPlease系等は、初期の正式互換対象には含めない
- 外部Mod側の自動対応でAMJ作物が正常に扱える場合は、専用Patchを追加しない
- 利用者から具体的な対応要望または不具合報告が出た場合に、対象Modの保守状況・利用状況・AMJ/MO研究体系との競合を確認した上で任意互換Patchを検討する
- 特に外部ModがMOの農業研究を削除・置換する場合は、AMJ側の研究進行・作物解禁設計を崩さないことを優先する

Vanilla Expanded Frameworkなども、必要な機能が小さい限り必須依存にはしない。

### 2.9 Grainsと別Modの境界

Grainsの責務は、**乾田穀物および畑地・水田に共通する稲Plantの所有とV1畑地栽培、穀物の一次加工・製粉・最低限の穀物食を一つのゲームループとして成立させること**に限定する。水田での生育条件・水管理はRice Cultivation側の拡張とする。

#### Grainsに入れる基準

- アワ・ヒエ・キビ・ソバ・大麦・小麦
- Vanilla `Plant_Rice` を再利用する共通稲（V1の畑地栽培）と、収穫物の稲束・籾の共通加工を経て得る `RawRice`
- 穀束・殻付き中間物・可食穀粒
- 脱穀・殻取り・製粉と、その最小設備
- 雑穀粉・蕎麦粉・大麦粉・小麦粉の4種。米粉および米粉団子は新規生産を停止し旧Def参照だけ保全する。
- 粉を無用途にしないための最低限粉食
- 各穀物の肥沃度・温度・生育日数・収量・保存性・加工価値
- Vanilla自己完結に必要なフォールバック
- MO存在時だけ適用する公式互換

#### Grainsに入れない基準

- 水田・同一稲Plantの水田栽培条件・田植え等の水田固有工程・稲作水利
- 小豆・大豆等の豆類
- 大麻・カラムシ等の繊維作物、紡績、製紙
- 里芋・大根等の根菜・一般野菜
- 採集一般、山菜、きのこ、果樹
- 発酵、酒造、塩蔵、一般乾燥保存
- 一般水利、温泉
- 開始シナリオ、開始用Faction/PawnKind、初期研究・物資
- 建築、衣服、武具、派閥、背景、イベント
- 将来Addonが使うかもしれないという理由だけの共通素材

旧Stage B（豆類）・C（繊維）・D（根菜）はGrainsロードマップから外す。内容自体を永久不採用としたわけではなく、主要ゲームループと自然な所有先が確定した時点で別機能として再評価する。

#### 開始シナリオの独立Mod化（2026-10-14 完全分離）

新規開始シナリオ「新しい村」、開始用プレイヤー派閥・PawnKind・開始物資・研究・翻訳と MO / Grains の開始用互換は [AMJ Scenarios](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Scenarios) が単独所有する。Grains は農業・穀物加工・料理に限定し、シナリオを読み込む条件付き旧互換コピーを持たない。

- Grains 単独では `AMJC_NewVillage`、`AMJC_PlayerVillage`、`AMJC_Villager` を提供しない。
- Scenarios は Vanilla 単独の開始を提供し、Grains と MO は任意互換である。Grains は Scenarios を必須としない。
- 開発段階で公開・旧セーブ運用がないため、Grains に旧 Core 開始を再提供する旧セーブ移行仕様は採用しない。
- 旧38件の MO 参照契約は履歴fixtureのまま保存し、Grains 実装の静的比較から移転済み3Defだけを除外する。新規開始の正本・恒久テスト・日英ローカライズは Scenarios 所有。
- 分離後の回帰契約は [Docs/ScenarioExtraction.md](ScenarioExtraction.md) を参照。Grains での既存の加工・栽培・調理テストは維持する。

#### 詳細設計前の先行Mod監査

AMJ横断の先行Mod監査・比較候補はProjectの [ExistingModAudit.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/ExistingModAudit.md) を正本とする。GrainsにはGrains自身の依存・互換・実装判断だけを残す。
#### 先行Mod監査の現時点結論（2026-10）

横断的な結論と未所属候補はProjectの [ExistingModAudit.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/ExistingModAudit.md) へ移管済み。Grains固有の現行互換方針は本書のFood Drying・MO統合・互換優先度の各節を正本とする。
#### Mod構成そのものを設定として扱う

AMJ群では、巨大なSettings画面で多数の機能をON/OFFさせるより、**導入するModそのものによって機能構成を決める**設計を優先する。

方針:
- 機能単位で十分に独立できるものは別Modへ分割する
- 各Modは、導入した時点のデフォルト設定でそのModらしい遊びが成立することを目標とする
- 「Enable Fishing」「Enable Japanese Factions」「Enable Fermentation」のような大項目ON/OFFを大量に設けない
- Settingsは数値調整、難易度、互換上必要な例外、デバッグ等に限定する
- ただし、**同じ機能構成のまま難度・数値・互換挙動を変える**ように、Mod分割では不自然な差はオプション化してよい
- ユーザーが多数のAMJ系Modを併用することを前提にし、**Modリスト自体を構成画面として扱う**
- Mod数が増えること自体より、巨大Mod化によって不要機能・競合範囲・保守範囲が増えることを避ける

Addonの基本的な役割は、Coreで粗く抽象化している生活・生産工程を**必要な分野だけ細かい作業粒度へ展開すること**とする。Addonを追加するほど、専用設備・中間素材・温度管理・追加加工・研究段階・物流管理などが増え、プレイヤーが扱う生産工程は原則として複雑になり、難度も上がる方向でよい。これは意図した進行であり、Addonを「入れるほど単純に強くなる追加コンテンツ」にはしない。

ただし細分化そのものを目的にはしない。各Addon内部でも、**判断・在庫・設備配置・季節/温度管理・投入コストなどにゲーム上の意味があり、かつUXとして遊んで面白い工程だけを残し、意味のない史実上の微細工程は引き続き抽象化する。** 面倒さそのものを難度として扱わず、選択・計画・リスク管理・設備運用などの遊びに変換できない負荷は削る。したがって「Coreより粒度は細かいが、そのAddon内では必要以上に細かくしない」を共通原則とする。

#### 基礎Addon

Salt Preservation、Fermentation / Brewing、Sake等の専用リポジトリ作成前の詳細設計はProjectの [FermentationBrewingPreservationCandidate.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/FermentationBrewingPreservationCandidate.md) を正本とする。Grainsはそれらを必須依存にしない。
#### AMJ容器（甕）— 既存Mod・時代考証監査後の方針

容器・甕・陶器の横断設計はProjectの [ContainersPotteryCandidate.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/ContainersPotteryCandidate.md) へ移管済み。Grainsは汎用容器システムを所有せず、必要な収納・加工設備は各機能の所有Modで判断する。
#### Japanese Environment / 日本環境Mod

日本の気候・地形・河川・海岸線・バイオーム・野生植生・季節景観は、AMJ Coreへ統合せず、**独立姉妹Mod `Ancient-Medieval-Japan-Environment`** が所有する。

- Repository: `sucRo-RimWorld/Ancient-Medieval-Japan-Environment`
- EnvironmentはAMJ Coreを必須にしない
- Grains側は乾田穀物と**畑・水田に共通する稲Plant・米**、それらの基準生育値・一次加工・製粉を所有する。**水田地形・給排水・水田での生育／収量補正**は独立したRice Cultivation Modが所有する
- Environment側は、それらが置かれる外部環境である気候・標高/Hilliness・河川・海岸線・バイオーム・野生植生等を所有する
- AMJ Core / CCTO / MO等との接続は必要に応じて任意互換とする

Environment固有の詳細数値・世界生成仕様はCore側へ重複記載せず、**Environmentリポジトリの `Docs/Design.md` を正本**とする。

#### Hunting & Gathering / 狩猟採集Mod

未所属の狩猟採集・山野資源・沿岸採集設計はProjectの [HuntingGatheringCoastalCandidate.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/HuntingGatheringCoastalCandidate.md) を正本とする。Grainsは採集システムを所有しない。
#### AMJ Backgrounds / 背景Mod

未所属の背景Mod候補はProjectの [SocietyModulesCandidate.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/SocietyModulesCandidate.md) を正本とする。GrainsはBackstoryを所有しない。
#### AMJ Clothing / 一般生活者の衣服Mod

未所属の一般生活者衣服候補と既存和服Mod比較はProjectの [SocietyModulesCandidate.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/SocietyModulesCandidate.md) を正本とする。Grainsは衣服Defを所有しない。
#### AMJ Factions / 派閥Mod

未所属のFaction・集落生成候補はProjectの [SocietyModulesCandidate.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/SocietyModulesCandidate.md) と [ExistingModAudit.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/ExistingModAudit.md) を正本とする。GrainsはNPC派閥を所有しない。
#### AMJ Events / 生活・社会イベントMod

未所属の生活・社会イベント候補はProjectの [SocietyModulesCandidate.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/SocietyModulesCandidate.md) を正本とする。GrainsはイベントModの実装仕様を保持しない。
#### AMJ - Medieval Overhaul Japanization / MO日本化レイヤー

JapanizationはGrainsの機能ではない。専用リポジトリ作成前の設計正本はProjectの [MedievalOverhaulJapanizationCandidate.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/MedievalOverhaulJapanizationCandidate.md)、詳細監査はProject `Docs/Research/MedievalOverhaulJapanization*.md` とする。GrainsはMOとの自分自身の互換Patchだけを所有する。
#### Vanilla生活文化・心情はJapanizationへ抱え込まない

VanillaのThought / Trait / drug attitude / room・家具期待等を前近代日本へ合わせる構想は、MO固有PatchであるJapanizationの責務外とする。**独立Modとして切り出される前の構想・監査は `sucRo-RimWorld/Ancient-Medieval-Japan-Project` が正本**であり、Grainsでは詳細仕様を保持しない。

Project側の現行候補は仮称 `AMJ - Premodern Culture`。詳細は Project `Docs/Ideas.md`、`Docs/Roadmap.md`、`Docs/Research/VanillaPremodernCultureThoughtAudit.md` を参照する。Japanization側では、将来同Modが成立した場合のMO家具・食事・酒等との任意互換境界だけを扱う。

MO 1.6.2.2の研究ツリー全67ノード（MO独自51 + MOが移動/再構成するVanilla 16）の初回分類、史料アンカー、未解決監査項目は [Project MedievalOverhaulJapanizationResearchAudit.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/MedievalOverhaulJapanizationResearchAudit.md) を詳細正本とする。研究・武器・料理等の個別分類は同文書の監査を経て実装へ落とし込み、チャット上の一時分類だけでXMLを変更しない。

### 2.10 MOの研究フローを古代～中世日本史へ再構成する

MO全体の研究再構成はGrainsの責務外で、AMJ - Medieval Overhaul Japanizationが所有する。専用リポジトリ作成前の詳細設計はProjectの [MedievalOverhaulJapanizationCandidate.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/MedievalOverhaulJapanizationCandidate.md) を正本とする。Grainsは自分が所有する作物・加工の研究解禁と、MO併用時の条件付き接続だけを管理する。
### 2.11 保管フィルタの穀物分類（2026-10-19 改訂）

GrainsはMOの `DankPyon_Cereal` と同じ **`Foods` 直下**に `AMJC_Grains（穀物）` を置き、`AMJC_GrainSheaves（穀物束）`・`AMJC_GrainKernels（穀粒）`・`AMJC_GrainFlour（穀粉）` の3分類を子に置く。表示は `食物 > 穀物` であり、従来の `食物 > 生の食材 > 生野菜 > 穀物` は廃止する。穀物束・殻付き穀粒・可食穀粒・粉の個別DefName、ゲーム数値、Recipeと既存グラフィックは変えない。MOなしでも同じGrains分類を提供する。

- **穀物束:** 雑穀束、ソバ束、大麦束、稲束、小麦束（Vanilla：`AMJC_RawWheat`、MO：`DankPyon_RawWheat`）。
- **穀粒:** 殻付き雑穀・ソバ・大麦・籾、可食雑穀・ソバ・大麦・小麦穀粒、Vanilla `RawRice` と白米。
- **穀粉:** 雑穀粉、蕎麦粉、大麦粉、小麦粉（Vanilla：`AMJC_WheatFlour`、MO：`DankPyon_Flour`）。米粉は通常の生産対象外。

**MO穀物カテゴリの統合（2026-10-19）:** MOの `DankPyon_Cereal` に分類されていた可食小麦穀粒 `AMJC_Wheat` を `AMJC_GrainKernels` だけへ移し、MO/Grainsの保管表示を `Foods > AMJC_Grains > AMJC_GrainKernels` に一本化する。MO小麦束 `DankPyon_RawWheat` は従来どおり `AMJC_GrainSheaves`。MO側の独立カテゴリ `DankPyon_Cereal` のDefは互換参照用として保持するが、標準の小麦アイテムは所属させない。小麦DefName・栄養・加工段階は変更しない。

**MO製粉・醸造の入力互換:** MO 1.6の製粉レシピ3件、エール麦汁レシピ、Processor Frameworkの風車・水車の `DankPyon_Cereal` 入力を、可食小麦穀粒 `AMJC_Wheat` の直接指定に置換する。MOの製粉・醸造で未脱穀束や他の穀粒を誤消費させず、Grainsの既存専用粉・レシピも変更しない。MOの収納フィルタ内に旧カテゴリ参照がある場合も同様に小麦直接指定へ置き換える。未把握の第三者Modが旧カテゴリを動的に使う場合は互換性を保証しない。実機で表示と投入を確認する。

**MO粉カテゴリの整理:** `DankPyon_Flour` の独立した備蓄表示への所属を除き、`AMJC_GrainFlour` のみへ所属させる。MOが定義する `DankPyon_Flour` ThingCategoryDefそのものは、他Modの参照を直ちに未定義にしないため空の互換Defとして維持する。MOの既存料理Recipe・穀物棚がこの旧カテゴリを材料/収納許可に用いる箇所は、`DankPyon_Flour` ThingDefを直接許可するPatchに置換し、他のGrains穀粉をMO料理へ無条件追加しない。第三者の未把握の旧カテゴリ参照については動作保証しない。

従来 `PlantFoodRaw` や `FoodRaw` の子孫として自動許可されていたRecipeおよびThingDef収納フィルタには、`AMJC_Grains` を明示追加する互換Patchを設ける。親カテゴリそのものを元へ戻したり、`PlantFoodRaw` の直接所属を穀粒へ復活させたりせず、UIに同じ穀粒が複数表示されることを防ぐ。個別料理の材料定義は変更しない。Vanilla/MOのロード後UI・Recipe・フィルタ挙動は最新ソースで別途実機確認する。

#### Adaptive Primitive Storage任意互換（2026-10-17）

Grains単独で使用する独自穀倉は追加しない。MO + APS併用時の藁建築版はAPS既存穀倉の派生Defとして追加する。Adaptive Primitive Storage（Workshop 3400037215／packageId `Adaptive.PrimitiveStorage`）既存の `ASF_Granary`（3×3、54スタック）を利用する。同設備の固定・既定収納フィルタは `PlantFoodRaw` と `PlantMatter` を受け入れるが、`AMJC_Grains` は今後 `Foods` 直下のため両設定への明示追加が必要となる。APS存在時だけ両フィルタへ `AMJC_Grains` を追加し、親カテゴリ・既存食材を除外しない。Grainsのハード依存にAPSやAdaptive Storage Frameworkを加えない。

APSの1.6現物Defが `Hay` を建築材料に使う4設備（`ASF_Granary` 50、`ASF_WovenBasket` 75、`ASF_WovenBasketDouble` 150、`ASF_WovenBasketTriple` 225）は、**元DefNameと干し草（`Hay`）の必要数を変更しない**。Grains + MO + APSの三者併用時のみ、同じ建築カテゴリに藁版の別建築選択肢 `AMJC_APS_{Granary,WovenBasket,WovenBasketDouble,WovenBasketTriple}_Straw` を提供する。藁版は干し草版と同じ必要数の `DankPyon_Straw` のみを消費し、元の石材（穀倉75）・木材（穀倉25）・研究・外観・収納容量・腐敗防止等はAPSの1.6仕様に合わせる。建築画面で干し草版と藁版を別アイコンから選ぶ（材料切替ギズモではない）。`costList` にHayとStrawを併記して両方を要求することはしない。Grains本体単独・MOなし・APSなしでは派生Defを読み込まない。APS側の仕様が変わった場合は互換先の差分再監査が必要。既存の建築物DefNameとセーブ参照は保護する。なお、この互換はGrainsに同梱するため、GrainsなしでのAPS+MO一般互換まで提供するものではない。**利用者向け推奨度は「併用を強く推奨（任意）」とする。** 大容量の穀物保管を既存の穀倉で実現し、重複したGrains独自の穀倉を追加しないため。README・Workshop英日・2game・About.xmlにも同じ位置づけを掲載する。1.6実機での収納フィルタ操作・建築材料表示は未検証。

## 3. Grains本体（他AMJ Modなし）の位置づけ

現CoreはAMJ Mod群すべての共通基盤ではなく、**乾田穀物の選択と一次加工を扱うGrains系独立コンテンツMod**として再編する。

他のAMJ Modを導入しなくても、RimWorld + Grainsだけで「栽培 → 収穫 → 脱穀/殻取り → 可食穀粒 → 必要に応じて製粉 → 最低限の粉食」まで成立させる。

発酵・酒造・水利・温泉・派閥・イベント・豆類・繊維・根菜はGrainsの自己完結条件に含めない。MO併用時はMOの小麦・粉・石臼・Straw等を条件付き公式互換から再利用する。

### 鉄資源を前提にしたAMJ側コスト設計

Core自身は鉄鉱床・砂鉄等の**供給量や生成分布を変更しない**。一方で、Coreを含むAMJ側の設備・レシピ・加工経路は、**鉄を潤沢な汎用建材として浪費しない**ことを前提にコスト設計する。

ただし、**鉄を不足させること自体をゲーム目的にはしない**。希少性を導入するのは、それが「何に鉄を使うか」「何を別素材で代替するか」「採掘・砂鉄・交易のどの供給経路を選ぶか」といった判断を生み、プレイの違いにつながる場合に限る。単に必要量に対して供給を減らし、待ち時間・作業量・詰まりだけを増やす調整は避ける。

方針:
- 日本側で木・石・土・竹等による現実的な代替が成立する設備は、鉄を必須にしないか、使用量を抑える
- 刃物・金具等、機能上鉄が必要な箇所へ重点的に鉄を使う
- 同じ役割を持つMO側の西洋的設備・工程とAMJ側ルートが併存する場合、**AMJを導入しているならAMJ側ルートを選ぶことに資源上の意味がある**バランスを許容する
- 通常AMJ環境ではMO側ルートを強制削除せず、鉄消費・材料構成・工程差によってAMJ側を自然な選択肢にする
- Japanizationでは、MO側の西洋的ルートを日本の同等技術へ再解釈できる場合は研究/名称/外観を置換し、対応する意味がなく除去しても進行が成立する場合は無効化・非表示化できる。独立した日本側ゲームループの追加は各所有Modの責務とする
- 最低限の砂鉄供給・製鉄はIronmakingが所有する。既存鉱床の希少化・大規模地域分布変更はCoreでもIronmaking導入時の標準変更でもなく、将来の資源分布Mod候補として保留する
- **修理・再利用そのものは日本固有要素ではないためCoreの責務にしない**。既存の汎用修理Mod（R⁴等）または必要時のみ独立したRepair & Reuse機能枠で扱う
- **AMJの資源バランスはRepair & Reuseなしでも成立させる**。同Modによる修理・素材回収を前提に、鉄・布・革等の供給量やAMJレシピを不足側へ追い込まない
- Repair & Reuseは資源効率・継戦能力・装備寿命を改善する**強く推奨する補助システム**であって、AMJ進行を成立させるための隠れた必須依存にはしない
- 鉄が不足したときは、**代替素材・低鉄消費のAMJ設備・砂鉄・採掘・交易・優先順位付け**など、複数の対処経路を持たせる
- 鉄の希少性によって「どの設備を先に作るか」「武具へ回すか農具へ回すか」「現地調達を続けるか交易へ頼るか」等の選択が生じることを狙う
- どの経路を選んでも恒常的な待機や単純作業だけが増える状態、または鉄不足によってMO/AMJの主要進行が事実上停止する状態は、**バランス失敗**として扱う

これにより、Ironmakingや資源分布Mod未導入でもAMJ側の設備・工程は鉄を節約する設計として成立する。Ironmakingは任意の砂鉄・製鉄経路を追加し、既存供給を強制的に減らさない。Repair & Reuseを併用すれば資源循環は改善するが、未導入でもAMJの基本的な生活・生産・進行は破綻しない。

### Repair & Reuse / 修理・再利用Mod（仮）

Grainsは修理・再利用を所有しない。独自実装の要否とR⁴ / Simple Mending比較はProjectの [RepairReuseCandidate.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/RepairReuseCandidate.md) を正本とする。
### 開始シナリオ（Scenarios Mod所有）

`AMJC_NewVillage`（新しい村）と `AMJC_PlayerVillage` / `AMJC_Villager` は Grains から除去済み。開始人数・派閥・初期物資・研究・開始イベント・翻訳の正式な設計と実装は独立 [AMJ Scenarios](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Scenarios) の正本を参照する。Grains 自体の栽培・製粉・食事に特定の開始シナリオは不要。

### 最初の公開Alphaテーマ

**「古代～中世前期日本の畑作・雑穀農業OH」**

Grainsの公開・依存解除に鉄資源ModやIronmakingの同時公開を要求しない。**Ironmakingは独立開発・任意導入**とし、最低限の砂鉄供給・製鉄を所有する。旧Japanese Iron Resourcesの同時公開案は撤回し、既存鉱床の地域化だけを将来候補として再評価する。

最初の公開Alphaでは、まず**水田を必要としない畑作作物**を中心に完成させる。

最優先目標は、

> バニラ/MOの「最も効率のよい一種類を大量栽培する農業」から、  
> 気候・土地・季節によって作物を植え分ける農業へ変える。

こと。

#### Grainsの存在意義・存続条件

Grainsの存在意義は「日本の穀物Defを増やすこと」ではなく、**コロニーマップの環境条件に応じて主食穀物を選び分ける農業判断を成立させること**に置く。

- 土壌肥沃度、気温、生育可能期間、霜・低温耐性、収量、加工先等の組み合わせによって、代表的な環境ごとに有利な穀物が変わること
- ソバ/キビの短期・痩せ地適性、ヒエ/大麦の寒冷側、小麦の肥沃地・粉食価値、アワの標準～肥沃地での収穫回数削減等、既存の役割差を維持すること
- Environmentは土地・気候差を強く見せる推奨姉妹Modとするが、Grains単体でもVanillaマップ条件の範囲で可能な限り役割差が残ること
- 自動評価では複数の代表環境を使い、単一穀物がほぼ全条件で最適解になっていないことを回帰確認する
- 将来の調整で環境ごとの使い分けが実質的に消えた場合は、史実上存在するという理由だけで穀物数を維持せず、数値再調整・役割統合・削減を行う

この条件を満たす限り、Grainsは独立Modとして一旦十分なゲーム上の存在意義を持つものとする。

---

## 4. 最初の公開Alpha実装範囲

### 4.1 作物

最初の公開Alphaの中心作物は以下とする。

| 作物 | 主な役割 | ゲーム上の方向性 |
|---|---|---|
| 粟（アワ） | 標準畑の主力雑穀 | キビより生育は長いが1回の収量が多い。普通～高肥沃度の主力畑で、播種・収穫回数を抑えやすい |
| 稗（ヒエ） | 寒冷地向け雑穀 | 3種の中で低温側の成長域が広く、春秋・寒冷地で栽培期間を確保しやすい |
| 黍（キビ） | 最短期・低肥沃度向け雑穀 | 3種の中で最短期。肥沃度低下による成長減速も小さく、痩せ地や残り生育期間が短い状況に向く |
| 大麦 | 日常主食＋将来の加工用途 | 寒冷寄り。粒食可能。後に麦味噌・麦茶へ拡張 |
| 小麦 | 製粉・粉食向け | 粒食より加工価値を重視 |
| 蕎麦 | 短期・痩せ地向け | 成長が速く肥沃度依存が小さい。霜には弱め |
| 稲（Vanilla `Plant_Rice`、V1は畑地栽培） | 水田を使わない米作 | VanillaのRiceを別作物として増やさず同一Plantを畑作基準に調整する。最終的な成長・収量役割は既存6作物との七穀回帰で確定する |

### 4.2 作物の差別化軸

単なる収量違いにはしない。

- 成長速度
- 最低成長温度
- 低温耐性
- 枯死耐性
- 最低栽培可能肥沃度
- 肥沃度感応度
- 必要栽培スキル
- 収量
- 保存期間
- 生食可否
- 一次加工先

**注意:**  
「低温に強い」を安易に「氷点下でも成長」にしない。  
冬越し・低温成長・霜耐性は別要素として扱う。

最初の公開AlphaではXMLだけで表現可能な範囲を優先する。低温枯死についてはCore独自のC#実装を重複して持たず、独立姉妹Mod **Crop Cold Tolerance Overhaul（CCTO）** が導入されている場合に、AMJ側の互換PatchからCCTOのXML-facing extensionを付与して固定枯死温度を設定する。CCTO未導入時も、AMJ PlantDef自身の成長速度・肥沃度・最低成長温度・加工等のCore仕様は成立させる。

AMJC固有作物の耐寒値と保存候補範囲の正本は [AMJC作物の耐寒データ](Balance/Crops/ColdTolerance.md) に置く。最低成長温度はAMJCのPlantDef、固定枯死・休眠はAMJC側の条件付き互換XMLが持ち、CCTOの実装予定表から参照・転載する運用は行わない。

### 4.2.1 Stage A畑作6作物の確定バランス

2026-10-01〜02のStage A詳細設計で確定していた数値を、MO併用プロファイルとCCTO独立姉妹Mod方針に基づく初期値として正本化する。**MO本体が提供する小麦Plantの研究条件・供給元はMOに属し、AMJG所有の大麦や加工台には波及させない。Grains移行後の所有境界は §2.8 を優先する。** growDays・収量・肥沃度・温度等の6穀物バランス値は移行後も原則維持し、変更する場合は競合作物をまとめて再監査する。

| 作物 | growDays | 可食穀粒の基準収量 | fertilityMin | fertilitySensitivity | 成長可能温度 | 最適温度 | CCTO固定枯死温度 | sowMinSkill | 栽培解禁 |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---|
| ソバ | 4 | 8 | 0.4 | 0.25 | 5～35℃ | 12～25℃ | -2℃ | 1 | 初期 |
| キビ | 4 | 9 | 0.5 | 0.3 | 8～42℃ | 18～32℃ | -3℃ | 0 | 初期 |
| アワ | 9 | 27 | 0.5 | 0.4 | 8～42℃ | 18～32℃ | -3℃ | 0 | 初期 |
| ヒエ | 7 | 18 | 0.5 | 0.5 | 5～40℃ | 15～30℃ | -2℃ | 0 | 初期 |
| 大麦 | 10 | 22 | 0.5 | 0.6 | 0～35℃ | 5～22℃ | -8℃ | 2 | 初期（MO併用時も同じ） |
| 小麦（MO） | 12 | 28 | 0.7 | 0.9 | 最低0℃。高温側はMO/RimWorld基準を維持 | MO/RimWorld基準 | -6℃ | 0（MO 1.6で明示なし） | MO `DankPyon_BasicAgriculture` |

**2026-10-19 実プレイ後の雑穀差別化改訂:** 従来のアワ6/13、ヒエ6/12、キビ5/11は、収穫日数と収量が近すぎて作物選択に結びつかなかった。アワ9/27、ヒエ7/18、キビ4/9へ変更し、収穫1回の数量差を9個ずつ、生育日数差を2日以上とする。キビは短期間・少量、ヒエは冷涼期も育つ中期作、アワは長期・多収で植え直し回数を減らす主力とする。ソバ・大麦・小麦・稲、CCTOの低温枯死閾値、脱穀後の保存性と加工・調理数値は変更しない。Finite-season検証の前提と結果は[Millet Cultivation Balance](Balance/Crops/Millet_Cultivation_Balance.md)に示す。農作業・季節変化・実ゲームUIでは未確認であり、`growDays` は実際の暦日を意味しない。

### 4.2.1a Vanilla/MO既存作物との生育期間整合（2026-10-15 公開連携確定）

**作者決定：Grainsの生育日数のバランスを基準に、バニラ・MOの既存作物の相対的な生育期間を揃えるための任意姉妹Modが [AMJ Crop Growth Balance](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Crop-Growth-Balance) である。** CCTOの耐寒性・枯死温度調整とは別目的。Grains側は七穀（アワ9・ヒエ7・キビ4・ソバ4・大麦10・小麦12・稲5）と加工・収穫量・栽培条件、Vanilla `Plant_Rice` とMO小麦の統合だけを所有する。Crop Growth Balance側がバニラ6作物とMO導入時の7作物の `growDays` だけを変更し、Grainsの作物をパッチしない。

両Modは技術的には独立して単独導入可能。ただし**目的はGrainsの数値基準との整合性**であり、現実の暦日への厳密な換算、全作物の収量・栄養・労働効率の均等化は採用しない。2026-10-15のMod分離時点ではGrainsの作物値を変更していなかったが、2026-10-19の実プレイを踏まえた雑穀数値改訂は上記§4.2.1に反映した。CGB所有の既存作物13件はこの変更で自動上書きしない。元の広域パッチは旧Grains PR #99から所有分離PR #101で削除済み。Crop Growth Balanceの正式仕様・実装正本はそのリポジトリの `Docs/Design.md` とXML。

**GitHub Actions配布方針（2026-10-15）：** GrainsとCrop Growth Balanceを含む今後のAMJ各ModのWorkshopアップロードでは**YADAを使わずGitHub Actionsに統一**する。実際の配布物をGitHub Actionsで許可リストにより構築し、その成果物とSHAを検証した上で、明示的な手動公開操作による保護されたSteamCMD／Steamworks経路から送信する。既存のGrains `.rimignore` は歴史的なYADAフィルタの静的検査資料であり、**Actions配布物の抽出に用いない**。同一Modの著者アカウント、Workshop ID、公開状態、バージョンおよびタグは独立の確認対象。Grains用の実Steamアップロードworkflow／対象アイテムの確定は未完了。push/PRで勝手にアップロードしない。

**公開時期の条件（作者確定）：Crop Growth Balanceを単独先行公開せず、Grainsの初回一般公開と同時に公開する。** CGBのための別個の長期実プレイは求めず、Grainsの実プレイでGrains作物を優先して確認する。CGBの作物Def反映・代表播種/成長/収穫・Grains共存は既存のLinux 1.6 Pickle実機検証済み。MO原本由来FMOD音声ERROR各2件が残るため、ゲーム全体の厳格ERROR0を4/4 PASSと表示しない。CGBの収穫量・栄養・全季節・Pawn作業時間の追加精密監査は同時公開の条件ではない。**Grains本体の加工・料理・画像・セーブ等の独自公開条件は維持**し、それらの未完了をCGBの作物検証へ転嫁しない。Workshop/2gameでの公開実行は両リポジトリが準備できた段階で別途実施する。

アワ・ヒエ・キビについて、プレイヤーが「どの土地・気温・残り生育期間ならどれを植えるか」を判断できるよう、肥沃度・温度のゲーム内式を含む比較資料を [`Docs/Balance/Crops/Millet_Cultivation_Balance.md`](Balance/Crops/Millet_Cultivation_Balance.md) に置く。そこに掲載する数値・グラフは**現実の農業値ではなく、史実・農学的特徴を参考にAMJへ落とし込んだゲーム内バランス値**である。

補足:
- 収量列は、穀束・殻付き中間物を経由する場合も含めた**最終的な可食穀粒量のバランス基準**であり、PlantDefの未脱穀物 `harvestYield` をそのままこの値にするという意味ではない。未脱穀物の収量とRecipe変換比率は、この最終基準値を再現するよう実装時に決める。
- アワ・ヒエ・キビは栽培中のみ別PlantDefとし、収穫後は共通の「雑穀束」→殻付き雑穀→「雑穀」へ統合するため、収穫後の性能差を残さない。
- **現行MO必須版では**MO 1.6の `DankPyon_Plant_Wheat` を利用する。Grains移行後はBaseにフォールバック小麦Plant/穀束を持ち、MO併用時だけMO小麦を可視供給元として利用する。growDays 12、収量28、fertilitySensitivity 0.9、AMJ基準 `fertilityMin=0.7` の役割は両プロファイルで維持する。
- CCTO固定枯死温度は**CCTO導入時のAMJ互換値**である。AMJ CoreはCCTOを内部実装として複製せず、CCTOの `ColdToleranceExtension` を条件付き互換Patchから利用する。小麦はCCTOのMO 1.6バランス（最低成長0℃・固定枯死-6℃）をそのまま使う。
- 2026-10-04の再監査では、低温で成長できる性質と凍霜害への耐性を分離して評価した。キビは-3℃を維持、アワは-4℃から-3℃、ヒエは-4℃から-2℃へ改定した。詳細な根拠・確度は `Docs/Balance/Crops/ColdTolerance.md` を正本とする。
- CCTOの固定枯死判定は閾値未満（strict `<`）で発生するため、表の温度ちょうどでは生存する。

#### 穀物の保存期間・一次加工

Stage Aの穀物は、加工前後の保存性も作物選択と備蓄判断に使う。共通基準は **未脱穀穀束30日 → 殻付き穀粒120日 → 可食穀粒90日 → 粉60日** とし、ソバと小麦に下記の例外を持たせる。束は早期脱穀が必要で、籾などの殻付き穀粒は長期備蓄に向く。日数は乾燥と気候差を省略したゲーム内バランス値であり、現実の保存期間を一律に断定するものではない。

| 作物 | 一次加工経路 | 保存期間 | 収穫直後の生食 |
|---|---|---|---|
| アワ / ヒエ / キビ | 雑穀束 → 脱穀 → 殻付き雑穀 → 殻取り → 雑穀 | 30日 → 120日 → 90日 | 不可。可食化は殻取り後 |
| 大麦 | 穀束 → 脱穀 → 殻付き大麦 → 殻取り → 大麦穀粒 | 30日 → 120日 → 90日 | 不可。可食化は殻取り後 |
| 小麦（MO） | 小麦束 → 脱穀 → 小麦穀粒 + `DankPyon_Straw` → 必要に応じてMO製粉 | 30日 → 90日。小麦粉は60日 | 不可。小麦は殻付き中間状態を省略 |
| ソバ | 穀束 → 脱穀 → 殻付きソバ → 殻取り → ソバ穀粒 | 30日 → 120日 → 60日。実装済みの蕎麦粉は60日 | 不可。可食化は殻取り後 |

- 雑穀粉 `AMJC_MilletFlour` は実装済み。保存期間60日で、雑穀団子に使用する。
- 蕎麦粉 `AMJC_BuckwheatFlour` はそばがきとともに実装済み。保存期間60日。蕎麦切りは初期範囲外とする。
- 大麦粉は穀粉全般を扱う方針により本番ThingDef・製粉Recipeと最低限料理をソース実装済み（実機未検証）。既存の可食大麦穀粒・加工経路・栄養は変更しない。
- Straw副産物は製粉ではなく脱穀段階へ統一する。ただし `DankPyon_Straw` はMO併用時だけ出力し、BaseではStraw ThingDefを所有しない。
- **MO併用時は** `DankPyon_Plant_Wheat` / `DankPyon_RawWheat` を可視供給元として再利用し、`DankPyon_RawWheat` を未脱穀の小麦束として `DankPyon_Cereal` から外す。脱穀後の `AMJC_Wheat` だけをMO製粉へ接続する。BaseではGrains所有の小麦Plant/穀束を同じ `AMJC_Wheat` へ合流させる。
- MO 1.6の**上流原本**では小麦収穫時の `Plant_SecondaryDrop` と製粉Recipe3種でHayが生じる。GrainsのMO条件付き `MedievalOverhaul_StageA_Wheat.xml` は収穫時副産物拡張を除去し、`DankPyon_CraftFlour_Manual` / `DankPyon_CraftFlour` / `DankPyon_CraftFlourBulk` の `products/Hay` を削除する。したがって**Grains適用後の製粉成果物は小麦粉のみ**であり、`DankPyon_Straw` はAMJ脱穀Recipeでのみ生じる。上流MOのHay存在とGrains適用後の製粉仕様を混同しない。
- MO `DankPyon_RawWheat` は原本120日から、Base `AMJC_RawWheat` と同じ束30日へ条件付きPatchで変更する。脱穀後の `AMJC_Wheat` は90日、MO `DankPyon_Flour` は60日。

この確定表は「一種類の最適作物」を作らないための基準である。短期・痩せ地はソバ/キビ、標準～肥沃な畑で収穫回数を抑える雑穀はアワ、寒冷側はヒエ/大麦、肥沃地で長期高収量・粉食は小麦、という役割差を維持する。

### 4.2.2 共通の稲（Vanilla Rice）とV1の畑地栽培

GrainsではVanillaの `Plant_Rice` を削除・複製せず、**V1では畑地栽培向けに上書きし、将来は同じPlantを水田でも利用する**。旧XMLでは `RawRice` を直接収穫する実装上の近道があった。**収穫後の共通経路は `稲束（AMJC_RiceSheaf）→脱穀→籾（AMJC_RiceInHull）→籾摺り→RawRice（玄米）／AMJC_WhiteRice（白米）`。既存のVanilla `RawRice` のDefNameと料理参照を残して互換性を守る。**** 実装前の確定・未確定条件は [RicePostHarvestProcessing.md](Balance/Crops/RicePostHarvestProcessing.md) を参照する。

固定方針:
- `AMJC_UplandRice` や水稲専用PlantDefは作らないが、**可食米は玄米 `RawRice` と別の白米 `AMJC_WhiteRice`** に分ける。稲束・籾の非可食中間物2件は共通加工経路として維持する。
- **V1から** `Plant_Rice` の表示名を日本語「稲」・英語「rice plant」に統一する。ゲーム内説明には畑の陸稲と水田の水稲、同種ながら適応品種群が異なること、AMJでは1つの稲に抽象化することを記す。**ゲーム内説明にV1などの版数、未実装の水田機能、別Modへの機能分担は記載しない。** V1の栽培条件は畑地基準のままで、水田機能はGrainsに追加しない。採用済みの画像・栽培値・加工経路は変更しない。表示本文・出典の正本は [LocalizationHistoricalReview.md §7–8](LocalizationHistoricalReview.md)。
- 史実説明は工程ごとに割り当てる。稲Plantは栽培史、稲束は古代・中世の租税、籾は籾摺り・精白によるビタミンB1の減少を説明する。`RawRice` は玄米と低精白米を代表し、`AMJC_WhiteRice` は精白工程と身分による利用の差を説明する。江戸患い・近代栄養学の経緯は参考史実であり病気のゲーム実装を意味しない。史料・日英文言の正本は [LocalizationHistoricalReview.md §7–8/§10](LocalizationHistoricalReview.md)。
- Vanillaの `Hydroponic` sowTagを除き、`Ground` のみとする。実ゲームの播種可否は別途検証する。
- `RawRice` は籾摺り**後**の玄米として再利用する。`AMJC_WhiteRice` は追加の精白加工と歩留まり損失を必要とする。陸稲・水稲、うるち・もちの違いでさらにPlantや米Defを分割しない。現行では収穫先とRecipe・E2Eのソースを二段階加工へ更新済み（実ゲームテストは未実行）。
- CCTO導入時はCCTOが既に `Plant_Rice` へ設定する最低生育10℃・固定枯死-1℃を利用し、Grains側に重複したColdToleranceExtensionを追加しない。
- Vanilla Riceの現行3日成長・収量6は採用せず、初期実装値を **growDays 5、harvestYield 11、fertilityMin 0.7、fertilitySensitivity 0.8、成長10～42℃・最適18～32℃** とする。既存六穀との七穀27セル分析で全作物の勝利条件と単一作物の支配防止を確認し、実機挙動は未検証とする。
- **2026-10-19新方針:** 米粉は中世の庶民の日常穀粉として必須とする根拠がないため、Grainsの通常製粉から除外。米粉アイテム・団子・レシピの古いDefName/原画は既存セーブへの配慮として残すが、加工／調理Billを設備へ登録せず、共通粉食の原料にも使わない。
- **旧・米粉の品種統合（2026-10-13、2026-10-19廃止）:** かつてはうるち／もち米粉をひとつの `AMJC_RiceFlour` として製粉する想定だった。現在はどちらの米も製粉の新規生産に使わない。将来の菓子・儀礼食は当該料理に必要な史料とゲーム上の用途が確定した場合のみ別途検討する。

**2026-10-13 作者確定の共通稲方針:** うるち米・もち米を分けないのと同様に、陸稲・水稲も独立したPlantDefに分けない。畑・水田の違いは同一 `Plant_Rice` に対する水管理・栽培環境・生育速度・収量で表現する。現実の品種や乾湿適性が同じという主張ではない。水田対応はRice Cultivationの責務とし、Grains併用時は畑・水田とも `AMJC_RiceSheaf → AMJC_RiceInHull → RawRice` を共用する。Vanillaの肥沃度は主に生育速度へ作用するため、株当たり収量や生育期間の水分履歴を反映するなら別途ロジックが必要。数値・干ばつ判定・田植えJob等はVEと非VEの先行Mod比較と実機検証を経てRice Cultivation側で確定する。Rice Cultivation単独時はVanilla `Plant_Rice` / `RawRice` を使い、Grains固有の中間物に無条件依存しない食料経路を持つ。Waterworksは任意の水利基盤のままとする。

### 4.3 低肥沃度環境との接続

自然に生成される土壌品質・TerrainDef・マップ上の分布は、独立姉妹Mod **Ancient & Medieval Japan: Environment** の責務とする。Coreは自然Terrainを重複定義せず、**作物側の `fertilityMin` と `fertilitySensitivity` を所有する**。

Environment側の正本は `sucRo-RimWorld/Ancient-Medieval-Japan-Environment:Docs/Design.md §10 Natural soil fertility`。AlphaではVanilla/MOの既存地形を再利用しつつ、Environmentが耕作可能な `AMJ_ThinSoil`（fertility 0.50）を追加する。0.40の第二の極端な痩せ地TerrainはAlphaでは追加しない。

Core側では0.50を代表的な低肥沃度テスト点として扱う。Stage A確定値では:

| 作物 | fertilityMin | fertilitySensitivity | fertility 0.50で播種 | 肥沃度成長倍率 |
|---|---:|---:|---|---:|
| ソバ | 0.4 | 0.25 | 可 | 87.5% |
| キビ | 0.5 | 0.3 | 可 | 85% |
| アワ | 0.5 | 0.4 | 可 | 80% |
| ヒエ | 0.5 | 0.5 | 可 | 75% |
| 大麦 | 0.5 | 0.6 | 可 | 70% |
| 小麦（MO） | 0.7 | 0.9 | **不可** | — |

これにより、Environment併用時はThin Soil上で小麦を外しつつ、短期・低投入のソバ/キビほど減速が小さいというStage Aの土地適性がそのまま表れる。

ソバの `fertilityMin=0.4` は、Environmentへ0.40 Terrain追加を要求する値ではない。外部Terrain Mod・将来の土地設計との互換余地、および作物自身の最低条件として保持する。Environment側で0.40帯を追加するのは、0.50/0.70の既存段階では不足する具体的なゲームプレイ上の理由が確認された場合だけとする。

現行のCore + MOプロファイルでも穀物農業・一次加工は成立し、VanillaのGravel 0.70でも肥沃度感応度差は働く。GrainsのVanillaプロファイルでも同じ土地適性原則を維持する。EnvironmentはGrainsの必須依存ではなく、**自然地形分布によって土地選択をより強く表現する推奨姉妹Mod**と位置づける。

### 4.3.1 肥沃度の収穫量化・高肥沃度互換（2026-10-19 200%上限確定／数値・実装未確定）

**作者が承認した方向性・未実装:** 肥沃度が生育日数を左右する現行Vanillaモデルから、原則として **生育速度への肥沃度補正を外し、土壌条件による差を1回の収穫量に反映する**方式へ移行する。作物ごとに肥沃度への**収量**反応を持たせ、Nice Plants Menuでも「基礎収穫量 × 土壌肥沃度係数」の予想収穫数を確認できるようにする。将来的にCCTOと同様の独立した収量Mod／共通機構へ分離できる所有境界を保つ。**現行のPlantDef・肥沃度感応度はまだ変更しない。係数・成熟日数・収穫基準値は未確定。** 既存の七穀27セルと `Millet_Cultivation_Balance.md` の比較値は**従来の生育速度補正が働く現行実装の値**であり、新方式の検証結果ではない。

**作者確定・2026-10-19：肥沃度由来の収穫量係数の参照範囲は0～2.00（200%）とし、200%を超える実効肥沃度でも収穫量係数は200%時点の値で頭打ちとする。** 土壌Defの肥沃度そのもの、MO耕地、DBHの灌漑・肥料状態、他Modの肥沃度表示や作物の播種判定は2.00に書き換えない。140%で打ち切らず、MO耕地125%・Rich Soil140%・灌漑/施肥の重ね掛け～200%までの増収余地を維持する。**200%超の超過分に基づく追加の増収はない**。係数関数の中間勾配・200%時点の最大倍率・端数処理は未確定。

高肥沃度の互換対象は次のとおり。

| 対象 | 既知の供給元・契約 | 新収量方式で守ること |
|---|---|---|
| Environment `AMJ_ThinSoil` | 自然肥沃度0.50。農業ゾーン設置修正はEnvironment所有 | 肥沃度不足で減収しても適応作物を育てられる。各Plantの `fertilityMin` を勝手に下げない |
| Vanilla `Soil` / `SoilRich` | 1.00 / 1.40 | 1.00を各作物の収穫基準とし、1.40には明確な増収余地を持たせる |
| MO `DankPyon_PlowedSoil` | MO 1.6原本XMLの恒常的な肥沃度 **1.25**。耕す行為・地形更新はMO所有 | 収量係数に1.25を反映する。GrainsはMOの耕地Def、建築・研究・耕すJobを書き換えない |
| DBH灌漑 | DBHの灌漑網による**時間的に減衰する**一時的な肥沃度上昇。効果量はDBH設定にも依存する | 生育速度だけから肥沃度効果を外しても灌漑を無効化しない。**収穫時刻の瞬間値だけ**で収量を決めず、栽培期間中の受益を評価する |
| DBHバイオ肥料 | DBHが管理する有効期限付きの土壌改良。強さ・期間はDBH設定に依存する | 灌漑・MO耕地との重なりを認める。効果の重複計上、失効後の残存を防止する |
| その他の高肥沃度設備（例: Vanilla水耕栽培2.80） | 肥沃度が2.00を超え得る上限ケース。稲は現行Ground専用など作物の植え付け制限は別 | 収穫量係数の計算に使用する肥沃度だけを2.00に制限。播種・生育・設備の既存制約と高肥沃度表示は変更しない |

**計算の原則（上限部分は確定、作物別係数は候補）:** `収量計算用肥沃度 = min(実効肥沃度, 2.00)`、`最終収穫数 = 基準収穫数 × 作物別収量係数(収量計算用肥沃度)`。肥沃度100%を基準値1.0として作物ごとの反応曲線を設計する。実効肥沃度は、MOの恒常的な地形値・DBHの一時的な灌漑・DBHの肥料・ほかのModの補正が**実際に反映された状態**を参照する。DBHの湿潤/施肥グリッドが `map.fertilityGrid.FertilityAt` に反映済みか、別の生育補正なのかは現行1.6/1.7のロード後実装で確認する。**既に合算済みの値へ灌漑効果を再加算しない。** 時間変化するDBHの灌漑・施肥では**各時点の実効肥沃度を200%で上限制限してから栽培期間全体の受益を集計**するのを原則とし、収穫直前だけ一時的に増えた効果や200%超のピークで収量が跳ねないようにする。観測・履歴保存・サンプリングの具体的手法は低TPS負荷とセーブ互換を前提に後で決める。

**Nice Plants Menu:** 既存の基準収穫量・予測生育日数を維持し、同Modが有効な場合のみ収量計算を拡張する案を検討。**収量表示でも実収穫処理と同じ200%上限を必ず用い、200%と280%で同じ作物別収量係数を返す。**混合肥沃度のゾーンは、単一の平均肥沃度から概算するのではなく、可能なら**セルごとの予想収穫数を合計**して表示する。灌漑の時間変動を含む場合は未来の持続を保証せず予測値だと明記する。Nice Plants Menuの実DLLと表示APIはまだ確認できていないため、互換実装を完了扱いにしない。

**関連所有者・境界:** Grainsは七穀と料理・加工の基礎数値、将来の独立収量Modは収量係数の実装、MOは耕作地形、DBHは灌漑・肥料、Environmentは自然土壌、Waterworksは任意のDBH水供給経路、Rice Cultivationは共通稲の水田効果を所有する。AMJ内でMO/DBHの機能を複製しない。未導入時にGrainsの農業ループが成立すること、既存Crop Growth Balanceの `growDays` 所有境界、CCTOの低温枯死・休眠の責務、保存期間30→120日の加工メリットを維持する。

**実装前の留保:** 先行Mod（VE/非VE）、既存 `Plant.YieldNow()` 系Modとの重複とNice Plants Menu互換を監査し、対象7作物の生育日数・基準収穫数・栄養・加工労働・MO＋DBHの重ね掛けを**肥沃度50/70/100/125/140/160/180/200/280%**の代表点で一括再計算する。**280%の収量係数は200%と同じであること**を確認する。現在の収量コード・DLL・Workshop公開状態は変更していない。

### 4.4 ワールド地形・Hillinessの責務

ワールドHilliness、標高、気候、河川、海岸線、およびそれらに連動する自然土壌分布は **Ancient & Medieval Japan: Environment** が所有する。Coreはワールド生成Patch・Hilliness補正・自然土壌GenStepを持たない。

以前のCore案にあった「Flat→Small Hills等を一定確率で昇格する」「Hillinessに応じてCore自身が痩せ地を生成する」という設計は廃止し、Environmentの実装・検証済み世界生成へ一本化する。具体的なHilliness比率、地形分布、生成アルゴリズム、検証値はEnvironmentリポジトリの `Docs/Design.md` を唯一の正本とし、Core側へ複製しない。

Coreが保持するのは、生成された土地条件に対して各作物がどう反応するかという農業バランスである。


---

## 5. 最初の公開Alphaの一次加工

完成料理を大量追加しない。

最初の公開Alphaで必要なのは、作物を既存の食事体系へ自然に流すための最小限の加工。

### 5.1 基本方針

- BaseはMO設備なしで穀束→可食穀粒→粉まで自己完結する。
- 脱穀・殻取りは既存AMJC穀物加工設備を維持し、V1はVanilla・MO双方で燃料式／電気式調理台も利用する。
- 製粉はGrains単体用の手動石臼Defを維持し、MO併用時はMO Millstoneを使用する。V1の非MO経路は、石臼に加えVanillaの燃料式・電気式調理台でも製粉できる。
- 粉食Recipeは研究不要とし、専用上位料理設備を要求しない。
- Baseの主要経路にMO ResearchDef / ThingDef / StuffCategory / ThingCategoryを直接参照しない。
- MO併用時だけ同等資産を条件付き互換から再利用する。
- 同一プロファイルに同目的のAMJ石臼とMO石臼、AMJ小麦とMO小麦、AMJ小麦粉とMO小麦粉を標準経路として並存させない。

### 5.2 Grainsで追加する一次加工の考え方

Baseの穀類フロー:
- アワ・ヒエ・キビ: 穀束 → 脱穀 → 殻付き雑穀 → 殻取り → 雑穀 → 雑穀粉
- ソバ: 穀束 → 脱穀 → 殻付きソバ → 殻取り → ソバ穀粒 → 蕎麦粉
- 大麦: 穀束 → 脱穀 → 殻付き大麦 → 殻取り → 大麦穀粒 → 大麦粉 → 麦粉粥（追加分は実ゲーム未検証）
- 米（陸稲）: 稲束 → 脱穀 → 籾 → 玄米（Vanilla `RawRice`）または精白米（`AMJC_WhiteRice`）→ 飯・粥。米粉の製造は行わない。
- 小麦: 小麦束 → 脱穀 → AMJC_Wheat → 小麦粉

Baseでは脱穀時のStrawをThingDef化しない。MO併用時のみ上記脱穀Recipeへ DankPyon_Straw を副産物として追加する。

Grainsの現行製粉対象は**雑穀粉・蕎麦粉・大麦粉・小麦粉の4種**。米粉のThingDef/製粉Recipeと米粉団子は歴史的な制作成果としてDefNameを残すが、新規製粉Bill・米粉団子調理Billは設備登録せず通常生産できない。米粉は共通粉食の材料からも除外し、米の玄米・白米・飯・粥の供給は維持する。

**米粉の非生産方針（2026-10-19）：** 単一 `AMJC_RiceFlour` は既存セーブ・原画参照の保護のために残すのみ。玄米粉の中世一般食材としての存在を推定で必須工程にしない。うるち／もち米粉の別Defも追加しない。

可食穀粒→粉の製粉では総栄養を原則保存し、製粉そのもので食料を増加させない。粉を作れるだけの状態にはせず、§5.3の最低限粉食へ接続する。

**2026-10-13 五粉実装の履歴（2026-10-19に米粉のみ方針変更）：** 当時は5種の製粉Recipeと米粉団子まで用意したが、現在の有効な4穀粉は小麦・蕎麦・雑穀・大麦。米粉生産および米粉団子の通常Billは休止済みで、史料根拠が十分な料理が必要になるまで復活させない。

#### 穀物一次加工設備

**Grains V1の暫定設備とV1後の専用化（2026-10-13）**

- **V1脱穀・殻取り:** 雑穀・ソバ・大麦・米の単品／10個一括、および小麦の単品／10個一括は、既存AMJ加工設備に加えてVanillaの燃料式・電気式調理台で実施する。MOの有無を問わず適用する（PR #66でmainに反映済み）。
- **V1製粉:** MOなしは既存AMJ手動石臼Def・WorkGiverを保存し、全5種の製粉RecipeをVanillaの燃料式・電気式調理台にも登録する。MOありでは4種のAMJ製粉Recipeの設備一覧をMO石臼のみに条件付き置換し、MO所有の小麦製粉経路も従来どおり維持する。MO構成には調理台製粉を追加しない。
- **V1後:** 本来想定していた加工場所・穀物加工台・石臼の画像と役割を順次整え、工程別の設備・速度の振り分けを再評価する。専用設備の種類・正式な割当・移行バージョンは実プレイと別途設計で決定する。V1のためだけに追加の専用作業台、独立WorkGiver、重複レシピを増やさない。
- **移行の条件:** 後の設備変更時はRecipeDef.recipeUsersとWorkGiver_DoBillの割当、既存Bill／DefName／セーブ互換、MO条件付き分岐、Recipe入力・出力・副産物と実ジョブを一体で監査する。暫定設備の撤去を自動的・一括には行わない。
- **現行V1製粉の固定数値・検証:** 各Recipeは入力10→粉10、workAmount 300、作業Skill Cooking、研究不要、栄養保存、副産物なし。実ゲームでは非MOで燃料式調理台から製粉Billを発行してWorkGiver_DoBill経由で完了させ、MOでは石臼以外の製粉設備を追加しない。四構成の実機検証が完了するまでは実動作確認済みと記載しない。
- **簡易穀物加工場所 / AMJC_GrainProcessingSpot:** 初期から使える低速の脱穀・殻取り設備。
- **穀物加工台 / AMJC_GrainProcessingTable:** 同じ脱穀・殻取りRecipeを高速に処理する上位設備。BaseではMO研究・MO素材を要求しない。
- **Grains石臼:** Baseで小麦・ソバ・雑穀・大麦・米の五種類を製粉するAMJ所有設備。最低限の製粉経路はMO研究なしで成立させる。step 3初期実装は `AMJC_ManualMillstone`、BlocksGranite 30 + WoodLog 20、WorkToBuild 500。設備・製粉Recipeに研究前提は置かないが、石材の入手はVanillaの採掘・加工・交易等に依存し、全マップで初日建設できることは保証しない。
- MO併用時は DankPyon_Millstone を標準石臼として使い、Grains手動石臼は重複表示しない。
- MOのCraftingSpot手挽き等を残す場合も、未脱穀穀束から直接粉へ飛ばないようGrains工程順を維持する。
- x10等のBulk Recipeは単品処理より作業量を減らし、大量処理時のBill/Job負荷を抑える。
- Recipeの作業SkillはBase/MOともCookingに統一する。既存の材料・出力・設備・Billは維持する。

### 5.3 完成料理

**2026-10-19 米の精白・料理分化（作者採用、実ゲーム未検証）**

- 玄米はVanilla `RawRice`を再利用（旧DefName/旧Bill/加工1:1を保護）。籾10→玄米10はworkAmount 80、籾10→白米8はworkAmount 240の二択Bill。白米単品の端数レシピは設けず、10個単位の加工で数量減を表現する。白米栄養は1個0.05とし、8個分の栄養に減る。白米を作ると米が増える経路を作らない。
- 共通の飯・粥・粉食を削除しない。米飯・白飯・米粥・白粥を材料限定の指定料理として追加し、単品と4食Billを備える。玄米側の米飯/米粥は共通飯・粥より調理workを軽くする（220<240、180<200）。白飯は食後Mood +3/0.5日、白粥は+1/0.5日を仮採用し、実プレイで再評価する。白飯と白粥以外はこの指定料理によるMoodバフを持たない。
- 白米は余りを共通飯・粥にも利用できる。米粉の通常製粉は2026-10-19に停止したため、玄米も白米も米粉へ変換しない。専用の白米/米飯/白飯/粥画像は未承認であり、既存米・共通料理の画像を暫定利用する。
- 裕福な出自を理由に全員が玄米・雑穀食を嫌うとは決めない。食事への明確な期待が書かれた一部背景への条件付き心情は、Culture Characters側で背景と特性の整合を先に検討し、Grains本体は背景名へ直接依存しない。
- **既存Mod比較（VE/非VEの実装入口）:** VEのVanilla Cooking ExpandedとVanilla Plants Expanded More Plantsは粉・作物・料理の追加に先行例があるが、本件の玄米/白米を同一籾から異なる歩留まり・労力で選択するBillを既存のGrains/Vanilla互換へ提供しない。非VEのMedieval Overhaulには独自の穀物処理・料理があるが、GrainsのVanilla `RawRice` を維持したこの二択経路を代替しない。調査範囲は既存の本Repo VE/MO調査と公開説明の確認に限り、他の全Modとの競合不存在を断定しない。独立の新たな精米Frameworkは導入せず、既存AMJ加工台のBillとして最小差分にとどめる。


**2026-10-19 共通料理の画像（実ゲーム確認待ち）:** 作者は飯 `AMJC_MixedGrainMeal`、粥 `AMJC_MixedGrainPorridge`、粉食 `AMJC_MixedFlourFood` の生成画像3点を、通常ズームでの実ゲーム確認用として暫定採用。原画は `Art/Sources/Things/Item/Meal/<DefName>/<DefName>.png` に無加工で保存し、対応する256×256透過PNGは `Textures/Things/Item/Meal/<DefName>/<DefName>_a,b,c.png` に同一内容で書き出す。各ThingDefの`graphicData/texPath`のみ固有パスへ切替え、`Graphic_StackCount`、DefName、Recipe、素材条件、栄養、心情、保存期間は維持。実ロード時のBadTex・通常ズームの見た目・3品の識別性は未確認。確認前に公開・正式画像完成と主張しない。


**2026-10-13 五粉の食用途の履歴（2026-10-19修正）：** 麦粉粥は現行生産を維持。米粉団子は当時採用したが、米粉の必須性が不十分なため現在は製粉・調理の新規Billを停止し、既存DefName・画像だけを互換用に保管する。史料に基づく菓子・儀礼食の追加は独立の判断とし、既存の米粉団子を説明文だけで復活させない。

最初の公開Alphaでは専用料理を最小限にする。

**基本食材としてのVanilla簡単な食事（2026-10-08検証契約）:** アワ・ヒエ・キビは共有`AMJC_Millet`、ソバは`AMJC_Buckwheat`、大麦は`AMJC_Barley`、小麦は`AMJC_Wheat`、陸稲はVanilla `RawRice` として、Vanillaの `CookMealSimple` を使えることを保証する。静的な食材フィルタ受入だけで完了とせず、収穫→各穀物の必要な脱穀/殻取り→各食材を単独指定した実`CookMealSimple` Billの完了→所定量の原料消費と`MealSimple`生成まで、Vanilla/MO × CCTO有無の四構成でPickle検証する。共有雑穀Defに合流した後のアワ/ヒエ/キビは同一の料理食材として数え、料理Defを作物数分重複作成しない。これは専用粉食3品とは別の基本食材互換テストである。**実装したPickleステップ自体の成功はまだ実ゲーム未検証であり、公開前の通過条件として残す。**

優先順位:

1. 既存の「簡単な食事」等へ原料として利用可能
2. 既存料理ModのIngredientCategoryで利用可能
3. 歴史的・ゲーム的に必要な場合のみ専用料理を追加

#### Grains再編時の最低限の粉食

現CoreをGrainsへ再編して製粉まで自己完結させる場合、**粉を作るだけで食文化が西欧パンへ吸収されないよう、日本の古代～中世に根拠のある簡易粉食を少数だけ持つ。** 料理Mod化はせず、穀物の一次加工を食用へ接続するための最低限のRecipeに限定する。

- **餺飥（ほうとう系）** — 小麦粉を水で練って煮る前近代の粉食を代表させる。現代山梨の味噌・具材を固定した「ほうとう鍋」そのものではなく、平安期以来の餺飥系粉食をゲーム上1料理へ抽象化する。『和名類聚抄』には小麦粉を練って薄く延ばし、方形に切る記述がある。一方、実際の完成形や盛り付けは確定できないため、採用画像は推定表現として説明する。小麦は弥生期には存在し、鎌倉中期頃から稲の裏作としての栽培が進み、鎌倉～室町期には製粉具と小麦粉食の利用が拡大したため、中世小麦の主要用途として扱う。
- **そばがき** — 蕎麦粉を練って食べる簡易粉食。蕎麦切りを標準料理にせず、江戸以前のソバ利用として簡素な粉食を優先する。
- **雑穀団子** — 雑穀粉を水で練り、茹でる/蒸す類の粉食を1料理へ抽象化する。アワ団子・キビ団子等を別Recipeへ分割せず、共通雑穀粉の利用先とする。

小麦粉について、中世に存在した饅頭・うどん・素麺・麩等をすべてGrainsへ実装しない。饅頭は餡・菓子、麩は寺院/精進料理、麺類の高度化は追加の料理・加工体系へ広がるため、具体的なゲーム上の役割が生じた場合だけ後続機能で再検討する。

米粉のうるち／もちの統合案は過去のもの。2026-10-19から Grains の通常製粉・米粉団子のBillは有効化しない。将来の儀礼食・菓子は史料・用途・加工工程を独立に評価する。

**4食まとめ調理（2026-10-13 作者の続行指示に基づく追加）:** Grains粉食5品に、各単品Recipeを残したまま `AMJC_Cook*Bulk` の4食まとめ調理を追加する。Vanillaの `CookMealSimple_Bulk` と同じ数量・基本労働倍率を採用し、粉Nutrition 2.0（1食分0.5×4）→同一料理4個、調理workAmount 1200（単品300×4）とする。料理の総Nutritionは0.9×4=3.6。研究不要・追加スキル要件なし、Campfire / FueledStove / ElectricStove。単品の栄養・心情+2（食べた1人に付与、1個で4倍にならない）・2.5日保存・画像/Defを共有する。まとめ調理はBill実行回数と素材搬送等のオーバーヘッドを減らすもので、**調理Workや製粉Workの無料割引はしない**。まとまった粉がなければ単品料理を選択できる。

**先行実装の限定比較（2026-10-13）:** Vanilla既存の4食簡単料理は Nutrition 2.0→4個・作業量1200。VE Vanilla Cooking Expanded 1.6 の粉/製菓RecipeはVCE独自の粉に依存し、Grainsの穀粉5種を用いた既存5品のバッチ調理を代替しない。非VEの Bulk Meals と Bulk Cooking Recipes は一般料理のバッチ加工に先行例があるが、Grains各品の出力・MO条件付き小麦粉入力を維持する固有Recipeを供給することは確認されていない。新Frameworkや必須Modを追加せず、現在の5品にだけ同構造のRecipeを併置する限定差分とする。ゲーム内のBills/調理ジョブは実機Pickleの別ゲートとして確認する。

**V1から継続する共通料理と固有料理の性能差（2026-10-13 作者確定方針、実装前）:** 全七穀から飯・粥・粉食の**共通3系統**に接続し、**異なる可食穀粒を混ぜる飯・粥、および異なる穀粉を混ぜる粉食を、将来の料理分化後も恒久的な余剰穀物消費先として維持する**。共通料理は単一原料でも製造可能。特定原料・指定配合の固有料理は、対応する共通料理に対して**作業量・保存性・Mood・携行性・栄養効率等のうち実用上有意な性能優位**を持たせる。値だけでなく製粉や特殊設備の追加負担も考慮し、単なる別名のRecipeは作らない。共通料理に「腐る直前の在庫を自動で選ぶ」機能が備わるという意味ではない。材料の混合可否は設計確定、混合Billのロード実行と既存食事との優先関係は未検証。V1の作物差は栽培・加工・保存に残す。今の本番5粉食ThingDef/各x1・x4 Recipeは**まだ統合していない**。Vanilla簡単な食事と新たな「飯」の重複、既存Def/セーブ互換、MO小麦粉の条件付き原料指定は移行前に確認する。固有料理の具体名・数値・史料と比較テストは `Docs/Research/GrainMealRoadmap.md` に記載し、個別の料理・バランス案の採否は未承認とする。

**V1料理の表示名称:** 共通3料理の日本語は「飯」「粥」「粉食」、英語は `cooked grains`、`porridge`、`flour dish`。混合可能だが混合は必須ではないため、「混合」は料理名に用いない。単品・4食Bill名と日英説明もこれに統一する。内部DefName、原料条件、数値は維持。対応表と全文は `Docs/LocalizationHistoricalReview.md §16`。

**V1の食事評価基準（2026-10-13、作者指示でバニラ日本語名へ統一）:** 「美味しい食事」「普段の食事」を独自の食事等級名として用いず、**バニラの正式な料理名**「簡単な食事」「手の込んだ食事」「豪華な食事」を比較の基準とする。Grains料理のアイテム名そのものは「飯」「粥」「粉食」等のまま。下表のMoodは食後に追加される心情値を比較したもので、料理の等級・思考・必要スキルが同じという意味ではない。

| 基準・位置付け | 食後Mood | Grainsで該当する料理 | 解釈 |
|---|---:|---|---|
| バニラ「簡単な食事」(`MealSimple`) | 0 | 飯・粥・粉食（共通3品、Mood 0） | 気分上昇がない普段の穀物料理として同程度 |
| **簡単な食事と手の込んだ食事の中間**（バニラの正式等級ではない） | **+2** | 餺飥・そばがき・雑穀団子・麦粉粥（現行4品） | 特定の穀粉に応じた控えめな食後効果 |
| バニラ「手の込んだ食事」(`MealFine`) | +5 | 比較基準のみ | Grainsの指定5品を同一の上位食や同じ心情Thoughtとは見なさない |
| バニラ「豪華な食事」(`MealLavish`) | +12 | 比較基準のみ | Grains V1には相当する上位食を設けない |

共通料理は原料1種類でも複数種類でも調理できる。指定5品は各料理に対応する穀粉を要求し、食後Mood +2（0.5日、非累積）、腐敗2.5日（共通粉食2日）。**共通粉食と指定5品は調理Work300・完成栄養0.9が同じ**で、調理工程がVanillaの「手の込んだ食事」と同等に高度という意味ではない。飯・粥は製粉不要だが、共通粉食も指定5品も原料粉の製粉が必要。全8料理の入力栄養は0.5、完成栄養は0.9。**共通3品と指定5品という二群の整理であり、独自の「美味しい食事」等級を追加しない。** ゲーム内名称、DefName、Recipe、食品カテゴリ、栄養・調理Work・Mood・腐敗・研究・スキル、画像は変更しない。実際の食事優先順位は一括実機検証と実プレイで評価する。

**比較参考:** [RimWorld Wiki - Simple meal](https://rimworldwiki.com/wiki/Simple_meal)、[Fine meal](https://rimworldwiki.com/wiki/Fine_meal)、[Lavish meal](https://rimworldwiki.com/wiki/Lavish_meal)、[RimWorld Wiki* 日本語・食料](https://wikiwiki.jp/rimworld/%E9%A3%9F%E6%96%99)。バニラの「おいしい食事を食べた」は食後の思考の説明であり、料理名ではない。

**共通料理3品（2026-10-13 V1実装試行値）:** `AMJC_MixedGrainMeal` は可食穀粒5種を混合可能、粉/穀粒を混ぜず、1食の原料栄養0.5→料理0.9、調理Work240、常温腐敗3日。`AMJC_MixedGrainPorridge` は同原料、Work200、腐敗1.5日。`AMJC_MixedFlourFood` は対応する穀粉5種を混合可能、Work300、腐敗2日、Moodボーナスなし。共通料理は単一種類の原料からも作れる。各料理の4食Billは原料栄養2.0→4食、調理Work4倍。Vanilla簡単な食事は温存。既存の材料指定粉食5品はMood+2・腐敗2.5日を維持し、共通粉食より優位にする。Base小麦粉はMO併用時に `DankPyon_Flour` へ切替。全Recipe研究不要で既存調理設備を使う。既存5食品・Bill・画像を維持し、専用画像は後日。数値は実プレイ・四構成E2Eによって再調整する試行値であり、腐敗間近の素材を自動選別する機能はない。

**Grains粉食の解禁・MOパンとのバランス原則:**
- 餺飥・そばがき・雑穀団子は**研究不要**とする。粉を作れる状態になった時点で利用可能にし、料理そのものへ追加研究ゲートを置かない
- 石窯等の専用上位設備は要求せず、既存の簡易調理設備で作れる日常粉食として扱う
- 代わりに、MOパンのような大きな栄養増幅・長い保存性・高い加工価値を与えない
- **Grains粉食はVanillaの「簡単な食事」相当を基準にしつつ、製粉という追加工程への小さな報酬としてMood等をわずかに上乗せする。** 初期バランス基準は粉0.5栄養相当 → 完成0.9栄養相当、Mood +2程度とし、栄養効率そのものはVanilla簡単な食事を大きく上回らせない
- 保存性はVanilla簡単な食事より短め（初期目安2～3日）を基本とし、「作り置きに強い上位食」にはしない
- MO 1.6のパンは粉0.25栄養からパン2個（合計0.8栄養）、作業量350、石窯とOven研究を要求し、8日保存できる。Grains粉食はこの効率を直接模倣せず、**研究不要・低設備・短保存・標準的な食料効率**を基本に差別化する
- Grains側の進行ゲートが必要な場合は料理Recipeではなく、作物・製粉設備・製粉工程側へ置く。粉を得た後に「その粉を簡単に食べる」ための追加研究は要求しない
- 正確な個数、workAmount、保存期間、Mood値は自動バランステストで微調整するが、**製粉で総栄養を増やさないこと**と、**粉食をパン級の栄養増幅へしないこと**は固定方針とする

---

## 5.4 Grains食材の料理接続対応表

| 食材 | Grains Base | MO併用時 |
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

原則:
- Vanilla + Grainsだけでも「栽培できるが食べられない」「粉を作れるが用途がない」状態を作らない。
- アワ・ヒエ・キビは栽培中だけ別PlantDefとし、収穫後は共通雑穀チェーンへ統合する。
- 雑穀粉・蕎麦粉・大麦粉は小麦粉とは別ThingDefとし、原料と加工経路を維持する。米粉の旧Defは互換用に残し、新規生産・共通粉食への接続は行わない。
- 水田・水稲栽培はGrains外。陸稲とVanilla `RawRice` の穀物側接続はGrainsが扱う。豆・根菜・繊維はGrains所有ではない。
- MOカテゴリへの登録は、具体的なMO Recipeへ参加させたい場合だけ条件付きで行う。

## 6. MO併用プロファイル設計

この章は**MOを導入した場合の公式統合仕様**を定める。現行公開CoreではMOがまだ必須だが、依存監査・分離後はこの章を条件付きMO互換として維持する。

### 6.1 基本方針

MO併用時は、Grains Baseと同じ目的の設備・素材を二重に並べず、**MO資産を実Def供給元として差し替える公式互換**を行う。

- 小麦Plant/穀束: MO DankPyon_Plant_Wheat / DankPyon_RawWheat
- 脱穀後小麦: Grains AMJC_Wheat
- 小麦粉: MO DankPyon_Flour
- 石臼: MO DankPyon_Millstone
- Straw: MO DankPyon_Straw
- ソバ・大麦・雑穀および蕎麦粉・雑穀粉・大麦粉: Grains所有。米粉は通常の加工対象から除外し、旧DefNameのみGrainsが維持する。

MO資産を使っても、Grainsの工程順・七穀（陸稲を含む）のバランス・粉食バランスは維持する。MO固有PatchはMO存在時だけ適用し、Base XMLからMO DefNameを参照しない。

### 6.2 MO研究ツリーとの統合

Grains Baseの主要ループはMO ResearchDefを必要としない。MO併用時の研究接続は追加の進行統合として条件付きで行う。

- Baseで栽培・脱穀・殻取り・最低限の手動製粉がMO研究なしで成立することを優先する。
- MO小麦やMO石臼に既存研究前提がある場合、Grains主要ループを止めるかを監査し、必要ならMO互換Patchで前提を調整する。
- 研究Tierを作物性能の帳尻合わせに使わない。
- MO全研究ツリーの全面改変はGrainsの責務にしない。
- 繊維・製紙・水利等、Grains外の研究統合は各所有Modが担当する。

### 6.3 Rice Cultivationとの所有境界

Grainsは**共通の稲Plantと穀物の収穫後加工・製粉**を所有し、将来のRice Cultivationは**水田・水管理・同一Plantの水田栽培への拡張**を所有する。Rice Cultivationの詳細設計は専用所有先ができるまではProject側を正本とし、本書ではGrainsとの接続境界だけを固定する。

- GrainsはVanilla `Plant_Rice` をV1の畑地栽培に合わせて調整し、表示名を「稲」とする。稲束→籾→Vanilla `RawRice` の収穫後加工を共通所有する（収穫先とRecipeは本番XMLへ実装済み、実ゲーム未検証）。
- 水田Terrain、水深・給排水、苗代・田植え、同一 `Plant_Rice` への水田生育・収量補正等の水田固有ロジックはGrainsに入れない。
- Rice Cultivation + Grainsでは、畑・水田とも同一 `Plant_Rice` からGrainsの稲束・籾の共通加工経路へ接続し、脱穀・籾摺りを迂回しない。水稲専用Plant、米・米粉・精米設備・製粉RecipeをRice Cultivation側で重複定義しない。
- GrainsはRice Cultivationを必須依存にしない。Rice Cultivation単独時の簡易収穫経路と、稲架掛け・藁・保存差等の水稲固有設計はRice Cultivation側で決める。
- WaterworksはRice Cultivationへ任意に水を供給する基盤であり、Grainsの陸稲・製粉ループには介入しない。

### 6.4 Def・カテゴリの所有方針

GrainsはBase主要ループに必要なDefを自分で所有し、MO併用時だけ同等資産の供給元を条件付きでMOへ差し替える。

**Grainsが常時所有**
- アワ・ヒエ・キビ・ソバ・大麦
- Vanilla `Plant_Rice` の陸稲化Patchと、`RawRice` への穀物側接続
- 各穀束・殻付き中間物・可食穀粒
- AMJC_Wheat
- 雑穀粉・蕎麦粉・大麦粉（米粉は通常生産から除外）
- 脱穀・殻取りRecipe
- Base用製粉Recipe・最低限粉食Recipe
- Base用穀物加工設備
- 各穀物の栽培・保存・加工バランス

**Base用フォールバック**
- 非MO小麦Plant / 小麦束
- 非MO小麦粉
- 手動石臼

**MO併用時の供給元**
- DankPyon_Plant_Wheat / DankPyon_RawWheat
- DankPyon_Flour
- DankPyon_Millstone
- DankPyon_Straw
- 必要なMO研究・素材・カテゴリ接続

**Grainsが所有しない**
- AMJ独自Straw
- 水田・水管理・同一 `Plant_Rice` の水田栽培条件（稲Plant本体と `RawRice`、共通の穀物加工はGrainsが担当する）
- 豆類
- 繊維・紡績・紙
- 根菜・一般野菜
- 塩・粘土等、Grains主要ループ外の横断資源

### 6.5 MO 1.6基盤資産の所有監査

| 分野 | MO側の正本 | Grains側の扱い |
|---|---|---|
| 小麦 | DankPyon_Plant_Wheat / DankPyon_RawWheat | MO時の可視供給元。GrainsバランスPatchを適用 |
| 小麦粉 | DankPyon_Flour | MO時の唯一の標準小麦粉 |
| 製粉設備 | `DankPyon_Millstone` | MO時に石臼と既存の製粉Recipeを再利用する。CraftingSpot・風車・水車による別経路は現行Grainsの正式互換対象ではなく、未脱穀束から工程を飛ばさない |
| 藁 | DankPyon_Straw | MO時だけ脱穀副産物として利用 |
| 穀物カテゴリ | DankPyon_Cereal | MO時も `AMJC_Grains` へ保管表示を統合。旧カテゴリDefは空の参照として残し、MO製粉・醸造の入力は `AMJC_Wheat` 直接指定へ移行 |
| 農業研究 | DankPyon_BasicAgriculture 等 | Grains主要ループの必須依存にはしない |

#### DankPyon_Cereal は汎用穀物カテゴリとして使わない

MO 1.6の `DankPyon_Cereal` は製粉Recipe、Ale wort Recipe、風車・水車のProcessor Framework入力に使われる旧材料カテゴリであり、Grains併用時は標準アイテムを所属させず空の互換Defとして残す。保管は `AMJC_Grains` へ一本化し、加工入力は `AMJC_Wheat` を直接指定する。

- `DankPyon_RawWheat` は未脱穀小麦束として `AMJC_GrainSheaves` へ入れる。MO製粉へ直接投入しない。
- 脱穀後の `AMJC_Wheat` は `AMJC_GrainKernels` に単独所属させ、MO製粉・醸造のレシピ/Processor FrameworkフィルタからThingDef名で許可する。
- 雑穀・ソバ・大麦は従来どおり専用粉経路を持ち、MOの汎用製粉・醸造に自動追加しない。
- Strawの発生点はカテゴリではなく脱穀Recipeで制御する。第三者の旧カテゴリ依存は実機未検証とする。

#### MO紙・紡績等はGrains外

旧Core案にあった麻・カラムシ、MO Paper、Spinning Wheel、Paper Press等の統合は、Grains再編後はGrainsの責務ではない。設計知見は将来の繊維/Materials系機能の参考として保持してよいが、Grains BaseにもGrains-MO互換にも実装しない。

## 7. Food Drying 連携

Food Dryingは**必須依存にしない**。

ただし思想が合っているため、積極的に互換する。

### 屋根下乾燥の方針

**AMJ側で汎用の屋根下乾燥設備を新規追加する計画はいったん外す。**

- 一般食品の乾燥はFood Dryingへ任意互換する
- 稲架掛け等の稲作固有の任意工程はRice Cultivation側で別途設計する。MO既存Drying Rackは必須前提にしない
- 「屋根下でも使える日本式乾燥設備」という外観・利便性だけを理由に、既存乾燥システムと重複するBuildingを追加しない
- 将来、特定のAMJ生産物に既存設備では表現できない乾燥条件やゲーム上の選択が生じた場合だけ、その機能を所有するAddon側で専用設備を再検討する
### Grains側

- 乾燥システムを重複実装しない
- Food Dryingは `paseri.FoodDrying` の任意互換対象として扱い、存在確認付きPatchだけを使う
- 互換は**意味の合う乾燥先が存在する食材単位**で追加し、別作物の乾燥品へ便宜的に変換しない
- Stage Aの雑穀・大麦・小麦・ソバは、脱穀・殻取り後の穀粒自体が乾燥保存食であり、現行Food DryingのDried Rice等へ変換する利益より食材同一性の破壊が大きいため、公開Alphaでは互換対象外とする
- Food Dryingを導入しなくてもVanilla + Grainsの穀物農業は成立する

Food Drying 1.6はProcessor Frameworkを利用し、米・ジャガイモ・トウモロコシ・果物・きのこ等に専用の乾燥品を持ち、乾燥品から元食材へ戻す再水和経路も持つ。このため互換では入出力の意味を一致させることを必須条件とする。将来、山菜・きのこ・果実・根菜等を追加した段階で互換価値が大きくなる。

---

## 8. 最初の公開Alphaでは実装しないもの

### 8.1 水田・水稲栽培（Rice Cultivationへ分離）

**Grainsは畑・水田で共通する稲Plantと穀物共通の収穫後加工・製粉を担当する。** 現行実装ではVanilla `Plant_Rice` を陸稲として再定義し、稲束→籾→`RawRice` の必須加工を本番XMLへ導入した。同一の穀物加工設備に接続し、公開Alpha前に実機テストを必要とする。共通稲Plantや米加工は水田Modへ移管しない。

**Rice Cultivationは水田・給排水・苗代・田植え・共通稲Plantの水田栽培条件を扱う。** Grains併用時は同一 `Plant_Rice` の畑・水田双方からGrainsの**必須加工経路（稲束・籾）**へ合流させ、可食米・精米・製粉の共通経路を二重実装しない。稲架掛け・稲藁等の水稲固有工程はRice Cultivationが独立に設計する。

Waterworksは自然取水・用水路・分水・暗渠・引湯等の水利基盤を持ち、Rice Cultivationとは任意連携する。Grainsは両者に必須依存しない。詳細仕様は各所有リポジトリを正本とし、Grainsではこの接続境界だけ固定する。

#### Waterworks / 水利Modの基本方針

WaterworksはGrainsとは独立した水利Modであり、正式な詳細設計は `sucRo-RimWorld/Ancient-Medieval-Japan-Waterworks:Docs/Design.md` を正本とする。Grainsは水田・水利を所有しない。

現行v1の最小境界:
- プレイヤーが素掘り水路を掘る
- 水路の連結成分が川・池等の有効な自然淡水へ上下左右で直接接していれば通水する
- 通水時は濡れた水路、非通水時は乾いた溝として表示する
- 水路は埋め戻せる
- 別個の取水口Building、水門、暗渠、温泉水区分、DBH Adapter等はv1必須ではなく、実際の利用先が必要とした段階でWaterworks側が再評価する
- Rice CultivationはWaterworksを必須依存にせず、併用時のみ任意接続する

### 8.2 大豆

大豆はGrains外。将来の所有先・発酵用途・既存大豆Modとの比較はProjectの [DeferredPlantsAndProcessing.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/DeferredPlantsAndProcessing.md) と [ExistingModAudit.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/ExistingModAudit.md) を正本とする。
### 8.3 発酵食品

Grains外。詳細候補はProjectの [FermentationBrewingPreservationCandidate.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/FermentationBrewingPreservationCandidate.md) を正本とする。
### 8.4 酒造

Grains外。酒造・Sake / Morohaku候補はProjectの [FermentationBrewingPreservationCandidate.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/FermentationBrewingPreservationCandidate.md) を正本とする。
### 8.5 漁業・貝塚

Grains外。一般釣りを重複実装しない方針と沿岸採集・貝塚候補はProjectの [HuntingGatheringCoastalCandidate.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/HuntingGatheringCoastalCandidate.md) を正本とする。
### 8.5.1 AMJ共通リテクスチャ方針

技術実装・既存リテクスチャMod監査・競合規則の共通正本は [`Docs/RetextureImplementationGuidelines.md`](RetextureImplementationGuidelines.md) とする。**AMJが所有する前提Mod資産は、AMJ固有texPathへ明示的にPatchする方式を標準とし、同名texture pathのロード順上書きだけには依存しない。** 対象は1枚のPNGではなく、実際にロードされたDefが使用するgraphic state一式として監査する。

AMJでいう**リテクスチャ**は、AMJ独自Defの画像制作だけを指さない。**各AMJ Modが、自分の責務範囲で使用・再利用するVanilla / Medieval Overhaul等の前提Mod資産についても、AMJ追加資産と並べた際に画風・輪郭・色数・陰影・情報密度・解像感が統一されるよう、必要なテクスチャをAMJ側から差し替えること**を含む。

目的は「前提Modを日本風に全面変換すること」ではなく、**各Modを導入したとき、そのModが担当するゲーム領域の見た目が一つのアートセットとして成立すること**である。

#### 所有原則

- **各AMJ Modは、自分の機能・景観責務に属する前提Mod資産の画風統一までを原則として所有する。**
- Coreは、Coreが直接扱う作物、食材、一次加工品、資源、農業・一次加工設備、Core責務の収納等について、Vanilla / MO等の既存テクスチャも必要に応じてCore内でリテクスチャする。
- Environmentは樹木・植物・地形・自然景観等をEnvironment側で所有する。Fermentation / Sake等のAddonも、それぞれが所有する工程・設備・容器等の前提資産を自Mod内で統一する。
- **同一Def / 同一前提資産のテクスチャを複数のAMJ Modが競合して上書きしない。AMJ内で1資産1所有Modを原則とする。**
- 所有先が曖昧な場合は、その資産の**主要なゲーム上の責務を持つMod**を正本とする。単に先に作業したModへ置く、ロード順で勝たせる、といった運用はしない。
- 1つの前提資産が複数Modから広く利用され、自然な所有先を決められない場合だけ、共通Retexture Modへの分離を再検討する。**前提資産をリテクスチャするという理由だけで最初から専用Retexture Modへ集約しない。**

#### 変更範囲

- 純粋なリテクスチャでは、対象Defの機能、容量、Recipe、カテゴリ、研究、数値バランス等を変更しない。必要なXML Patchはgraphic / texPath等の表示差し替えに限定する。
- 機能変更も必要な場合は、リテクスチャに便乗させず、その機能を所有する設計節・Modで別途仕様化する。
- 既存DefNameや外部参照は可能な限り維持し、前提Modとの互換性を壊さない。
- AMJ画風への統一が主目的であり、**全ての前提Mod画像を機械的に描き直すことは目標にしない。** AMJ追加物と並べて明確に浮く、頻繁に表示される、シリーズとして統一感へ大きく影響する資産を優先する。

#### 時代考証

- リテクスチャも通常のAMJ時代考証対象とする。
- 画風を揃えるだけなら元の機能・物体同一性を保つ。形状を日本の器物・建築・道具へ変更する場合は、その形態がAMJ対象時代に存在することを確認する。
- 「和風に見える」という理由だけで江戸以降の意匠へ置き換えない。
- 既存前提資産を歴史的に不適切な別物へ見せ替える必要がある場合は、単純リテクスチャではなく、Def追加・互換・除去等を含めて責務を再検討する。

#### 他Modとの境界

- **MO本体の日本化リテクスチャは `AMJ - Medieval Overhaul Japanization` が所有する。** AMJ独自Defの画像は各所有Modが引き続き所有し、同一MO資産を複数AMJ Modから競合上書きしない。
- 汎用の専用Retexture Modは標準構成では作らない。ただしJapanizationはMO全体を日本化するPatch Modであり、その責務の一部としてMO Retextureを同梱する明示的な例外とする。
- 外部Modそのものの配布物を改変・再配布するのではなく、AMJが権利上問題のない独自テクスチャを持ち、必要なPatchから参照させる。

#### 開発順

機能設計が不安定なAlpha段階ではプレースホルダーを許容するが、**公開版で担当領域の見た目を完成させる最終アート工程には、AMJ独自資産だけでなく必要な前提Mod資産のリテクスチャも含める。** 「前提Modの画像だから別作業」として無期限に後回しにはしない。


### 8.6 建築

Grains外。大型建築Addonを安易に作らず既存MO/和風家具を優先する横断方針はProjectの [ArchitectureAndReligionBoundaries.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/ArchitectureAndReligionBoundaries.md) を正本とする。Grains固有の加工設備だけはGrainsが所有する。
### 8.7 宗教・価値観

Grains外。現在の境界記録はProjectの [ArchitectureAndReligionBoundaries.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/ArchitectureAndReligionBoundaries.md) を参照する。
### 8.8 塩蔵

Grains外。塩蔵・非発酵保存候補はProjectの [FermentationBrewingPreservationCandidate.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/FermentationBrewingPreservationCandidate.md) を正本とする。
## 9. 将来のCore候補

v1以降にCore本体へ追加する候補。

### Core + Environment 自動ゲームプレイ評価方針

AMJでは「値が設計どおりロードされたか」だけでなく、**その値の組み合わせが実際に異なる選択肢を生んでいるか**についても、機械的に判定できる範囲は自動テストへ寄せる。

自動化対象:
- Stage A各作物の成長日数、収量、肥沃度最低値・感応度、成長温度、研究解禁、加工工程等の最終ロード値;
- 作物間の役割差が将来の調整で潰れていないこと。例として、短期作と高収量作、寒冷適応、痩せ地適応、研究前後の選択差を関係式として検証する;
- Environment併用時の実マップ土壌分布とCore作物の接続。自然土壌4段階で、薄い土壌を利用できる作物と利用できない作物が実際に分かれること、肥沃度感応度から実効成長倍率に差が出ることを検証する;
- Environmentの年間温度サンプルとCore/CCTO側の成長温度・枯死温度を接続し、暖地・温帯・寒冷地・高地の温度機会が同一化していないことを検証する;
- New Villageの開始人数・研究・物資・初期加工可能性;
- 加工時の数量保存、作業量、研究ゲート、食材カテゴリ接続、保存日数;
- 自動テストでRimWorldを起動する場合のERRORゼロゲート。

Stage Aでは、Pickleに**「作物の値が一致する」テストに加えて「作物の役割差が維持されている」回帰テスト**を置く。Core + Environment統合時はEnvironment側の固定バイオームQuickstartで実マップ土壌を生成し、Core作物の播種可能範囲と平均肥沃度成長倍率を評価する。

自動評価で扱わない / 最終的に人間の判断を残すもの:
- 画面上の統一感、シルエット、読みやすさ;
- 「選択肢が存在する」ことを超えた、面白さ・煩雑さ・テンポ・判断疲れ;
- 通常プレイでの長期的な体感、AI挙動や偶発イベントを含む総合的な村運営感;
- 日本語UIや説明文の自然さ。

したがって、手動プレイで最初に確認するのは「数値差があるか」ではなく、**自動テストで差が成立していることを前提に、その差が実際に面白い判断として感じられるか**に限定する。機械判定できる既知条件を毎回人間に再確認させない。

### Grains実装ロードマップ

Grainsの主要実装は、次の順で管理する。

| Stage | 主題 | 主な内容 | 現在の状態 |
|---|---|---|---|
| **Stage A / 現行実装** | 乾田穀物 | アワ・ヒエ・キビ・ソバ・大麦・MO小麦統合、脱穀・殻取り、穀物加工設備 | MO必須版として実装済み |
| **Grains分離移行** | Vanilla自己完結 | 非MO小麦、小麦粉・蕎麦粉・雑穀粉、手動石臼、最低限粉食、BaseからMO参照除去、MO条件付き互換 | **step 3 小麦・製粉・最低限粉食XML実装・実機未検証** |
| **陸稲統合** | Vanilla Riceの中世日本化 | `Plant_Rice` を陸稲として再定義、稲束→籾→`RawRice` 必須加工、Hydroponic除去、七穀バランス・CCTO回帰 | **Production XML・静的七穀分析・Pickleの加工経路ソースを追加。実ゲーム・画像・セーブ回帰は未完** |
| **回帰固定** | 環境別穀物選択 | Base/MO/CCTO各プロファイルで単一穀物がほぼ全条件の最適解にならないことを自動確認 | 分離移行と同時に追加 |
| **Grains V1** | 暫定加工設備で基本ループを成立 | 脱穀・殻取りはVanilla/MO両方で調理台対応。非MOの5製粉Recipeは調理台にも対応し、MO製粉はMO石臼を維持 | XML・静的テスト・実機検証項目を実装。実ゲームの新経路とV1リリース検証は未完了 |
| **V1後の設備整備** | 専用作業台への振り分け | 加工場所・加工台・石臼の用途・完成画像とRecipe割当を整理して暫定調理台経路を再評価 | 将来の更新項目。時期・構成未確定 |

旧Stage B（豆類）、Stage C（繊維）、Stage D（根菜）はGrainsロードマップから削除する。将来必要なら、それぞれの主要用途と自然な所有Modを決めて別途設計する。

### Grains外へ移した旧Core候補

旧Core由来でGrainsの所有外となった未所属候補はProjectの [DeferredPlantsAndProcessing.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/DeferredPlantsAndProcessing.md) に移管した。Grainsには「所有しない」という境界だけを適用する。
### 旧Core候補の履歴（Grains外）

履歴と再検討条件はProjectの [DeferredPlantsAndProcessing.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/DeferredPlantsAndProcessing.md) を正本とする。
### Grainsに入れない / 旧Coreで保留した農作物・植物

未所属・保留・不採用候補の詳細台帳はProjectの [DeferredPlantsAndProcessing.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/DeferredPlantsAndProcessing.md) へ移管した。Grainsの実装範囲は本書4章の乾田穀物・陸稲と、それらの収穫後加工・製粉に限定する。
### 冬季加工の拡張候補

Grains外の保存・発酵候補としてProjectの [DeferredPlantsAndProcessing.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/DeferredPlantsAndProcessing.md) へ移管済み。
### 既存Mod側を優先し、Coreは互換中心

採集・困窮食等の未所属互換候補はProjectの [HuntingGatheringCoastalCandidate.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/HuntingGatheringCoastalCandidate.md) と [ExistingModAudit.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/ExistingModAudit.md) へ移管した。Grains自身に必要な互換だけを本リポジトリで管理する。
### 別アドオン / 姉妹Mod寄り

AMJ全体の未所属・将来Mod一覧は `Ancient-Medieval-Japan-Project/Docs/Roadmap.md` を正本とする。Grainsでは自分の所有外であることと、必要な互換境界だけを記録する。
### Ironmaking / 古代・中世製鉄（2026-10-07 作者確定）

IronmakingはGrains外の独立Modコンセプト。専用リポジトリ作成前の設計正本はProjectの [IronmakingDesign.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/IronmakingDesign.md) とする。GrainsはIronmakingを必須依存にせず、必要な互換だけを将来条件付きで持つ。
## 10. 推奨バイオーム

初期の動作・バランス確認は以下3種に絞る。

- 温帯森林
- 温帯湿地
- 針葉樹林

目的:

- 日本列島に近い環境幅
- 温暖・湿潤・寒冷の差を作物性能へ反映しやすい
- 作物ごとの個性を確認しやすい

他バイオームでの動作は禁止しないが、v1の主要バランス対象にはしない。

Hilliness補正についてはバイオームとは独立したワールド側の基礎傾向として扱う。初期テストではFlat / Small Hills / Large Hills / Mountainousをそれぞれ含むワールドを生成し、平地の希少化と丘陵・山岳増加が過剰でないか確認する。

---

## 11. 互換対象の優先順位

### 基準環境

- RimWorld Vanilla
- Medieval Overhaul（公式互換・主要比較対象）

### 優先度A

- Medieval Overhaul互換の継続回帰
- Food Drying

### 優先度B

- Dubs Bad Hygiene
- Famine Food
- Vanilla Plants Expanded - More Plants
- Medieval Kingdoms: The Later Tang

### 優先度C

- ReGrowth 2
- RimImmortal / 仙路系
- Edo Themed Expansion
- MoeLotl
- Yuran
- VGP系
- その他竹・紙・農業素材Mod

### 評価待ちの互換候補

Grains以外の将来機能に関わる評価待ち候補はProjectの [ExistingModAudit.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/ExistingModAudit.md) に移管済み。Grainsの具体的な互換対象として採用された時点で、本節ではなく対応するGrains互換設計へ戻す。
### Faction / Background系の互換方針

Faction / Background / Eventsの横断互換はGrainsの責務外。専用リポジトリ作成前の詳細方針はProjectの [SocietyModulesCandidate.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/SocietyModulesCandidate.md) を正本とする。Grains固有Defとの直接競合が発生した場合だけ、本リポジトリに互換仕様を追加する。
## 12. グラフィック方針

### 12.1 目標

Medieval Overhaulと並べても違和感が少ない、**ベクター画像的な整理されたゲームアート**を目標とする。

重視する特徴:

- 輪郭が明快で、シルエットだけでも判別しやすい
- 細かな筆致より、面の分割と色のまとまりを優先する
- 過度なテクスチャ・グラデーション・描き込みを避ける
- 木・石・金属などの材質は、細密表現より記号化して分かりやすくする
- RimWorldの斜め上視点で形状を読み取りやすくする
- 小サイズ表示でも情報が潰れないよう、要素数を絞る
- MO既存テクスチャの輪郭線、陰影量、彩度、情報密度を基準にする

「手描きらしさ」を目的にはせず、**ベクターで形を組み、必要最小限の陰影や質感を加えたような見た目**を基準とする。

#### 12.1.1 AMJ画風の正本

2026-10-03に、MOの実物テクスチャ（成熟小麦・Leather・Hide）との反復比較を経て、AMJの標準画風を確定した。以後の画像生成・手動作画では、**RimWorld VanillaではなくMedieval Overhaulのフラットさを基準**とし、詳細な再現ルールは `Docs/ArtStyle.md` を正本とする。

特に以下を固定ルールとする。

- 少数の大きな色面で構成し、グラデーションや写実的な陰影を使わない
- 太めの暖色系暗色輪郭を維持する
- 植物は「個々の粒・葉脈」よりシルエットを優先する
- アイテムは植物以上に陰影を減らし、1素材あたり基本色＋補助1面程度を目安にする
- MO小麦・Leather・Hideより情報密度が高くなった場合は、完成度を上げるのではなく情報を削る
- 生成時は比較表やUI画像ではなく、最終的に**透明背景の単独アセット**を出力する
- 256×256と約64px相当の両方で確認し、小サイズでの判別性を優先する

アワ成熟株で採用した「大きな穂形状・少数の葉・オリーブ系緑＋黄土系の穂＋共通暗色輪郭」を最初のスタイルアンカーとし、他の作物・収穫物・加工品も同程度の色数・陰影量・情報密度へ揃える。

#### 雑穀3種の葉なし簡略化（2026-10-09）

作者指定の今回の範囲は、アワ・ヒエ・キビの未熟／成熟と共有の雑穀束のみ。栽培中は葉を省略し、穂の外形と茎を中心に表現する。未熟は緑、成熟は黄土色。雑穀束はアワの長い穂、ヒエの短く垂れる穂、キビの開いた分枝を一つの帯で束ねた混合束とする。枡入り穀粒・蕎麦・大麦・稲・料理はこの変更範囲に含めない。

2026-10-09に作者が「では一旦これでFixとする」と最終採用。`Art/Candidates/MilletSimplification-20261009/` にプロンプト、256px書き出し、比較画像、検査結果を保存し、採用した7枚の生成原本は新しい画像パスに対応する `Art/Sources/` 配下へ同一バイトで保存した。過去の採用済み原本は上書きしない。ローカル反映済みであり、Gitコミット・公開・実ゲーム表示確認は未実施。

3種は `Things/Plants/{FullGrown,Immature}/AMJC_{Awa,Hie,Kibi}`、雑穀束は `Things/Item/Resource/AMJC_Millet/MilletSheaf` を参照する。2026-10-09当時は大麦・稲の仮画像を維持するため `_Simple` という別パスを作成したが、各作物に専用画像が統合された現在、その区別は不要。本番は通常の `AMJC_<作物>` パスに統一し、旧本番PNGを重複保持しない。制作当時は束の3スタックスロットを同一PNGとしていたが、現在は `_a` 1枚に集約（§12.1.4）。作物・収穫量・加工・食品数値は変更しない。

#### ソバ・大麦・AMJ小麦および密集した束への拡張（2026-10-09）

作者の追加依頼により、ソバ・大麦・AMJ小麦の未熟／成熟を同じ葉なし・輪郭主体の画風で制作。ソバは三角形の実と赤みのある成熟茎、大麦は長い芒、小麦は短い先端を持つ太い穂で区別する。参照先は `Things/Plants/{FullGrown,Immature}/AMJC_{Soba,Barley,Wheat}`。AMJ小麦は `BaseWithoutMO` の独自作物に適用する。MO併用時も `WheatGraphics.xml` でMO小麦の未熟・成熟・束を同じAMJ画像へ割り当て、MOのDef識別子・ゲーム仕様は維持する。

さらに「雑穀束、ソバ束、大麦束、AMJ小麦束はよりMO小麦束に似せてほしい／よりたくさんの穂が束ねられてるようにする」という作者指示により、束4種を幅広く密集した穂・実の塊、単純な帯、太く短い裾へ改稿。雑穀束はアワ・ヒエ・キビの特徴を外周に残す。現在のローカル参照は `AMJC_Millet/MilletSheaf`、`AMJC_Buckwheat/BuckwheatSheaf`、`AMJC_Barley/BarleySheaf`、`AMJC_Wheat/WheatSheaf`（共通接頭辞 `Things/Item/Resource/`）。制作当時は3スロットを同一PNGで埋めていたが、現在は `_a` 1枚に集約（§12.1.4）。

新規栽培画像の生成原本・プロンプト・QAは `Art/Candidates/GrainSimplification-20261009/`、最新の密集束4種は `Art/Candidates/DenseSheaves-20261009/` に保存。これらはローカル確認用 `review` であり、先に採用された雑穀栽培6枚と区別する。旧原本・旧束は保持。加工後の穀粒・粉・料理・稲・収穫量などのゲーム仕様は変更しない。

PNG全53枚の構造検査と生成原本／書き出しの機械QAは合格。既存の移管ファイルにCRLF由来のハッシュ不一致があるため、隔離した検査コピーで当該未変更ファイルのみGit原本バイトへ戻したうえで、Stage A・Base/MO契約・小麦加工契約の静的検査が合格した（`TestResults/ArtStatic-20261009/static-result.txt`）。ライブ側の当該移管ファイルは変更していない。実ゲーム描画・最終視覚採用・GitHub／Steam公開は未完了。

#### 陸稲と未熟輪郭色（2026-10-09）

作者依頼により、陸稲の未熟・成熟・稲束を追加した。未熟は上向き〜斜め上向きの軽い穂、成熟は実の重みで垂れる穂とする。最初の垂れた未熟案は作者指摘により不採用。稲束は他の密集束と同じく多数の穂をまとめた幅広い塊、単純な帯、太い裾で表す。

さらに未熟7種（アワ・ヒエ・キビ・ソバ・大麦・AMJ小麦・陸稲）の輪郭を、MO小麦未熟の暗い灰緑色を基準に変更した。MO実画像から測定した代表輪郭RGBは `(80,83,69)` / `#505345`。ImageGenによる色合わせであり、全輪郭ピクセルのRGB完全一致や元画像とのピクセル単位の形状不変を保証する処理ではない。成熟・他の束はこの輪郭色変更の対象外。

所有者はGrains、元DefはCoreの `Plant_Rice`、全構成で有効。`Patches/UplandRiceGraphics.xml` は `graphicData/texPath` と `plant/immatureGraphicPath` だけを `Things/Plants/{FullGrown,Immature}/AMJC_Rice` に置換し、Graphic_Random・サイズ・植生設定を維持する。稲束は `Things/Item/Resource/AMJC_Rice/RiceSheaf`、現在は `_a` 1枚（§12.1.4）。インストール済みCoreの両対象フィールドを確認し、MO 1.6配下のXMLにはこれらを上書きする競合を検出しなかった。全Modのロード後競合・実描画は未確認。

生成原本・プロンプト・QA・比較画像は `Art/Candidates/UplandRice-20261009/` と `Art/Candidates/ImmatureOutline-20261009/` に保存。既存の採用原本を保持し、変更前のゲーム用未熟画像も後者の `Before/` に保存。今回の画像はローカル確認用 `review`。PNG全58枚、陸稲の7作物比較27条件／構成の静的回帰、隔離コピーでのStage A・Base/MO契約が合格（`TestResults/RiceArtStatic-20261009/static-result.txt`）。隔離コピーでは未変更移管ファイルのCRLFだけをGit原本バイトへ戻して検査した。実ゲーム表示・作者の最終採用・コミット／公開は未実施。

2026-10-10 GitHub反映: 作者依頼により生成済み画像・原本・参照設定を統合する。最新の未熟7種は `Art/Candidates/ImmatureUpright-20261009/` の工程で輪郭色 `#4D4E3C` に統一し、アワ・ヒエ・キビの未熟穂を上向きへ修正済み。上記の未公開記述は各制作時点の記録。実ゲーム表示とreview画像の最終視覚採用は別途未確認。


2026-10-10 原本整理: PR #20で本番に参照された7種の成熟株・7種の未熟株・5種の束について、当該制作用高解像度原本19件を `Art/Sources/` に対応付け（従来保存済み3件を保持・未保存16件を原本バイトで追加）。現行の未熟株は `Art/Candidates/ImmatureUpright-20261009/Normalized/` の輪郭調整済み高解像度原本を用い、元の生成画像は同系列の `Sources/` に残す。原本・候補・出力の対応と取り込み時のハッシュは `Docs/References/GrainsCropSourceManifest.json` に記録する。採用後の差し替えを旧ハッシュで禁止しない。実描画検証は未実施。旧Core元画像の回収状況は別の棚卸しとして保持。

2026-10-11 画像原本整理：`Textures/Things/Plants/` の通常名パスに合わせ、7種の未熟・成熟の現行原本を `Art/Sources/Things/Plants/{Immature,FullGrown}/AMJC_<Crop>/AMJC_<Crop>_<State>.png` の14ファイルに集約した。各原本は既存の採用版Git blobを再利用し、再生成・再圧縮しない。`_Simple` を含む原本側の旧パス24ファイルと、現在の密集雑穀束と同一バイトの旧 `MixedMilletSheaf` 原本1ファイルは削除。削除した独自の画像データはすべて `Art/Candidates/` にも存在し、過去の採用記録はGit履歴に残る。現行原本・候補・ゲーム用パスの正本は `Docs/References/GrainsCropSourceManifest.json`。過去の制作段階での「旧原本を保持」「_Simpleに分離」は履歴であり、現在のファイル配置を指定しない。枡・穀粒・その他の採用原本、全本番PNG、Def/XML、数値とロード契約には変更なし。

#### 12.1.2 縦切り実装の画像完了条件

機能単位の縦切り開発では、成熟画像1枚だけを完成させて「画像完了」としない。**その実装でプレイヤーが通常見る主要な表示状態を一通り本番画像へ置き換え、ゲーム内で確認してから次の機能へ進む。**

作物の場合の基本確認対象:
- 未成熟状態
- 成熟状態
- 収穫直後のアイテム
- その作物専用または当該スライスで新規追加した主要中間素材・最終素材

ただし、複数作物が同じThingDefへ合流する場合は、同じ共有画像を作物ごとに重複制作しない。アワ・ヒエ・キビでは、栽培中のPlantDef画像は各作物ごとに持つが、収穫後の `雑穀束 → 殻付き雑穀 → 雑穀` は共有ThingDefとして一度だけ制作・確認する。

設備も、その機能でAMJ固有BuildingDefを追加した場合は仮画像のまま機能完了とせず、数値・動作確認後に本番画像へ置き換えて通常ズームで確認する。**Grains V1の加工設備・石臼は例外**とし、当初想定した専用作業台やその完成画像をV1必須ゲートから外し、設備の完成・工程別の振り分けをV1後へ延期する。ただし暫定画像の実ファイル参照、描画エラー、操作・Bill実行、ERROR 0の確認は必要。他の画像や本番データの破損・不整合まで免除しない。

これにより「仮画像で実装 → 動作/数値確認 → 主要な可視状態を本番画像化 → ゲーム内見た目確認 → 次機能」の順を守る。

**2026-10-13 作者による現行優先順位の例外（植栽状態・穀物束）:** 現在採用・統合済みの7種の未熟／成熟植物画像（計14状態）と5種の穀物束画像は、当面そのまま使用する。これらの外観の手直し・再生成・差し替えは**将来の改善対象として延期**し、今は実施しない。上記の縦切り画像完成順序にかかわらず、当該画像の美観改善を他のGrains実装・検証・非公開プレイの開始条件にはしない。一方で、本番PNGの破損・参照切れ・実ゲーム描画上の不具合を検出する検証は継続する。具体的な不具合が確認された場合の修正は、単なる美観改善と区別する。この延期は穀粒・粉・料理・設備の画像制作には適用しない。

**2026-10-17 実プレイによる画像修正方針（将来実施）:** 穀物束・加工前後の穀粒・穀粉などGrains所有アイテムの輪郭線が、ゲーム内の通常ズームで周囲のVanilla／Medieval Overhaul画像と比べて細く見える。次の画像改修では**外側の輪郭を現行より太くし**、小さな表示時のシルエットと視認性・画風の統一感を優先する。対象画像の形状や可読性はゲーム画面で比較判断する。2026-10-13に延期した植栽14状態と束5種の美観改修は引き続き延期であり、この方針を理由に今すぐ既存採用画像を再生成・差し替えない。数値・Def・Recipe・画像参照先の機能変更を伴わない。Workshop等の「今後の予定」に記載する際は実装済みと混同させない。

**2026-10-17 今後の画像修正・追加を公開予定に明記:** 既存の作物・穀物束・穀粒・穀粉の輪郭・可読性改善（採用済み作物14状態と束5種の美観修正は延期）に加え、未完成の穀粒・米関連の専用画像整備、既存料理8品の共有仮画像から各料理の専用画像への置換、Grains所有の加工場所・加工台・非MO石臼の専用画像追加、穀物束・穀粉の数量別表示画像を将来の画像作業として掲示する。穀物束の数量別画像は引き続き延期、粉の数量別画像は延期対象外。正式な対象・表示状態・現行採用版の区別は§12.1.2～12.1.4およびV1後の設備整備方針に従う。既存画像・ゲーム機能・数値・配布版は本記録では変更しない。

**2026-10-19 加工場所・穀物加工台の採用および全方位画像仕様（後続の作者指示を反映）:**
既存の `AMJC_GrainProcessingSpot` と `AMJC_GrainProcessingTable` は各々1×1マスの独立した設備。中世日本の穀物加工を示す俯瞰画像を使う。簡易加工場所は作者が許容したへら無し・1×1構図、加工台は脚が切れない最新版の作者承認画像を使用する。両設備とも**各設備につき1枚のPNGを東西南北の4ファイルに全く同じバイトでコピー**する。向きによる回転・左右反転・再生成はしない。`Graphic_Multi` のまま `drawRotated=false`、`drawSize=(1,1)`。
簡易加工場所の今回の原本は `Art/Sources/Things/Building/Production/AMJC_GrainProcessingSpot/GrainProcessingSpot_1x1_NoPaddle.png`、加工台の原本は `Art/Sources/Things/Building/Production/AMJC_GrainProcessingTable/GrainProcessingTable.png`。どちらも原本を無加工保存し、本番PNGは原本から等比縮小し透明な256×256キャンバスへ中央配置する。出力は `Textures/Things/Building/Production/AMJC_GrainProcessingSpot/GrainProcessingSpot_{north,east,south,west}.png` と `Textures/Things/Building/Production/AMJC_GrainProcessingTable/GrainProcessingTable_{north,east,south,west}.png`。以前の簡易加工場所の「北南は横／東西は90度回転版」という仕様は撤回した。過去の採用原画を再生成・破棄せず、旧ソースは履歴として保持する。
XMLの設備サイズ・作業速度・素材コスト・Recipe・DefName・操作位置の機能値は変更しない。画像参照と4方向のPNGのみ変更する。通常ズーム・4方向・ERROR 0の実ゲーム描画確認は別途必要。

**2026-10-19 非MO石臼の採用と表示:** 作者が最終確認した、台・横の容器のない灰色の手挽き石臼（上臼・下臼・木製取手）の透明256×256画像を、非MO用 `AMJC_ManualMillstone` の専用表示として採用する。石臼も北・東・南・西すべてで**同一PNGバイト**を使用し、回転画像・再生成は作らない。`Graphic_Multi` / `drawRotated=false` / `drawSize=(1,1)` / `size=(1,1)` を維持する。ゲーム用パスは `Textures/Things/Building/Production/AMJC_ManualMillstone/ManualMillstone_{north,east,south,west}.png`、XMLの `texPath` は `Things/Building/Production/AMJC_ManualMillstone/ManualMillstone`。
作者が直接確認・承認した256px確定画像の元バイトを `Art/Sources/Things/Building/Production/AMJC_ManualMillstone/ManualMillstone_Accepted256.png` に保全する。同系列の高解像度参考画像 `ManualMillstone_Reference1254.png` も保全するが、256px承認版とのピクセル単位の派生手順は未検証であり、原画を再書き出しして承認画像を置換しない。非MO Defにのみ画像参照を接続し、MO併用時は従来どおり `DankPyon_Millstone` を使う。設置コスト（花崗岩ブロック30・木材20）、作業量、素材、五粉Recipe、Bill、DefNameは変更しない。実ゲーム描画・通常ズーム・ERROR 0は未確認。

#### 12.1.3 加工後の雑穀画像の採用状況（2026-10-19確認）

2026-10-10に統合済みの **7種の未熟株・7種の成熟株・5種の収穫束（19表示状態）** に、加工後の雑穀2状態は含まれない。殻付き雑穀は作者が新たに提示・採用した枡入り画像に同日差し替え、その原画を本節の専用パスへ無加工保存してゲーム用3枚へ書き出した（旧画像の履歴はGitに保持）。殻なし雑穀は2026-10-11に作者が追加した128×128の単一 `_a` 本番画像が存在し、既存ThingDefの参照パスにも接続されている。画像未制作とは扱わない。

| 表示状態 | DefName | `texPath` | 画像・検証状態 |
| --- | --- | --- | --- |
| 殻付き雑穀（脱穀後・殻取り前） | `AMJC_MilletInHull` | `Things/Item/Resource/AMJC_Millet/MilletInHull` | 作者採用済み原本を保全。2026-10-11以降は128×128の単一 `_a` 本番画像に集約。実ゲーム通常ズームは最終確認待ち |
| 殻なし雑穀（殻取り後・可食） | `AMJC_Millet` | `Things/Item/Resource/AMJC_Millet/Millet` | 作者が2026-10-11に投入した128×128の本番 `Millet_a.png`（同名DDSも存在）を参照済み。専用画像の新規制作は不要。高解像度採用原本は `Art/Sources/.../Millet/` に未保存（現在 `.gitkeep` のみ）。通常ズーム・最終画風確認は未完了 |

アワ・ヒエ・キビの収穫後は `AMJC_RawMillet → AMJC_MilletInHull → AMJC_Millet` の共通ThingDefへ統合するため、作物ごとに加工後画像を増やさない。殻付き／殻なしの違いは穀粒の内容物で表す。枡の画像管理は `Docs/GoldenPaths/BoxedResourceIconPipeline.md` を準拠とする。

殻付き雑穀の新原画のGit blob SHA-1は `61d6720c7354107424400dd9abf0edc0a118b859`、ゲーム用3枚共通のGit blob SHA-1は `039c6f11bbe5426d169b011a706cfcb3e6802c2f`。既存Defの `texPath` を維持し、穀物の歩留まり・保存日数・Recipe・加工設備は変更しない。元画像・本番PNG・Def参照は静的検証し、ゲーム実機でのサイズ・視認性は別途確認する。

殻なし雑穀・食用大麦・小麦・籾・Vanilla `RawRice` の本番単一 `_a` 画像は2026-10-11に既存／専用パスへ接続済み。これらを「画像未制作」と一律に扱わない。高解像度原本の由来・最終採用状態とゲーム内通常ズームは別途確認する。粉の数量差分・専用料理残品・加工設備の未完成は別件で管理する。殻付き大麦（`AMJC_BarleyInHull`）は作者採用の元画像を `Art/Sources/Things/Item/Resource/AMJC_Barley/BarleyInHull/BarleyInHull.png` に保存し、専用 `Things/Item/Resource/AMJC_Barley/BarleyInHull` パスの256pxスタック画像3枚へ反映済み（ゲーム内表示は未検証）。食用米はGrainsが担当し、既存の `RawRice` DefName・料理互換性・数値を維持して画像参照だけをAMJ画風へ差し替える（画像制作・実装・ゲーム表示検証は未完了）。

2026-10-11 画像参照更新：作者がmainに追加した128×128の穀粒単一 `_a` 画像を使用し、共有雑穀 `AMJC_Millet` は既存専用パスを継続、`AMJC_Barley`・`AMJC_Wheat`・`AMJC_RiceInHull` はそれぞれ大麦・小麦・籾専用のtexPathに切り替えた。Vanilla RawRiceは新しいDefを作らず、Patches/RawRiceGraphics.xmlで描画パスのみGrainsのRice_aへ切り替え、Graphic_StackCountによる1種類の外観とする。雑穀とソバの既存参照は変更なし。原本のArt/Sourcesへの保全と実ゲームでの表示検証は別途OPEN。栄養・Recipe・農業数値は不変。

#### 12.1.4 穀物・製粉品のスタック画像（2026-10-10確定）

作者がMO 1.6の小麦束・小麦粉とVanilla `RawRice` の実Def／画像構成を比較し、Grainsの制作単位を次のように確定した。**数量別の本番画像制作は未実施**であり、この仕様確定だけで現行テクスチャ・Def・テストは変更しない。

| 表示対象 | 必要な異なる画像数 | 表示ルール |
|---|---:|---|
| 収穫直後の穀束（雑穀／ソバ／大麦／小麦／稲） | 3 | MO小麦束を基準として `Graphic_StackCount` の `_a`／`_b`／`_c` に少量・中量・大量の異なる外観を割り当てる |
| 殻付き穀粒・籾 | 1 | 数量による外観変化を設けない |
| 殻なしの可食穀粒・米 | 1 | Vanilla `RawRice` の単一画像表示に準じ、数量による外観変化を設けない |
| 製粉後の粉（小麦粉／蕎麦粉／雑穀粉／大麦粉） | 3 | 4種の現行生産穀粉はMO小麦粉を基準に少量・中量・大量の異なる外観を用意する |

アワ・ヒエ・キビは収穫後に同一ThingDefへ統合するため、雑穀束・殻付き雑穀・可食雑穀はそれぞれ共有画像とし、品種別の重複画像は作らない。MO併用時の小麦束・小麦粉・石臼は既存MOのThingDefを条件付き利用する。製粉対象の画像は非MO小麦粉・蕎麦粉・雑穀粉・大麦粉の4種を現行として扱い、米粉の過去採用画像は削除せず保全する。

過去の制作記録にある穀束「3スタックスロット同一PNG」は当時の仮配置であり、現在の必要素材ではない。米粉の旧画像も履歴・原画として残すが、2026-10-19以降の通常生産対象には含まない。

**2026-10-13 優先順位補正:** 穀束の少量・中量・大量の異なる外観を揃える作業も**将来の改善対象として延期**する。現在の穀束 `_a` 単一画像運用と、採用済み画像の参照を維持する。これは最終目標の3種を撤回して1種へ仕様変更するものではない。製粉後の粉の数量別画像はこの延期の対象外とする。

差分画像の導入時には `Docs/References/GrainsCropSourceManifest.json` の対象画像に関する原本対応・採用状態を更新し、原本保全・PNG整合性・通常ズームでのスタック表示を検証する。テストは本番の参照切れ・破損・加工やロード条件の不具合を検出する。特定画像の旧ハッシュ・原画寸法・候補コピーの恒久的同一性、説明書の定型句、C#テストソースの文字列、テスト内の定数計算は合否条件にしない。実ジョブの成否は既存のPickleで確認する。

穀束の現行パスは `Things/Item/Resource/AMJC_<Family>/<Family>Sheaf`（Family: Millet / Buckwheat / Barley / Wheat / Rice）。`Dense` は制作時の改稿名であり、別Def・数量段階ではないため本番・原本パスから除去した。穀粒は `<Family>`、殻付き穀粒は `<Family>InHull`、穀束は `<Family>Sheaf` で区別する。採用原本とゲームPNGは再生成・再圧縮せず同一バイトで移動し、原本対応表・Base/MOのDef参照・既存のパス検証も揃える。歴史的候補フォルダ名は制作記録として保持する。

### 12.2 制作環境

用途別に使い分ける。

**Krita**
- 作物・有機物など、多少の手描き調整が必要な素材
- 図形・選択範囲・変形を使った面構成
- 最終的な陰影・輪郭調整

**Inkscape等のベクターツール**
- 石臼
- 桶
- 作業台


### Grains 環境・実ジョブ回帰の追加（2026-10-07）

**2026-10-08陸稲実装後の状態:** 六穀Fixtureは陸稲追加前の履歴回帰として維持する。Productionの `Patches/UplandRice.xml`、七穀静的回帰 `Tests/test_upland_rice.py`、七穀Pickle環境比較・収穫テストの**ソース**は追加済み。実ゲームでのPickle成功、C#コンパイル、季節条件を含む播種ジョブ、ERROR 0は未確認。六穀の履歴PASSだけでリリース完了とはしない。

六穀の有限成長時間比較を `Docs/Balance/Crops/GrainsEnvironment.md` に定義し、
肥沃度3×温度3×季節3の27セルで成熟穀粒収量と勝者を回帰固定する。
全六穀に代表的な最大収量条件を残し、有効セルの2/3以上を単一穀物が占めない。
光量・休眠・労働・霜死・Straw価値を除いた分析モデルであり、リリースの環境ゲート全体は未完。
既存PlantDefの値・About依存・開始シナリオの分離方針は変更しない。

四プロファイルのPickleは穀物専用6シナリオ（開始シナリオ2件はScenariosへ移管）。ロード済みPlantUtilityの環境係数比較と、
New Villageに依存しないStage A Quickstart上の七穀の実収穫を検証する。元の11件の加工・製粉・粉食Billに、追加の脱穀・殻取りと5種類の可食穀粒による `CookMealSimple` Billを加えている（いずれも実ゲームの成功は未確認）。
成熟株の配置を開始点とし、収穫物・加工品はテスト側で生成しない。
現在はC#ソースと起動配線の追加までであり、実ゲームのビルド・実行・ERROR 0は未検証。
実ジョブ経路と検証境界の正本は `Docs/GrainsProfileTesting.md`。


実ジョブの経路確認により、AMJ加工Spot/TableとBase手動石臼にWorkGiverの接続がないことを確認した。
共有 `AMJC_DoGrainProcessing` と非MO限定 `AMJC_DoGrainsMilling` を追加し、
WorkGiver_DoBillから固定対象へ通常作業を割り当てる（当初はCrafting、2026-10-17以降はCooking）。
レシピのrecipeUsersだけではWorkGiver_DoBillの固定対象一覧へ追加されないため、両方を持つ。
MO石臼は既存MO WorkGiverを利用し、重複作業Defを追加しない。既存38契約は変更しない。


### 開始シナリオの完全分離（2026-10-14）

旧 `LegacyStartingScenarios/` とそれを起動する条件付きロード領域、旧 New Village の専用 E2E / fixture を Grains から撤去。Grains の `loadFolders.xml` は基本・MO・非MO領域のみをロードする。Grains は開始シナリオ/開始用派閥/開始用 PawnKind を供給しない。新規開始の4構成テストは Scenarios 所有であり、Grains 側の独立した穀物テストは従来どおり実施する。

旧 Core / Grains の未公開セーブ互換や仮シナリオ抽出パッケージは不要。履歴は Git に残し、現在の正本は [Docs/ScenarioExtraction.md](ScenarioExtraction.md) と Scenarios リポジトリ。

### 日本語説明文・翻訳監査（2026-10-08）

歴史説明の候補、根拠、翻訳対象、既存の不一致の一覧は [LocalizationHistoricalReview.md](LocalizationHistoricalReview.md) を正本とする。まず機能の事実関係（MOなしの大麦研究不要、藁が出るのはMO互換時だけ、麦茶・味噌は未実装、小麦製粉はBase/MOとも利用可能）を日本語DefInjectedで修正した。餺飥・蕎麦掻き・雑穀団子等の15件の歴史説明・作業表示について、作者は2026-10-08に日本語を承認した（`Grains` 表記をプレイヤー向け説明では `AMJGrains` へ変更し、小麦の「実は食材になる」の曖昧さを解消した）。日本語DefInjectedへ承認済み表現を反映し、対応する英語原文のdescription・jobStringを追加した。日英の正本文面と典拠は `Docs/LocalizationHistoricalReview.md` §2/§6。DefName・既存label・栽培/加工数値・MOとの依存/切替は変更しない。既存植物の追加史実説明、実ゲーム4構成での文字列ロード・UI表示・ERROR 0は未検証。


### 既存六作物の歴史説明追加監査（2026-10-08）

前回承認済みの15件（粉食・製粉・非MO小麦等）の日本語・英語説明は確定済みとし、その文面を変更しない。残る栽培対象の**粟・稗・黍・蕎麦・大麦・陸稲**について、原文・日本語DefInjected・現行Plant/陸稲Patchと史料を再照合した。2026-10-08に作者が**六作物の日本語説明も承認**したため、[LocalizationHistoricalReview.md §7](LocalizationHistoricalReview.md) の日本語を各DefInjectedへ反映し、§8の対応英訳を共有PlantDef5件とVanilla `Plant_Rice` の条件付きdescription Patchへ反映した。生育温度・収量と霜の耐性に関する表現は区別し、農村の地域的事例を古代から全国一律へ広げない。既存の `label`、料理/加工・農業バランス、DefNameは維持する。テストは六作物の**承認済み日本語・英語・本番XMLの一致**と、PlantDef/陸稲Patchの所有境界を検証する。RimWorldでの実表示・言語切替・四構成ERROR 0は未検証。


### 七穀の収穫→加工→粉食バランス監査（2026-10-08）

七穀の相対収量と各工程・製粉・完成料理までを [Balance/Crops/SevenGrainFoodChainAudit.md](Balance/Crops/SevenGrainFoodChainAudit.md) にまとめ、`Tests/test_grains_balance_chain.py` で静的な歩留まり・作業量・Nutrition・保存期間・Mood・陸稲の同率首位を監査する。**既存六穀の作物・Recipe・温度・栄養・画像設定は維持。陸稲の収穫先と専用加工Recipeだけを新規追加した。** 七穀全てに同率を含む最大収量セルがあるが、陸稲は6セル全て同率で独自の収量首位なし。水田不要という特徴はあるが、**収穫後加工ゼロは現行暫定XMLの問題であり採用すべき永続的な優位ではない**。稲束→籾→`RawRice` の加工義務化を実装・再テストする。MO原本の小麦製粉一括workAmount 800は非MO石臼の300より大きい一方、MO小麦粉は90日保存（Baseは60日）。手動加工・製粉・調理の異なるworkSpeedStatを単純合算したものを実作業時間とはしない。MO原本の製粉効率や陸稲の労働優位は今後の実プレイ評価項目に残し、変更を要するかは別判断とする。CCTO/気象/播種労働・収穫技能・実機栄養/画面/保存・セーブ移行は未確認。


### 粉食3品の画像参照 — 実機エラー修正（2026-10-08）

実機4構成のPickle全24シナリオが成功した一方で、食事3品のVanilla仮画像 `Things/Item/Meal/Simple` は `Graphic_Single` では読み込めず、ERROR 0を阻害した。さらに以前の `Things/Item/Meal/SimpleMeal` も実機で不在を確認済み。そこで最終画像の承認・制作に先立ち、食事 `AMJC_Houtou` と `AMJC_MilletDumplings` には同梱の `Things/Item/Resource/AMJC_Millet/Millet`、`AMJC_Sobagaki` には同梱の `Things/Item/Resource/AMJC_Buckwheat/Buckwheat` を `Graphic_StackCount` で暫定使用する。各パスは `_a/_b/_c.png` の3枚すべてがGrains内に存在し、静的回帰テストで厳密に照合する。**料理の完成した見た目ではなく、未承認の仮画像**。専用の料理画像を機能テスト合格後に製作し、実機で表示確認する。今回の変更は描画参照のみで、Recipe/食品栄養/腐敗・Mood・加工導線に変更はない。


### Grains 実機四構成スモーク完了（2026-10-08）

作者の `automated-gates(4).log` により、Vanilla / Vanilla+CCTO / MO / MO+CCTOの**各6/6 Pickle・合計24/24、隔離ランタイムERROR 0**を確認。C#コンパイル成功（CS1684警告のみ）、食事の一時画像参照も読込可能で、実収穫・実加工・食事Billを含むテストがすべて成功した。四構成の**実ゲーム・フレッシュ環境のスモークゲートは完了**とする。日英の実表示確認、旧セーブ互換／追加削除、季節変化中の播種・低温成長、専用料理・設備画像、実プレイバランスについてはこのログでは検証していない。GrainsをMOなしで本番公開するための依存メタデータ変更は行っておらず、`About.xml` のMO必須指定は維持する。詳細は `Docs/GrainsProfileTesting.md`（現在の検証正本）と `Docs/Balance/Crops/RicePostHarvestProcessing.md`。旧節の実機未検証記載は当時の履歴であり、実機スモークの最新状態は本節による。


### ネイティブ播種・低温成長E2Eの追加（2026-10-08、実機未検証）

従来の「陸稲の播種可否だけを直接問い合わせるテスト」から一歩進め、既存4×6ケースの実生産シナリオ末尾に `WorkGiver_GrowerSow` が実際の `JobDefOf.Sow` を返し、それをポーンが完了し、`Plant.TickLong` で成長/停止/再開するまでの検査を追加した。25℃陸稲播種→暦15日移動・5℃陸稲播種禁止/生育停止・大麦播種/成長→復温25℃で陸稲生育再開。温度をテスト用に固定する方式のため、実天候・自然季節曲線やCCTO寒害死亡を実証していない。ソースは追加したが実機は未実行。従来版のPickle24/24・ERROR 0はその版の記録として維持し、新版合格扱いに拡張しない。シナリオ数は6件のまま、別途新しい4構成実機の6/6・ERROR 0を確認する。

### 未熟穂の姿勢と共通輪郭色（2026-10-09）

作者の指定によりアワ・ヒエ・キビの未熟穂を下垂しない上向きの形へ更新した。未熟7種（アワ・ヒエ・キビ・ソバ・大麦・AMJ小麦・陸稲）の輪郭は、未熟陸稲から採った代表色 `#4D4E3C` に書出し時のパレットを統一した。この版では従来の生成時の色合わせに加え、256px書出し後も輪郭領域のRGB一致を検証する。アンチエイリアスの境界色は透明度・隣接する塗りに応じる。候補元画像、プロンプト、変更前画像、比較図、検証記録は `Art/Candidates/ImmatureUpright-20261009/` に保存。成熟画像・束・XMLにはこの改訂で変更を加えない。

ローカル画像に反映済み。輪郭のパレット調整による透明度と輪郭以外の画素の不変性、候補QA、全58PNGの整合性を確認。新しい形状は `review` のままとし、過去に承認された原本は保持する。この版の実ゲーム表示・公開は未確認。

### MO併用時の小麦画像（2026-10-09）

MOあり・なしの両構成でAMJ小麦の未熟・成熟・束画像を使用する。MO併用時は既存の `DankPyon_Plant_Wheat` と `DankPyon_RawWheat` を維持し、MO条件付きロード先の `Compatibility/MedievalOverhaul/Patches/WheatGraphics.xml` で画像参照3項目だけを置換する。AMJ小麦をMOなし限定の画像とした説明を訂正する。インストール済みMO 1.6の置換対象・画像実在・他項目不変を静的確認済み。実ゲーム表示は未確認。
