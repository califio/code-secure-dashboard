using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding.Command;


public class DeleteFindingTicketCommand(AppDbContext context) 
{
    public async Task<Result<bool>> ExecuteAsync(Guid findingId)
    {
        var finding = await context.Findings.FirstOrDefaultAsync(finding => finding.Id == findingId);
        if (finding == null)
        {
            return Result.Fail("Finding not found");
        }
        if (finding.TicketId != null)
        {
            var ticketId = finding.TicketId;
            await context.Findings.Where(record => record.Id == findingId)
                .ExecuteUpdateAsync(setter => setter.SetProperty(record => record.TicketId, (Guid?)null));
            await context.Tickets.Where(record => record.Id == ticketId).ExecuteDeleteAsync();
        }
        return true;
    }
}