using CodeSecure.Enum;

namespace CodeSecure.Database.Entity;

public class Tickets : BaseEntity
{
    public required string Name { get; set; }
    public required TicketType Type { get; set; }
    public required string Url { get; set; }
}