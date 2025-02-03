namespace CodeSecure.Manager.Project.Model;

public class JiraProjectSetting
{
    public bool Active { get; set; }
    public string ProjectKey { get; set; } = string.Empty;
    public string IssueType { get; set; } = string.Empty;
}