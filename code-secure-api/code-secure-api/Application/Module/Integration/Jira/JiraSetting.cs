namespace CodeSecure.Application.Module.Integration.Jira;

public record JiraSetting
{
    public bool Active { get; set; }
    public string WebUrl { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ProjectKey { get; set; } = string.Empty;
    public string IssueType { get; set; } = string.Empty;
}