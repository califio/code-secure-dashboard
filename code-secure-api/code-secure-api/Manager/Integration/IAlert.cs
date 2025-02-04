using CodeSecure.Manager.Integration.Model;

namespace CodeSecure.Manager.Integration;

public interface IAlert
{
    public Task<NotificationResult> TestAlert(string receiver);
    public Task AlertScanCompletedInfo(ScanInfoModel model, List<string>? receivers = null);
    public Task AlertNewFinding(NewFindingInfoModel model, List<string>? receivers = null);
    public Task AlertFixedFinding(FixedFindingInfoModel model, List<string>? receivers = null);
    public Task AlertNeedsTriageFinding(NeedsTriageFindingInfoModel model, List<string>? receivers = null);
    public Task AlertVulnerableDependencies(DependencyReportModel model, string? subject, List<string>? receivers = null);
    public Task AlertProjectWithoutMember(AlertProjectWithoutMemberModel model, List<string>? receivers = null);
}