using System.Text.Json.Serialization;

namespace CodeSecure.Application.Module.Project.Package;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProjectPackageSortField
{
    Name,
    RiskLevel
}