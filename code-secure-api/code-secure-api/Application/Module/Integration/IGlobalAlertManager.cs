using CodeSecure.Application.Module.Integration.Mail;
using CodeSecure.Application.Module.Integration.Teams;
using CodeSecure.Application.Services;
using CodeSecure.Core.Extension;

namespace CodeSecure.Application.Module.Integration;

public interface IGlobalAlertManager
{
    public Task AlertNewFinding(AlertStatusFindingModel model);
    public Task AlertFixedFinding(AlertStatusFindingModel model);
    public Task AlertNeedTriageFinding(AlertNeedTriageFindingModel model);
    public Task AlertVulnerableProjectPackage(AlertVulnerableProjectPackageModel model);
    public Task AlertProjectWithoutMember(AlertProjectWithoutMemberModel model);
}

public class GlobalAlertManager(
    TeamsAlertSetting teamsAlertSetting,
    MailAlertSetting mailAlertSetting,
    // mail
    ISmtpService smtpService,
    IRazorRender render,
    ILogger<GlobalAlertManager> logger
) : IGlobalAlertManager
{
    public async Task AlertNewFinding(AlertStatusFindingModel model)
    {
        model.Findings.Sort((first, two) => two.Severity - first.Severity);
        // mail
        if (mailAlertSetting is { Active: true, NewFindingEvent: true, Receivers.Count: > 0 })
        {
            var result = await new AlertNewFindingMail(smtpService, render)
                .AlertAsync(mailAlertSetting.Receivers, model);
            if (result.IsFailed)
            {
                logger.LogError(result.ListErrors().First());
            }
        }

        // teams
        if (teamsAlertSetting is { Active: true, NewFindingEvent: true })
        {
            var result = await new AlertNewFindingTeams(teamsAlertSetting.Webhook)
                .AlertAsync([], model);
            if (result.IsFailed)
            {
                logger.LogError(result.ListErrors().First());
            }
        }
    }

    public async Task AlertFixedFinding(AlertStatusFindingModel model)
    {
        // mail
        if (mailAlertSetting is { Active: true, FixedFindingEvent: true, Receivers.Count: > 0 })
        {
            var result = await new AlertFixedFindingMail(render, smtpService)
                .AlertAsync(mailAlertSetting.Receivers, model);
            if (result.IsFailed)
            {
                logger.LogError(result.ListErrors().First());
            }
        }

        // teams
        if (teamsAlertSetting is { Active: true, FixedFindingEvent: true })
        {
            var result = await new AlertNewFindingTeams(teamsAlertSetting.Webhook)
                .AlertAsync([], model);
            if (result.IsFailed)
            {
                logger.LogError(result.ListErrors().First());
            }
        }
    }

    public async Task AlertNeedTriageFinding(AlertNeedTriageFindingModel model)
    {
        // mail
        if (mailAlertSetting is { Active: true, NeedTriageFindingEvent: true, Receivers.Count: > 0 })
        {
            var result = await new AlertNeedTriageFindingMail(smtpService, render)
                .AlertAsync(mailAlertSetting.Receivers, model);
            if (result.IsFailed)
            {
                logger.LogError(result.ListErrors().First());
            }
        }

        // teams
        if (teamsAlertSetting is { Active: true, NeedTriageFindingEvent: true })
        {
            var result = await new AlertNeedTriageFindingTeams(teamsAlertSetting.Webhook)
                .AlertAsync([], model);
            if (result.IsFailed)
            {
                logger.LogError(result.ListErrors().First());
            }
        }
    }

    public async Task AlertVulnerableProjectPackage(AlertVulnerableProjectPackageModel model)
    {
        // mail
        if (mailAlertSetting is { Active: true, SecurityAlertEvent: true, Receivers.Count: > 0 })
        {
            var result = await new AlertVulnerableProjectPackageMail(smtpService, render)
                .AlertAsync(mailAlertSetting.Receivers, model);
            if (result.IsFailed)
            {
                logger.LogError(result.ListErrors().First());
            }
        }

        // teams
        if (teamsAlertSetting is { Active: true, SecurityAlertEvent: true })
        {
            var result = await new AlertVulnerableProjectPackageTeams(teamsAlertSetting.Webhook)
                .AlertAsync([], model);
            if (result.IsFailed)
            {
                logger.LogError(result.ListErrors().First());
            }
        }
    }

    public async Task AlertProjectWithoutMember(AlertProjectWithoutMemberModel model)
    {
        // mail
        if (mailAlertSetting is { Active: true, ProjectWithoutMemberEvent: true, Receivers.Count: > 0 })
        {
            var result = await new AlertProjectWithoutMemberMail(render, smtpService)
                .AlertAsync(mailAlertSetting.Receivers, model);
            if (result.IsFailed)
            {
                logger.LogError(result.ListErrors().First());
            }
        }

        // teams
        if (teamsAlertSetting is { Active: true, ProjectWithoutMemberEvent: true })
        {
            var result = await new AlertProjectWithoutMemberTeams(teamsAlertSetting.Webhook)
                .AlertAsync([], model);
            if (result.IsFailed)
            {
                logger.LogError(result.ListErrors().First());
            }
        }
    }
}