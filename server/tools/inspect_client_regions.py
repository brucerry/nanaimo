from __future__ import annotations

import argparse
import hashlib
from pathlib import Path

import pefile
from capstone import CS_ARCH_X86, CS_MODE_32, Cs


DEFAULT_ADDRESSES = (
    0x00401096,
    0x007ED120,
    0x00A99B50,
    0x00A99BC6,
    0x00A99D64,
    0x00AB35D0,
    0x00AB35FA,
)


def file_offset(pe: pefile.PE, va: int) -> int:
    return pe.get_offset_from_rva(va - pe.OPTIONAL_HEADER.ImageBase)


def inspect(path: Path, addresses: tuple[int, ...]) -> None:
    data = path.read_bytes()
    pe = pefile.PE(data=data)
    print(f"FILE {path}")
    print(f"SIZE {len(data)} SHA256 {hashlib.sha256(data).hexdigest()}")
    for va in addresses:
        offset = file_offset(pe, va)
        print(f"{va:08X} file={offset:08X} bytes={data[offset:offset + 20].hex()}")

    disassembler = Cs(CS_ARCH_X86, CS_MODE_32)
    for start, size in (
        (0x00401090, 0x20),
        (0x007ED120, 0x90),
        (0x00AB35D0, 0x90),
        (0x00ACB780, 0x2B0),
        (0x00ACBBF0, 0x2B0),
    ):
        offset = file_offset(pe, start)
        print(f"DISASM {start:08X}")
        for instruction in disassembler.disasm(data[offset:offset + size], start):
            print(f"{instruction.address:08X} {instruction.mnemonic:8} {instruction.op_str}")


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("paths", nargs="+", type=Path)
    args = parser.parse_args()
    for path in args.paths:
        inspect(path.resolve(), DEFAULT_ADDRESSES)


if __name__ == "__main__":
    main()
