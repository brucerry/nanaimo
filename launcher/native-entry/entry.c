#define UNICODE
#define _UNICODE
#include <windows.h>
#include <wchar.h>

#define PATH_CAPACITY 32768
typedef LONG (WINAPI *VersionQuery)(OSVERSIONINFOEXW *);
typedef VOID (WINAPI *NativeSystemQuery)(LPSYSTEM_INFO);
typedef WCHAR **(WINAPI *CommandLineParser)(const WCHAR *, int *);

static int locate(WCHAR *target, const WCHAR *directory, const WCHAR *relative)
{
    DWORD attributes;
    if (wcslen(directory) + wcslen(relative) + 2 >= PATH_CAPACITY) return 0;
    wcscpy(target, directory);
    wcscat(target, L"\\");
    wcscat(target, relative);
    attributes = GetFileAttributesW(target);
    return attributes != INVALID_FILE_ATTRIBUTES && !(attributes & FILE_ATTRIBUTE_DIRECTORY);
}

int WINAPI WinMain(HINSTANCE instance, HINSTANCE previous, LPSTR arguments, int show)
{
    static WCHAR directory[PATH_CAPACITY], executable[PATH_CAPACITY];
    static WCHAR command[PATH_CAPACITY + 32], working[PATH_CAPACITY];
    WCHAR variant[64], server[128], sibling[128], direct[128];
    OSVERSIONINFOEXW version = { sizeof(OSVERSIONINFOEXW) };
    SYSTEM_INFO system;
    VersionQuery query = (VersionQuery)GetProcAddress(GetModuleHandleW(L"ntdll.dll"), "RtlGetVersion");
    NativeSystemQuery systemQuery = (NativeSystemQuery)GetProcAddress(GetModuleHandleW(L"kernel32.dll"), "GetNativeSystemInfo");
    int legacy, x86;
    int count, i;
    WCHAR **options;
    HMODULE shell = LoadLibraryW(L"shell32.dll");
    CommandLineParser parse = shell ? (CommandLineParser)GetProcAddress(shell, "CommandLineToArgvW") : NULL;
    STARTUPINFOW startup = { sizeof(STARTUPINFOW) };
    PROCESS_INFORMATION process;
    WCHAR *separator;
    DWORD length = GetModuleFileNameW(NULL, directory, PATH_CAPACITY);
    if (!systemQuery) return 1;
    systemQuery(&system);
    if (system.wProcessorArchitecture != PROCESSOR_ARCHITECTURE_AMD64 &&
        system.wProcessorArchitecture != PROCESSOR_ARCHITECTURE_INTEL) {
        MessageBoxW(NULL, L"\u76ee\u524d\u53ea\u652f\u63f4 x86 \u6216 x64 Windows\u3002", L"Nanaimo\uff0d\u62ff\u62ff\u8da3", MB_OK | MB_ICONERROR);
        return 1;
    }
    if (!query || query(&version) != 0) return 1;
    legacy = version.dwMajorVersion < 10;
    x86 = system.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_INTEL;
    options = parse ? parse(GetCommandLineW(), &count) : NULL;
    if (options) {
        for (i = 1; i < count; i++) {
            if (wcscmp(options[i], L"--win7") == 0) legacy = 1;
            if (wcscmp(options[i], L"--x86") == 0) x86 = 1;
        }
        LocalFree(options);
    }
    if (shell) FreeLibrary(shell);
    if (version.dwMajorVersion == 6 && version.dwMinorVersion == 1 &&
        (version.wServicePackMajor < 1 || !GetProcAddress(GetModuleHandleW(L"kernel32.dll"), "AddDllDirectory"))) {
        MessageBoxW(NULL, L"Windows 7 \u9700\u8981 Service Pack 1 \u53ca\u7cfb\u7d71\u66f4\u65b0 KB3063858\uff08\u6216 KB2533623\uff09\u3002",
            L"Nanaimo\uff0d\u62ff\u62ff\u8da3", MB_OK | MB_ICONERROR);
        return 1;
    }
    wcscpy(variant, legacy ? L"launcher-win7" : L"launcher");
    if (x86) wcscat(variant, L"-x86");
    wcscpy(server, L"server\\"); wcscat(server, variant); wcscat(server, L"\\Nanaimo.Launcher.exe");
    wcscpy(sibling, L"..\\server\\"); wcscat(sibling, variant); wcscat(sibling, L"\\Nanaimo.Launcher.exe");
    wcscpy(direct, variant); wcscat(direct, L"\\Nanaimo.Launcher.exe");
    if (!length || length >= PATH_CAPACITY) return 1;
    separator = wcsrchr(directory, L'\\');
    if (!separator) return 1;
    *separator = 0;
    if (!locate(executable, directory, server) &&
        !locate(executable, directory, direct) &&
        !locate(executable, directory, sibling)) {
        MessageBoxW(NULL,
            L"\u627e\u4e0d\u5230\u555f\u52d5\u5668\u3002\u8acb\u5c07\u5b8c\u6574\u7684 server \u8cc7\u6599\u593e\u653e\u5728\u904a\u6232\u76ee\u9304\u5167\u6216\u65c1\u908a\u3002",
            L"Nanaimo", MB_OK | MB_ICONERROR);
        return 1;
    }
    wcscpy(working, executable);
    *wcsrchr(working, L'\\') = 0;
    wcscpy(command, L"\"");
    wcscat(command, executable);
    wcscat(command, L"\" --server-ui");
    if (!CreateProcessW(executable, command, NULL, NULL, FALSE, 0, NULL, working, &startup, &process)) {
        WCHAR *message = NULL;
        FormatMessageW(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
            NULL, GetLastError(), 0, (WCHAR *)&message, 0, NULL);
        MessageBoxW(NULL, message ? message : L"\u7121\u6cd5\u555f\u52d5\u904a\u6232\u555f\u52d5\u5668\u3002", L"Nanaimo", MB_OK | MB_ICONERROR);
        if (message) LocalFree(message);
        return 1;
    }
    CloseHandle(process.hThread);
    CloseHandle(process.hProcess);
    return 0;
}
