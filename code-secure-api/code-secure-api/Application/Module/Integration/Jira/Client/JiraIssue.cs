namespace CodeSecure.Application.Module.Integration.Jira.Client;

public class JiraIssue
{

    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime? DueDate { get; set; }
    public required string Type { get; set; }
    public required string ProjectKey { get; set; }
}