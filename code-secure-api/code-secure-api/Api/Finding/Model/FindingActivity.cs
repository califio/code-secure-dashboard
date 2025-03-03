using CodeSecure.Database.Entity;
using CodeSecure.Enum;

namespace CodeSecure.Api.Finding.Model;

public record FindingActivity
{
    public Guid? UserId { get; set; }
    public required string? Username { get; set; }
    public required string? Avatar { get; set; }
    public required FindingActivityType Type { get; set; }
    public string? Comment { get; set; }
    public string? OldState { get; set; }
    public string? NewState { get; set; }
    public GitCommits? Commit { get; set; }
    public required DateTime CreatedAt { get; set; }
}