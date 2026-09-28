using System.ComponentModel;
using FlightIslandServer.Desktop.Services;

namespace FlightIslandServer.Desktop.Models;

public sealed class GmCharacterEdit
{
    private int level = 1;
    [Browsable(false)] public long AccountId { get; set; }
    [Category("角色"), DisplayName("名稱")] public string Name { get; set; } = "";
    [Category("角色"), DisplayName("等級")]
    public int Level { get => level; set { level = value; Experience = CharacterProgression.ExperienceRequiredForLevel(value); } }
    [Category("角色"), DisplayName("經驗值")] public long Experience { get; set; }
    [Category("貨幣"), DisplayName("金幣")] public long Hans { get; set; }
    [Category("貨幣"), DisplayName("點數")] public long Cash { get; set; }
    [Category("能力值"), DisplayName("未分配能力點數")] public int AttributePoints { get; set; }
    [Category("能力值"), DisplayName("力量")] public int Strength { get; set; }
    [Category("能力值"), DisplayName("體力")] public int Vitality { get; set; }
    [Category("能力值"), DisplayName("敏捷")] public int Agility { get; set; }
    [Category("能力值"), DisplayName("智力")] public int Intelligence { get; set; }
    [Category("能力值"), DisplayName("幸運")] public int Luck { get; set; }
    [Category("能力值"), DisplayName("技能點數")] public int SkillPoints { get; set; }
    [Category("能力值"), DisplayName("生命值上限"), ReadOnly(true)] public int MaxHp => CharacterProgression.CalculateMaxHp(Level, Vitality);
    [Category("能力值"), DisplayName("魔力值上限"), ReadOnly(true)] public int MaxMp => CharacterProgression.CalculateMaxMp(Level, Intelligence);
    [Category("能力值"), DisplayName("恢復生命值與魔力值")] [TypeConverter(typeof(TraditionalBooleanConverter))] public bool RestoreHealth { get; set; }
    [Category("寵物"), DisplayName("而家寵物等級")] public int PetLevel { get; set; } = 1;
    [Category("帳號"), DisplayName("管理員權限")] [TypeConverter(typeof(TraditionalBooleanConverter))] public bool IsGm { get; set; }
    [Category("帳號"), DisplayName("停權")] [TypeConverter(typeof(TraditionalBooleanConverter))] public bool IsBanned { get; set; }

    public static GmCharacterEdit From(CharacterRecord c, AccountRecord a) => new()
    {
        AccountId = c.AccountId, Name = c.Name, Level = c.Level, Experience = c.Experience,
        Hans = c.Hans, Cash = c.Cash, AttributePoints = c.AttributePoints, Strength = c.Strength,
        Vitality = c.Vitality, Agility = c.Agility, Intelligence = c.Intelligence, Luck = c.Luck,
        SkillPoints = c.SkillPoints, PetLevel = c.PetLevel, IsGm = a.IsGm, IsBanned = a.IsBanned
    };
}

public sealed record GmCatalogItem(string Kind, uint Code, string Name, string Category);
public sealed record GmAuditRecord(long Id, string Time, long AccountId, string Action, string Details);
