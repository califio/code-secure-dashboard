using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding;

public interface IDeleteTicketFindingHandler : IHandler<Guid, bool>;

public class DeleteTicketFindingHandler(AppDbContext context) : IDeleteTicketFindingHandler
{
    public async Task<Result<bool>> HandleAsync(Guid request)
    {
        //var finding = await FindByIdAsync(findingId);
        //if (!HasPermission(finding, PermissionAction.Update)) throw new AccessDeniedException();
        var finding = await context.Findings.FirstOrDefaultAsync(finding => finding.Id == request);
        if (finding == null)
        {
            return Result.Fail("Finding not found");
        }

        if (finding.TicketId != null)
        {
            var ticketId = finding.TicketId;
            await context.Findings.Where(record => record.Id == request)
                .ExecuteUpdateAsync(setter => setter.SetProperty(record => record.TicketId, (Guid?)null));
            await context.Tickets.Where(record => record.Id == ticketId).ExecuteDeleteAsync();
        }

        return true;
    }
}