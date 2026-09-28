param([string]$Screenshot)
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing
Add-Type @'
using System;
using System.Text;
using System.Runtime.InteropServices;
public static class DesktopProbe {
    public delegate bool EnumProc(IntPtr window, IntPtr state);
    [DllImport("user32.dll")] public static extern bool EnumWindows(EnumProc callback, IntPtr state);
    [DllImport("user32.dll")] public static extern bool EnumChildWindows(IntPtr parent, EnumProc callback, IntPtr state);
    [DllImport("user32.dll", CharSet=CharSet.Unicode)] public static extern int GetWindowText(IntPtr window, StringBuilder text, int length);
    [DllImport("user32.dll")] public static extern uint GetWindowThreadProcessId(IntPtr window, out uint pid);
    [DllImport("user32.dll")] public static extern bool IsWindowVisible(IntPtr window);
    [DllImport("user32.dll")] public static extern IntPtr SendMessage(IntPtr window, uint message, IntPtr wparam, IntPtr lparam);
}
'@
$rows = [Collections.Generic.List[object]]::new()
$callback = [DesktopProbe+EnumProc]{param($window,$state)
    [uint32]$processId = 0
    [void][DesktopProbe]::GetWindowThreadProcessId($window,[ref]$processId)
    $process = Get-Process -Id $processId -ErrorAction SilentlyContinue
    if($process -and $process.ProcessName -match '^(game|nanaimo|WerFault)') {
        $text = [Text.StringBuilder]::new(2048)
        [void][DesktopProbe]::GetWindowText($window,$text,$text.Capacity)
        $rows.Add([pscustomobject]@{Window=$window.ToInt64();ProcessId=$processId;Process=$process.ProcessName;Visible=[DesktopProbe]::IsWindowVisible($window);Text=$text.ToString()})
        $child = [DesktopProbe+EnumProc]{param($childWindow,$childState)
            $childText = [Text.StringBuilder]::new(2048)
            [void][DesktopProbe]::GetWindowText($childWindow,$childText,$childText.Capacity)
            if($childText.Length) {$rows.Add([pscustomobject]@{Window=$childWindow.ToInt64();Parent=$window.ToInt64();Text=$childText.ToString()})}
            return $true
        }
        [void][DesktopProbe]::EnumChildWindows($window,$child,[IntPtr]::Zero)
    }
    return $true
}
[void][DesktopProbe]::EnumWindows($callback,[IntPtr]::Zero)
$rows | ConvertTo-Json -Depth 3
if($Screenshot) {
    $rect=[Windows.Forms.SystemInformation]::VirtualScreen
    $bitmap=[Drawing.Bitmap]::new($rect.Width,$rect.Height)
    $graphics=[Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.CopyFromScreen($rect.Location,[Drawing.Point]::Empty,$rect.Size)
        $bitmap.Save([IO.Path]::GetFullPath($Screenshot),[Drawing.Imaging.ImageFormat]::Png)
    } finally {$graphics.Dispose();$bitmap.Dispose()}
}
