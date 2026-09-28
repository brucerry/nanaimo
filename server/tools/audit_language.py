"""Audit workspace text, editable locales, and encrypted client catalogs.

Binary libraries, game art, saves, and installed third-party dependency data are
not interpreted as text. Bitmap outputs are verified by test_game_ui.py against
font-rendered locale keys; OCR alone cannot prove a glyph's Chinese script.
"""
from __future__ import annotations
import argparse,json,re
from pathlib import Path
from opencc import OpenCC
from Crypto.Cipher import AES
from Crypto.Util.Padding import unpad
from localize_catalogs import KEY
from localization import traditional_text

HAN=re.compile('[\u3400-\u9fff]')
TEXT_EXT={'.cs','.c','.h','.inc','.py','.ps1','.md','.txt','.json','.xml','.ini','.cmd','.bat','.csproj','.sln','.props','.targets','.config','.csv','.tsv','.log','.htm','.html','.yml','.yaml','.toml','.cfg','.sh','.resx'}
SKIP={'.git','__pycache__','venv','.venv','site-packages'}
ENCODINGS={'client/StateOption/Animaloption.ini':'cp949'}

def read_text(path,relative):
    data=path.read_bytes()
    if relative in ENCODINGS:return data.decode(ENCODINGS[relative])
    if data.startswith((b'\xff\xfe',b'\xfe\xff')):return data.decode('utf-16')
    try:return data.decode('utf-8-sig')
    except UnicodeDecodeError:return data.decode('gbk')

def audit(root,active_only=False):
    converter=OpenCC('s2t');issues=[];checked=0;excluded=0
    for path in root.rglob('*'):
        relative=path.relative_to(root).as_posix()
        if set(path.relative_to(root).parts)&SKIP:continue
        if active_only and (set(path.relative_to(root).parts)&{'.work','bin','bin-win7','obj','checks','launcher-check-output','tcc'} or path.suffix.lower()=='.log'):
            continue
        if HAN.search(path.name):issues.append(relative+': non-English path')
        if not path.is_file():continue
        if path.suffix.lower() not in TEXT_EXT and path.name not in {'LICENSE','.gitignore','.gitattributes'}:
            excluded+=1;continue
        # Client save content belongs to the player, not to the source locale.
        if relative.startswith('client/server/server-merged/data/') or relative.endswith('/local-account.json'):
            excluded+=1;continue
        try:text=read_text(path,relative)
        except UnicodeError:
            issues.append(relative+': unrecognized text encoding');continue
        checked+=1
        for number,line in enumerate(text.splitlines(),1):
            if HAN.search(line) and traditional_text(converter,line)!=line:
                issues.append(f'{relative}:{number}: non-Traditional text')
    catalogs=0
    for path in (root/'client').glob('*._D*'):
        catalogs+=1
        try:
            text=unpad(AES.new(KEY,AES.MODE_CBC,iv=bytes(16)).decrypt(path.read_bytes()),16).decode('gbk')
            if traditional_text(converter,text)!=text:issues.append(path.relative_to(root).as_posix()+': non-Traditional catalog')
        except (ValueError,UnicodeError):issues.append(path.name+': invalid catalog')
    return {'scope':'active source and game text' if active_only else 'workspace text',
            'text_files':checked,'encrypted_catalogs':catalogs,'non_text_or_player_files':excluded,'issues':issues}

def main():
    parser=argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--root',type=Path,default=Path(__file__).resolve().parents[2])
    parser.add_argument('--active-only',action='store_true',help='Exclude historical evidence, build output, and dependency source')
    parser.add_argument('--output',type=Path)
    args=parser.parse_args();result=audit(args.root.resolve(),args.active_only)
    if args.output:
        args.output.parent.mkdir(parents=True,exist_ok=True)
        args.output.write_text(json.dumps(result,indent=2)+'\n',encoding='utf-8')
    print(json.dumps(result,indent=2));return int(bool(result['issues']))

if __name__=='__main__':raise SystemExit(main())
