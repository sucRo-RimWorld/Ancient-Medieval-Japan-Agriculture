"""Contracts for V1 meals and wiring of deferred native Bill execution."""
import hashlib
import json
import sys
import unittest
import xml.etree.ElementTree as ET
from copy import deepcopy
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
sys.path.insert(0, str(ROOT / 'Tests'))
from amj_profile_xml import profile_xml
from stack_graphics import stack_textures

GRAINS = {'AMJC_Millet','AMJC_Buckwheat','AMJC_Barley','AMJC_Wheat','RawRice'}
OWN_FLOURS = {'AMJC_MilletFlour','AMJC_BuckwheatFlour','AMJC_BarleyFlour','AMJC_RiceFlour'}
MENU = {
    'MixedGrainMeal': (GRAINS, 240, 3.0,
                       'Things/Item/Meal/AMJC_MixedGrainMeal/AMJC_MixedGrainMeal'),
    'MixedGrainPorridge': (GRAINS, 200, 1.5,
                           'Things/Item/Meal/AMJC_MixedGrainPorridge/AMJC_MixedGrainPorridge'),
    'MixedFlourFood': (None, 300, 2.0,
                       'Things/Item/Meal/AMJC_MixedFlourFood/AMJC_MixedFlourFood')
}
SPECIFIC = ('Houtou','Sobagaki','MilletDumplings',
            'BarleyFlourGruel','RiceFlourDumplings')


def require_recipe(node, ingredient_set, suffix, multiplier, work):
    assert node is not None and node.tag == 'RecipeDef'
    assert node.findtext('allowMixingIngredients') == 'true', suffix
    assert node.findtext('ingredientValueGetterClass') == 'IngredientValueGetter_Nutrition'
    assert node.findtext('workSpeedStat') == 'CookSpeed'
    assert node.findtext('workSkill') == 'Cooking'
    assert float(node.findtext('workAmount')) == multiplier * work, suffix
    assert len(node.findall('ingredients/li')) == 1
    actual = {li.text for li in node.findall('ingredients/li/filter/thingDefs/li')}
    fixed = {li.text for li in node.findall('fixedIngredientFilter/thingDefs/li')}
    assert len(node.findall('ingredients/li/filter/thingDefs/li')) == 5
    assert len(node.findall('fixedIngredientFilter/thingDefs/li')) == 5
    assert actual == fixed == ingredient_set, (suffix, actual, fixed)
    assert node.findtext('fixedIngredientFilter/specialFiltersToDisallow/li') == 'AllowRotten'
    assert float(node.findtext('ingredients/li/count')) == 0.5 * multiplier
    assert [(x.tag, int(x.text)) for x in node.findall('products/*')] == [
        ('AMJC_'+suffix, multiplier)]
    assert [li.text for li in node.findall('recipeUsers/li')] == [
        'Campfire','ElectricStove','FueledStove']
    assert node.find('researchPrerequisite') is None
    assert node.find('researchPrerequisites') is None
    assert node.find('skillRequirements') is None


