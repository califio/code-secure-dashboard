using System.Net.Mime;
using CodeSecure.Application.Module.Finding;
using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Authentication;
using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Extension;
using FluentResults.Extensions;
using Microsoft.AspNetCore.Mvc;
namespace CodeSecure.Api.Finding;

public class FindingController(
    IFindingAuthorize findingAuthorize,
    IFindFindingHandler findFindingHandler,
    IListFindingRulesHandler listFindingRulesHandler,
    IDeleteTicketFindingHandler deleteTicketFindingHandler,
    IListFindingCategoryHandler listFindingCategoryHandler,
    ICreateTicketFindingHandler createTicketFindingHandler,
    ICreateCommentFindingHandler createCommentFindingHandler,
    IFindActivityFindingHandler findActivityFindingHandler,
    IFindFindingByIdHandler findFindingByIdHandler,
    IUpdateFindingHandler updateFindingHandler,
    IExportFindingHandler exportFindingHandler,
    IUpdateStatusScanFindingHandler updateStatusScanFindingHandler
) : BaseController
{
    [HttpPost]
    [Route("filter")]
    public async Task<Page<FindingSummary>> GetFindings(FindingFilter filter)
    {
        var currentUser = CurrentUser();
        filter.CanReadAllFinding = currentUser.HasClaim(PermissionType.Finding, PermissionAction.Read);
        filter.CurrentUserId = currentUser.Id;
        var result = await findFindingHandler.HandleAsync(filter);
        return result.GetResult();
    }

    [HttpPost]
    [Route("export")]
    [Produces(MediaTypeNames.Application.Octet)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    public async Task<FileContentResult> ExportFinding(FindingFilter filter)
    {
        filter.CanReadAllFinding = CurrentUser().HasClaim(PermissionType.Finding, PermissionAction.Read);
        filter.CurrentUserId = CurrentUser().Id;
        var result = await exportFindingHandler.HandleAsync(filter);
        return new FileContentResult(result.GetResult(), MediaTypeNames.Application.Octet)
        {
            FileDownloadName = "export_findings.xlsx"
        };
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<FindingDetail> GetFinding(Guid id)
    {
        findingAuthorize.Authorize(id, CurrentUser(), PermissionAction.Read);
        var result = await findFindingByIdHandler.HandleAsync(id);
        return result.GetResult();
    }

    [HttpPatch]
    [Route("{id:guid}")]
    public async Task<FindingDetail> UpdateFinding(Guid id, UpdateFindingRequest request)
    {
        findingAuthorize.Authorize(id, CurrentUser(), PermissionAction.Update);
        request.CurrentUserId = CurrentUser().Id;
        request.FindingId = id;
        var result = await updateFindingHandler.HandleAsync(request)
            .Bind(finding => findFindingByIdHandler.HandleAsync(finding.Id));
        return result.GetResult();
    }

    [HttpPatch]
    [Route("{findingId:guid}/scan-status")]
    public async Task<FindingStatus> UpdateStatusScanFinding(Guid findingId, UpdateStatusScanFindingRequest request)
    {
        findingAuthorize.Authorize(findingId, CurrentUser(), PermissionAction.Update);
        request.CurrentUserId = CurrentUser().Id;
        request.FindingId = findingId;
        var result = await updateStatusScanFindingHandler.HandleAsync(request);
        return result.GetResult();
    }


    [HttpPost]
    [Route("{id:guid}/activity")]
    public async Task<Page<FindingActivity>> GetFindingActivities(Guid id, QueryFilter filter)
    {
        findingAuthorize.Authorize(id, CurrentUser(), PermissionAction.Read);
        var result = await findActivityFindingHandler.HandleAsync(new ActivityFindingFilter
        {
            Size = filter.Size,
            Page = filter.Page,
            Desc = filter.Desc,
            FindingId = id
        });
        return result.GetResult();
    }

    [HttpPost]
    [Route("{id:guid}/comment")]
    public async Task<FindingActivity> AddComment(Guid id, FindingCommentRequest request)
    {
        // allow project member comment on finding
        findingAuthorize.Authorize(id, CurrentUser(), PermissionAction.Read);
        var result = await createCommentFindingHandler.HandleAsync(new CreateCommentFindingRequest
        {
            FindingId = id,
            Comment = request.Comment,
            CurrentUser = CurrentUser()
        });
        return result.GetResult();
    }

    [HttpPost]
    [Route("{id:guid}/ticket")]
    public async Task<Tickets> CreateTicket(Guid id, TicketType type)
    {
        findingAuthorize.Authorize(id, CurrentUser(), PermissionAction.Update);
        var result = await createTicketFindingHandler.HandleAsync(new CreateTicketFindingRequest
        {
            FindingId = id,
            TicketType = type
        });
        return result.GetResult();
    }

    [HttpDelete]
    [Route("{id:guid}/ticket")]
    public async Task<bool> DeleteTicket(Guid id)
    {
        findingAuthorize.Authorize(id, CurrentUser(), PermissionAction.Update);
        var result = await deleteTicketFindingHandler.HandleAsync(id);
        return result.GetResult();
    }

    [HttpPost]
    [Route("rule")]
    public async Task<List<string>> GetFindingRules(FindingFilter filter)
    {
        var currentUser = CurrentUser();
        filter.CanReadAllFinding = currentUser.HasClaim(PermissionType.Finding, PermissionAction.Read);
        filter.CurrentUserId = currentUser.Id;
        var result = await listFindingRulesHandler.HandleAsync(filter);
        return result.GetResult();
    }

    [HttpPost]
    [Route("category")]
    public async Task<List<string>> GetFindingCategories(FindingFilter filter)
    {
        var currentUser = CurrentUser();
        filter.CanReadAllFinding = currentUser.HasClaim(PermissionType.Finding, PermissionAction.Read);
        filter.CurrentUserId = currentUser.Id;
        var result = await listFindingCategoryHandler.HandleAsync(filter);
        return result.GetResult();
    }
}