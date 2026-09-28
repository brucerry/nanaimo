"""Check or repair the client's forced Sound OFF startup branch."""
import argparse
import hashlib
from pathlib import Path


OFFSET = 0x3A8C1
OLD = bytes.fromhex('eb30')  # jmp 0x43a8f3: disable sound (1)
NEW = bytes.fromhex('eb3f')  # jmp 0x43a902: disable sound (0)


def digest(data):
    return hashlib.sha256(data).hexdigest()


def inspect(data):
    branch = data[OFFSET:OFFSET + len(OLD)]
    if branch not in (OLD, NEW):
        raise ValueError('Unrecognized startup branch; refusing to patch')
    return branch == NEW


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('client', type=Path)
    parser.add_argument('--apply', action='store_true')
    args = parser.parse_args()
    data = args.client.read_bytes()
    fixed = inspect(data)
    if args.apply and not fixed:
        backup = args.client.with_name(args.client.name + '.before-sound-fix.bak')
        if backup.exists():
            if backup.read_bytes() != data:
                raise ValueError('Existing backup differs; refusing to overwrite it')
        else:
            with backup.open('xb') as output:
                output.write(data)
        repaired = bytearray(data)
        repaired[OFFSET:OFFSET + len(NEW)] = NEW
        assert inspect(repaired)
        with args.client.open('r+b') as output:
            if output.read() != data:
                raise ValueError('Client changed during repair')
            output.seek(OFFSET)
            output.write(NEW)
        data = args.client.read_bytes()
        fixed = inspect(data)
        print('Backup:', backup)
    print('Startup sound:', 'ON' if fixed else 'FORCED OFF')
    print('SHA256:', digest(data))


if __name__ == '__main__':
    main()
