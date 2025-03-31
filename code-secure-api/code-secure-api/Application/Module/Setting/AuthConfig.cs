namespace CodeSecure.Application.Module.Setting;

public record AuthConfig
{
    public bool DisablePasswordLogon { get; set; }
    public bool OpenIdConnectEnable { get; set; }
    public string OpenIdConnectProvider { get; set; } = string.Empty;
}