"""Static seven-crop harvest-to-food balance audit.

Uses only committed Grains XML and the historical six-crop finite-season
fixture; no RimWorld inheritance, live Bill execution, CCTO survival or
Medieval Overhaul binary execution is simulated here.
"""
import unittest
import xml.etree.ElementTree as ET
from collections import Counter
from pathlib import Path

import sys
sys.path.insert(0, str(Path(__file__).resolve().parents[1]))
from amj_profile_xml import ROOT
from validate_grains_environment import output

RICE = (5, 11, 0.7, 0.8, 10, 18, 32, 42)
PROCESSED = {
    "Millet": ("AMJC_RawMillet", "AMJC_MilletInHull", "AMJC_Millet"),
    "Buckwheat": ("AMJC_RawBuckwheat", "AMJC_BuckwheatInHull", "AMJC_Buckwheat"),
    "Barley": ("AMJC_RawBarley", "AMJC_BarleyInHull", "AMJC_Barley"),
    "Rice": ("AMJC_RiceSheaf", "AMJC_RiceInHull", "RawRice"),
}
FLOUR = {
    "Buckwheat": ("AMJC_Buckwheat", "AMJC_BuckwheatFlour", "AMJC_MillBuckwheat",
                  "AMJC_CookSobagaki", "AMJC_Sobagaki"),
    "Millet": ("AMJC_Millet", "AMJC_MilletFlour", "AMJC_MillMillet",
               "AMJC_CookMilletDumplings", "AMJC_MilletDumplings"),
}
BATCH_INPUT = 10


def defined(path, typ):
    doc = ET.parse(ROOT / path).getroot()
    return {node.findtext("defName"): node for node in doc.findall(typ)
            if node.findtext("defName")}


def require_recipe(recipes, name, grain, quantity, item, produced, work):
    recipe = recipes[name]
    inputs = recipe.findall("ingredients/li")
    assert len(inputs) == 1, name + " must have one input"
    assert inputs[0].findtext("filter/thingDefs/li") == grain, name
    assert float(inputs[0].findtext("count")) == quantity, name
    outputs = [(child.tag, float(child.text)) for child in recipe.findall("products/*")]
    assert outputs == [(item, float(produced))], name + " changes yield or byproducts"
    assert float(recipe.findtext("workAmount")) == work, name
    return recipe


