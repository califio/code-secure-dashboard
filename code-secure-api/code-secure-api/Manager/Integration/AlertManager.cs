using CodeSecure.Manager.Integration.Mail;
using CodeSecure.Manager.Integration.Model;
using CodeSecure.Manager.Integration.Teams;
using CodeSecure.Manager.Setting;

namespace CodeSecure.Manager.Integration;

public class AlertManager : IAlert
{
    private readonly IList<IAlert> notifications = new List<IAlert>();

    public AlertManager(
        MailSetting mailSetting,
        MailAlertSetting mailAlertSetting,
        TeamsSetting teamsSetting,
        ILogger<IAlert> logger)
    {
        // Mail
        if (mailAlertSetting.Active)
        {
            notifications.Add(new MailAlert(mailSetting, mailAlertSetting, logger));
        }
        // MS Teams
        if (teamsSetting.Active)
        {
            try
            {
                notifications.Add(new TeamsAlert(teamsSetting));
            }
            catch (System.Exception e)
            {
                logger?.LogError(e.Message);
            }
        }
    }

    public Task<NotificationResult> TestAlert(string email)
    {
        throw new NotImplementedException();
    }

    public async Task AlertScanCompletedInfo(IEnumerable<string> receivers, ScanInfoModel model)
    {
        foreach (var notification in notifications)
        {
            await notification.AlertScanCompletedInfo(receivers, model);
        }
    }

    public async Task AlertNewFinding(IEnumerable<string> receivers, NewFindingInfoModel model)
    {
        foreach (var notification in notifications)
        {
            await notification.AlertNewFinding(receivers, model);
        }
    }

    public async Task AlertFixedFinding(IEnumerable<string> receivers, FixedFindingInfoModel model)
    {
        foreach (var notification in notifications)
        {
            await notification.AlertFixedFinding(receivers, model);
        }
    }

    public async Task AlertNeedsTriageFinding(IEnumerable<string> receivers, NeedsTriageFindingInfoModel model)
    {
        foreach (var notification in notifications)
        {
            await notification.AlertNeedsTriageFinding(receivers, model);
        }
    }

    public async Task PushDependencyReport(IEnumerable<string> receivers, DependencyReportModel model,
        string? subject = null)
    {
        foreach (var notification in notifications)
        {
            await notification.PushDependencyReport(receivers, model, subject);
        }
    }

    public async Task AlertProjectWithoutMember(IEnumerable<string> receivers, AlertProjectWithoutMemberModel model)
    {
        foreach (var notification in notifications)
        {
            await notification.AlertProjectWithoutMember(receivers, model);
        }
    }
}