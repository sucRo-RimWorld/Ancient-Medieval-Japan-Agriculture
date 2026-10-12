# Tests agent instructions — AMJ Grains

Read the repository-root `AGENTS.md`, `main:Docs/Coordination.md` and the Project shared rules first. Tests must state the specification assumptions and actual coverage, not only PASS counts.

## Production PNG integrity

Every committed production PNG must pass `python Tests/validate_png_assets.py`. The validator checks every `Textures/**/*.png` for complete chunk boundaries, CRCs, a complete IDAT/zlib stream, valid scanlines and final IEND, reporting all detected broken PNGs in one run.

GitHub Actions `validate-png-assets` is a prerequisite for Stage A validation. For binary writes through automation or Git/GitHub APIs, treat committed/checked-out bytes as authoritative: do not report success until the repository-side PNG integrity gate passes. A valid signature, dimensions or viewer-open alone is insufficient.