class SevenGrainChainBalance(unittest.TestCase):
    def test_threshing_improves_storage_life(self):
        shared = defined("Defs/ThingDefs_Items/Items_StageA_Grains.xml", "ThingDef")
        for crop, (sheaf, hull, _) in PROCESSED.items():
            with self.subTest(crop=crop):
                self.assertEqual(float(shared[sheaf].findtext("comps/li/daysToRotStart")), 30)
                self.assertEqual(float(shared[hull].findtext("comps/li/daysToRotStart")), 120)
        base_wheat = defined("BaseWithoutMO/Defs/Items_Wheat.xml", "ThingDef")
        self.assertEqual(float(base_wheat["AMJC_RawWheat"].findtext(
            "comps/li/daysToRotStart")), 30)
        mo_patch = ET.parse(ROOT / "Compatibility/MedievalOverhaul/Patches/"
                            "MedievalOverhaul_StageA_Wheat.xml")
        xpath = ('/Defs/ThingDef[defName="DankPyon_RawWheat"]'
                 '/comps/li[@Class="CompProperties_Rottable"]/daysToRotStart')
        ops = [op for op in mo_patch.findall("Operation") if op.findtext("xpath") == xpath]
        self.assertEqual(len(ops), 1)
        self.assertEqual(ops[0].get("Class"), "PatchOperationReplace")
        self.assertEqual(ops[0].findtext("value/daysToRotStart"), "30")


    def test_two_stage_processing_and_bulk_labor(self):
        recipes = defined("Defs/RecipeDefs/Recipes_GrainProcessing.xml", "RecipeDef")
        for crop, (raw, hull, edible) in PROCESSED.items():
            with self.subTest(crop=crop):
                require_recipe(recipes, "AMJC_Thresh" + crop, raw, 1, hull, 1, 15)
                require_recipe(recipes, "AMJC_Thresh" + crop + "Bulk",
                               raw, 10, hull, 10, 120)
                require_recipe(recipes, "AMJC_Hull" + crop, hull, 1, edible, 1, 10)
                require_recipe(recipes, "AMJC_Hull" + crop + "Bulk",
                               hull, 10, edible, 10, 80)

    def test_adaptive_primitive_storage_granary_and_mo_straw_patch(self):
        import xml.etree.ElementTree as ET

        granary = ET.parse(ROOT / "Patches/ZZ_AdaptivePrimitiveStorage_Granary.xml").getroot()
        self.assertEqual(len(granary.findall("Operation")), 1)
        root = granary.find("Operation")
        self.assertEqual(root.get("Class"), "PatchOperationFindMod")
        self.assertEqual(root.findtext("mods/li"), "Adaptive Primitive Storage")
        inserts = root.findall("match/operations/li")
        self.assertEqual(len(inserts), 2)
        for target, node in zip(("fixedStorageSettings", "defaultStorageSettings"), inserts):
            self.assertEqual(node.get("Class"), "PatchOperationConditional")
            self.assertEqual(node.findtext("match/value/li"), "AMJC_Grains")
            self.assertIn("/" + target + "/filter/categories", node.findtext("match/xpath"))

        straw = ET.parse(ROOT / "Compatibility/MedievalOverhaul/Patches/"
                         "ZZ_AdaptivePrimitiveStorage_Straw.xml").getroot()
        op = straw.find("Operation")
        self.assertEqual(op.get("Class"), "PatchOperationFindMod")
        self.assertEqual(op.findtext("mods/li"), "Adaptive Primitive Storage")
        expected = {
            "ASF_Granary": "50",
            "ASF_WovenBasket": "75",
            "ASF_WovenBasketDouble": "150",
            "ASF_WovenBasketTriple": "225",
        }
        applied = {}
        for step in op.findall("match/operations/li"):
            self.assertEqual(step.get("Class"), "PatchOperationConditional")
            path = step.findtext("xpath")
            self.assertEqual(step.findtext("match/xpath"), path)
            self.assertEqual(step.find("match").get("Class"), "PatchOperationReplace")
            found = next((name for name in expected if f'defName="{name}"' in path), None)
            self.assertIsNotNone(found)
            self.assertTrue(path.endswith("/costList/Hay"))
            applied[found] = step.findtext("match/value/DankPyon_Straw")
        self.assertEqual(applied, expected)
        meta = ET.parse(ROOT / "About/About.xml").getroot()
        self.assertIn("Adaptive.PrimitiveStorage",
                      [node.text for node in meta.findall("loadAfter/li")])

    def test_wheat_processing_profiles_keep_output_and_straw_separate(self):
        for mode, path, raw in (
            ("base", "BaseWithoutMO/Defs/Recipes_Wheat.xml", "AMJC_RawWheat"),
            ("mo", "Compatibility/MedievalOverhaul/Defs/Recipes_MOWheat.xml",
             "DankPyon_RawWheat"),
        ):
            recipes = defined(path, "RecipeDef")
            for suffix, units, work in (("", 1, 15), ("Bulk", 10, 120)):
                name = "AMJC_ThreshWheat" + suffix
                recipe = recipes[name]
                self.assertEqual(recipe.findtext("ingredients/li/filter/thingDefs/li"), raw)
                self.assertEqual(float(recipe.findtext("ingredients/li/count")), units)
                self.assertEqual(float(recipe.findtext("workAmount")), work)
                products = {child.tag: float(child.text) for child in recipe.findall("products/*")}
                expected = {"AMJC_Wheat": float(units)}
                if mode == "mo":
                    expected["DankPyon_Straw"] = float(units)
                self.assertEqual(products, expected)
        patch = ET.parse(ROOT / "Compatibility/MedievalOverhaul/Patches/"
                        "MedievalOverhaul_StageA_Base.xml").getroot()
        for crop in PROCESSED:
            for suffix, units in (("", "1"), ("Bulk", "10")):
                selector = ('/Defs/RecipeDef[defName="AMJC_Thresh' +
                            crop + suffix + '"]/products')
                matches = [op.findtext("value/DankPyon_Straw") for op in patch.findall("Operation")
                           if op.findtext("xpath") == selector]
                self.assertEqual(matches, [units], selector)

    def test_all_own_flour_conserves_grain_nutrition_but_costs_work(self):
        milled = defined("Defs/RecipeDefs/Recipes_GrainsMilling.xml", "RecipeDef")
        grain_defs = defined("Defs/ThingDefs_Items/Items_StageA_Grains.xml", "ThingDef")
        flours = defined("Defs/ThingDefs_Items/Items_GrainsFlour.xml", "ThingDef")
        for crop, (grain, flour, recipe, _, _) in FLOUR.items():
            with self.subTest(crop=crop):
                require_recipe(milled, recipe, grain, 10, flour, 10, 300)
                self.assertAlmostEqual(float(grain_defs[grain].findtext("statBases/Nutrition")),
                                       0.05)
                self.assertAlmostEqual(float(flours[flour].findtext("statBases/Nutrition")),
                                       0.05)
                self.assertEqual(float(flours[flour].findtext(
                    "comps/li/daysToRotStart")), 60)
        wheat_mill = defined("BaseWithoutMO/Defs/Recipes_Milling.xml", "RecipeDef")
        require_recipe(wheat_mill, "AMJC_MillWheat", "AMJC_Wheat", 10,
                       "AMJC_WheatFlour", 10, 300)
        wheat_flour = defined("BaseWithoutMO/Defs/Items_Flour.xml", "ThingDef")
        self.assertAlmostEqual(float(wheat_flour["AMJC_WheatFlour"].findtext(
            "statBases/Nutrition")), 0.05)
        self.assertEqual(float(wheat_flour["AMJC_WheatFlour"].findtext(
            "comps/li/daysToRotStart")), 60)

    def test_flour_food_reward_and_short_shelf_life(self):
        meals = defined("Defs/RecipeDefs/Recipes_GrainsFood.xml", "RecipeDef")
        foods = defined("Defs/ThingDefs_Items/Items_GrainsFood.xml", "ThingDef")
        for crop, (_, flour, _, recipe, meal) in FLOUR.items():
            with self.subTest(crop=crop):
                item = meals[recipe]
                self.assertEqual(item.findtext("ingredientValueGetterClass"),
                                 "IngredientValueGetter_Nutrition")
                self.assertEqual(item.findtext("ingredients/li/filter/thingDefs/li"), flour)
                self.assertAlmostEqual(float(item.findtext("ingredients/li/count")), 0.5)
                self.assertEqual(item.findtext("products/" + meal), "1")
                self.assertEqual(float(item.findtext("workAmount")), 300)
                self.assertEqual(item.findtext("workSpeedStat"), "CookSpeed")
                self.assertAlmostEqual(float(foods[meal].findtext("statBases/Nutrition")), 0.9)
                self.assertEqual(float(foods[meal].findtext(
                    "comps/li[@Class='CompProperties_Rottable']/daysToRotStart")), 2.5)
                self.assertEqual(foods[meal].findtext("ingestible/tasteThought"),
                                 "AMJC_AteFlourFood")
        wheat_meal = meals["AMJC_CookHoutou"]
        self.assertEqual(wheat_meal.findtext("ingredients/li/filter/thingDefs/li"),
                         "AMJC_WheatFlour")
        self.assertEqual(float(wheat_meal.findtext("ingredients/li/count")), 0.5)
        self.assertEqual(wheat_meal.findtext("products/AMJC_Houtou"), "1")
        self.assertEqual(float(wheat_meal.findtext("workAmount")), 300)
        self.assertEqual(float(foods["AMJC_Houtou"].findtext("statBases/Nutrition")), 0.9)
        self.assertEqual(float(foods["AMJC_Houtou"].findtext(
            "comps/li[@Class='CompProperties_Rottable']/daysToRotStart")), 2.5)
        mood = defined("Defs/ThoughtDefs/Thoughts_GrainsFood.xml", "ThoughtDef")[
            "AMJC_AteFlourFood"]
        self.assertEqual(float(mood.findtext("stages/li/baseMoodEffect")), 2)
        self.assertEqual(float(mood.findtext("durationDays")), 0.5)
        self.assertEqual(float(mood.findtext("stackLimit")), 1)

    def test_seven_crop_niches_and_rice_ties_are_explicit(self):
        import json
        fixture = json.loads((ROOT / "Tests/Fixtures/Grains_Environment.json").read_text(
            encoding="utf-8"))
        for profile in ("vanilla", "mo"):
            with self.subTest(profile=profile):
                counts = Counter()
                viable, rice_ties, rice_unique = 0, 0, 0
                for cell in fixture[profile]:
                    grain = cell["yields"]
                    self.assertEqual(len(grain), 6)
                    rice = output(RICE, cell["fertility"], cell["temperature"], cell["season"])
                    produced = grain + [rice]
                    best = max(produced)
                    if best == 0:
                        continue
                    viable += 1
                    for i, value in enumerate(produced):
                        if value == best:
                            counts[i] += 1
                    if rice == best:
                        rice_ties += best in grain
                        rice_unique += best not in grain
                self.assertEqual(viable, 23)
                self.assertEqual([counts[i] for i in range(7)],
                                 [1, 2, 8, 4, 4, 6, 6])
                self.assertEqual((rice_ties, rice_unique), (6, 0),
                                 "Rice currently has shared harvest maxima, not a solo one")


if __name__ == "__main__":
    unittest.main()
