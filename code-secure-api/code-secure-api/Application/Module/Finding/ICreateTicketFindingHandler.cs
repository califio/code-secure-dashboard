using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Application.Module.Integration;
using CodeSecure.Application.Module.Integration.Jira;
using CodeSecure.Application.Module.Integration.Redmine;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding;

public interface ICreateTicketFindingHandler : IHandler<CreateTicketFindingRequest, Tickets>;

public class CreateTicketFindingHandler(
    AppDbContext context,
    JiraTicketTracker jiraTicketTracker,
    RedmineTicketTracker redmineTicketTracker
)
    : ICreateTicketFindingHandler
{
    public async Task<Result<Tickets>> HandleAsync(CreateTicketFindingRequest request)
    {
        var finding = await context.Findings.FirstOrDefaultAsync(finding => finding.Id == request.FindingId);
        if (finding == null)
        {
            return Result.Fail("Ticket not found");
        }

        var project = context.Projects.First(record => record.Id == finding.ProjectId);
        var scanner = context.Scanners.First(record => record.Id == finding.ScannerId);
        var scanFinding = await context.ScanFindings
            .Include(record => record.Scan!)
            .OrderByDescending(record => record.Scan!.CompletedAt)
            .Where(record => record.FindingId == finding.Id)
            .FirstAsync();
        var ticket = new SastTicket
        {
            Commit = scanFinding.CommitHash,
            Project = project,
            Finding = finding,
            Scanner = scanner
        };
        if (request.TicketType == TicketType.Jira)
        {
            return await jiraTicketTracker.CreateTicketAsync(ticket);
        }

        if (request.TicketType == TicketType.Redmine)
        {
            return await redmineTicketTracker.CreateTicketAsync(ticket);
        }

        return Result.Fail("Not implement ticket type");
    }
}