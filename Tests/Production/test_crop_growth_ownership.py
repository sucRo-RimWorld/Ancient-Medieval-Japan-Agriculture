"""Grains owns rice, wheat and seven grain crops, not broad Vanilla/MO crop timing."""
from pathlib import Path
import re
import unittest
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[2]
EXTERNAL_VANILLA = {
    "Plant_Potato", "Plant_Corn", "Plant_Strawberry",
    "Plant_Cotton", "Plant_Hops", "Plant_Smokeleaf",
}
EXTERNAL_MO = {
    "DankPyon_Plant_Onions", "DankPyon_Plant_Lentils",
    "DankPyon_Plant_Garlic", "DankPyon_Plant_Flax",
    "DankPyon_Plant_Sugarcane", "DankPyon_Plant_Carrots",
    "DankPyon_Plant_Tomatoes",
}
EXTERNAL = EXTERNAL_VANILLA | EXTERNAL_MO
PATTERN = re.compile(r'/Defs/ThingDef\[defName="([^"]+)"\]/plant/growDays$')

class GrainsCropGrowthOwnership(unittest.TestCase):
    def test_no_external_growth_balance_patch(self):
        self.assertFalse((ROOT / "Patches/CropGrowthBalance.xml").exists())
        self.assertFalse((ROOT / "Compatibility/MedievalOverhaul/Patches/CropGrowthBalance.xml").exists())
        for folder in ("Patches", "Compatibility/MedievalOverhaul/Patches", "BaseWithoutMO/Patches"):
            for file in (ROOT / folder).rglob("*.xml"):
                for element in ET.parse(file).getroot().iter("xpath"):
                    match = PATTERN.fullmatch(element.text or "")
                    if match:
                        self.assertNotIn(match.group(1), EXTERNAL,
                            f"Grains must not override optional Crop Growth Balance crop: {file}")

    def test_rice_remains_grains_owned(self):
        root = ET.parse(ROOT / "Patches/UplandRice.xml").getroot()
        self.assertTrue(any(
            node.findtext("xpath") == '/Defs/ThingDef[defName="Plant_Rice"]/plant/growDays'
            and node.findtext("value/growDays") == "5"
            for node in root.findall(".//li[@Class='PatchOperationReplace']")
        ))

    def test_standalone_mo_wheat_balance_remains(self):
        path = ROOT / "Compatibility/MedievalOverhaul/Patches/MedievalOverhaul_StageA_Wheat.xml"
        self.assertTrue(path.exists())
        root = ET.parse(path).getroot()
        self.assertTrue(any(
            node.findtext("xpath") == '/Defs/ThingDef[defName="DankPyon_Plant_Wheat"]/plant/fertilityMin'
            for node in root.findall(".//Operation")
        ))

if __name__ == "__main__":
    unittest.main()
