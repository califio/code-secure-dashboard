using CodeSecure.Manager.Integration.TicketTracker.Jira;

namespace CodeSecure.Api.Project.Model;

public record JiraProjectSettingResponse
{
    public required bool Active { get; set; }
    public required string ProjectKey { get; set; } 
    public required string IssueType { get; set; }
    public required List<JiraProject> JiraProjects { get; set; } = new();
}