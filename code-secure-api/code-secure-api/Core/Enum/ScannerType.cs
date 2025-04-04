using System.Text.Json.Serialization;

namespace CodeSecure.Core.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ScannerType
{
    Sast,
    Dast,
    Iast,
    Dependency,
    Container,
    Secret
}