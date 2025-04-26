using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Core.Entity;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Command;

public class UpdateProjectPackageCommand(AppDbContext context)
{
    public async Task<Result<ProjectPackages>> ExecuteAsync(Guid projectId, Guid packageId,
        UpdateProjectPackageRequest request)
    {
        var projectPackage = await context.ProjectPackages
            .FirstOrDefaultAsync(x => x.ProjectId == projectId && x.PackageId == packageId);
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