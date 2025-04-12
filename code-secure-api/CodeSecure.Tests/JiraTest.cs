using System.Net;
using System.Text.RegularExpressions;
using Atlassian.Jira;
using CodeSecure.Core.Utils;

namespace CodeSecure.Tests;

public class JiraTest
{
    public required ConfigTest Config;
    public required Jira jira;

    [SetUp]
    public void Setup()
    {
        Config = ConfigTest.GetConfig();
        var setting = new JiraRestClientSettings();
        setting.Proxy = new WebProxy("http://127.0.0.1:8080");
        jira = Jira.CreateRestClient(Config.JiraUrl, Config.JiraUserName, Config.JiraPassword, setting);
    }

    [Test]
    public void ExtractJiraIssueIdTest()
    {
        string title = "feature SEC-01 SEC-02 fix";
        string pattern = "([A-Z]+-\\d+)";

        var match = Regex.Match(title, pattern);
        if (match.Success)
        {
            string issueId = match.Groups[1].Value;
            Console.WriteLine($"Found issue ID: {issueId}");
        }
        else
        {
            Console.WriteLine("No issue ID found.");
        }
    }

    [Test]
    public void TestCreateIssueJira()
    {
        var issueTypes = jira.IssueTypes.GetIssueTypesAsync().Result;
        foreach (var issueType in issueTypes)
        {
            Console.WriteLine(JSONSerializer.Serialize(issueType));
        }
    }

    [Test]
    public void TestUpdateIssueJira()
    {
        var issue = jira.Issues.GetIssueAsync("ATTT-504").Result;
        issue.Description = "Test create issue jira description. Ignore this issue\n{code}\ntest1\n{code}";
        jira.Issues.UpdateIssueAsync(issue).Wait();
        //Console.WriteLine(issue.Description);
    }

    [Test]
    public void TestGetProjectJira()
    {
        try
        {
            var project = jira.Projects.GetProjectAsync("ATTT").Result;
            Console.WriteLine(project.Key);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}