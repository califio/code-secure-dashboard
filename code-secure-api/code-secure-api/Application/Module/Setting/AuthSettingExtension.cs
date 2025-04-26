using CodeSecure.Authentication;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Utils;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace CodeSecure.Application.Module.Setting;

public static class AuthSettingExtension
{
    private static AuthSetting? authSetting;
    
    public static async Task<AuthSetting> GetAuthSettingAsync(this AppDbContext context)
    {
        if (authSetting == null)
        {
            var setting = await context.GetAppSettingsAsync();
            authSetting = JSONSerializer.DeserializeOrDefault(setting.AuthSetting, new AuthSetting());
        }
        return authSetting with {};
    }
    
    public static async Task UpdateAuthSettingAsync(this AppDbContext context, AuthProviderManager authProviderManager, AuthSetting request)
    {
        var setting = await context.GetAppSettingsAsync();
        setting.AuthSetting = JSONSerializer.Serialize(request);
        context.AppSettings.Update(setting);
        await context.SaveChangesAsync();
        authSetting = request;
        if (request.OpenIdConnectSetting.Enable)
        {
            var authProvider = await authProviderManager.FindBySchemeAsync(OpenIdConnectDefaults.AuthenticationScheme);
            if (authProvider == null)
            {
                await authProviderManager.AddAsync(new AuthProviders
                {
                    Scheme = OpenIdConnectDefaults.AuthenticationScheme,
                    HandlerType =
                        authProviderManager.ManagedHandlerType.First(t => t.Name == nameof(OpenIdConnectHandler)),
                    DisplayName = request.OpenIdConnectSetting.DisplayName,
                    Options = request.OpenIdConnectSetting.ToOpenIdConnectOptions(),
                    Enable = request.OpenIdConnectSetting.Enable
                });
            }
            else
            {
                if (authProvider.Options is OpenIdConnectOptions authOptions)
                {
                    authOptions.Authority = request.OpenIdConnectSetting.Authority;
                    authOptions.ClientId = request.OpenIdConnectSetting.ClientId;
                    authOptions.ClientSecret = request.OpenIdConnectSetting.ClientSecret;
                    authProvider.Options = authOptions;
                }

                authProvider.DisplayName = request.OpenIdConnectSetting.DisplayName;
                authProvider.Enable = request.OpenIdConnectSetting.Enable;
                await authProviderManager.UpdateAsync(authProvider);
            }
        }
    }
}