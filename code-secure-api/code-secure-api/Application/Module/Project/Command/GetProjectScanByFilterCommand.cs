using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Command;

public class GetProjectScanByFilterCommand(AppDbContext context)
{
    public async Task<Result<Page<ProjectScan>>> ExecuteAsync(Guid projectId, ProjectScanFilter filter)
    {
        return await context.Scans
            .Include(scan => scan.Scanner)
            .Include(scan => scan.Commit)
            .Where(scan => scan.ProjectId == projectId)
            .Where(scan => filter.Status == null || filter.Status == scan.Status)
            .Where(scan => filter.Type == null || scan.Scanner!.Type == filter.Type)
            .Where(scan => string.IsNullOrEmpty(filter.Scanner) || scan.Scanner!.Name.Contains(filter.Scanner))
            .OrderBy(nameof(Scans.CompletedAt), true)
            .Select(scan => new ProjectScan
            {
                Id = scan.Id,
                CommitType = scan.Commit!.Type,
                Metadata = scan.Metadata,
                ScannerId = scan.Scanner!.Id,
                Scanner = scan.Scanner!.Name,
                Type = scan.Scanner!.Type,
                Status = scan.Status,
                StartedAt = scan.StartedAt,
                CompletedAt = scan.CompletedAt,
                SeverityCritical = context.ScanFindings.Include(e => e.Finding)
                    .Count(e =>
                        e.ScanId == scan.Id &&
                        e.Finding!.Severity == FindingSeverity.Critical &&
                        e.Status != FindingStatus.Incorrect &&
                        e.Status != FindingStatus.Fixed),
                SeverityHigh = context.ScanFindings.Include(e => e.Finding)
                    .Count(e =>
                        e.ScanId == scan.Id &&
                        e.Finding!.Severity == FindingSeverity.High &&
                        e.Status != FindingStatus.Incorrect &&
                        e.Status != FindingStatus.Fixed),
                SeverityMedium = context.ScanFindings.Include(e => e.Finding)
                    .Count(e =>
                        e.ScanId == scan.Id &&
                        e.Finding!.Severity == FindingSeverity.Medium &&
                        e.Status != FindingStatus.Incorrect &&
                        e.Status != FindingStatus.Fixed),
                SeverityLow = context.ScanFindings.Include(e => e.Finding)
                    .Count(e =>
                        e.ScanId == scan.Id &&
                        e.Finding!.Severity == FindingSeverity.Low &&
                        e.Status != FindingStatus.Incorrect &&
                        e.Status != FindingStatus.Fixed),
                SeverityInfo = context.ScanFindings.Include(e => e.Finding)
                    .Count(e =>
                        e.ScanId == scan.Id &&
                        e.Finding!.Severity == FindingSeverity.Info &&
                        e.Status != FindingStatus.Incorrect &&
                        e.Status != FindingStatus.Fixed),
                CommitBranch = scan.Commit.Branch,
                TargetBranch = scan.Commit.TargetBranch,
                CommitTitle = scan.Name,
                JobUrl = scan.JobUrl,
                CommitId = scan.CommitId
            }).PageAsync(filter.Page, filter.Size);
    }
}