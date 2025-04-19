using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project;

public interface IGetProjectCommitScanSummary
{
    Task<Result<Page<ProjectCommitScanSummary>>> HandleAsync(Guid projectId, ProjectCommitFilter request);
}

public class GetProjectCommitScanSummary(AppDbContext context) : IGetProjectCommitScanSummary
{
    public async Task<Result<Page<ProjectCommitScanSummary>>> HandleAsync(Guid projectId, ProjectCommitFilter filter)
    {
        var commitPage = await context.ProjectCommits
            .Where(x => x.ProjectId == projectId)
            .Where(x => string.IsNullOrEmpty(filter.Name) || (x.CommitTitle != null && x.CommitTitle.Contains(filter.Name)))
            .PageAsync(filter.Page, filter.Size);
        var commitIds = commitPage.Items.Select(x => x.Id);
        var scans = await context.Scans
            .Include(x => x.Scanner)
            .Include(x => x.Commit)
            .Where(x => commitIds.Contains(x.CommitId))
            .OrderByDescending(x => x.StartedAt)
            .Select(x => new ScanCommitProject
            {
                ScanId = x.Id,
                Commit = x.Commit!,
                Scanner = x.Scanner!.Name,
                Type = x.Scanner.Type,
                Status = x.Status,
                Started = x.StartedAt,
                Completed = x.CompletedAt,
                Open = context.ScanFindings.Include(e => e.Finding)
                    .Count(e => e.ScanId == x.Id && e.Status == FindingStatus.Open),
                Confirmed = context.ScanFindings.Include(e => e.Finding)
                    .Count(e => e.ScanId == x.Id && e.Status == FindingStatus.Confirmed),
                Ignored = context.ScanFindings.Include(e => e.Finding)
                    .Count(e => e.ScanId == x.Id && e.Status == FindingStatus.AcceptedRisk),
                Fixed = context.ScanFindings.Include(e => e.Finding)
                    .Count(e => e.ScanId == x.Id && e.Status == FindingStatus.Fixed),
                JobUrl = x.JobUrl,
                Critical = context.ScanFindings.Include(e => e.Finding)
                    .Count(e => e.ScanId == x.Id && e.Finding!.Severity == FindingSeverity.Critical),
                High = context.ScanFindings.Include(e => e.Finding)
                    .Count(e => e.ScanId == x.Id && e.Finding!.Severity == FindingSeverity.High),
                Medium = context.ScanFindings.Include(e => e.Finding)
                    .Count(e => e.ScanId == x.Id && e.Finding!.Severity == FindingSeverity.Medium),
                Low = context.ScanFindings.Include(e => e.Finding)
                    .Count(e => e.ScanId == x.Id && e.Finding!.Severity == FindingSeverity.Low),
            })
            .ToListAsync();
        var item = scans.GroupBy(x => x.Commit).Select(x => new ProjectCommitScanSummary
        {
            CommitId = x.Key.Id,
            Branch = x.Key.Branch,
            Type = x.Key.Type,
            TargetBranch = x.Key.TargetBranch,
            IsDefault = x.Key.IsDefault,
            Title = x.Key.CommitTitle ?? string.Empty,
            Scans = x.Select(e => new ProjectScanSummary
            {
                Scanner = e.Scanner,
                Type = e.Type,
                Status = e.Status,
                Started = e.Started,
                Completed = e.Completed,
                Open = e.Open,
                Confirmed = e.Confirmed,
                Ignored = e.Ignored,
                Fixed = e.Fixed,
                JobUrl = e.JobUrl,
                Critical = e.Critical,
                High = e.High,
                Medium = e.Medium,
                Low = e.Low,
            }).ToList()
        }).ToList();
        return new Page<ProjectCommitScanSummary>
        {
            Items = item,
            Count = commitPage.Count,
            PageCount = commitPage.PageCount,
            CurrentPage = commitPage.CurrentPage,
            Size = commitPage.Size
        };
    }
}

internal record ScanCommitProject
{
    public required Guid ScanId { get; set; }
    public required GitCommits Commit { get; set; }
    public required string Scanner { get; set; }
    public required ScannerType Type { get; set; }
    public required ScanStatus Status { get; set; }
    public required DateTime Started { get; set; }
    public required DateTime? Completed { get; set; }
    // status
    public required int Open { get; set; }
    public required int Confirmed { get; set; }
    public required int Ignored { get; set; }
    public required int Fixed { get; set; }
    // severity
    public required int Critical { get; set; }
    public required int High { get; set; }
    public required int Medium { get; set; }
    public required int Low { get; set; }
    public required string JobUrl { get; set; }
}