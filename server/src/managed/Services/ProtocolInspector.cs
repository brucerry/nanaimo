namespace FlightIslandServer.Desktop.Services;

public static class ProtocolInspector
{
    public static string ToHex(ReadOnlySpan<byte> bytes)
        => Convert.ToHexString(bytes);
}
