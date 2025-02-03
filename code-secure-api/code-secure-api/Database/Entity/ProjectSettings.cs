using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Database.Entity;

[PrimaryKey(nameof(ProjectId))]
public class ProjectSettings
{
    public required Guid ProjectId { get; set; }
    public string? SastSetting { get; set; }
    public string? ScaSetting { get; set; }
    public string? JiraSetting { get; set; }
    public string? TeamsSetting { get; set; }
}