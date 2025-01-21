using CodeSecure.Enum;

namespace CodeSecure.Manager.Project.Model;

public record Member
{
    public required Guid UserId { get; set; }
    public required string UserName { get; set; }
    public string? FullName { get; set; }
    public string? Avatar { get; set; }
    public required string Email { get; set; }
    public required ProjectRole Role { get; set; }
    public required UserStatus Status { get; set; }
}