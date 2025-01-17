using CodeSecure.Database.Metadata;
using CodeSecure.Enum;

namespace CodeSecure.Api.Project.Model;

public record ProjectInfo
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string RepoId { get; set; }
    public required string RepoUrl { get; set; }
    public required SourceType SourceType { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime? UpdatedAt { get; set; }
    public ProjectSettingMetadata? Setting { get; set; }
}