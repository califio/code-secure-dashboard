using System.ComponentModel.DataAnnotations;
using CodeSecure.Application.Module.Integration;
using CodeSecure.Application.Module.Integration.Jira;
using CodeSecure.Application.Module.Integration.Jira.Client;
using CodeSecure.Application.Module.Integration.JiraWebhook;
using CodeSecure.Application.Module.Integration.Mail;
using CodeSecure.Application.Module.Integration.Teams;
using CodeSecure.Authentication;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Integration;

public class IntegrationController(
    IMailAlertSettingService mailAlertSettingService,
    IJiraSettingService jiraSettingService,
    IJiraWebhookSettingService jiraWebhookSettingService,
    ITeamsAlertSettingService teamsAlertSettingService
) : BaseController
{
    [HttpGet]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<IntegrationStatus> GetIntegrationSetting()
    {
        return new IntegrationStatus
        {
            Mail = (await mailAlertSettingService.GetSettingAsync()).Active,
            Jira = (await jiraSettingService.GetSettingAsync()).Active,
            Teams = (await teamsAlertSettingService.GetSettingAsync()).Active,
            JiraWebhook = (await jiraWebhookSettingService.GetSettingAsync()).Active,
        };
    }

    #region Mail

    [HttpGet]
    [Route("mail")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public Task<MailAlertSetting> GetMailIntegrationSetting()
    {
        return mailAlertSettingService.GetSettingAsync();
    }

    [HttpPost]
    [Route("mail")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public Task UpdateMailIntegrationSetting([FromBody] MailAlertSetting request)
    {
        return mailAlertSettingService.UpdateSettingAsync(request);
    }

    #endregion

    #region Teams

    [HttpGet]
    [Route("teams")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<TeamsAlertSetting> GetTeamsIntegrationSetting()
    {
        return (await teamsAlertSettingService.GetSettingAsync()) with{Webhook = string.Empty};
    }

    [HttpPost]
    [Route("teams")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<bool> UpdateTeamsIntegrationSetting([FromBody] TeamsAlertSetting request)
    {
        var result = await teamsAlertSettingService.UpdateSettingAsync(request);
        return result.GetResult();
    }

    [HttpPost]
    [Route("teams/test")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<bool> TestTeamsIntegrationSetting()
    {
        var result = await teamsAlertSettingService.TestConnectionAsync();
        return result.GetResult();
    }

    #endregion

    #region Jira

    [HttpGet]
    [Route("jira")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<JiraSetting> GetJiraIntegrationSetting()
    {
        var setting = await jiraSettingService.GetSettingAsync();
        return setting with { Password = string.Empty };
    }

    [HttpPost]
    [Route("jira")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<bool> UpdateJiraIntegrationSetting([FromBody] JiraSetting request)
    {
        var result = await jiraSettingService.UpdateSettingAsync(request);
        return result.GetResult();
    }

    [HttpPost]
    [Route("jira/test")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<bool> TestJiraIntegrationSetting()
    {
        var result = await jiraSettingService.TestConnectionAsync();
        return result.GetResult();
    }

    [HttpPost]
    [Route("jira/projects")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<List<JiraProject>> GetJiraProjects([FromBody] JiraSetting? setting = null, bool reload = false)
    {
        setting ??= await jiraSettingService.GetSettingAsync();
        var jiraInstance = new JiraClient(new JiraConnection
        {
            Url = setting.WebUrl,
            Password = setting.Password,
            Username = setting.UserName
        });
        return await jiraInstance.GetProjectsSummaryAsync(reload);
    }

    [HttpPost]
    [Route("jira/issue-types")]
    //[Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<List<string>> GetJiraIssueTypes([Required] string projectKey)
    {
        var jiraSetting = await jiraSettingService.GetSettingAsync();
        return await new JiraClient(new JiraConnection
        {
            Url = jiraSetting.WebUrl,
            Password = jiraSetting.Password,
            Username = jiraSetting.UserName
        }).GetIssueTypesForProjectAsync(projectKey);
    }

    #endregion
    
    #region JiraWebhook
    [HttpGet]
    [Route("jira-webhook")]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<JiraWebhookSetting> GetJiraWebhookIntegrationSetting()
    {
        var setting = await jiraWebhookSettingService.GetSettingAsync();
        return setting with { Token = string.Empty };
    }
    [HttpPost]
    [Route("jira-webhook")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<bool> UpdateJiraWebhookIntegrationSetting([FromBody] JiraWebhookSetting request)
    {
        var result = await jiraWebhookSettingService.UpdateSettingAsync(request);
        return result.GetResult();
    }

    [HttpPost]
    [Route("jira-webhook/test")]
    [Permission(PermissionType.Config, PermissionAction.Update)]
    public async Task<bool> TestJiraWebhookIntegrationSetting()
    {
        var result = await jiraWebhookSettingService.TestConnectionAsync();
        return result.GetResult();
    }
    #endregion

    [HttpGet]
    [Route("ticket-trackers")]
    public async Task<List<TicketTracker>> GetTicketTrackers()
    {
        var jiraSettings = await jiraSettingService.GetSettingAsync();
        return
        [
            new TicketTracker
            {
                Active = jiraSettings.Active,
                Type = TicketType.Jira
            }
        ];
    }
}