using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Nanaimo.Launcher;

internal static class ProcessPaths
{
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern SafeProcessHandle OpenProcess(uint access, bool inherit, int processId);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool QueryFullProcessImageName(SafeProcessHandle process, uint flags, StringBuilder path, ref uint size);

    internal static string? Executable(int processId)
    {
        using var handle = OpenProcess(0x1000, false, processId);
        if (handle.IsInvalid) return null;
        var path = new StringBuilder(32768);
        uint size = (uint)path.Capacity;
        return QueryFullProcessImageName(handle, 0, path, ref size) ? path.ToString() : null;
    }
}
