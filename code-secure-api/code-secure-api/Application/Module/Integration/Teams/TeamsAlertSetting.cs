namespace CodeSecure.Application.Module.Integration.Teams;

public record TeamsAlertSetting: AlertSetting
{
    public string Webhook { get; set; } = string.Empty;
}