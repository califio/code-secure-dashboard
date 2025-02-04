namespace CodeSecure.Api.Integration.Model;

public record IntegrationSetting
{
    public required bool Mail { get; set; }
    public required bool Jira { get; set; }
    public required bool Teams { get; set; }
}