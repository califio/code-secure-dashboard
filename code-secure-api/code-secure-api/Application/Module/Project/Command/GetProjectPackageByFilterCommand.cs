using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Core.EntityFramework;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Command;

public class GetProjectPackageByFilterCommand(AppDbContext context)
{
    public async Task<Result<Page<ProjectPackage>>> ExecuteAsync(Guid projectId, ProjectPackageFilter filter)
    {
        var project = await context.Projects.FirstOrDefaultAsync(project => project.Id == projectId);
        if (project == null)
        {
            return Result.Fail("Project not found");
        }

        var query = context.ProjectPackages
            .Include(record => record.Package)
            .Where(record => record.ProjectId == project.Id);
        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(record => record.Package!.PkgId.Contains(filter.Name));
        }

        // branch
        if (filter.CommitId != null)
        {
            query = query.Where(record => context.ScanProjectPackages.Any(packageOfBranch =>
                packageOfBranch.ProjectPackageId == record.Id && packageOfBranch.Scan!.CommitId == filter.CommitId));
        }

        // severity
        if (filter.Severity is { Count: > 0 })
        {
            query = query.Where(record => filter.Severity.Contains(record.Package!.RiskLevel));
        }

        // status
        query = query.Where(record => context.ScanProjectPackages.Any(packageOfBranch =>
            packageOfBranch.ProjectPackageId == record.Id &&
            packageOfBranch.Status == filter.Status &&
            (filter.CommitId == null || packageOfBranch.Scan!.CommitId == filter.CommitId))
        );

        return await query.Distinct().Select(record => new ProjectPackage
        {
            Location = record.Location,
            Group = record.Package!.Group,
            Name = record.Package.Name,
            Version = record.Package.Version,
            Type = record.Package.Type,
            PackageId = record.PackageId,
            FixedVersion = record.Package.FixedVersion,
            RiskImpact = record.Package.RiskImpact,
            RiskLevel = record.Package.RiskLevel,
        }).OrderBy(filter.SortBy.ToString(), filter.Desc).PageAsync(filter.Page, filter.Size);
    }
}