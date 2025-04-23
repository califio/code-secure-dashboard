using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Package;

public record DeleteTicketPackageRequest
{
    public Guid ProjectId { get; set; }
    public Guid PackageId { get; set; }
}

public interface IDeleteTicketPackageHandler : IHandler<DeleteTicketPackageRequest, bool>;

public class DeleteTicketPackageHandler(AppDbContext context) : IDeleteTicketPackageHandler
{
    public async Task<Result<bool>> HandleAsync(DeleteTicketPackageRequest request)
    {
        var projectPackage = await context.ProjectPackages
            .FirstOrDefaultAsync(record =>
                record.ProjectId == request.ProjectId && record.PackageId == request.PackageId);
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