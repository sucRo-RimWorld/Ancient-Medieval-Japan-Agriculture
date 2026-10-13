"""Static Grains JP localization source audit; not historical approval or a game loader test."""
import unittest
import xml.etree.ElementTree as ET
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
SHARED_THINGS = {
    'AMJC_BuckwheatFlour', 'AMJC_MilletFlour', 'AMJC_Houtou',
    'AMJC_Sobagaki', 'AMJC_MilletDumplings',
}
SHARED_RECIPES = {
    'AMJC_MillBuckwheat', 'AMJC_MillMillet', 'AMJC_CookHoutou',
    'AMJC_CookSobagaki', 'AMJC_CookMilletDumplings',
}
BASE_THINGS = {
    'AMJC_Plant_Wheat', 'AMJC_RawWheat',
    'AMJC_WheatFlour', 'AMJC_ManualMillstone',
}
BASE_RECIPES = {'AMJC_MillWheat'}


def labels(relative_path):
    return ET.parse(ROOT / relative_path).getroot()


def all_localizations(*bases):
    found = {}
    for base in bases:
        for path in sorted((ROOT / base).rglob('*.xml')):
            for node in ET.parse(path).getroot():
                assert node.tag not in found, 'Duplicate localized key: ' + node.tag
                found[node.tag] = node.text or ''
    return found


RICE_ITEMS = {'AMJC_RiceSheaf', 'AMJC_RiceInHull', 'RawRice'}
RICE_RECIPES = {'AMJC_ThreshRice', 'AMJC_ThreshRiceBulk',
                'AMJC_HullRice', 'AMJC_HullRiceBulk'}


def validate_rice_localizations(root):
    """Reject drift from the approved JP-first rice processing copy in §10.

    Checks live JP labels/descriptions/jobStrings and English XML defaults.
    It does not claim runtime UI language-switching or historical provenance.
    """
    import re
    source = (root / 'Docs/LocalizationHistoricalReview.md').read_text(encoding='utf-8')
    assert '## 10. 陸稲の新加工語彙' in source
    chapter = source.split('## 10. 陸稲の新加工語彙', 1)[1]
    entries, jobs = {}, {}
    for line in chapter.splitlines():
        matched = re.fullmatch(r'- `(AMJC_[A-Za-z]+|RawRice)`：(.+?)。(.+?)English: \*\*([^*]+)\*\* — (.+)', line)
        if matched:
            name, jp_label, jp_desc, en_label, en_desc = matched.groups()
            assert name not in entries, 'Duplicate approved rice description: ' + name
            entries[name] = (jp_label.strip(' *'), jp_desc.strip(), en_label.strip(), en_desc.strip())
        matched_job = re.fullmatch(r'- `(AMJC_[A-Za-z]+)\.jobString`：(.+?) / (.+)', line)
        if matched_job:
            name, jp, en = matched_job.groups()
            assert name not in jobs, 'Duplicate approved rice jobString: ' + name
            jobs[name] = (jp.strip(), en.strip())
    assert set(entries) == RICE_ITEMS | RICE_RECIPES, 'Rice approved description inventory changed'
    assert set(jobs) == RICE_RECIPES, 'Rice approved jobString inventory changed'

    thing_source = ET.parse(root / 'Defs/ThingDefs_Items/Items_StageA_Grains.xml').getroot()
    recipe_source = ET.parse(root / 'Defs/RecipeDefs/Recipes_GrainProcessing.xml').getroot()
    jp_items = ET.parse(root / 'Languages/Japanese/DefInjected/ThingDef/AMJC_RiceProcessing.xml').getroot()
    jp_recipes = ET.parse(root / 'Languages/Japanese/DefInjected/RecipeDef/AMJC_RiceProcessing.xml').getroot()
    for name, (ja_label, ja_desc, en_label, en_desc) in entries.items():
        if name == 'RawRice':
            assert ja_label == '米' and en_label == 'rice'
            assert jp_items.findtext('RawRice.description') == ja_desc
            assert jp_items.find('RawRice.label') is None
            assert not any(x.findtext('defName') == 'RawRice' for x in thing_source)
            patch = ET.parse(root / 'Patches/RawRiceDescription.xml').getroot()
            patch_ops = patch.findall('.//li')
            assert len(patch_ops) == 1, 'Only a description patch for Vanilla RawRice'
            operation = patch_ops[0]
            assert operation.get('Class') == 'PatchOperationReplace'
            assert operation.findtext('xpath') == '/Defs/ThingDef[defName="RawRice"]/description'
            assert operation.findtext('value/description') == en_desc
            assert ja_desc.startswith('米（こめ）。')
            for term in ('江戸患い', '高木兼寛', '疫学', '1910年', '鈴木梅太郎', 'オリザニン'):
                assert term in ja_desc
            for term in ('Edo sickness', 'Kanehiro Takaki', 'epidemiological', '1910', 'Umetaro Suzuki', 'oryzanin'):
                assert term in en_desc
            assert ja_desc.count(r'\n\n') == en_desc.count(r'\n\n') == 3
            continue
        source_group = thing_source if name in RICE_ITEMS else recipe_source
        jp_group = jp_items if name in RICE_ITEMS else jp_recipes
        nodes = [node for node in source_group if node.findtext('defName') == name]
        assert len(nodes) == 1, 'Missing or duplicate rice Def: ' + name
        node = nodes[0]
        assert node.findtext('label') == en_label, 'English rice label drift: ' + name
        assert node.findtext('description') == en_desc, 'English rice description drift: ' + name
        assert jp_group.findtext(name + '.label') == ja_label, 'Japanese rice label drift: ' + name
        assert jp_group.findtext(name + '.description') == ja_desc, 'Japanese rice description drift: ' + name
        if name in ('AMJC_RiceSheaf', 'AMJC_RiceInHull'):
            assert ja_desc.count(r'\n\n') == en_desc.count(r'\n\n') == 1
            assert len(ja_desc) < 280, 'Avoid unreadably long rice-stage text'
            if name == 'AMJC_RiceSheaf':
                assert '年貢' in ja_desc and 'commodity money' in en_desc
            else:
                assert all(term in ja_desc for term in ('玄米', '白米', 'ビタミンB1'))
                assert 'brown rice' in en_desc and 'white rice' in en_desc
        if name in RICE_RECIPES:
            assert node.findtext('jobString') == jobs[name][1], 'English rice jobString drift: ' + name
            assert jp_group.findtext(name + '.jobString') == jobs[name][0], 'Japanese rice jobString drift: ' + name


