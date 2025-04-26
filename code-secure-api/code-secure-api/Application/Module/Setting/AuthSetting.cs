using System.ComponentModel.DataAnnotations;
using CodeSecure.Authentication.OpenIdConnect;

namespace CodeSecure.Application.Module.Setting;

public record AuthSetting
{
    public bool DisablePasswordLogon { get; set; }
    public bool AllowRegister { get; set; }
    public string WhiteListEmails { get; set; } = string.Empty;
    [Required]
    public OpenIdConnectSetting OpenIdConnectSetting { get; set; } = new();
}