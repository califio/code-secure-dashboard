using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Project.Model;

public record ProjectSummary
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required SourceType SourceType { get; set; }
    public required int SeverityCritical { get; set; }
    public required int SeverityHigh { get; set; }
    public required int SeverityMedium { get; set; }
    public required int SeverityLow { get; set; }
    public required int SeverityInfo { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime? UpdatedAt { get; set; }
}