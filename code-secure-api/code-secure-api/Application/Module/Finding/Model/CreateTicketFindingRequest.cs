using System.Text.Json.Serialization;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Finding.Model;

public record CreateTicketFindingRequest
{
    [JsonIgnore] public Guid FindingId { get; set; }
    public TicketType TicketType { get; set; }
}
