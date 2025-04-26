using CodeSecure.Application.Module.Integration;
using CodeSecure.Application.Module.Integration.Jira;
using CodeSecure.Application.Module.Integration.Redmine;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding.Command;

public class CreateFindingTicketCommand(AppDbContext context)
{
    public async Task<Result<Tickets>> ExecuteAsync(Guid findingId, TicketType ticketType)
    {
        var finding = await context.Findings.FirstOrDefaultAsync(finding => finding.Id == findingId);
        if (finding == null)
        {
            return Result.Fail("Finding not found");
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
        if (ticketType == TicketType.Jira)
        {
            var jiraTicketTracker = new JiraTicketTracker(context);
            return await jiraTicketTracker.CreateTicketAsync(ticket);
        }

        if (ticketType == TicketType.Redmine)
        {
            var redmineTicketTracker = new RedmineTicketTracker(context);
            return await redmineTicketTracker.CreateTicketAsync(ticket);
        }

        return Result.Fail("Not implement ticket type");
    }
}