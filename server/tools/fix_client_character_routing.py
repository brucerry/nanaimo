"""Restore the retail post-login character/no-character router."""
import argparse
import hashlib
from pathlib import Path

IMAGE_BASE = 0x400000
ROUTER = 0x43A2A7
ROUTE_CALL = 0x4E2F1E
POST_LOGIN_CALL = 0x4E3A8E

ORIGINAL_ROUTER = bytes.fromhex(
    "D9 00 89 85 58 78 FE FF 83 BD 58 78 FE FF 07 74 "
    "04 EB 1B EB 19 68 EC BF C2 00 68 FC BF C2 00 8D "
    "95 78 1F FF FF 52 E8 D0 D2 70 00 83 C4 0C"
)

def call(source, target):
    return b"\xE8" + (target - (source + 5)).to_bytes(4, "little", signed=True)

def replacement_router():
    data = bytearray(b"\x51\x8B\x0D\xD4\x69\xD8\x00")
    data += call(0x43A2AE, 0x41E0D8)
    data += b"\x84\xC0\x59\x75\x08\x6A\x01"
    data += call(0x43A2BA, 0x403995) + b"\xC3"
    data += b"\xE9" + (0x4180F7 - 0x43A2C5).to_bytes(4, "little", signed=True)
    return bytes(data).ljust(len(ORIGINAL_ROUTER), b"\x90")

def patch(data):
    result = bytearray(data)
    result[ROUTER - IMAGE_BASE:ROUTER - IMAGE_BASE + len(ORIGINAL_ROUTER)] = replacement_router()
    result[ROUTE_CALL - IMAGE_BASE:ROUTE_CALL - IMAGE_BASE + 5] = call(ROUTE_CALL, 0x41E0D8)
    result[POST_LOGIN_CALL - IMAGE_BASE:POST_LOGIN_CALL - IMAGE_BASE + 5] = call(POST_LOGIN_CALL, ROUTER)
    return bytes(result)

def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("client", type=Path)
    parser.add_argument("--apply", action="store_true")
    args = parser.parse_args()
    data = args.client.read_bytes()
    current = data[ROUTER - IMAGE_BASE:ROUTER - IMAGE_BASE + len(ORIGINAL_ROUTER)]
    expected = replacement_router()
    if current not in (ORIGINAL_ROUTER, expected):
        raise ValueError("Unrecognized client router; refusing to patch")
    if data[ROUTE_CALL - IMAGE_BASE:ROUTE_CALL - IMAGE_BASE + 5] not in (call(ROUTE_CALL, 0x41055A), call(ROUTE_CALL, 0x41E0D8)):
        raise ValueError("Unrecognized character result call; refusing to patch")
    if data[POST_LOGIN_CALL - IMAGE_BASE:POST_LOGIN_CALL - IMAGE_BASE + 5] not in (call(POST_LOGIN_CALL, 0x4180F7), call(POST_LOGIN_CALL, ROUTER)):
        raise ValueError("Unrecognized post-login call; refusing to patch")
    repaired = patch(data)
    if not args.apply or data == repaired:
        print("Character routing:", "retail" if data == repaired else "legacy")
        print("SHA256:", hashlib.sha256(data).hexdigest())
        return
    backup = args.client.with_name(args.client.name + ".before-character-routing.bak")
    if backup.exists() and backup.read_bytes() != data:
        raise ValueError("Existing backup differs; refusing to overwrite it")
    if not backup.exists():
        with backup.open("xb") as output: output.write(data)
    with args.client.open("r+b") as output:
        if output.read() != data: raise ValueError("Client changed during repair")
        output.seek(0)
        output.write(repaired)
    print("Backup:", backup)
    print("Character routing: retail")
    print("SHA256:", hashlib.sha256(repaired).hexdigest())

if __name__ == "__main__": main()
