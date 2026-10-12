"""Guard the valid 1/2/3-image Graphic_StackCount contract."""
import struct
import tempfile
import unittest
import zlib
from pathlib import Path
import sys
sys.path.insert(0, str(Path(__file__).resolve().parents[1]))
from stack_graphics import stack_textures


TEX_PATH = "Things/Item/Resource/Test/TestSheaf"


def png(pixel=0, size=256):
    def chunk(tag, data):
        return (struct.pack(">I", len(data)) + tag + data
                + struct.pack(">I", zlib.crc32(tag + data) & 0xffffffff))
    hdr = struct.pack(">IIBBBBB", size, size, 8, 6, 0, 0, 0)
    raw = (b"\x00" + bytes((pixel, 0, 0, 255)) * size) * size
    return (b"\x89PNG\r\n\x1a\n" + chunk(b"IHDR", hdr)
            + chunk(b"IDAT", zlib.compress(raw)) + chunk(b"IEND", b""))


class StackGraphicsTests(unittest.TestCase):
    def setUp(self):
        self.temp = tempfile.TemporaryDirectory()
        self.addCleanup(self.temp.cleanup)
        self.root = Path(self.temp.name)
        self.folder = self.root / "Textures" / TEX_PATH
        self.folder.mkdir(parents=True)

    def put(self, suffix, data=None):
        path = self.folder / f"TestSheaf_{suffix}.png"
        path.write_bytes(png() if data is None else data)
        return path

    def test_single_a_is_valid(self):
        self.put("a")
        self.assertEqual(len(stack_textures(self.root, TEX_PATH)), 1)

    def test_two_different_images_are_valid(self):
        self.put("a")
        self.put("b", png(1))
        self.assertEqual(len(stack_textures(self.root, TEX_PATH)), 2)

    def test_three_different_images_are_valid(self):
        self.put("a")
        self.put("b", png(1))
        self.put("c", png(2))
        self.assertEqual(len(stack_textures(self.root, TEX_PATH)), 3)

    def test_128_texture_is_valid(self):
        self.put("a", png(size=128))
        self.assertEqual(len(stack_textures(self.root, TEX_PATH)), 1)

    def test_existing_identical_copies_are_not_a_failure(self):
        for suffix in "abc":
            self.put(suffix)
        self.assertEqual(len(stack_textures(self.root, TEX_PATH)), 3)

    def test_missing_a_fails(self):
        self.put("b")
        with self.assertRaisesRegex(AssertionError, "missing _a"):
            stack_textures(self.root, TEX_PATH)

    def test_c_without_b_fails(self):
        self.put("a")
        self.put("c")
        with self.assertRaisesRegex(AssertionError, "noncontiguous"):
            stack_textures(self.root, TEX_PATH)

    def test_bad_png_and_size_fail(self):
        p = self.put("a", b"invalid")
        with self.assertRaisesRegex(AssertionError, "invalid PNG"):
            stack_textures(self.root, TEX_PATH)
        p.write_bytes(png(size=64))
        with self.assertRaisesRegex(AssertionError, "wrong stack texture size"):
            stack_textures(self.root, TEX_PATH)

    def test_unsupported_d_variant_fails(self):
        self.put("a")
        self.put("d")
        with self.assertRaisesRegex(AssertionError, "unexpected stack texture"):
            stack_textures(self.root, TEX_PATH)


if __name__ == "__main__":
    unittest.main()
