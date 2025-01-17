using CodeSecure.Api.Admin.Config.Model;
using CodeSecure.Authentication;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Metadata;
using CodeSecure.Manager.Notification;
using CodeSecure.Manager.Setting;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace CodeSecure.Api.Admin.Config.Service;

public class DefaultConfigService(
    IAppSettingManager settingManager,
    INotification notification,
    AuthProviderManager authProviderManager) : IConfigService
{
    public async Task<AuthInfo> GetAuthInfoAsync()
    {
        var config = await settingManager.AppSettingAsync();
        return new AuthInfo
        {
            DisablePasswordLogon = config.AuthSetting.DisablePasswordLogon,
            OpenIdConnectEnable = config.AuthSetting.OpenIdConnectSetting is { Enable: true },
            OpenIdConnectProvider = config.AuthSetting.OpenIdConnectSetting?.DisplayName ?? "Open ID Connect"
        };
    }

    public async Task<MailSetting> GetMailSettingAsync()
    {
        var setting = await settingManager.GetMailSettingAsync();
        return setting with { Password = string.Empty };
    }

    public async Task<MailSetting> UpdateMailSettingAsync(MailSettingRequest request)
    {
        var setting = await settingManager.GetMailSettingAsync();
        if (string.IsNullOrEmpty(request.Password))
        {
            request.Password = setting.Password;
        }

        setting = new MailSetting
        {
            Server = request.Server,
            Port = request.Port,
            UserName = request.UserName,
            Password = request.Password,
            UseSsl = request.UseSsl
        };
        await settingManager.UpdateMailSettingAsync(setting);
        return setting with { Password = string.Empty };
    }

    public async Task<TeamsNotificationSetting> GetTeamsNotificationSettingAsync()
    {
        var setting = await settingManager.GetTeamsNotificationSettingAsync();
        return setting with { Webhook = string.Empty };
    }

    public async Task<TeamsNotificationSetting> UpdateTeamsNotificationSettingAsync(
        TeamsNotificationSettingRequest request)
    {
        var setting = await settingManager.GetTeamsNotificationSettingAsync();
        if (string.IsNullOrEmpty(request.Webhook))
        {
            request.Webhook = setting.Webhook;
        }

        setting = new TeamsNotificationSetting
        {
            Active = request.Active,
            Webhook = request.Webhook,
            SecurityAlertEvent = request.SecurityAlertEvent,
            ScanResultEvent = request.ScanResultEvent,
            NewFindingEvent = request.NewFindingEvent,
            FixedFindingEvent = request.FixedFindingEvent
        };
        return await settingManager.UpdateTeamsNotificationSettingAsync(setting);
    }

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
}