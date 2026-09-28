import argparse
import ctypes as c
from ctypes import wintypes as w
import json


class Module(c.Structure):
    _fields_=[('size',w.DWORD),('id',w.DWORD),('pid',w.DWORD),('global_usage',w.DWORD),
              ('process_usage',w.DWORD),('base',c.c_void_p),('base_size',w.DWORD),
              ('handle',w.HMODULE),('name',w.WCHAR*256),('path',w.WCHAR*260)]


def modules(pid):
    kernel=c.WinDLL('kernel32',use_last_error=True)
    kernel.CreateToolhelp32Snapshot.argtypes=[w.DWORD,w.DWORD]
    kernel.CreateToolhelp32Snapshot.restype=w.HANDLE
    kernel.Module32FirstW.argtypes=[w.HANDLE,c.POINTER(Module)]
    kernel.Module32NextW.argtypes=[w.HANDLE,c.POINTER(Module)]
    kernel.CloseHandle.argtypes=[w.HANDLE]
    handle=kernel.CreateToolhelp32Snapshot(0x18,pid)
    if handle==c.c_void_p(-1).value:
        raise c.WinError(c.get_last_error())
    result=[]
    try:
        entry=Module();entry.size=c.sizeof(entry)
        more=kernel.Module32FirstW(handle,c.byref(entry))
        if not more:
            raise c.WinError(c.get_last_error())
        while more:
            result.append({'name':entry.name,'path':entry.path,'base':hex(entry.base),'size':entry.base_size})
            more=kernel.Module32NextW(handle,c.byref(entry))
    finally:
        kernel.CloseHandle(handle)
    return result


if __name__=='__main__':
    ap=argparse.ArgumentParser()
    ap.add_argument('pid',type=int)
    args=ap.parse_args()
    result=modules(args.pid)
    forbidden=[m for m in result if m['name'].lower().startswith(('tersafe','tensafe','tenprotect','tenslx','tenslu','tensm','tentc'))]
    print(json.dumps({'pid':args.pid,'tp_modules':forbidden,'modules':result},indent=2,ensure_ascii=True))
    raise SystemExit(1 if forbidden else 0)
