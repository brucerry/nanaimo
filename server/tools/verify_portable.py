"""Verify clone-and-run inputs, neutral defaults, and published file integrity."""
import argparse
import hashlib
import json
import struct
from pathlib import Path


def verify(root):
    client = root / 'client'
    required = ['game.exe', 'Start-Game.exe', 'ddraw.dll', 'nanaimo-renderer.dll', 'D3DImm.dll',
                'dgVoodoo-NOTICE.txt', 'images/interface.pack',
                'server/launchsettings.json', 'server/config/profile.ini',
                'server/launcher/Nanaimo.Launcher.exe',
                'server/server-merged/bin/Nanaimo.Server.exe',
                'server/server-merged/bin/nanaimo_gameplay_bridge.exe']
    for name in required:
        if not (client / name).is_file():
            raise ValueError('Missing portable input: ' + name)
    for name in ['ddraw.dll', 'nanaimo-renderer.dll', 'D3DImm.dll']:
        data = (client / name).read_bytes()
        offset = struct.unpack_from('<I', data, 60)[0]
        if struct.unpack_from('<H', data, offset + 4)[0] != 0x014c:
            raise ValueError('The original game requires x86 display libraries: ' + name)
    profile = (client / 'server/config/profile.ini').read_text(encoding='utf-8-sig')
    values = dict(line.split('=', 1) for line in profile.splitlines() if '=' in line and not line.startswith('#'))
    if values['name_hex'] or values['coin'] != '0' or values['nana_point'] != '0' or values['skip_tutorial'] != '0':
        raise ValueError('Non-neutral compatibility profile')
    for forbidden in ['server/server-merged/data', 'server/save-backups', 'server/server-merged/save-backups',
                      'server/launcher/local-account.json', 'server/launcher/display-settings.json',
                      'StateOption/gamestartoption.ini', 'dxwrapper.ini', 'display.ini', 'dgVoodoo.conf', 'server/launcher.log']:
        if (client / forbidden).exists():
            raise ValueError('Private runtime state present: ' + forbidden)
    for name in ['server/launcher/Nanaimo.Launcher.exe', 'server/server-merged/bin/Nanaimo.Server.exe']:
        data = (client / name).read_bytes()
        offset = struct.unpack_from('<I', data, 60)[0]
        if struct.unpack_from('<H', data, offset + 4)[0] != 0x8664:
            raise ValueError('Managed application is not x64: ' + name)
    manifest = json.loads((client / 'server/server-merged/build-manifest.json').read_text(encoding='utf-8-sig'))
    output = client / 'server/server-merged/bin'
    if list(output.glob('*.dat')) or list(output.glob('*.handles')):
        raise ValueError('Native runtime state present in the publish directory')
    for entry in manifest['files']:
        data = (output / entry['path']).read_bytes()
        if len(data) != entry['size'] or hashlib.sha256(data).hexdigest().lower() != entry['sha256'].lower():
            raise ValueError('Published file differs from manifest: ' + entry['path'])
    print('PORTABLE_PACKAGE_PASS')


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--root', type=Path, default=Path(__file__).resolve().parents[2])
    verify(parser.parse_args().root.resolve())
