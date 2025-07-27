using CodeSecure.Application.Module.Package.Command;
using CodeSecure.Application.Module.Package.Model;
using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Extension;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Package;

public interface IPackageService
{
    Task<Page<ProjectPackage>> GetPackagesByFilterAsync(PackageFilter filter);
    Task<PackageDetail> GetPackageByIdAsync(Guid packageId);
    Task<List<PackageInfo>> ListPackageDependencyAsync(Guid packageId);
}

public class PackageService(AppDbContext context): IPackageService
{
    public async Task<Page<ProjectPackage>> GetPackagesByFilterAsync(PackageFilter filter)
    {
        // Note: Currently all users can access a project’s dependencies. We may update this behavior in the future.
        var query = context.ProjectPackages
            .Include(record => record.Package)
            .Include(record => record.Project)
            .AsNoTracking();
        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(record =>  (record.Package!.Group + "/" + record.Package.Name + "@" + record.Package.Version).Contains(filter.Name));
        }
        // severity
        if (filter.Severity is { Count: > 0 })
        {
            query = query.Where(record => filter.Severity.Contains(record.Package!.RiskLevel));
        }

        // status
        query = query.Where(record => context.ScanProjectPackages.Any(packageOfBranch =>
            packageOfBranch.ProjectPackageId == record.Id && packageOfBranch.Status == filter.Status)
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
            ProjectId = record.ProjectId,
            ProjectName = record.Project!.Name,
        }).OrderBy(filter.SortBy.ToString(), filter.Desc).PageAsync(filter.Page, filter.Size);
    }

    public async Task<PackageDetail> GetPackageByIdAsync(Guid packageId)
    {
        return (await new GetPackageByIdCommand(context).ExecuteAsync(packageId)).GetResult();
    }

    public async Task<List<PackageInfo>> ListPackageDependencyAsync(Guid packageId)
    {
        return (await new ListPackageDependencyCommand(context).ExecuteAsync(packageId)).GetResult();
    }
}