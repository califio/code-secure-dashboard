using CodeSecure.Api.Integration.Model;
using CodeSecure.Enum;
using CodeSecure.Exception;
using CodeSecure.Manager.Integration.Teams;
using CodeSecure.Manager.Integration.TicketTracker.Jira;
using CodeSecure.Manager.Setting;

namespace CodeSecure.Api.Integration.Service;

public class IntegrationService(ISettingManager settingManager, IJiraManager jiraManager) : IIntegrationService
{
    #region Teams

    public async Task<TeamsSetting> GetTeamsSettingAsync()
    {
        var setting = await settingManager.GetTeamsSettingAsync();
        return setting with { Webhook = string.Empty };
    }

    public async Task UpdateTeamsSettingAsync(TeamsSetting request)
    {
        var setting = await settingManager.GetTeamsSettingAsync();
        if (string.IsNullOrEmpty(request.Webhook))
        {
            request.Webhook = setting.Webhook;
        }

        await settingManager.UpdateTeamsSettingAsync(request);
    }

    public async Task TestTeamsSettingAsync(string username)
    {
        var setting = await settingManager.GetTeamsSettingAsync();
        var notification = new TeamsAlert(setting);
        var result = await notification.TestAlert(username);
        if (!result.Succeeded)
        {
            throw new BadRequestException(result.Error);
        }
    }

    #endregion

    public async Task<JiraSetting> GetJiraSettingAsync()
    {
        var setting = await settingManager.GetJiraSettingAsync();
        return setting with { Password = string.Empty };
    }

    public async Task UpdateJiraSettingAsync(JiraSetting request)
    {
        var setting = await settingManager.GetJiraSettingAsync();
        if (string.IsNullOrEmpty(request.Password))
        {
            request.Password = setting.Password;
        }

        await settingManager.UpdateJiraSettingAsync(request);
    }

    public async Task<List<JiraProject>> GetJiraProjectsAsync(JiraSetting? setting, bool reload)
    {
        setting ??= await settingManager.GetJiraSettingAsync();
        var jiraInstance = new JiraManager(setting);
        return await jiraInstance.GetProjectsSummaryAsync(reload);
    }

    public Task<List<string>> GetJiraIssueTypesAsync(string projectKey)
    {
        return jiraManager.GetIssueTypesForProjectAsync(projectKey);
    }

    public async Task<List<TicketTracker>> GetTicketTrackersAsync()
    {
        var jiraSettings = await GetJiraSettingAsync();
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