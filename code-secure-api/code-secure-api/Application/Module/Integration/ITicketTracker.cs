using CodeSecure.Core.Entity;
using FluentResults;

namespace CodeSecure.Application.Module.Integration;

public record ScaTicket
{
    public required string Location { get; set; }
    public required Projects Project { get; set; }
    public required Packages Package { get; set; }
    public required List<Vulnerabilities> Vulnerabilities { get; set; }
}

public record SastTicket
{
    public required string Commit { get; set; }
    public required Projects Project { get; set; }
    public required Findings Finding { get; set; }
    public required Scanners Scanner { get; set; }
}

public interface ITicketTracker
{
    public Task<Result<Tickets>> CreateTicketAsync(SastTicket request);

    public Task<Result<Tickets>> CreateTicketAsync(ScaTicket request);
}