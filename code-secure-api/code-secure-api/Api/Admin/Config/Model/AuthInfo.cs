namespace CodeSecure.Api.Admin.Config.Model;

public record AuthInfo
{
    public bool DisablePasswordLogon { get; set; }
    public bool OpenIdConnectEnable { get; set; }
    public string OpenIdConnectProvider { get; set; } = string.Empty;
}