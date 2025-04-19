using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Project.Model;

public record ProjectScanSummary
{
    public required string Scanner { get; set; }
    public required ScannerType Type { get; set; }
    public required ScanStatus Status { get; set; }
    public required DateTime Started { get; set; }
    public required DateTime? Completed { get; set; }
    public required int Open { get; set; }
    public required int Confirmed { get; set; }
    public required int Ignored { get; set; }
    public required int Fixed { get; set; }
    
    public required int Critical { get; set; }
    public required int High { get; set; }
    public required int Medium { get; set; }
    public required int Low { get; set; }
    public required string JobUrl { get; set; }
}