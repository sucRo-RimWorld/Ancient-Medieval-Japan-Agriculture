"""Stage-2 boundary and pre-split MO contracts; no runtime/art/wheat-chain claim."""
import hashlib
import json
import re
import xml.etree.ElementTree as ET
from copy import deepcopy
from amj_profile_xml import ROOT, MO_FOLDER, profile_xml, contract_signature
from stack_graphics import stack_textures

MO_GRAPHIC_PATHS = {
    "Things/Plants/FullGrown/WheatPlant",
    "Things/Plants/Immature/WheatPlant",
    "Things/Building/Production/StonecuttingSpot",
    "Things/Building/Production/Millstone",
}

# Explicitly requested visual changes only. Historical hashes remain immutable;
# validate exact current paths, then normalize only graphics for the MO contract.
AMJ_GRAPHICS = {
    "AMJC_Plant_FoxtailMillet_Awa": {
        "graphicData/texPath": ("Things/Plants/FullGrown/AMJC_Awa", "Things/Plants/FullGrown/AMJC_Awa"),
        "plant/immatureGraphicPath": ("Things/Plants/Immature/AMJC_Awa", "Things/Plants/Immature/AMJC_Awa"),
    },
    "AMJC_Plant_BarnyardMillet_Hie": {
        "graphicData/texPath": ("Things/Plants/FullGrown/AMJC_Hie", "Things/Plants/FullGrown/AMJC_Hie"),
        "plant/immatureGraphicPath": ("Things/Plants/Immature/AMJC_Hie", "Things/Plants/Immature/AMJC_Hie"),
    },
    "AMJC_Plant_ProsoMillet_Kibi": {
        "graphicData/texPath": ("Things/Plants/FullGrown/AMJC_Kibi", "Things/Plants/FullGrown/AMJC_Kibi"),
        "plant/immatureGraphicPath": ("Things/Plants/Immature/AMJC_Kibi", "Things/Plants/Immature/AMJC_Kibi"),
    },
    "AMJC_Plant_Buckwheat_Soba": {
        "graphicData/texPath": ("Things/Plants/FullGrown/AMJC_Soba", "Things/Plants/FullGrown/AMJC_Soba"),
        "plant/immatureGraphicPath": ("Things/Plants/Immature/AMJC_Soba", "Things/Plants/Immature/AMJC_Soba"),
    },
    "AMJC_Plant_Barley": {
        "graphicData/texPath": ("Things/Plants/FullGrown/AMJC_Barley", "Things/Plants/FullGrown/WheatPlant"),
        "plant/immatureGraphicPath": ("Things/Plants/Immature/AMJC_Barley", "Things/Plants/Immature/WheatPlant"),
    },
    "AMJC_RawMillet": {
        "graphicData/texPath": ("Things/Item/Resource/AMJC_Millet/MilletSheaf", "Things/Item/Resource/AMJC_Millet/RawMillet"),
    },
    "AMJC_RawBuckwheat": {
        "graphicData/texPath": ("Things/Item/Resource/AMJC_Buckwheat/BuckwheatSheaf", "Things/Item/Resource/AMJC_Buckwheat/RawBuckwheat"),
    },
    "AMJC_RawBarley": {
        "graphicData/texPath": ("Things/Item/Resource/AMJC_Barley/BarleySheaf", "Things/Item/Resource/AMJC_Millet/RawMillet"),
    },
    "AMJC_BarleyInHull": {
        "graphicData/texPath": ("Things/Item/Resource/AMJC_Barley/BarleyInHull", "Things/Item/Resource/AMJC_Millet/MilletInHull"),
    },
    "AMJC_Barley": {
        "graphicData/texPath": ("Things/Item/Resource/AMJC_Barley/Barley", "Things/Item/Resource/AMJC_Millet/Millet"),
    },
    "AMJC_Wheat": {
        "graphicData/texPath": ("Things/Item/Resource/AMJC_Wheat/Wheat", "Things/Item/Resource/AMJC_Millet/Millet"),
    },
    "AMJC_GrainProcessingSpot": {
        "graphicData/texPath": ("Things/Building/Production/TableStonecutter", "Things/Building/Production/StonecuttingSpot"),
    },
    "AMJC_GrainProcessingTable": {
        "graphicData/texPath": ("Things/Building/Production/TableStonecutter", "Things/Building/Production/Millstone"),
    },
}


