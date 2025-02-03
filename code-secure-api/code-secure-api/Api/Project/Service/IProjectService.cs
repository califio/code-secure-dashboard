using CodeSecure.Api.Project.Model;
using CodeSecure.Database.Extension;
using CodeSecure.Manager.Project.Model;

namespace CodeSecure.Api.Project.Service;

public interface IProjectService
{
    Task<Page<ProjectSummary>> GetProjectsAsync(ProjectFilter filter);
    Task<ProjectInfo> GetProjectInfoAsync(Guid projectId);
    Task<Page<ProjectScan>> GetScansAsync(Guid projectId, ProjectScanFilter filter);
    Task<List<ProjectCommitSummary>> GetCommitsAsync(Guid projectId);
    Task<List<ProjectScanner>> GetScannersAsync(Guid projectId);
    Task<Page<ProjectFinding>> GetFindingsAsync(Guid projectId, ProjectFindingFilter filter);
    Task<Page<ProjectPackage>> GetPackagesAsync(Guid projectId, ProjectPackageFilter filter);
    Task<ProjectStatistics> GetStatisticsAsync(Guid projectId);
    Task<Page<ProjectUser>> GetMembersAsync(Guid projectId, ProjectUserFilter filter);
    Task<ProjectUser> AddMemberAsync(Guid projectId, AddMemberRequest request);
    Task<ProjectUser> UpdateMemberAsync(Guid projectId, Guid userId, UpdateMemberRequest request);
    Task DeleteMemberAsync(Guid projectId, Guid userId);
    Task<ProjectSetting> GetProjectSettingAsync(Guid projectId);
    Task UpdateSastSettingAsync(Guid projectId, ThresholdSetting request);
    Task UpdateScaSettingAsync(Guid projectId, ThresholdSetting request);
    Task<JiraProjectSettingResponse> GetJiraProjectSettingAsync(Guid projectId, bool reload);
    Task UpdateJiraProjectSettingAsync(Guid projectId, JiraProjectSetting request);
    
}