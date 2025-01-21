using CodeSecure.Database.Extension;
using CodeSecure.Enum;

namespace CodeSecure.Api.Admin.User.Model;

public sealed record UserFilter : QueryFilter
{
    public string? Name { get; set; }
    public Guid? RoleId { get; set; }
    public UserStatus? Status { get; set; }
    public UserSortField SortBy { get; set; } = UserSortField.CreatedAt;
}