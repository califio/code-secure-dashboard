namespace CodeSecure.Application.Module.Integration.Jira.Client;

public record JiraConnection
{
    public required string Url { get; set; }
    public required string Password { get; set; }
    public required string Username { get; set; }
}