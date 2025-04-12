namespace CodeSecure.Application.Module.Integration;

public record IntegrationStatus
{
    public required bool Mail { get; set; }
    public required bool Jira { get; set; }
    public required bool JiraWebhook { get; set; }
    public required bool Teams { get; set; }
}