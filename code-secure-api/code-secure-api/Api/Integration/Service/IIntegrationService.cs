using CodeSecure.Api.Integration.Model;
using CodeSecure.Manager.Integration.TicketTracker.Jira;
using CodeSecure.Manager.Setting;

namespace CodeSecure.Api.Integration.Service;

public interface IIntegrationService
{
    Task<IntegrationSetting> GetIntegrationSettingAsync();
    // mail
    Task<MailAlertSetting> GetMailIntegrationSettingAsync();
    Task UpdateMailIntegrationSettingAsync(MailAlertSetting request);
    // ms teams
    Task<TeamsSetting> GetTeamsIntegrationSettingAsync();
    Task UpdateTeamsIntegrationSettingAsync(TeamsSetting setting);
    Task TestTeamsIntegrationSettingAsync(string username);
    // jira
    Task<JiraSetting> GetJiraIntegrationSettingAsync();
    Task UpdateJiraIntegrationSettingAsync(JiraSetting setting);
    Task TestJiraIntegrationSettingAsync();
    Task<List<JiraProject>> GetJiraProjectsAsync(JiraSetting? setting, bool reload);
    Task<List<string>> GetJiraIssueTypesAsync(string projectKey);
    Task<List<TicketTracker>> GetTicketTrackersAsync();
}