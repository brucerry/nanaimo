"""Remove publisher-cover code/assets and pin the ANSI dialogue font charset."""
import argparse
import hashlib
import json
from pathlib import Path


def patch(data, manifest):
    digest = hashlib.sha256(data).hexdigest()
    if digest == manifest['patched_sha256']:
        return data
    if digest != manifest['original_sha256']:
        raise ValueError('Unrecognized client version; refusing startup patches')
    result = bytearray(data)
    for item in manifest['patches']:
        before, after = bytes.fromhex(item['before_hex']), bytes.fromhex(item['after_hex'])
        offset = item['offset']
        if len(before) != len(after) or data[offset:offset + len(before)] != before:
            raise ValueError('Startup instruction precondition failed')
        result[offset:offset + len(after)] = after
    if hashlib.sha256(result).hexdigest() != manifest['patched_sha256']:
        raise ValueError('Startup patch verification failed')
    return bytes(result)


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--apply', action='store_true')
    args = parser.parse_args()
    root = Path(__file__).resolve().parents[2]
    manifest = json.loads(Path(__file__).with_name('client-startup-patches.json').read_text())
    path = root / manifest['path']
    original = path.read_bytes()
    baseline = original
    if hashlib.sha256(original).hexdigest() in [manifest.get('previous_sha256'), *manifest.get('previous_versions',[])]:
        baseline = (root / '.work/startup-cover-backup.exe').read_bytes()
    result = patch(baseline, manifest)
    if args.apply and result != original:
        backup = root / '.work/startup-cover-backup.exe'
        if backup.exists() and backup.read_bytes() != baseline:
            raise ValueError('Existing startup backup differs')
        if not backup.exists():
            backup.parent.mkdir(parents=True, exist_ok=True)
            with backup.open('xb') as stream:
                stream.write(baseline)
        with path.open('r+b') as stream:
            if stream.read() != original:
                raise ValueError('Client changed during patching')
            stream.seek(0)
            stream.write(result)
    if args.apply:
        for relative in manifest['remove_assets']:
            asset = (root / relative).resolve()
            asset.relative_to(root / 'client')
            if asset.is_file():
                backup = root / '.work/removed-cover-assets' / relative
                backup.parent.mkdir(parents=True, exist_ok=True)
                if backup.exists() and backup.read_bytes() != asset.read_bytes():
                    raise ValueError('Cover asset backup differs: ' + relative)
                if not backup.exists():
                    backup.write_bytes(asset.read_bytes())
                asset.unlink()
    complete = path.read_bytes() == result and all(not (root / name).exists() for name in manifest['remove_assets'])
    print('STARTUP_COVER_REMOVED' if complete else 'COVER_REMOVAL_PENDING')
