using System.Text.Json.Serialization;

namespace CodeSecure.Application.Module.Project.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProjectSortField
{
    Name,
    CreatedAt,
    UpdatedAt
}