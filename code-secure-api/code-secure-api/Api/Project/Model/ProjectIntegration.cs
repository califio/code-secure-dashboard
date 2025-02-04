namespace CodeSecure.Api.Project.Model;

public record ProjectIntegration
{
    public required bool Jira { get; set; }
    public required bool Teams { get; set; }
}