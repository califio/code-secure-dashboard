using System.Net.Mime;
using CodeSecure.Application;
using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Application.Module.Integration.Jira;
using CodeSecure.Application.Module.Integration.Redmine;
using CodeSecure.Application.Module.Project;
using CodeSecure.Application.Module.Project.Integration;
using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Authentication;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Extension;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Project;

public class ProjectController(
    IProjectService projectService,
    IProjectAuthorize projectAuthorize, 
    IProjectSettingService projectSettingService,
    AppDbContext context
    ) : BaseController
{
    [HttpPost]
    [Route("filter")]
    public Task<Page<ProjectSummary>> GetProjectByFilter(ProjectFilter filter)
    {
        return projectService.GetProjectByFilterAsync(filter);
    }

    [HttpGet]
    [Route("{projectId:guid}")]
    public async Task<ProjectInfo> GetProjectInfo(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser, PermissionAction.Read);
        return await projectService.GetProjectByIdAsync(projectId);
    }
    
    [HttpPost]
    [Route("{projectId:guid}/scan/filter")]
    public async Task<Page<ProjectScan>> GetProjectScans(Guid projectId, ProjectScanFilter filter)
    {
        projectAuthorize.Authorize(projectId, CurrentUser, PermissionAction.Read);
        return await projectService.GetProjectScanByFilterAsync(projectId, filter);
    }

    [HttpGet]
    [Route("{projectId:guid}/statistic")]
    public async Task<ProjectStatistics> GetProjectStatistic(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser, PermissionAction.Read);
        return await projectService.GetStatsAsync(projectId);
    }
    
    [HttpPost]
    [Route("{projectId:guid}/commit/filter")]
    public async Task<Page<ProjectCommitScanSummary>> GetProjectCommitScanSummary(Guid projectId, ProjectCommitFilter filter)
    {
        projectAuthorize.Authorize(projectId, CurrentUser, PermissionAction.Read);
        return await projectService.GetProjectCommitScanSummaryAsync(projectId, filter);
    }

    [HttpGet]
    [Route("{projectId:guid}/commit")]
    public async Task<List<ProjectCommitSummary>> ListProjectCommit(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser, PermissionAction.Read);
        return await projectService.ListProjectCommitAsync(projectId);
    }
    
    
    #region Integration
    [HttpGet]
    [Route("{projectId:guid}/integration")]
    public async Task<ProjectIntegration> GetIntegrationProject(Guid projectId)
    {
        projectAuthorize.Authorize(projectId, CurrentUser, PermissionAction.Read);
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
        projectAuthorize.Authorize(projectId, CurrentUser, PermissionAction.Read);
        var result = await projectSettingService.GetDefaultBranchesAsync(projectId);
        return result.GetResult();
    }
    [HttpPost]
    [Route("{projectId:guid}/defaultBranch")]
    public async Task<bool> UpdateDefaultBranchesProject(Guid projectId, HashSet<string> defaultBranches)
    {
        projectAuthorize.Authorize(projectId, CurrentUser, PermissionAction.Update);
        var result = await projectSettingService.UpdateDefaultBranchesAsync(projectId, defaultBranches);
        return result.GetResult();
    }
    #endregion
    [HttpPost]
    [Route("{projectId:guid}/export")]
    [Produces(MediaTypeNames.Application.Octet)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    public async Task<FileContentResult> Export(Guid projectId, [FromQuery] ExportType exportType, [FromBody] FindingFilter filter)
    {
        var data = await projectService.ExportAsync(projectId, exportType, filter);
        return new FileContentResult(data, MediaTypeNames.Application.Octet)
        {
            FileDownloadName = "export"
        };
    }
}