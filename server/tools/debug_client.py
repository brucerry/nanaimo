"""Bounded Windows debugger for this workspace's 32-bit client."""
import argparse
import ctypes as c
from ctypes import wintypes as w
import json
from pathlib import Path
import time

K = c.WinDLL("kernel32", use_last_error=True)


class Startup(c.Structure):
    _fields_ = [("cb", w.DWORD), ("reserved", w.LPWSTR), ("desktop", w.LPWSTR),
                ("title", w.LPWSTR), ("x", w.DWORD), ("y", w.DWORD),
                ("sx", w.DWORD), ("sy", w.DWORD), ("cx", w.DWORD),
                ("cy", w.DWORD), ("fill", w.DWORD), ("flags", w.DWORD),
                ("show", w.WORD), ("reserved2", w.WORD), ("reserved3", c.c_void_p),
                ("stdin", w.HANDLE), ("stdout", w.HANDLE), ("stderr", w.HANDLE)]


class ProcessInfo(c.Structure):
    _fields_ = [("process", w.HANDLE), ("thread", w.HANDLE), ("pid", w.DWORD), ("tid", w.DWORD)]


class ExceptionRecord(c.Structure):
    _fields_ = [("code", w.DWORD), ("flags", w.DWORD), ("record", c.c_void_p),
                ("address", c.c_void_p), ("count", w.DWORD), ("info", c.c_size_t * 15)]


class ExceptionInfo(c.Structure):
    _fields_ = [("record", ExceptionRecord), ("first", w.DWORD)]


class EventData(c.Union):
    _fields_ = [("exception", ExceptionInfo), ("raw", c.c_ubyte * 160)]


class DebugEvent(c.Structure):
    _fields_ = [("code", w.DWORD), ("pid", w.DWORD), ("tid", w.DWORD), ("data", EventData)]


class FloatSave(c.Structure):
    _fields_ = [("control", w.DWORD), ("status", w.DWORD), ("tag", w.DWORD),
                ("error", w.DWORD), ("errorselector", w.DWORD), ("data", w.DWORD),
                ("dataselector", w.DWORD), ("registers", c.c_byte * 80), ("cr0", w.DWORD)]


class Context(c.Structure):
    _fields_ = [("flags", w.DWORD)] + [(s, w.DWORD) for s in ("dr0", "dr1", "dr2", "dr3", "dr6", "dr7")] + [
        ("float", FloatSave)] + [(s, w.DWORD) for s in (
            "gs", "fs", "es", "ds", "edi", "esi", "ebx", "edx", "ecx", "eax",
            "ebp", "eip", "cs", "eflags", "esp", "ss")] + [("extended", c.c_byte * 512)]


class MemoryInfo(c.Structure):
    _fields_ = [("base",c.c_void_p),("allocation",c.c_void_p),("allocation_protect",w.DWORD),
                ("size",c.c_size_t),("state",w.DWORD),("protect",w.DWORD),("kind",w.DWORD)]


K.CreateProcessW.argtypes = [w.LPCWSTR,w.LPWSTR,c.c_void_p,c.c_void_p,w.BOOL,w.DWORD,c.c_void_p,w.LPCWSTR,c.POINTER(Startup),c.POINTER(ProcessInfo)]
K.CreateProcessW.restype = w.BOOL
K.OpenProcess.argtypes = [w.DWORD,w.BOOL,w.DWORD]
K.OpenProcess.restype = w.HANDLE
K.OpenThread.argtypes = [w.DWORD,w.BOOL,w.DWORD]
K.OpenThread.restype = w.HANDLE
K.ReadProcessMemory.argtypes = [w.HANDLE,c.c_void_p,c.c_void_p,c.c_size_t,c.POINTER(c.c_size_t)]
K.WriteProcessMemory.argtypes = [w.HANDLE,c.c_void_p,c.c_void_p,c.c_size_t,c.POINTER(c.c_size_t)]
K.Wow64GetThreadContext.argtypes = [w.HANDLE,c.POINTER(Context)]
K.Wow64SetThreadContext.argtypes = [w.HANDLE,c.POINTER(Context)]
K.CloseHandle.argtypes = [w.HANDLE]
K.TerminateProcess.argtypes = [w.HANDLE,w.UINT]
K.WaitForDebugEvent.argtypes = [c.POINTER(DebugEvent),w.DWORD]
K.VirtualQueryEx.argtypes = [w.HANDLE,c.c_void_p,c.POINTER(MemoryInfo),c.c_size_t]


