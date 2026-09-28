"""Check that the startup change is bounded and refuses unknown executables."""
import json
import unittest
from pathlib import Path

from remove_client_cover import patch

ROOT = Path(__file__).resolve().parents[2]


class StartupPatchTests(unittest.TestCase):
    def test_installed_patch_changes_only_manifest_ranges(self):
        manifest = json.loads(Path(__file__).with_name('client-startup-patches.json').read_text())
        original = bytearray((ROOT / manifest['path']).read_bytes())
        for entry in manifest['patches']:
            before = bytes.fromhex(entry['before_hex'])
            original[entry['offset']:entry['offset']+len(before)] = before
        original = bytes(original)
        result = patch(original, manifest)
        self.assertEqual(result, (ROOT / manifest['path']).read_bytes())
        self.assertEqual(len(original), len(result))
        allowed = {offset for entry in manifest['patches']
                   for offset in range(entry['offset'], entry['offset'] + len(bytes.fromhex(entry['before_hex'])))}
        actual = {i for i, (a,b) in enumerate(zip(original,result)) if a != b}
        self.assertTrue(actual)
        self.assertLessEqual(actual, allowed)
        self.assertEqual(patch(result,manifest),result)

    def test_unknown_binary_is_rejected(self):
        manifest = json.loads(Path(__file__).with_name('client-startup-patches.json').read_text())
        with self.assertRaisesRegex(ValueError, 'Unrecognized'):
            patch(b'unknown executable',manifest)

    def test_cover_assets_and_resource_references_are_removed(self):
        manifest = json.loads(Path(__file__).with_name('client-startup-patches.json').read_text())
        installed = (ROOT / manifest['path']).read_bytes()
        self.assertNotIn(b'corp_logo', installed)
        for relative in manifest['remove_assets']:
            self.assertFalse((ROOT / relative).exists(),relative)
        shim = next(item for item in manifest['patches'] if item['va']=='0x41fc00')
        # Return address + eight preceding DWORD arguments = charset at 36.
        self.assertEqual(bytes.fromhex(shim['after_hex'])[:8],bytes.fromhex('c744242486000000'))


if __name__ == '__main__':
    unittest.main()
