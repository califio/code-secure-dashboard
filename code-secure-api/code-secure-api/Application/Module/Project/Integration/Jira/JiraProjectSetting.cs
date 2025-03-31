using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.Project.Integration.Jira;

public record JiraProjectSetting
{
    [Required]
    public bool Active { get; set; }
    [Required]
    public string ProjectKey { get; set; } = string.Empty;
    [Required]
    public string IssueType { get; set; } = string.Empty;
}