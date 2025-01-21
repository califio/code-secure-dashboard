using CodeSecure.Enum;

namespace CodeSecure.Api.Admin.User.Model;

public record UserInfo : UserSummary
{
    public required UserStatus Status { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime? UpdatedAt { get; set; }
    public required string Role { get; set; }
    public required bool Verified { get; set; }
    public required DateTimeOffset? Lockout { get; set; }
    public required bool Enable2Fa { get; set; }
}