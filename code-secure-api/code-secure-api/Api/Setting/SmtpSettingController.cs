using CodeSecure.Application;
using CodeSecure.Application.Module.Setting;
using CodeSecure.Application.Services;
using CodeSecure.Authentication;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Setting;

[Route("api/setting/smtp")]
[ApiExplorerSettings(GroupName = "Setting")]
public class SmtpSettingController(AppDbContext context, ISmtpService smtpService): BaseController
{
    [HttpGet]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<SmtpSetting> GetSmtpSetting()
    {
        var setting = await context.GetSmtpSettingAsync();
        return setting with { Password = string.Empty };
    }

    [HttpPost]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task UpdateSmtpSetting([FromBody] SmtpSetting request)
    {
        await context.UpdateSmtpSettingAsync(request);
    }

    [HttpPost]
    [Route("test")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<bool> TestSmtpSetting(string? email)
    {
        if (string.IsNullOrEmpty(email))
        {
            email = CurrentUser.Email;
        }
        var result = await smtpService.TestConnectionAsync(email);
        return result.GetResult();
    }
}