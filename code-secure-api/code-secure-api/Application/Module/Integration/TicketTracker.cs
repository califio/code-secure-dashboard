using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Integration;

public record TicketTracker
{
    public required bool Active { get; set; }
    public required TicketType Type { get; set; }
}