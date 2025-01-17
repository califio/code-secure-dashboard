using CodeSecure.Enum;

namespace CodeSecure.Api.Project.Model;

public record ProjectFinding
{
    public required Guid Id { get; set; }
    public required string Identity { get; set; }
    public required string Name { get; set; }
    public required FindingStatus Status { get; set; }
    public required FindingSeverity Severity { get; set; }
    public required string Scanner { get; set; }
    public required ScannerType Type { get; set; }
}