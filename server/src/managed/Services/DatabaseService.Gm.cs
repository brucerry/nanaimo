using System.Text;
using System.Text.Json;
using FlightIslandServer.Desktop.Models;
using Microsoft.Data.Sqlite;

namespace FlightIslandServer.Desktop.Services;

public sealed partial class DatabaseService
{
    public async Task<IReadOnlyList<GmCatalogItem>> GetShopWishlistsForGmAsync(long accountId, CancellationToken token = default)
    {
        var character = await GetCharacterAsync(accountId, token);
        if (character is null) return [];
        var codes = (await GetShopWishlistAsync(character.Id, token)).Select(i=>i.ItemCode)
            .Concat(await GetNanaWishlistAsync(character.Id, token))
            .Concat((await GetInteriorWishlistAsync(character.Id, token)).Select(i=>i.ItemCode)).Distinct();
        return codes.Select(code=>ShopCatalog.TryGet(code,out var item)
            ? new GmCatalogItem("item",code,item.Name,item.SectionName) : new GmCatalogItem("item",code,"未知具","")).ToArray();
    }

    public static IReadOnlyList<GmCatalogItem> GetGmCatalog() => ShopCatalog.All
        .Select(i => new GmCatalogItem("item", i.ItemCode, i.Name, i.SectionName))
        .Concat(CardCatalog.All.Select(i => new GmCatalogItem("card", i.CardCode, i.Name, i.CategoryName)))
        .Concat(SkillCatalog.All.Select(i => new GmCatalogItem("skill", i.SkillCode, i.Name, "技能"))).ToArray();

    private static async Task<long> RequireGmOfflineAsync(SqliteConnection connection, SqliteTransaction transaction, long accountId, CancellationToken token)
    {
        await using var command = connection.CreateCommand(); command.Transaction = transaction;
        command.CommandText = """
            SELECT c.Id FROM Characters c JOIN Accounts a ON a.Id=c.AccountId
            WHERE a.Id=$account AND a.IsOnline=0 AND c.IsOnline=0
                AND a.ActiveSessionId IS NULL AND c.ActiveSessionId IS NULL
            """;
        command.Parameters.AddWithValue("$account", accountId);
        var id = await command.ExecuteScalarAsync(token);
        if (id is null) throw new InvalidOperationException("請先退出該角色嘅遊戲，等待會話釋放後刷新；賬號一定要已有角色。");
        return Convert.ToInt64(id);
    }

