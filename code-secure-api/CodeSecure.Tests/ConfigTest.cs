using CodeSecure.Config;

namespace CodeSecure.Tests;

public class ConfigTest
{
    [Option(Env = "JIRA_URL", Default = "")]
    public string JiraUrl { get; set; } = string.Empty;

    [Option(Env = "JIRA_USERNAME", Default = "")]
    public string JiraUserName { get; set; } = string.Empty;
    
    [Option(Env = "JIRA_PASSWORD", Default = "")]
    public string JiraPassword { get; set; } = string.Empty;

    public static ConfigTest GetConfig()
    {
        return ConfigParser.Parse<ConfigTest>("codesecure-config.test.json");
        
    }
}