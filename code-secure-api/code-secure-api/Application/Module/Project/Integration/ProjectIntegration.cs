namespace CodeSecure.Application.Module.Project.Integration;

public record ProjectIntegration
{
    public required bool Mail { get; set; }
    public required bool Jira { get; set; }
    public required bool Teams { get; set; }
}