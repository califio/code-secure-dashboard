namespace CodeSecure.Application.Module.Integration.Mail;

public record MailAlertSetting: AlertSetting
{
   public List<string> Receivers { get; set; } = new();
}