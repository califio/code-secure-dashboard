using System.Text.Json.Serialization;

namespace CodeSecure.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExportType
{
    Pdf,
    Excel,
    JSON
}