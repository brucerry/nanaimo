"""Compile editable zh-HK text into client catalogs, sprites, and managed source.

No historical binary backups are needed. All inputs are current assets and
text-free sprite templates. All validation completes before any file is written.
"""
from __future__ import annotations
import argparse
import hashlib
import json
import re
import struct
from collections import defaultdict
from pathlib import Path
from Crypto.Cipher import AES
from Crypto.Util.Padding import pad, unpad
from PIL import Image, ImageDraw, ImageFont
from im3_codec import decode, encode
from im3_codec import archive_entries
from localize_catalogs import KEY
from localization import traditional_text
from opencc import OpenCC

ROOT = Path(__file__).resolve().parents[2]
LOCALE = ROOT / 'localization'
HAN = re.compile('[\u3400-\u9fff]')
CS_STRING = re.compile(r'"(?:\\.|[^"\\\r\n])*"')
FONT = 'C:/Windows/Fonts/msjhbd.ttc'

def load(path):
    return json.loads(path.read_text(encoding='utf-8-sig'))

def format_tokens(s):
    return re.findall(r'%(?:\d+\$)?[-+ #0]*(?:\d+|\*)?(?:\.(?:\d+|\*))?[hlLI]*[diuoxXfFeEgGaAcCsSpn%]|\{[^{}]*\}',s)

def validate_text(text, key):
    if '\x00' in text:
        raise ValueError('NUL in text: '+key)
    if traditional_text(CC, text) != text:
        raise ValueError('Non-Traditional text: '+key)

CC = OpenCC('s2t')

def render(template, regions, texts):
    image = decode(template)
    for region in regions:
        text = texts[region['key']]
        x,y,w,h = region['box']
        if x < 0 or y < 0 or x+w > image.width or y+h > image.height:
            raise ValueError('Text region exceeds sprite: '+region['key'])
        size = region.get('size', min(18,h-2))
        lines = text.split('\n')
        # Explicitly reject unreadable overflow instead of clipping or truncating.
        while size >= 8:
            font = ImageFont.truetype(FONT,size*4)
            widths = [font.getlength(line) for line in lines]
            line_height = size*4+4
            if max(widths,default=0)<=w*4-4 and line_height*len(lines)<=h*4+4:break
            size-=1
        if size<8:raise ValueError('Text does not fit: '+region['key'])
        layer=Image.new('RGBA',(w*4,h*4))
        draw=ImageDraw.Draw(layer)
        for i,line in enumerate(lines):
            bounds=font.getbbox(line)
            px=(w*4-font.getlength(line))/2
            py=(h*4-line_height*len(lines))/2+i*line_height-bounds[1]+4
            draw.text((px,py),line,font=font,fill=region.get('color','white'),
                      stroke_width=region.get('stroke',0)*4,stroke_fill=region.get('outline','#333333'))
        image.alpha_composite(layer.resize((w,h),Image.Resampling.LANCZOS),(x,y))
    return encode(image,template)

