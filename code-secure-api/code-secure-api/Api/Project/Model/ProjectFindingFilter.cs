using CodeSecure.Database.Extension;
using CodeSecure.Enum;

namespace CodeSecure.Api.Project.Model;

public record ProjectFindingFilter : QueryFilter
{
    public Guid? CommitId { get; set; }
    public string? Name { get; set; }
    public FindingSeverity? Severity { get; set; }
    public FindingStatus? Status { get; set; }
    public string? Scanner { get; set; }
    public ScannerType? Type { get; set; }
    public ProjectFindingSortField SortBy { get; set; } = ProjectFindingSortField.CreatedAt;
}