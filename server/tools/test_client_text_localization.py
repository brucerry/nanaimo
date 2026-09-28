"""Verify binary text patch safety without launching the legacy executables."""
import hashlib
import unittest

from localize_client_text import patch_binary


class ClientTextPatchTests(unittest.TestCase):
    def fixture(self):
        before = '\u9519\u8bef: %s %d'.encode('gbk')
        after = '\u932f\u8aa4: %s %d'.encode('gbk')
        original = b'HEADER\0' + before + b'\0CODE'
        localized = b'HEADER\0' + after + b'\0CODE'
        record = {'path': 'client/fixture.exe',
                  'original_sha256': hashlib.sha256(original).hexdigest(),
                  'localized_sha256': hashlib.sha256(localized).hexdigest(),
                  'patches': [{'offset': 7, 'encoding': 'gbk',
                               'before_hex': before.hex(), 'after_hex': after.hex()}]}
        return original, localized, record

    def test_preserves_surrounding_bytes_and_is_idempotent(self):
        original, localized, record = self.fixture()
        self.assertEqual(patch_binary(original, record), localized)
        self.assertEqual(len(original), len(localized))
        self.assertEqual(patch_binary(localized, record), localized)

    def test_rejects_unrecognized_binary(self):
        original, _, record = self.fixture()
        with self.assertRaisesRegex(ValueError, 'Unrecognized binary'):
            patch_binary(original + b'x', record)

    def test_rejects_format_placeholder_change(self):
        original, _, record = self.fixture()
        patch = record['patches'][0]
        patch['after_hex'] = bytes.fromhex(patch['after_hex']).replace(b'%s', b'%d').hex()
        with self.assertRaisesRegex(ValueError, 'ASCII component'):
            patch_binary(original, record)

    def test_rejects_growth(self):
        original, _, record = self.fixture()
        record['patches'][0]['after_hex'] += '20'
        with self.assertRaisesRegex(ValueError, 'Patch length'):
            patch_binary(original, record)


if __name__ == '__main__':
    unittest.main()
