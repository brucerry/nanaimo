"""Apply reviewed, fixed-length Traditional Chinese client text patches.

The manifest pins both binary versions. No instruction bytes, addresses, resource
sizes, format placeholders, Korean text, or image assets are rewritten.
"""
from __future__ import annotations

import argparse
import hashlib
import json
import re
from pathlib import Path


def digest(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest()


def patch_binary(data: bytes, record: dict) -> bytes:
    if digest(data) in [record['localized_sha256'], *record.get('compatible_sha256', [])]:
        return data
    if digest(data) != record['original_sha256']:
        raise ValueError('Unrecognized binary: ' + record['path'])
    result = bytearray(data)
    for patch in record['patches']:
        before = bytes.fromhex(patch['before_hex'])
        after = bytes.fromhex(patch['after_hex'])
        offset = patch['offset']
        if len(before) != len(after) or data[offset:offset + len(before)] != before:
            raise ValueError('Patch length or original bytes do not match')
        original_text = before.decode(patch['encoding'], errors='strict')
        localized_text = after.decode(patch['encoding'], errors='strict')
        if re.sub(r'[^\x00-\x7f]', '', original_text) != re.sub(r'[^\x00-\x7f]', '', localized_text):
            raise ValueError('Patch changed an ASCII component or format placeholder')
        result[offset:offset + len(after)] = after
    if digest(result) != record['localized_sha256']:
        raise ValueError('Localized binary hash does not match')
    return bytes(result)


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--root', type=Path, default=Path(__file__).resolve().parents[2])
    parser.add_argument('--apply', action='store_true')
    args = parser.parse_args()
    root = args.root.resolve()
    manifest = json.loads(Path(__file__).with_name('client-text-patches.json').read_text(encoding='utf-8'))
    pending = []
    for record in manifest['files']:
        target = (root / record['path']).resolve()
        if not target.is_relative_to(root / 'client'):
            raise ValueError('Patch target is outside the client directory')
        original = target.read_bytes()
        localized = patch_binary(original, record)
        backup = root / '.work/client-text-backups' / record['path']
        if args.apply and original != localized and backup.exists() and backup.read_bytes() != original:
            raise ValueError('Existing backup differs: ' + str(backup))
        pending.append((target, original, localized, backup))
        print(f"{record['path']}: {'pending' if original != localized else 'localized'} ({len(record['patches'])} strings)")
    if args.apply:
        for target, original, localized, backup in pending:
            if original == localized:
                continue
            backup.parent.mkdir(parents=True, exist_ok=True)
            if not backup.exists():
                with backup.open('xb') as stream:
                    stream.write(original)
            with target.open('r+b') as stream:
                if stream.read() != original:
                    raise ValueError('Client changed during localization')
                stream.seek(0)
                stream.write(localized)


if __name__ == '__main__':
    main()
