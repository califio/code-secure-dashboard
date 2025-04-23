using CodeSecure.Application;
using CodeSecure.Application.Module.Integration;
using CodeSecure.Application.Module.Integration.Jira;
using CodeSecure.Application.Module.Integration.JiraWebhook;
using CodeSecure.Application.Module.Integration.Mail;
using CodeSecure.Application.Module.Integration.Redmine;
using CodeSecure.Application.Module.Integration.Teams;
using CodeSecure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Integration;

[ApiExplorerSettings(GroupName = "Integration")]
public class IntegrationController(
    AppDbContext context
) : BaseController
{
    [HttpGet]
    [Permission(PermissionType.Config, PermissionAction.Read)]
    public async Task<IntegrationStatus> GetIntegrationSetting()
    {
        return new IntegrationStatus
        {
            Mail = (await context.GetMailAlertSettingAsync()).Active,
            Jira = (await context.GetJiraSettingAsync()).Active,
            Teams = (await context.GetTeamsAlertSettingAsync()).Active,
            JiraWebhook = (await context.GetJiraWebhookSettingAsync()).Active,
            Redmine = (await context.GetRedmineSettingAsync()).Active,
        };
    }

    [HttpGet]
    [Route("ticket-tracker-status")]
    public async Task<TicketTrackerStatus> GetTicketTrackerStatus()
    {
        return new TicketTrackerStatus
        {
            Jira = (await context.GetJiraSettingAsync()).Active,
            Redmine = (await context.GetRedmineSettingAsync()).Active
        };
    }
}