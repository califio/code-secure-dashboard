namespace CodeSecure.Application.Module.Project.Setting.Threshold;

public record ThresholdProject
{
    public required ThresholdSetting Sast { get; set; } = new();
    public required ThresholdSetting Sca { get; set; } = new();
}