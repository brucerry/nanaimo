using System.Text.Json;

namespace Nanaimo.Launcher;

internal sealed class AccountLoginControl : UserControl
{
    private readonly TextBox account = new() { Dock = DockStyle.Fill, MaxLength = 64, AccessibleName = "Login account" };
    private readonly string saved;
    internal string AccountName => account.Text.Trim();
    internal Button LoginButton { get; } = new() { Text = "登入並開始玩", AutoSize = true, AccessibleName = "Login and play" };
    internal event EventHandler? LoginRequested;

    internal AccountLoginControl(string root)
    {
        Size = new Size(450, 160);
        Font = new Font("Segoe UI", 10);
        saved = Path.Combine(root, "launcher", "local-account.json");
        if (File.Exists(saved))
        {
            try { account.Text = JsonSerializer.Deserialize<string>(File.ReadAllText(saved)) ?? ""; }
            catch (JsonException) { }
        }
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(20), RowCount = 3, ColumnCount = 1 };
        foreach (int height in new[] { 28, 38 }) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, height));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.Controls.Add(new Label { Text = "帳號", AutoSize = true }, 0, 0);
        layout.Controls.Add(account, 0, 1);
        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill };
        buttons.Controls.Add(LoginButton);
        layout.Controls.Add(buttons, 0, 2);
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
