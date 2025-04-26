using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Utils;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding.Command;

public class GetFindingByIdCommand(AppDbContext context)
{
    public async Task<Result<FindingDetail>> ExecuteAsync(Guid findingId)
    {
        var finding = await context.Findings.FirstOrDefaultAsync(finding => finding.Id == findingId);
        if (finding == null)
        {
            return Result.Fail("Finding not found");
        }

        var project = await context.Projects
            .Include(record => record.SourceControl)
            .FirstAsync(project => project.Id == finding.ProjectId);
        var scanner = await context.Scanners.FirstAsync(scanner => scanner.Id == finding.ScannerId);
        var scans = await context.ScanFindings
            .Include(record => record.Scan)
            .ThenInclude(scan => scan!.Commit)
            .Where(record => record.FindingId == findingId)
            .Select(record => new FindingScan
            {
                Branch = record.Scan!.Commit!.Branch,
                CommitHash = record.CommitHash,
                IsDefault = record.Scan.Commit.IsDefault,
                ScanId = record.ScanId,
                Action = record.Scan.Commit.Type,
                TargetBranch = record.Scan.Commit.TargetBranch,
                Status = record.Status
            }).ToListAsync();
        Tickets? ticket = null;
        if (finding.TicketId != null)
        {
            ticket = await context.Tickets.FirstOrDefaultAsync(record => record.Id == finding.TicketId);
        }

        var metadata = JSONSerializer.Deserialize<FindingMetadata>(finding.Metadata);
        return new FindingDetail
        {
            Id = finding.Id,
            Identity = finding.Identity,
            Name = finding.Name,
            Description = finding.Description,
            Recommendation = finding.Recommendation,
            Status = finding.Status,
            Severity = finding.Severity,
            Project = new FindingProject
            {
                Id = project.Id,
                Name = project.Name,
                SourceType = project.SourceControl!.Type,
                RepoUrl = project.RepoUrl,
                RepoId = project.RepoId
            },
            Scanner = scanner!.Name,
            Type = scanner.Type,
            Location = new FindingLocation
            {
                Path = finding.Location ?? string.Empty,
                Snippet = finding.Snippet,
                StartLine = finding.StartLine,
                EndLine = finding.EndLine,
                StartColumn = finding.StartColumn,
                EndColumn = finding.EndColumn
            },
            Scans = scans,
            Metadata = metadata,
            FixDeadline = finding.FixDeadline,
            Ticket = ticket,
            RuleId = finding.RuleId
        };
    }
}