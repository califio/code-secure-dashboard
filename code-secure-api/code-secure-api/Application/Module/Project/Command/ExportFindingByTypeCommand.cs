using CodeSecure.Application.Module.Finding;
using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Application.Module.Report;
using CodeSecure.Application.Module.Report.Model;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Command;

public class ExportFindingByTypeCommand(AppDbContext context, JwtUserClaims currentUser)
{
    public async Task<Result<byte[]>> ExecuteAsync(ExportType exportType, FindingFilter filter)
    {
        if (filter.CommitId == null)
        {
            return Result.Fail("CommitId is required");
        }

        if (filter.ProjectId == null)
        {
            return Result.Fail("ProjectId is required");
        }

        var project = await context.Projects
            .Include(record => record.SourceControl)
            .FirstOrDefaultAsync(project => project.Id == filter.ProjectId);
        if (project == null)
        {
            return Result.Fail("Project not found");
        }

        var result = await context.Findings
            .Include(finding => finding.Scanner)
            .Include(finding => finding.Ticket)
            .FindingFilter(context, currentUser, filter)
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
            commit.Id == filter.CommitId && commit.ProjectId == filter.ProjectId);
        var scans = context.Scans
            .Include(scan => scan.Scanner)
            .Where(scan => scan.ProjectId == filter.ProjectId && scan.CommitId == commit.Id).ToList();
        var scanners = scans.Select(scan => scan.Scanner!)
            .DistinctBy(scanner => scanner.Id)
            .Where(scanner => filter.Scanner is { Count: > 0 } == false ||
                              filter.Scanner.Contains(scanner.Id))
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

        if (exportType == ExportType.Pdf)
        {
            return ReportManager.ExportPdf(model);
        }

        if (exportType == ExportType.Excel)
        {
            return ReportManager.ExportExcel(model);
        }

        if (exportType == ExportType.JSON)
        {
            return ReportManager.ExportJson(model);
        }

        return Result.Fail($"Not support export type {exportType.ToString()}");
    }
}