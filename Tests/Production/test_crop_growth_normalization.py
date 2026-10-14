"""Static 1.6 Vanilla/MO growth-time compatibility contract; not a native-game PASS."""
import unittest
import xml.etree.ElementTree as ET
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
VANILLA = {
    "Plant_Potato": (5.8, 6), "Plant_Corn": (11.3, 10),
    "Plant_Strawberry": (4.6, 6), "Plant_Cotton": (8, 10),
    "Plant_Hops": (5, 7), "Plant_Smokeleaf": (7.5, 9)
}
MO = {
    "DankPyon_Plant_Onions": (4, 8), "DankPyon_Plant_Lentils": (3, 8),
    "DankPyon_Plant_Garlic": (4, 10), "DankPyon_Plant_Flax": (7, 9),
    "DankPyon_Plant_Sugarcane": (5.8, 12),
    "DankPyon_Plant_Carrots": (3.25, 6),
    "DankPyon_Plant_Tomatoes": (4.25, 7)
}

def rows(path):
    root = ET.parse(ROOT / path).getroot()
    assert root.tag == "Patch"
    found = {}
    for op in root.findall("Operation"):
        assert op.attrib == {"Class": "PatchOperationReplace"}
        assert len(op) == 2
        xpath = op.findtext("xpath")
        assert xpath and xpath.startswith('/Defs/ThingDef[defName="')
        assert xpath.endswith('"]/plant/growDays')
        assert len(op.find("value")) == 1
        new = float(op.findtext("value/growDays"))
        assert xpath not in found, "Duplicate crop patch: " + xpath
        found[xpath] = new
    return found

class AllCropGrowthCompatibility(unittest.TestCase):
    def test_vanilla_crop_days(self):
        self.check("Patches/CropGrowthBalance.xml", VANILLA)

    def test_mo_crop_days_are_optional(self):
        self.check("Compatibility/MedievalOverhaul/Patches/CropGrowthBalance.xml", MO)
        loader = ET.parse(ROOT / "loadFolders.xml").getroot()
        self.assertEqual(loader.find('v1.6/li[@IfModActive="DankPyon.Medieval.Overhaul"]').text,
                         "Compatibility/MedievalOverhaul")

    def check(self, path, expected):
        actual = rows(path)
        names = {f'/Defs/ThingDef[defName="{name}"]/plant/growDays': float(new)
                 for name, (old, new) in expected.items()}
        self.assertEqual(actual, names)
        self.assertNotIn('Plant_Rice', repr(actual))
        self.assertNotIn('DankPyon_Plant_Wheat', repr(actual))
        self.assertGreater(old_to_new_lentil_growth_ratio(), 2.0)

    def test_preserve_rice_and_wheat_owners(self):
        rice = ET.parse(ROOT / "Patches/UplandRice.xml").getroot()
        self.assertTrue(any(x.findtext("xpath") == '/Defs/ThingDef[defName="Plant_Rice"]/plant/growDays'
                            and x.findtext("value/growDays") == "5"
                            for x in rice.findall(".//li[@Class='PatchOperationReplace']")))
        for file in ("Patches/CropGrowthBalance.xml",
                     "Compatibility/MedievalOverhaul/Patches/CropGrowthBalance.xml"):
            xml = (ROOT / file).read_text(encoding="utf-8")
            self.assertNotIn("harvestYield", xml)
            self.assertNotIn("Temperature", xml)
            self.assertNotIn("CCTO", xml)
            self.assertNotIn("AMJC_Plant_", xml)

def old_to_new_lentil_growth_ratio():
    old, new = MO["DankPyon_Plant_Lentils"]
    return new / old

if __name__ == "__main__":
    unittest.main()
