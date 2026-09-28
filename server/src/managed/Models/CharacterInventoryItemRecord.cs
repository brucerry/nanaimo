using FlightIslandServer.Desktop.Services;

namespace FlightIslandServer.Desktop.Models;

public sealed class CharacterInventoryItemRecord
{
    public uint ItemCode { get; init; }
    public ushort Quantity { get; init; }
    public string Name { get; init; } = string.Empty;
    public InventorySection Section { get; init; }
    public bool IsEquipped { get; init; }
    public bool IsProtected { get; init; }
    public string IconPath { get; init; } = string.Empty;

    public string SectionName => Section switch
    {
        InventorySection.Clothing => "衣物箱",
        InventorySection.Pet => "寵物箱",
        InventorySection.GameItem => "遊戲道具",
        InventorySection.Furniture => "裝飾傢俱",
        _ => Section.ToString()
    };
    public string State => IsProtected ? "教程寵物" : IsEquipped ? "使用中" : "庫存";
}
