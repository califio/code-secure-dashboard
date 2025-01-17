using CodeSecure.Api.Project.Model;
using CodeSecure.Database.Extension;
using CodeSecure.Database.Metadata;

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
    Task<ProjectSettingMetadata> GetProjectSettingAsync(Guid projectId);
    Task<ProjectSettingMetadata> UpdateProjectSettingAsync(Guid projectId, ProjectSettingMetadata request);
    
}