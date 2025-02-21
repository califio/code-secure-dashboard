using CodeSecure.Enum;

namespace CodeSecure.Manager.Report.Model;

public record ScannerModel
{
    public required string Name { get; set; }
    public required ScannerType Type { get; set; }
}