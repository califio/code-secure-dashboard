using CodeSecure.Api.Setting.Model;
using CodeSecure.Authentication;
using CodeSecure.Database.Entity;
using CodeSecure.Manager.Setting;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace CodeSecure.Api.Setting.Service;

public class DefaultSettingService(
    ISettingManager settingManager,
    AuthProviderManager authProviderManager) : ISettingService
{
    public async Task<AuthSetting> GetAuthSettingAsync()
    {
        return await settingManager.GetAuthSettingAsync();
    }

    public async Task UpdateAuthSettingAsync(AuthSetting request)
    {
        await settingManager.UpdateAuthSettingAsync(request);
        if (request.OpenIdConnectSetting != null)
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
                if (authProvider.Options is OpenIdConnectOptions authOptions) // GoogleOptions is OAuthOptions
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

    public async Task<SlaSetting> GetSlaSettingAsync()
    {
        return new SlaSetting
        {
            Sast = await settingManager.GetSlaSastSettingAsync(),
            Sca = await settingManager.GetSlaScaSettingAsync(),
        };
    }

    public async Task UpdateSlaSettingAsync(SlaSetting request)
    {
        await settingManager.UpdateSlaSastSettingAsync(request.Sast);
        await settingManager.UpdateSlaScaSettingAsync(request.Sca);
    }

    public async Task<MailSetting> GetMailSettingAsync()
    {
        var setting = await settingManager.GetMailSettingAsync();
        return setting with { Password = string.Empty };
    }

    public async Task UpdateMailSettingAsync(MailSetting request)
    {
        var setting = await settingManager.GetMailSettingAsync();
        if (string.IsNullOrEmpty(request.Password))
        {
            request.Password = setting.Password;
        }
        
        await settingManager.UpdateMailSettingAsync(request);
    }
}