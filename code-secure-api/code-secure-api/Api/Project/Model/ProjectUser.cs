using CodeSecure.Enum;

namespace CodeSecure.Api.Project.Model;

public record ProjectUser
{
    public required Guid UserId { get; set; }
    public required string UserName { get; set; }
    public required string FullName { get; set; }
    public required string? Email { get; set; }
    public required string? Avatar { get; set; }
    public required ProjectRole Role { get; set; }
    public required DateTime CreatedAt { get; set; }
}