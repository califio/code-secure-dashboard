using CodeSecure.Database.Extension;

namespace CodeSecure.Api.Project.Model;

public record ProjectPackageFilter : QueryFilter
{
    public string? Name { get; set; }
    public ProjectPackageSortField SortBy { get; set; } = ProjectPackageSortField.RiskLevel;
}