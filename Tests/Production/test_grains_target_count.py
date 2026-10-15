#!/usr/bin/env python3
"""Source-only gate: target-count support for MO multi-product threshing."""
from pathlib import Path
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[2]
CLASS = "AMJ.Grains.RecipeWorkerCounter_PrimaryGrain"
SOURCE = ROOT / "Source/AMJGrains/RecipeWorkerCounter_PrimaryGrain.cs"
MO_PATCH = ROOT / "Compatibility/MedievalOverhaul/Patches/MedievalOverhaul_StageA_Base.xml"
MO_WHEAT = ROOT / "Compatibility/MedievalOverhaul/Defs/Recipes_MOWheat.xml"
BASE = ROOT / "Defs/RecipeDefs/Recipes_GrainProcessing.xml"


def main():
    code = SOURCE.read_text(encoding="utf-8")
    assert "override bool CanCountProducts(Bill_Production bill)" in code
    assert "RecipeWorkerCounter_PrimaryGrain : RecipeWorkerCounter" in code
    patch = ET.parse(MO_PATCH)
    names = {f"AMJC_Thresh{grain}{bulk}"
             for grain in ("Millet", "Buckwheat", "Barley", "Rice")
             for bulk in ("", "Bulk")}
    assigned = set()
    for op in patch.findall("./Operation"):
        if op.findtext("value/workerCounterClass") != CLASS:
            continue
        xpath = op.findtext("xpath") or ""
        name = next((n for n in names if xpath == f'/Defs/RecipeDef[defName="{n}"]'), None)
        assert name and name not in assigned, ("unexpected/duplicate counter assignment", xpath)
        assert op.get("Class") == "PatchOperationAdd"
        assigned.add(name)
    assert assigned == names, ("missing counter assignment", names - assigned)
    root = ET.parse(BASE)
    recipes = {el.findtext("defName"): el for el in root.findall("./RecipeDef")}
    for name in names:
        rec = recipes[name]
        assert rec.find("workerCounterClass") is None
        output = rec.findall("products/*")
        assert len(output) == 1 and output[0].tag != "DankPyon_Straw"
        expected = "10" if name.endswith("Bulk") else "1"
        assert output[0].text == expected
        assert rec.findtext("targetCountAdjustment") == expected
    wheat = ET.parse(MO_WHEAT)
    assert wheat.findtext("./RecipeDef[@Name='AMJC_WheatProcessingBase']/workerCounterClass") == CLASS
    for rec in wheat.findall("./RecipeDef[@ParentName='AMJC_WheatProcessingBase']"):
        count = "10" if rec.findtext("defName").endswith("Bulk") else "1"
        products = rec.findall("products/*")
        assert [(p.tag, p.text) for p in products] == [
            ("AMJC_Wheat", count), ("DankPyon_Straw", count)]
    print("MO multi-output grain-primary target-count source: PASS (NOT a compiled DLL/runtime test)")


if __name__ == "__main__":
    main()
