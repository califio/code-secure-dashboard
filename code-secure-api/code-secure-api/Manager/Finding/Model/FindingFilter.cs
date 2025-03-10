using CodeSecure.Database.Extension;
using CodeSecure.Enum;

namespace CodeSecure.Manager.Finding.Model;

public record FindingFilter : QueryFilter
{
    public Guid? SourceControlId { get; set; }
    public Guid? ProjectId { get; set; }
    public string? RuleId { get; set; }
    public string? Category { get; set; }
    public Guid? CommitId { get; set; }
    public string? Name { get; set; }
    public List<FindingSeverity>? Severity { get; set; }
    public List<FindingStatus>? Status { get; set; }
    public List<Guid>? Scanner { get; set; }
    public Guid? ProjectManagerId { get; set; }
    public DateTime? StartCreatedAt { get; set; }
    public DateTime? EndCreatedAt { get; set; }
    public DateTime? StartFixedAt { get; set; }
    public DateTime? EndFixedAt { get; set; }
    public FindingSortField SortBy { get; set; } = FindingSortField.CreatedAt;
}