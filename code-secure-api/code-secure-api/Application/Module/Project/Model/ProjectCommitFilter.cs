using CodeSecure.Core.EntityFramework;

namespace CodeSecure.Application.Module.Project.Model;

public record ProjectCommitFilter : QueryFilter
{
    public string? Name { get; set; }
}