class JapaneseLocalizationAudit(unittest.TestCase):
    def test_labels_and_approved_description_inventory(self):
        audit = (ROOT / 'Docs/LocalizationHistoricalReview.md').read_text(encoding='utf-8')
        self.assertIn('作者承認済み', audit)
        shared = all_localizations('Languages/Japanese/DefInjected')
        vanilla = all_localizations('Languages/Japanese/DefInjected',
                                    'BaseWithoutMO/Languages/Japanese/DefInjected')
        medieval = all_localizations('Languages/Japanese/DefInjected',
                                     'Compatibility/MedievalOverhaul/Languages/Japanese/DefInjected')
        self.assertIn('Plant_Rice.label', shared)
        self.assertIn('Plant_Rice.description', shared)
        for name in sorted(SHARED_THINGS | SHARED_RECIPES):
            self.assertTrue(shared.get(name + '.label', '').strip(), name)
            self.assertIn('`' + name + '`', audit, name)
        for name in sorted(BASE_THINGS | BASE_RECIPES):
            self.assertTrue(vanilla.get(name + '.label', '').strip(), name)
            self.assertIn('`' + name + '`', audit, name)
        for name in BASE_THINGS | BASE_RECIPES:
            self.assertNotIn(name + '.label', medieval, 'Base-only label leaked into MO: ' + name)
        for name in ('AMJC_ThreshWheat', 'AMJC_ThreshWheatBulk'):
            self.assertIn(name + '.description', vanilla)
            self.assertIn(name + '.description', medieval)

    def test_new_flour_functional_localizations_present(self):
        item_ja = labels('Languages/Japanese/DefInjected/ThingDef/AMJC_GrainsChain.xml')
        recipe_ja = labels('Languages/Japanese/DefInjected/RecipeDef/AMJC_GrainsChain.xml')
        sources = [
            ET.parse(ROOT/'Defs/ThingDefs_Items/Items_GrainsFlour.xml').getroot(),
            ET.parse(ROOT/'Defs/ThingDefs_Items/Items_GrainsFood.xml').getroot(),
            ET.parse(ROOT/'Defs/RecipeDefs/Recipes_GrainsMilling.xml').getroot(),
            ET.parse(ROOT/'Defs/RecipeDefs/Recipes_GrainsFood.xml').getroot(),
        ]
        items = ('AMJC_BarleyFlour','AMJC_RiceFlour',
                 'AMJC_BarleyFlourGruel','AMJC_RiceFlourDumplings')
        recs = ('AMJC_MillBarley','AMJC_MillRice',
                'AMJC_CookBarleyFlourGruel','AMJC_CookRiceFlourDumplings')
        for name in items + recs:
            found = [n for group in sources for n in group if n.findtext('defName') == name]
            self.assertEqual(len(found),1,name)
            ja = item_ja if name in items else recipe_ja
            keys = ('label','description') if name in items else ('label','description','jobString')
            for key in keys:
                self.assertTrue((found[0].findtext(key) or '').strip(),name+'.'+key)
                self.assertTrue((ja.findtext(name+'.'+key) or '').strip(),name+'.'+key)

    def test_bulk_flour_recipes_have_functional_japanese_and_english(self):
        japanese = labels('Languages/Japanese/DefInjected/RecipeDef/AMJC_GrainsChain.xml')
        source = ET.parse(ROOT/'Defs/RecipeDefs/Recipes_GrainsFood.xml').getroot()
        foods = ('Houtou','Sobagaki','MilletDumplings','BarleyFlourGruel','RiceFlourDumplings')
        # Protect exactly the five author-reviewed special-food batches.
        # The three separately guarded generic fallback batches must not
        # cause historical-copy acceptance to expand implicitly.
        specialized = {'AMJC_Cook' + suffix + 'Bulk' for suffix in foods}
        self.assertEqual({n.findtext('defName') for n in source.findall('RecipeDef')
                          if n.findtext('defName') in specialized}, specialized)
        for suffix in foods:
            name = 'AMJC_Cook'+suffix+'Bulk'
            rec = source.find('RecipeDef[defName="' + name + '"]')
            self.assertIsNotNone(rec, name)
            for field in ('label', 'description', 'jobString'):
                self.assertTrue((rec.findtext(field) or '').strip(), name+'.'+field)
                self.assertTrue((japanese.findtext(name+'.'+field) or '').strip(),
                                name+'.'+field)
            self.assertIn('4食', japanese.findtext(name+'.label'))
            self.assertIn('栄養2.0', japanese.findtext(name+'.description'))
            self.assertEqual(rec.findtext('products/AMJC_'+suffix), '4')

    def test_base_facts_match_real_recipe_outputs(self):
        shared = labels('Languages/Japanese/DefInjected/RecipeDef/AMJC_StageA.xml')
        base = labels('BaseWithoutMO/Languages/Japanese/DefInjected/RecipeDef/AMJC_MOWheat.xml')
        mo = labels('Compatibility/MedievalOverhaul/Languages/Japanese/DefInjected/RecipeDef/AMJC_MOWheat.xml')
        for crop in ('Millet', 'Buckwheat', 'Barley'):
            for suffix in ('', 'Bulk'):
                key = 'AMJC_Thresh' + crop + suffix + '.description'
                node = shared.find(key)
                self.assertIsNotNone(node, key)
                self.assertNotIn('藁', node.text or '', key + ' cannot claim Base straw output')
                self.assertIn('殻付き', node.text or '', key)
        for suffix in ('', 'Bulk'):
            key = 'AMJC_ThreshWheat' + suffix + '.description'
            self.assertNotIn('藁', base.findtext(key), key)
            self.assertIn('藁', mo.findtext(key), key)
        for path in ('Defs/RecipeDefs/Recipes_GrainProcessing.xml',
                     'BaseWithoutMO/Defs/Recipes_Wheat.xml'):
            document = ET.parse(ROOT / path).getroot()
            for recipe in document.findall('RecipeDef'):
                if (recipe.findtext('defName') or '').startswith('AMJC_Thresh'):
                    self.assertIsNone(recipe.find('products/DankPyon_Straw'))
        mo_recipes = [
            recipe for recipe in ET.parse(
                ROOT / 'Compatibility/MedievalOverhaul/Defs/Recipes_MOWheat.xml').getroot().findall('RecipeDef')
            if (recipe.findtext('defName') or '') in ('AMJC_ThreshWheat', 'AMJC_ThreshWheatBulk')
        ]
        self.assertEqual(len(mo_recipes), 2)
        for recipe in mo_recipes:
            self.assertIsNotNone(recipe.find('products/DankPyon_Straw'))

    def test_approved_rice_processing_copy_matches_source(self):
        validate_rice_localizations(ROOT)

    def test_rice_copy_mutations_are_rejected(self):
        import shutil
        import tempfile
        relative = (
            'Docs/LocalizationHistoricalReview.md',
            'Defs/ThingDefs_Items/Items_StageA_Grains.xml',
            'Defs/RecipeDefs/Recipes_GrainProcessing.xml',
            'Languages/Japanese/DefInjected/ThingDef/AMJC_RiceProcessing.xml',
            'Languages/Japanese/DefInjected/RecipeDef/AMJC_RiceProcessing.xml',
            'Patches/RawRiceDescription.xml',
        )
        for target, old, new in (
            (relative[1], '<label>rice sheaf</label>', '<label>unreviewed sheaf</label>'),
            (relative[3], '収穫した稲を束ねたもの。', '収穫した稲を乾燥させたもの。'),
            (relative[2], '<jobString>Hulling rice in bulk.</jobString>',
             '<jobString>Milling rice in bulk.</jobString>'),
            (relative[0], '籾10個をまとめて籾摺りし', '籾100個をまとめて籾摺りし'),
            (relative[5], 'Edo sickness', 'altered sickness'),
        ):
            with self.subTest(file=target), tempfile.TemporaryDirectory() as directory:
                root = Path(directory)
                for name in relative:
                    destination = root / name
                    destination.parent.mkdir(parents=True, exist_ok=True)
                    shutil.copyfile(ROOT / name, destination)
                original = (root / target).read_text(encoding='utf-8')
                self.assertIn(old, original)
                (root / target).write_text(original.replace(old, new), encoding='utf-8')
                with self.assertRaises(AssertionError):
                    validate_rice_localizations(root)

    def test_flour_food_mood_stage_has_japanese_description(self):
        source = labels('Defs/ThoughtDefs/Thoughts_GrainsFood.xml')
        japanese = labels('Languages/Japanese/DefInjected/ThoughtDef/AMJC_GrainsFood.xml')
        stage = source.find("ThoughtDef[defName='AMJC_AteFlourFood']/stages/li")
        self.assertIsNotNone(stage)
        self.assertEqual(stage.findtext('description'), 'I ate a dish made from ground grain.')
        self.assertEqual(japanese.findtext('AMJC_AteFlourFood.stages.0.label'), '粉食を食べた')
        self.assertEqual(japanese.findtext('AMJC_AteFlourFood.stages.0.description'),
                         '挽いた穀物から作った料理を食べた。')

    def test_stage_and_shared_meal_copy_matches_author_review(self):
        """Exact bilingual source paragraphs; checks definitions, not loaded game UI."""
        source = (ROOT/'Docs/LocalizationHistoricalReview.md').read_text(
            encoding='utf-8')
        self.assertIn('## 16. 作者指示による穀粒状態', source)
        rows = {}
        for line in source.split('## 16.', 1)[1].splitlines():
            if not line.startswith('| `AMJC_'):
                continue
            cells = [x.strip() for x in line.split('|')]
            self.assertEqual(len(cells), 5, line)
            name = cells[1].strip('`')
            self.assertNotIn(name, rows)
            rows[name] = (cells[2], cells[3])
        grain_names = {
            'AMJC_RawMillet', 'AMJC_MilletInHull', 'AMJC_Millet',
            'AMJC_RawBuckwheat', 'AMJC_BuckwheatInHull', 'AMJC_Buckwheat',
            'AMJC_RawBarley', 'AMJC_BarleyInHull', 'AMJC_Barley',
        }
        meal_names = {
            'AMJC_MixedGrainMeal', 'AMJC_MixedGrainPorridge',
            'AMJC_MixedFlourFood',
        }
        self.assertEqual(set(rows), grain_names | meal_names)
        ja_grains = labels('Languages/Japanese/DefInjected/ThingDef/AMJC_StageA.xml')
        ja_foods = labels('Languages/Japanese/DefInjected/ThingDef/AMJC_GrainsChain.xml')
        en_grains = labels('Defs/ThingDefs_Items/Items_StageA_Grains.xml')
        en_foods = labels('Defs/ThingDefs_Items/Items_GrainsFood.xml')
        for name, (japanese, english) in rows.items():
            with self.subTest(name=name):
                jp = ja_grains if name in grain_names else ja_foods
                en = en_grains if name in grain_names else en_foods
                self.assertEqual(jp.findtext(name + '.description'), japanese)
                item = en.find('ThingDef[defName="' + name + '"]')
                self.assertIsNotNone(item)
                self.assertEqual(item.findtext('description'), english)
                self.assertTrue(japanese.endswith('。'))
                self.assertTrue(english.endswith('.'))

        self.assertNotEqual(
            rows['AMJC_MixedGrainMeal'], rows['AMJC_MixedGrainPorridge'])
        self.assertIn('粥より日持ちする', rows['AMJC_MixedGrainMeal'][0])
        self.assertIn('常温では傷みやすい', rows['AMJC_MixedGrainPorridge'][0])
        for name in meal_names:
            self.assertIn('一種類', rows[name][0])
            self.assertIn('one', rows[name][1].lower())

        # Production behavior assumed by descriptions: common grain meal
        # 240 work / 3 days; porridge 200 / 1.5; flour 300 / 2.
        recipes = labels('Defs/RecipeDefs/Recipes_GrainsFood.xml')
        metrics = (
            ('MixedGrainMeal', 240, 3.0),
            ('MixedGrainPorridge', 200, 1.5),
            ('MixedFlourFood', 300, 2.0),
        )
        for suffix, work, rot in metrics:
            food = en_foods.find('ThingDef[defName="AMJC_' + suffix + '"]')
            recipe = recipes.find('RecipeDef[defName="AMJC_Cook' + suffix + '"]')
            bulk = recipes.find('RecipeDef[defName="AMJC_Cook' + suffix + 'Bulk"]')
            self.assertIsNotNone(recipe)
            self.assertIsNotNone(bulk)
            self.assertEqual(float(recipe.findtext('workAmount')), work)
            self.assertEqual(float(bulk.findtext('workAmount')), work * 4)
            self.assertEqual(food.findtext(
                "comps/li[@Class='CompProperties_Rottable']/daysToRotStart"),
                str(rot).rstrip('0').rstrip('.'))
            self.assertEqual(recipe.findtext('allowMixingIngredients'), 'true')

    def test_stone_mill_display_name_and_five_flour_paths(self):
        """UI label changes; preserve savegame DefName and Base/MO split."""
        lang = labels('BaseWithoutMO/Languages/Japanese/DefInjected/ThingDef/AMJC_WheatFallback.xml')
        wheat = labels('BaseWithoutMO/Defs/Items_Wheat.xml')
        stone = labels('BaseWithoutMO/Defs/Buildings_Millstone.xml')
        text_ja = lang.findtext('AMJC_RawWheat.description')
        text_en = wheat.findtext('ThingDef[defName="AMJC_RawWheat"]/description')
        self.assertEqual(lang.findtext('AMJC_RawWheat.label'), '小麦束')
        self.assertTrue(text_ja.startswith('穂と茎を付けたまま'))
        self.assertTrue(text_en.startswith('Harvested wheat'))
        self.assertNotIn('こむぎたば', text_ja)
        self.assertNotIn('komugitaba', text_en)
        self.assertEqual(lang.findtext('AMJC_ManualMillstone.label'), '石臼')
        self.assertEqual(stone.findtext(
            'ThingDef[defName="AMJC_ManualMillstone"]/label'), 'stone mill')
        description = lang.findtext('AMJC_ManualMillstone.description')
        for grain in ('小麦', '蕎麦', '雑穀', '大麦', '米'):
            self.assertIn(grain, description)
        for value in ('wheat', 'buckwheat', 'millet', 'barley', 'rice'):
            self.assertIn(value, stone.findtext(
                'ThingDef[defName="AMJC_ManualMillstone"]/description'))

        all_mills = [
            labels('BaseWithoutMO/Defs/Recipes_Milling.xml'),
            labels('Defs/RecipeDefs/Recipes_GrainsMilling.xml'),
        ]
        expected = {
            'AMJC_MillWheat', 'AMJC_MillBuckwheat', 'AMJC_MillMillet',
            'AMJC_MillBarley', 'AMJC_MillRice',
        }
        mills = [recipe for source in all_mills for recipe in source
                 if recipe.findtext('defName') in expected]
        self.assertEqual({r.findtext('defName') for r in mills}, expected)
        for recipe in mills:
            self.assertIn('AMJC_ManualMillstone',
                          [li.text for li in recipe.findall('recipeUsers/li')])
            self.assertEqual(float(recipe.findtext('workAmount')), 300)
        mo = labels('Compatibility/MedievalOverhaul/Languages/Japanese/DefInjected/ThingDef/AMJC_MO_Overrides.xml')
        self.assertIsNone(mo.find('AMJC_ManualMillstone.label'))

    def test_shared_thing_explanations_do_not_claim_unimplemented_features(self):
        things = labels('Languages/Japanese/DefInjected/ThingDef/AMJC_StageA.xml')
        barley = things.findtext('AMJC_Plant_Barley.description')
        edible = things.findtext('AMJC_Barley.description')
        wheat = things.findtext('AMJC_Wheat.description')
        self.assertNotIn('基礎農業研究が必要', barley)
        self.assertNotIn('麦茶', edible)
        self.assertNotIn('麦味噌', edible)
        self.assertNotIn('Medieval Overhaul', wheat)
        self.assertIn('製粉', wheat)

    def test_six_approved_crop_descriptions_match_live_japanese_and_english(self):
        review = (ROOT/'Docs/LocalizationHistoricalReview.md').read_text(encoding='utf-8')
        self.assertIn('## 7. 既存六作物の承認済み歴史説明', review)
        self.assertIn('## 8. English descriptions translated', review)
        japanese_section = review.split('## 7.', 1)[1].split('## 8.', 1)[0]
        english_section = review.split('## 8.', 1)[1]
        self.assertIn('日本語説明を作者承認済み', japanese_section)

        def approved_table(section):
            rows = {}
            for line in section.splitlines():
                if not line.startswith('| `'):
                    continue
                cells = [cell.strip() for cell in line.split('|')]
                name = cells[1].strip('`')
                self.assertNotIn(name, rows, 'duplicate approved crop: ' + name)
                rows[name] = cells[2].replace('**', '').replace('。 ', '。')
            return rows

        japanese = approved_table(japanese_section)
        english = approved_table(english_section)
        names = {
            'AMJC_Plant_FoxtailMillet_Awa': '粟（あわ）',
            'AMJC_Plant_BarnyardMillet_Hie': '稗（ひえ）',
            'AMJC_Plant_ProsoMillet_Kibi': '黍（きび）',
            'AMJC_Plant_Buckwheat_Soba': '蕎麦（そば）',
            'AMJC_Plant_Barley': '大麦（おおむぎ）',
            'Plant_Rice': '稲（いね）',
        }
        self.assertEqual(set(japanese), set(names))
        self.assertEqual(set(english), set(names))
        localizations = all_localizations('Languages/Japanese/DefInjected')
        plants = {
            item.findtext('defName'): item
            for item in ET.parse(ROOT/'Defs/ThingDefs_Plants/Plants_StageA.xml').getroot()
            if item.tag == 'ThingDef'
        }
        for name, name_form in names.items():
            self.assertTrue(japanese[name].startswith(name_form), name)
            self.assertIn('AMJGrains', japanese[name], name)
            self.assertIn('AMJGrains', english[name], name)
            self.assertIn(name + '.label', localizations)
            self.assertEqual(localizations[name + '.description'], japanese[name],
                             'Japanese crop description drift: ' + name)
            if name != 'Plant_Rice':
                self.assertIn(name, plants)
                self.assertEqual(plants[name].findtext('description'), english[name],
                                 'English PlantDef description drift: ' + name)

        patches = ET.parse(ROOT/'Patches/UplandRice.xml').getroot()
        rice_target = '/Defs/ThingDef[defName="Plant_Rice"]/description'
        matched = [node.findtext('value/description') for node in patches.findall('.//li')
                   if node.findtext('xpath') == rice_target]
        self.assertEqual(matched, [english['Plant_Rice']],
                         'English shared rice Patch description drift')
        rice_label_target = '/Defs/ThingDef[defName="Plant_Rice"]/label'
        rice_labels = [node.findtext('value/label') for node in patches.findall('.//li')
                       if node.findtext('xpath') == rice_label_target]
        self.assertEqual(rice_labels, ['rice plant'])
        self.assertEqual(localizations['Plant_Rice.label'], '稲')
        # Cultivation is the only history owned by the living plant's description.
        for term in ('水稲', '陸稲', '同じイネ', '弥生時代', 'AMJGrains'):
            self.assertIn(term, japanese['Plant_Rice'])
        for term in ('same rice species', 'Yayoi period', 'AMJGrains'):
            self.assertIn(term, english['Plant_Rice'])
        for term in ('江戸患い', 'オリザニン', '高木兼寛', '年貢', 'ビタミンB1'):
            self.assertNotIn(term, japanese['Plant_Rice'])
        for item in (japanese['Plant_Rice'], english['Plant_Rice']):
            self.assertEqual(item.count(r'\n\n'), 1)
        self.assertLess(max(map(len, japanese['Plant_Rice'].split(r'\n\n'))), 220)
        for forbidden in ('V1', '未実装', '別Mod'):
            self.assertNotIn(forbidden, japanese['Plant_Rice'],
                             'Release stage / ownership must not appear in player prose')
        for forbidden in ('V1', 'not implemented', 'another mod'):
            self.assertNotIn(forbidden, english['Plant_Rice'],
                             'Release stage / ownership must not appear in player prose')
        self.assertIn('霜への強さまで保証するものではない',
                      japanese['AMJC_Plant_BarnyardMillet_Hie'])
        self.assertIn('does not guarantee greater resistance to frost',
                      english['AMJC_Plant_BarnyardMillet_Hie'])
        self.assertIn('古い蕎麦の食べ方と', japanese['AMJC_Plant_Buckwheat_Soba'])
        self.assertIn('Edo period', english['AMJC_Plant_Buckwheat_Soba'])

        properties = {name: node.find('plant') for name, node in plants.items()}
        self.assertLess(float(properties['AMJC_Plant_BarnyardMillet_Hie'].findtext('minGrowthTemperature')),
                        float(properties['AMJC_Plant_FoxtailMillet_Awa'].findtext('minGrowthTemperature')))
        self.assertLess(float(properties['AMJC_Plant_ProsoMillet_Kibi'].findtext('growDays')),
                        float(properties['AMJC_Plant_FoxtailMillet_Awa'].findtext('growDays')))
        self.assertLess(float(properties['AMJC_Plant_Buckwheat_Soba'].findtext('growDays')),
                        float(properties['AMJC_Plant_Barley'].findtext('growDays')))
        upland = (ROOT/'Patches/UplandRice.xml').read_text(encoding='utf-8')
        self.assertIn('<fertilityMin>0.7</fertilityMin>', upland)
        self.assertIn('<minGrowthTemperature>10</minGrowthTemperature>', upland)
        self.assertIn('<maxGrowthTemperature>42</maxGrowthTemperature>', upland)

    def test_approved_japanese_english_parity_and_recipe_work_strings(self):
        audit = (ROOT/'Docs/LocalizationHistoricalReview.md').read_text(encoding='utf-8')
        self.assertIn('作者承認済み', audit)
        self.assertIn('国土交通省', audit)
        self.assertIn('## 6. English localization', audit)

        def table(section):
            values = {}
            for line in section.splitlines():
                if not line.startswith('| `AMJC_'):
                    continue
                cells = [cell.strip() for cell in line.split('|')]
                if len(cells) < 4 or not cells[1].startswith('`AMJC_') or not cells[1].endswith('`'):
                    continue
                name = cells[1].strip('`')
                description = cells[2].replace('**', '').replace('。 ', '。')
                job = cells[3] if len(cells) >= 5 and cells[3] != '—' else ''
                self.assertNotIn(name, values, 'duplicate approval row')
                values[name] = (description, job)
            return values

        jp = table(audit.split('## 2.', 1)[1].split('## 3.', 1)[0])
        en = table(audit.split('## 6.', 1)[1].split('## 7.', 1)[0])
        names = SHARED_THINGS | SHARED_RECIPES | BASE_THINGS | BASE_RECIPES
        self.assertEqual(set(jp), names)
        self.assertEqual(set(en), names)

        japanese = all_localizations('Languages/Japanese/DefInjected',
                                    'BaseWithoutMO/Languages/Japanese/DefInjected')
        source_groups = (
            ('Defs/ThingDefs_Items/Items_GrainsFlour.xml', SHARED_THINGS),
            ('Defs/ThingDefs_Items/Items_GrainsFood.xml', SHARED_THINGS),
            ('Defs/RecipeDefs/Recipes_GrainsMilling.xml', SHARED_RECIPES),
            ('Defs/RecipeDefs/Recipes_GrainsFood.xml', SHARED_RECIPES),
            ('BaseWithoutMO/Defs/Plants_Wheat.xml', BASE_THINGS),
            ('BaseWithoutMO/Defs/Items_Wheat.xml', BASE_THINGS),
            ('BaseWithoutMO/Defs/Items_Flour.xml', BASE_THINGS),
            ('BaseWithoutMO/Defs/Buildings_Millstone.xml', BASE_THINGS),
            ('BaseWithoutMO/Defs/Recipes_Milling.xml', BASE_RECIPES),
        )
        seen = set()
        for path, relevant in source_groups:
            for node in ET.parse(ROOT/path).getroot():
                name = node.findtext('defName')
                if name not in relevant:
                    continue
                self.assertNotIn(name, seen, 'duplicate Def across runtime scopes: ' + name)
                seen.add(name)
                self.assertEqual(japanese[name+'.description'], jp[name][0],
                                 'Japanese description drift: ' + name)
                self.assertEqual(node.findtext('description'), en[name][0],
                                 'English description drift: ' + name)
                if name in SHARED_RECIPES | BASE_RECIPES:
                    self.assertTrue(jp[name][1], 'missing Japanese jobString: ' + name)
                    self.assertTrue(en[name][1], 'missing English jobString: ' + name)
                    self.assertEqual(japanese[name+'.jobString'], jp[name][1])
                    self.assertEqual(node.findtext('jobString'), en[name][1])
                else:
                    self.assertEqual(jp[name][1], '')
                    self.assertEqual(en[name][1], '')
        self.assertEqual(seen, names)

        wheat = japanese['AMJC_Plant_Wheat.description']
        self.assertIn('小麦の穀粒は食事の材料に使え', wheat)
        self.assertNotIn('実は食材になる', wheat)
        for name in ('AMJC_MilletFlour', 'AMJC_Plant_Wheat',
                     'AMJC_WheatFlour', 'AMJC_ManualMillstone'):
            self.assertIn('AMJGrains', japanese[name+'.description'], name)
            self.assertIn('AMJGrains', en[name][0], name)
        self.assertIn('料理として同一だったとは断定できない',
                      japanese['AMJC_Houtou.description'])
        self.assertIn('cannot be assumed to be the same dish',
                      en['AMJC_Houtou'][0])


if __name__ == '__main__':
    unittest.main()
