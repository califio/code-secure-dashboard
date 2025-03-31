using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Finding.Model;

public record FindingProject
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required SourceType SourceType { get; set; }
    public required string RepoUrl { get; set; }
    public required string RepoId { get; set; }
}