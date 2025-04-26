using CodeSecure.Application.Module.Integration.Jira;
using CodeSecure.Application.Module.Integration.Redmine;
using CodeSecure.Application.Module.Project.Integration.Mail;
using CodeSecure.Application.Module.Project.Integration.Redmine;
using CodeSecure.Application.Module.Project.Integration.Teams;
using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Utils;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using JiraProjectSetting = CodeSecure.Application.Module.Project.Integration.Jira.JiraProjectSetting;

namespace CodeSecure.Application.Module.Project;

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

        jiraSetting.Active = globalSetting.Active;

        return jiraSetting;
    }

    public static RedmineProjectSetting GetRedmineSetting(this ProjectSettings setting, RedmineSetting globalSetting)
    {
        var redmineSetting = JSONSerializer.DeserializeOrDefault(setting.RedmineSetting, new RedmineProjectSetting());
        if (redmineSetting.ProjectId == 0)
        {
            redmineSetting.ProjectId = globalSetting.ProjectId;
        }

        if (redmineSetting.StatusId == 0)
        {
            redmineSetting.StatusId = globalSetting.StatusId;
        }

        if (redmineSetting.TrackerId == 0)
        {
            redmineSetting.TrackerId = globalSetting.TrackerId;
        }

        if (redmineSetting.PriorityId == 0)
        {
            redmineSetting.PriorityId = globalSetting.PriorityId;
        }

        redmineSetting.Active = globalSetting.Active;
        return redmineSetting;
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