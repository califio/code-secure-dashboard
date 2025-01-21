using CodeSecure.Database.Metadata;

namespace CodeSecure.Manager.Setting;

public class AppSetting
{
    public required SLA SlaSastSetting { get; set; }
    public required SLA SlaScaSetting { get; set; }
    public required AuthSetting AuthSetting { get; set; }
    public required MailSetting MailSetting { get; set; }
    public required TeamsNotificationSetting TeamsNotificationSetting { get; set; }
    
}