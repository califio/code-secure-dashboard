using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Enum;
using CodeSecure.Extension;
using CodeSecure.Manager.Project.Model;
using CodeSecure.Manager.Scanner;
using CodeSecure.Manager.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CodeSecure.Manager.Project;

public class ProjectManager(
    AppDbContext context,
    IScannerManager scannerManager,
    ISettingManager settingManager,
    IMemoryCache cache,
    ILogger<ProjectManager> logger
    ) : IProjectManager
{
    private static readonly SemaphoreSlim Lock = new(1, 1);

    public async Task<SourceType> GetSourceTypeAsync(Guid sourceControlId)
    {
        var key = $"{nameof(SourceControls)}:{sourceControlId}";
        if (cache.TryGetValue(key, out SourceControls? sourceControl))
        {
            return sourceControl!.Type;
        }
        sourceControl = await context.SourceControls.FirstAsync(record => record.Id == sourceControlId);
        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(3));
        cache.Set(key, sourceControl, options);
        return sourceControl.Type;
    }

    public async Task<Projects> CreateOrUpdateAsync(Projects input)
    {
        await Lock.WaitAsync();
        try
        {
            var project = await context.Projects.FirstOrDefaultAsync(record =>
                record.SourceControlId == input.SourceControlId && record.RepoId == input.RepoId);
            if (project == null)
            {
                project = input;
                project.Id = Guid.NewGuid();
                context.Projects.Add(project);
                await context.SaveChangesAsync();
                var setting = new ProjectSettings
                {
                    ProjectId = project.Id,
                    SastSetting = JSONSerializer.Serialize(new ThresholdSetting()),
                    ScaSetting = JSONSerializer.Serialize(new ThresholdSetting()),
                };
                context.ProjectSettings.Add(setting);
                await context.SaveChangesAsync();
            }
            else
            {
                if (project.Name != input.Name)
                {
                    project.Name = input.Name;
                }

                if (project.RepoUrl != input.RepoUrl)
                {
                    project.RepoUrl = input.RepoUrl;
                }

                context.Projects.Update(project);
                await context.SaveChangesAsync();
            }

            return project;
        }
        finally
        {
            Lock.Release();
        }
    }

    public Task<List<Member>> GetMembersAsync(Guid projectId)
    {
        return context.ProjectUsers
            .Include(record => record.User)
            .Where(record => record.ProjectId == projectId)
            .Select(record => new Member
            {
                UserId = record.UserId,
                UserName = record.User!.UserName!,
                Email = record.User.Email!,
                Role = record.Role,
                Status = record.User.Status,
                FullName = record.User.FullName,
                Avatar = record.User.Avatar
            })
            .ToListAsync();
    }

    public async Task<DependencyReport?> DependencyReportAsync(Guid projectId)
    {
        var project = await context.Projects.FirstOrDefaultAsync(record => record.Id == projectId);
        if (project == null)
        {
            return null;
        }

        return await DependencyReportAsync(project);
    }

    public async Task<DependencyReport> DependencyReportAsync(Projects project)
    {
        logger.LogInformation($"Dependency report for project: {project.Name}");
        var scanners = (await scannerManager.GetScannerByTypeAsync(ScannerType.Dependency))
            .Select(item => item.Id);
        
        var findings = context.Findings
            .Where(record =>
                record.ProjectId == project.Id &&
                (record.Status == FindingStatus.Open || record.Status == FindingStatus.Confirmed) &&
                scanners.Contains(record.ScannerId)
            ).OrderByDescending(record => record.Severity)
            .ToList();
        var projectPackages = context.ProjectPackages
            .Include(record => record.Package!)
            .Where(record => record.ProjectId == project.Id).ToList();
        var packages = projectPackages
            .FindAll(item => !string.IsNullOrEmpty(item.Package!.FixedVersion))
            .Select(item =>
            {
                var name = string.IsNullOrEmpty(item.Package!.Group)
                    ? $"{item.Package.Name}@{item.Package.Version}"
                    : $"{item.Package.Group}:{item.Package.Name}@{item.Package.Version}";
                var recommendation = "";
                if (!string.IsNullOrEmpty(item.Package.FixedVersion))
                {
                    recommendation = $"Upgrade to version {item.Package.FixedVersion}";
                }

                return new DependencyProject
                {
                    Name = name,
                    Location = item.Location,
                    Recommendation = recommendation,
                    Impact = item.Package.RiskLevel.ToString()
                };
            }).ToList();
        var report = new DependencyReport
        {
            Critical = findings.Count(finding => finding.Severity == FindingSeverity.Critical),
            High = findings.Count(finding => finding.Severity == FindingSeverity.High),
            Medium = findings.Count(finding => finding.Severity == FindingSeverity.Medium),
            Low = findings.Count(finding => finding.Severity == FindingSeverity.Low),
            Packages = packages,
            RepoUrl = project.RepoUrl,
            RepoName = project.Name,
            ProjectDependencyUrl = $"{Application.Config.FrontendUrl}/#/project/{project.Id.ToString()}/dependency"
        };
        return report;
    }

    public async Task<ProjectSetting> GetProjectSettingAsync(Guid projectId)
    {
        var setting = await FindProjectSettingsAsync(projectId);
        var jiraSetting = JSONSerializer.DeserializeOrDefault(setting.JiraSetting, new JiraProjectSetting());
        var jiraGlobalSetting = await settingManager.GetJiraSettingAsync();
        jiraSetting.Active = jiraGlobalSetting.Active;
        if (string.IsNullOrEmpty(jiraSetting.ProjectKey))
        {
            jiraSetting.ProjectKey = jiraGlobalSetting.ProjectKey;
        }

        return new ProjectSetting
        {
            SastSetting = JSONSerializer.DeserializeOrDefault(setting.SastSetting,
                new ThresholdSetting()),
            ScaSetting = JSONSerializer.DeserializeOrDefault(setting.ScaSetting,
                new ThresholdSetting()),
            JiraSetting = jiraSetting,
        };
    }

    public async Task<ThresholdSetting> GetSastSettingAsync(Guid projectId)
    {
        var setting = await FindProjectSettingsAsync(projectId);
        return JSONSerializer.DeserializeOrDefault(setting.SastSetting, new ThresholdSetting());
    }

    public async Task<ThresholdSetting> GetScaSettingAsync(Guid projectId)
    {
        var setting = await FindProjectSettingsAsync(projectId);
        return JSONSerializer.DeserializeOrDefault(setting.ScaSetting, new ThresholdSetting());
    }

    public async Task UpdateSastSettingAsync(Guid projectId, ThresholdSetting setting)
    {
        var projectSetting = await FindProjectSettingsAsync(projectId);
        projectSetting.SastSetting = JSONSerializer.Serialize(setting);
        context.ProjectSettings.Update(projectSetting);
        await context.SaveChangesAsync();
    }

    public async Task UpdateScaSettingAsync(Guid projectId, ThresholdSetting setting)
    {
        var projectSetting = await FindProjectSettingsAsync(projectId);
        projectSetting.ScaSetting = JSONSerializer.Serialize(setting);
        context.ProjectSettings.Update(projectSetting);
        await context.SaveChangesAsync();
    }

    public async Task<JiraProjectSetting> GetJiraSettingAsync(Guid projectId)
    {
        var setting = await FindProjectSettingsAsync(projectId);
        var jiraSetting = JSONSerializer.DeserializeOrDefault(setting.JiraSetting, new JiraProjectSetting());
        var globalSetting = await settingManager.GetJiraSettingAsync();
        jiraSetting.Active = globalSetting.Active;
        if (string.IsNullOrEmpty(jiraSetting.ProjectKey))
        {
            jiraSetting.ProjectKey = globalSetting.ProjectKey;
        }

        if (string.IsNullOrEmpty(jiraSetting.IssueType))
        {
            jiraSetting.IssueType = globalSetting.IssueType;
        }
        return jiraSetting;
    }

    public async Task UpdateJiraSettingAsync(Guid projectId, JiraProjectSetting setting)
    {
        var projectSetting = await FindProjectSettingsAsync(projectId);
        projectSetting.JiraSetting = JSONSerializer.Serialize(setting);
        context.ProjectSettings.Update(projectSetting);
        await context.SaveChangesAsync();
    }

    public async Task<AlertSetting> GetMailSettingAsync(Guid projectId)
    {
        var setting = await FindProjectSettingsAsync(projectId);
        return JSONSerializer.DeserializeOrDefault(setting.MailSetting, new AlertSetting
        {
            Active = true,
            SecurityAlertEvent = true,
            NewFindingEvent = true,
            FixedFindingEvent = true,
            ScanCompletedEvent = false,
            ScanFailedEvent = true
        });
    }

    public async Task UpdateMailSettingAsync(Guid projectId, AlertSetting request)
    {
        var projectSetting = await FindProjectSettingsAsync(projectId);
        // always active by default
        request.Active = true;
        projectSetting.MailSetting = JSONSerializer.Serialize(request);
        context.ProjectSettings.Update(projectSetting);
        await context.SaveChangesAsync();
    }

    public async Task<TeamsSetting> GetTeamsSettingAsync(Guid projectId)
    {
        var setting = await FindProjectSettingsAsync(projectId);
        return JSONSerializer.DeserializeOrDefault(setting.TeamsSetting, new TeamsSetting());
    }

    public async Task UpdateTeamsSettingAsync(Guid projectId, TeamsSetting request)
    {
        var projectSetting = await FindProjectSettingsAsync(projectId);
        projectSetting.TeamsSetting = JSONSerializer.Serialize(request);
        context.ProjectSettings.Update(projectSetting);
        await context.SaveChangesAsync();
    }

    private async Task<ProjectSettings> FindProjectSettingsAsync(Guid projectId)
    {
        return await context.ProjectSettings
            .Where(record => record.ProjectId == projectId)
            .FirstAsync();
    }
}