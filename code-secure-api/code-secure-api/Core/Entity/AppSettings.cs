using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Core.Entity;

public class AppSettings
{
    [Key] public required Guid Id { get; set; }
    public string? AuthSetting { get; set; }
    public string? SlaSastSetting { get; set; }
    public string? SlaScaSetting { get; set; }
    // email
    public string? MailSetting { get; set; }
    public string? MailAlertSetting { get; set; }
    public string? TeamsSetting { get; set; }
    public string? JiraSetting { get; set; }
    public string? JiraWebhookSetting { get; set; }
}