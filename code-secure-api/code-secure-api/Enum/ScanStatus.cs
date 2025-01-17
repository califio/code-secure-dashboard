using System.Text.Json.Serialization;

namespace CodeSecure.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ScanStatus
{
    Queue,
    Running,
    Completed,
    Error
}