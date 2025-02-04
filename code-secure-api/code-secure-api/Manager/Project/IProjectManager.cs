using CodeSecure.Database.Entity;
using CodeSecure.Enum;
using CodeSecure.Manager.Project.Model;
using CodeSecure.Manager.Setting;

namespace CodeSecure.Manager.Project;

public interface IProjectManager
{
    Task<SourceType> GetSourceTypeAsync(Guid sourceControlId);
    Task<Projects> CreateOrUpdateAsync(Projects input);
    Task<List<Member>> GetMembersAsync(Guid projectId);
    Task<DependencyReport?> DependencyReportAsync(Guid projectId);
    Task<DependencyReport> DependencyReportAsync(Projects project);
    Task<ProjectSetting> GetProjectSettingAsync(Guid projectId);
    Task<ThresholdSetting> GetSastSettingAsync(Guid projectId);
    Task UpdateSastSettingAsync(Guid projectId, ThresholdSetting setting);
    Task<ThresholdSetting> GetScaSettingAsync(Guid projectId);
    Task UpdateScaSettingAsync(Guid projectId, ThresholdSetting setting);
    Task<JiraProjectSetting> GetJiraSettingAsync(Guid projectId);
    Task UpdateJiraSettingAsync(Guid projectId, JiraProjectSetting setting);
    
    Task<AlertSetting> GetMailSettingAsync(Guid projectId);
    Task UpdateMailSettingAsync(Guid projectId, AlertSetting request);
    Task<TeamsSetting> GetTeamsSettingAsync(Guid projectId);
    Task UpdateTeamsSettingAsync(Guid projectId, TeamsSetting request);
}