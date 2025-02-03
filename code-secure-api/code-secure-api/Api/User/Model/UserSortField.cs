using System.Text.Json.Serialization;

namespace CodeSecure.Api.User.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserSortField
{
    Status,
    CreatedAt,
    UpdatedAt
}