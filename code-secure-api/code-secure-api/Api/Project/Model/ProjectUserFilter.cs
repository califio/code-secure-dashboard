using CodeSecure.Database.Extension;
using CodeSecure.Enum;

namespace CodeSecure.Api.Project.Model;

public record ProjectUserFilter : QueryFilter
{
    public string? Name { get; set; }
    public ProjectRole? Role { get; set; }
}