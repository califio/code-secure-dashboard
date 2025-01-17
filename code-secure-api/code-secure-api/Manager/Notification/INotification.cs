using CodeSecure.Manager.Notification.Model;

namespace CodeSecure.Manager.Notification;

public interface INotification
{
    public Task<NotificationResult> PushTestNotification(string receiver);
    public Task PushScanInfo(IEnumerable<string> receivers, ScanInfoModel model);
    public Task PushNewFindingInfo(IEnumerable<string> receivers, NewFindingInfoModel model);
    public Task PushFixedFindingInfo(IEnumerable<string> receivers, FixedFindingInfoModel model);
    public Task PushNeedsTriageFindingInfo(IEnumerable<string> receivers, NeedsTriageFindingInfoModel model);
    public Task PushDependencyReport(IEnumerable<string> receivers, DependencyReportModel model, string? subject);
    public Task PushAlertProjectWithoutMember(IEnumerable<string> receivers, AlertProjectWithoutMemberModel model);
}