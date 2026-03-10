using CodeSecure.Application;
using CodeSecure.Application.Module.Setting;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

namespace CodeSecure.Authentication.OpenIdConnect;

/// <summary>
/// Applies the SchemeOverride from OpenIdConnectSetting to the redirect_uri sent to the OIDC provider.
/// Using IPostConfigureOptions ensures this runs on every options resolution, including after
/// Aguacongas reloads providers from the database on startup.
/// </summary>
public class OpenIdConnectSchemeOverrideOptions(IServiceScopeFactory scopeFactory)
    : IPostConfigureOptions<OpenIdConnectOptions>
{
    public void PostConfigure(string? name, OpenIdConnectOptions options)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var setting = context.GetAuthSettingAsync().GetAwaiter().GetResult();
        var schemeOverride = setting.OpenIdConnectSetting.SchemeOverride;

        if (string.IsNullOrWhiteSpace(schemeOverride))
            return;

        var previous = options.Events?.OnRedirectToIdentityProvider;
        options.Events ??= new OpenIdConnectEvents();
        options.Events.OnRedirectToIdentityProvider = async ctx =>
        {
            if (previous != null)
                await previous(ctx);

            var uri = ctx.ProtocolMessage.RedirectUri;
            var schemeEnd = uri?.IndexOf("://") ?? -1;
            if (schemeEnd >= 0)
                ctx.ProtocolMessage.RedirectUri = schemeOverride + uri![schemeEnd..];
        };
    }
}
