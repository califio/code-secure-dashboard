using CodeSecure.Enum;
using CodeSecure.Manager.Integration.Mail;
using CodeSecure.Manager.Integration.Model;
using CodeSecure.Manager.Integration.Teams;
using CodeSecure.Manager.Project;
using CodeSecure.Manager.Setting;

namespace CodeSecure.Manager.Integration;

public class AlertManager(
    IProjectManager projectManager,
    MailAlertSetting mailAlertSetting,
    TeamsSetting teamsSetting,
    MailAlert mailAlert,
    TeamsAlert teamsAlert,
    ILogger<IAlert> logger
) : IAlertManager
{
    public async Task AlertScanCompletedInfo(ScanInfoModel model)
    {
        // GLOBAL
        // mail
        if (mailAlertSetting is { Active: true, ScanCompletedEvent: true })
        {
            await mailAlert.AlertScanCompletedInfo(model, mailAlertSetting.Receivers);
        }
        // teams
        if (teamsSetting is { Active: true, ScanCompletedEvent: true })
        {
            await teamsAlert.AlertScanCompletedInfo(model);
        }
        // PROJECT
        var receivers = (await projectManager.GetMembersAsync(model.ProjectId))
            .FindAll(member => member.Status == UserStatus.Active)
            .Select(member => member.Email).ToList();
        // mail
        var mailProjectSetting = await projectManager.GetMailSettingAsync(model.ProjectId);
        if (mailProjectSetting.ScanCompletedEvent)
        {
            mailAlert.AlertScanCompletedInfo(model, receivers);
        }
        // teams
        var teamsProjectSetting = await projectManager.GetTeamsSettingAsync(model.ProjectId);
        if (teamsProjectSetting is { Active: true, ScanCompletedEvent: true })
        {
            var projectTeamsAlert = new TeamsAlert(teamsProjectSetting, logger);
            projectTeamsAlert.AlertScanCompletedInfo(model, receivers);
        }
    }

    public async Task AlertNewFinding(NewFindingInfoModel model)
    {
        // GLOBAL
        // mail
        if (mailAlertSetting is { Active: true, NewFindingEvent: true })
        {
            await mailAlert.AlertNewFinding(model, mailAlertSetting.Receivers);
        }
        // teams
        if (teamsSetting is { Active: true, NewFindingEvent: true })
        {
            await teamsAlert.AlertNewFinding(model);
        }
        // PROJECT
        var receivers = (await projectManager.GetMembersAsync(model.ProjectId))
            .FindAll(member => member.Status == UserStatus.Active && member.Role != ProjectRole.Developer)
            .Select(member => member.Email).ToList();
        // mail
        var mailProjectSetting = await projectManager.GetMailSettingAsync(model.ProjectId);
        if (mailProjectSetting.NewFindingEvent)
        {
            mailAlert.AlertNewFinding(model, receivers);
        }
        
        // teams
        var teamsProjectSetting = await projectManager.GetTeamsSettingAsync(model.ProjectId);
        if (teamsProjectSetting is { Active: true, NewFindingEvent: true })
        {
            var projectTeamsAlert = new TeamsAlert(teamsProjectSetting, logger);
            projectTeamsAlert.AlertNewFinding(model, receivers);
        }
    }

    public async Task AlertFixedFinding(FixedFindingInfoModel model)
    {
        // GLOBAL
        // mail
        if (mailAlertSetting is { Active: true, FixedFindingEvent: true })
        {
            await mailAlert.AlertFixedFinding(model, mailAlertSetting.Receivers);
        }
        // teams
        if (teamsSetting is { Active: true, FixedFindingEvent: true })
        {
            await teamsAlert.AlertFixedFinding(model);
        }
        // PROJECT
        var receivers = (await projectManager.GetMembersAsync(model.ProjectId))
            .FindAll(member => member.Status == UserStatus.Active && member.Role != ProjectRole.Developer)
            .Select(member => member.Email).ToList();
        // mail
        var mailProjectSetting = await projectManager.GetMailSettingAsync(model.ProjectId);
        if (mailProjectSetting.FixedFindingEvent)
        {
            mailAlert.AlertFixedFinding(model, receivers);
        }
        
        // teams
        var teamsProjectSetting = await projectManager.GetTeamsSettingAsync(model.ProjectId);
        if (teamsProjectSetting is { Active: true, FixedFindingEvent: true })
        {
            var projectTeamsAlert = new TeamsAlert(teamsProjectSetting, logger);
            projectTeamsAlert.AlertFixedFinding(model, receivers);
        }
    }

    public async Task AlertNeedsTriageFinding(NeedsTriageFindingInfoModel model)
    {
        // GLOBAL
        // mail
        if (mailAlertSetting is { Active: true })
        {
            mailAlert.AlertNeedsTriageFinding(model, mailAlertSetting.Receivers);
        }
        // teams
        if (teamsSetting is { Active: true })
        {
            teamsAlert.AlertNeedsTriageFinding(model);
        }
        // PROJECT
        // mail
        var receivers = (await projectManager.GetMembersAsync(model.ProjectId))
            .FindAll(member => member.Status == UserStatus.Active && member.Role != ProjectRole.Developer)
            .Select(member => member.Email).ToList();
        mailAlert.AlertNeedsTriageFinding(model, receivers);
        // teams
        var teamsProjectSetting = await projectManager.GetTeamsSettingAsync(model.ProjectId);
        if (teamsProjectSetting.Active)
        {
            var projectTeamsAlert = new TeamsAlert(teamsProjectSetting, logger);
            projectTeamsAlert.AlertNeedsTriageFinding(model, receivers);
        }
    }

    public async Task AlertVulnerableDependencies(DependencyReportModel model, string? subject = null)
    {
        // GLOBAL
        // mail
        if (mailAlertSetting is { Active: true, SecurityAlertEvent: true })
        {
            await mailAlert.AlertVulnerableDependencies(model, subject, mailAlertSetting.Receivers);
        }
        // teams
        if (teamsSetting is { Active: true, SecurityAlertEvent: true })
        {
            await teamsAlert.AlertVulnerableDependencies(model, subject);
        }
        // PROJECT
        // mail
        var receivers = (await projectManager.GetMembersAsync(model.ProjectId))
            .FindAll(member => member.Status == UserStatus.Active)
            .Select(member => member.Email).ToList();
        var mailProjectSetting = await projectManager.GetMailSettingAsync(model.ProjectId);
        if (mailProjectSetting.SecurityAlertEvent)
        {
            mailAlert.AlertVulnerableDependencies(model, subject, receivers);
        }
        
        // teams
        var teamsProjectSetting = await projectManager.GetTeamsSettingAsync(model.ProjectId);
        if (teamsProjectSetting is { Active: true, SecurityAlertEvent: true })
        {
            var projectTeamsAlert = new TeamsAlert(teamsProjectSetting, logger);
            projectTeamsAlert.AlertVulnerableDependencies(model, subject, receivers);
        }
    }

    public async Task AlertProjectWithoutMember(AlertProjectWithoutMemberModel model)
    {
        // GLOBAL
        // mail
        if (mailAlertSetting is { Active: true })
        {
            await mailAlert.AlertProjectWithoutMember(model, mailAlertSetting.Receivers);
        }
        // teams
        if (teamsSetting is { Active: true })
        {
            await teamsAlert.AlertProjectWithoutMember(model);
        }
        // PROJECT
        // mail: without member
        // teams
        var teamsProjectSetting = await projectManager.GetTeamsSettingAsync(model.ProjectId);
        if (teamsProjectSetting is { Active: true })
        {
            var projectTeamsAlert = new TeamsAlert(teamsProjectSetting, logger);
            projectTeamsAlert.AlertProjectWithoutMember(model);
        }
    }
}