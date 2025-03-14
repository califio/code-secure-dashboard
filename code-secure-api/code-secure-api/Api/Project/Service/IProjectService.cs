using CodeSecure.Api.Project.Model;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Extension;
using CodeSecure.Enum;
using CodeSecure.Manager.Finding.Model;
using CodeSecure.Manager.Project.Model;
using CodeSecure.Manager.Setting;

namespace CodeSecure.Api.Project.Service;

public interface IProjectService
{
    Task<Page<ProjectSummary>> GetProjectsAsync(ProjectFilter filter);
    Task<ProjectInfo> GetProjectInfoAsync(Guid projectId);
    Task<Page<ProjectScan>> GetScansAsync(Guid projectId, ProjectScanFilter filter);
    Task<List<ProjectCommitSummary>> GetCommitsAsync(Guid projectId);
    Task<List<Scanners>> GetScannersAsync(Guid projectId);
    Task<Page<FindingSummary>> GetFindingsAsync(Guid projectId, ProjectFindingFilter filter);
    Task<Page<ProjectPackage>> GetPackagesAsync(Guid projectId, ProjectPackageFilter filter);
    Task<ProjectPackageDetail> UpdateProjectPackageAsync(Guid projectId, Guid packageId, UpdateProjectPackageRequest request);
    Task<ProjectPackageDetail> GetPackageDetailAsync(Guid projectId, Guid packageId);
    Task<Tickets> CreateTicketAsync(Guid projectId, Guid packageId, TicketType ticketType);
    Task DeleteTicketAsync(Guid projectId, Guid packageId);
    
    Task<ProjectStatistics> GetStatisticsAsync(Guid projectId);
    Task<Page<ProjectUser>> GetMembersAsync(Guid projectId, ProjectUserFilter filter);
    Task<ProjectUser> AddMemberAsync(Guid projectId, AddMemberRequest request);
    Task<ProjectUser> UpdateMemberAsync(Guid projectId, Guid userId, UpdateMemberRequest request);
    Task DeleteMemberAsync(Guid projectId, Guid userId);
    Task<ProjectSetting> GetProjectSettingAsync(Guid projectId);
    Task UpdateSastSettingAsync(Guid projectId, ThresholdSetting request);
    Task UpdateScaSettingAsync(Guid projectId, ThresholdSetting request);
    
    Task<ProjectIntegration> GetIntegrationSettingAsync(Guid projectId);
    Task<JiraProjectSettingResponse> GetJiraIntegrationSettingAsync(Guid projectId, bool reload);
    Task UpdateJiraIntegrationSettingAsync(Guid projectId, JiraProjectSetting request);
    
    Task<TeamsSetting> GetTeamsIntegrationSettingAsync(Guid projectId);
    Task UpdateTeamsIntegrationSettingAsync(Guid projectId, TeamsSetting request);
    Task TestTeamsIntegrationSettingAsync(Guid projectId);
    
    Task<AlertSetting> GetMailIntegrationSettingAsync(Guid projectId);
    Task UpdateMailIntegrationSettingAsync(Guid projectId, AlertSetting request);
    Task<ExportModel> ExportAsync(ExportType type, Guid projectId, ProjectFindingFilter filter);

}