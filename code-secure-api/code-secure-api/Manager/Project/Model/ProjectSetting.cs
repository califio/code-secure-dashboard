namespace CodeSecure.Manager.Project.Model;

public class ProjectSetting
{
    public required ThresholdSetting SastSetting { get; set; } = new();
    public required ThresholdSetting ScaSetting { get; set; } = new();
    public required JiraProjectSetting JiraSetting { get; set; } = new();
    
}