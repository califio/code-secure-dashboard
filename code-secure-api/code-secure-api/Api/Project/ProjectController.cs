using System.Net.Mime;
using CodeSecure.Api.Project.Model;
using CodeSecure.Api.Project.Service;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Extension;
using CodeSecure.Enum;
using CodeSecure.Manager.EnvVariable;
using CodeSecure.Manager.EnvVariable.Model;
using CodeSecure.Manager.Finding.Model;
using CodeSecure.Manager.Project.Model;
using CodeSecure.Manager.Setting;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.Project;

public class ProjectController(IProjectService projectService, IEnvVariableManager envVariableManager) : BaseController
{
    [HttpGet]
    public async Task<Page<ProjectSummary>> GetProjects([FromQuery] ProjectFilter filter)
    {
        return await projectService.GetProjectsAsync(filter);
    }

    [HttpGet]
    [Route("{projectId}")]
    public async Task<ProjectInfo> GetProjectInfo(Guid projectId)
    {
        return await projectService.GetProjectInfoAsync(projectId);
    }

    [HttpGet]
    [Route("{projectId}/statistic")]
    public async Task<ProjectStatistics> GetProjectStatistic(Guid projectId)
    {
        return await projectService.GetStatisticsAsync(projectId);
    }

    [HttpGet]
    [Route("{projectId}/commit")]
    public async Task<List<ProjectCommitSummary>> GetProjectCommits(Guid projectId)
    {
        return await projectService.GetCommitsAsync(projectId);
    }

    [HttpGet]
    [Route("{projectId}/scanner")]
    public async Task<List<Scanners>> GetProjectScanners(Guid projectId)
    {
        return await projectService.GetScannersAsync(projectId);
    }

    [HttpPost]
    [Route("{projectId}/scan/filter")]
    public async Task<Page<ProjectScan>> GetProjectScans(Guid projectId, ProjectScanFilter filter)
    {
        return await projectService.GetScansAsync(projectId, filter);
    }

    [HttpPost]
    [Route("{projectId}/finding/filter")]
    public async Task<Page<FindingSummary>> GetProjectFindings(Guid projectId, ProjectFindingFilter filter)
    {
        return await projectService.GetFindingsAsync(projectId, filter);
    }

    [HttpPost]
    [Route("{projectId}/package/filter")]
    public async Task<Page<ProjectPackage>> GetProjectPackages(Guid projectId, ProjectPackageFilter filter)
    {
        return await projectService.GetPackagesAsync(projectId, filter);
    }

    [HttpPost]
    [Route("{projectId}/member/filter")]
    public async Task<Page<ProjectUser>> GetProjectUsers(Guid projectId, ProjectUserFilter filter)
    {
        return await projectService.GetMembersAsync(projectId, filter);
    }

    [HttpPost]
    [Route("{projectId}/member")]
    public async Task<ProjectUser> AddMember(Guid projectId, AddMemberRequest request)
    {
        return await projectService.AddMemberAsync(projectId, request);
    }

    [HttpPut]
    [Route("{projectId}/member/{userId:guid}")]
    public async Task<ProjectUser> UpdateProjectMember(Guid projectId, Guid userId, UpdateMemberRequest request)
    {
        return await projectService.UpdateMemberAsync(projectId, userId, request);
    }

    [HttpDelete]
    [Route("{projectId}/member/{userId:guid}")]
    public async Task DeleteProjectMember(Guid projectId, Guid userId)
    {
        await projectService.DeleteMemberAsync(projectId, userId);
    }

    [HttpGet]
    [Route("{projectId}/setting")]
    public async Task<ProjectSetting> GetProjectSetting(Guid projectId)
    {
        return await projectService.GetProjectSettingAsync(projectId);
    }

    [HttpPost]
    [Route("{projectId}/setting/sast")]
    public async Task UpdateSastSettingProject(Guid projectId, ThresholdSetting request)
    {
        await projectService.UpdateSastSettingAsync(projectId, request);
    }

    [HttpPost]
    [Route("{projectId}/setting/sca")]
    public async Task UpdateScaSettingProject(Guid projectId, ThresholdSetting request)
    {
        await projectService.UpdateScaSettingAsync(projectId, request);
    }
    
    [HttpGet]
    [Route("{projectId}/integration")]
    public async Task<ProjectIntegration> GetIntegrationProject(Guid projectId)
    {
        return await projectService.GetIntegrationSettingAsync(projectId);
    }

    [HttpGet]
    [Route("{projectId}/integration/jira")]
    public async Task<JiraProjectSettingResponse> GetJiraIntegrationProject(Guid projectId, bool reload)
    {
        return await projectService.GetJiraIntegrationSettingAsync(projectId, reload);
    }

    [HttpPost]
    [Route("{projectId}/integration/jira")]
    public async Task UpdateJiraIntegrationProject(Guid projectId, [FromBody] JiraProjectSetting setting)
    {
        await projectService.UpdateJiraIntegrationSettingAsync(projectId, setting);
    }
    
    [HttpGet]
    [Route("{projectId}/integration/teams")]
    public async Task<TeamsSetting> GetTeamsIntegrationProject(Guid projectId)
    {
        return await projectService.GetTeamsIntegrationSettingAsync(projectId);
    }

    [HttpPost]
    [Route("{projectId}/integration/teams")]
    public async Task UpdateTeamsIntegrationProject(Guid projectId, [FromBody] TeamsSetting setting)
    {
        await projectService.UpdateTeamsIntegrationSettingAsync(projectId, setting);
    }
    
    [HttpPost]
    [Route("{projectId}/integration/teams/test")]
    public async Task TestTeamsIntegrationProject(Guid projectId)
    {
        await projectService.TestTeamsIntegrationSettingAsync(projectId);
    }
    
    [HttpGet]
    [Route("{projectId}/integration/mail")]
    public async Task<AlertSetting> GetMailIntegrationProject(Guid projectId)
    {
        return await projectService.GetMailIntegrationSettingAsync(projectId);
    }

    [HttpPost]
    [Route("{projectId}/integration/mail")]
    public async Task UpdateMailIntegrationProject(Guid projectId, [FromBody] AlertSetting setting)
    {
        await projectService.UpdateMailIntegrationSettingAsync(projectId, setting);
    }

    [HttpPost]
    [Route("{projectId}/env/filter")]
    public async Task<Page<EnvironmentVariable>> GetProjectEnvironment(Guid projectId, QueryFilter filter)
    {
        return await envVariableManager.GetProjectEnvironmentAsync(projectId, filter);
    }

    [HttpPost]
    [Route("{projectId}/env")]
    public async Task<EnvironmentVariable> SetProjectEnvironment(Guid projectId, EnvironmentVariable request)
    {
        return await envVariableManager.SetProjectEnvironmentAsync(projectId, request.Name, request.Value);
    }

    [HttpDelete]
    [Route("{projectId}/env/{name}")]
    public async Task RemoveProjectEnvironment(Guid projectId, string name)
    {
        await envVariableManager.RemoveProjectEnvironmentAsync(projectId, name);
    }
    
    [HttpPost]
    [Route("{projectId}/export")]
    [Produces(MediaTypeNames.Application.Octet)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    public async Task<FileContentResult> Export(ExportType format, Guid projectId, ProjectFindingFilter filter)
    {
        var result = await projectService.ExportAsync(format, projectId, filter);
        return new FileContentResult(result.Data, MediaTypeNames.Application.Octet)
        {
            FileDownloadName = result.FileName
        };
    }
}