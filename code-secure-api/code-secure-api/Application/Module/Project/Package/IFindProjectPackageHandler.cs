using System.Text.Json.Serialization;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Package;

public record ProjectPackageFilter : QueryFilter
{
    public string? Name { get; set; }
    public Guid? CommitId { get; set; }
    public PackageStatus? Status { get; set; } = PackageStatus.Open;
    public List<RiskLevel>? Severity { get; set; }
    public ProjectPackageSortField SortBy { get; set; } = ProjectPackageSortField.RiskLevel;
    [JsonIgnore] public Guid ProjectId { get; set; }
}

public interface IFindProjectPackageHandler : IHandler<ProjectPackageFilter, Page<ProjectPackage>>;

public class FindProjectPackageHandler(AppDbContext context) : IFindProjectPackageHandler
{
    public async Task<Result<Page<ProjectPackage>>> HandleAsync(ProjectPackageFilter request)
    {
        var project = await context.Projects.FirstOrDefaultAsync(project => project.Id == request.ProjectId);
        if (project == null)
        {
            return Result.Fail("Project not found");
        }

        var query = context.ProjectPackages
            .Include(record => record.Package)
            .Where(record => record.ProjectId == project.Id);
        if (!string.IsNullOrEmpty(request.Name))
        {
            query = query.Where(record => record.Package!.PkgId.Contains(request.Name));
        }

        // branch
        if (request.CommitId != null)
        {
            query = query.Where(record => context.ScanProjectPackages.Any(packageOfBranch =>
                packageOfBranch.ProjectPackageId == record.Id && packageOfBranch.Scan!.CommitId == request.CommitId));
        }

        // severity
        if (request.Severity is { Count: > 0 })
        {
            query = query.Where(record => request.Severity.Contains(record.Package!.RiskLevel));
        }

        // status
        query = query.Where(record => context.ScanProjectPackages.Any(packageOfBranch =>
            packageOfBranch.ProjectPackageId == record.Id &&
            packageOfBranch.Status == request.Status &&
            (request.CommitId == null || packageOfBranch.Scan!.CommitId == request.CommitId))
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
        }).OrderBy(request.SortBy.ToString(), request.Desc).PageAsync(request.Page, request.Size);
    }
}