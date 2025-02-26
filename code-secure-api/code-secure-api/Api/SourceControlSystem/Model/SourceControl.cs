using CodeSecure.Enum;

namespace CodeSecure.Api.SourceControlSystem.Model;

public record SourceControl
{
    public required Guid Id { get; set; }
    public required SourceType Type { get; set; }
    public required string Name { get; set; }
}