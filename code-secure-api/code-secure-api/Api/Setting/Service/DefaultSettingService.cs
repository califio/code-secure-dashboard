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
        var setting = await settingManager.AppSettingAsync();
        return setting.AuthSetting;
    }

    public async Task<AuthSetting> UpdateAuthSettingAsync(AuthSetting request)
    {
        var setting = await settingManager.AppSettingAsync();
        setting.AuthSetting = request;
        setting = await settingManager.UpdateAppSettingAsync(setting);
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

        return setting.AuthSetting;
    }

    public async Task<SlaSetting> GetSlaSettingAsync()
    {
        var setting = await settingManager.AppSettingAsync();
        return new SlaSetting
        {
            Sast = setting.SlaSastSetting,
            Sca = setting.SlaScaSetting
        };
    }

    public async Task<SlaSetting> UpdateSlaSettingAsync(SlaSetting request)
    {
        var setting = await settingManager.AppSettingAsync();
        setting.SlaSastSetting = request.Sast;
        setting.SlaScaSetting = request.Sca;
        await settingManager.UpdateAppSettingAsync(setting);
        return request;
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