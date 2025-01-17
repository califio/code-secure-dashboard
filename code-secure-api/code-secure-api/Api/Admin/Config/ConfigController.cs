using CodeSecure.Api.Admin.Config.Model;
using CodeSecure.Api.Admin.Config.Service;
using CodeSecure.Authentication;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Database.Extension;
using CodeSecure.Database.Metadata;
using CodeSecure.Exception;
using CodeSecure.Manager.EnvVariable;
using CodeSecure.Manager.Notification;
using CodeSecure.Manager.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Admin.Config;

public class ConfigController(
    IConfigService configService,
    IEnvVariableManager envVariableManager,
    IMailSender mailSender,
    IAppSettingManager settingManager
) : BaseController
{
    [HttpGet]
    [Route("authInfo")]
    [AllowAnonymous]
    public async Task<AuthInfo> GetAuthInfo()
    {
        return await configService.GetAuthInfoAsync();
    }

    [HttpGet]
    [Route("mail")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<MailSetting> GetMailSetting()
    {
        return await configService.GetMailSettingAsync();
    }

    [HttpPost]
    [Route("mail")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<MailSetting> UpdateMailSetting(MailSettingRequest request)
    {
        return await configService.UpdateMailSettingAsync(request);
    }
    
    [HttpPost]
    [Route("mail/test")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<string> TestMailSetting(string? email)
    {
        if (string.IsNullOrEmpty(email))
        {
            email = User.UserClaims().Email;
        }

        var result = await mailSender.SendTestMailAsync(email);
        if (!result.Succeeded)
        {
            throw new BadRequestException(result.Error);
        }

        return email;
    }
    
    [HttpGet]
    [Route("teams")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<TeamsNotificationSetting> GetTeamsSetting()
    {
        return await configService.GetTeamsNotificationSettingAsync();
    }

    [HttpPost]
    [Route("teams")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<TeamsNotificationSetting> UpdateTeamsSetting(TeamsNotificationSettingRequest request)
    {
        return await configService.UpdateTeamsNotificationSettingAsync(request);
    }
    
    [HttpPost]
    [Route("teams/test")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task TestTeamsSetting()
    {
        var setting = await settingManager.GetTeamsNotificationSettingAsync();
        var notification = new TeamsNotification(setting);
        var result = await notification.PushTestNotification(User.UserClaims().Email);
        if (!result.Succeeded)
        {
            throw new BadRequestException(result.Error);
        }
    }
    
    [HttpGet]
    [Route("auth")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<AuthSetting> GetAuthSetting()
    {
        return await configService.GetAuthSettingAsync();
    }

    [HttpPost]
    [Route("auth")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<AuthSetting> UpdateAuthSetting(AuthSetting request)
    {
        return await configService.UpdateAuthSettingAsync(request);
    }

    [HttpGet]
    [Route("sla")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<SlaSetting> GetSlaSetting()
    {
        return await configService.GetSlaSettingAsync();
    }

    [HttpPost]
    [Route("sla")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<SlaSetting> UpdateSlaSetting(SlaSetting request)
    {
        return await configService.UpdateSlaSettingAsync(request);
    }

    [HttpPost]
    [Route("env/filter")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<Page<string>> GetEnvVariable(QueryFilter filter)
    {
        return await envVariableManager.FilterAsync(filter);
    }

    [HttpPost]
    [Route("env")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task CreateEnvVariable(EnvVariableRequest request)
    {
        await envVariableManager.CreateAsync(request.Name);
    }

    [HttpDelete]
    [Route("env/{env}")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task DeleteEnvVariable(string env)
    {
        await envVariableManager.RemoveAsync(env);
    }
}