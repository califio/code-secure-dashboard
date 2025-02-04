namespace CodeSecure.Manager.Setting;

public record MailAlertSetting: AlertSetting
{
   public List<string> Receivers { get; set; } = new();
}