def validate(root=ROOT):
    for profile in ('vanilla','mo'):
        doc = profile_xml(profile, root)
        defs = {x.findtext('defName'):x for x in doc if x.findtext('defName')}
        flour_set = OWN_FLOURS | {
            'DankPyon_Flour' if profile == 'mo' else 'AMJC_WheatFlour'
        }
        assert len(flour_set) == 5
        for suffix, (raw, work, spoil, tex) in MENU.items():
            item = defs['AMJC_'+suffix]
            # The Vanilla FineMeal parent inherits AteFineMeal (+5), even if XML
            # omits tasteThought. CookedIngredientless has no tasteThought.
            assert item.tag == 'ThingDef' and item.get('ParentName') == 'MealCookedIngredientless'
            assert float(item.findtext('statBases/Nutrition')) == 0.9
            assert item.findtext('ingestible/preferability') == 'MealSimple'
            assert item.find('ingestible/tasteThought') is None, suffix
            assert item.find('comps').get('Inherit') == 'False'
            assert [x.get('Class') for x in item.findall('comps/li')] == [
                'CompProperties_Forbiddable','CompProperties_Ingredients',
                'CompProperties_FoodPoisonable','CompProperties_Rottable']
            assert float(item.findtext(
                "comps/li[@Class='CompProperties_Rottable']/daysToRotStart")) == spoil
            assert item.findtext('graphicData/graphicClass') == 'Graphic_StackCount'
            assert item.findtext('graphicData/texPath') == tex
            stack_textures(root, tex)
            pool = flour_set if raw is None else raw
            for batch in (1,4):
                name = 'AMJC_Cook'+suffix+('Bulk' if batch == 4 else '')
                require_recipe(defs[name], pool, suffix, batch, work)
        # Old save and author-owned specialty food paths must not be replaced.
        for suffix in SPECIFIC:
            food = defs['AMJC_'+suffix]
            assert food.findtext('ingestible/tasteThought') == 'AMJC_AteFlourFood'
            assert float(food.findtext(
                "comps/li[@Class='CompProperties_Rottable']/daysToRotStart")) == 2.5
            for tag in ('','Bulk'):
                recipe = defs['AMJC_Cook'+suffix+tag]
                assert recipe.find('allowMixingIngredients') is None or (
                    recipe.findtext('allowMixingIngredients') == 'false')
                assert len(recipe.findall('fixedIngredientFilter/thingDefs/li')) == 1
                assert len(recipe.findall('ingredients/li/filter/thingDefs/li')) == 1
    patch = ET.parse(root/'Compatibility/MedievalOverhaul/Patches/'
                     'MedievalOverhaul_GrainsFlour.xml').getroot()
    for suff in ('MixedFlourFood','MixedFlourFoodBulk'):
        for tail in ('ingredients/li/filter/thingDefs/li[5]',
                     'fixedIngredientFilter/thingDefs/li[5]'):
            path = '/Defs/RecipeDef[defName="AMJC_Cook'+suff+'"]/'+tail
            matches = [x for x in patch.findall('Operation') if x.findtext('xpath') == path]
            assert len(matches) == 1, path
            assert matches[0].get('Class') == 'PatchOperationReplace'
            assert matches[0].findtext('value/li') == 'DankPyon_Flour'
    return True


