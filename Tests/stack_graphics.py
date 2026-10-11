"""Check installed Graphic_StackCount image families without requiring redundant PNGs."""
from pathlib import Path
import struct


def stack_textures(root, tex_path):
    """Require a valid _a PNG; accept optional contiguous _b/_c PNGs.

    RimWorld handles 1, 2, or 3 Graphic_StackCount subgraphics. Do not
    require identical placeholder copies or forbid distinct count artwork.
    """
    folder = Path(root) / "Textures" / tex_path
    stem = Path(tex_path).name
    paths = [folder / f"{stem}_{suffix}.png" for suffix in "abc"]
    assert paths[0].is_file(), f"missing _a stack texture: {paths[0]}"
    assert paths[1].is_file() or not paths[2].exists(), (
        f"noncontiguous stack textures: {paths[2]} requires {paths[1]}"
    )
    available = [path for path in paths if path.is_file()]
    permitted = {path.name for path in paths}
    unexpected = sorted(path.name for path in folder.glob(f"{stem}_*.png")
                        if path.name not in permitted)
    assert not unexpected, f"unexpected stack texture variants: {unexpected}"
    for path in available:
        header = path.read_bytes()[:24]
        assert (len(header) == 24 and header[:8] == b"\x89PNG\r\n\x1a\n"
                and header[12:16] == b"IHDR"), f"invalid PNG header: {path}"
        assert struct.unpack(">II", header[16:24]) == (256, 256), (
            f"wrong stack texture size: {path}"
        )
    return available
