using CodeSecure.Application.Module.Integration.Mail;
using CodeSecure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Integration;

[Route("api/integration/mail")]
[ApiExplorerSettings(GroupName = "Integration")]
public class MailIntegrationController(IMailAlertSettingService mailAlertSettingService): BaseController
{
    [HttpGet]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public Task<MailAlertSetting> GetMailIntegrationSetting()
    {
        return mailAlertSettingService.GetSettingAsync();
    }

    [HttpPost]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public Task UpdateMailIntegrationSetting([FromBody] MailAlertSetting request)
    {
        return mailAlertSettingService.UpdateSettingAsync(request);
    }
}