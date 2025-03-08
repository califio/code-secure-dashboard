using CodeSecure.Api.Finding.Model;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Extension;
using CodeSecure.Enum;
using CodeSecure.Manager.Finding.Model;

namespace CodeSecure.Api.Finding.Service;

public interface IFindingService
{
    Task<Page<FindingSummary>> GetFindingsAsync(FindingFilter filter);
    Task<byte[]> Export(FindingFilter filter);

    Task<FindingDetail> GetFindingAsync(Guid id);
    Task<FindingDetail> UpdateFindingAsync(Guid findingId, UpdateFindingRequest request);
    Task UpdateStatusScanFindingAsync(Guid findingId, Guid scanId, FindingStatus status);
    Task<Page<FindingActivity>> GetFindingActivitiesAsync(Guid sid, QueryFilter filter);

    Task<FindingActivity> AddComment(Guid findingId, FindingCommentRequest request);
    Task<Tickets> CreateTicketAsync(Guid findingId, TicketType ticketType);
    Task DeleteTicketAsync(Guid findingId);
    Task<List<string>> GetFindingRulesAsync(FindingFilter filter);
}