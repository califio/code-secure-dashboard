using System.ComponentModel.DataAnnotations;
using CodeSecure.Api.Integration.Model;
using CodeSecure.Api.Integration.Service;
using CodeSecure.Authentication;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Manager.Integration.TicketTracker.Jira;
using CodeSecure.Manager.Setting;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Integration;

public class IntegrationController(IIntegrationService integrationService): BaseController
{
    #region Teams
    [HttpGet]
    [Route("teams")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public Task<TeamsSetting> GetTeamsSetting()
    {
        return integrationService.GetTeamsSettingAsync();
    }

    [HttpPost]
    [Route("teams")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public Task UpdateTeamsSetting([FromBody] TeamsSetting request)
    {
        return integrationService.UpdateTeamsSettingAsync(request);
    }

    [HttpPost]
    [Route("teams/test")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public Task TestTeamsSetting()
    {
        return integrationService.TestTeamsSettingAsync(User.UserClaims().Email);
    }
    #endregion

    #region Jira

    [HttpGet]
    [Route("jira")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public Task<JiraSetting> GetJiraSetting()
    {
        return integrationService.GetJiraSettingAsync();
    }

    [HttpPost]
    [Route("jira")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public Task UpdateJiraSetting([FromBody] JiraSetting request)
    {
        return integrationService.UpdateJiraSettingAsync(request);
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