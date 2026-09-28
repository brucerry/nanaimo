using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace Nanaimo.Launcher;

internal static class Program
{
    private static string root = "";

    [STAThread]
    private static int Main(string[] args)
    {
        System.Globalization.CultureInfo.DefaultThreadCurrentUICulture =
            System.Globalization.CultureInfo.GetCultureInfo("zh-TW");
        try
        {
            root = FindRoot();
            if (args.Contains("--server-ui"))
            {
                string identity = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(root.ToUpperInvariant())))[..16];
                using var mutex = new Mutex(true, "Local\\NanaimoServerManager-" + identity, out bool first);
                if (!first) { ActivateManager(); return 0; }
                ApplicationConfiguration.Initialize();
                Application.Run(new ServerManagerForm(root));
                return 0;
            }
            if (args.Length == 0)
            {
                var manager = new ProcessStartInfo(Environment.ProcessPath!) { UseShellExecute = false };
                manager.ArgumentList.Add("--server-ui");
                Process.Start(manager)?.Dispose();
            }
            return RunAsync(args).GetAwaiter().GetResult();
        }
        catch (Exception error)
        {
            if (root.Length > 0)
                File.AppendAllText(Path.Combine(root, "launcher.log"), $"{DateTimeOffset.Now:O} ERROR {error}\n");
            MessageBox.Show(error.Message, "Nanaimo 啟動器", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return 1;
        }
    }

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr window, int command);
    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr window);

    private static void ActivateManager()
    {
        foreach (var process in Process.GetProcessesByName("Nanaimo.Launcher"))
        {
            using (process)
            {
                try
                {
                    if (process.Id != Environment.ProcessId && process.MainWindowHandle != IntPtr.Zero &&
                        string.Equals(ProcessPaths.Executable(process.Id), Path.GetFullPath(Environment.ProcessPath!), StringComparison.OrdinalIgnoreCase))
                    {
                        ShowWindow(process.MainWindowHandle, 9);
                        SetForegroundWindow(process.MainWindowHandle);
                        return;
                    }
                }
                catch (System.ComponentModel.Win32Exception) { }
                catch (InvalidOperationException) { }
            }
        }
    }

    private static string FindRoot()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory != null; directory = directory.Parent)
        {
            if (File.Exists(Path.Combine(directory.FullName, "launchsettings.json")))
                return directory.FullName;
            string runtime = Path.Combine(directory.FullName, "server");
            if (File.Exists(Path.Combine(runtime, "launchsettings.json"))) return runtime;
            runtime = Path.Combine(directory.FullName, "client", "server");
            if (File.Exists(Path.Combine(runtime, "launchsettings.json"))) return runtime;
        }
        throw new FileNotFoundException("搵唔到啟動器上層目錄中的 launchsettings.json。");
    }

    internal static async Task<int> RunAsync(string[] args)
    {
        string client = ClientLocator.Find(root);
        bool merged = IsMergedServer(root);
        string server = ServerPath(root);
        string profile = Resolve("config/profile.ini");
        void VerifyLaunchFiles()
        {
            if (!File.Exists(client)) throw new FileNotFoundException("搵唔到遊戲用戶端。", client);
            if (!File.Exists(server)) throw new FileNotFoundException("搵唔到伺服器程式。", server);
            string native = Path.Combine(Path.GetDirectoryName(server)!, "nanaimo_gameplay_bridge.exe");
            if (!File.Exists(native)) throw new FileNotFoundException("搵唔到遊戲橋接程式。", native);
            if (!File.Exists(profile)) throw new FileNotFoundException("搵唔到伺服器設定檔。", profile);
        }
        if (Application.MessageLoop) await Task.Run(VerifyLaunchFiles);
        else VerifyLaunchFiles();
        if (args.Contains("--verify"))
        {
            Log("VERIFY_PASS 啟動所需檔案完整。");
            return 0;
        }
        if (args.Contains("--stop-server"))
        {
            if (MatchingProcess(client) != null)
                throw new InvalidOperationException("請先閂埋遊戲，再停伺服器。");
            using var owned = MatchingProcess(server);
            if (owned != null)
            {
                if (merged)
                {
                    await File.WriteAllTextAsync(Resolve("server-merged/data/stop.request"), "stop");
                    using var stopTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(20));
                    await owned.WaitForExitAsync(stopTimeout.Token);
                }
                else { owned.Kill(); await owned.WaitForExitAsync(); }
            }
            Log("伺服器已停止。");
            return 0;
        }
        using var existingClient = MatchingProcess(client);
        if (existingClient != null && !args.Contains("--server-only"))
        {
            Log($"遊戲已在執行，處理程序編號為 {existingClient.Id}。");
            return 0;
        }
        string? localAccount = null;
        if (merged && !args.Contains("--server-only") && !args.Contains("--original-profile"))
        {
            int accountOption = Array.IndexOf(args, "--account");
            if (accountOption >= 0)
            {
                if (accountOption + 1 >= args.Length) throw new ArgumentException("缺少 --account 帳號參數。");
                localAccount = args[accountOption + 1];
            }
            else
            {
                using var login = new AccountLoginForm(root);
                if (login.ShowDialog() != DialogResult.OK) return 0;
                localAccount = login.AccountName;
            }
        }
        using var existingServer = MatchingProcess(server);
        if (existingServer == null)
        {
            foreach (int port in ServerPorts(root))
                if (await PortOpen(port)) throw new InvalidOperationException($"連接埠 {port} 已被其他處理程序佔用。");
            var start = new ProcessStartInfo(server) { UseShellExecute = false, CreateNoWindow = true, WorkingDirectory = root };
            foreach (string value in new[] { "--data", Resolve("server-merged/data"),
                "--native", Path.Combine(Path.GetDirectoryName(server)!, "nanaimo_gameplay_bridge.exe"),
                "--profile", profile, "--login-port", "11005", "--world-port", "12050", "--profile-port", "11999",
                "--log-directory", Resolve("server-merged") })
                start.ArgumentList.Add(value);
            using var started = Process.Start(start) ?? throw new InvalidOperationException("唔能夠開伺服器。");
            await Task.Delay(700);
            if (started.HasExited) throw new InvalidOperationException("伺服器啟動失敗，請睇下 server-merged/server-error.log。");
        }
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        while (!await PortOpen(11005) || !await PortOpen(11999) || !await PortOpen(12050))
            await Task.Delay(200, timeout.Token);
        if (args.Contains("--server-only")) { Log("伺服器已就緒。"); return 0; }

        byte[] profileBytes = localAccount is null ? await File.ReadAllBytesAsync(profile)
            : JsonSerializer.SerializeToUtf8Bytes(new { LocalAccount = localAccount });
        using (var registration = new TcpClient())
        using (var registerTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
        {
            await registration.ConnectAsync(IPAddress.Loopback, 11999, registerTimeout.Token);
            var stream = registration.GetStream();
            await stream.WriteAsync(BitConverter.GetBytes((uint)profileBytes.Length), registerTimeout.Token);
            await stream.WriteAsync(profileBytes, registerTimeout.Token);
            byte[] response = new byte[3];
            await stream.ReadExactlyAsync(response, registerTimeout.Token);
            if (!response.AsSpan().SequenceEqual("OK\n"u8)) throw new IOException("帳號登入失敗，請睇下伺服器記錄。");
        }
        string clientDirectory = Path.GetDirectoryName(client)!;
        string optionPath = Path.Combine(clientDirectory, "StateOption", "gamestartoption.ini");
        Directory.CreateDirectory(Path.GetDirectoryName(optionPath)!);
        await File.WriteAllTextAsync(optionPath,
            "[GameInfo]\r\nFullScreen=0\r\nUsePatch=0\r\nScreenWidth=800\r\nScreenHeight=600\r\n" +
            "[ServerInfo]\r\nPort=12050\r\nServerIP=127.0.0.1\r\n" +
            "[LoginInfo]\r\nLogin=Network Login Game\r\n[NexonPlugInfo]\r\nID=123\r\nPW=123\r\n", Encoding.ASCII);
        var gameStart = new ProcessStartInfo(client) { WorkingDirectory = clientDirectory, UseShellExecute = false };
        foreach (string argument in new[] { "-q", ":1:1:0:3:4:-i", "5:-r", "6:7:1:127.0.0.1:" })
            gameStart.ArgumentList.Add(argument);
        using var game = Process.Start(gameStart) ?? throw new InvalidOperationException("唔能夠啟動遊戲。");
        await SetGameTitleAsync(game);
        Log($"遊戲已啟動，處理程序編號為 {game.Id}；角色資料已註冊，而家喺度載入遊戲。");
        return 0;
    }

    internal static bool IsMergedServer(string projectRoot)
    {
        using var settings = JsonDocument.Parse(File.ReadAllText(Path.Combine(projectRoot, "launchsettings.json")));
        if (!settings.RootElement.TryGetProperty("serverMode", out var mode) || mode.GetString() != "merged")
            throw new InvalidDataException("本專案僅支援整合伺服器。");
        return true;
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool SetWindowTextW(IntPtr window, string text);

    private static async Task SetGameTitleAsync(Process game)
    {
        // The legacy ANSI caption uses the system code page. Set it through
        // the Unicode API so Traditional Chinese Windows does not show mojibake.
        for (int attempt = 0; attempt < 50; attempt++)
        {
            if (game.HasExited) return;
            game.Refresh();
            if (game.MainWindowHandle != IntPtr.Zero)
            {
                SetWindowTextW(game.MainWindowHandle, "Nanaimo－拿拿趣");
                return;
            }
            await Task.Delay(100);
        }
    }

    internal static bool LegacyRuntime => Environment.Version.Major < 8;

    internal static string ServerPath(string projectRoot) => Path.GetFullPath(Path.Combine(projectRoot,
        "server-merged", (LegacyRuntime ? "bin-win7" : "bin") + (Environment.Is64BitProcess ? "" : "-x86"),
        "Nanaimo.Server.exe"));

    internal static int[] ServerPorts(string projectRoot) => IsMergedServer(projectRoot)
        ? [11005, 11999, 12050, 22051, 22052, 22053, 22054, 51005, 51999, 52050, 62050, 62051, 62052, 62053, 62054, 62055]
        : [11005, 11999, 12050, 32050, 32051, 32052, 32053, 32054, 32055];

    private static string Resolve(string relative)
    {
        string path = Path.GetFullPath(Path.Combine(root, relative));
        string workspace = Path.GetFileName(root) == "server" ? Directory.GetParent(root)!.FullName : root;
        if (!path.StartsWith(workspace + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("設定的路徑一定要位於專案目錄內。");
        return path;
    }

    private static Process? MatchingProcess(string executable)
    {
        foreach (var process in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(executable)))
        {
            try
            {
                if (string.Equals(ProcessPaths.Executable(process.Id), executable, StringComparison.OrdinalIgnoreCase))
                    return process;
            }
            catch (System.ComponentModel.Win32Exception) { }
            catch (InvalidOperationException) { }
            process.Dispose();
        }
        return null;
    }

    private static async Task<bool> PortOpen(int port)
    {
        using var client = new TcpClient();
        using var timeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(250));
        try { await client.ConnectAsync(IPAddress.Loopback, port, timeout.Token); return true; }
        catch (SocketException) { return false; }
        catch (OperationCanceledException) { return false; }
    }

    private static void Log(string text) => File.AppendAllText(Path.Combine(root, "launcher.log"), $"{DateTimeOffset.Now:O} {text}\n");
}
