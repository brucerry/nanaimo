using System.Text.Json;

namespace Nanaimo.Launcher;

internal sealed class AccountLoginControl : UserControl
{
    private readonly TextBox account = new() { Dock = DockStyle.Fill, MaxLength = 64, AccessibleName = "Login account" };
    private readonly ComboBox resolution = new() { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList,
        AccessibleName = "視窗解像度" };
    private readonly CheckBox fullScreen = new() { Text = "全螢幕（用螢幕原本解像度）", AutoSize = true, AccessibleName = "全螢幕" };
    private readonly string root;
    private readonly string saved;
    internal string AccountName => account.Text.Trim();
    internal Button LoginButton { get; } = new() { Text = "登入並開始玩", AutoSize = true, AccessibleName = "Login and play" };
    internal event EventHandler? LoginRequested;

    internal AccountLoginControl(string root)
    {
        Size = new Size(450, 260);
        Font = new Font("Segoe UI", 10);
        this.root = root;
        saved = Path.Combine(root, "launcher", "local-account.json");
        if (File.Exists(saved))
        {
            try { account.Text = JsonSerializer.Deserialize<string>(File.ReadAllText(saved)) ?? ""; }
            catch (JsonException) { }
        }
        foreach (GameResolution preset in GameDisplaySettings.Presets) resolution.Items.Add(preset.ToString());
        GameDisplayMode display = GameDisplaySettings.Load(root);
        resolution.SelectedIndex = Array.IndexOf(GameDisplaySettings.Presets, display.Resolution);
        fullScreen.Checked = display.FullScreen;
        resolution.Enabled = !fullScreen.Checked;
        fullScreen.CheckedChanged += (_, _) => resolution.Enabled = !fullScreen.Checked;
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(20), RowCount = 6, ColumnCount = 1 };
        foreach (int height in new[] { 28, 38, 28, 38, 32 }) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, height));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.Controls.Add(new Label { Text = "帳號", AutoSize = true }, 0, 0);
        layout.Controls.Add(account, 0, 1);
        layout.Controls.Add(new Label { Text = "視窗解像度", AutoSize = true }, 0, 2);
        layout.Controls.Add(resolution, 0, 3);
        layout.Controls.Add(fullScreen, 0, 4);
        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill };
        buttons.Controls.Add(LoginButton);
        layout.Controls.Add(buttons, 0, 5);
        Controls.Add(layout);
        LoginButton.Click += (_, _) => Submit();
    }

    internal void FocusAccount() { account.Focus(); account.SelectAll(); }

    internal void Submit()
    {
        if (AccountName.Length == 0 || AccountName.Any(char.IsControl))
        {
            MessageBox.Show(this, "輸入帳號名稱，唔可以含有控制字元。", "帳號登入", MessageBoxButtons.OK, MessageBoxIcon.Information);
            FocusAccount(); return;
        }
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(saved)!);
            File.WriteAllText(saved, JsonSerializer.Serialize(AccountName));
            GameResolution selected = GameDisplaySettings.Presets[resolution.SelectedIndex];
            GameDisplaySettings.Save(root, new GameDisplayMode(selected.Width, selected.Height, fullScreen.Checked));
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException)
        {
            MessageBox.Show(this, error.Message, "帳號登入", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        LoginRequested?.Invoke(this, EventArgs.Empty);
    }

    internal string[] Arguments => ["--account", AccountName];
}
