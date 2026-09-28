"""Convert Nanaimo's AES/GBK catalogs to Traditional Chinese, with backups.

Run without --apply to inspect changes. Install opencc-python-reimplemented==0.1.7
and pycryptodome==3.23.0 in a virtual environment first.
"""
from __future__ import annotations
import argparse
import hashlib
import json
from pathlib import Path
import re
from Crypto.Cipher import AES
from Crypto.Util.Padding import pad, unpad
from opencc import OpenCC
from localization import traditional_text

KEY = bytes.fromhex('0123456789abcdef123456789abcdef0')

def convert_catalog(data: bytes, converter: OpenCC) -> tuple[bytes, int]:
    plain = unpad(AES.new(KEY, AES.MODE_CBC, iv=bytes(16)).decrypt(data), 16)
    original = plain.decode('gbk', errors='strict')
    fields = original.split('#')
    converted = [traditional_text(converter, field) for field in fields]
    for before, after in zip(fields, converted):
        # ASCII includes IDs, filenames, markup, numeric fields, and delimiters.
        if re.sub(r'[^\x00-\x7f]', '', before) != re.sub(r'[^\x00-\x7f]', '', after):
            raise ValueError('Conversion changed an ASCII field component')
        if len(after.encode('gbk', errors='strict')) > len(before.encode('gbk')):
            raise ValueError('Conversion expanded a GBK field')
    count = sum(a != b for a, b in zip(fields, converted))
    if not count:
        return data, 0
    output = '#'.join(converted).encode('gbk', errors='strict')
    encrypted = AES.new(KEY, AES.MODE_CBC, iv=bytes(16)).encrypt(pad(output, 16))
    verified = unpad(AES.new(KEY, AES.MODE_CBC, iv=bytes(16)).decrypt(encrypted), 16)
    if verified != output or len(verified.decode('gbk').split('#')) != len(fields):
        raise ValueError('Catalog round-trip verification failed')
    return encrypted, count

def main():
    from build_game_locale import main as build
    build()

if __name__ == '__main__':
    main()