# Immutable MO pre-split hashes include the five shared PlantDef descriptions.
# Do not rewrite that snapshot for author-approved historical localization.
# Check all five present descriptions against the canonical approved English in
# Docs/LocalizationHistoricalReview.md §8, then restore ONLY the historical
# description text on a deep copy for the existing all-fields hash contract.
HISTORICAL_MO_PLANT_DESCRIPTIONS = {
    "AMJC_Plant_FoxtailMillet_Awa": "Foxtail millet (awa), a traditional dryland millet used as a staple grain. It takes longer to mature than kibi but produces more grain per harvest, making it well suited to ordinary main fields.",
    "AMJC_Plant_BarnyardMillet_Hie": "Japanese barnyard millet (hie), a traditional grain crop suited to cool growing conditions. It can keep growing at lower temperatures than awa or kibi, but it is not especially frost hardy.",
    "AMJC_Plant_ProsoMillet_Kibi": "Proso millet (kibi), a fast-growing traditional millet suited to short growing seasons and poorer soils. It matures faster than awa or hie but produces less grain per harvest.",
    "AMJC_Plant_Buckwheat_Soba": "Buckwheat (soba), a very fast-growing grain crop suited to poor soils and short growing seasons. It performs well where richer-soil cereals are inefficient, but it is relatively vulnerable to frost.",
    "AMJC_Plant_Barley": "Barley, a cool-season cereal crop suited to relatively poor soils and cold growing conditions. It takes longer to mature than the Stage A millets but yields a larger harvest and is useful as a staple grain.",
}


def approved_plant_descriptions(root):
    text = (root / "Docs/LocalizationHistoricalReview.md").read_text(encoding="utf-8")
    assert "## 8. English descriptions translated" in text, "Missing approved English crop section"
    chapter = text.split("## 8.", 1)[1].split("## 9.", 1)[0]
    entries = {}
    for row in chapter.splitlines():
        if not row.startswith("| `"):
            continue
        cells = [value.strip() for value in row.split("|")]
        name = cells[1].strip("`")
        assert name not in entries, "Duplicate approved English crop description: " + name
        entries[name] = cells[2]
    assert set(entries) == set(HISTORICAL_MO_PLANT_DESCRIPTIONS) | {"Plant_Rice"}, (
        "Approved English crop list changed"
    )
    return entries



# The nine previously hashed shared item descriptions were updated by the
# author's V1 localization request. Preserve immutable MO history by checking
# each CURRENT copy against the bilingual canonical table in §16, then reverting
# ONLY the description on the deepcopy used for the old 38-contract comparison.
HISTORICAL_MO_STAGE_DESCRIPTIONS = {
    "AMJC_RawMillet": "Harvested millet stalks and seed heads bundled together before threshing and hulling. They keep well in storage but are not yet suitable for eating.",
    "AMJC_MilletInHull": "Millet grain after threshing but before the hulls have been removed. The hull protects the grain during storage, but it is not yet suitable for eating.",
    "AMJC_Millet": "Edible millet grain with its hull removed. A basic staple that can be eaten as grain or used as an ingredient in ordinary meals.",
    "AMJC_RawBuckwheat": "Harvested buckwheat stems and seed clusters before threshing. It stores well but is not yet suitable for eating.",
    "AMJC_BuckwheatInHull": "Threshed buckwheat grain with its hull still attached. The hull protects it in storage, but it is not yet suitable for eating.",
    "AMJC_Buckwheat": "Edible buckwheat grain with its hull removed. It is a short-lived staple grain suited to direct cooking; flour is introduced only when a concrete flour-based use is implemented.",
    "AMJC_RawBarley": "Harvested barley stalks and heads before threshing. It stores well but is not yet suitable for eating.",
    "AMJC_BarleyInHull": "Threshed barley grain with its hull still attached. The hull protects it during storage, but it is not yet suitable for eating.",
    "AMJC_Barley": "Edible barley grain with its hull removed. A durable staple grain that can be cooked directly and can support additional barley-based processing in later content."
}


