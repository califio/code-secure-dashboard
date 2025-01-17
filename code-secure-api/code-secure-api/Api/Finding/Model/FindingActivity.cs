using CodeSecure.Database.Metadata;
using CodeSecure.Enum;

namespace CodeSecure.Api.Finding.Model;

public record FindingActivity
{
    public required Guid? UserId { get; set; }
    public required string Username { get; set; }
    public required string Fullname { get; set; }
    public required string? Avatar { get; set; }
    public required string? Comment { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required FindingActivityType Type { get; set; }
    public required FindingActivityMetadata? Metadata { get; set; }
    public required string? MetadataString { get; set; }
}