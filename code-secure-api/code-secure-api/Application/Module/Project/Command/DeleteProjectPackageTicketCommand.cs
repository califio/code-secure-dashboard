using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Command;

public class DeleteProjectPackageTicketCommand(AppDbContext context)
{
    public async Task<Result<bool>> ExecuteAsync(Guid projectId, Guid packageId)
    {
        var projectPackage = await context.ProjectPackages
            .FirstOrDefaultAsync(record =>
                record.ProjectId == projectId && record.PackageId == packageId);
        if (projectPackage == null)
        {
            return Result.Fail("Project package not found");
        }

        if (projectPackage.TicketId != null)
        {
            var ticketId = projectPackage.TicketId;

            await context.ProjectPackages.Where(record => record.Id == projectPackage.Id)
                .ExecuteUpdateAsync(setter => setter.SetProperty(record => record.TicketId, (Guid?)null));
            await context.Tickets.Where(record => record.Id == ticketId).ExecuteDeleteAsync();
            return true;
        }

        return false;
    }
}