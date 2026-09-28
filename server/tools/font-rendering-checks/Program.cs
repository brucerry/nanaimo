using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

internal static class FontRenderingChecks
{
    [DllImport("gdi32.dll")]
    private static extern IntPtr SelectObject(IntPtr dc, IntPtr obj);
    [DllImport("gdi32.dll", EntryPoint = "TextOutA")]
    private static extern bool TextOutAnsi(IntPtr dc, int x, int y, byte[] text, int length);
    [DllImport("gdi32.dll", EntryPoint = "TextOutW", CharSet = CharSet.Unicode)]
    private static extern bool TextOutUnicode(IntPtr dc, int x, int y, string text, int length);
    [DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr obj);

    private static Bitmap Render(byte charset, bool unicode)
    {
        var bitmap = new Bitmap(600, 40);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);
        using var font = new Font("SimSun", 18, FontStyle.Regular, GraphicsUnit.Pixel, charset);
        IntPtr fontHandle = font.ToHfont();
        IntPtr dc = graphics.GetHdc();
        IntPtr previous = SelectObject(dc, fontHandle);
        try
        {
            const string text = "\u64b3\u4f4f Ctrl \u9375\uff0c\u518d\u7528\u65b9\u5411\u9375\u90c1\u52d5\u3002\u63c0\u5497\u5bf5\u7269\uff0c\u64b3\u597d\u5440\u3002";
            byte[] bytes = Encoding.GetEncoding(936).GetBytes(text);
            bool success = unicode
                ? TextOutUnicode(dc, 0, 0, text, text.Length)
                : TextOutAnsi(dc, 0, 0, bytes, bytes.Length);
            if (!success) throw new InvalidOperationException("GDI text rendering failed.");
        }
        finally
        {
            SelectObject(dc, previous);
            graphics.ReleaseHdc(dc);
            DeleteObject(fontHandle);
        }
        return bitmap;
    }

    private static void Main()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using var original = Render(1, false);
        using var corrected = Render(134, false);
        using var expected = Render(134, true);
        int differences = 0, originalDifferences = 0;
        for (int y = 0; y < expected.Height; y++)
        for (int x = 0; x < expected.Width; x++)
        {
            if (corrected.GetPixel(x, y) != expected.GetPixel(x, y)) differences++;
            if (original.GetPixel(x, y) != expected.GetPixel(x, y)) originalDifferences++;
        }
        Directory.CreateDirectory(".work/localization-checks");
        corrected.Save(".work/localization-checks/charset.png");
        Console.WriteLine($"EXPLICIT_CHARSET_DIFF={differences} DEFAULT_CHARSET_DIFF={originalDifferences}");
        if (differences != 0)
            throw new InvalidOperationException("ANSI rendering differs from the Unicode reference.");
    }
}
