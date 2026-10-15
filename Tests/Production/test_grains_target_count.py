#!/usr/bin/env python3
"""Static MO thresh primary-product counter contract; not a compiled/runtime PASS."""
from pathlib import Path
import xml.etree.ElementTree as ET
R = Path(__file__).resolve().parents[2]
C = "AncientMedievalJapan.Grains.RecipeWorkerCounter_PrimaryGrain"
expected = {"AMJC_Thresh" + crop + batch for crop in ("Millet", "Buckwheat", "Barley", "Rice") for batch in ("", "Bulk")}
patch = ET.parse(R / "Compatibility/MedievalOverhaul/Patches/MedievalOverhaul_StageA_Base.xml").getroot()
seen = {}
for op in patch.findall("Operation"):
    c = op.findtext("value/workerCounterClass")
    if c is None:
        continue
    xp = op.findtext("xpath")
    assert op.get("Class") == "PatchOperationAdd" and xp.startswith('/Defs/RecipeDef[defName="') and xp.endswith('"]')
    n = xp.split('"')[1]
    assert n not in seen
    seen[n] = c
assert seen == dict.fromkeys(expected, C), seen
common = ET.parse(R / "Defs/RecipeDefs/Recipes_GrainProcessing.xml").getroot()
defs = {r.findtext("defName"): r for r in common.findall("RecipeDef") if r.findtext("defName")}
for name in expected:
    rec = defs[name]
    out = rec.findall("products/*")
    assert len(out) == 1 and out[0].tag != "DankPyon_Straw" and rec.find("workerCounterClass") is None, name
wheat = ET.parse(R / "Compatibility/MedievalOverhaul/Defs/Recipes_MOWheat.xml").getroot()
wheat_recs = [r for r in wheat.findall("RecipeDef") if r.findtext("defName")]
assert {r.findtext("defName") for r in wheat_recs} == {"AMJC_ThreshWheat", "AMJC_ThreshWheatBulk"}
for r in wheat_recs:
    assert r.findtext("workerCounterClass") == C
    assert [o.tag for o in r.findall("products/*")] == ["AMJC_Wheat", "DankPyon_Straw"]
assert all(r.find("workerCounterClass") is None for r in ET.parse(R / "BaseWithoutMO/Defs/Recipes_Wheat.xml").getroot().findall("RecipeDef"))
s = (R / "Source/AMJGrains/RecipeWorkerCounter_PrimaryGrain.cs").read_text()
assert "override bool CanCountProducts(Bill_Production bill)" in s
assert 'recipe.products[i].thingDef.defName != "DankPyon_Straw"' in s
assert "override int CountProducts(" not in s
cs = ET.parse(R / "Source/AMJGrains/AMJGrains.csproj")
assert cs.findtext("PropertyGroup/TargetFramework") == "net481"
assert cs.findtext("PropertyGroup/AssemblyName") == "AMJGrains"
print("PASS: 10 MO thresh counters, Base unchanged, primary grain count only")
