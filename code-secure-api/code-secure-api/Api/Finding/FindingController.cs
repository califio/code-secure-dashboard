using CodeSecure.Api.Finding.Model;
using CodeSecure.Api.Finding.Service;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Extension;
using CodeSecure.Enum;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Finding;

public class FindingController(IFindingService findingService) : BaseController
{
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<FindingDetail> GetFinding(Guid id)
    {
        return await findingService.GetFindingAsync(id);
    }

    [HttpPatch]
    [Route("{id:guid}")]
    public async Task<FindingDetail> UpdateFinding(Guid id, UpdateFindingRequest request)
    {
        return await findingService.UpdateFindingAsync(id, request);
    }

    [HttpPost]
    [Route("{id:guid}/activity")]
    public async Task<Page<FindingActivity>> GetFindingActivities(Guid id, QueryFilter filter)
    {
        return await findingService.GetFindingActivitiesAsync(id, filter);
    }
    
    [HttpPost]
    [Route("{id:guid}/comment")]
    public async Task<FindingActivity> AddComment(Guid id, FindingCommentRequest request)
    {
        return await findingService.AddComment(id, request);
    }
    
    [HttpPost]
    [Route("{id:guid}/ticket")]
    public async Task<Tickets> CreateTicket(Guid id, TicketType type)
    {
        return await findingService.CreateTicketAsync(id, type);
    }
    
    [HttpDelete]
    [Route("{id:guid}/ticket")]
    public async Task DeleteTicket(Guid id)
    {
         await findingService.DeleteTicketAsync(id);
    }
}