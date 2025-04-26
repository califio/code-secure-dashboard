namespace CodeSecure.Application.Module.User.Model;

public record UserSummary
{
    public required Guid Id { get; set; }
    public required string UserName { get; set; }
    public required string FullName { get; set; }
    public required string? Avatar { get; set; }
    public required string? Email { get; set; }
}