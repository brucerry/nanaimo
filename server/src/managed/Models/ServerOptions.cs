namespace FlightIslandServer.Desktop.Models;

public sealed class ServerOptions
{
    public string BindAddress { get; set; } = "127.0.0.1";
    public int GameServerPort { get; set; } = 11005;
    public int WorldServerPort { get; set; } = 12050;
    public int WebAdminPort { get; set; } = 22222;
    public bool DailyRegistrationLimitEnabled { get; set; }
    public int DailyRegistrationLimit { get; set; } = 100;
    public bool IpRegistrationLimitEnabled { get; set; }
    public int IpRegistrationLimit { get; set; } = 5;
    public bool IpDailyRegistrationLimitEnabled { get; set; }
    public int IpDailyRegistrationLimit { get; set; } = 3;
    public bool LoginAuthorizationRequired { get; set; }
    public bool PerAccountTrialLimitEnabled { get; set; }
    public bool GlobalTrialLimitEnabled { get; set; }
    public int GlobalTrialMinutes { get; set; } = 10;
    public List<int> AdminModuleOrder { get; set; } = [];
}
