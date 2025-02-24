using CodeSecure.Database.Extension;

namespace CodeSecure.Api.Project.Model;

public sealed record ProjectFilter : QueryFilter
{
    public string? Name { get; set; }
    public Guid? UserId { get; set; }
    public ProjectSortField SortBy { get; set; } = ProjectSortField.CreatedAt;
}