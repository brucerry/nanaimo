using System.Text.Json;

namespace Nanaimo.Launcher;

internal static class ClientLocator
{
    internal static string Find(string serverRoot)
    {
        serverRoot = Path.GetFullPath(serverRoot);
        string parent = Directory.GetParent(serverRoot)?.FullName ?? serverRoot;
        // Local placement takes precedence over a config copied from another layout.
        foreach (string directory in new[] { serverRoot, parent })
        {
            string client = Path.Combine(directory, "game.exe");
            if (File.Exists(client)) return client;
        }
        using var settings = JsonDocument.Parse(File.ReadAllText(Path.Combine(serverRoot, "launchsettings.json")));
        if (settings.RootElement.TryGetProperty("client", out var value) && value.GetString() is { Length: > 0 } configured)
        {
            string client = Path.GetFullPath(Path.Combine(serverRoot, configured));
            if (!client.StartsWith(parent.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                throw new InvalidDataException("遊戲用戶端路徑一定要位於遊戲或專案目錄內。");
            if (File.Exists(client)) return client;
        }
        string sibling = Path.Combine(parent, "client", "game.exe");
        if (File.Exists(sibling)) return sibling;
        throw new FileNotFoundException("搵唔到 game.exe。請將完整的 server 資料夾放在遊戲目錄內。", Path.Combine(parent, "game.exe"));
    }
}