class MixedCookingRegression(unittest.TestCase):
    def test_actual_base_and_mo_xml_contracts(self):
        self.assertTrue(validate())

    def test_common_meals_cannot_inherit_vanilla_fine_meal_thought(self):
        root = ET.parse(ROOT/'Defs/ThingDefs_Items/Items_GrainsFood.xml').getroot()
        for suffix in MENU:
            item = deepcopy(root.find('ThingDef[defName="AMJC_'+suffix+'"]'))
            self.assertEqual(item.get('ParentName'), 'MealCookedIngredientless')
            self.assertIsNone(item.find('ingestible/tasteThought'))
            # The inherited FineMeal tasteThought was observed in actual
            # Linux 1.6 Pickle, despite no explicit child XML element.
            item.set('ParentName', 'MealFineBase')
            with self.assertRaises(AssertionError):
                assert item.get('ParentName') == 'MealCookedIngredientless'
        for suffix in SPECIFIC:
            food = root.find('ThingDef[defName="AMJC_'+suffix+'"]')
            self.assertEqual(food.get('ParentName'), 'MealFineBase')
            self.assertEqual(food.findtext('ingestible/tasteThought'),
                             'AMJC_AteFlourFood')

    def test_mixing_disabled_must_be_rejected(self):
        node = deepcopy(ET.parse(ROOT/'Defs/RecipeDefs/Recipes_GrainsFood.xml')
                        .getroot().find('RecipeDef[defName="AMJC_CookMixedGrainMeal"]'))
        node.find('allowMixingIngredients').text = 'false'
        with self.assertRaises(AssertionError):
            require_recipe(node, GRAINS, 'MixedGrainMeal', 1, 240)

    def test_wrong_input_crosses_raw_and_flour_boundary(self):
        node = deepcopy(ET.parse(ROOT/'Defs/RecipeDefs/Recipes_GrainsFood.xml')
                        .getroot().find('RecipeDef[defName="AMJC_CookMixedFlourFood"]'))
        node.find('ingredients/li/filter/thingDefs/li').text = 'RawRice'
        with self.assertRaises(AssertionError):
            require_recipe(node, OWN_FLOURS | {'AMJC_WheatFlour'},
                           'MixedFlourFood', 1, 300)

    def test_bulk_nutrition_multiplication_is_rejected(self):
        node = deepcopy(ET.parse(ROOT/'Defs/RecipeDefs/Recipes_GrainsFood.xml')
                        .getroot().find('RecipeDef[defName="AMJC_CookMixedGrainPorridgeBulk"]'))
        node.find('ingredients/li/count').text = '0.5'
        with self.assertRaises(AssertionError):
            require_recipe(node, GRAINS, 'MixedGrainPorridge', 4, 200)

    def test_entry_audit_bytes_and_coverage(self):
        dec = json.loads((ROOT/'Docs/Research/GrainsMixedMealsPriorArt-20261013.json')
                         .read_text(encoding='utf-8'))
        self.assertEqual(dec['schema_version'], 1)
        self.assertEqual(dec['status'], 'ready')
        self.assertEqual(dec['owner'], 'sucRo-RimWorld/Ancient-Medieval-Japan-Grains')
        self.assertEqual(dec['unresolved'], [])
        self.assertEqual({x['ecosystem'] for x in dec['candidates']},
                         {'vanilla_mo','amj','ve','non_ve'})
        self.assertEqual(dec['decision']['kind'], 'independent')
        for evidence in dec['evidence']:
            file = ROOT/evidence['path']
            self.assertEqual(hashlib.sha256(file.read_bytes()).hexdigest(),
                             evidence['sha256'])

    def test_real_common_bill_scenarios_are_wired_in_all_profiles(self):
        # Static harness guard only: real in-game outcomes are a separate gate.
        body = (ROOT/'Tests/E2E/GrainsSimulationSteps.cs').read_text(
            encoding='utf-8')
        self.assertIn('[Then("Grains common food single and mixed Bills complete through real jobs")]', body)
        self.assertIn('await scope.CommonFoodBill(family, bulk, mix)', body)
        self.assertIn('foreach (string family in new[] { "MixedGrainMeal", "MixedGrainPorridge", "MixedFlourFood" })', body)
        self.assertIn('foreach (bool bulk in new[] { false, true })', body)
        self.assertIn('foreach (bool mix in new[] { false, true })', body)
        for code in ('recipe.allowMixingIngredients', 'recipe.MakeNewBill()',
                     'typeof(WorkGiver_DoBill)', 'giver.JobOnThing(worker, Campfire, true)',
                     'offered.def == JobDefOf.DoBill', 'await Complete(job',
                     'Count(materials[i]) == starting[i]', 'productBefore + output'):
            self.assertIn(code, body)
        self.assertIn('MO ? "DankPyon_Flour" : "AMJC_WheatFlour"', body)
        self.assertIn('bill.ingredientFilter.SetDisallowAll()', body)
        self.assertIn('bill.ingredientFilter.SetAllow(', body)
        self.assertIn('ThingMaker.MakeThing(', body)  # isolated inputs, not harvested evidence
        self.assertIn('created.Add(input)', body)
        self.assertIn('if (scope != null) await RuntimeThread.Run(scope.Dispose)', body)

        profile_file = (ROOT/'Scripts/GrainsTestProfiles.ps1').read_text(encoding='utf-8')
        self.assertIn('Grains $Name common food single and mixed Bills complete', profile_file)
        for profile in ('vanilla','vanilla-ccto','mo','mo-ccto'):
            feat = (ROOT/'Tests/E2E/Profiles'/('grains-'+profile+'.feature')).read_text(
                encoding='utf-8')
            self.assertEqual(feat.count('  Scenario:'), 10, profile)
            self.assertEqual(feat.count('@quickstart:AmjStageAQuickstart'), 5)
            self.assertIn('Scenario: Grains '+profile+' common food single and mixed Bills complete', feat)
            self.assertIn('Then Grains common food single and mixed Bills complete through real jobs', feat)
        self.assertIn('$spec.Scenarios.Count', (ROOT/'Scripts/Run-17Tests.ps1').read_text(
            encoding='utf-8'))


    def test_v1_native_gap_scenarios_are_wired(self):
        # Source assertion only; native Build and complete Pickle logs are independent.
        source = (ROOT/'Tests/E2E/GrainsSimulationSteps.cs').read_text(encoding='utf-8')
        profiles = (ROOT/'Scripts/GrainsTestProfiles.ps1').read_text(encoding='utf-8')
        cases = (
            ('V1 alternate processing benches complete',
             'Grains V1 alternate processing benches complete native Bills'),
            ('cooking Bills reject wrong ingredients',
             'Grains cooking Bills reject wrong ingredient families through native work givers'),
            ('food ingestion applies correct thoughts',
             'Grains common and specialty foods apply actual ingestion thoughts'))
        for name, step in cases:
            self.assertIn('Grains $Name '+name, profiles)
            self.assertIn('[Then("'+step+'")]', source)
            for profile in ('vanilla', 'vanilla-ccto', 'mo', 'mo-ccto'):
                feature = (ROOT/'Tests/E2E/Profiles'/('grains-'+profile+'.feature')).read_text(encoding='utf-8')
                self.assertIn('Scenario: Grains '+profile+' '+name, feature)
                self.assertIn('Then '+step, feature)
        for evidence in (
            'scope.Bill("AMJC_ThreshMilletBulk", scope.Stove',
            'scope.Bill("AMJC_HullMilletBulk", scope.Spot',
            'scope.Bill("AMJC_ThreshMilletBulk", scope.Spot',
            'scope.Bill("AMJC_HullMilletBulk", scope.Stove',
            'scope.Bill("AMJC_MillBuckwheat", scope.Stove',
            'stove.AllRecipes.Contains(', 'giver.JobOnThing(worker, Campfire, true)',
            'offered == null || offered.bill != bill',
            'JobMaker.MakeJob(JobDefOf.Ingest, item)', 'await Complete(job, () => item.Destroyed',
            'worker.needs.mood.thoughts.memories.Memories.Count('):
            self.assertIn(evidence, source)
        thought = ET.parse(ROOT/'Defs/ThoughtDefs/Thoughts_GrainsFood.xml').getroot()
        t = thought.find('ThoughtDef[defName="AMJC_AteFlourFood"]')
        self.assertEqual(float(t.findtext('durationDays')), 0.5)
        self.assertEqual(int(t.findtext('stackLimit')), 1)
        self.assertEqual(float(t.findtext('stages/li/baseMoodEffect')), 2.0)

    def test_generic_ja_and_english_functional_text_present(self):
        recipes = ET.parse(ROOT/'Defs/RecipeDefs/Recipes_GrainsFood.xml').getroot()
        foods = ET.parse(ROOT/'Defs/ThingDefs_Items/Items_GrainsFood.xml').getroot()
        localized_recipes = ET.parse(ROOT/'Languages/Japanese/DefInjected/'
                                    'RecipeDef/AMJC_GrainsChain.xml').getroot()
        localized_foods = ET.parse(ROOT/'Languages/Japanese/DefInjected/'
                                  'ThingDef/AMJC_GrainsChain.xml').getroot()
        for suffix in MENU:
            name = 'AMJC_'+suffix
            food = foods.find('ThingDef[defName="'+name+'"]')
            self.assertIsNotNone(food)
            for field in ('label','description'):
                self.assertTrue(food.findtext(field))
                self.assertTrue(localized_foods.findtext(name+'.'+field))
            for bulk in ('','Bulk'):
                recipe_name = 'AMJC_Cook'+suffix+bulk
                item = recipes.find('RecipeDef[defName="'+recipe_name+'"]')
                self.assertIsNotNone(item)
                for field in ('label','description','jobString'):
                    self.assertTrue(item.findtext(field))
                    self.assertTrue(localized_recipes.findtext(recipe_name+'.'+field))


if __name__ == '__main__':
    unittest.main()
