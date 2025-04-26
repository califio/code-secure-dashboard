using System.Text.Json.Serialization;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Project.Model;

public record CreateProjectPackageTicketRequest
{
    public TicketType TicketType { get; set; }
    [JsonIgnore] public Guid ProjectId { get; set; }
    [JsonIgnore] public Guid PackageId { get; set; }
}