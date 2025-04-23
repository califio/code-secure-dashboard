using CodeSecure.Application.Module.Project;
using CodeSecure.Application.Module.Project.Integration;
using CodeSecure.Application.Module.Project.Integration.Mail;
using CodeSecure.Authentication;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Project;

[Route("api/project")]
[ApiExplorerSettings(GroupName = "Project")]
public class MailIntegrationController(
    IMailProjectIntegrationSetting mailProjectIntegrationSetting,
    IProjectAuthorize projectAuthorize
) : BaseController
{
    [HttpGet]
    [Route("{projectId:guid}/integration/mail")]
    public async Task<ProjectAlertEvent> GetMailIntegrationProject(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        var result = await mailProjectIntegrationSetting.GetSettingAsync(projectId);
        return result.GetResult();
    }

    [HttpPost]
    [Route("{projectId:guid}/integration/mail")]
    public async Task<bool> UpdateMailIntegrationProject(Guid projectId, [FromBody] MailProjectAlertSetting request)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        var result = await mailProjectIntegrationSetting.UpdateSettingAsync(projectId, request);
        return result.GetResult();
    }
}