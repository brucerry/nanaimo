using System.Text.Json;

namespace Nanaimo.Launcher;

internal readonly record struct GameResolution(int Width, int Height)
{
    public override string ToString() => $"{Width} × {Height}";
}

internal readonly record struct GameDisplayMode(int Width, int Height, bool FullScreen)
{
    internal GameResolution Resolution => new(Width, Height);
}

internal static class GameDisplaySettings
{
    internal static readonly GameResolution[] Presets =
    [
        new(800, 600), new(1024, 768),
        new(1280, 720), new(1280, 800), new(1366, 768),
        new(1440, 900), new(1600, 900), new(1920, 1080),
        new(1920, 1200), new(2560, 1440), new(3840, 2160)
    ];

    internal static GameDisplayMode Default => new(Presets[0].Width, Presets[0].Height, false);

    internal static GameDisplayMode Load(string root)
    {
        string file = Path.Combine(root, "launcher", "display-settings.json");
        if (!File.Exists(file)) return Default;
        try
        {
            GameDisplayMode saved = JsonSerializer.Deserialize<GameDisplayMode>(File.ReadAllText(file));
            return Presets.Contains(saved.Resolution) ? saved : Default;
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException or JsonException)
        {
            return Default;
        }
    }

    internal static void Save(string root, GameDisplayMode mode)
    {
        if (!Presets.Contains(mode.Resolution)) throw new ArgumentOutOfRangeException(nameof(mode));
        string file = Path.Combine(root, "launcher", "display-settings.json");
        Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        File.WriteAllText(file, JsonSerializer.Serialize(mode));
    }

    internal static string GameOptions(GameDisplayMode mode)
    {
        if (!Presets.Contains(mode.Resolution)) throw new ArgumentOutOfRangeException(nameof(mode));
        // The original renderer draws an 800 x 600 surface regardless of these values.
        // The DirectDraw wrapper scales that surface to the selected display mode.
        return "[GameInfo]\r\nFullScreen=0\r\nUsePatch=0\r\n" +
            "ScreenWidth=800\r\nScreenHeight=600\r\n" +
            "[ServerInfo]\r\nPort=12050\r\nServerIP=127.0.0.1\r\n" +
            "[LoginInfo]\r\nLogin=Network Login Game\r\n[NexonPlugInfo]\r\nID=123\r\nPW=123\r\n";
    }

    internal static string WrapperOptions(GameDisplayMode mode)
    {
        if (!Presets.Contains(mode.Resolution)) throw new ArgumentOutOfRangeException(nameof(mode));
        return "[Display]\r\n" +
            $"Width={mode.Width}\r\nHeight={mode.Height}\r\nFullscreen={(mode.FullScreen ? 1 : 0)}\r\n";
    }

    // GDI text and sprites share the unchanged native surface. Only the final
    // primary-surface blit is resized by our game-specific DirectDraw shim.
    internal static string RendererOptions(int samples)
    {
        if (samples is not (0 or 2 or 4 or 8 or 16)) throw new ArgumentOutOfRangeException(nameof(samples));
        string antialiasing = samples == 0 ? "off" : $"{samples}x";
        return
        "Version = 0x287\r\n[General]\r\nOutputAPI = d3d11_fl10_0\r\nAdapters = 1\r\n" +
        "FullScreenMode = false\r\nCaptureMouse = false\r\n" +
        "[GeneralExt]\r\nFreeMouse = true\r\nSystemHookFlags = gdi, cursor\r\n" +
        "[DirectX]\r\nResolution = unforced\r\nAppControlledScreenMode = false\r\n" +
        $"Antialiasing = {antialiasing}\r\nBilinear2DOperations = true\r\n" +
        "dgVoodooWatermark = false\r\nDisableAltEnterToToggleScreenMode = true\r\n";
    }
}
