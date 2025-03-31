using System.Text.Json.Serialization;
using CodeSecure.Core.EntityFramework;

namespace CodeSecure.Application.Module.Project.Model;

public sealed record ProjectFilter : QueryFilter
{
    public string? Name { get; set; }
    public Guid? MemberUserId { get; set; }
    public Guid? SourceControlId { get; set; }
    public ProjectSortField SortBy { get; set; } = ProjectSortField.CreatedAt;
    [JsonIgnore]
    public bool CanReadAllProject { get; set; }
    [JsonIgnore]
    public Guid CurrentUserId { get; set; }
}