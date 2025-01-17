using CodeSecure.Manager.Notification.Model;
using CodeSecure.Manager.Setting;

namespace CodeSecure.Manager.Notification;

public class NotificationManager : INotification
{
    private readonly IList<INotification> notifications = new List<INotification>();

    public NotificationManager(IAppSettingManager settingManager, ILogger<INotification> logger)
    {
        notifications.Add(new MailNotification(settingManager.GetMailSettingAsync().Result, logger));
        // MS Teams
        var teams = settingManager.GetTeamsNotificationSettingAsync().Result;
        if (teams.Active)
        {
            try
            {
                notifications.Add(new TeamsNotification(teams));
            }
            catch (System.Exception e)
            {
                logger?.LogError(e.Message);
            }
        }
    }

    public Task<NotificationResult> PushTestNotification(string email)
    {
        throw new NotImplementedException();
    }

    public async Task PushScanInfo(IEnumerable<string> receivers, ScanInfoModel model)
    {
        foreach (var notification in notifications)
        {
            await notification.PushScanInfo(receivers, model);
        }
    }

    public async Task PushNewFindingInfo(IEnumerable<string> receivers, NewFindingInfoModel model)
    {
        foreach (var notification in notifications)
        {
            await notification.PushNewFindingInfo(receivers, model);
        }
    }

    public async Task PushFixedFindingInfo(IEnumerable<string> receivers, FixedFindingInfoModel model)
    {
        foreach (var notification in notifications)
        {
            await notification.PushFixedFindingInfo(receivers, model);
        }
    }

    public async Task PushNeedsTriageFindingInfo(IEnumerable<string> receivers, NeedsTriageFindingInfoModel model)
    {
        foreach (var notification in notifications)
        {
            await notification.PushNeedsTriageFindingInfo(receivers, model);
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

    public async Task PushAlertProjectWithoutMember(IEnumerable<string> receivers, AlertProjectWithoutMemberModel model)
    {
        foreach (var notification in notifications)
        {
            await notification.PushAlertProjectWithoutMember(receivers, model);
        }
    }
}