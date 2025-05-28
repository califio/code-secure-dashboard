using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Atlassian.Jira;
using CodeSecure.Application.Module.Integration.JiraWebhook;
using CodeSecure.Core.Utils;
using Microsoft.IdentityModel.Tokens;

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
        string title = "feat/sec-01-fix-sqli";
        var issueId = title.JiraIssueId();
        Console.WriteLine(!issueId.IsNullOrEmpty() ? $"Found issue ID: {issueId}" : "No issue ID found.");
    }

    [Test]
    public void TestJiraWebhook()
    {
        var token = "";
        var webhook = "";
        // var message = "{panel:title=semgrep|bgColor=#FFFF00|borderStyle=solid}\nTest\n{panel}";
        // var message = "{panel:title=semgrep|bgColor=#00FF00|borderStyle=solid}\nSuccess\n{panel}";
        var message = "{panel:title=semgrep|borderStyle=solid}\nInfo\n{panel}";
        message = message.Replace("\n", "\\n");
        var body = $"{{\"issues\":[\"CAL-44\"], \"data\": {{\"message\":\"{message}\"}}}}";
        Console.WriteLine(body);
        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("X-Automation-Webhook-Token", token);
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        client.PostAsync(webhook, content).Wait();
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