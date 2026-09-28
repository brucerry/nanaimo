"""Check the reviewed logo assets and native game title without a font dependency."""
import argparse
import hashlib
import json
from pathlib import Path


def verify(root):
    brand = json.loads((root / 'localization/branding.json').read_text(encoding='utf-8'))
    bindings = json.loads((root / 'localization/sprite-bindings.json').read_text(encoding='utf-8'))
    if not brand.get('logos_removed'):
        raise ValueError('Game logos must remain removed')
    if any(region['key'] == brand['key'] for binding in bindings for region in binding['regions']):
        raise ValueError('A removed wordmark is still bound to an image')
    for asset in brand['assets']:
        path = root / asset['path']
        if hashlib.sha256(path.read_bytes()).hexdigest() != asset['sha256']:
            raise ValueError('Logo asset differs from the reviewed locale build: ' + asset['path'])
    client = (root / 'client/game.exe').read_bytes()
    title = brand['name'].encode('utf-16le')
    if client.count(title) < 3 or 'QQ\u98db\u884c\u5cf6'.encode('utf-16le') in client:
        raise ValueError('Native game title is stale')
    manifest = json.loads((root / 'server/tools/client-startup-patches.json').read_text())
    if hashlib.sha256(client).hexdigest() != manifest['patched_sha256']:
        raise ValueError('Client executable differs from the reviewed patch')
    print(f"BRANDING_PASS {len(brand['assets'])} logo-free backgrounds and native title")


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--root', type=Path, default=Path(__file__).resolve().parents[2])
    verify(parser.parse_args().root.resolve())
