using CodeSecure.Application;
using CodeSecure.Application.Module.Setting;
using CodeSecure.Application.Services;
using CodeSecure.Authentication;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Setting;

[Route("api/setting")]
public class SettingController(
    AppDbContext context,
    IAuthSetting authSetting,
    ISmtpService smtpService
) : BaseController
{
    [HttpGet]
    [Route("smtp")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<SmtpSetting> GetSmtpSetting()
    {
        var setting = await context.GetSmtpSettingAsync();
        return setting with { Password = string.Empty };
    }

    [HttpPost]
    [Route("smtp")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task UpdateSmtpSetting([FromBody] SmtpSetting request)
    {
        await context.UpdateSmtpSettingAsync(request);
    }

    [HttpPost]
    [Route("smtp/test")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<bool> TestSmtpSetting(string? email)
    {
        if (string.IsNullOrEmpty(email))
        {
            email = CurrentUser().Email;
        }

        var result = await smtpService.TestConnectionAsync(email);
        return result.GetResult();
    }

    [HttpGet]
    [Route("auth")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public Task<AuthSetting> GetAuthSetting()
    {
        return authSetting.GetAuthSettingAsync();
    }

    [HttpPost]
    [Route("auth")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task UpdateAuthSetting([FromBody] AuthSetting request)
    {
        await authSetting.UpdateAuthSettingAsync(request);
    }

    [HttpGet]
    [Route("sla")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<SlaSetting> GetSlaSetting()
    {
        return await context.GetSlaSettingAsync();
    }

    [HttpPost]
    [Route("sla")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task UpdateSlaSetting([FromBody] SlaSetting request)
    {
        await context.UpdateSlaSettingAsync(request);
    }
}