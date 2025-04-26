using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Authentication;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Command;


public class GetProjectByFilterCommand(AppDbContext context, JwtUserClaims currentUser) 
{
    public async Task<Result<Page<ProjectSummary>>> ExecuteAsync(ProjectFilter filter)
    {
        var canReadAllProject = currentUser.HasClaim(PermissionType.Project, PermissionAction.Read);
        var query = context.Projects
            .Include(record => record.SourceControl)
            .Where(project =>
                canReadAllProject ||
                context.ProjectUsers.Any(projectUser =>
                    projectUser.ProjectId == project.Id &&
                    projectUser.UserId == currentUser.Id
                )
            )
            .Distinct();
        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(p => p.Name.Contains(filter.Name));
        }

        if (filter.SourceControlId != null)
        {
            query = query.Where(project => project.SourceControlId == filter.SourceControlId);
        }

        if (filter.MemberUserId != null)
        {
            query = query.Where(project => context.ProjectUsers.Any(
                record => record.ProjectId == project.Id && record.UserId == filter.MemberUserId)
            );
        }

        return await query.OrderBy(filter.SortBy.ToString(), filter.Desc).Select(p => new ProjectSummary
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
        }).PageAsync(filter.Page, filter.Size);
    }
}