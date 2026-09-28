"""Verify the shared locale outputs without retaining historical asset copies."""
import json
import struct
import unittest
from pathlib import Path
from im3_codec import decode,encode,archive_entries
from build_game_locale import render,load,format_tokens,validate_text

ROOT=Path(__file__).resolve().parents[2]
LOC=ROOT/'localization'

class GameUiTests(unittest.TestCase):
    def test_installed_sprites_match_editable_text_and_geometry(self):
        texts=load(LOC/'zh-HK/ui.json')
        pack=dict(archive_entries((ROOT/'client/images/interface.pack').read_bytes()))
        for binding in load(LOC/'sprite-bindings.json'):
            template=(LOC/'templates'/binding['template']).read_bytes()
            expected=render(template,binding['regions'],texts)
            for target in binding['targets']:
                actual=pack[target['pack']] if 'pack' in target else (ROOT/target['path']).read_bytes()
                with self.subTest(target=target):
                    self.assertEqual(actual,expected)
                    self.assertEqual(actual[12:44],template[12:44])
                    frames=struct.unpack_from('<I',template,32)[0]
                    self.assertEqual(actual[44:44+20*frames],template[44:44+20*frames])

    def test_editing_one_key_changes_rendered_pixels(self):
        binding=next(b for b in load(LOC/'sprite-bindings.json') if b['template']=='tutorial.move.im3')
        template=(LOC/'templates'/binding['template']).read_bytes()
        texts=load(LOC/'zh-HK/ui.json');original=render(template,binding['regions'],texts)
        texts[binding['regions'][0]['key']]='WASD'
        changed=render(template,binding['regions'],texts)
        self.assertNotEqual(decode(original).tobytes(),decode(changed).tobytes())
        self.assertEqual(original[12:44],changed[12:44])

    def test_overflow_is_rejected(self):
        binding=next(b for b in load(LOC/'sprite-bindings.json') if b['regions'])
        texts=load(LOC/'zh-HK/ui.json');texts[binding['regions'][0]['key']]='W'*1000
        with self.assertRaisesRegex(ValueError,'does not fit'):
            render((LOC/'templates'/binding['template']).read_bytes(),binding['regions'],texts)

    def test_removed_gold_banner_stays_empty(self):
        for path in (ROOT/'client').rglob('guide_intro_logo.im3'):
            self.assertIsNone(decode(path.read_bytes()).getbbox())

if __name__=='__main__':unittest.main()
