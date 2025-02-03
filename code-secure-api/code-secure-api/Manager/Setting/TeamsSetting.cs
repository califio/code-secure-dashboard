namespace CodeSecure.Manager.Setting;

public record TeamsSetting: AlertSetting
{
    public string Webhook { get; set; } = string.Empty;
}