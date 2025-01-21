using System.ComponentModel.DataAnnotations;
using CodeSecure.Enum;

namespace CodeSecure.Database.Metadata;

public record SeverityThreshold
{
    [Required] public int Critical { get; set; } = 0;

    [Required] public int High { get; set; } = 0;

    [Required] public int Medium { get; set; } = 0;

    [Required] public int Low { get; set; } = 0;
}

public record SastSetting
{
    [Required] public ThresholdMode Mode { get; set; }

    [Required] public SeverityThreshold? SeverityThreshold { get; set; }
}

public record ScaSetting
{
    [Required] public ThresholdMode Mode { get; set; }

    [Required] public SeverityThreshold? SeverityThreshold { get; set; }
}

public record ProjectSettingMetadata
{
    [Required] public SastSetting? SastSetting { get; set; }

    [Required] public ScaSetting? ScaSetting { get; set; }
}