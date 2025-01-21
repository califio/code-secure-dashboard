using System.Text.Json.Serialization;

namespace CodeSecure.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SourceType
{
    Local,
    GitLab,
    GitHub,
    Bitbucket
}