def prepare(root=ROOT, locale=LOCALE):
    pending={};stats={}
    catalog_text=load(locale/'zh-HK/catalogs.json')
    managed_text=load(locale/'zh-HK/managed.json')
    ui_text=load(locale/'zh-HK/ui.json') if (locale/'zh-HK/ui.json').exists() else {}
    for group in (catalog_text,managed_text,ui_text):
        for key,text in group.items():validate_text(text,key)
    fields_changed=0
    for name,binding in load(locale/'catalog-bindings.json').items():
        path=root/'client'/name
        data=path.read_bytes()
        fields=unpad(AES.new(KEY,AES.MODE_CBC,iv=bytes(16)).decrypt(data),16).decode('gbk').split('#')
        if len(fields)!=binding['field_count']:raise ValueError('Catalog shape changed: '+name)
        for index,ref in binding['fields'].items():
            i=int(index);text=catalog_text[ref['key']]
            if '#' in text or len(text.encode('gbk'))>ref['max_bytes']:
                raise ValueError('Catalog delimiter/byte limit: '+name+':'+index)
            if any(len(line.encode('gbk')) > ref.get('max_line_bytes', 65535) for line in text.split('^&')):
                raise ValueError('Catalog display-line limit: '+name+':'+index)
            if format_tokens(text)!=format_tokens(fields[i]):raise ValueError('Catalog format tokens changed: '+name+':'+index)
            fields_changed+=fields[i]!=text;fields[i]=text
        plain='#'.join(fields).encode('gbk')
        output=AES.new(KEY,AES.MODE_CBC,iv=bytes(16)).encrypt(pad(plain,16))
        if unpad(AES.new(KEY,AES.MODE_CBC,iv=bytes(16)).decrypt(output),16)!=plain:raise ValueError('Catalog roundtrip failed')
        pending[path]=output
        # Keep development and published runtime catalogs in sync.
        for base in [root/'server/src/managed/resources/data',*list((root/'client/server').glob('**/resources/data'))]:
            target=base/name
            if target.exists():pending[target]=output
    stats['catalog_fields_changed']=fields_changed
    grouped=defaultdict(list)
    for ref in load(locale/'managed-bindings.json'):grouped[ref['path']].append(ref)
    for relative,refs in grouped.items():
        path=root/relative;s=path.read_text(encoding='utf-8-sig');matches=list(CS_STRING.finditer(s))
        edits=[]
        for ref in refs:
            match=matches[ref['literal']];text=managed_text[ref['key']]
            if HAN.sub('',match[0][1:-1])!=ref['ascii'] or HAN.sub('',text)!=ref['ascii']:
                raise ValueError('Source structure changed; rebind literal: '+ref['key'])
            edits.append((match.start()+1,match.end()-1,text))
        for start,end,text in sorted(edits,reverse=True):s=s[:start]+text+s[end:]
        pending[path]=s.encode('utf-8')
    stats['managed_strings']=len(managed_text)
    if (locale/'sprite-bindings.json').exists():
        pack_path=root/'client/images/interface.pack';data=pack_path.read_bytes();entries=archive_entries(data);replacements={}
        installed=dict(entries)
        state_path=locale/'generated-state.json'
        previous=load(state_path) if state_path.exists() else {}
        state={}
        font_hash=hashlib.sha256(Path(FONT).read_bytes()).hexdigest()
        for binding in load(locale/'sprite-bindings.json'):
            template=(locale/'templates'/binding['template']).read_bytes()
            inputs=json.dumps([binding['regions'],{r['key']:ui_text[r['key']] for r in binding['regions']},font_hash,'renderer-v1'],sort_keys=True).encode()
            input_hash=hashlib.sha256(template+inputs).hexdigest()
            first=binding['targets'][0]
            current=installed[first['pack']] if 'pack' in first else (root/first['path']).read_bytes()
            cached=previous.get(binding['template'],{})
            if cached.get('input')==input_hash and cached.get('output')==hashlib.sha256(current).hexdigest():
                result=current
            else:
                result=render(template,binding['regions'],ui_text)
            state[binding['template']]={'input':input_hash,'output':hashlib.sha256(result).hexdigest()}
            for target in binding['targets']:
                if 'pack' in target:replacements[target['pack']]=result
                else:
                    target_path=(root/target['path']).resolve()
                    if not target_path.is_relative_to((root/'client').resolve()):raise ValueError('Sprite target outside client')
                    if struct.unpack_from('<11I',target_path.read_bytes())[3:9]!=struct.unpack_from('<11I',result)[3:9]:raise ValueError('Sprite geometry changed')
                    pending[target_path]=result
        rebuilt=bytearray(data[:13+len(entries)*8])
        for i,(rid,payload) in enumerate(entries):
            payload=replacements.get(rid,payload)
            struct.pack_into('<II',rebuilt,13+i*8,rid,len(rebuilt))
            rebuilt.extend(struct.pack('<I',len(payload)));rebuilt.extend(payload)
        pending[pack_path]=bytes(rebuilt)
        pending[state_path]=(json.dumps(state,indent=2)+'\n').encode()
        stats['sprite_entries']=len(replacements)
    brand_path=locale/'branding.json'
    if brand_path.exists():
        brand=load(brand_path)
        brand['name']=ui_text[brand['key']]
        for asset in brand['assets']:
            target=(root/asset['path']).resolve()
            if not target.is_relative_to((root/'client').resolve()):raise ValueError('Brand target outside client')
            output=pending.get(target)
            if output is None:output=target.read_bytes()
            asset['sha256']=hashlib.sha256(output).hexdigest()
        pending[brand_path]=(json.dumps(brand,ensure_ascii=False,indent=2)+'\n').encode('utf-8')
    return pending,stats

def main():
    ap=argparse.ArgumentParser(description=__doc__)
    ap.add_argument('--apply',action='store_true')
    ap.add_argument('--root',type=Path,default=ROOT)
    args=ap.parse_args()
    root=args.root.resolve()
    pending,stats=prepare(root,root/'localization')
    changes={p:data for p,data in pending.items() if not p.exists() or p.read_bytes()!=data}
    if args.apply:
        for path,data in changes.items():
            temp=path.with_name(path.name+'.locale-tmp');temp.write_bytes(data);temp.replace(path)
    print(json.dumps(dict(applied=args.apply,changed_files=len(changes),**stats)))

if __name__=='__main__':main()
