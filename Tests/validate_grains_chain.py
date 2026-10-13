"""Static Base/MO flour-chain contracts; no XML inheritance/game execution claim."""
import xml.etree.ElementTree as ET
from pathlib import Path
from zipfile import ZipFile
from amj_profile_xml import ROOT, profile_xml
from stack_graphics import stack_textures

SHARED_NAMES = {
    'AMJC_BuckwheatFlour','AMJC_MilletFlour','AMJC_Houtou','AMJC_Sobagaki',
    'AMJC_MilletDumplings','AMJC_AteFlourFood','AMJC_MillBuckwheat','AMJC_MillMillet',
    'AMJC_CookHoutou','AMJC_CookSobagaki','AMJC_CookMilletDumplings','AMJC_DoGrainProcessing',
    'AMJC_BarleyFlour','AMJC_RiceFlour','AMJC_BarleyFlourGruel','AMJC_RiceFlourDumplings',
    'AMJC_MillBarley','AMJC_MillRice','AMJC_CookBarleyFlourGruel','AMJC_CookRiceFlourDumplings',
    'AMJC_CookHoutouBulk','AMJC_CookSobagakiBulk','AMJC_CookMilletDumplingsBulk',
    'AMJC_CookBarleyFlourGruelBulk','AMJC_CookRiceFlourDumplingsBulk',
}
FALLBACK_NAMES = {'AMJC_Plant_Wheat','AMJC_RawWheat','AMJC_WheatFlour','AMJC_ManualMillstone','AMJC_MillWheat','AMJC_DoGrainsMilling'}


