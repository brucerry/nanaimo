namespace Nanaimo.Launcher;

internal sealed class AccountLoginForm : Form
{
    private readonly AccountLoginControl login;
    internal string AccountName => login.AccountName;

    internal AccountLoginForm(string root)
    {
        Text = "Nanaimo - Account login";
        Font = new Font("Segoe UI", 10);
        ClientSize = new Size(450, 160);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false; MinimizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        login = new AccountLoginControl(root) { Dock = DockStyle.Fill };
        Controls.Add(login);
        AcceptButton = login.LoginButton;
        login.LoginRequested += (_, _) => DialogResult = DialogResult.OK;
        Shown += (_, _) => login.FocusAccount();
    }
}
