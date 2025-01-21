using Atlassian.Jira;
using Atlassian.Jira.Remote;

namespace CodeSecure.Tests;

public class JiraTest
{
    public required ConfigTest Config;
    public required Jira Jira;

    [SetUp]
    public void Setup()
    {
        Config = ConfigTest.GetConfig();
        Jira = Jira.CreateRestClient(Config.JiraUrl, Config.JiraUserName, Config.JiraPassword);
    }

    [Test]
    public void TestConnectJira()
    {
        try
        {
            var project = this.Jira.Projects.GetProjectAsync("ATTT").Result;
            Console.WriteLine(project.Url);
            Console.WriteLine(project.Key);
            Console.WriteLine(project.Lead);
        }
        catch (ResourceNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }
        
        
    }
}