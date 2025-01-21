using CodeSecure.Api.Project.Model;
using CodeSecure.Api.Project.Service;
using CodeSecure.Database.Extension;
using CodeSecure.Database.Metadata;
using CodeSecure.Manager.EnvVariable;
using CodeSecure.Manager.EnvVariable.Model;
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
    public async Task<List<ProjectScanner>> GetProjectScanners(Guid projectId)
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
    public async Task<Page<ProjectFinding>> GetProjectFindings(Guid projectId, ProjectFindingFilter filter)
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
    public async Task<ProjectSettingMetadata> GetProjectSetting(Guid projectId)
    {
        return await projectService.GetProjectSettingAsync(projectId);
    }

    [HttpPost]
    [Route("{projectId}/setting")]
    public async Task<ProjectSettingMetadata> UpdateProjectSetting(Guid projectId, ProjectSettingMetadata request)
    {
        return await projectService.UpdateProjectSettingAsync(projectId, request);
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
}