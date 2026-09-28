namespace FlightIslandServer.Desktop.Models;

public sealed class MentorAdvertisementAdminRecord
{
    public long CharacterId { get; init; }
    public long AccountId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string CharacterName { get; init; } = string.Empty;
    public bool IsAdvertising { get; init; }
    public bool IsOnline { get; init; }
    public int? ChannelId { get; init; }
    public DateTime UpdatedAtUtc { get; init; }

    public string AdvertisingStatus => IsAdvertising ? "發佈中" : "已停止";
    public string OnlineStatus => IsOnline ? "在線" : "離線";
    public string ChannelStatus => ChannelId?.ToString() ?? "-";
    public string UpdatedAtText => UpdatedAtUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
}

public sealed class MentorInteractionAdminRecord
{
    public long Id { get; init; }
    public ushort RequestOpcode { get; init; }
    public string RequesterName { get; init; } = string.Empty;
    public string TargetName { get; init; } = string.Empty;
    public uint LessonCode { get; init; }
    public ushort Status { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }

    public string Direction => RequestOpcode == 0xC583 ? "學生請求家教" : "老師邀請授課";
    public string Protocol => $"0x{RequestOpcode:X4}";
    public string StatusText => Status switch
    {
        0 => "等待答覆",
        2 => "超時",
        10 => "已接受",
        20 => "已拒絕",
        7 => "已唔要",
        _ => $"官方狀態 {Status}"
    };
    public string CreatedAtText => CreatedAtUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
    public string UpdatedAtText => UpdatedAtUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
}
