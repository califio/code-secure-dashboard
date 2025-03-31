namespace CodeSecure.Application.Module.Project.Model;

public record ProjectIntegrationStatus
{
    public bool Jira { get; set; }
    public bool Teams { get; set; }
}