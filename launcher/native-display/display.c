/* Preserve native text rendering; scale only the completed frame and mouse input. */
#include <wchar.h>
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
static HMODULE renderer;
static HWND gameWindow;
static WNDPROC originalProc;
static WCHAR directory[MAX_PATH];
static LRESULT CALLBACK GameProc(HWND window, UINT message, WPARAM wp, LPARAM lp)
{
    if (message >= WM_MOUSEFIRST && message <= WM_MOUSELAST && message != WM_MOUSEWHEEL && message != 0x020e) {
        RECT r;
        if (GetClientRect(window, &r) && r.right > 0 && r.bottom > 0) {
            int x = (short)LOWORD(lp), y = (short)HIWORD(lp);
            x = x * 800 / r.right; y = y * 600 / r.bottom;
            lp = MAKELPARAM(x, y);
        }
    }
    return CallWindowProcA(originalProc, window, message, wp, lp);
}
static BOOL CALLBACK FindGame(HWND window, LPARAM unused)
{
    DWORD pid;
    RECT r;
    GetWindowThreadProcessId(window, &pid);
    if (pid == GetCurrentProcessId() && IsWindowVisible(window) && !GetWindow(window, GW_OWNER) &&
        GetClientRect(window, &r) && r.right >= 640 && r.bottom >= 480) {
        gameWindow = window;
        return FALSE;
    }
    return TRUE;
}
static DWORD ConfigureWindow(void *unused)
{
    WCHAR path[MAX_PATH];
    int width, height, full;
    RECT r;
    DWORD style, exstyle;
    lstrcpyW(path, directory); lstrcatW(path, L"display.ini");
    width = GetPrivateProfileIntW(L"Display", L"Width", 800, path);
    height = GetPrivateProfileIntW(L"Display", L"Height", 600, path);
    full = GetPrivateProfileIntW(L"Display", L"Fullscreen", 0, path);
    EnumWindows(FindGame, 0);
    if (!gameWindow) return 0;
    if (width < 800 || width > 3840 || height < 600 || height > 2160) { width = 800; height = 600; }
    originalProc = (WNDPROC)SetWindowLongA(gameWindow, GWL_WNDPROC, (LONG)GameProc);
    style = GetWindowLongA(gameWindow, GWL_STYLE);
    exstyle = GetWindowLongA(gameWindow, GWL_EXSTYLE);
    if (full) {
        MONITORINFO monitor;
        monitor.cbSize = sizeof(monitor);
        GetMonitorInfoA(MonitorFromWindow(gameWindow, MONITOR_DEFAULTTONEAREST), &monitor);
        SetWindowLongA(gameWindow, GWL_STYLE, (style & ~WS_OVERLAPPEDWINDOW) | WS_POPUP);
        SetWindowPos(gameWindow, NULL, monitor.rcMonitor.left, monitor.rcMonitor.top,
            monitor.rcMonitor.right - monitor.rcMonitor.left, monitor.rcMonitor.bottom - monitor.rcMonitor.top,
            SWP_NOZORDER | SWP_FRAMECHANGED);
    } else {
        r.left = r.top = 0; r.right = width; r.bottom = height;
        AdjustWindowRectEx(&r, style, FALSE, exstyle);
        SetWindowPos(gameWindow, NULL, 0, 0, r.right-r.left, r.bottom-r.top, SWP_NOMOVE | SWP_NOZORDER);
    }
    return 0;
}
typedef HRESULT (WINAPI *BltFunction)(void *, RECT *, void *, RECT *, DWORD, void *);
typedef HRESULT (WINAPI *CreateSurfaceFunction)(void *, void *, void **, void *);
static BltFunction originalBlt;
static CreateSurfaceFunction originalCreateSurface;
static void *primarySurface;
static HRESULT WINAPI ScaledBlt(void *surface, RECT *destination, void *source, RECT *sourceRect, DWORD flags, void *fx)
{
    RECT target;
    if (!gameWindow && surface == primarySurface) ConfigureWindow(NULL);
    if (surface == primarySurface && source && destination && gameWindow &&
        destination->right-destination->left == 800 && destination->bottom-destination->top == 600) {
        POINT origin = {0, 0};
        GetClientRect(gameWindow, &target);
        ClientToScreen(gameWindow, &origin);
        OffsetRect(&target, origin.x, origin.y);
        return originalBlt(surface, &target, source, sourceRect, flags, fx);
    }
    return originalBlt(surface, destination, source, sourceRect, flags, fx);
}
static void *ReplaceMethod(void *object, int index, void *replacement)
{
    void **table = *(void ***)object;
    DWORD protection;
    void *original = table[index];
    if (original == replacement) return NULL;
    if (!VirtualProtect(table+index, sizeof(void *), PAGE_READWRITE, &protection)) return NULL;
    table[index] = replacement;
    VirtualProtect(table+index, sizeof(void *), protection, &protection);
    return original;
}
static HRESULT WINAPI CreateSurface(void *draw, void *description, void **surface, void *outer)
{
    HRESULT result = originalCreateSurface(draw, description, surface, outer);
    /* DDSURFACEDESC and DDSURFACEDESC2 share the caps offset (104). */
    if (SUCCEEDED(result) && *(DWORD *)description >= 108 && (*(DWORD *)((char *)description+104) & 0x200)) {
        void *method;
        primarySurface = *surface;
        method = ReplaceMethod(*surface, 5, ScaledBlt);
        if (method) originalBlt = (BltFunction)method;
    }
    return result;
}
static FARPROC Resolve(const char *name)
{
    if (!renderer) {
        WCHAR path[MAX_PATH];
        lstrcpyW(path, directory); lstrcatW(path, L"nanaimo-renderer.dll");
        renderer = LoadLibraryW(path);
        if (!renderer) return NULL;

    }
    return GetProcAddress(renderer, name);
}
__declspec(dllexport) HRESULT WINAPI DirectDrawCreateEx(void *guid, void **out, const void *iid, void *outer)
{
    typedef HRESULT (WINAPI *Function)(void *, void **, const void *, void *);
    Function fn = (Function)Resolve("DirectDrawCreateEx");
    HRESULT result = fn ? fn(guid, out, iid, outer) : E_FAIL;
    if (SUCCEEDED(result)) {
        void *method = ReplaceMethod(*out, 6, CreateSurface);
        if (method) originalCreateSurface = (CreateSurfaceFunction)method;
    }
    return result;
}
__declspec(dllexport) HRESULT WINAPI DirectDrawEnumerateExA(void *callback, void *context, DWORD flags)
{
    typedef HRESULT (WINAPI *Function)(void *, void *, DWORD);
    Function fn = (Function)Resolve("DirectDrawEnumerateExA");
    return fn ? fn(callback, context, flags) : E_FAIL;
}
BOOL WINAPI DllMain(HINSTANCE instance, DWORD reason, void *reserved)
{
    if (reason == DLL_PROCESS_ATTACH) {
        WCHAR *last;
        DWORD length = GetModuleFileNameW(instance, directory, MAX_PATH);
        if (!length || length >= MAX_PATH - 32) return FALSE;
        last = wcsrchr(directory, L'\\');
        if (!last) return FALSE;
        last[1] = 0;
        DisableThreadLibraryCalls(instance);
    }
    return TRUE;
}
