using System.Text.Json.Serialization;

namespace CodeSecure.Api.Project.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProjectSortField
{
    Name,
    CreatedAt,
    UpdatedAt
}