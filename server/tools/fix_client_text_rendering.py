"""Repair DirectDraw text rendering on drivers that reject GetDC while locked."""
from __future__ import annotations

import argparse
import hashlib
from pathlib import Path

import pefile


PATCHES = (
    # Both text paths push five Lock arguments. Drop the invalid Lock call and
    # its arguments before calling GetDC on the surface.
    (0x00ACB7CF, bytes.fromhex("8b4a64ffd1"), bytes.fromhex("83c4149090")),
    (0x00ACBC3E, bytes.fromhex("8b5064ffd2"), bytes.fromhex("83c4149090")),
    # Unlock is no longer needed. Release the two already-pushed arguments.
    (0x00ACBA0B, bytes.fromhex("8b8180000000ffd0"), bytes.fromhex("83c4089090909090")),
    (0x00ACBE7A, bytes.fromhex("8b8a80000000ffd1"), bytes.fromhex("83c4089090909090")),
)


def offset_for_va(pe: pefile.PE, va: int) -> int:
    return pe.get_offset_from_rva(va - pe.OPTIONAL_HEADER.ImageBase)


def patch_client(data: bytes) -> tuple[bytes, bool]:
    pe = pefile.PE(data=data)
    result = bytearray(data)
    changed = False
    for va, original, patched in PATCHES:
        offset = offset_for_va(pe, va)
        actual = bytes(result[offset:offset + len(patched)])
        if actual == patched:
            continue
        if actual != original:
            raise ValueError(f"Unrecognized bytes at VA 0x{va:08X}: {actual.hex()}")
        result[offset:offset + len(patched)] = patched
        changed = True
    return bytes(result), changed


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("client", type=Path)
    parser.add_argument("--apply", action="store_true")
    args = parser.parse_args()

    original = args.client.read_bytes()
    repaired, changed = patch_client(original)
    if args.apply and changed:
        backup = args.client.with_name(args.client.name + ".before-text-rendering-fix.bak")
        if not backup.exists():
            with backup.open("xb") as output:
                output.write(original)
        with args.client.open("r+b") as output:
            if output.read() != original:
                raise ValueError("Client changed during repair")
            output.seek(0)
            output.write(repaired)
        original = repaired
        changed = False
        print("Backup:", backup)

    print("DirectDraw text rendering:", "COMPATIBLE" if not changed else "LEGACY")
    print("SHA256:", hashlib.sha256(original).hexdigest())


if __name__ == "__main__":
    main()
