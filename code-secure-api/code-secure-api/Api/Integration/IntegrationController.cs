using System.ComponentModel.DataAnnotations;
using CodeSecure.Api.Integration.Model;
using CodeSecure.Api.Integration.Service;
using CodeSecure.Authentication;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Manager.Integration.TicketTracker.Jira;
using CodeSecure.Manager.Setting;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Integration;

public class IntegrationController(IIntegrationService integrationService) : BaseController
{
    [HttpGet]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<IntegrationSetting> GetIntegrationSetting()
    {
        return await integrationService.GetIntegrationSettingAsync();
    }

    #region Mail

    [HttpGet]
    [Route("mail")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public Task<MailAlertSetting> GetMailIntegrationSetting()
    {
        return integrationService.GetMailIntegrationSettingAsync();
    }

    [HttpPost]
    [Route("mail")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public Task UpdateMailIntegrationSetting([FromBody] MailAlertSetting request)
    {
        return integrationService.UpdateMailIntegrationSettingAsync(request);
    }

    #endregion

    #region Teams

    [HttpGet]
    [Route("teams")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public Task<TeamsSetting> GetTeamsIntegrationSetting()
    {
        return integrationService.GetTeamsIntegrationSettingAsync();
    }

    [HttpPost]
    [Route("teams")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public Task UpdateTeamsIntegrationSetting([FromBody] TeamsSetting request)
    {
        return integrationService.UpdateTeamsIntegrationSettingAsync(request);
    }

    [HttpPost]
    [Route("teams/test")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public Task TestTeamsIntegrationSetting()
    {
        return integrationService.TestTeamsIntegrationSettingAsync(User.UserClaims().Email);
    }

    #endregion

    #region Jira

    [HttpGet]
    [Route("jira")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public Task<JiraSetting> GetJiraIntegrationSetting()
    {
        return integrationService.GetJiraIntegrationSettingAsync();
    }

    [HttpPost]
    [Route("jira")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public Task UpdateJiraIntegrationSetting([FromBody] JiraSetting request)
    {
        return integrationService.UpdateJiraIntegrationSettingAsync(request);
    }

    [HttpPost]
    [Route("jira/test")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public Task TestJiraIntegrationSetting()
    {
        return integrationService.TestJiraIntegrationSettingAsync();
    }

    [HttpPost]
    [Route("jira/projects")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public Task<List<JiraProject>> GetJiraProjects([FromBody] JiraSetting? setting = null, bool reload = false)
    {
        return integrationService.GetJiraProjectsAsync(setting, reload);
    }

    [HttpPost]
    [Route("jira/issue-types")]
    //[Permission(PermissionType.Config, PermissionAction.Read)]
    public Task<List<string>> GetJiraIssueTypes([Required] string projectKey)
    {
        return integrationService.GetJiraIssueTypesAsync(projectKey);
    }

    #endregion

    [HttpGet]
    [Route("ticket-trackers")]
    public Task<List<TicketTracker>> GetTicketTrackers()
    {
        return integrationService.GetTicketTrackersAsync();
    }
}