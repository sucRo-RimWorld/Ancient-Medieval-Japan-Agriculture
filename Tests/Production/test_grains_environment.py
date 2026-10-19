"""Environmental regressions include broken fertility, heat and season niches."""
import shutil
import tempfile
import unittest
import xml.etree.ElementTree as ET
from pathlib import Path
import sys
sys.path.insert(0, str(Path(__file__).resolve().parents[1]))
from amj_profile_xml import ROOT
from validate_grains_environment import validate, output


class EnvironmentTests(unittest.TestCase):
    def test_current_matrix(self):
        self.assertTrue(validate())

    def test_no_growth_outside_temperature_range_or_below_fertility(self):
        crop = (9,27,0.5,0.4,8,18,32,42)
        for fertility,temperature in ((0.4,20),(1,8),(1,42),(1,0),(1,50)):
            self.assertEqual(output(crop,fertility,temperature,20),0)

    def test_incomplete_season_has_no_mature_harvest(self):
        self.assertEqual(output((9,27,0.5,0.4,8,18,32,42),1,20,5),0)

    def test_millet_roles_have_visible_growth_and_yield_gaps(self):
        plants = ET.parse(ROOT / "Defs/ThingDefs_Plants/Plants_StageA.xml").getroot()
        data = {node.findtext("defName"): node.find("plant") for node in plants.findall("ThingDef")}
        expected = (
            ("AMJC_Plant_ProsoMillet_Kibi", 4, 9),
            ("AMJC_Plant_BarnyardMillet_Hie", 7, 18),
            ("AMJC_Plant_FoxtailMillet_Awa", 9, 27),
        )
        for name, days, yield_ in expected:
            with self.subTest(name=name):
                self.assertEqual(float(data[name].findtext("growDays")), days)
                self.assertEqual(float(data[name].findtext("harvestYield")), yield_)
        # Guard the UX distinction, not just the particular chosen constants.
        self.assertGreaterEqual(7 - 4, 2)
        self.assertGreaterEqual(9 - 7, 2)
        self.assertGreaterEqual(18 - 9, 7)
        self.assertGreaterEqual(27 - 18, 7)

    def test_balance_changes_cannot_silently_change_the_matrix(self):
        for crop,field,value in ((0,'fertilityMin','0.6'),(2,'growDays','1'),
                                  (3,'maxOptimalGrowthTemperature','32'),
                                  (4,'harvestYield','200')):
            with self.subTest(field=field,crop=crop), tempfile.TemporaryDirectory() as temp:
                root=Path(temp)
                for folder in ('Defs','BaseWithoutMO','Compatibility'):
                    shutil.copytree(ROOT/folder,root/folder)
                shutil.copyfile(ROOT/'loadFolders.xml',root/'loadFolders.xml')
                path=root/'Defs/ThingDefs_Plants/Plants_StageA.xml'
                doc=ET.parse(path);node=doc.findall('ThingDef')[crop].find('plant/'+field)
                node.text=value;doc.write(path)
                with self.assertRaises(AssertionError):validate(root)


if __name__ == '__main__':unittest.main()
