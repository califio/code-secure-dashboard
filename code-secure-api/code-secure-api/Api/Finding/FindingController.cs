using System.Net.Mime;
using CodeSecure.Api.Finding.Model;
using CodeSecure.Api.Finding.Service;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Extension;
using CodeSecure.Enum;
using CodeSecure.Manager.Finding.Model;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Finding;

public class FindingController(IFindingService findingService) : BaseController
{
    [HttpPost]
    [Route("filter")]
    public async Task<Page<FindingSummary>> GetFindings(FindingFilter filter)
    {
        return await findingService.GetFindingsAsync(filter);
    }

    [HttpPost]
    [Route("export")]
    [Produces(MediaTypeNames.Application.Octet)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    public async Task<FileContentResult> ExportFinding(FindingFilter filter)
    {
        var data = await findingService.Export(filter);
        return new FileContentResult(data, MediaTypeNames.Application.Octet)
        {
            FileDownloadName = "export_findings.xlsx"
        };
    }

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

    [HttpPatch]
    [Route("{findingId:guid}/scan-status")]
    public async Task UpdateStatusScanFinding(Guid findingId, UpdateStatusScanFindingRequest request)
    {
        await findingService.UpdateStatusScanFindingAsync(findingId, request.ScanId, request.Status);
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
    
    [HttpPost]
    [Route("rule")]
    public async Task<List<string>> GetFindingRules(FindingFilter filter)
    {
        return await findingService.GetFindingRulesAsync(filter);
    }
    
    [HttpPost]
    [Route("category")]
    public async Task<List<string>> GetFindingCategories(FindingFilter filter)
    {
        return await findingService.GetFindingCategoriesAsync(filter);
    }
}