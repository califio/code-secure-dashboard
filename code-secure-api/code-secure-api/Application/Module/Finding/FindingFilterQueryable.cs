using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Finding;

public static class FindingFilterQueryable
{
    public static IQueryable<Findings> FindingFilter(this IQueryable<Findings> query, AppDbContext context,
        FindingFilter filter)
    {
        query = query.Where(finding => 
                filter.CanReadAllFinding || 
                context.ProjectUsers.Any(projectUser => 
                    projectUser.ProjectId == finding.ProjectId && 
                    projectUser.UserId == filter.CurrentUserId
                )
            )
            .Where(finding => filter.SourceControlId == null || context.Projects.Any(record =>
                record.Id == finding.ProjectId
                && record.SourceControlId == filter.SourceControlId)
            )
            .Where(finding => filter.ProjectId == null || finding.ProjectId == filter.ProjectId)
            .Where(finding => filter.StartCreatedAt == null || finding.CreatedAt >= filter.StartCreatedAt)
            .Where(finding => filter.EndCreatedAt == null || finding.CreatedAt <= filter.EndCreatedAt)
            .Where(finding => filter.StartFixedAt == null || finding.FixedAt >= filter.StartFixedAt)
            .Where(finding => filter.EndFixedAt == null || finding.FixedAt <= filter.EndFixedAt)
            .Where(finding => filter.CommitId == null || context.ScanFindings.Any(record =>
                record.FindingId == finding.Id &&
                record.Scan!.CommitId == filter.CommitId
            ))
            .Where(finding => filter.Scanner == null || filter.Scanner.Count == 0 ||
                              filter.Scanner.Contains(finding.ScannerId))
            .Where(finding => filter.Severity == null || filter.Severity.Count == 0 ||
                              filter.Severity.Contains(finding.Severity))
            .Where(finding => string.IsNullOrEmpty(filter.Category) || finding.Category == filter.Category)
            .Where(finding => string.IsNullOrEmpty(filter.RuleId) || finding.RuleId == filter.RuleId)
            .Where(finding => string.IsNullOrEmpty(filter.Name) || finding.Name.Contains(filter.Name))
            .Where(finding => filter.ProjectManagerId == null || context.ProjectUsers.Any(record =>
                record.Role == ProjectRole.Manager
                && finding.ProjectId == record.ProjectId
                && record.UserId == filter.ProjectManagerId)
            );
        if (filter.Status is { Count: > 0 })
        {
            if (filter.CommitId != null)
            {
                query = query.Where(finding =>
                    context.ScanFindings.Any(record =>
                        record.FindingId == finding.Id &&
                        filter.Status.Contains(record.Status)
                    )
                );
            }
            else
            {
                query = query.Where(finding => filter.Status.Contains(finding.Status));
            }
        }

        return query;
    }
}