    private static async Task AuditGmAsync(SqliteConnection connection, SqliteTransaction transaction, long account, string action, object details, CancellationToken token)
    {
        await using var command = connection.CreateCommand(); command.Transaction = transaction;
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS GmAudit(Id INTEGER PRIMARY KEY AUTOINCREMENT, Time TEXT NOT NULL,
                AccountId INTEGER NOT NULL, Action TEXT NOT NULL, Details TEXT NOT NULL);
            INSERT INTO GmAudit(Time,AccountId,Action,Details) VALUES($time,$account,$action,$details)
            """;
        command.Parameters.AddWithValue("$time", DateTime.UtcNow.ToString("O"));
        command.Parameters.AddWithValue("$account", account);
        command.Parameters.AddWithValue("$action", action);
        command.Parameters.AddWithValue("$details", JsonSerializer.Serialize(details));
        await command.ExecuteNonQueryAsync(token);
    }

    public async Task<IReadOnlyList<GmAuditRecord>> GetGmAuditAsync(CancellationToken token = default)
    {
        await using var connection = await OpenConnectionAsync(token);
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='GmAudit'";
        if (Convert.ToInt32(await command.ExecuteScalarAsync(token)) == 0) return [];
        command.CommandText = "SELECT Id,Time,AccountId,Action,Details FROM GmAudit ORDER BY Id DESC LIMIT 200";
        await using var reader = await command.ExecuteReaderAsync(token);
        var rows = new List<GmAuditRecord>();
        while (await reader.ReadAsync(token)) rows.Add(new(reader.GetInt64(0), reader.GetString(1), reader.GetInt64(2), reader.GetString(3), reader.GetString(4)));
        return rows;
    }

    public async Task SaveGmCharacterAsync(GmCharacterEdit edit, CancellationToken token = default)
    {
        string name = edit.Name.Trim();
        var gbk = Encoding.GetEncoding(936, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);
        if (name.Length == 0 || name.Any(char.IsControl) || gbk.GetByteCount(name) > 14)
            throw new InvalidDataException("角色名要 1..14 個 GBK 字節，唔得包含控制字符。");
        if (edit.Level is < 1 or > 99 || edit.Experience is < 0 or > uint.MaxValue || CharacterProgression.CalculateLevel(edit.Experience) != edit.Level)
            throw new InvalidDataException("等級範圍為 1..99，經驗一定要處於該等級區間；修改等級會自動填寫起始經驗。");
        if (edit.Hans is < 0 or > uint.MaxValue || edit.Cash is < 0 or > uint.MaxValue)
            throw new InvalidDataException("金幣和點券範圍為 0..4294967295。");
        if (new[] { edit.AttributePoints, edit.Strength, edit.Vitality, edit.Agility, edit.Intelligence, edit.Luck, edit.SkillPoints }.Any(v => v is < 0 or > ushort.MaxValue)
            || edit.MaxHp > ushort.MaxValue || edit.MaxMp > ushort.MaxValue || edit.PetLevel is < 1 or > 99)
            throw new InvalidDataException("屬性與技能點範圍為 0..65535；計算後的生命和魔法唔得超過 65535；寵物等級為 1..99。");
        await using var connection = await OpenConnectionAsync(token);
        await using var transaction = connection.BeginTransaction();
        long id = await RequireGmOfflineAsync(connection, transaction, edit.AccountId, token);
        await using var command = connection.CreateCommand(); command.Transaction = transaction;
        command.CommandText = "SELECT COUNT(*) FROM Characters WHERE Id<>$id AND Name=$name COLLATE NOCASE";
        command.Parameters.AddWithValue("$id", id); command.Parameters.AddWithValue("$name", name);
        if (Convert.ToInt32(await command.ExecuteScalarAsync(token)) != 0) throw new InvalidDataException("角色名已經被使用。");
        command.CommandText = """
            UPDATE Characters SET Name=$name,Level=$level,Experience=$exp,Hans=$hans,Cash=$cash,
              AttributePoints=$points,Strength=$str,Vitality=$vit,Agility=$agi,Intelligence=$int,Luck=$luck,SkillPoints=$skill,
              MaxHp=$hp,MaxMp=$mp,CurrentHp=CASE WHEN $heal=1 THEN $hp ELSE MIN(CurrentHp,$hp) END,
              CurrentMp=CASE WHEN $heal=1 THEN $mp ELSE MIN(CurrentMp,$mp) END,PetLevel=$pet,LastSavedAt=$now WHERE Id=$id;
            UPDATE CharacterItems SET PetLevel=$pet,UpdatedAt=$now WHERE CharacterId=$id
              AND ItemCode=(SELECT EquippedPetItemCode FROM Characters WHERE Id=$id);
            UPDATE Accounts SET IsGm=$gm,IsBanned=$ban WHERE Id=$account;
            """;
        foreach (var (key, value) in new (string, object)[] {
            ("$level",edit.Level),("$exp",edit.Experience),("$hans",edit.Hans),("$cash",edit.Cash),("$points",edit.AttributePoints),
            ("$str",edit.Strength),("$vit",edit.Vitality),("$agi",edit.Agility),("$int",edit.Intelligence),("$luck",edit.Luck),
            ("$skill",edit.SkillPoints),("$hp",edit.MaxHp),("$mp",edit.MaxMp),("$heal",edit.RestoreHealth ? 1 : 0),
            ("$pet",edit.PetLevel),("$now",DateTime.UtcNow.ToString("O")),("$gm",edit.IsGm ? 1 : 0),
            ("$ban",edit.IsBanned ? 1 : 0),("$account",edit.AccountId) }) command.Parameters.AddWithValue(key,value);
        await command.ExecuteNonQueryAsync(token);
        await AuditGmAsync(connection, transaction, edit.AccountId, "保存角色", edit, token);
        await transaction.CommitAsync(token);
    }

    public async Task ChangeGmStockAsync(long account, string kind, uint code, int quantity, CancellationToken token = default)
    {
        if (kind is not ("item" or "card" or "skill")) throw new InvalidDataException("未知資源類型。");
        if (kind == "item" && !ShopCatalog.TryGet(code, out _) || kind == "card" && !CardCatalog.TryGet(code, out _)
            || kind == "skill" && !SkillCatalog.TryGet(code, out _)) throw new InvalidDataException("編號唔喺資源目錄中。");
        if (kind == "skill" ? quantity is < 0 or > 5 : quantity is 0 or < -65535 or > 65535)
            throw new InvalidDataException("數量無效；技能等級範圍為 0..5。");
        await using var connection = await OpenConnectionAsync(token);
        await using var transaction = connection.BeginTransaction();
        long id = await RequireGmOfflineAsync(connection, transaction, account, token);
        // Identifiers below come only from the fixed kind allowlist.
        string table = kind == "item" ? "CharacterItems" : kind == "card" ? "CharacterCards" : "CharacterSkills";
        string key = kind == "item" ? "ItemCode" : kind == "card" ? "CardCode" : "SkillCode";
        string field = kind == "skill" ? "Grade" : "Quantity";
        await using var command = connection.CreateCommand(); command.Transaction = transaction;
        command.Parameters.AddWithValue("$id", id); command.Parameters.AddWithValue("$code", code);
        command.CommandText = $"SELECT {field} FROM {table} WHERE CharacterId=$id AND {key}=$code";
        int before = Convert.ToInt32(await command.ExecuteScalarAsync(token));
        int after = kind == "skill" ? quantity : checked(before + quantity);
        if (after < 0 || after > (kind == "item" ? 65535 : kind == "card" ? 255 : 5)) throw new InvalidDataException("庫存不足或超過該資源數量上限。");
        if (kind == "item")
        {
            if (NativeDungeonState.IsNativeItem(code))
            {
                command.CommandText = "SELECT COALESCE(SUM(Quantity),0) FROM CharacterItems WHERE CharacterId=$id AND ItemCode/1000000 IN (14,17,19,21)";
                if (Convert.ToInt64(await command.ExecuteScalarAsync(token)) + quantity > 255)
                    throw new InvalidDataException("原生地宮消耗品總數最多 255，發放會超出容量。");
            }
            if (after == 0)
            {
                command.CommandText = "SELECT EquippedPetItemCode,Appearance,PetVariant FROM Characters WHERE Id=$id";
                await using var reader = await command.ExecuteReaderAsync(token); await reader.ReadAsync(token);
                byte[] appearance = (byte[])reader[1];
                bool equipped = reader.GetInt64(0) == code || reader.GetInt32(2) is >= 1 and <= 3 && 15000000 + reader.GetInt32(2) == code;
                for (int offset = 0; offset < 28; offset += 4)
                    equipped |= System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(appearance.AsSpan(offset,4)) == code;
                if (equipped) throw new InvalidDataException("已裝備物品或教學寵物唔得扣減至零，請先喺遊戲中卸下。");
            }
        }
        command.Parameters.AddWithValue("$value", after); command.Parameters.AddWithValue("$now", DateTime.UtcNow.ToString("O"));
        if (after == 0)
        {
            command.CommandText = $"DELETE FROM {table} WHERE CharacterId=$id AND {key}=$code";
            if (kind == "item") command.CommandText += "; DELETE FROM CharacterQuickSlots WHERE CharacterId=$id AND ItemCode=$code";
            if (kind == "skill") command.CommandText += "; UPDATE Characters SET SelectedSkill0=CASE WHEN SelectedSkill0=$code THEN 0 ELSE SelectedSkill0 END,SelectedSkill1=CASE WHEN SelectedSkill1=$code THEN 0 ELSE SelectedSkill1 END WHERE Id=$id";
        }
        else command.CommandText = $"INSERT INTO {table}(CharacterId,{key},{field},UpdatedAt) VALUES($id,$code,$value,$now) ON CONFLICT(CharacterId,{key}) DO UPDATE SET {field}=$value,UpdatedAt=$now";
        await command.ExecuteNonQueryAsync(token);
        if (kind == "item" && after > 0 && ShopCatalog.TryGet(code, out var pet) && pet.Section == InventorySection.Pet)
        {
            command.CommandText = "UPDATE CharacterItems SET PetCurrentStage=MAX(PetCurrentStage,$stage),PetMaximumStage=MAX(PetMaximumStage,$maximum) WHERE CharacterId=$id AND ItemCode=$code";
            command.Parameters.AddWithValue("$stage", Math.Max(1,(int)pet.PetModelStage));
            command.Parameters.AddWithValue("$maximum", Math.Max(1,(int)pet.PetUpgradeStage));
            await command.ExecuteNonQueryAsync(token);
        }
        await AuditGmAsync(connection, transaction, account, kind == "skill" ? "設置技能" : "調整庫存", new { kind, code, before, after }, token);
        await transaction.CommitAsync(token);
    }
}
