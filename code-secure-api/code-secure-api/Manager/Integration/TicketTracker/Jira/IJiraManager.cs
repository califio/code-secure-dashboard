using Atlassian.Jira;

namespace CodeSecure.Manager.Integration.TicketTracker.Jira;

public interface IJiraManager
{
    Task<List<Atlassian.Jira.Project>> GetProjectsAsync(bool reload);
    Task<List<JiraProject>> GetProjectsSummaryAsync(bool reload = false);
    Task<List<string>> GetIssueTypesForProjectAsync(string projectKey);
    Task<Atlassian.Jira.Project?> GetProjectAsync(string key);
    Task<Issue> CreateIssueAsync(JiraIssue issue);
}