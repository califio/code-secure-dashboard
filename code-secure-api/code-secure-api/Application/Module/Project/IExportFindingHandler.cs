using System.ComponentModel.DataAnnotations;
using CodeSecure.Application.Module.Finding;
using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Application.Module.Report;
using CodeSecure.Application.Module.Report.Model;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project;

public record ExportFindingRequest
{
    [Required]
    public ExportType ExportType { get; set; }
    [Required]
    public required FindingFilter Filter { get; set; }
}

public interface IExportFindingHandler : IHandler<ExportFindingRequest, byte[]>;

public class ExportFindingHandler(AppDbContext context, IReportManager reportManager) : IExportFindingHandler
{
    public async Task<Result<byte[]>> HandleAsync(ExportFindingRequest request)
    {
        if (request.Filter.CommitId == null)
        {
            return Result.Fail("Commit is required");
        }

        var project = await context.Projects
            .Include(record => record.SourceControl)
            .FirstOrDefaultAsync(project => project.Id == request.Filter.ProjectId);
        if (project == null)
        {
            return Result.Fail("Project not found");
        }

        var result = await context.Findings
            .Include(finding => finding.Scanner)
            .Include(finding => finding.Ticket)
            .FindingFilter(context, request.Filter)
            .ToListAsync();
        var findings = result.Select(finding => new FindingModel
        {
            Id = finding.Id,
            Name = finding.Name,
            Description = finding.Description,
            Recommendation = finding.Recommendation,
            Status = finding.Status,
            Severity = finding.Severity,
            Location = finding.Location,
            Snippet = finding.Snippet,
            StartLine = finding.StartLine,
            EndLine = finding.EndLine,
            Scanner = finding.Scanner!.Name,
            Type = finding.Scanner!.Type,
            TicketUrl = finding.Ticket?.Url,
            TicketName = finding.Ticket?.Name
        }).ToList();
        var commit = await context.ProjectCommits.FirstAsync(commit =>
            commit.Id == request.Filter.CommitId && commit.ProjectId == request.Filter.ProjectId);
        var scans = context.Scans
            .Include(scan => scan.Scanner)
            .Where(scan => scan.ProjectId == request.Filter.ProjectId && scan.CommitId == commit.Id).ToList();
        var scanners = scans.Select(scan => scan.Scanner!)
            .DistinctBy(scanner => scanner.Id)
            .Where(scanner => request.Filter.Scanner is { Count: > 0 } == false ||
                              request.Filter.Scanner.Contains(scanner.Id))
            .Select(scanner => new ScannerModel
            {
                Name = scanner.Name,
                Type = scanner.Type
            }).ToList();
        //
        findings = findings.FindAll(finding => finding.Status != FindingStatus.Incorrect);
        findings.Sort((f1, f2) => f2.Severity - f1.Severity);
        var model = new ReportModel
        {
            SourceType = project.SourceControl!.Type,
            RepoName = project.Name,
            RepoUrl = project.RepoUrl,
            CommitTitle = commit.CommitTitle ?? string.Empty,
            CommitSha = commit.CommitHash ?? string.Empty,
            CommitBranch = commit.Branch,
            TargetBranch = commit.TargetBranch,
            MergeRequestId = commit.MergeRequestId,
            Time = scans.First().StartedAt,
            Scanners = scanners,
            Findings = findings
        };

        if (request.ExportType == ExportType.Pdf)
        {
            return reportManager.ExportPdf(model);
        }

        if (request.ExportType == ExportType.Excel)
        {
            return reportManager.ExportExcel(model);
        }

        if (request.ExportType == ExportType.JSON)
        {
            return reportManager.ExportJson(model);
        }

        return Result.Fail($"Not support export type {request.ExportType.ToString()}");
    }
}