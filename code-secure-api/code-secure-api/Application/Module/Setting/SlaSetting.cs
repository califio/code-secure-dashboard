using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.Setting;

// Service Level Agreement (SLA): day unit
public record SLA
{
    [Range(0, int.MaxValue)]
    public int Critical { get; set; }
    [Range(0, int.MaxValue)]
    public int High { get; set; }
    [Range(0, int.MaxValue)]
    public int Medium { get; set; }
    [Range(0, int.MaxValue)]
    public int Low { get; set; }
    [Range(0, int.MaxValue)]
    public int Info { get; set; }
}

public record SlaSetting
{
    public SLA Sast { get; set; } = new();
    public SLA Sca { get; set; } = new();
}