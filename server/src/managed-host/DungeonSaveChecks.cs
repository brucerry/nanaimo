using System.Buffers.Binary;
using FlightIslandServer.Desktop.Models;
using FlightIslandServer.Desktop.Services;

internal static class DungeonSaveChecks
{
    public static async Task RunAsync(DatabaseService db, CancellationToken token)
    {
        long account = await db.OpenLocalAccountAsync("dungeon-save-check", token);
        await db.CreateLocalCharacterAsync(account, "SaveCheck", 1, token);
        var character = (await db.GetCharacterAsync(account, token))!;
        var baseline = NativeDungeonState.Create(character, [], []);
        string session = Guid.NewGuid().ToString("N");
        Check(await db.BeginWorldSessionAsync(account, character.Id, session, 1, "127.0.0.1", token), "dungeon save fixture online");
        try
        {
            // Legacy wire selector 2 is ordinary LOW, not logical HIGH.
            var legacy = new NativeDungeonState(baseline.Bytes.ToArray());
            Put(legacy, 5024, 1); Put(legacy, 5028, 1); Put(legacy, 5044, 2);
            await db.ApplyNativeDungeonDeltaAsync(account, character.Id, session, baseline, legacy, token);
            var masks = await db.GetDungeonClearMasksAsync(character.Id, token);
            Check(masks[0] == 1 && masks[2] == 0, "legacy ordinary clear maps to low difficulty");
            Put(legacy, 5040, 2); Put(legacy, 5048, 1);
            await db.ApplyNativeDungeonDeltaAsync(account, character.Id, session, baseline, legacy, token);
            masks = await db.GetDungeonClearMasksAsync(character.Id, token);
            Check(masks[2] == 8, "legacy Super-BOSS clear retains its own bit and logical difficulty");

            var restored = NativeDungeonState.Create(character, [], []);
            await new DatabaseService(Path.GetDirectoryName(db.DatabasePath)!).RestoreNativeDungeonProgressAsync(character.Id, restored, token);
            Check(restored.Bytes[5052] == 1 && restored.Bytes[5054] == 8, "reopening database restores both clear records");
            var replies = new List<byte[]>();
            await using var bridge = new NativeDungeonClient(frame => { replies.Add(frame); return Task.CompletedTask; });
            await bridge.ConnectAsync(token);
            var imported = await bridge.ExchangeAsync(null, restored, token);
            Check(imported.Get(5112) == 1 && imported.Bytes.AsSpan(5052, 60).SequenceEqual(restored.Bytes.AsSpan(5052, 60)), "native worker round-trips all dungeon clear masks");
            // A lower clear after a higher frontier must still be archived.
            imported.Bytes[5052 + 3] = 2;
            imported.Bytes[5052 + 5] = 4;
            await db.ApplyNativeDungeonDeltaAsync(account, character.Id, session, restored, imported, token, "multi-clear-check");
            await db.ApplyNativeDungeonDeltaAsync(account, character.Id, session, restored, imported, token, "multi-clear-check");
            masks = await db.GetDungeonClearMasksAsync(character.Id, token);
            Check(masks[0] == 1 && masks[2] == 8 && masks[3] == 2 && masks[5] == 4, "all clears survive a fixed highest frontier and duplicate commit");
            async Task<NativeDungeonState> Request(ushort opcode, byte[] payload, ushort response)
            {
                replies.Clear();
                var result = await bridge.ExchangeAsync(NativeDungeonClient.Frame(opcode, payload), null, token);
                Check(replies.Any(f => BinaryPrimitives.ReadUInt16LittleEndian(f.AsSpan(6)) == response), $"level-one dungeon 0x{opcode:X4} receives 0x{response:X4}");
                return result;
            }
            // Retail quick entry omits CF6C. Preserve its captured mode-10
            // CF77 tuple so CFEB can select the map before the start request.
            await Request(0xCF09, new byte[56], 0xCF0A);
            await Request(0xCF77, [10, 0, 0, 0, 0, 2, 255, 255], 0xCF78);
            await Request(0xCFD1, new byte[20], 0xCFD2);
            await Request(0xC587, [], 0xC588);
            await Request(0xCF70, BitConverter.GetBytes(55), 0xCF71);
            await Request(0xCFD9, [], 0xCFDA);
            await Request(0xCFEB, new byte[4], 0xCFEC);
            await Request(0xCFD3, [], 0xCFD4);
            await Request(0xCFD5, BitConverter.GetBytes(1), 0xCFD6);
            await Request(0xCF7F, [], 0xCF80);
            Check(true, "retail quick-entry tutorial starts without CF6C");

            await Request(0xCF09, new byte[56], 0xCF0A);
            var entry = new byte[44]; entry[30] = 2;
            await Request(0xCF6C, entry, 0xCF6D);
            await Request(0xCF70, [], 0xCF71);
            Check(replies.Single(f => BinaryPrimitives.ReadUInt16LittleEndian(f.AsSpan(6)) == 0xCF71)[0x49] == 1,
                "restored dungeon grade is reflected in room character data");
            await Request(0xCFEB, new byte[4], 0xCFEC);
            await Request(0xCFD3, [], 0xCFD4);
            await Request(0xCFD5, BitConverter.GetBytes(1), 0xCFD6);
            await Request(0xCF7F, [], 0xCF80);
            replies.Clear();
            var premature = await bridge.ExchangeAsync(NativeDungeonClient.Frame(0xCF87, new byte[4]), null, token);
            Check(!replies.Any(f => BinaryPrimitives.ReadUInt16LittleEndian(f.AsSpan(6)) == 0xCF88)
                && premature.Get(12) == imported.Get(12), "premature settlement before Boss defeat grants no experience");
        }
        finally
        {
            await db.EndWorldSessionAsync(account, character.Id, session,
                new CharacterRuntimeState(character.CurrentHp, character.CurrentMp, character.CurrentMapId, character.CurrentTownPage, character.PositionX, character.PositionY, 1), token);
        }
    }

    private static void Put(NativeDungeonState state, int offset, uint value) => BinaryPrimitives.WriteUInt32LittleEndian(state.Bytes.AsSpan(offset), value);
    private static void Check(bool success, string name)
    {
        if (!success) throw new InvalidDataException("CHECK_FAILED " + name);
        Console.WriteLine("CHECK_PASS " + name);
    }
}
