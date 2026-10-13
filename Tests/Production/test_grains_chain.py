"""Reject nutrition multiplication, bread-like storage and new research gates."""
import shutil
import tempfile
from pathlib import Path
from zipfile import ZipFile
import unittest
import xml.etree.ElementTree as ET
import sys
sys.path.insert(0, str(Path(__file__).resolve().parents[1]))
from amj_profile_xml import ROOT
from validate_grains_chain import validate, validate_mo_source

class ChainRegressionTests(unittest.TestCase):
    def setUp(self):
        self.temp = tempfile.TemporaryDirectory();self.addCleanup(self.temp.cleanup)
        self.root = Path(self.temp.name)
        for folder in ('Defs','BaseWithoutMO','Compatibility','LegacyStartingScenarios','Textures'):
            shutil.copytree(ROOT/folder,self.root/folder)
        shutil.copyfile(ROOT/'loadFolders.xml',self.root/'loadFolders.xml')

    def mutate(self,path,xpath,text=None,new_element=None):
        f=self.root/path;xml=ET.parse(f);n=xml.find(xpath);assert n is not None
        if new_element is not None:n.append(ET.fromstring(new_element))
        else:n.text=text
        xml.write(f)
        with self.assertRaises(AssertionError):validate(self.root)

    def test_current_chain(self):validate(self.root)
    def test_flour_uses_dedicated_art(self):
        self.mutate('Defs/ThingDefs_Items/Items_GrainsFlour.xml',
                    'ThingDef[defName="AMJC_BuckwheatFlour"]/graphicData/texPath',
                    'Things/Item/Resource/AMJC_Millet/Millet')

    def test_fallback_wheat_flour_uses_dedicated_art(self):
        self.mutate('BaseWithoutMO/Defs/Items_Flour.xml',
                    'ThingDef/graphicData/texPath',
                    'Things/Item/Resource/AMJC_Millet/Millet')

    def test_flour_requires_a_stack_texture(self):
        path = self.root/'Textures/Things/Item/Resource/AMJC_Millet/MilletFlour/MilletFlour_a.png'
        path.unlink()
        with self.assertRaisesRegex(AssertionError, 'missing _a'):
            validate(self.root)

    def test_mo_flour_retexture_must_reuse_wheat_art(self):
        self.mutate('Compatibility/MedievalOverhaul/Patches/MedievalOverhaul_FlourGraphics.xml',
                    'Operation/value/texPath',
                    'Things/Item/Resource/AMJC_Millet/MilletFlour')

    def test_flour_does_not_multiply_nutrition(self):
        self.mutate('BaseWithoutMO/Defs/Items_Flour.xml','ThingDef/statBases/Nutrition','0.1')
    def test_barley_flour_rejects_wrong_input(self):
        self.mutate('Defs/RecipeDefs/Recipes_GrainsMilling.xml',
                    'RecipeDef[defName="AMJC_MillBarley"]/ingredients/li/filter/thingDefs/li',
                    'AMJC_BarleyInHull')

    def test_rice_flour_must_use_existing_vanilla_rice(self):
        self.mutate('Defs/RecipeDefs/Recipes_GrainsMilling.xml',
                    'RecipeDef[defName="AMJC_MillRice"]/ingredients/li/filter/thingDefs/li',
                    'AMJC_RiceInHull')

    def test_rice_flour_must_not_increase_output(self):
        self.mutate('Defs/RecipeDefs/Recipes_GrainsMilling.xml',
                    'RecipeDef[defName="AMJC_MillRice"]/products/AMJC_RiceFlour', '20')

    def test_mo_barley_mill_must_use_mo_millstone(self):
        self.mutate('Compatibility/MedievalOverhaul/Patches/MedievalOverhaul_GrainsFlour.xml',
                    'Operation[5]/value/recipeUsers/li', 'AMJC_ManualMillstone')

    def test_milling_does_not_create_straw(self):
        self.mutate('Defs/RecipeDefs/Recipes_GrainsMilling.xml','RecipeDef/products',new_element='<AMJC_Millet>1</AMJC_Millet>')
    def test_meal_must_replace_inherited_comp_list(self):
        target = self.root/'Defs/ThingDefs_Items/Items_GrainsFood.xml'
        xml = ET.parse(target)
        del xml.find('ThingDef/comps').attrib['Inherit']
        xml.write(target)
        with self.assertRaises(AssertionError): validate(self.root)

    def test_meal_must_not_reference_unavailable_vanilla_texture(self):
        self.mutate('Defs/ThingDefs_Items/Items_GrainsFood.xml',
                    'ThingDef/graphicData/texPath', 'Things/Item/Meal/Simple')

    def test_meal_must_use_packaged_stack_graphics(self):
        self.mutate('Defs/ThingDefs_Items/Items_GrainsFood.xml',
                    'ThingDef/graphicData/graphicClass', 'Graphic_Single')

    def test_meal_must_have_a_texture(self):
        path = self.root/'Textures/Things/Item/Resource/AMJC_Buckwheat/Buckwheat/Buckwheat_a.png'
        path.unlink()
        with self.assertRaisesRegex(AssertionError, 'missing _a'):
            validate(self.root)

    def test_meal_can_use_only_a_texture(self):
        for tex in ('Things/Item/Resource/AMJC_Millet/Millet',
                    'Things/Item/Resource/AMJC_Buckwheat/Buckwheat'):
            folder = self.root/'Textures'/tex
            for suffix in 'bc':
                (folder/(Path(tex).name + '_' + suffix + '.png')).unlink(missing_ok=True)
        validate(self.root)

    def test_meal_rejects_a_c_without_b(self):
        tex = 'Things/Item/Resource/AMJC_Buckwheat/Buckwheat'
        folder = self.root/'Textures'/tex
        (folder/'Buckwheat_b.png').unlink(missing_ok=True)
        (folder/'Buckwheat_c.png').write_bytes((folder/'Buckwheat_a.png').read_bytes())
        with self.assertRaisesRegex(AssertionError, 'noncontiguous'):
            validate(self.root)

    def test_mood_memory_must_have_stage_description(self):
        self.mutate('Defs/ThoughtDefs/Thoughts_GrainsFood.xml',
                    'ThoughtDef/stages/li/description', '')

    def test_powder_food_not_bread_storage(self):
        self.mutate('Defs/ThingDefs_Items/Items_GrainsFood.xml',
                    'ThingDef/comps/li[@Class="CompProperties_Rottable"]/daysToRotStart','8')
    def test_cooking_remains_research_free(self):
        self.mutate('Defs/RecipeDefs/Recipes_GrainsFood.xml','RecipeDef',new_element='<researchPrerequisite>Cooking</researchPrerequisite>')
    def test_mo_food_uses_mo_flour(self):
        self.mutate('Compatibility/MedievalOverhaul/Patches/MedievalOverhaul_GrainsFlour.xml','Operation[3]/value/li','AMJC_WheatFlour')
    def test_processing_workgiver_must_reach_its_benches(self):
        self.mutate('Defs/WorkGiverDefs/WorkGivers_Grains.xml','WorkGiverDef/fixedBillGiverDefs/li','CraftingSpot')
    def test_fallback_mill_requires_bill_worker(self):
        self.mutate('BaseWithoutMO/Defs/WorkGivers_Milling.xml','WorkGiverDef/giverClass','WorkGiver_PlantsCut')

    def test_mo_hay_removal_is_not_lost(self):
        path = self.root/'Compatibility/MedievalOverhaul/Patches/MedievalOverhaul_StageA_Wheat.xml'
        doc = ET.parse(path)
        target = '/Defs/RecipeDef[defName="DankPyon_CraftFlourBulk"]/products/Hay'
        nodes = [op.find('xpath') for op in doc.getroot().findall('Operation')
                 if op.get('Class') == 'PatchOperationRemove' and op.findtext('xpath') == target]
        self.assertEqual(len(nodes), 1)
        nodes[0].text = target.replace('/products/Hay', '/products/Flour')
        doc.write(path)
        with self.assertRaises(AssertionError):
            validate(self.root)

    def test_mo_16_archive_provider_contract(self):
        # Synthetic compressed MO provider: run without the proprietary game.
        things = """<Defs>
          <ThingDef><defName>DankPyon_Flour</defName><statBases><Nutrition>0.05</Nutrition></statBases></ThingDef>
          <ThingDef><defName>DankPyon_Millstone</defName><researchPrerequisites><li>DankPyon_BasicAgriculture</li></researchPrerequisites></ThingDef>
          <ThingDef><defName>DankPyon_Plant_Wheat</defName><plant><growDays>12</growDays><harvestYield>28</harvestYield><harvestedThingDef>DankPyon_RawWheat</harvestedThingDef></plant></ThingDef>
          <ThingDef><defName>DankPyon_RawWheat</defName></ThingDef>
        </Defs>"""
        recipes = """<Defs>
          <RecipeDef><defName>DankPyon_CraftFlour_Manual</defName><workAmount>300</workAmount><recipeUsers><li>CraftingSpot</li></recipeUsers><ingredients><li><count>1</count></li></ingredients><products><DankPyon_Flour>1</DankPyon_Flour><Hay>1</Hay></products></RecipeDef>
          <RecipeDef><defName>DankPyon_CraftFlour</defName><workAmount>100</workAmount><recipeUsers><li>DankPyon_Millstone</li></recipeUsers><ingredients><li><count>1</count></li></ingredients><products><DankPyon_Flour>1</DankPyon_Flour><Hay>1</Hay></products></RecipeDef>
          <RecipeDef><defName>DankPyon_CraftFlourBulk</defName><workAmount>800</workAmount><recipeUsers><li>DankPyon_Millstone</li></recipeUsers><ingredients><li><count>10</count></li></ingredients><products><DankPyon_Flour>10</DankPyon_Flour><Hay>10</Hay></products></RecipeDef>
        </Defs>"""
        worker = """<Defs><WorkGiverDef><defName>DankPyon_DoBillsMillstone</defName>
          <giverClass>WorkGiver_DoBill</giverClass><workType>Cooking</workType>
          <fixedBillGiverDefs><li>DankPyon_Millstone</li></fixedBillGiverDefs>
        </WorkGiverDef></Defs>"""
        archive = self.root/'3219596926.zip'
        def package(work):
            with ZipFile(archive,'w') as z:
                z.writestr('3219596926/1.6/Defs/Things.xml',things)
                z.writestr('3219596926/1.6/Defs/Recipes.xml',recipes)
                z.writestr('3219596926/1.6/Defs/WorkGivers.xml',work)
                z.writestr('3219596926/1.4/Defs/Things.xml',things)
        package(worker)
        validate_mo_source(archive)
        package(worker.replace('WorkGiver_DoBill','WorkGiver_PlantsCut'))
        with self.assertRaises(AssertionError):validate_mo_source(archive)
        # Work values are balance data: stale MO provider work must not pass.
        with ZipFile(archive,'w') as z:
            z.writestr('3219596926/1.6/Defs/Things.xml',things)
            z.writestr('3219596926/1.6/Defs/Recipes.xml',
                       recipes.replace('<workAmount>800</workAmount>', '<workAmount>30</workAmount>'))
            z.writestr('3219596926/1.6/Defs/WorkGivers.xml',worker)
        with self.assertRaises(AssertionError):validate_mo_source(archive)
        package(worker.replace('DankPyon_Millstone</li>','OtherMillstone</li>'))
        with self.assertRaises(AssertionError):validate_mo_source(archive)

    def test_no_duplicate_fallback_in_mo(self):
        self.mutate('Defs/ThingDefs_Items/Items_GrainsFlour.xml','.',new_element='<ThingDef><defName>AMJC_WheatFlour</defName></ThingDef>')

if __name__ == '__main__':unittest.main()
