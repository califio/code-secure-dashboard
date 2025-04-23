using CodeSecure.Application.Module.Project;
using CodeSecure.Application.Module.Project.Package;
using CodeSecure.Authentication;
using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Extension;
using FluentResults.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Project;

[Route("api/project")]
[ApiExplorerSettings(GroupName = "Project")]
public class ProjectPackageController(
    IProjectAuthorize projectAuthorize,
    IFindProjectPackageHandler findProjectPackageHandler,
    IFindProjectPackageDetailHandler findProjectPackageDetailHandler,
    IUpdateProjectPackageHandler updateProjectPackageHandler,
    ICreateTicketPackageHandler createTicketPackageHandler,
    IDeleteTicketPackageHandler deleteTicketPackageHandler
    ): BaseController
{
    [HttpPost]
    [Route("{projectId}/package/filter")]
    public async Task<Page<ProjectPackage>> GetProjectPackages(Guid projectId, ProjectPackageFilter filter)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Read);
        filter.ProjectId = projectId;
        var result = await findProjectPackageHandler.HandleAsync(filter);
        return result.GetResult();
    }
    
    [HttpGet]
    [Route("{projectId:guid}/package/{packageId:guid}")]
    public async Task<ProjectPackageDetailResponse> GetProjectPackageDetail(Guid projectId, Guid packageId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Read);
        var result = await findProjectPackageDetailHandler.HandleAsync(new ProjectPackageDetailRequest
            { ProjectId = projectId, PackageId = packageId });
        return result.GetResult();
    }
    
    [HttpPatch]
    [Route("{projectId:guid}/package/{packageId:guid}")]
    public async Task<ProjectPackageDetailResponse> UpdateProjectPackage(Guid projectId, Guid packageId, UpdateProjectPackageRequest request)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        request.ProjectId = projectId;
        request.PackageId = packageId;
        var result = await updateProjectPackageHandler.HandleAsync(request)
            .Bind(projectPackage => findProjectPackageDetailHandler.HandleAsync(new ProjectPackageDetailRequest
            {
                ProjectId = projectPackage.ProjectId,
                PackageId = projectPackage.PackageId
            }));
        return result.GetResult();
    }
    
    [HttpPost]
    [Route("{projectId:guid}/package/{packageId:guid}/ticket")]
    public async Task<Tickets> CreateProjectTicket(Guid projectId, Guid packageId, TicketType ticketType)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        var result = await createTicketPackageHandler.HandleAsync(new CreateTicketPackageProjectRequest
        {
            TicketType = ticketType,
            ProjectId = projectId,
            PackageId = packageId
        });
        return result.GetResult();
    }
    
    [HttpDelete]
    [Route("{projectId:guid}/package/{packageId:guid}/ticket")]
    public async Task<bool> DeleteProjectTicket(Guid projectId, Guid packageId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        var result = await deleteTicketPackageHandler.HandleAsync(new DeleteTicketPackageRequest
        {
            ProjectId = projectId,
            PackageId = packageId
        });
        return result.GetResult();
    }
}