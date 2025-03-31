using System.Text.Json.Serialization;
using CodeSecure.Core.EntityFramework;

namespace CodeSecure.Application.Module.Finding.Model;

public record ActivityFindingFilter : QueryFilter
{
    [JsonIgnore] public Guid FindingId { get; set; }
}
