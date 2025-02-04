using CodeSecure.Database.Extension;
using CodeSecure.Enum;

namespace CodeSecure.Api.Project.Model;

public record ProjectFindingFilter : QueryFilter
{
    public Guid? CommitId { get; set; }
    public string? Name { get; set; }
    public FindingSeverity? Severity { get; set; }
    public List<FindingStatus>? Status { get; set; }
    public List<Guid>? Scanner { get; set; }
    public ProjectFindingSortField SortBy { get; set; } = ProjectFindingSortField.CreatedAt;
}