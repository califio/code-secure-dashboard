using CodeSecure.Enum;

namespace CodeSecure.Api.Integration.Model;

public record TicketTracker
{
    public required bool Active { get; set; }
    public required TicketType Type { get; set; }
}