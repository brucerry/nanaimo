namespace Nanaimo.Launcher;

internal static class SaveReset
{
    internal static string Archive(string root, Func<bool> isRunning)
    {
        if (isRunning()) throw new InvalidOperationException("請先離開遊戲並停伺服器，再重設存檔。");
        string server = Path.GetFullPath(Path.Combine(root, "server-merged"));
        string data = Path.Combine(server, "data");
        if (!Directory.Exists(data)) throw new InvalidOperationException("而家冇可重設的存檔。");
        foreach (string directory in new[] { root, server, data })
            if ((File.GetAttributes(directory) & FileAttributes.ReparsePoint) != 0)
                throw new InvalidOperationException("存檔目錄唔可以使用目錄連結，請檢查路徑。");
        string backups = Path.Combine(server, "save-backups");
        Directory.CreateDirectory(backups);
        if ((File.GetAttributes(backups) & FileAttributes.ReparsePoint) != 0)
            throw new InvalidOperationException("備份目錄唔可以使用目錄連結。");
        string backup = Path.Combine(backups, DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + Guid.NewGuid().ToString("N")[..8]);
        if (isRunning()) throw new InvalidOperationException("遊戲或伺服器而家喺度啟動，已唔要重設存檔。");
        // Same-volume rename preserves the database, WAL and native journals together.
        Directory.Move(data, backup);
        return backup;
    }
}
