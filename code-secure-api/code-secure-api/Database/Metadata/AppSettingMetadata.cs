using System.ComponentModel.DataAnnotations;
using CodeSecure.Authentication.OpenIdConnect;

namespace CodeSecure.Database.Metadata;

public record AuthSetting
{
    [Required]
    public bool DisablePasswordLogon { get; set; }
    [Required]
    public bool AllowRegister { get; set; }
    public string WhiteListEmails { get; set; } = string.Empty;
    [Required] 
    public required OpenIdConnectSetting OpenIdConnectSetting { get; set; }
}

// Service Level Agreement (SLA): day unit
public record SLA
{
    [Required] public int Critical { get; set; }
    [Required] public int High { get; set; }
    [Required] public int Medium { get; set; }
    [Required] public int Low { get; set; }
    [Required] public int Info { get; set; }
}

public record MailSetting
{
    [Required] 
    public string Server { get; set; } = string.Empty;
    [Required] public int Port { get; set; }
    
    [Required]
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    [Required] public bool UseSsl { get; set; }
}

public record TeamsNotificationSetting
{
    public bool Active { get; set; }
    public string Webhook { get; set; } = string.Empty;
    public bool SecurityAlertEvent { get; set; } = true;
    public bool ScanResultEvent { get; set; } = true;
    public bool NewFindingEvent { get; set; } = true;
    public bool FixedFindingEvent { get; set; } = true;
}