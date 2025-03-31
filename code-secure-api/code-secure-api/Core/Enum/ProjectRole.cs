using System.Text.Json.Serialization;

namespace CodeSecure.Core.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProjectRole
{
    Developer,
    Validator,
    Manager
}