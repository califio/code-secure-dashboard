using CodeSecure.Application.Module.Finding.Command;
using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Extension;

namespace CodeSecure.Application.Module.Finding;

public interface IFindingService
{
    Task<FindingActivity> CreateCommentAsync(Guid findingId, string comment);
    Task<Tickets> CreateTicketAsync(Guid findingId, TicketType ticketType);
    Task<bool> DeleteTicketAsync(Guid findingId);
    Task<byte[]> ExportFindingAsync(FindingFilter filter);
    Task<Page<FindingActivity>> GetActivityAsync(Guid findingId, QueryFilter filter);
    Task<FindingDetail> GetFindingByIdAsync(Guid findingId);
    Task<Page<FindingSummary>> GetFindingByFilterAsync(FindingFilter filter);
    Task<List<string>> ListFindingCategoryAsync(FindingFilter filter);
    Task<List<string>> ListFindingRuleAsync(FindingFilter filter);
    Task<FindingDetail> UpdateFindingAsync(Guid findingId, UpdateFindingRequest request);
    Task<FindingStatus> UpdateStatusScanFindingAsync(Guid findingId, UpdateStatusScanFindingRequest request);
}

public class FindingService(IHttpContextAccessor accessor, AppDbContext context, IFindingAuthorize authorize)
    : BaseService(accessor), IFindingService
{
    public async Task<FindingActivity> CreateCommentAsync(Guid findingId, string comment)
    {
        return (await new CreateFindingCommentCommand(context, CurrentUser)
            .ExecuteAsync(findingId, comment)).GetResult();
    }

    public async Task<Tickets> CreateTicketAsync(Guid findingId, TicketType ticketType)
    {
        return (await new CreateFindingTicketCommand(context)
            .ExecuteAsync(findingId, ticketType)).GetResult();
    }

    public async Task<bool> DeleteTicketAsync(Guid findingId)
    {
        return (await new DeleteFindingTicketCommand(context)
            .ExecuteAsync(findingId)).GetResult();
    }

    public async Task<byte[]> ExportFindingAsync(FindingFilter filter)
    {
        return (await new ExportFindingCommand(context, CurrentUser)
            .ExecuteAsync(filter)).GetResult();
    }

    public async Task<Page<FindingActivity>> GetActivityAsync(Guid findingId, QueryFilter filter)
    {
        return (await new GetFindingActivityCommand(context)
            .ExecuteAsync(findingId, filter)).GetResult();
    }

    public async Task<FindingDetail> GetFindingByIdAsync(Guid findingId)
    {
        return (await new GetFindingByIdCommand(context)
            .ExecuteAsync(findingId)).GetResult();
    }

    public async Task<Page<FindingSummary>> GetFindingByFilterAsync(FindingFilter filter)
    {
        return (await new GetFindingByFilterCommand(context, CurrentUser)
            .ExecuteAsync(filter)).GetResult();
    }

    public async Task<List<string>> ListFindingCategoryAsync(FindingFilter filter)
    {
        return (await new ListFindingCategoryCommand(context, CurrentUser)
            .ExecuteAsync(filter)).GetResult();
    }

    public async Task<List<string>> ListFindingRuleAsync(FindingFilter filter)
    {
        return (await new ListFindingRulesCommand(context, CurrentUser)
            .ExecuteAsync(filter)).GetResult();
    }

    public async Task<FindingDetail> UpdateFindingAsync(Guid findingId, UpdateFindingRequest request)
    {
        var finding = (await new UpdateFindingCommand(context, CurrentUser)
            .ExecuteAsync(findingId, request)).GetResult();
        return await GetFindingByIdAsync(finding.Id);
    }

    public async Task<FindingStatus> UpdateStatusScanFindingAsync(Guid findingId, UpdateStatusScanFindingRequest request)
    {
        return (await new UpdateStatusScanFindingCommand(context, CurrentUser)
            .ExecuteAsync(findingId, request)).GetResult();
    }
}