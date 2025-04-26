namespace CodeSecure.Application.Module.Auth.Model;

public record UserProfile
{
    public required Guid UserId { get; set; }
    public required string UserName { get; set; }
    public required string FullName { get; set; }
    public required string? Email { get; set; }
    public required string? Avatar { get; set; }
}