namespace CodeSecure.Application.Module.Integration;

public record TicketTrackerStatus
{
    public required bool Jira { get; set; }
    public required bool Redmine { get; set; }
}