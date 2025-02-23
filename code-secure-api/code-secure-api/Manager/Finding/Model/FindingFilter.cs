using CodeSecure.Database.Extension;
using CodeSecure.Enum;

namespace CodeSecure.Manager.Finding.Model;

public record FindingFilter : QueryFilter
{
    public Guid? ProjectId { get; set; }
    public string? RuleId { get; set; }
    public Guid? CommitId { get; set; }
    public string? Name { get; set; }
    public List<FindingSeverity>? Severity { get; set; }
    public List<FindingStatus>? Status { get; set; }
    public List<Guid>? Scanner { get; set; }
    public Guid? ProjectManagerId { get; set; }
    public FindingSortField SortBy { get; set; } = FindingSortField.CreatedAt;
}