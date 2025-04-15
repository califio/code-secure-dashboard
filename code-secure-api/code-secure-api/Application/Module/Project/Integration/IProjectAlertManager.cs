using CodeSecure.Application.Module.Integration;
using CodeSecure.Application.Module.Integration.Mail;
using CodeSecure.Application.Module.Integration.Teams;
using CodeSecure.Application.Module.Project.Integration.Mail;
using CodeSecure.Application.Module.Project.Integration.Teams;
using CodeSecure.Application.Module.Project.Setting;
using CodeSecure.Application.Services;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Integration;

public interface IProjectAlertManager
{
    public Task AlertNewFinding(AlertStatusFindingModel model);
    public Task AlertFixedFinding(AlertStatusFindingModel model);
    public Task AlertNeedTriageFinding(AlertNeedTriageFindingModel model);
    public Task AlertConfirmedFinding(AlertConfirmedFindingModel model);
    public Task AlertVulnerableProjectPackage(AlertVulnerableProjectPackageModel model);
}

public class ProjectAlertManager : IProjectAlertManager
{
    private readonly MailProjectAlertSetting mailAlertSetting;
    private readonly TeamsProjectSetting teamsSetting;
    private readonly List<ProjectUsers> projectUsers;
    private readonly ISmtpService smtpService;
    private readonly IRazorRender render;

    public ProjectAlertManager(AppDbContext context, Guid projectId, ISmtpService smtpService, IRazorRender render)
    {
        this.render = render;
        this.smtpService = smtpService;
        var projectSetting = context.ProjectSettings.First(x => x.ProjectId == projectId);
        mailAlertSetting = projectSetting.GetMailAlertSetting();
        teamsSetting = projectSetting.GetTeamsAlertSetting();
        projectUsers = context.ProjectUsers
            .Include(x => x.User)
            .Where(x => x.ProjectId == projectId)
            .ToList();
    }

    public async Task AlertNewFinding(AlertStatusFindingModel model)
    {
        var receivers = projectUsers.Select(x => x.User?.Email!).Distinct().ToList();
        if (!receivers.Any()) return;
        model.Findings.Sort((first, two) => two.Severity - first.Severity);
        // mail
        if (mailAlertSetting is { Active: true, NewFindingEvent: true })
        {
            await new AlertNewFindingMail(smtpService, render).AlertAsync(receivers, model);
        }

        // teams
        if (teamsSetting is { Active: true, NewFindingEvent: true })
        {
            await new AlertNewFindingTeams(teamsSetting.Webhook)
                .AlertAsync([], model);
        }
    }

    public async Task AlertFixedFinding(AlertStatusFindingModel model)
    {
        var receivers = projectUsers.Select(x => x.User?.Email!).Distinct().ToList();
        if (!receivers.Any()) return;
        model.Findings.Sort((first, two) => two.Severity - first.Severity);
        // mail
        if (mailAlertSetting is { Active: true, FixedFindingEvent: true })
        {
            await new AlertFixedFindingMail(render, smtpService).AlertAsync(receivers, model);
        }

        // teams
        if (teamsSetting is { Active: true, FixedFindingEvent: true })
        {
            await new AlertFixedFindingTeams(teamsSetting.Webhook).AlertAsync([], model);
        }
    }

    public async Task AlertNeedTriageFinding(AlertNeedTriageFindingModel model)
    {
        var receivers = projectUsers
            .Where(x => x.Role == ProjectRole.Validator)
            .Select(x => x.User?.Email!)
            .Distinct().ToList();
        if (!receivers.Any()) return;
        // mail
        if (mailAlertSetting is { Active: true, NeedTriageFindingEvent: true })
        {
            await new AlertNeedTriageFindingMail(smtpService, render).AlertAsync(receivers, model);
        }

        // teams
        if (teamsSetting is { Active: true, NeedTriageFindingEvent: true })
        {
            await new AlertNeedTriageFindingTeams(teamsSetting.Webhook).AlertAsync([], model);
        }
    }

    public async Task AlertConfirmedFinding(AlertConfirmedFindingModel model)
    {
        var receivers = projectUsers
            .Where(x => x.Role is ProjectRole.Developer or ProjectRole.Manager)
            .Select(x => x.User?.Email!)
            .Distinct().ToList();
        if (!receivers.Any()) return;
        // mail
        if (mailAlertSetting is { Active: true, SecurityAlertEvent: true })
        {
            await new AlertConfirmedFindingMail(smtpService, render).AlertAsync(receivers, model);
        }

        // teams
        if (teamsSetting is { Active: true, SecurityAlertEvent: true })
        {
            await new AlertConfirmedFindingTeams(teamsSetting.Webhook).AlertAsync([], model);
        }
    }

    public async Task AlertVulnerableProjectPackage(AlertVulnerableProjectPackageModel model)
    {
        var receivers = projectUsers.Select(x => x.User?.Email!).Distinct().ToList();
        if (!receivers.Any()) return;
        // mail
        if (mailAlertSetting is { Active: true, SecurityAlertEvent: true })
        {
            await new AlertVulnerableProjectPackageMail(smtpService, render).AlertAsync(receivers, model);
        }

        // teams
        if (teamsSetting is { Active: true, SecurityAlertEvent: true })
        {
            await new AlertVulnerableProjectPackageTeams(teamsSetting.Webhook).AlertAsync([], model);
        }
    }
}