using System.Text.Json;
using Nanaimo.Launcher;

internal static class Checks
{
    [STAThread]
    private static void Main(string[] args)
    {
        string output = Path.GetFullPath(args[0]);
        if (!string.Equals(ProcessPaths.Executable(Environment.ProcessId), Environment.ProcessPath, StringComparison.OrdinalIgnoreCase))
            throw new Exception("Native process image lookup failed.");
        if (ProcessPaths.Executable(int.MaxValue) is not null) throw new Exception("Missing process lookup failed.");
        var clock = System.Diagnostics.Stopwatch.StartNew();
        for (int i = 0; i < 10; i++)
            foreach (var process in System.Diagnostics.Process.GetProcessesByName("Nanaimo.Server"))
                using (process) ProcessPaths.Executable(process.Id);
        Console.WriteLine("PROCESS_SCAN_AVERAGE_MS=" + clock.Elapsed.TotalMilliseconds / 10);
        string Fixture(string name, string client)
        {
            string root = Path.Combine(output, name, "Server");
            Directory.CreateDirectory(root);
            File.WriteAllText(Path.Combine(root, "launchsettings.json"), JsonSerializer.Serialize(new { client, serverMode = "merged" }));
            return root;
        }
        string Game(string path)
        {
            path = Path.GetFullPath(path);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllBytes(path, []);
            return path;
        }
        void Check(bool value, string message)
        {
            if (!value) throw new InvalidOperationException(message);
            Console.WriteLine("CHECK_PASS " + message);
        }
        string root = Fixture("sibling", "../client/game.exe");
        string expected = Game(Path.Combine(root, "../client/game.exe"));
        Check(ClientLocator.Find(root) == expected, "Existing sibling-client layout");
        root = Fixture("portable folder with spaces", "../client/game.exe");
        expected = Game(Path.Combine(root, "../game.exe"));
        Game(Path.Combine(root, "../client/game.exe"));
        Check(ClientLocator.Find(root) == expected, "Portable server folder prefers adjacent game over stale config");
        root = Fixture("same folder", "../missing/game.exe");
        expected = Game(Path.Combine(root, "game.exe"));
        Check(ClientLocator.Find(root) == expected, "Server and game in same directory");
        root = Fixture("renamed", "../custom/game.exe");
        expected = Game(Path.Combine(root, "../custom/game.exe"));
        Check(ClientLocator.Find(root) == expected, "Configured client with renamed server folder");
        root = Fixture("missing", "../missing/game.exe");
        try { ClientLocator.Find(root); throw new Exception("Missing game was accepted"); }
        catch (FileNotFoundException) { Console.WriteLine("CHECK_PASS Missing game has actionable error"); }
        root = Fixture("escape", "../../outside/game.exe");
        try { ClientLocator.Find(root); throw new Exception("Escaping config was accepted"); }
        catch (InvalidDataException) { Console.WriteLine("CHECK_PASS Config cannot escape game directory"); }

        ApplicationConfiguration.Initialize();
        root = Fixture("account form", "../game.exe");
        using var login = new AccountLoginControl(root);
        IEnumerable<Control> Descendants(Control control) => control.Controls.Cast<Control>()
            .SelectMany(child => new[] { child }.Concat(Descendants(child)));
        var controls = Descendants(login).ToArray();
        controls.Single(control => control.AccessibleName == "Login account").Text = "  Test Account  ";
        Check(controls.OfType<TextBox>().Count() == 1 && !controls.OfType<RadioButton>().Any(),
            "Launcher contains account only, without character creation controls");
        bool requested = false;
        login.LoginRequested += (_, _) => requested = true;
        login.Submit();
        Check(requested && login.Arguments.SequenceEqual(new[] { "--account", "Test Account" }),
            "Embedded login forwards only the trimmed account");
        var selector = controls.OfType<ComboBox>().Single(control => control.AccessibleName == "視窗解像度");
        Check(selector.Items.Count >= 8 && selector.SelectedIndex == 0,
            "Login offers common resolutions and defaults to 800 by 600");
        selector.SelectedIndex = Array.IndexOf(GameDisplaySettings.Presets, new GameResolution(1920, 1080));
        login.Submit();
        var fullScreen = controls.OfType<CheckBox>().Single(control => control.AccessibleName == "全螢幕");
        fullScreen.Checked = true;
        login.Submit();
        Check(GameDisplaySettings.Load(root) == new GameDisplayMode(1920, 1080, true) &&
            GameDisplaySettings.GameOptions(GameDisplaySettings.Load(root)).Contains("ScreenWidth=800\r\nScreenHeight=600\r\n") &&
            GameDisplaySettings.WrapperOptions(GameDisplaySettings.Load(root)).Contains("Fullscreen=1\r\n") &&
            !selector.Enabled,
            "Fullscreen uses the native game surface and the display's full screen");
        Check(GameDisplaySettings.WrapperOptions(new GameDisplayMode(1280, 720, false))
            .Contains("Width=1280\r\nHeight=720\r\n") &&
            GameDisplaySettings.WrapperOptions(new GameDisplayMode(1280, 720, false)).Contains("Fullscreen=0\r\n"),
            "Windowed mode scales the native game surface to the selected preset");
        Check(GameDisplaySettings.RendererOptions(8).Contains("Resolution = unforced\r\n"),
            "Renderer preserves native GDI text coordinates before the final frame is scaled");
        Check(GameGraphicsQuality.SelectMaxSamples((format, count) => count <= 16) == 16 &&
            GameGraphicsQuality.SelectMaxSamples((format, count) => count <= 8) == 8 &&
            GameGraphicsQuality.SelectMaxSamples((format, count) => count <= 4) == 4 &&
            GameGraphicsQuality.SelectMaxSamples((format, count) => count <= 2) == 2 &&
            GameGraphicsQuality.SelectMaxSamples((format, count) => false) == 0,
            "Automatic AA selects the highest supported renderer sample count or disables unsupported MSAA");
        Check(GameGraphicsQuality.SelectMaxSamples((format, count) => format != 45 || count <= 2) == 2,
            "Automatic AA requires compatible color and depth formats");
        Check(GameDisplaySettings.RendererOptions(8).Contains("Antialiasing = 8x\r\n") &&
            GameDisplaySettings.RendererOptions(8).Contains("Bilinear2DOperations = true\r\n") &&
            GameDisplaySettings.RendererOptions(0).Contains("Antialiasing = off\r\n"),
            "AA and final-frame smoothing are generated independently");
        Console.WriteLine($"HARDWARE_MSAA_SAMPLES={GameGraphicsQuality.DetectMaxSamples()}");
        using var reopened = new AccountLoginControl(root);
        Check(reopened.AccountName == "Test Account" &&
            reopened.Controls.OfType<TableLayoutPanel>().SelectMany(control => control.Controls.OfType<ComboBox>())
                .Single().SelectedIndex == selector.SelectedIndex &&
            Descendants(reopened).OfType<CheckBox>().Single().Checked,
            "Standalone and embedded login share saved account and display mode");
        File.WriteAllText(Path.Combine(root, "launcher", "display-settings.json"), "{\"Width\":123,\"Height\":456}");
        Check(GameDisplaySettings.Load(root) == GameDisplaySettings.Default,
            "Unsupported saved resolution falls back to 800 by 600");
        Check(GameDisplaySettings.GameOptions(GameDisplaySettings.Default).Contains("FullScreen=0\r\n"),
            "Default game mode starts windowed");
        using var form = new Form { ClientSize = new Size(740, 420) };
        using var rendered = new AccountLoginControl(root) { Size = new Size(480, 260), Location = new Point(4, 4) };
        form.Controls.Add(rendered);
        form.Show();
        Application.DoEvents();
        Check(rendered.LoginButton.Visible && rendered.LoginButton.Height > 20 &&
            rendered.RectangleToScreen(rendered.ClientRectangle).Contains(rendered.LoginButton.RectangleToScreen(rendered.LoginButton.ClientRectangle)),
            "Login command is visible and contained at compact window size");
        form.Close();
        string resetRoot = Fixture("save reset", "../game.exe");
        string dataDirectory = Path.Combine(resetRoot, "server-merged", "data");
        Directory.CreateDirectory(dataDirectory);
        File.WriteAllText(Path.Combine(dataDirectory, "game.db"), "test-save");
        File.WriteAllText(Path.Combine(dataDirectory, "game.db-wal"), "pending-test-save");
        bool refused = false;
        try { SaveReset.Archive(resetRoot, () => true); }
        catch (InvalidOperationException) { refused = true; }
        Check(refused && Directory.Exists(dataDirectory), "Clear save refuses running game or server without touching data");
        string backup = SaveReset.Archive(resetRoot, () => false);
        Check(!Directory.Exists(dataDirectory) && File.ReadAllText(Path.Combine(backup, "game.db")) == "test-save"
            && File.ReadAllText(Path.Combine(backup, "game.db-wal")) == "pending-test-save"
            && File.Exists(Path.Combine(resetRoot, "launchsettings.json")), "Clear save preserves complete backup and launch configuration");
        Console.WriteLine("LAUNCHER_CHECKS_PASS");
    }
}
