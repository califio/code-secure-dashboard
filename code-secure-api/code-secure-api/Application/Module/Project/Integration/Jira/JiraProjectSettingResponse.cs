using CodeSecure.Application.Module.Integration.Jira.Client;

namespace CodeSecure.Application.Module.Project.Integration.Jira;

public record JiraProjectSettingResponse: JiraProjectSetting
{
    public required List<JiraProject> JiraProjects { get; set; }
}