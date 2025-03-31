using CodeSecure.Application.Module.Integration.Jira;
using CodeSecure.Application.Module.Scanner;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Extension;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Integration;

public interface ITicketTrackerManager
{
    public Task CreateTicketAsync(Findings finding);
    public Task CreateTicketAsync(SastTicket request);
    public Task CreateTicketAsync(ScaTicket request);
}

public class TicketTrackerManager : ITicketTrackerManager
{
    private readonly AppDbContext context;
    public readonly List<ITicketTracker> IssueTrackers = new();

    public TicketTrackerManager(
        AppDbContext context,
        JiraTicketTracker jiraTicketTracker,
        JiraSetting jiraSetting)
    {
        this.context = context;
        if (jiraSetting.Active)
        {
            IssueTrackers.Add(jiraTicketTracker);
        }
    }

    public async Task CreateTicketAsync(SastTicket request)
    {
        foreach (var tracker in IssueTrackers)
        {
            await tracker.CreateTicketAsync(request);
        }
    }

    public async Task CreateTicketAsync(Findings finding)
    {
        var project = context.Projects.First(record => record.Id == finding.ProjectId);
        var scanner = (await context.FindScannerByIdAsync(finding.ScannerId)).GetResult();
        var scanFinding = await context.ScanFindings
            .Include(record => record.Scan!)
            .OrderByDescending(record => record.Scan!.CompletedAt)
            .Where(record => record.FindingId == finding.Id)
            .FirstAsync();
        await CreateTicketAsync(new SastTicket
        {
            Commit = scanFinding.CommitHash,
            Project = project,
            Finding = finding,
            Scanner = scanner
        });
    }

    public async Task CreateTicketAsync(ScaTicket request)
    {
        foreach (var tracker in IssueTrackers)
        {
            await tracker.CreateTicketAsync(request);
        }
    }
}