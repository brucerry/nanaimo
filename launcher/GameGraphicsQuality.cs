using System.Runtime.InteropServices;

namespace Nanaimo.Launcher;

internal static class GameGraphicsQuality
{
    // dgVoodoo 2.87 accepts these sample counts. Check both color channel
    // orders and depth/stencil so a color-only capability does not overpromise.
    internal static int SelectMaxSamples(Func<int, int, bool> supports)
    {
        foreach (int samples in new[] { 16, 8, 4, 2 })
            if (new[] { 28, 87, 45 }.All(format => supports(format, samples)))
                return samples;
        return 0;
    }

    internal static int DetectMaxSamples()
    {
        IntPtr device = IntPtr.Zero, context = IntPtr.Zero;
        try
        {
            // Match OutputAPI=d3d11_fl10_0 and Adapters=1 in RendererOptions.
            // A null adapter with HARDWARE uses the default (first) adapter.
            int result = D3D11CreateDevice(IntPtr.Zero, 1, IntPtr.Zero, 0,
                new[] { 0xa000 }, 1, 7, out device, out _, out context);
            if (result < 0 || device == IntPtr.Zero) return 0;
            // ID3D11Device slot 30: CheckMultisampleQualityLevels (IUnknown included).
            IntPtr method = Marshal.ReadIntPtr(Marshal.ReadIntPtr(device), 30 * IntPtr.Size);
            var check = Marshal.GetDelegateForFunctionPointer<CheckMultisampleQualityLevels>(method);
            return SelectMaxSamples((format, samples) =>
                check(device, format, samples, out uint levels) >= 0 && levels > 0);
        }
        catch (Exception error) when (error is DllNotFoundException or EntryPointNotFoundException or BadImageFormatException)
        {
            // A missing/unavailable hardware API must not prevent local login.
            return 0;
        }
        finally
        {
            if (context != IntPtr.Zero) Marshal.Release(context);
            if (device != IntPtr.Zero) Marshal.Release(device);
        }
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate int CheckMultisampleQualityLevels(IntPtr device, int format, int samples, out uint levels);

    [DllImport("d3d11.dll", CallingConvention = CallingConvention.StdCall)]
    private static extern int D3D11CreateDevice(IntPtr adapter, int driverType, IntPtr software, uint flags,
        int[] featureLevels, uint featureLevelCount, uint sdkVersion, out IntPtr device,
        out int featureLevel, out IntPtr immediateContext);
}
