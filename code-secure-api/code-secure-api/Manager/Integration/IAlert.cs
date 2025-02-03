using CodeSecure.Manager.Integration.Model;

namespace CodeSecure.Manager.Integration;

public interface IAlert
{
    public Task<NotificationResult> TestAlert(string receiver);
    public Task AlertScanCompletedInfo(IEnumerable<string> receivers, ScanInfoModel model);
    public Task AlertNewFinding(IEnumerable<string> receivers, NewFindingInfoModel model);
    public Task AlertFixedFinding(IEnumerable<string> receivers, FixedFindingInfoModel model);
    public Task AlertNeedsTriageFinding(IEnumerable<string> receivers, NeedsTriageFindingInfoModel model);
    public Task PushDependencyReport(IEnumerable<string> receivers, DependencyReportModel model, string? subject);
    public Task AlertProjectWithoutMember(IEnumerable<string> receivers, AlertProjectWithoutMemberModel model);
}