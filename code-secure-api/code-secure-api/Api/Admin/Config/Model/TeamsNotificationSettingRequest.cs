using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Api.Admin.Config.Model;

public class TeamsNotificationSettingRequest
{
    [Required]
    public bool Active { get; set; }
    
    public string Webhook { get; set; } = string.Empty;
    [Required]
    public bool SecurityAlertEvent { get; set; }
    [Required]
    public bool ScanResultEvent { get; set; }
    [Required]
    public bool NewFindingEvent { get; set; }
    [Required]
    public bool FixedFindingEvent { get; set; }
}