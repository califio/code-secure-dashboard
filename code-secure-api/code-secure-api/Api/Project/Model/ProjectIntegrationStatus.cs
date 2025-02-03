namespace CodeSecure.Api.Project.Model;

public record ProjectIntegrationStatus
{
    public bool Jira { get; set; }
    public bool Teams { get; set; }
}