def validate(root=ROOT):
    for profile in ('vanilla','mo'):
        doc = profile_xml(profile, root)
        defs = {n.findtext('defName'): n for n in doc if n.findtext('defName')}
        assert len(defs) == len([n for n in doc if n.findtext('defName')]), 'Duplicate DefName'
        assert SHARED_NAMES <= defs.keys()
        if profile == 'mo':
            assert FALLBACK_NAMES.isdisjoint(defs), 'Fallback wheat/flour/mill leaked into MO'
        else:
            assert FALLBACK_NAMES <= defs.keys()
            p = defs['AMJC_Plant_Wheat'].find('plant')
            for key, value in {'growDays':12,'harvestYield':28,'fertilityMin':0.7,'fertilitySensitivity':0.9,
                               'minGrowthTemperature':0,'sowMinSkill':0}.items():
                assert float(p.findtext(key)) == value, key
            assert p.findtext('harvestedThingDef') == 'AMJC_RawWheat'
            assert p.find('sowResearchPrerequisites') is None
            assert float(defs['AMJC_RawWheat'].findtext('comps/li/daysToRotStart')) == 120
            assert defs['AMJC_RawWheat'].findtext('ingestible/preferability') == 'NeverForNutrition'
            stone = defs['AMJC_ManualMillstone']
            assert stone.find('researchPrerequisites') is None
            assert stone.findtext('costList/BlocksGranite') == '30' and stone.findtext('costList/WoodLog') == '20'
        work = defs['AMJC_DoGrainProcessing']
        assert work.tag == 'WorkGiverDef' and work.findtext('giverClass') == 'WorkGiver_DoBill'
        assert work.findtext('workType') == 'Crafting'
        assert [n.text for n in work.findall('fixedBillGiverDefs/li')] == ['AMJC_GrainProcessingSpot','AMJC_GrainProcessingTable']
        if profile == 'vanilla':
            work = defs['AMJC_DoGrainsMilling']
            assert work.tag == 'WorkGiverDef' and work.findtext('giverClass') == 'WorkGiver_DoBill'
            assert work.findtext('workType') == 'Crafting'
            assert [n.text for n in work.findall('fixedBillGiverDefs/li')] == ['AMJC_ManualMillstone']
        # All single/bulk grain-processing Bills inherit this Base-and-MO stove fallback.
        processing_users = ['AMJC_GrainProcessingSpot', 'AMJC_GrainProcessingTable',
                            'FueledStove', 'ElectricStove']
        for crop in ('Millet', 'Buckwheat', 'Barley', 'Rice', 'Wheat'):
            parent = 'AMJC_' + crop + 'ProcessingBase'
            matches = [n for n in doc.findall('RecipeDef')
                       if n.get('Name') == parent and n.get('Abstract') == 'True']
            assert len(matches) == 1, ('Processing parent missing', profile, parent)
            assert [n.text for n in matches[0].findall('recipeUsers/li')] == processing_users, (
                'Processing stove providers', profile, parent)
            operations = ('Thresh',) if crop == 'Wheat' else ('Thresh', 'Hull')
            for operation in operations:
                for bulk in ('', 'Bulk'):
                    name = 'AMJC_' + operation + crop + bulk
                    rec = defs[name]
                    assert rec.get('ParentName') == parent and rec.find('recipeUsers') is None, (
                        'Processing recipe must inherit shared bench users', name)
        for name, count in [('AMJC_ThreshWheat',1), ('AMJC_ThreshWheatBulk',10)]:
            rec = defs[name]
            expected = 'DankPyon_RawWheat' if profile == 'mo' else 'AMJC_RawWheat'
            assert rec.findtext('ingredients/li/filter/thingDefs/li') == expected
            assert float(rec.findtext('ingredients/li/count')) == count
            assert int(rec.findtext('products/AMJC_Wheat')) == count
            assert [n.tag for n in rec.findall('products/*')] == (['AMJC_Wheat','DankPyon_Straw'] if profile == 'mo' else ['AMJC_Wheat'])
        chains = [('AMJC_MillBuckwheat','AMJC_Buckwheat','AMJC_BuckwheatFlour'),
                  ('AMJC_MillMillet','AMJC_Millet','AMJC_MilletFlour'),
                  ('AMJC_MillBarley','AMJC_Barley','AMJC_BarleyFlour'),
                  ('AMJC_MillRice','RawRice','AMJC_RiceFlour')]
        if profile == 'vanilla':chains += [('AMJC_MillWheat','AMJC_Wheat','AMJC_WheatFlour')]
        for recipe, grain, flour in chains:
            rec = defs[recipe]
            assert rec.findtext('ingredients/li/filter/thingDefs/li') == grain
            amount = float(rec.findtext('ingredients/li/count'))
            output = float(rec.findtext('products/'+flour))
            assert len(rec.findall('products/*')) == 1
            # A balanced ratio alone allows an unintended 1:1, 5:5 or 20:20
            # batch, so preserve the authored 10-unit recipe and job costs.
            assert amount == 10 and output == 10, ('Milling batch size', recipe)
            assert rec.findtext('targetCountAdjustment') == '10', ('Bill count step', recipe)
            assert float(rec.findtext('workAmount')) == 300, ('Milling labor', recipe)
            assert rec.findtext('workSpeedStat') == 'GeneralLaborSpeed', ('Milling work stat', recipe)
            assert rec.findtext('workSkill') == 'Crafting', ('Milling skill', recipe)
            assert rec.findtext('allowMixingIngredients') == 'false', ('Milling mixed inputs', recipe)
            assert rec.findtext('fixedIngredientFilter/thingDefs/li') == grain, ('Fixed milling ingredient', recipe)
            assert rec.findtext('fixedIngredientFilter/specialFiltersToDisallow/li') == 'AllowRotten', ('Rotten grain filter', recipe)
            grain_nutrition = 0.05 if grain == 'RawRice' else float(defs[grain].findtext('statBases/Nutrition'))
            # RawRice belongs to Vanilla: loaded-game assertions check its actual stat.
            assert amount * grain_nutrition == output * float(defs[flour].findtext('statBases/Nutrition'))
            assert rec.findtext('recipeUsers/li') == ('DankPyon_Millstone' if profile == 'mo' else 'AMJC_ManualMillstone')
            assert rec.find('researchPrerequisite') is None and rec.find('researchPrerequisites') is None
            assert float(defs[flour].findtext('comps/li/daysToRotStart')) == 60
            assert defs[flour].findtext('ingestible/preferability') == 'NeverForNutrition'
        flour_graphics = {
            'AMJC_BuckwheatFlour': 'Things/Item/Resource/AMJC_Buckwheat/BuckwheatFlour',
            'AMJC_MilletFlour': 'Things/Item/Resource/AMJC_Millet/MilletFlour',
            'AMJC_BarleyFlour': 'Things/Item/Resource/AMJC_Barley/BarleyFlour',
            'AMJC_RiceFlour': 'Things/Item/Resource/AMJC_Rice/RiceFlour',
        }
        if profile == 'vanilla':
            flour_graphics['AMJC_WheatFlour'] = 'Things/Item/Resource/AMJC_Wheat/WheatFlour'
        for name, tex in flour_graphics.items():
            graphic = defs[name].find('graphicData')
            assert graphic is not None and graphic.findtext('texPath') == tex, 'Flour graphic path: ' + name
            assert graphic.findtext('graphicClass') == 'Graphic_StackCount', 'Flour graphic class: ' + name
            stack_textures(root, tex)
        # These temporary meal sprites are shipped by Grains. Never regress
        # to a guessed Vanilla texPath: those failed real-game Texture2D loads
        # despite matching older public Vanilla XML examples.
        meal_graphics = {
            'Houtou': 'Things/Item/Resource/AMJC_Millet/Millet',
            'Sobagaki': 'Things/Item/Resource/AMJC_Buckwheat/Buckwheat',
            'MilletDumplings': 'Things/Item/Resource/AMJC_Millet/Millet',
            'BarleyFlourGruel': 'Things/Item/Resource/AMJC_Barley/Barley',
            'RiceFlourDumplings': 'Things/Item/Resource/AMJC_Rice/Rice',
        }
        for suffix, flour in [('Houtou','DankPyon_Flour' if profile == 'mo' else 'AMJC_WheatFlour'),
                              ('Sobagaki','AMJC_BuckwheatFlour'),('MilletDumplings','AMJC_MilletFlour'),
                              ('BarleyFlourGruel','AMJC_BarleyFlour'),('RiceFlourDumplings','AMJC_RiceFlour')]:
            rec = defs['AMJC_Cook'+suffix];food=defs['AMJC_'+suffix]
            assert rec.findtext('ingredientValueGetterClass') == 'IngredientValueGetter_Nutrition'
            assert rec.findtext('ingredients/li/filter/thingDefs/li') == flour
            assert rec.findtext('fixedIngredientFilter/thingDefs/li') == flour
            assert float(rec.findtext('ingredients/li/count')) == 0.5
            assert rec.findtext('products/AMJC_'+suffix) == '1'
            assert len(rec.findall('products/*')) == 1
            assert float(food.findtext('statBases/Nutrition')) == 0.9
            # Inherited MealFineBase already provides a Rottable comp.
            # Grains must replace, not append to, the complete base meal list.
            assert food.find('comps').get('Inherit') == 'False'
            assert [node.get('Class') for node in food.findall('comps/li')] == [
                'CompProperties_Forbiddable',
                'CompProperties_Ingredients',
                'CompProperties_FoodPoisonable',
                'CompProperties_Rottable',
            ]
            assert float(food.findtext("comps/li[@Class='CompProperties_Rottable']/daysToRotStart")) == 2.5
            tex = food.findtext('graphicData/texPath')
            assert tex == meal_graphics[suffix], 'Unexpected temporary meal graphic: ' + suffix
            assert food.findtext('graphicData/graphicClass') == 'Graphic_StackCount'
            # One _a is sufficient; any optional _b/_c must be contiguous,
            # packaged, and have valid PNG dimensions.
            stack_textures(root, tex)
            assert food.findtext('ingestible/tasteThought') == 'AMJC_AteFlourFood'
            assert food.findtext('ingestible/preferability') == 'MealSimple'
            assert rec.find('researchPrerequisite') is None and rec.find('researchPrerequisites') is None
            assert [n.text for n in rec.findall('recipeUsers/li')] == ['Campfire','ElectricStove','FueledStove']
            # Vanilla 4-meal behavior: one Bill yields four meals, with four
            # times the inputs AND four times the cooking work; savings are
            # fewer jobs/ingredient fetches, not free nutrition or work.
            bulk = defs['AMJC_Cook'+suffix+'Bulk']
            assert bulk.tag == 'RecipeDef', suffix
            assert bulk.findtext('ingredientValueGetterClass') == 'IngredientValueGetter_Nutrition'
            assert bulk.findtext('ingredients/li/filter/thingDefs/li') == flour
            assert bulk.findtext('fixedIngredientFilter/thingDefs/li') == flour
            assert bulk.findtext('fixedIngredientFilter/specialFiltersToDisallow/li') == 'AllowRotten'
            assert float(bulk.findtext('ingredients/li/count')) == 2.0, ('Bulk flour nutrition', suffix)
            assert [n.tag for n in bulk.findall('products/*')] == ['AMJC_'+suffix]
            assert bulk.findtext('products/AMJC_'+suffix) == '4', ('Bulk meal quantity', suffix)
            assert float(rec.findtext('workAmount')) == 300
            assert float(bulk.findtext('workAmount')) == 1200, ('Bulk cooking work', suffix)
            assert bulk.findtext('workSpeedStat') == rec.findtext('workSpeedStat') == 'CookSpeed'
            assert bulk.findtext('workSkill') == rec.findtext('workSkill') == 'Cooking'
            assert [n.text for n in bulk.findall('recipeUsers/li')] == ['Campfire','ElectricStove','FueledStove']
            assert bulk.find('researchPrerequisite') is None and bulk.find('researchPrerequisites') is None
            assert bulk.findtext('ingredientValueGetterClass') == rec.findtext('ingredientValueGetterClass')
            assert bulk.find('skillRequirements') is None
            assert bulk.find('targetCountAdjustment') is None
        assert float(defs['AMJC_AteFlourFood'].findtext('stages/li/baseMoodEffect')) == 2
    mood = ET.parse(root/'Defs/ThoughtDefs/Thoughts_GrainsFood.xml').getroot()
    stage = mood.find("ThoughtDef[defName='AMJC_AteFlourFood']/stages/li")
    assert stage is not None and (stage.findtext('description') or '').strip()
    assert stage.findtext('baseMoodEffect') == '2'
    patch = ET.parse(root/'BaseWithoutMO/Patches/CCTO_Wheat.xml')
    assert patch.findtext('.//coldDeathTemperature') == '-6'
    assert patch.findtext('.//xpath') == '/Defs/ThingDef[defName="AMJC_Plant_Wheat"]'
    mill_patch = ET.parse(root/'Compatibility/MedievalOverhaul/Patches/MedievalOverhaul_GrainsMillstone.xml')
    assert mill_patch.findtext('Operation/xpath') == '/Defs/ThingDef[defName="DankPyon_Millstone"]/researchPrerequisites'
    assert mill_patch.find('Operation/match').get('Class') == 'PatchOperationRemove'
    # Upstream MO grinding emits Hay, but AMJ removes it at all three
    # recipes: no redundant feed/bedding from the milling stage.
    wheat_patch = ET.parse(root/'Compatibility/MedievalOverhaul/Patches/MedievalOverhaul_StageA_Wheat.xml')
    expected_hay_removals = {
        '/Defs/RecipeDef[defName="' + name + '"]/products/Hay'
        for name in ('DankPyon_CraftFlour_Manual', 'DankPyon_CraftFlour',
                     'DankPyon_CraftFlourBulk')
    }
    hay_removals = {op.findtext('xpath') for op in wheat_patch.getroot().findall('Operation')
                    if op.get('Class') == 'PatchOperationRemove'
                    and (op.findtext('xpath') or '').endswith('/products/Hay')}
    assert hay_removals == expected_hay_removals, 'AMJ MO grinding must remove upstream Hay for all three recipes'
    flour_tex = 'Things/Item/Resource/AMJC_Wheat/WheatFlour'
    flour_patch = ET.parse(root/'Compatibility/MedievalOverhaul/Patches/MedievalOverhaul_FlourGraphics.xml')
    mo_flour_ops = flour_patch.getroot().findall('Operation')
    assert len(mo_flour_ops) == 1, 'MO flour graphics patch must only change one texture'
    assert mo_flour_ops[0].get('Class') == 'PatchOperationReplace', 'MO flour graphic override'
    assert mo_flour_ops[0].findtext('xpath') == '/Defs/ThingDef[defName="DankPyon_Flour"]/graphicData/texPath'
    assert mo_flour_ops[0].findtext('value/texPath') == flour_tex, 'MO flour texture path'
    stack_textures(root, flour_tex)
    print('Grains Base/MO wheat/flour/milling/minimum-food static contracts: PASS')


