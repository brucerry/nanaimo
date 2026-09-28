using System.IO.Pipes;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace FlightIslandServer.Desktop.Services;

public static class GmRuntimeControl
{
    private static string PipeName(string data) => "NanaimoGM-" + Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(Path.GetFullPath(data).ToUpperInvariant())))[..20];

    public static async Task<JsonElement> RequestAsync(string data, string command, string value, CancellationToken token = default)
    {
        using var timeout=CancellationTokenSource.CreateLinkedTokenSource(token); timeout.CancelAfter(TimeSpan.FromSeconds(5));
        await using var pipe=new NamedPipeClientStream(".",PipeName(data),PipeDirection.InOut,PipeOptions.Asynchronous|PipeOptions.CurrentUserOnly);
        try { await pipe.ConnectAsync(timeout.Token); }
        catch(OperationCanceledException) { throw new IOException("服務端未啓動或管理接口未響應，請先啓動服務端。"); }
        using var reader=new StreamReader(pipe,Encoding.UTF8,leaveOpen:true);
        using var writer=new StreamWriter(pipe,new UTF8Encoding(false),leaveOpen:true) { AutoFlush=true };
        await writer.WriteLineAsync(JsonSerializer.Serialize(new { Command=command,Value=value }).AsMemory(),timeout.Token);
        using var response=JsonDocument.Parse(await reader.ReadLineAsync(timeout.Token) ?? throw new IOException("管理連接已閂埋。"));
        if(!response.RootElement.GetProperty("Ok").GetBoolean()) throw new InvalidOperationException(response.RootElement.GetProperty("Error").GetString());
        return response.RootElement.GetProperty("Data").Clone();
    }

    public static async Task RunAsync(string data, NetworkHostService host, NativeDungeonPool rooms, Action<string> log, CancellationToken token)
    {
        while(!token.IsCancellationRequested)
        {
            await using var pipe=new NamedPipeServerStream(PipeName(data),PipeDirection.InOut,1,PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous|PipeOptions.CurrentUserOnly);
            await pipe.WaitForConnectionAsync(token);
            using var timeout=CancellationTokenSource.CreateLinkedTokenSource(token); timeout.CancelAfter(TimeSpan.FromSeconds(5));
            using var reader=new StreamReader(pipe,Encoding.UTF8,leaveOpen:true);
            using var writer=new StreamWriter(pipe,new UTF8Encoding(false),leaveOpen:true) { AutoFlush=true };
            try
            {
                string line=await reader.ReadLineAsync(timeout.Token) ?? throw new IOException("管理員請求唔可以為空。");
                if(line.Length>4096)throw new InvalidDataException("管理員請求過大。");
                using var request=JsonDocument.Parse(line);
                string command=request.RootElement.GetProperty("Command").GetString() ?? "";
                string value=request.RootElement.GetProperty("Value").GetString() ?? "";
                object result;
                if(command=="snapshot") result=value switch
                {
                    "connections"=>host.GetOnlineConnections(), "native"=>await rooms.GetSnapshotAsync(timeout.Token),
                    "arena"=>host.GetArenaRooms(), "parties"=>host.GetParties(), "trades"=>host.GetTradeRooms(),
                    _=>throw new InvalidDataException("未知的狀態查詢。")
                };
                else if(command=="kick")
                {
                    result=host.KickConnection(value,"GM 管理斷開連接");
                    log($"管理員中斷連線：session={value} result={result}");
                }
                else throw new InvalidDataException("未知的管理員指令。");
                await writer.WriteLineAsync(JsonSerializer.Serialize(new { Ok=true,Data=result,Error="" }).AsMemory(),timeout.Token);
            }
            catch(Exception ex) when(!token.IsCancellationRequested)
            {
                log($"管理員操作：{ex.Message}");
                try { await writer.WriteLineAsync(JsonSerializer.Serialize(new { Ok=false,Data=(object?)null,Error=ex.Message }).AsMemory(),timeout.Token); }
                catch(Exception) when(!token.IsCancellationRequested) { }
            }
        }
    }
}
