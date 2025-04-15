using CodeSecure.Application.Module.Integration.Jira;
using CodeSecure.Application.Module.Project.Integration.Mail;
using CodeSecure.Application.Module.Project.Integration.Teams;
using CodeSecure.Application.Module.Project.Setting.Threshold;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Utils;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using JiraProjectSetting = CodeSecure.Application.Module.Project.Integration.Jira.JiraProjectSetting;

namespace CodeSecure.Application.Module.Project.Setting;

public static class ProjectSettingExtension
{
    public static async Task<Result<ProjectSettings>> GetProjectSettingsAsync(this AppDbContext context, Guid projectId)
    {
        var projectSetting = await context.ProjectSettings.FirstOrDefaultAsync(record => record.ProjectId == projectId);
        if (projectSetting == null) return Result.Fail($"Project does not exist");
        return projectSetting;
    }

    public static MailProjectAlertSetting GetMailAlertSetting(this ProjectSettings setting)
    {
        return JSONSerializer.DeserializeOrDefault(setting.MailSetting, new MailProjectAlertSetting
        {
            Active = true,
            SecurityAlertEvent = true,
            NewFindingEvent = false,
            FixedFindingEvent = false,
            ScanCompletedEvent = false,
            ScanFailedEvent = false
        });
    }

    public static TeamsProjectSetting GetTeamsAlertSetting(this ProjectSettings setting)
    {
        return JSONSerializer.DeserializeOrDefault(setting.TeamsSetting, new TeamsProjectSetting());
    }

    public static JiraProjectSetting GetJiraSetting(this ProjectSettings setting, JiraSetting globalSetting)
    {
        var jiraSetting = JSONSerializer.DeserializeOrDefault(setting.JiraSetting, new JiraProjectSetting());
        if (string.IsNullOrEmpty(jiraSetting.ProjectKey))
        {
            jiraSetting.ProjectKey = globalSetting.ProjectKey;
        }

        if (string.IsNullOrEmpty(jiraSetting.IssueType))
        {
            jiraSetting.IssueType = globalSetting.IssueType;
        }

        if (globalSetting.Active == false)
        {
            jiraSetting.Active = false;
        }

        return jiraSetting;
    }

    public static ThresholdSetting GetSastSetting(this ProjectSettings setting)
    {
        return JSONSerializer.DeserializeOrDefault(setting.SastSetting, new ThresholdSetting());
    }

    public static ThresholdSetting GetScaSetting(this ProjectSettings setting)
    {
        return JSONSerializer.DeserializeOrDefault(setting.ScaSetting, new ThresholdSetting());
    }
    
    public static HashSet<string> GetDefaultBranches(this ProjectSettings setting)
    {
        var branches = JSONSerializer.DeserializeOrDefault(setting.DefaultBranch, new HashSet<string>());
        if (branches.Count == 0)
        {
            branches = ["main"];
        }
        return branches;
    }
}