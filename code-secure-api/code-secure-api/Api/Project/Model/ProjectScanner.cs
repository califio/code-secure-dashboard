using CodeSecure.Enum;

namespace CodeSecure.Api.Project.Model;

public record ProjectScanner
{
    public required string Name { get; set; }
    public required ScannerType Type { get; set; }
}