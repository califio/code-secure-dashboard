using CodeSecure.Enum;

namespace CodeSecure.Api.Project.Model;

public record ProjectScanner
{
    public required Guid ScannerId { get; set; }
    public required string Name { get; set; }
    public required ScannerType Type { get; set; }
}