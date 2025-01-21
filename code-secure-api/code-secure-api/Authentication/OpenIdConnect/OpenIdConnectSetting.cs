using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace CodeSecure.Authentication.OpenIdConnect;

public class OpenIdConnectSetting
{
    public string DisplayName { get; set; } = "Open ID Connect";
    public string Authority { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public bool Enable { get; set; } = false;

    public OpenIdConnectOptions ToOpenIdConnectOptions()
    {
        var options = new OpenIdConnectOptions
        {
            Authority = Authority,
            ClientId = ClientId,
            ClientSecret = ClientSecret,
            CallbackPath = "/auth/oidc/callback",
            RequireHttpsMetadata = false,
            ResponseType = OpenIdConnectResponseType.Code,
            SaveTokens = true,
            GetClaimsFromUserInfoEndpoint = true,
            Scope = { OpenIdConnectScope.OpenIdProfile, OpenIdConnectScope.Email },
            TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateLifetime = true
            }
        };
        return options;
    }
}