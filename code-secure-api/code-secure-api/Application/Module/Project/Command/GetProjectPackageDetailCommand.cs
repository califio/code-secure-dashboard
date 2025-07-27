using CodeSecure.Application.Module.Project.Model;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Command;

public class GetProjectPackageDetailCommand(AppDbContext context)
{
    public async Task<Result<ProjectPackageDetailResponse>> ExecuteAsync(Guid projectId, Guid packageId)
    {
        var projectPackage = await context.ProjectPackages
            .Include(record => record.Package)
            .Include(record => record.Ticket)
            .Include(record => record.Project)
            .Where(record => record.ProjectId == projectId && record.PackageId == packageId)
            .FirstOrDefaultAsync();
        if (projectPackage == null)
        {
            return Result.Fail("Package not found");
        }

        var dependencies = context.PackageDependencies
            .Include(record => record.Dependency!)
            .Where(record => record.PackageId == packageId)
            .Select(record => record.Dependency!)
            .OrderByDescending(record => record.RiskLevel)
            .ToList();
        var vulnerabilities = context.PackageVulnerabilities
            .Include(record => record.Vulnerability)
            .Where(record => record.PackageId == packageId)
            .Select(record => record.Vulnerability!)
            .OrderByDescending(record => record.Severity)
            .ToList();
        var branchStatus = await context.ScanProjectPackages
            .Include(record => record.Scan)
            .ThenInclude(scan => scan!.Commit)
            .Where(record => record.ProjectPackageId == projectPackage.Id)
            .Distinct()
            .Select(record => new BranchStatusPackage
            {
                CommitHash = record.Scan!.Commit!.CommitHash,
                CommitTitle = record.Scan!.Commit!.CommitTitle,
                CommitType = record.Scan!.Commit!.Type,
                CommitBranch = record.Scan!.Commit!.Branch,
                TargetBranch = record.Scan!.Commit!.TargetBranch,
                MergeRequestId = record.Scan!.Commit!.MergeRequestId,
                Status = record.Status
            })
            .ToListAsync();

        return new ProjectPackageDetailResponse
        {
            Info = projectPackage.Package!,
            Vulnerabilities = vulnerabilities,
            Dependencies = dependencies,
            Location = projectPackage.Location,
            BranchStatus = branchStatus,
            Status = projectPackage.Status,
            IgnoreReason = projectPackage.IgnoredReason,
            Ticket = projectPackage.Ticket,
            ProjectId = projectPackage.ProjectId,
            ProjectName = projectPackage.Project!.Name
        };
    }
}