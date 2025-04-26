using System.Net.Mime;
using CodeSecure.Application.Module.Finding;
using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Authentication;
using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Finding;

public class FindingController(
    IFindingService findingService,
    IFindingAuthorize findingAuthorize
) : BaseController
{
    [HttpPost]
    [Route("filter")]
    public async Task<Page<FindingSummary>> GetFindings(FindingFilter filter)
    {
        return await findingService.GetFindingByFilterAsync(filter);
    }

    [HttpPost]
    [Route("export")]
    [Produces(MediaTypeNames.Application.Octet)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    public async Task<FileContentResult> ExportFinding(FindingFilter filter)
    {
        var data = await findingService.ExportFindingAsync(filter);
        return new FileContentResult(data, MediaTypeNames.Application.Octet)
        {
            FileDownloadName = "export_findings.xlsx"
        };
    }

    [HttpGet]
    [Route("{findingId:guid}")]
    public async Task<FindingDetail> GetFindingById(Guid findingId)
    {
        findingAuthorize.Authorize(findingId, CurrentUser, PermissionAction.Read);
        return await findingService.GetFindingByIdAsync(findingId);
    }

    [HttpPatch]
    [Route("{findingId:guid}")]
    public async Task<FindingDetail> UpdateFinding(Guid findingId, UpdateFindingRequest request)
    {
        findingAuthorize.Authorize(findingId, CurrentUser, PermissionAction.Update);
        return await findingService.UpdateFindingAsync(findingId, request);
    }

    [HttpPatch]
    [Route("{findingId:guid}/scan-status")]
    public async Task<FindingStatus> UpdateStatusScanFinding(Guid findingId, UpdateStatusScanFindingRequest request)
    {
        findingAuthorize.Authorize(findingId, CurrentUser, PermissionAction.Update);
        return await findingService.UpdateStatusScanFindingAsync(findingId, request);
    }


    [HttpPost]
    [Route("{findingId:guid}/activity")]
    public async Task<Page<FindingActivity>> GetFindingActivities(Guid findingId, QueryFilter filter)
    {
        findingAuthorize.Authorize(findingId, CurrentUser, PermissionAction.Read);
        return await findingService.GetActivityAsync(findingId, filter);
    }

    [HttpPost]
    [Route("{findingId:guid}/comment")]
    public async Task<FindingActivity> AddComment(Guid findingId, FindingCommentRequest request)
    {
        // allow project member comment on finding
        findingAuthorize.Authorize(findingId, CurrentUser, PermissionAction.Read);
        return await findingService.CreateCommentAsync(findingId, request.Comment);
    }

    [HttpPost]
    [Route("{findingId:guid}/ticket")]
    public async Task<Tickets> CreateTicket(Guid findingId, TicketType type)
    {
        findingAuthorize.Authorize(findingId, CurrentUser, PermissionAction.Update);
        return await findingService.CreateTicketAsync(findingId, type);
    }

    [HttpDelete]
    [Route("{findingId:guid}/ticket")]
    public async Task<bool> DeleteTicket(Guid findingId)
    {
        findingAuthorize.Authorize(findingId, CurrentUser, PermissionAction.Update);
        return await findingService.DeleteTicketAsync(findingId);
    }

    [HttpPost]
    [Route("rule")]
    public async Task<List<string>> GetFindingRules(FindingFilter filter)
    {
        return await findingService.ListFindingRuleAsync(filter);
    }

    [HttpPost]
    [Route("category")]
    public async Task<List<string>> ListFindingCategory(FindingFilter filter)
    {
        return await findingService.ListFindingCategoryAsync(filter);
    }
}