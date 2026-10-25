# 2026-10-25 MO sweet pancakes and grape history audit

**Owner:** Agriculture, optional MO 1.6. **Status:** Western pancake ordinary-source exclusion; cultivated-grape historical status HOLD; no new dish.

## Original MO and patch scope

User-supplied original MO 1.6.2.2 (Workshop 3219596926), `1.6/Defs/RecipeDefs/Recipes_Meals.xml` has `DankPyon_CookMealSweetPancakes` and `DankPyon_CookMealSweetPancakesBulk`, both with explicit `recipeUsers` (one/five meals). They consume MO `DankPyon_Sugar`, flour, egg and milk. `1.6/Defs/ThingDefs_Items/Items_Food.xml` describes the resulting `DankPyon_MealSweetPancakes` as fluffy pancakes with sweet syrup. `Patches/MO_SweetPancakesExclusion.xml` clears the two Bill users and sets only the finished pancake food `tradeability=None`. It retains all original DefNames for saves/other Mods and alters no game/nutrition/cooking numeric values. There is no mandate to create an alternate sweet.

## History and certainty

- Yao Keisuke, ALIC, *History of sugar food in Japan*: https://www.alic.go.jp/joho-s/joho07_000217.html. Mentions `さたうまんぢう` in `七十一番職人歌合` and sugar yokan in `庭訓往来`, supporting sugar confection names in Muromachi-era records; **not a verified sugar-manju ingredient formula** or mood, work, price, nutrition or image. Prior invented prototype Draft PR #264 was closed unmerged; no asset/recipe adopted.
- National Diet Library, collaborative reference answer on Pancake and Hotcake: https://crd.ndl.go.jp/reference/entry/reference/show?id=1000354094&page=ref_view. Modern Japanese pancake introduction is Meiji (1884 named printed reference) and common commercial hotcake is Taisho (1923). This does not rule out all historic skillet bread worldwide, only MO's **specific syrup-stacked Western confection** as routine Japanese medieval food.
- Harada Nobuo, *Edo Fruit: Grapes and Wine*, National Museum of Japanese History botanical bulletin No.176 (2007): https://archive.rekihaku.ac.jp/exhibitions/plant/column/2007/176.pdf. Publicly indexed abstract/text indicates no reliable Japanese grape cultivation attestation through Muromachi and 1186 origin has weak support. **Limit:** original PDF open produced 502; visual page review unavailable, no more precise claim made.
- Yamanashi Prefecture historic grapes: https://www.pref.yamanashi.jp/oishii-mirai/nochikusanbutsu/budou.html ; Kofu city: https://www.city.kofu.yamanashi.jp/welcome/brand/budo.html . 718/1186 early-vine origins presented explicitly as legends; `本朝食鑑` (1695) corroborates Edo specialty, not medieval documentation.
- Agency for Cultural Affairs preserved grape/vine tools: https://kunishitei.bunka.go.jp/heritage/detail/311/00000248 . Describes Edo trellis expansion and Meiji modern winemaking.

**Decision boundary:** 718/1186 traditions are not proof of cultivated Japanese grapes in Kamakura/Muromachi; absence of contemporary records is not proof no grapevine ever existed. MO `DankPyon_Plant_Grape` plus `DankPyon_VinePost`/raw fruit are source-retained, pending explicit historic crop adoption, with no current grape numeric, sow tag, images or research change. Existing `MO_FruitWineProductionExclusion.xml` preserves imported finished wine while disconnecting local modern grape/mulberry wine; ancient mulberry sericulture is unaffected. Finished `DankPyon_Sugar` stays trader count3–6 and value5.0. No replacement meal is forced even when no approved cooking Bill consumes it.

**Validation boundary:** MO-original XML XPath targeted match and XML syntax are static checks, NOT loaded 1.6/1.7 kitchen, trade, old-save or economy acceptance; native runs deliberately not triggered.
