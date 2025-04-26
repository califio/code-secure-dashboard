namespace CodeSecure.Application.Module.Project.Model;

public record UpdateProjectThresholdRequest
{
    public ThresholdSetting? Sast { get; set; }
    public ThresholdSetting? Sca { get; set; }
}