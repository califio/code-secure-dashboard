namespace CodeSecure.Application.Module.Project.Model;

public record ThresholdProject
{
    public required ThresholdSetting Sast { get; set; } = new();
    public required ThresholdSetting Sca { get; set; } = new();
}