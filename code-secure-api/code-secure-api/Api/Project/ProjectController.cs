using System.Net.Mime;
using CodeSecure.Application.Module.Project;
using CodeSecure.Application.Module.Project.Integration;
using CodeSecure.Application.Module.Project.Integration.Jira;
using CodeSecure.Application.Module.Project.Integration.Mail;
using CodeSecure.Application.Module.Project.Integration.Teams;
using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Application.Module.Project.Package;
using CodeSecure.Application.Module.Project.Setting;
using CodeSecure.Application.Module.Project.Setting.Member;
using CodeSecure.Application.Module.Project.Setting.Threshold;
using CodeSecure.Authentication;
using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Extension;
using FluentResults.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Project;

public class ProjectController(
    IProjectAuthorize projectAuthorize, 
    IGeneralSettingProjectService generalSettingProjectService,
    IFindProjectByIdHandler findProjectByIdHandler,
    IFindProjectScanHandler findProjectScanHandler,
    IListProjectCommitHandler listProjectCommitHandler,
    IFindProjectPackageHandler findProjectPackageHandler,
    IFindProjectPackageDetailHandler findProjectPackageDetailHandler,
    IUpdateProjectPackageHandler updateProjectPackageHandler,
    ICreateTicketPackageHandler createTicketPackageHandler,
    IDeleteTicketPackageHandler deleteTicketPackageHandler,
    IGetStatisticsProjectHandler getStatisticsProjectHandler,
    IFindProjectMemberHandler findProjectMemberHandler,
    ICreateProjectMemberHandler createProjectMemberHandler,
    IUpdateProjectMemberHandler updateProjectMemberHandler,
    IDeleteProjectMemberHandler deleteProjectMemberHandler,
    IGetProjectThresholdHandler getProjectThresholdHandler,
    IUpdateProjectThresholdHandler updateProjectThresholdHandler,
    IJiraProjectIntegrationSetting jiraProjectIntegrationSetting,
    ITeamsProjectIntegrationSetting teamsProjectIntegrationSetting,
    IMailProjectIntegrationSetting mailProjectIntegrationSetting,
    IExportFindingHandler exportFindingHandler,
    IFindProjectHandler findProjectHandler,
    IGetProjectCommitScanSummary projectCommitScanSummary
    ) : BaseController
{
    [HttpPost]
    [Route("filter")]
    public async Task<Page<ProjectSummary>> GetProjects(ProjectFilter filter)
    {
        filter.CanReadAllProject = CurrentUser().HasClaim(PermissionType.Project, PermissionAction.Read);
        filter.CurrentUserId = CurrentUser().Id;
        var result = await findProjectHandler.HandleAsync(filter);
        return result.GetResult();
    }

    [HttpGet]
    [Route("{projectId:guid}")]
    public async Task<ProjectInfo> GetProjectInfo(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Read);
        var result = await findProjectByIdHandler.HandleAsync(projectId);
        return result.GetResult();
    }
    
    [HttpPost]
    [Route("{projectId:guid}/scan/filter")]
    public async Task<Page<ProjectScan>> GetProjectScans(Guid projectId, ProjectScanFilter filter)
    {
        filter.ProjectId = projectId;
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Read);
        var result = await findProjectScanHandler.HandleAsync(filter);
        return result.GetResult();
    }

    [HttpGet]
    [Route("{projectId:guid}/statistic")]
    public async Task<ProjectStatistics> GetProjectStatistic(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Read);
        var result = await getStatisticsProjectHandler.HandleAsync(projectId);
        return result.GetResult();
    }
    
    [HttpPost]
    [Route("{projectId:guid}/commit/filter")]
    public async Task<Page<ProjectCommitScanSummary>> GetProjectCommitScanSummary(Guid projectId, ProjectCommitFilter filter)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Read);
        var result = await projectCommitScanSummary.HandleAsync(projectId,filter);
        return result.GetResult();
    }

    [HttpGet]
    [Route("{projectId:guid}/commit")]
    public async Task<List<ProjectCommitSummary>> GetProjectCommits(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Read);
        var result = await listProjectCommitHandler.HandleAsync(projectId);
        return result.GetResult();
    }

    #region Package
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

    #endregion

    #region Member

    [HttpPost]
    [Route("{projectId}/member/filter")]
    public async Task<Page<ProjectMember>> GetProjectUsers(Guid projectId, ProjectMemberFilter filter)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Read);
        filter.ProjectId = projectId;
        var result = await findProjectMemberHandler.HandleAsync(filter);
        return result.GetResult();
    }

    [HttpPost]
    [Route("{projectId}/member")]
    public async Task<ProjectMember> AddMember(Guid projectId, CreateProjectMemberRequest request)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        request.ProjectId = projectId;
        request.CurrentUserId = CurrentUser().Id;
        var result = await createProjectMemberHandler.HandleAsync(request);
        return result.GetResult();
    }

    [HttpPatch]
    [Route("{projectId}/member/{userId:guid}")]
    public async Task<ProjectMember> UpdateProjectMember(Guid projectId, Guid userId, UpdateProjectMemberRequest request)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        request.ProjectId = projectId;
        request.UserId = userId;
        var result = await updateProjectMemberHandler.HandleAsync(request);
        return result.GetResult();
    }

    [HttpDelete]
    [Route("{projectId}/member/{userId:guid}")]
    public async Task<bool> DeleteProjectMember(Guid projectId, Guid userId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        var result = await deleteProjectMemberHandler.HandleAsync(new DeleteProjectMemberRequest
        {
            ProjectId = projectId,
            UserId = userId
        });
        return result.GetResult();
    }

    #endregion
    
    #region Threshold
    [HttpGet]
    [Route("{projectId}/threshold")]
    public async Task<ThresholdProject> GetThresholdProject(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Read);
        var result = await getProjectThresholdHandler.HandleAsync(projectId);
        return result.GetResult();
    }

    [HttpPost]
    [Route("{projectId}/threshold")]
    public async Task<bool> UpdateThresholdProject(Guid projectId, UpdateProjectThresholdRequest request)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        request.ProjectId = projectId;
        var result = await updateProjectThresholdHandler.HandleAsync(request);
        return result.GetResult();
    }
    
    #endregion
    
    #region Integration
    [HttpGet]
    [Route("{projectId:guid}/integration")]
    public async Task<ProjectIntegration> GetIntegrationProject(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Read);
        return new ProjectIntegration
        {
            Mail = (await mailProjectIntegrationSetting.GetSettingAsync(projectId)).GetResult().Active,
            Jira = (await jiraProjectIntegrationSetting.GetSettingAsync(projectId)).GetResult().Active,
            Teams = (await teamsProjectIntegrationSetting.GetSettingAsync(projectId)).GetResult().Active,
        };

    }

    [HttpGet]
    [Route("{projectId:guid}/integration/jira")]
    public async Task<JiraProjectSettingResponse> GetJiraIntegrationProject(Guid projectId, bool reload)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        var result = await jiraProjectIntegrationSetting.GetSettingAsync(projectId, reload);
        return  result.GetResult();
    }

    [HttpPost]
    [Route("{projectId:guid}/integration/jira")]
    public async Task<bool> UpdateJiraIntegrationProject(Guid projectId, [FromBody] JiraProjectSetting request)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        var result = await jiraProjectIntegrationSetting.UpdateSettingAsync(projectId, request);
        return result.GetResult();
    }
    
    [HttpGet]
    [Route("{projectId:guid}/integration/teams")]
    public async Task<TeamsProjectSetting> GetTeamsIntegrationProject(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        var result = await teamsProjectIntegrationSetting.GetSettingAsync(projectId);
        return result.GetResult();
    }

    [HttpPost]
    [Route("{projectId:guid}/integration/teams")]
    public async Task<bool> UpdateTeamsIntegrationProject(Guid projectId, [FromBody] TeamsProjectSetting request)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        var result = await teamsProjectIntegrationSetting.UpdateSettingAsync(projectId, request);
        return result.GetResult();
    }
    
    [HttpPost]
    [Route("{projectId:guid}/integration/teams/test")]
    public async Task<bool> TestTeamsIntegrationProject(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        var result = await teamsProjectIntegrationSetting.TestConnectionAsync(projectId);
        return result.GetResult();
    }
    
    [HttpGet]
    [Route("{projectId:guid}/integration/mail")]
    public async Task<ProjectAlertEvent> GetMailIntegrationProject(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        var result = await mailProjectIntegrationSetting.GetSettingAsync(projectId);
        return result.GetResult();
    }

    [HttpPost]
    [Route("{projectId:guid}/integration/mail")]
    public async Task<bool> UpdateMailIntegrationProject(Guid projectId, [FromBody] MailProjectAlertSetting request)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        var result = await mailProjectIntegrationSetting.UpdateSettingAsync(projectId, request);
        return result.GetResult();
    }
    #endregion

    #region Default Branches
    [HttpGet]
    [Route("{projectId:guid}/defaultBranch")]
    public async Task<HashSet<string>> GetDefaultBranchesProject(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Read);
        var result = await generalSettingProjectService.GetDefaultBranchesAsync(projectId);
        return result.GetResult();
    }
    [HttpPost]
    [Route("{projectId:guid}/defaultBranch")]
    public async Task<bool> UpdateDefaultBranchesProject(Guid projectId, HashSet<string> defaultBranches)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Update);
        var result = await generalSettingProjectService.UpdateDefaultBranchesAsync(projectId, defaultBranches);
        return result.GetResult();
    }
    #endregion
    [HttpPost]
    [Route("{projectId:guid}/export")]
    [Produces(MediaTypeNames.Application.Octet)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    public async Task<FileContentResult> Export(Guid projectId, ExportFindingRequest request)
    {
        request.Filter.ProjectId = projectId;
        request.Filter.CanReadAllFinding = CurrentUser().HasClaim(PermissionType.Finding, PermissionAction.Read);
        request.Filter.CurrentUserId = CurrentUser().Id;
        var result = await exportFindingHandler.HandleAsync(request);
        return new FileContentResult(result.GetResult(), MediaTypeNames.Application.Octet)
        {
            FileDownloadName = "export"
        };
    }
}