using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Project.Model;

public class ThresholdSetting
{
    public ThresholdMode Mode { get; set; }
    public int Critical { get; set; }

    public int High { get; set; }

    public int Medium { get; set; }

    public int Low { get; set; }
}