def reviewed_stage_descriptions(root):
    review = (root / "Docs/LocalizationHistoricalReview.md").read_text(encoding="utf-8")
    assert "## 16. 作者指示による穀粒状態" in review
    rows = {}
    for row in review.split("## 16.", 1)[1].splitlines():
        if not row.startswith("| `AMJC_"):
            continue
        cells = [v.strip() for v in row.split("|")]
        assert len(cells) == 5, row
        name = cells[1].strip("`")
        assert name not in rows, "Duplicate review Def: " + name
        rows[name] = cells[3]
    expected = set(HISTORICAL_MO_STAGE_DESCRIPTIONS) | {
        "AMJC_MixedGrainMeal", "AMJC_MixedGrainPorridge", "AMJC_MixedFlourFood"
    }
    assert set(rows) == expected, "Reviewed bilingual Def inventory changed"
    return {name: rows[name] for name in HISTORICAL_MO_STAGE_DESCRIPTIONS}


def validate():
    # MO identifiers/classes must be absent from Base Defs and localization.
    for folder in ("Defs", "Languages", "BaseWithoutMO"):
        for path in sorted((ROOT / folder).rglob("*.xml")):
            document = ET.parse(path).getroot()
            for node in document.iter():
                text = " ".join([str(node.tag), (node.text or "").strip(), *node.attrib.values()])
                assert not re.search(r"DankPyon_|MedievalOverhaul\.", text), f"Base MO reference: {path} {node.tag}"
    assert not (ROOT / "Patches/MedievalOverhaul_StageA_Wheat.xml").exists()
    assert not (ROOT / "Languages/Japanese/DefInjected/ThingDef/AMJC_MO_Overrides.xml").exists()
    for path in (ROOT / "Patches").rglob("*.xml"):
        assert "DankPyon_" not in path.read_text(encoding="utf-8"), f"MO patch escaped conditional load folder: {path}"
    conditional_label = ROOT / MO_FOLDER / "Languages/Japanese/DefInjected/ThingDef/AMJC_MO_Overrides.xml"
    assert ET.parse(conditional_label).findtext("DankPyon_RawWheat.label") == "小麦束"

    base = profile_xml("vanilla")
    mo = profile_xml("mo")
    # AMJG-owned fields must not inherit obsolete MO research/material rules.
    for document in (base, mo):
        barley = document.find('ThingDef[defName="AMJC_Plant_Barley"]')
        table = document.find('ThingDef[defName="AMJC_GrainProcessingTable"]')
        assert barley is not None and barley.find("plant/sowResearchPrerequisites") is None, (
            "AMJ MO field priority lost: barley sow research")
        assert table is not None and table.find("researchPrerequisites") is None, (
            "AMJ MO field priority lost: processing table research")
        cost = table.find("costList") if table is not None else None
        assert cost is not None and len(cost) == 1 and cost.findtext("Steel") == "30", (
            "AMJ MO field priority lost: processing table cost")
    for name, paths in AMJ_GRAPHICS.items():
        for document in (base, mo):
            target = document.find('ThingDef[defName="' + name + '"]')
            assert target is not None, name
            for element, (expected, historical) in paths.items():
                assert target.findtext(element) == expected, "AMJ graphics priority lost: " + name + "/" + element
    for node in base.iter():
        if node.tag in {"texPath", "immatureGraphicPath"}:
            assert (node.text or "").strip() not in MO_GRAPHIC_PATHS, "Base MO texture reference: " + str(node.text)
    base_recipes = {n.findtext("defName") for n in base.findall("RecipeDef")}
    for path in list((ROOT / "Languages").glob("*/DefInjected/RecipeDef/*.xml")) + list((ROOT / "BaseWithoutMO/Languages").glob("*/DefInjected/RecipeDef/*.xml")):
        for translation in ET.parse(path).getroot():
            assert translation.tag.split(".")[0] in base_recipes, f"Base orphan RecipeDef translation: {translation.tag}"
    names = [n.findtext("defName") or n.get("Name") for n in mo]
    assert len(names) == len(set(names)), "Duplicate explicit AMJ Def/Parent name"
    golden = json.loads((ROOT / "Tests/Fixtures/MO_PreSplit_Contracts.json").read_text())
    for source, expected in golden["movedFileSha256"].items():
        assert hashlib.sha256((ROOT / MO_FOLDER / source).read_bytes()).hexdigest() == expected, source
    translations = {}
    for path in (ROOT / "Languages/Japanese/DefInjected/RecipeDef/AMJC_StageA.xml",
                 ROOT / MO_FOLDER / "Languages/Japanese/DefInjected/RecipeDef/AMJC_MOWheat.xml"):
        for node in ET.parse(path).getroot():
            assert node.tag not in translations, f"Duplicate RecipeDef translation: {node.tag}"
            translations[node.tag] = node.text
    # The 2026-10-08 approved *functional* wording correction removes the
    # unsupported claim of a Straw byproduct in the common/Base locale.
    # Preserve the immutable MO pre-split fixture, compare this exactly
    # allowlisted translation delta, then compare ALL other keys unchanged.
    # (MO still gets Straw from its conditional thresh Recipe.)
    neutral_descriptions = {
        'AMJC_ThreshMillet.description': '雑穀束を脱穀し、殻付き雑穀にする。',
        'AMJC_ThreshMilletBulk.description': '雑穀束10個をまとめて脱穀し、殻付き雑穀にする。',
        'AMJC_ThreshBuckwheat.description': 'ソバ束を脱穀し、殻付きソバにする。',
        'AMJC_ThreshBuckwheatBulk.description': 'ソバ束10個をまとめて脱穀し、殻付きソバにする。',
        'AMJC_ThreshBarley.description': '大麦束を脱穀し、殻付き大麦にする。',
        'AMJC_ThreshBarleyBulk.description': '大麦束10個をまとめて脱穀し、殻付き大麦にする。',
    }
    for key, value in neutral_descriptions.items():
        assert translations[key] == value, 'Approved neutral processing text changed: ' + key
        translations[key] = golden["recipeTranslations"][key]
    assert translations == golden["recipeTranslations"], "MO recipe translations changed during split"
    crop_descriptions = approved_plant_descriptions(ROOT)
    stage_descriptions = reviewed_stage_descriptions(ROOT)
    # The Grains storage filter hierarchy is separate from MO's recipe filter.
    categories = {n.findtext("defName"): n.findtext("parent") for n in base.findall("ThingCategoryDef")}
    assert {n: categories[n] for n in (
        "AMJC_Grains", "AMJC_GrainSheaves", "AMJC_GrainKernels", "AMJC_GrainFlour"
    )} == {
        "AMJC_Grains": "Foods", "AMJC_GrainSheaves": "AMJC_Grains",
        "AMJC_GrainKernels": "AMJC_Grains", "AMJC_GrainFlour": "AMJC_Grains",
    }, "Grain storage category hierarchy"
    grain_members = {
        "AMJC_GrainSheaves": ("AMJC_RawMillet", "AMJC_RawBuckwheat", "AMJC_RawBarley", "AMJC_RiceSheaf"),
        "AMJC_GrainKernels": ("AMJC_MilletInHull", "AMJC_BuckwheatInHull",
            "AMJC_BarleyInHull", "AMJC_RiceInHull", "AMJC_Millet", "AMJC_Buckwheat", "AMJC_Barley"),
        "AMJC_GrainFlour": ("AMJC_BuckwheatFlour", "AMJC_MilletFlour",
            "AMJC_BarleyFlour", "AMJC_RiceFlour"),
    }
    for doc in (base, mo):
        items = {n.findtext("defName"): n for n in doc.findall("ThingDef")}
        for category, names in grain_members.items():
            for name in names:
                assert [li.text for li in items[name].findall("thingCategories/li")] == [category], (
                    "Grain storage membership: " + name)
    # PlantFoodRawBase lists are additive: edible grains must not inherit a second
    # direct PlantFoodRaw leaf beside their dedicated grain-kernel category.
    for doc in (base, mo):
        for name in ("AMJC_Millet", "AMJC_Buckwheat", "AMJC_Barley"):
            assert doc.find('ThingDef[defName="' + name + '"]/thingCategories').get("Inherit") == "False", (
                "Duplicate raw-vegetable membership: " + name)
    for path, item, category in (
        ("BaseWithoutMO/Defs/Items_Wheat.xml", "AMJC_RawWheat", "AMJC_GrainSheaves"),
        ("BaseWithoutMO/Defs/Items_Flour.xml", "AMJC_WheatFlour", "AMJC_GrainFlour"),
    ):
        defs = ET.parse(ROOT / path)
        assert defs.findtext("ThingDef/defName") == item
        assert [n.text for n in defs.findall("ThingDef/thingCategories/li")] == [category], item
    wheat_patch = ET.parse(ROOT / "BaseWithoutMO/Patches/GrainCategories_Wheat.xml")
    assert wheat_patch.findtext("Operation/xpath") == '/Defs/ThingDef[defName="AMJC_Wheat"]'
    assert wheat_patch.findtext("Operation/value/thingCategories/li") == "AMJC_GrainKernels"
    assert wheat_patch.find("Operation/value/thingCategories").get("Inherit") == "False"
    rice_patch = ET.parse(ROOT / "Patches/GrainCategories_Rice.xml")
    assert rice_patch.findtext("Operation/xpath") == '/Defs/ThingDef[defName="RawRice"]/thingCategories'
    assert rice_patch.findtext("Operation/match/xpath") == '/Defs/ThingDef[defName="RawRice"]/thingCategories'
    assert rice_patch.findtext("Operation/match/value/li") == "AMJC_GrainKernels"
    assert rice_patch.findtext("Operation/nomatch/xpath") == '/Defs/ThingDef[defName="RawRice"]'
    assert rice_patch.findtext("Operation/nomatch/value/thingCategories/li") == "AMJC_GrainKernels"
    assert rice_patch.find("Operation/nomatch/value/thingCategories").get("Inherit") == "False"
    assert rice_patch.findtext("Operation[@Class='PatchOperationAttributeSet']/attribute") == "Inherit"
    assert rice_patch.findtext("Operation[@Class='PatchOperationAttributeSet']/value") == "False"
    assert rice_patch.findall("Operation[@Class='PatchOperationConditional']")[-1].findtext("match/xpath") == (
        '/Defs/ThingDef[defName="RawRice"]/thingCategories/li[text()="PlantFoodRaw"]')
    mo_base = ET.parse(ROOT / MO_FOLDER / "Patches/MedievalOverhaul_StageA_Base.xml")
    mo_cereal = mo_base.find('Operation/value/thingCategories')
    assert mo_cereal is not None and mo_cereal.get("Inherit") == "False"
    assert [x.text for x in mo_cereal.findall("li")] == ["DankPyon_Cereal"]
    mo_patch = ET.parse(ROOT / MO_FOLDER / "Patches/ZZ_GrainCategories_MO.xml")
    mo_ops = mo_patch.findall("Operation")
    assert [(op.get("Class"), op.findtext("xpath")) for op in mo_ops] == [
        ("PatchOperationReplace", '/Defs/ThingDef[defName="DankPyon_RawWheat"]/thingCategories'),
        ("PatchOperationAdd", '/Defs/ThingDef[defName="AMJC_Wheat"]/thingCategories'),
        ("PatchOperationReplace", '/Defs/ThingDef[defName="DankPyon_Flour"]/thingCategories'),
        ("PatchOperationAdd", '/Defs/RecipeDef//*[categories/li="DankPyon_Flour"]/thingDefs[not(li="DankPyon_Flour")]'),
        ("PatchOperationAdd", '/Defs/RecipeDef//*[categories/li="DankPyon_Flour" and not(thingDefs)]'),
        ("PatchOperationRemove", '/Defs/RecipeDef//categories/li[text()="DankPyon_Flour"]'),
        ("PatchOperationAdd", '/Defs/ThingDef//filter[categories/li="DankPyon_Flour"]/thingDefs[not(li="DankPyon_Flour")]'),
        ("PatchOperationAdd", '/Defs/ThingDef//filter[categories/li="DankPyon_Flour" and not(thingDefs)]'),
        ("PatchOperationRemove", '/Defs/ThingDef//filter/categories/li[text()="DankPyon_Flour"]'),
    ], "MO flour tag removal must preserve recipe and shelf item filters"
    assert mo_ops[0].findtext("value/thingCategories/li") == "AMJC_GrainSheaves"
    assert mo_ops[0].find("value/thingCategories").get("Inherit") == "False"
    assert mo_ops[1].findtext("value/li") == "AMJC_GrainKernels"
    assert [n.text for n in mo_ops[2].findall("value/thingCategories/li")] == ["AMJC_GrainFlour"]
    assert mo_ops[2].find("value/thingCategories").get("Inherit") == "False"
    bridge = ET.parse(ROOT / "Patches/GrainCategories_RawFoodFilters.xml")
    assert len(bridge.findall("Operation[@Class='PatchOperationConditional']")) == 2
    for op in bridge.findall("Operation"):
        assert 'AMJC_Grains' in op.findtext("match/value/li"), "Raw-food category acceptance bridge"
    mo_wheat = next(n for n in mo.findall("ThingDef") if n.findtext("defName") == "AMJC_Wheat")
    assert [li.text for li in mo_wheat.findall("thingCategories/li")] == ["DankPyon_Cereal"]
    actual = {}
    for node in mo:
        name = node.findtext("defName") or node.get("Name")
        if node.tag == "ThingCategoryDef":
            continue  # New storage UI categories are absent from the historical MO fixture.
        if name and name.startswith("AMJC_"):
            normalized = deepcopy(node)
            if node.tag == "ThingDef" and name in {
                "AMJC_RawMillet", "AMJC_RawBuckwheat", "AMJC_RawBarley",
                "AMJC_MilletInHull", "AMJC_BuckwheatInHull", "AMJC_BarleyInHull",
                "AMJC_Millet", "AMJC_Buckwheat", "AMJC_Barley",
            }:
                category = ("AMJC_GrainSheaves" if name.startswith("AMJC_Raw")
                            else "AMJC_GrainKernels")
                found = normalized.find("thingCategories")
                assert found is not None and [li.text for li in found.findall("li")] == [category], (
                    "Grain category contract drift: " + name)
                normalized.remove(found)
                if name.startswith("AMJC_Raw") or name.endswith("InHull"):
                    historical = ET.SubElement(normalized, "thingCategories")
                    ET.SubElement(historical, "li").text = "Foods"
            # Preserve the immutable six-crop snapshot by undoing only
            # the explicitly checked storage correction on the comparison copy.
            if node.tag == "ThingDef" and name in {
                "AMJC_RawMillet", "AMJC_RawBuckwheat", "AMJC_RawBarley",
            }:
                rot = normalized.find("comps/li[@Class='CompProperties_Rottable']/daysToRotStart")
                assert rot is not None and rot.text == "30", "Sheaf spoilage regression: " + name
                rot.text = "120"
            for element, (expected, historical) in AMJ_GRAPHICS.get(name, {}).items():
                normalized.find(element).text = historical
            # Normalize only the three superseded MO settings on a deepcopy
            # for the immutable historical fixture, never in production XML.
            # Beta thresh/hull stove fallback changes only the abstract AMJ
            # processing bench list. Compare that approved delta separately,
            # then normalize just the two added stoves for the immutable
            # historical pre-split MO snapshot. Do not rewrite the fixture.
            if name in {
                "AMJC_MilletProcessingBase", "AMJC_BuckwheatProcessingBase",
                "AMJC_BarleyProcessingBase", "AMJC_WheatProcessingBase",
            }:
                users = normalized.find("recipeUsers")
                assert users is not None, "Historical AMJ processing parent has no recipeUsers: " + name
                assert [li.text for li in users.findall("li")] == [
                    "AMJC_GrainProcessingSpot", "AMJC_GrainProcessingTable",
                    "FueledStove", "ElectricStove",
                ], "Beta processing stove allowlist differs: " + name
                for li in list(users)[2:]:
                    users.remove(li)
            if name == "AMJC_Plant_Barley":
                sow = ET.SubElement(normalized.find("plant"), "sowResearchPrerequisites")
                ET.SubElement(sow, "li").text = "DankPyon_BasicAgriculture"
            if name == "AMJC_GrainProcessingTable":
                cost = normalized.find("costList")
                cost.remove(cost.find("Steel"))
                ET.SubElement(cost, "DankPyon_IronIngot").text = "30"
                research = ET.SubElement(normalized, "researchPrerequisites")
                ET.SubElement(research, "li").text = "DankPyon_BasicAgriculture"
            if name in HISTORICAL_MO_STAGE_DESCRIPTIONS:
                assert normalized.findtext("description") == stage_descriptions[name], (
                    "Reviewed stage description drift: " + name
                )
                normalized.find("description").text = HISTORICAL_MO_STAGE_DESCRIPTIONS[name]
            if name in HISTORICAL_MO_PLANT_DESCRIPTIONS:
                assert normalized.findtext("description") == crop_descriptions[name], (
                    "Approved English crop description changed: " + name
                )
                normalized.find("description").text = HISTORICAL_MO_PLANT_DESCRIPTIONS[name]
            # Counter is a new, independently checked MO-only addition.
            # Keep the immutable historical recipe fixture unchanged.
            if node.tag == "RecipeDef" and name in {
                "AMJC_Thresh" + crop + bulk
                for crop in ("Millet", "Buckwheat", "Barley", "Rice", "Wheat")
                for bulk in ("", "Bulk")
            }:
                counter = normalized.find("workerCounterClass")
                assert counter is not None and counter.text == "AncientMedievalJapan.Grains.RecipeWorkerCounter_PrimaryGrain", (
                    "MO primary-grain target-count counter missing: " + name)
                normalized.remove(counter)
            raw = json.dumps(contract_signature(normalized), ensure_ascii=False, separators=(",", ":"))
            actual[node.tag + ":" + name] = hashlib.sha256(raw.encode()).hexdigest()
    from validate_grains_chain import SHARED_NAMES
    rice_names = {"ThingDef:AMJC_RiceSheaf", "ThingDef:AMJC_RiceInHull",
                  "RecipeDef:AMJC_RiceProcessingBase",
                  "RecipeDef:AMJC_ThreshRice", "RecipeDef:AMJC_ThreshRiceBulk",
                  "RecipeDef:AMJC_HullRice", "RecipeDef:AMJC_HullRiceBulk"}
    # Preserve the immutable historical fixture; scenario contracts belong to Scenarios.
    scenario_keys = {"FactionDef:AMJC_PlayerVillage", "PawnKindDef:AMJC_Villager", "ScenarioDef:AMJC_NewVillage"}
    grains_golden = {k: v for k, v in golden["sha256"].items() if k not in scenario_keys}
    assert set(actual) == set(grains_golden) | {n.tag+":"+n.findtext("defName") for n in mo if n.findtext("defName") in SHARED_NAMES} | rice_names, "Unknown MO AMJC contract"
    assert {k: actual[k] for k in grains_golden} == grains_golden, "MO-loaded explicit AMJC grain contracts differ from the pre-split snapshot"
    for doc in (base, mo):
        assert not any(node.findtext("defName") in {"AMJC_NewVillage", "AMJC_PlayerVillage", "AMJC_Villager"} for node in doc), "Grains must not provide Scenarios-owned Defs"
    def get(name):
        matches = [node for node in base if node.findtext("defName") == name]
        assert len(matches) == 1, name
        return matches[0]
    assert get("AMJC_Plant_Barley").find("plant/sowResearchPrerequisites") is None
    barley = get("AMJC_Plant_Barley")
    for element, folder in (("graphicData/texPath", "FullGrown"), ("plant/immatureGraphicPath", "Immature")):
        stem = barley.findtext(element)
        assert stem == "Things/Plants/" + folder + "/AMJC_Barley", "Unexpected Base barley graphic"
        assert any((ROOT / "Textures" / stem).glob("*.png")), "Missing AMJ barley graphic family"
    wheat = get("AMJC_Plant_Wheat")
    for element, folder in (("graphicData/texPath", "FullGrown"), ("plant/immatureGraphicPath", "Immature")):
        stem = wheat.findtext(element)
        assert stem == "Things/Plants/" + folder + "/AMJC_Wheat"
        assert any((ROOT / "Textures" / stem).glob("*.png"))
    for name, folder, stem in (("AMJC_RawMillet", "AMJC_Millet", "MilletSheaf"),
                               ("AMJC_RawBuckwheat", "AMJC_Buckwheat", "BuckwheatSheaf"),
                               ("AMJC_RawBarley", "AMJC_Barley", "BarleySheaf"),
                               ("AMJC_RawWheat", "AMJC_Wheat", "WheatSheaf")):
        path = "Things/Item/Resource/" + folder + "/" + stem
        assert get(name).findtext("graphicData/texPath") == path
        stack_textures(ROOT, path)
    table = get("AMJC_GrainProcessingTable")
    assert table.findtext("costList/Steel") == "30" and table.find("researchPrerequisites") is None
    assert [n.text for n in get("AMJC_GrainProcessingSpot").findall("stuffCategories/li")] == ["Woody"]
    for node in base.findall("RecipeDef"):
        assert node.find("products/DankPyon_Straw") is None
        if (node.findtext("defName") or "").startswith("AMJC_Thresh"):
            assert len(node.findall("products/*")) == 1
    assert any(n.findtext("defName") == "AMJC_ThreshWheat" for n in base)
    print("Grains Base references and historical contracts with allowlisted graphics and MO research/material overrides: PASS")
    print("Known MO texture paths absent from Base: PASS; inherited/full game references and runtime texture resolution are not validated; see Docs/GrainsDependencyAudit.md")


if __name__ == "__main__":
    validate()
