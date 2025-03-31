namespace CodeSecure.Application.Module.Project.Integration.Teams;

public record TeamsProjectSetting : ProjectAlertEvent
{
    public string Webhook { get; set; } = string.Empty;
}