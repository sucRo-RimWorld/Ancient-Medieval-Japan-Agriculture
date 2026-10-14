#!/usr/bin/env python3
"""Prepare Grains Workshop candidate from an explicit file allowlist, not YADA.

This is a nonpublishing GitHub Actions CI preflight only. The protected
SteamCMD+Steamworks submission workflow remains a separate release gate.
"""
import argparse
import hashlib
import json
from pathlib import Path, PurePosixPath
import shutil
import subprocess
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[2]
VALID_RUNTIME = {
    "About": {".xml", ".png", ".jpg", ".jpeg", ".txt"},
    "Assemblies": {".dll"},
    "Defs": {".xml"},
    "Patches": {".xml"},
    "Languages": {".xml"},
    "Textures": {".png", ".jpg", ".jpeg", ".dds"},
    "Sounds": {".ogg", ".wav"},
}
ROOT_LICENSE = {"LICENSE", "LICENSE.txt", "NOTICE", "NOTICE.txt", "COPYING", "COPYING.txt"}
REQUIRED = {
    "About/About.xml", "About/Manifest.xml", "About/Changelog.txt",
    "loadFolders.xml", "LICENSE"
}


def permitted(path: str) -> bool:
    p = PurePosixPath(path)
    if path == "loadFolders.xml" or path in ROOT_LICENSE:
        return True
    segments = p.parts
    if segments[:2] == ("Compatibility", "MedievalOverhaul"):
        segments = segments[2:]
    elif segments[:1] == ("BaseWithoutMO",):
        segments = segments[1:]
    if len(segments) < 2:
        return False
    return segments[0] in VALID_RUNTIME and p.suffix.lower() in VALID_RUNTIME[segments[0]]


def stage(output: Path) -> dict:
    output = output.resolve()
    parent = (ROOT / "TestResults").resolve()
    if parent not in output.parents or output == parent:
        raise ValueError("Staging is restricted below the untracked TestResults directory")
    tracked = subprocess.check_output(["git", "ls-files", "-z"], cwd=ROOT).decode("utf-8").strip("\0").split("\0")
    allowed = sorted(path for path in tracked if permitted(path))
    if not REQUIRED.issubset(allowed):
        raise ValueError("Missing mandatory subscriber files: " + repr(sorted(REQUIRED - set(allowed))))
    if output.is_symlink():
        raise ValueError("Refusing symlinked candidate output")
    if output.exists():
        shutil.rmtree(output)
    output.mkdir(parents=True)
    checksums = {}
    for path in allowed:
        source = ROOT / path
        if not source.is_file() or source.is_symlink():
            raise ValueError("Tracked runtime file missing/not regular: " + path)
        if source.suffix.lower() == ".xml":
            ET.parse(source)
        target = output / path
        target.parent.mkdir(parents=True, exist_ok=True)
        shutil.copyfile(source, target)
        source_hash = hashlib.sha256(source.read_bytes()).hexdigest()
        if hashlib.sha256(target.read_bytes()).hexdigest() != source_hash:
            raise ValueError("Source/candidate hash mismatch: " + path)
        checksums[path] = source_hash
    actual = sorted(x.relative_to(output).as_posix() for x in output.rglob("*") if x.is_file())
    if actual != allowed:
        raise ValueError("Candidate file list differs from explicit allowlist")
    return {"mode": "preflight-only; no Steam upload", "count": len(allowed), "sha256": checksums}


def main():
    p = argparse.ArgumentParser(description=__doc__)
    p.add_argument("--output", default="TestResults/WorkshopCandidate")
    args = p.parse_args()
    print(json.dumps(stage(Path(args.output)), sort_keys=True, indent=2))


if __name__ == "__main__":
    main()
