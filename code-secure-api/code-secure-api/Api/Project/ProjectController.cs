using System.Net.Mime;
using CodeSecure.Application;
using CodeSecure.Application.Module.Integration.Jira;
using CodeSecure.Application.Module.Integration.Redmine;
using CodeSecure.Application.Module.Project;
using CodeSecure.Application.Module.Project.Integration;
using CodeSecure.Application.Module.Project.Integration.Jira;
using CodeSecure.Application.Module.Project.Integration.Mail;
using CodeSecure.Application.Module.Project.Integration.Teams;
using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Application.Module.Project.Setting;
using CodeSecure.Authentication;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Project;

public class ProjectController(
    IProjectAuthorize projectAuthorize, 
    IGeneralSettingProjectService generalSettingProjectService,
    IFindProjectByIdHandler findProjectByIdHandler,
    IFindProjectScanHandler findProjectScanHandler,
    IListProjectCommitHandler listProjectCommitHandler,
    IGetStatisticsProjectHandler getStatisticsProjectHandler,
    IJiraProjectIntegrationSetting jiraProjectIntegrationSetting,
    ITeamsProjectIntegrationSetting teamsProjectIntegrationSetting,
    IMailProjectIntegrationSetting mailProjectIntegrationSetting,
    IExportFindingHandler exportFindingHandler,
    IFindProjectHandler findProjectHandler,
    IGetProjectCommitScanSummary projectCommitScanSummary,
    AppDbContext context
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
    
    
    #region Integration
    [HttpGet]
    [Route("{projectId:guid}/integration")]
    public async Task<ProjectIntegration> GetIntegrationProject(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser(), PermissionAction.Read);
        var projectSetting = (await context.GetProjectSettingsAsync(projectId)).GetResult();
        var jiraGlobalSetting = await context.GetJiraSettingAsync();
        var redmineGlobalSetting = await context.GetRedmineSettingAsync();
        return new ProjectIntegration
        {
            Mail = projectSetting.GetMailAlertSetting().Active,
            Jira = projectSetting.GetJiraSetting(jiraGlobalSetting).Active,
            Teams = projectSetting.GetTeamsAlertSetting().Active,
            Redmine = projectSetting.GetRedmineSetting(redmineGlobalSetting).Active,
        };

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