using FlightIslandServer.Desktop.Services;

namespace FlightIslandServer.Desktop.Models;

public sealed class ApartmentPlacementRecord
{
    public byte SlotIndex { get; init; }
    public uint ItemCode { get; init; }
    public short X { get; init; }
    public short Y { get; init; }
    public byte Layer { get; init; }
    public byte Mirror { get; init; }
    public byte InteriorType { get; init; }

    public string ItemName => ShopCatalog.TryGet(ItemCode, out var item) ? item.Name : "未知傢俱";
    public string InteriorTypeName => InteriorType switch
    {
        0 => "地板",
        1 => "牆面",
        2 => "地面傢俱",
        3 => "牆面裝飾",
        4 => "交互傢俱",
        _ => $"類型 {InteriorType}"
    };
    public string PositionStatus => InteriorType <= 1 ? "固定表面" : $"{X}, {Y}";
}
