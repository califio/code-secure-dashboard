using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Manager.Integration.TicketTracker.Jira;
using CodeSecure.Manager.Scanner;
using CodeSecure.Manager.Setting;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Manager.Integration.TicketTracker;

public class TicketTrackerManager : ITicketTrackerManager
{
    private readonly AppDbContext context;
    private readonly IScannerManager scannerManager;
    public readonly List<ITicketTracker> IssueTrackers = new();

    public TicketTrackerManager(
        AppDbContext context,
        IScannerManager scannerManager,
        JiraTicketTracker jiraTicketTracker,
        JiraSetting jiraSetting)
    {
        this.context = context;
        this.scannerManager = scannerManager;
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
        var scanner = await scannerManager.FindByIdAsync(finding.ScannerId);
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
            Scanner = scanner!
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