def read(process, address, length):
    buf = c.create_string_buffer(length)
    got = c.c_size_t()
    K.ReadProcessMemory(process,address,buf,length,c.byref(got))
    return bytes(buf[:got.value])


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("client", type=Path)
    ap.add_argument("--seconds", type=int, default=25)
    ap.add_argument("--break-at", type=lambda x:int(x,0))
    ap.add_argument("--dump", type=Path)
    ap.add_argument("--skip-loader-list", action="store_true")
    ap.add_argument("--text-dump", type=Path, help="Dump launched-client regions containing a supplied GBK needle")
    ap.add_argument("--text-needle", default="")
    args = ap.parse_args()
    client = args.client.resolve()
    startup, process = Startup(), ProcessInfo()
    startup.cb = c.sizeof(startup)
    command = c.create_unicode_buffer(f'"{client}" -q :1:1:0:3:4:-i 5:-r 6:7:1:127.0.0.1:')
    if not K.CreateProcessW(str(client),command,None,None,False,1,None,str(client.parent),c.byref(startup),c.byref(process)):
        raise c.WinError(c.get_last_error())
    processes = {process.pid: process.process}
    patched = {}
    deadline = time.monotonic()+args.seconds
    print(json.dumps({"start":process.pid}),flush=True)
    try:
        while time.monotonic()<deadline:
            event=DebugEvent()
            if not K.WaitForDebugEvent(c.byref(event),500):
                continue
            status=0x10002
            handle=processes.get(event.pid)
            if handle is None:
                handle=K.OpenProcess(0x1F0FFF,False,event.pid)
                processes[event.pid]=handle
            if event.code==3:
                print(json.dumps({"created":event.pid}),flush=True)
            if event.code==1:
                ex=event.data.exception
                thread=K.OpenThread(0x1F03FF,False,event.tid)
                ctx=Context();ctx.flags=0x1003F
                ok=K.Wow64GetThreadContext(thread,c.byref(ctx))
                record={"pid":event.pid,"exception":hex(ex.record.code),"address":hex(ex.record.address or 0),
                        "first":ex.first,"context_ok":bool(ok),
                        "registers":{s:hex(getattr(ctx,s)) for s in ("eip","esp","ebp","eax","ebx","ecx","edx","esi","edi")},
                        "code":read(handle,ctx.eip,48).hex(),"stack":read(handle,ctx.esp,96).hex(),
                        "info":[hex(x) for x in ex.record.info[:ex.record.count]]}
                print(json.dumps(record),flush=True)
                if args.dump and ex.record.code==0xc0000005 and ex.first:
                    args.dump.mkdir(parents=True,exist_ok=True)
                    (args.dump/'fault-image.bin').write_bytes(read(handle,0x400000,0xBFD000))
                    (args.dump/'fault-code.bin').write_bytes(read(handle,ctx.eip-128,384))
                    region=MemoryInfo()
                    K.VirtualQueryEx(handle,ctx.eip,c.byref(region),c.sizeof(region))
                    print(json.dumps({"allocation":hex(region.allocation or 0),"region":hex(region.base or 0),"size":region.size}),flush=True)
                    if region.allocation:
                        memory=bytearray()
                        for address in range(region.allocation,region.allocation+0x200000,0x1000):
                            memory.extend(read(handle,address,0x1000).ljust(0x1000,b'\0'))
                        (args.dump/'fault-allocation.bin').write_bytes(memory)
                if args.skip_loader_list and ex.record.code==0xc0000005 and ex.first and read(handle,ctx.eip,6)==bytes.fromhex('8b51103b55d0'):
                    mapping=int.from_bytes(read(handle,ctx.ebp-0x2c,4),'little')
                    K.WriteProcessMemory(handle,mapping,c.c_char_p(b'\0'*4),4,None)
                    ctx.eip += 0x1e
                    if not K.Wow64SetThreadContext(thread,c.byref(ctx)):
                        raise c.WinError(c.get_last_error())
                    print('SKIPPED_INVALID_LOADER_LIST',flush=True)
                elif ex.record.code in (0x80000003,0x4000001f):
                    if args.break_at and ex.record.address==args.break_at:
                        if args.dump:
                            args.dump.mkdir(parents=True,exist_ok=True)
                            image=read(handle,0x400000,0xBFD000)
                            (args.dump/'image.bin').write_bytes(image)
                            print(json.dumps({"dump_size":len(image)}),flush=True)
                        K.CloseHandle(thread)
                        break
                    if args.break_at and event.pid not in patched:
                        patched[event.pid]=read(handle,args.break_at,1)
                        K.WriteProcessMemory(handle,args.break_at,c.c_char_p(b'\xcc'),1,None)
                else:
                    status=0x80010001
                K.CloseHandle(thread)
            elif event.code==5:
                print(json.dumps({"exit":event.pid,"code":int.from_bytes(bytes(event.data.raw[:4]),'little')}),flush=True)
                if event.pid==process.pid and len(processes)==1:
                    break
            K.ContinueDebugEvent(event.pid,event.tid,status)
    finally:
        for handle in processes.values():
            if args.text_dump and args.text_needle:
                args.text_dump.mkdir(parents=True, exist_ok=True)
                address = 0
                needle = args.text_needle.encode('gbk')
                while address < 0x80000000:
                    region = MemoryInfo()
                    if not K.VirtualQueryEx(handle,address,c.byref(region),c.sizeof(region)) or not region.size:
                        break
                    if region.state == 0x1000 and not region.protect & 0x101:
                        data = read(handle,region.base,region.size)
                        if needle in data:
                            name = f'{region.base:08x}.bin'
                            (args.text_dump/name).write_bytes(data)
                            print(json.dumps({'text_region':name,'size':len(data)}),flush=True)
                    address = (region.base or address) + region.size
            K.TerminateProcess(handle,0)
            K.CloseHandle(handle)
        K.CloseHandle(process.thread)


if __name__=='__main__':
    main()
