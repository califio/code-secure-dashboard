using Atlassian.Jira;

namespace CodeSecure.Application.Module.Integration.Jira.Client;

public interface IJiraClient
{
    Task TestConnection();
    Task<List<Atlassian.Jira.Project>> GetProjectsAsync(bool reload);
    Task<List<JiraProject>> GetProjectsSummaryAsync(bool reload = false);
    Task<List<string>> GetIssueTypesForProjectAsync(string projectKey);
    Task<Atlassian.Jira.Project?> GetProjectAsync(string key);
    Task<Issue> CreateIssueAsync(JiraIssue issue);
}

public class JiraClient(JiraConnection setting) : IJiraClient
{
    private static List<Atlassian.Jira.Project>? jiraProjects;

    private readonly Atlassian.Jira.Jira jiraClient = Atlassian.Jira.Jira.CreateRestClient(setting.Url,
        setting.Username, setting.Password, new JiraRestClientSettings()
        {
            EnableUserPrivacyMode = true,
        });

    public async Task TestConnection()
    {
        await jiraClient.Users.GetMyselfAsync();
    }

    public async Task<List<Atlassian.Jira.Project>> GetProjectsAsync(bool reload = false)
    {
        if (reload || jiraProjects == null)
        {
            try
            {
                jiraProjects = (await jiraClient.Projects.GetProjectsAsync()).ToList();
            }
            catch (Exception)
            {
                return [];
            }
        }

        return jiraProjects;
    }

    public async Task<List<JiraProject>> GetProjectsSummaryAsync(bool reload)
    {
        return (await GetProjectsAsync(reload)).Select(item => new JiraProject
        {
            Key = item.Key,
            Name = item.Name
        }).ToList();
    }

    public async Task<List<string>> GetIssueTypesForProjectAsync(string projectKey)
    {
        try
        {
            var issueTypes = await jiraClient.IssueTypes.GetIssueTypesForProjectAsync(projectKey);
            return issueTypes.Select(item => item.Name).ToList();
        }
        catch (Exception)
        {
            return [];
        }
    }

    public async Task<Atlassian.Jira.Project?> GetProjectAsync(string key)
    {
        var project = (await GetProjectsAsync()).FirstOrDefault(item => item.Key == key);
        if (project != null)
        {
            return project;
        }

        try
        {
            return await jiraClient.Projects.GetProjectAsync(key);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Issue> CreateIssueAsync(JiraIssue issue)
    {
        // check jira project exists
        var jiraProject = await jiraClient.Projects.GetProjectAsync(issue.ProjectKey);
        var jql = $"project = {issue.ProjectKey} AND summary ~ \"{Escape(issue.Title)}\"";
        var result = await jiraClient.Issues.GetIssuesFromJqlAsync(jql, 1);
        if (result.TotalItems > 0)
        {
            return result.First();
        }

        var jiraIssue = jiraClient.CreateIssue(issue.ProjectKey);
        jiraIssue.Type = issue.Type;
        jiraIssue.Summary = issue.Title;
        jiraIssue.Description = issue.Description;
        jiraIssue.Assignee = jiraProject.LeadUser.AccountId;
        var issueKey = await jiraClient.Issues.CreateIssueAsync(jiraIssue);
        var remoteIssue = await jiraClient.Issues.GetIssueAsync(issueKey);
        return remoteIssue;
    }

    private string Escape(string input)
    {
        var output = input.Replace(@"\", @"\\");
        output = output.Replace("[", @"\\[");
        output = output.Replace("]", @"\\]");
        output = output.Replace("(", @"\\(");
        output = output.Replace(")", @"\\)");
        output = output.Replace("*", @"\\*");
        return output;
    }
}