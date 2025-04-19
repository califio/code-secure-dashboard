using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project;

public interface IFindProjectHandler : IHandler<ProjectFilter, Page<ProjectSummary>>;

public class FindProjectHandler(AppDbContext context) : IFindProjectHandler
{
    public async Task<Result<Page<ProjectSummary>>> HandleAsync(ProjectFilter request)
    {
        var query = context.Projects
            .Include(record => record.SourceControl)
            .Where(project =>
                request.CanReadAllProject ||
                context.ProjectUsers.Any(projectUser =>
                    projectUser.ProjectId == project.Id &&
                    projectUser.UserId == request.CurrentUserId
                )
            )
            .Distinct();
        if (!string.IsNullOrEmpty(request.Name))
        {
            query = query.Where(p => p.Name.Contains(request.Name));
        }

        if (request.SourceControlId != null)
        {
            query = query.Where(project => project.SourceControlId == request.SourceControlId);
        }

        if (request.MemberUserId != null)
        {
            query = query.Where(project => context.ProjectUsers.Any(
                record => record.ProjectId == project.Id && record.UserId == request.MemberUserId)
            );
        }

        return await query.OrderBy(request.SortBy.ToString(), request.Desc).Select(p => new ProjectSummary
        {
            Id = p.Id,
            Name = p.Name,
            SeverityCritical = context.Findings.Count(finding =>
                finding.ProjectId == p.Id &&
                finding.Severity == FindingSeverity.Critical &&
                finding.Status != FindingStatus.Incorrect),
            SeverityHigh = context.Findings.Count(finding =>
                finding.ProjectId == p.Id &&
                finding.Severity == FindingSeverity.High &&
                finding.Status != FindingStatus.Incorrect),
            SeverityMedium = context.Findings.Count(finding =>
                finding.ProjectId == p.Id &&
                finding.Severity == FindingSeverity.Medium &&
                finding.Status != FindingStatus.Incorrect),
            SeverityLow = context.Findings.Count(finding =>
                finding.ProjectId == p.Id &&
                finding.Severity == FindingSeverity.Low &&
                finding.Status != FindingStatus.Incorrect),
            SeverityInfo = context.Findings.Count(finding =>
                finding.ProjectId == p.Id &&
                finding.Severity == FindingSeverity.Info &&
                finding.Status != FindingStatus.Incorrect),
            
            Open = context.Findings.Count(finding =>
                finding.ProjectId == p.Id &&
                finding.Status == FindingStatus.Open),
            Confirmed = context.Findings.Count(finding =>
                finding.ProjectId == p.Id &&
                finding.Status == FindingStatus.Confirmed),
            Ignore = context.Findings.Count(finding =>
                finding.ProjectId == p.Id &&
                finding.Status == FindingStatus.AcceptedRisk),
            Fixed = context.Findings.Count(finding =>
                finding.ProjectId == p.Id &&
                finding.Status == FindingStatus.Fixed),
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            SourceType = p.SourceControl!.Type,
        }).PageAsync(request.Page, request.Size);
    }
}