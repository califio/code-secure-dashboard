using System.Text.Json.Serialization;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.User;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserSortField
{
    Status,
    CreatedAt,
    UpdatedAt
}

public sealed record UserFilter : QueryFilter
{
    public string? Name { get; set; }
    public Guid? RoleId { get; set; }
    public UserStatus? Status { get; set; }
    public UserSortField SortBy { get; set; } = UserSortField.CreatedAt;
}