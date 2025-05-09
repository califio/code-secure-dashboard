using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Core.Entity;

[PrimaryKey(nameof(ProjectId))]
public class ProjectSettings
{
    public required Guid ProjectId { get; set; }
    public string? SastSetting { get; set; }
    public string? ScaSetting { get; set; }
    public string? JiraSetting { get; set; }
    public string? RedmineSetting { get; set; }
    public string? TeamsSetting { get; set; }
    public string? MailSetting { get; set; }
    public string? DefaultBranch { get; set; }
}