using System.ComponentModel.DataAnnotations;
using CodeSecure.Authentication.OpenIdConnect;

namespace CodeSecure.Manager.Setting;

public class AuthSetting
{
    public bool DisablePasswordLogon { get; set; }
    public bool AllowRegister { get; set; }
    public string WhiteListEmails { get; set; } = string.Empty;
    [Required]
    public OpenIdConnectSetting OpenIdConnectSetting { get; set; } = new();
}