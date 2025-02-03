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

    public async Task<IntegrationSetting> GetIntegrationSettingAsync()
    {
        return new IntegrationSetting
        {
            Mail = (await settingManager.GetMailAlertSettingAsync()).Active,
            Jira = (await settingManager.GetJiraSettingAsync()).Active,
            Teams = (await settingManager.GetTeamsSettingAsync()).Active,
        };
    }

    public async Task<MailAlertSetting> GetMailIntegrationSettingAsync()
    {
        return await settingManager.GetMailAlertSettingAsync();
    }

    public async Task UpdateMailIntegrationSettingAsync(MailAlertSetting request)
    {
        await settingManager.UpdateMailAlertSettingAsync(request);
    }

    public async Task<TeamsSetting> GetTeamsIntegrationSettingAsync()
    {
        var setting = await settingManager.GetTeamsSettingAsync();
        return setting with { Webhook = string.Empty };
    }

    public async Task UpdateTeamsIntegrationSettingAsync(TeamsSetting request)
    {
        var setting = await settingManager.GetTeamsSettingAsync();
        if (string.IsNullOrEmpty(request.Webhook))
        {
            request.Webhook = setting.Webhook;
        }

        await settingManager.UpdateTeamsSettingAsync(request);
    }

    public async Task TestTeamsIntegrationSettingAsync(string username)
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

    public async Task<JiraSetting> GetJiraIntegrationSettingAsync()
    {
        var setting = await settingManager.GetJiraSettingAsync();
        return setting with { Password = string.Empty };
    }

    public async Task UpdateJiraIntegrationSettingAsync(JiraSetting request)
    {
        var setting = await settingManager.GetJiraSettingAsync();
        if (string.IsNullOrEmpty(request.Password))
        {
            request.Password = setting.Password;
        }

        await settingManager.UpdateJiraSettingAsync(request);
    }

    public async Task TestJiraIntegrationSettingAsync()
    {
        try
        {
            await jiraManager.TestConnection();
        }
        catch (System.Exception e)
        {
            throw new BadRequestException(e.Message);
        }
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
        var jiraSettings = await GetJiraIntegrationSettingAsync();
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