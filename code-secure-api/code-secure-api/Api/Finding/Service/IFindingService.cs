using CodeSecure.Api.Finding.Model;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Extension;
using CodeSecure.Enum;

namespace CodeSecure.Api.Finding.Service;

public interface IFindingService
{
    Task<FindingDetail> GetFindingAsync(Guid id);
    Task<FindingDetail> UpdateFindingAsync(Guid findingId, UpdateFindingRequest request);
    Task<Page<FindingActivity>> GetFindingActivitiesAsync(Guid sid, QueryFilter filter);

    Task<FindingActivity> AddComment(Guid findingId, FindingCommentRequest request);
    Task<Tickets> CreateTicketAsync(Guid findingId, TicketType ticketType);
    Task DeleteTicketAsync(Guid findingId);
}