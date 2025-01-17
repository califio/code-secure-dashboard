using System.Text.Json.Serialization;

namespace CodeSecure.Api.Project.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProjectFindingSortField
{
    Severity,
    Status,
    CreatedAt,
    UpdatedAt,
    Name
}