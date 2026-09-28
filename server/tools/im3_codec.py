"""Read and write the client's 16-bit IM3 sprites without changing frame geometry."""
from __future__ import annotations

import struct
from PIL import Image


def decode(data: bytes) -> Image.Image:
    header = struct.unpack_from('<11I', data)
    width, height, mode, frames = header[3], header[4], header[7], header[8]
    if mode not in (8, 10) or not 0 < width <= 4096 or not 0 < height <= 16384:
        raise ValueError('Unsupported IM3 header')
    offset = 44 + frames * 20
    image = Image.new('RGBA', (width, height))
    pixels = image.load()
    for y in range(height):
        words, segments = struct.unpack_from('<HH', data, offset)
        end, cursor, x = offset + words * 2, offset + 4, 0
        if words < 2 or end > len(data):
            raise ValueError('Invalid IM3 row length')
        count_segments = 0
        while cursor < end:
            skip, count = struct.unpack_from('<HH', data, cursor)
            cursor += 4
            x += skip
            if x + count > width or cursor + count * 2 > end:
                raise ValueError('IM3 run exceeds row bounds')
            for index in range(count):
                color = struct.unpack_from('<H', data, cursor)[0]
                cursor += 2
                pixels[x + index, y] = (
                    (((color >> 8) & 15) * 17, ((color >> 4) & 15) * 17,
                     (color & 15) * 17, ((color >> 12) & 15) * 17)
                    if mode == 10 else
                    (((color >> 10) & 31) * 255 // 31,
                     ((color >> 5) & 31) * 255 // 31, (color & 31) * 255 // 31, 255))
            x += count
            count_segments += 1
        if cursor != end or segments not in (count_segments, 65535):
            raise ValueError('Invalid IM3 segment count')
        offset = end
    if offset != len(data):
        raise ValueError('Unexpected trailing IM3 data')
    return image


def encode(image: Image.Image, original: bytes) -> bytes:
    fields = struct.unpack_from('<11I', original)
    if image.size != (fields[3], fields[4]):
        raise ValueError('Replacement sprite dimensions differ')
    header = bytearray(original[:44 + fields[8] * 20])
    mode = fields[7]
    if mode not in (8, 10):
        raise ValueError('Unsupported IM3 pixel format')
    image = image.convert('RGBA')
    pixels = image.load()
    rows = bytearray()
    for y in range(image.height):
        body, segments, previous, x = bytearray(), 0, 0, 0
        while x < image.width:
            if pixels[x, y][3] < (9 if mode == 10 else 128):
                x += 1
                continue
            start, colors = x, []
            while x < image.width and pixels[x, y][3] >= (9 if mode == 10 else 128):
                red, green, blue, alpha = pixels[x, y]
                if mode == 10:
                    color = ((alpha + 8) // 17 << 12) | ((red + 8) // 17 << 8) | ((green + 8) // 17 << 4) | ((blue + 8) // 17)
                else:
                    color = 0x8000 | ((red * 31 + 127) // 255 << 10) | ((green * 31 + 127) // 255 << 5) | ((blue * 31 + 127) // 255)
                colors.append(color)
                x += 1
            body.extend(struct.pack('<HH', start - previous, len(colors)))
            body.extend(struct.pack('<' + 'H' * len(colors), *colors))
            previous = x
            segments += 1
        rows.extend(struct.pack('<HH', len(body) // 2 + 2, segments))
        rows.extend(body)
    result = header + rows
    struct.pack_into('<I', result, 8, len(result) - 8)
    decode(result)
    return bytes(result)


def archive_entries(data):
    count = struct.unpack_from('<I',data,9)[0]
    result = []
    cursor = 13 + count*8
    for index in range(count):
        rid, offset = struct.unpack_from('<II',data,13+index*8)
        if offset != cursor:
            raise ValueError('Noncontiguous archive')
        size = struct.unpack_from('<I',data,offset)[0]
        result.append((rid,data[offset+4:offset+4+size]))
        cursor = offset+4+size
    if cursor != len(data):
        raise ValueError('Archive size mismatch')
    return result

