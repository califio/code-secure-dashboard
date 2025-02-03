using CodeSecure.Api.Setting.Model;
using CodeSecure.Api.Setting.Service;
using CodeSecure.Authentication;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Database.Extension;
using CodeSecure.Exception;
using CodeSecure.Manager.EnvVariable;
using CodeSecure.Manager.Integration;
using CodeSecure.Manager.Integration.Mail;
using CodeSecure.Manager.Setting;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Setting;

[Route("api/setting")]
public class SettingController(
    ISettingService settingService,
    IEnvVariableManager envVariableManager,
    IMailSender mailSender
) : BaseController
{
    [HttpGet]
    [Route("mail")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<MailSetting> GetMailSetting()
    {
        return await settingService.GetMailSettingAsync();
    }

    [HttpPost]
    [Route("mail")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task UpdateMailSetting([FromBody] MailSetting request)
    {
        await settingService.UpdateMailSettingAsync(request);
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
    [Route("auth")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<AuthSetting> GetAuthSetting()
    {
        return await settingService.GetAuthSettingAsync();
    }

    [HttpPost]
    [Route("auth")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<AuthSetting> UpdateAuthSetting([FromBody] AuthSetting request)
    {
        return await settingService.UpdateAuthSettingAsync(request);
    }

    [HttpGet]
    [Route("sla")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<SlaSetting> GetSlaSetting()
    {
        return await settingService.GetSlaSettingAsync();
    }

    [HttpPost]
    [Route("sla")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<SlaSetting> UpdateSlaSetting([FromBody] SlaSetting request)
    {
        return await settingService.UpdateSlaSettingAsync(request);
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