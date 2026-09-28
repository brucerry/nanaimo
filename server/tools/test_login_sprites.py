"""Verify lossless IM3 round trips and installed archive boundaries."""
import unittest
from pathlib import Path
from im3_codec import decode,encode,archive_entries
ROOT=Path(__file__).resolve().parents[2]

class LoginSpritesTests(unittest.TestCase):
    def test_pixels_survive_codec_round_trip(self):
        for path in (ROOT/'localization/templates').glob('tutorial.*.im3'):
            original=path.read_bytes();image=decode(original);encoded=encode(image,original)
            self.assertEqual(image.tobytes(),decode(encoded).tobytes())
            self.assertEqual(original[12:44],encoded[12:44])

    def test_archive_index_is_contiguous_and_unique(self):
        entries=archive_entries((ROOT/'client/images/interface.pack').read_bytes())
        self.assertEqual(len(entries),len({rid for rid,_ in entries}))
        self.assertEqual(len(entries),1202)

    def test_rejects_resized_sprite(self):
        original=next((ROOT/'localization/templates').glob('*.im3')).read_bytes()
        with self.assertRaisesRegex(ValueError,'dimensions'):
            encode(decode(original).resize((1,1)),original)

if __name__=='__main__':unittest.main()
