using System.Text.Json.Serialization;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Package;

public class UpdateProjectPackageRequest
{
    public PackageStatus? Status { get; set; }
    public string? IgnoreReason { get; set; }
    [JsonIgnore] public Guid PackageId { get; set; }
    [JsonIgnore] public Guid ProjectId { get; set; }
}

public interface IUpdateProjectPackageHandler : IHandler<UpdateProjectPackageRequest, ProjectPackages>;

public class UpdateProjectPackageHandler(AppDbContext context) : IUpdateProjectPackageHandler
{
    public async Task<Result<ProjectPackages>> HandleAsync(UpdateProjectPackageRequest request)
    {
        var projectPackage = await context.ProjectPackages.FirstOrDefaultAsync(record =>
            record.ProjectId == request.ProjectId && record.PackageId == request.PackageId);
        if (projectPackage == null)
        {
            return Result.Fail("Package not found");
        }

        if (request.Status != null && projectPackage.Status != request.Status)
        {
            projectPackage.Status = request.Status;
            await context.ScanProjectPackages
                .Where(record => record.ProjectPackageId == projectPackage.Id)
                .ExecuteUpdateAsync(setter => setter.SetProperty(column => column.Status, request.Status));
        }

        if (!string.IsNullOrEmpty(request.IgnoreReason))
        {
            projectPackage.IgnoredReason = request.IgnoreReason;
        }

        context.ProjectPackages.Update(projectPackage);
        await context.SaveChangesAsync();
        return projectPackage;
    }
}