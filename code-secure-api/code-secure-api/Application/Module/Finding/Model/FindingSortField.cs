using System.Text.Json.Serialization;

namespace CodeSecure.Application.Module.Finding.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FindingSortField
{
    Severity,
    Status,
    CreatedAt,
    UpdatedAt,
    Name
}