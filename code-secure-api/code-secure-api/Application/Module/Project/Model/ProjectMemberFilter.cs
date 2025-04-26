using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Project.Model;

public record ProjectMemberFilter : QueryFilter
{
    public string? Name { get; set; }
    public ProjectRole? Role { get; set; }
}