def validate_mo_source(mo_root):
    source = Path(mo_root)
    index = {}
    if source.is_file() and source.suffix.lower() == '.zip':
        with ZipFile(source) as archive:
            # Ignore older archived 1.4/1.5 defs: only 1.6 is loadable.
            entries = [name for name in archive.namelist()
                       if name.endswith('.xml') and '/1.6/Defs/' in ('/' + name)]
            assert entries, 'MO archive has no 1.6/Defs XML files'
            for entry in entries:
                for node in ET.fromstring(archive.read(entry)):
                    name = node.findtext('defName')
                    if name:index.setdefault((node.tag,name),[]).append(node)
    else:
        defs_root = source/'1.6/Defs' if (source/'1.6/Defs').is_dir() else source/'Defs'
        assert defs_root.is_dir(), 'MO 1.6 Defs directory is missing'
        for path in defs_root.rglob('*.xml'):
            for node in ET.parse(path).getroot():
                name = node.findtext('defName')
                if name:index.setdefault((node.tag,name),[]).append(node)
    def get(typ,name):
        matches=index.get((typ,name),[]);assert len(matches)==1, name
        return matches[0]
    flour=get('ThingDef','DankPyon_Flour')
    assert float(flour.findtext('statBases/Nutrition')) == 0.05
    stone=get('ThingDef','DankPyon_Millstone')
    assert stone.find('researchPrerequisites') is not None
    # These values are taken from the supplied MO 1.6 ZIP; compare raw
    # workAmount only, not clock time (bench factors and pawn stats differ).
    for recipe_name, count, mill_user, work in (
            ('DankPyon_CraftFlour_Manual', 1, 'CraftingSpot', 300),
            ('DankPyon_CraftFlour', 1, 'DankPyon_Millstone', 100),
            ('DankPyon_CraftFlourBulk', 10, 'DankPyon_Millstone', 800)):
        recipe = get('RecipeDef', recipe_name)
        assert recipe.findtext('recipeUsers/li') == mill_user, recipe_name
        assert float(recipe.findtext('ingredients/li/count')) == count, recipe_name
        assert float(recipe.findtext('products/DankPyon_Flour')) == count, recipe_name
        assert float(recipe.findtext('products/Hay')) == count, recipe_name
        assert float(recipe.findtext('workAmount')) == work, ('MO grinding work', recipe_name)
    # MO's native WorkGiver must actually route bills to the millstone.
    giver = get('WorkGiverDef','DankPyon_DoBillsMillstone')
    assert giver.findtext('giverClass') == 'WorkGiver_DoBill'
    assert giver.findtext('workType') == 'Cooking'
    assert 'DankPyon_Millstone' in [n.text for n in giver.findall('fixedBillGiverDefs/li')]
    wheat = get('ThingDef','DankPyon_Plant_Wheat')
    assert float(wheat.findtext('plant/growDays')) == 12
    assert float(wheat.findtext('plant/harvestYield')) == 28
    assert wheat.findtext('plant/harvestedThingDef') == 'DankPyon_RawWheat'
    get('ThingDef','DankPyon_RawWheat')
    print('Supplied MO 1.6 flour/mill/WorkGiver/wheat source audit: PASS')


if __name__ == '__main__':
    import argparse
    parser=argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--mo-root',type=Path,help='Installed MO folder or Workshop ZIP archive')
    args=parser.parse_args()
    validate()
    if args.mo_root:validate_mo_source(args.mo_root)
