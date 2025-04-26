using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Project.Model;

public record ProjectPackageFilter : QueryFilter
{
    public string? Name { get; set; }
    public Guid? CommitId { get; set; }
    public PackageStatus? Status { get; set; } = PackageStatus.Open;
    public List<RiskLevel>? Severity { get; set; }
    public ProjectPackageSortField SortBy { get; set; } = ProjectPackageSortField.RiskLevel;
}
