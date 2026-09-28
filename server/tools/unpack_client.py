"""Restore aPLib sections and original imports from the pinned local client."""
import argparse
import hashlib
from pathlib import Path
import struct

import aplib
import pefile

ORIGINAL = 'd56c80ca893990761bbb7faf412f54d8cb56763ad2d2d74aefc3f9fe63187515'


def align(value, alignment=0x1000):
    return (value+alignment-1)//alignment*alignment


def build(source, reference=None):
    original=source.read_bytes()
    if hashlib.sha256(original).hexdigest()!=ORIGINAL:
        raise ValueError('Unrecognized source client')
    pe=pefile.PE(data=original)
    result=bytearray(original[:0x1000])
    names=['.text','.rdata','.data','.idata','.data1','.rsrc','.vmcode','.pack','.vmp0']
    for section,name in zip(pe.sections,names):
        data=section.get_data()
        if data.startswith(b'AP32'):
            data=aplib.decompress(data)
        size=align(max(len(data),section.Misc_VirtualSize))
        pointer=len(result)
        result.extend(data.ljust(size,b'\0'))
        offset=section.get_file_offset()
        struct.pack_into('<8sIIII',result,offset,name.encode().ljust(8,b'\0'),size,section.VirtualAddress,size,pointer)
    optional=pe.OPTIONAL_HEADER
    struct.pack_into('<I',result,optional.get_field_absolute_offset('AddressOfEntryPoint'),0x750c03)
    struct.pack_into('<H',result,optional.get_field_absolute_offset('DllCharacteristics'),0)
    struct.pack_into('<II',result,optional.DATA_DIRECTORY[1].get_file_offset(),0x991000,0x400)
    for index in (4,5,10,11,12):
        struct.pack_into('<II',result,optional.DATA_DIRECTORY[index].get_file_offset(),0,0)
    struct.pack_into('<I',result,optional.get_field_absolute_offset('CheckSum'),0)
    restored=pefile.PE(data=bytes(result))
    if reference:
        reference_data=reference.read_bytes()
        if hashlib.sha256(reference_data).hexdigest()!='bad493ca377941f9be4ed5e79765ed09d595d93e4354ae640ecdf9b29ceae982':
            raise ValueError('Unexpected recovery reference')
        ref=pefile.PE(data=reference_data)
        for rva,size in ((0x93e000,0x53000),(0x995000,0x3000),(0x998000,0x2000)):
            if restored.get_data(rva,size)!=ref.get_data(rva,size):
                raise ValueError('Reference data does not match original')
        original_text=restored.sections[0].get_data()
        reference_text=ref.sections[0].get_data()
        if len(original_text)!=len(reference_text) or sum(a!=b for a,b in zip(original_text,reference_text))>1300:
            raise ValueError('Reference native code is not the matching version')
        appended=ref.sections[9:]
        if [s.Name.rstrip(b'\0') for s in appended]!=[b'.idata2',b'.vmpvm',b'.vmhp',b'.vmh2']:
            raise ValueError('Missing recovered VM sections')
        table=pe.sections[0].get_file_offset()
        for index,section in enumerate(appended,9):
            pointer=len(result)
            data=section.get_data()
            result.extend(data)
            header=bytearray(reference_data[section.get_file_offset():section.get_file_offset()+40])
            struct.pack_into('<I',header,20,pointer)
            result[table+index*40:table+(index+1)*40]=header
        struct.pack_into('<H',result,pe.FILE_HEADER.get_field_absolute_offset('NumberOfSections'),13)
        struct.pack_into('<I',result,optional.get_field_absolute_offset('SizeOfImage'),ref.OPTIONAL_HEADER.SizeOfImage)
        directory=ref.OPTIONAL_HEADER.DATA_DIRECTORY[1]
        struct.pack_into('<II',result,optional.DATA_DIRECTORY[1].get_file_offset(),directory.VirtualAddress,directory.Size)
        # Original IAT slots and API names have been recovered in the reference.
        offset=restored.get_offset_from_rva(0x991000)
        result[offset:offset+0x4000]=ref.get_data(0x991000,0x4000)
        for address in (0x43c100,0x5ce700,0x8a29b0):
            offset=restored.get_offset_from_rva(address-0x400000)
            result[offset:offset+10]=ref.get_data(address-0x400000,10)
    # Use the native WinMain and initialize the legacy floating-point helpers.
    patches={0xb50b95:bytes.fromhex('e846948eff'),0xb4ed97:b'\x90\x90',
             0xa6b03e:bytes.fromhex('31c0909090'),0xa6b060:bytes.fromhex('31c0c3'),
             0x4dfb1a:bytes.fromhex('e96e00000090'),
             0x43a53f:bytes.fromhex('eb19'),
             0x43a8b2:b'\x90'*6,0x43a8c1:bytes.fromhex('eb3f'),
             0x43a927:bytes.fromhex('eb3b')}
    # Restore the retail router: a missing character must reach the in-game
    # character creation screen; existing characters continue normally.
    patches[0x4e2f1e]=bytes.fromhex('e8b5b1f3ff')  # 0x41e0d8
    patches[0x4e3a8e]=bytes.fromhex('e81468f5ff')  # 0x43a2a7
    patches[0x43a2a7]=bytes.fromhex(
        '518b0dd469d800e8253efeff84c05975086a01e8d696fcffc3e932defdff'
        '90909090909090909090909090909090')
    # Preserve Chinese IME composition but hide the old in-game candidate
    # list. Also avoid dereferencing its stale row during error-popup cleanup.
    patches[0xa99bc6]=bytes.fromhex('e94c01000090')
    patches[0xa99d64]=bytes.fromhex('e96401000090')
    patches[0x7ed166]=bytes.fromhex('eb13')
    # Skip the legacy result-10 popup and continue at the retry-state reset.
    patches[0x4e35c6]=bytes.fromhex('e992000000')
    # Character-creation errors must clear manager+0x378 so the Create button
    # can be used again after a duplicate or invalid name.
    patches[0x4e3663]=bytes.fromhex('e87c71f3ff')
    patches[0x4e3789]=bytes.fromhex('e85670f3ff')
    # GetDC cannot be called while a DirectDraw surface is locked. Skip the
    # redundant Lock/Unlock calls in both narrow and wide text renderers.
    patches[0xacb7cf]=bytes.fromhex('83c4149090')
    patches[0xacbc3e]=bytes.fromhex('83c4149090')
    patches[0xacba0b]=bytes.fromhex('83c4089090909090')
    patches[0xacbe7a]=bytes.fromhex('83c4089090909090')
    # Skip the sound prompt to its Cancel / Sound ON branch at 0x43a902.
    for address in (0x4d6623,0x4d6683,0x4dac63,0x4dacc3):
        patches[address]=bytes.fromhex('b801000000')
    # The local adapter uses native plaintext frames. Preserve the IDProto
    # wrapper and select its native "not encrypted" result without TerSafe.
    patches.update({
        0xa6ab20:bytes.fromhex('890d8436d70031c089018bc1c3'),
        0xa6ab60:bytes.fromhex('31c0c3'),
        0xa6ab90:bytes.fromhex('31c0c20400'),
        0xa6ac80:bytes.fromhex('31c0c20400'),
        0xa6ad80:bytes.fromhex('b802000000c20400'),
        0xa6aed0:bytes.fromhex('83c8ffc20c00'),
    })
    for address,data in patches.items():
        offset=restored.get_offset_from_rva(address-0x400000)
        result[offset:offset+len(data)]=data
    return bytes(result)


if __name__=='__main__':
    ap=argparse.ArgumentParser()
    ap.add_argument('source',type=Path)
    ap.add_argument('output',type=Path)
    ap.add_argument('--reference',type=Path,required=True)
    args=ap.parse_args()
    data=build(args.source,args.reference)
    args.output.write_bytes(data)
    print('RESTORED',len(data),hashlib.sha256(data).hexdigest())
