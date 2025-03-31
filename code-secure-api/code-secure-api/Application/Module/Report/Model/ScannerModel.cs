using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Report.Model;

public record ScannerModel
{
    public required string Name { get; set; }
    public required ScannerType Type { get; set; }
}