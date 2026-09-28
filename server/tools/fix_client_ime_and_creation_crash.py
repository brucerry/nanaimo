"""Hide IME candidate lists and harden the character-creation error popup."""
from __future__ import annotations

import argparse
import hashlib
from pathlib import Path

import pefile


PATCHES = (
    # Keep IME composition and Chinese input enabled, but bypass both candidate
    # list display branches used by the old DirectX UI.
    (0x00A99BC6, bytes.fromhex("0f8e4b010000"), bytes.fromhex("e94c01000090")),
    (0x00A99D64, bytes.fromhex("0f8e63010000"), bytes.fromhex("e96401000090")),
    # Migrate the initial global list guard back to the retail implementation.
    # The stale value can also be a small positive integer, so the correct fix
    # belongs to the specific popup destructor below.
    (
        0x00AB35D3,
        (bytes.fromhex("8945fc85c07e4c8bc8eb1c9090"),),
        bytes.fromhex("cc" * 13),
    ),
    (
        0x00AB35F4,
        (
            bytes.fromhex("e9daffffff90"),
            bytes.fromhex("85c07e2e8bc8"),  # migrate the initial narrow guard
        ),
        bytes.fromhex("8945fc8b4dfc"),
    ),
    # The character-creation error-popup destructor selected row zero just
    # before destroying an already-empty child list. Skip that obsolete call.
    (0x007ED166, bytes.fromhex("85c0"), bytes.fromhex("eb13")),
    # Result 10 used to construct a legacy login-ending dialog. Go straight
    # to the retail create-in-progress reset so another name can be submitted
    # immediately without closing the game or the login connection.
    (0x004E35C6, bytes.fromhex("68f0010000"), bytes.fromhex("e992000000")),
    # Restore the retail create-in-progress reset after either name error.
    # Removing these calls leaves manager+0x378 set and permanently disables
    # subsequent Create clicks until the client is restarted.
    (0x004E3663, bytes.fromhex("9090909090"), bytes.fromhex("e87c71f3ff")),
    (0x004E3789, bytes.fromhex("9090909090"), bytes.fromhex("e85670f3ff")),
)


def offset_for_va(pe: pefile.PE, va: int) -> int:
    return pe.get_offset_from_rva(va - pe.OPTIONAL_HEADER.ImageBase)


def patch_client(data: bytes) -> tuple[bytes, bool]:
    pe = pefile.PE(data=data)
    result = bytearray(data)
    changed = False
    for entry in PATCHES:
        va, originals, patched = entry
        if isinstance(originals, bytes):
            originals = (originals,)
        offset = offset_for_va(pe, va)
        actual = bytes(result[offset:offset + len(patched)])
        if actual == patched:
            continue
        if actual not in originals:
            raise ValueError(
                f"Unrecognized bytes at VA 0x{va:08X}: {actual.hex()}"
            )
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
        backup = args.client.with_name(args.client.name + ".before-ime-popup-fix.bak")
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

    print("IME candidate lists:", "HIDDEN" if not changed else "VISIBLE")
    print("Creation error popup:", "HARDENED" if not changed else "UNPATCHED")
    print("SHA256:", hashlib.sha256(original).hexdigest())


if __name__ == "__main__":
    main()
