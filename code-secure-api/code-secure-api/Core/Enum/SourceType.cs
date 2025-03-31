using System.Text.Json.Serialization;

namespace CodeSecure.Core.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SourceType
{
    Local,
    GitLab,
    GitHub,
    Bitbucket
}