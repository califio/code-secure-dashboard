using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Package;

public record ProjectPackageDetailRequest
{
    public required Guid ProjectId { get; init; }
    public required Guid PackageId { get; init; }
}

public record ProjectPackageDetailResponse
{
    public required Packages Info { get; set; }
    public required string Location { get; set; }
    public required PackageStatus? Status { get; set; }
    public required string? IgnoreReason { get; set; }
    public required List<Vulnerabilities> Vulnerabilities { get; set; }
    public required List<Packages> Dependencies { get; set; }
    public required List<BranchStatusPackage> BranchStatus { get; set; }
    public required Tickets? Ticket { get; set; }
}

public interface IFindProjectPackageDetailHandler : IHandler<ProjectPackageDetailRequest, ProjectPackageDetailResponse>;

public class FindProjectPackageDetailHandler(AppDbContext context) : IFindProjectPackageDetailHandler
{
    public async Task<Result<ProjectPackageDetailResponse>> HandleAsync(ProjectPackageDetailRequest request)
    {
        var projectPackage = await context.ProjectPackages
            .Include(record => record.Package)
            .Include(record => record.Ticket)
            .Where(record => record.ProjectId == request.ProjectId && record.PackageId == request.PackageId)
            .FirstOrDefaultAsync();
        if (projectPackage == null)
        {
            return Result.Fail("Package not found");
        }

        var dependencies = context.PackageDependencies
            .Include(record => record.Dependency!)
            .Where(record => record.PackageId == request.PackageId)
            .Select(record => record.Dependency!)
            .OrderByDescending(record => record.RiskLevel)
            .ToList();
        var vulnerabilities = context.PackageVulnerabilities
            .Include(record => record.Vulnerability)
            .Where(record => record.PackageId == request.PackageId)
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
            Ticket = projectPackage.Ticket
        };
    }
}