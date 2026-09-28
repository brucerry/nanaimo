"""Capture and interact with a specific local application's main window."""
import argparse
import ctypes as c
from ctypes import wintypes as w
from pathlib import Path
import time

from PIL import ImageGrab


def main():
    ap=argparse.ArgumentParser()
    ap.add_argument('pid',type=int)
    ap.add_argument('--capture',type=Path)
    ap.add_argument('--click',type=int,nargs=2)
    ap.add_argument('--double',action='store_true')
    ap.add_argument('--key',type=lambda x:int(x,0))
    ap.add_argument('--hold',type=float,default=0.5)
    args=ap.parse_args()
    user=c.WinDLL('user32',use_last_error=True)
    callback_type=c.WINFUNCTYPE(w.BOOL,w.HWND,w.LPARAM)
    user.EnumWindows.argtypes=[callback_type,w.LPARAM]
    user.GetWindowThreadProcessId.argtypes=[w.HWND,c.POINTER(w.DWORD)]
    user.IsWindowVisible.argtypes=[w.HWND]
    user.GetWindowRect.argtypes=[w.HWND,c.POINTER(w.RECT)]
    user.SetForegroundWindow.argtypes=[w.HWND]
    user.ClientToScreen.argtypes=[w.HWND,c.POINTER(w.POINT)]
    windows=[]
    def callback(window,state):
        pid=w.DWORD();user.GetWindowThreadProcessId(window,c.byref(pid))
        if pid.value==args.pid and user.IsWindowVisible(window):
            rect=w.RECT();user.GetWindowRect(window,c.byref(rect))
            windows.append(((rect.right-rect.left)*(rect.bottom-rect.top),window,rect))
        return True
    user.EnumWindows(callback_type(callback),0)
    if not windows:raise RuntimeError('No visible window for target process')
    _,window,rect=max(windows,key=lambda row:row[0])
    user.SetForegroundWindow(window)
    time.sleep(0.25)
    if args.click:
        point=w.POINT(*args.click);user.ClientToScreen(window,c.byref(point))
        user.SetCursorPos(point.x,point.y)
        for _ in range(2 if args.double else 1):
            user.mouse_event(2,0,0,0,0);time.sleep(0.06)
            user.mouse_event(4,0,0,0,0);time.sleep(0.08)
    if args.key is not None:
        scan=user.MapVirtualKeyW(args.key,0)
        flags=1 if args.key in (0x25,0x26,0x27,0x28) else 0
        user.keybd_event(args.key,scan,flags,0)
        try:time.sleep(args.hold)
        finally:user.keybd_event(args.key,scan,flags|2,0)
    if args.capture:
        time.sleep(0.25)
        ImageGrab.grab(bbox=(rect.left,rect.top,rect.right,rect.bottom)).save(args.capture)
    print('WINDOW_CHECK',args.pid,window,rect.left,rect.top,rect.right,rect.bottom)


if __name__=='__main__':
    main()
