"""Regression checks for resource integrity and preflight failure handling."""
import json
import io
from contextlib import redirect_stdout
from pathlib import Path
import sys
import tempfile
import unittest
from unittest.mock import patch
from Crypto.Cipher import AES
from Crypto.Util.Padding import pad, unpad
from opencc import OpenCC
from localize_catalogs import KEY, convert_catalog, main

def encrypt(text):
    return AES.new(KEY, AES.MODE_CBC, iv=bytes(16)).encrypt(pad(text.encode('gbk'), 16))

def decrypt(data):
    return unpad(AES.new(KEY, AES.MODE_CBC, iv=bytes(16)).decrypt(data), 16).decode('gbk')

class CatalogLocalizationTests(unittest.TestCase):
    def test_preserves_ids_paths_and_empty_fields(self):
        original = encrypt('100#\u6d4b\u8bd5#images/pet.im3##1.25#')
        result, changed = convert_catalog(original, OpenCC('s2t'))
        self.assertEqual(changed, 1)
        self.assertEqual(decrypt(result), '100#\u6e2c\u8a66#images/pet.im3##1.25#')

    def test_already_traditional_names_and_jamming_are_stable(self):
        original = encrypt('\u74e6\u723e\u57fa\u91cc#\u5e72\u64fe\u5f48')
        self.assertEqual(convert_catalog(original, OpenCC('s2t')), (original, 0))

    def test_rejects_ascii_mutation_and_field_growth(self):
        class Converter:
            def __init__(self, result): self.result = result
            def convert(self, value): return self.result
        with self.assertRaisesRegex(ValueError, 'ASCII'):
            convert_catalog(encrypt('100'), Converter('200'))
        with self.assertRaisesRegex(ValueError, 'expanded'):
            convert_catalog(encrypt('\u6d4b'), Converter('\u6e2c\u8a66'))

    def write_locale(self, root, files):
        locale = root / 'localization'
        (locale / 'zh-HK').mkdir(parents=True)
        (locale / 'zh-HK/catalogs.json').write_text(json.dumps({'sample': '\u6e2c\u8a66'}))
        (locale / 'zh-HK/managed.json').write_text('{}')
        (locale / 'managed-bindings.json').write_text('[]')
        bindings = {name: {'field_count': 1, 'fields': {'0': {'key': 'sample', 'max_bytes': 4}}}
                    for name in files}
        (locale / 'catalog-bindings.json').write_text(json.dumps(bindings))

    def test_invalid_later_file_leaves_every_input_untouched(self):
        with tempfile.TemporaryDirectory(prefix='nanaimo-catalog-check-') as directory:
            root = Path(directory)
            (root / 'client').mkdir()
            first = root / 'client/a._D1'
            first.write_bytes(encrypt('\u6d4b\u8bd5'))
            second = root / 'client/z._D2'
            second.write_bytes(b'invalid ciphertext')
            before = first.read_bytes()
            self.write_locale(root, ['a._D1', 'z._D2'])
            args = ['build_game_locale.py', '--root', str(root), '--apply']
            with patch.object(sys, 'argv', args), self.assertRaises(ValueError):
                main()
            self.assertEqual(first.read_bytes(), before)
            self.assertEqual(second.read_bytes(), b'invalid ciphertext')

    def test_repeat_build_is_idempotent_without_backups(self):
        with tempfile.TemporaryDirectory(prefix='nanaimo-catalog-check-') as directory:
            root = Path(directory)
            (root / 'client').mkdir()
            catalog = root / 'client/a._D1'
            catalog.write_bytes(encrypt('\u6d4b\u8bd5'))
            self.write_locale(root, ['a._D1'])
            args = ['build_game_locale.py', '--root', str(root), '--apply']
            with patch.object(sys, 'argv', args), redirect_stdout(io.StringIO()):
                main()
                built = catalog.read_bytes()
                main()
            self.assertEqual(catalog.read_bytes(), built)
            self.assertEqual(decrypt(built), '\u6e2c\u8a66')
            self.assertFalse((root / '.work').exists())
