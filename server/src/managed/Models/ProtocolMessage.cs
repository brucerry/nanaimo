namespace FlightIslandServer.Desktop.Models;

public sealed record ProtocolMessage(string Channel, string Remote, int Length, string Description);
