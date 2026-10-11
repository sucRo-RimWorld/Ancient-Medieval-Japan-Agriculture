#!/usr/bin/env python3
"""Lock exact current crop/sheaf source bytes and production PNG paths, without game-runtime claims."""
import hashlib
import json
import struct
from pathlib import Path
from stack_graphics import stack_textures

ROOT = Path(__file__).resolve().parents[1]
M = ROOT / "Docs/References/GrainsCropSourceManifest.json"

def git_blob_sha1(data):
    return hashlib.sha1(b"blob " + str(len(data)).encode("ascii") + b"\x00" + data).hexdigest()

def main():
    data = json.loads(M.read_text(encoding="utf-8"))
    assets = data["assets"]
    assert len(assets) == 19, len(assets)
    assert len({r["source"] for r in assets}) == 19
    assert len([r for r in assets if r["state"] == "mature"]) == 7
    assert len([r for r in assets if r["state"] == "immature"]) == 7
    assert len([r for r in assets if r["state"] == "sheaf"]) == 5
    assert not list((ROOT / "Textures/Things/Plants").glob("*/*_Simple")), "obsolete _Simple production crop directories remain"
    # Each live crop stage owns one current high-resolution source at its canonical crop path.
    assert not list((ROOT / "Art/Sources/Things/Plants").glob("*/*_Simple")), "obsolete _Simple crop source directories remain"
    for row in assets:
        if row["state"] not in {"immature", "mature"}:
            continue
        crop, state = row["crop"], row["state"]
        stage = "Immature" if state == "immature" else "FullGrown"
        suffix = "Immature" if state == "immature" else "Mature"
        expected = f"Art/Sources/Things/Plants/{stage}/AMJC_{crop}/AMJC_{crop}_{suffix}.png"
        assert row["source"] == expected, f"noncanonical current source: {crop} {state}"

    for row in assets:
        source = ROOT / row["source"]
        candidate = ROOT / row["candidate"]
        production = ROOT / row["production"]
        assert source.is_file(), f"missing archived source: {source}"
        assert candidate.is_file(), f"missing exact-byte candidate: {candidate}"
        bytes_ = source.read_bytes()
        assert bytes_ == candidate.read_bytes(), f"archive drift: {source}"
        assert git_blob_sha1(bytes_) == row["git_blob_sha1"], f"blob SHA drift: {source}"
        assert bytes_[:8] == b"\x89PNG\r\n\x1a\n", f"bad source PNG: {source}"
        assert production.is_file(), f"missing production PNG: {production}"
        header = production.read_bytes()[:24]
        assert header[:8] == b"\x89PNG\r\n\x1a\n" and header[12:16] == b"IHDR", production
        assert struct.unpack(">II", header[16:24]) == (256, 256), production
        if row["state"] == "sheaf":
            assert production.name.endswith("_a.png"), production
            stack_textures(ROOT, production.parent.relative_to(ROOT / "Textures"))
    # AMJ-019: preserve accepted source and _a bytes; extra stack images are optional.
    hulled_source = ROOT / "Art/Sources/Things/Item/Resource/AMJC_Millet/MilletInHull/MilletInHull.png"
    source_bytes = hulled_source.read_bytes()
    assert git_blob_sha1(source_bytes) == "61d6720c7354107424400dd9abf0edc0a118b859", "hulled millet master drift"
    assert source_bytes[:8] == b"\x89PNG\r\n\x1a\n"
    assert struct.unpack(">II", source_bytes[16:24]) == (1429, 1100)
    base = ROOT / "Textures/Things/Item/Resource/AMJC_Millet/MilletInHull"
    millet_png = stack_textures(ROOT, base.relative_to(ROOT / "Textures"))[0].read_bytes()
    assert git_blob_sha1(millet_png) == "87346c67d58671d5d22150bd376be2ac7534775e", "hulled millet production drift"
    xml = (ROOT / "Defs/ThingDefs_Items/Items_StageA_Grains.xml").read_text(encoding="utf-8")
    assert xml.count("<defName>AMJC_MilletInHull</defName>") == 1
    graphic = xml.split("<defName>AMJC_MilletInHull</defName>", 1)[1].split("</ThingDef>", 1)[0]
    assert "<texPath>Things/Item/Resource/AMJC_Millet/MilletInHull</texPath>" in graphic
    assert "<graphicClass>Graphic_StackCount</graphicClass>" in graphic
    print("PASS: exact accepted millet in-hull master, _a PNG and loaded Def path")

    # AMJ-011: exact author-accepted hulled-barley source and isolated StackCount art.
    barley_source = ROOT / "Art/Sources/Things/Item/Resource/AMJC_Barley/BarleyInHull/BarleyInHull.png"
    barley_source_bytes = barley_source.read_bytes()
    assert git_blob_sha1(barley_source_bytes) == "2cb0f5eb972e3b0f574ae61c7b4150336c5adbff", "hulled barley source drift"
    assert barley_source_bytes[:8] == b"\x89PNG\r\n\x1a\n"
    assert struct.unpack(">II", barley_source_bytes[16:24]) == (1429, 1100)
    barley_base = ROOT / "Textures/Things/Item/Resource/AMJC_Barley/BarleyInHull"
    stack_textures(ROOT, barley_base.relative_to(ROOT / "Textures"))
    assert xml.count("<defName>AMJC_BarleyInHull</defName>") == 1
    barley_xml = xml.split("<defName>AMJC_BarleyInHull</defName>", 1)[1].split("</ThingDef>", 1)[0]
    assert "<texPath>Things/Item/Resource/AMJC_Barley/BarleyInHull</texPath>" in barley_xml
    assert "<graphicClass>Graphic_StackCount</graphicClass>" in barley_xml
    print("PASS: preserved hulled barley source and dedicated graphic path")

    print("PASS: 19 exact original-source blob identities, 19 production texture mappings, five flexible stack families")

if __name__ == "__main__":
    main()
