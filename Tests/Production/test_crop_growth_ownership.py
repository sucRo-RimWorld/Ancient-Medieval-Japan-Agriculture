"""Regression: Grains now owns the former CGB timing scope and MO lentil skill.

Read shipped XML rather than historical fixtures. Native RimWorld acceptance is separate.
"""
from pathlib import Path
import unittest
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[2]
VANILLA = {
    "Plant_Potato": "6",
    "Plant_Corn": "13",
    "Plant_Strawberry": "6",
    "Plant_Cotton": "10",
    "Plant_Hops": "7",
    "Plant_Smokeleaf": "9",
}
MO = {
    "DankPyon_Plant_Onions": "8",
    "DankPyon_Plant_Lentils": "8",
    "DankPyon_Plant_Garlic": "10",
    "DankPyon_Plant_Flax": "9",
    "DankPyon_Plant_Sugarcane": "12",
    "DankPyon_Plant_Carrots": "6",
    "DankPyon_Plant_Tomatoes": "7",
}
PREFIX = '/Defs/ThingDef[defName="'
SUFFIX = '"]/plant/growDays'


def growth_ops(path):
    root = ET.parse(ROOT / path).getroot()
    assert root.tag == "Patch"
    actual = {}
    other = []
    for op in root.findall("Operation"):
        xp = op.findtext("xpath")
        if xp and xp.startswith(PREFIX) and xp.endswith(SUFFIX):
            assert op.get("Class") == "PatchOperationReplace", (path, xp)
            name = xp[len(PREFIX):-len(SUFFIX)]
            assert name not in actual, name
            actual[name] = op.findtext("value/growDays")
            assert list(op.find("value"))[0].tag == "growDays"
        else:
            other.append(op)
    return actual, other


class GrainsCropGrowthOwnership(unittest.TestCase):
    def test_vanilla_six_timings_are_absorbed(self):
        timings, other = growth_ops("Patches/ExistingCropGrowthBalance.xml")
        self.assertEqual(timings, VANILLA)
        self.assertEqual(other, [])

    def test_mo_seven_timings_and_lentil_skill_are_absorbed(self):
        path = "Compatibility/MedievalOverhaul/Patches/MO_ExistingCropGrowthBalance.xml"
        timings, other = growth_ops(path)
        self.assertEqual(timings, MO)
        self.assertEqual(len(other), 1)
        op = other[0]
        target = '/Defs/ThingDef[defName="DankPyon_Plant_Lentils"]/plant/sowMinSkill'
        self.assertEqual(op.get("Class"), "PatchOperationConditional")
        self.assertEqual(op.findtext("xpath"), target)
        self.assertEqual(op.findtext("match/xpath"), target)
        self.assertEqual(op.find("match").get("Class"), "PatchOperationReplace")
        self.assertEqual(op.findtext("match/value/sowMinSkill"), "2")
        self.assertEqual(op.find("nomatch").get("Class"), "PatchOperationAdd")
        self.assertEqual(op.findtext("nomatch/xpath"),
                         '/Defs/ThingDef[defName="DankPyon_Plant_Lentils"]/plant')
        self.assertEqual(op.findtext("nomatch/value/sowMinSkill"), "2")

    def test_optional_mo_folder_does_not_shadow_base_or_leak(self):
        loader = ET.parse(ROOT / "loadFolders.xml").getroot()
        entries = loader.findall("v1.6/li")
        self.assertEqual(entries[0].text, "/")
        self.assertEqual(entries[1].text, "Compatibility/MedievalOverhaul")
        self.assertEqual(entries[1].get("IfModActive"), "DankPyon.Medieval.Overhaul")
        self.assertEqual(entries[2].get("IfModNotActive"), "DankPyon.Medieval.Overhaul")
        shared = {f.relative_to(ROOT / "Patches") for f in (ROOT / "Patches").rglob("*.xml")}
        mo = {f.relative_to(ROOT / "Compatibility/MedievalOverhaul/Patches")
              for f in (ROOT / "Compatibility/MedievalOverhaul/Patches").rglob("*.xml")}
        self.assertFalse(shared & mo, "An MO overlay shadows a shared XML patch")
        self.assertFalse((ROOT / "Patches/CropGrowthBalance.xml").exists())
        self.assertFalse((ROOT / "Compatibility/MedievalOverhaul/Patches/CropGrowthBalance.xml").exists())

    def test_rice_and_mo_wheat_remain_grains_owned(self):
        rice = ET.parse(ROOT / "Patches/UplandRice.xml").getroot()
        self.assertTrue(any(
            node.findtext("xpath") == '/Defs/ThingDef[defName="Plant_Rice"]/plant/growDays'
            and node.findtext("value/growDays") == "5"
            for node in rice.findall(".//li[@Class='PatchOperationReplace']")
        ))
        wheat = ET.parse(ROOT / "Compatibility/MedievalOverhaul/Patches/MedievalOverhaul_StageA_Wheat.xml").getroot()
        self.assertTrue(any(
            node.findtext("xpath") == '/Defs/ThingDef[defName="DankPyon_Plant_Wheat"]/plant/fertilityMin'
            for node in wheat.findall(".//Operation")
        ))


if __name__ == "__main__":
    unittest.main()
