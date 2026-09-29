using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace Nanaimo.Launcher;

internal sealed class ServerManagerForm : Form
{
    private readonly string root;
    private readonly Label status = new() { AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
    private readonly Label details = new() { AutoSize = true };
    private readonly Button start = new() { Text = "開伺服器", AutoSize = true };
    private readonly Button stop = new() { Text = "停伺服器", AutoSize = true };
    private readonly Button play = new() { Text = "開始玩", AutoSize = true };
    private readonly Button save = new() { Text = "儲低設定", AutoSize = true };
    private readonly Button clearSave = new() { Text = "重設存檔", AutoSize = true };
    private GmManagementControl? gm;
    private readonly DataGridView ports = new() { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
        AllowUserToDeleteRows = false, RowHeadersVisible = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        BackgroundColor = Color.White, BorderStyle = BorderStyle.None, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
    private readonly RichTextBox log = new() { Dock = DockStyle.Fill, ReadOnly = true, WordWrap = false,
        Font = new Font("Consolas", 9), BackColor = Color.FromArgb(247, 248, 249), BorderStyle = BorderStyle.None };
    private readonly PropertyGrid profile = new() { Dock = DockStyle.Fill, ToolbarVisible = false, HelpVisible = true };
    private readonly System.Windows.Forms.Timer timer = new() { Interval = 1000 };
    private readonly int[] portNumbers;
    private long logOffset;
    private bool busy;
    private bool refreshing;
    private int? observedServerId;
    private ProfileSettings settings;
    private readonly AccountLoginControl login;

    internal ServerManagerForm(string projectRoot)
    {
        root = projectRoot;
        portNumbers = Program.ServerPorts(root);
        Text = Program.IsMergedServer(root) ? "Nanaimo－遊戲與副本管理" : "Nanaimo－本機伺服器";
        Font = new Font("Segoe UI", 9);
        ClientSize = new Size(1000, 680);
        MinimumSize = new Size(760, 520);
        StartPosition = FormStartPosition.CenterScreen;
        try { Icon = Icon.ExtractAssociatedIcon(ClientLocator.Find(root)); }
        catch (FileNotFoundException) { }
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, Padding = new Padding(12) };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.Controls.Add(status, 0, 0);
        layout.Controls.Add(details, 0, 1);
        var commands = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
        var folder = new Button { Text = "伺服器資料夾", AutoSize = true };
        commands.Controls.AddRange([start, stop, play, save, folder, clearSave]);
        layout.Controls.Add(commands, 0, 2);
        var tabs = new TabControl { Dock = DockStyle.Fill };
        var loginTab = new TabPage("本機登入") { Padding = new Padding(4) };
        login = new AccountLoginControl(root) { Size = new Size(480, 260), Location = new Point(4, 4) };
        loginTab.Controls.Add(login);
        var runtime = new TabPage("診斷記錄") { Padding = new Padding(4) };
        var portTab = new TabPage("連接埠") { Padding = new Padding(4) };
        var profileTab = new TabPage("角色設定") { Padding = new Padding(4) };
        runtime.Controls.Add(log); portTab.Controls.Add(ports); profileTab.Controls.Add(profile);
        tabs.TabPages.AddRange([loginTab, runtime, portTab, profileTab]);
        if (Program.IsMergedServer(root))
        {
            var gmTab = new TabPage("管理員工具") { Padding = new Padding(4) };
            gm = new GmManagementControl(root);
            gmTab.Controls.Add(gm);
            tabs.TabPages.Add(gmTab);
        }
        layout.Controls.Add(tabs, 0, 3);
        Controls.Add(layout);
        ports.Columns.Add("service", "服務"); ports.Columns.Add("endpoint", "位址"); ports.Columns.Add("state", "狀態");
        for (int i = 0; i < portNumbers.Length; i++)
            ports.Rows.Add(PortLabel(portNumbers[i]), $"127.0.0.1:{portNumbers[i]}", "已停止");
        settings = new ProfileSettings(Path.Combine(root, "config", "profile.ini"));
        profile.SelectedObject = settings;
        start.Click += async (_, _) => await Command(["--server-only"]);
        stop.Click += async (_, _) => await Command(["--stop-server"]);
        clearSave.Click += async (_, _) => await ClearSaveAsync();
        login.LoginRequested += async (_, _) => await Command(login.Arguments);
        play.Click += (_, _) => { tabs.SelectedTab = loginTab; login.Submit(); };
        folder.Click += (_, _) => Process.Start(new ProcessStartInfo(Path.Combine(root, Program.IsMergedServer(root) ? "server-merged" : "server")) { UseShellExecute = true });
        save.Click += (_, _) =>
        {
            try
            {
                if (ServerId() != null) throw new InvalidOperationException("請先停伺服器，再儲存角色設定。");
                profile.Focus();
                settings.Save();
                MessageBox.Show(this, "設定已儲存。", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception error) { ShowError(error); }
        };
        timer.Tick += async (_, _) => await RefreshStateAsync();
        Shown += async (_, _) => { timer.Start(); login.FocusAccount(); await RefreshStateAsync(); };
        FormClosed += (_, _) => { timer.Stop(); timer.Dispose(); };
    }

    private async Task Command(string[] arguments)
    {
        if (busy) return;
        busy = true;
        start.Enabled = stop.Enabled = play.Enabled = save.Enabled = clearSave.Enabled = profile.Enabled = login.Enabled = false;
        try { await Program.RunAsync(arguments); }
        catch (Exception error) { ShowError(error); }
        finally { busy = false; await RefreshStateAsync(); }
    }

    private bool SaveProcessesRunning()
    {
        string client = ClientLocator.Find(root);
        foreach (string name in new[] { "game", "Nanaimo.Server", "nanaimo_gameplay_bridge" })
            foreach (var process in Process.GetProcessesByName(name))
                using (process)
                {
                    string? path = ProcessPaths.Executable(process.Id);
                    if (path is not null && (string.Equals(path, client, StringComparison.OrdinalIgnoreCase)
                        || path.StartsWith(Path.GetFullPath(root) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))) return true;
                }
        return false;
    }

    private async Task ClearSaveAsync()
    {
        if (busy) return;
        try
        {
            if (SaveProcessesRunning()) throw new InvalidOperationException("請先離開遊戲並停伺服器，再重設存檔。");
            if (gm?.IsBusy == true) throw new InvalidOperationException("等陣先管理員操作完成。");
            if (MessageBox.Show(this, "這會重設存檔中的所有帳號、角色、金幣、背包、任務、副本進度及伺服器設定。\n舊存檔將移至 save-backups，遊戲檔案與啟動設定會保留。\n確定重設所有存檔？",
                "重設存檔", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            busy = true;
            Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
            string backup = SaveReset.Archive(root, SaveProcessesRunning);
            gm?.ResetAfterClear();
            MessageBox.Show(this, "存檔已重設，下次啟動時會建立新存檔。\n備份位置：" + backup, "重設存檔", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception error) { ShowError(error); }
        finally { busy = false; await RefreshStateAsync(); }
    }

    private int? ServerId()
    {
        string expected = Program.ServerPath(root);
        foreach (var process in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(expected)))
        {
            using (process)
            {
                try { if (string.Equals(ProcessPaths.Executable(process.Id), expected, StringComparison.OrdinalIgnoreCase)) return process.Id; }
                catch (Win32Exception) { }
                catch (InvalidOperationException) { }
            }
        }
        return null;
    }

    private async Task RefreshStateAsync()
    {
        if (refreshing || IsDisposed) return;
        refreshing = true;
        try
        {
            var snapshot = await Task.Run(() => (Id: ServerId(), Listeners: IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners()
                .Where(endpoint => endpoint.Address.Equals(IPAddress.Loopback)).Select(endpoint => endpoint.Port).ToHashSet()));
            if (IsDisposed) return;
            int? id = snapshot.Id;
            var listeners = snapshot.Listeners;
            bool running = id.HasValue;
            int bound = portNumbers.Count(listeners.Contains);
            status.Text = running ? bound == portNumbers.Length ? "伺服器開緊" : "伺服器啟動中" : "伺服器已停止";
            status.ForeColor = running ? Color.FromArgb(20, 115, 70) : Color.FromArgb(110, 65, 65);
            details.Text = running ? $"處理程序 {id}     本機連接埠 {bound}/{portNumbers.Length}     用戶端：127.0.0.1" : $"本機連接埠 0/{portNumbers.Length}";
            for (int i = 0; i < portNumbers.Length; i++)
            {
                string value = running && listeners.Contains(portNumbers[i]) ? "監聽中" : "已停止";
                if (!Equals(ports.Rows[i].Cells[2].Value, value)) ports.Rows[i].Cells[2].Value = value;
            }
            start.Enabled = !busy && !running; stop.Enabled = !busy && running;
            play.Enabled = !busy; save.Enabled = !busy && !running; profile.Enabled = !busy && !running;
            clearSave.Enabled = !busy && !running;
            login.Enabled = !busy;
            if (id.HasValue && id != observedServerId) { logOffset = 0; log.Clear(); observedServerId = id; }
            if (log.Visible) ReadLog();
        }
        catch (IOException) { }
        catch (NetworkInformationException) { }
        finally { refreshing = false; }
    }

    private void ReadLog()
    {
        string path = Path.Combine(root, Program.IsMergedServer(root) ? "server-merged" : "server", "server.log");
        if (!File.Exists(path)) return;
        using var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
        if (file.Length < logOffset) { logOffset = 0; log.Clear(); }
        if (file.Length == logOffset) return;
        if (file.Length - logOffset > 65536) { logOffset = file.Length - 65536; log.Clear(); }
        file.Seek(logOffset, SeekOrigin.Begin);
        using var reader = new StreamReader(file, Encoding.UTF8);
        string text = reader.ReadToEnd(); logOffset = file.Position;
        log.AppendText(text);
        if (log.TextLength > 200000) { log.Select(0, log.TextLength - 150000); log.SelectedText = ""; }
        log.SelectionStart = log.TextLength; log.ScrollToCaret();
    }

    private void ShowError(Exception error) => MessageBox.Show(this, error.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

    private static string PortLabel(int port) => port switch
    {
        11005 => "登入", 11999 => "角色資料註冊", 12050 => "遊戲入口",
        >= 22051 and <= 22054 => "競技／休閒 " + (port - 22050),
        51999 => "副本角色資料介面", 52050 => "副本入口",
        51005 => "副本相容介面",
        >= 62050 and <= 62055 => "副本連線 " + (port - 62049),
        _ => "遊戲連線 " + (port - 32049)
    };
}

internal sealed class ProfileSettings
{
    private readonly string path;
    private readonly Dictionary<string, string> values;
    private readonly Encoding encoding;

    internal ProfileSettings(string file)
    {
        path = file;
        values = File.ReadAllLines(path).Where(line => line.Contains('='))
            .Select(line => line.Split('=', 2)).ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim());
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        encoding = Encoding.GetEncoding(936, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);
        CharacterName = encoding.GetString(Convert.FromHexString(values["name_hex"]));
        Level = Read("level"); MaximumHp = Read("hp_max"); MaximumMp = Read("mp_max");
        Pet = Read("pet"); Coins = ulong.Parse(values["coin"]); Cash = ulong.Parse(values["nana_point"]);
    }

    [Category("角色"), DisplayName("角色名稱")]
    public string CharacterName { get; set; }
    [Category("角色"), DisplayName("初始等級")]
    public int Level { get; set; }
    [Category("角色"), DisplayName("寵物編號")]
    public int Pet { get; set; }
    [Category("能力值"), DisplayName("生命值上限")]
    public int MaximumHp { get; set; }
    [Category("能力值"), DisplayName("魔力值上限")]
    public int MaximumMp { get; set; }
    [Category("貨幣"), DisplayName("初始金幣")]
    public ulong Coins { get; set; }
    [Category("貨幣"), DisplayName("初始點數")]
    public ulong Cash { get; set; }

    private int Read(string key) => int.Parse(values[key]);

    internal void Save()
    {
        byte[] name = encoding.GetBytes(CharacterName.Trim());
        if (name.Length is < 1 or > 15 || name.Contains((byte)0)) throw new InvalidDataException("角色名稱的 GBK 編碼長度一定要為 1 至 15 位元組。");
        if (Level is < 1 or > 99 || MaximumHp is < 1 or > 65535 || MaximumMp is < 1 or > 65535 || Pet <= 0)
            throw new InvalidDataException("等級須為 1 至 99，生命值與魔力值上限須為 1 至 65535，寵物編號須大於零。");
        values["name_hex"] = Convert.ToHexString(name); values["level"] = Level.ToString(); values["pet"] = Pet.ToString();
        values["hp_max"] = values["hp_current"] = MaximumHp.ToString();
        values["mp_max"] = values["mp_current"] = MaximumMp.ToString();
        values["coin"] = Coins.ToString(); values["nana_point"] = Cash.ToString();
        File.WriteAllLines(path + ".new", values.Select(pair => $"{pair.Key}={pair.Value}"), Encoding.ASCII);
        File.Replace(path + ".new", path, path + ".bak");
    }
}
