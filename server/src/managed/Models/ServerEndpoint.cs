using System.Text.Json.Serialization;

namespace FlightIslandServer.Desktop.Models;

public sealed class ServerEndpoint
{
    public int Id { get; set; }
    public string Name { get; set; } = "本地一區";
    public string Host { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 12050;
    public bool Enabled { get; set; } = true;
    public string Catalog { get; set; } = "local-1";
    public int Capacity { get; set; } = 1000;

    [JsonIgnore]
    public int CurrentPlayers { get; set; }

    [JsonIgnore]
    public string Status => !Enabled
        ? "停用"
        : CurrentPlayers >= Capacity
            ? "已滿"
            : "可進入";

    [JsonIgnore]
    public string LoadStatus => $"{CurrentPlayers}/{Capacity}";
}
