using CodeSecure.Api.Integration.Model;
using CodeSecure.Manager.Integration.TicketTracker.Jira;
using CodeSecure.Manager.Setting;

namespace CodeSecure.Api.Integration.Service;

public interface IIntegrationService
{
    // ms teams
    Task<TeamsSetting> GetTeamsSettingAsync();
    Task UpdateTeamsSettingAsync(TeamsSetting setting);
    Task TestTeamsSettingAsync(string username);
    // jira
    Task<JiraSetting> GetJiraSettingAsync();
    Task UpdateJiraSettingAsync(JiraSetting setting);
    Task<List<JiraProject>> GetJiraProjectsAsync(JiraSetting? setting, bool reload);
    Task<List<string>> GetJiraIssueTypesAsync(string projectKey);
    Task<List<TicketTracker>> GetTicketTrackersAsync();
}