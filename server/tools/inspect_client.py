import argparse
import hashlib
import json
from pathlib import Path

import capstone
import pefile


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("client", type=Path)
    parser.add_argument("--address", type=lambda s: int(s, 0), action="append", default=[])
    parser.add_argument("--size", type=lambda s: int(s, 0), default=128)
    args = parser.parse_args()
    pe = pefile.PE(str(args.client))
    print(json.dumps({"path": str(args.client), "sha256": hashlib.sha256(args.client.read_bytes()).hexdigest(),
                      "entry": hex(pe.OPTIONAL_HEADER.ImageBase + pe.OPTIONAL_HEADER.AddressOfEntryPoint),
                      "image_size": hex(pe.OPTIONAL_HEADER.SizeOfImage),
                      "sections": [{"name": s.Name.decode("ascii", "replace").rstrip("\0"),
                                    "rva": hex(s.VirtualAddress), "raw_size": s.SizeOfRawData,
                                    "virtual_size": s.Misc_VirtualSize, "entropy": s.get_entropy()}
                                   for s in pe.sections]}, indent=2))
    dis = capstone.Cs(capstone.CS_ARCH_X86, capstone.CS_MODE_32)
    for address in args.address:
        print("ADDRESS", hex(address))
        data = pe.get_data(address - pe.OPTIONAL_HEADER.ImageBase, args.size)
        for ins in dis.disasm(data, address):
            print(f"{ins.address:08X} {ins.bytes.hex():24} {ins.mnemonic:8} {ins.op_str}")


if __name__ == "__main__":
    main()
