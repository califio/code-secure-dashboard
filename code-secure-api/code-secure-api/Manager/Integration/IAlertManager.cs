using CodeSecure.Manager.Integration.Model;

namespace CodeSecure.Manager.Integration;

public interface IAlertManager
{
    public Task AlertScanCompletedInfo(ScanInfoModel model);
    public Task AlertNewFinding(NewFindingInfoModel model);
    public Task AlertFixedFinding(FixedFindingInfoModel model);
    public Task AlertNeedsTriageFinding(NeedsTriageFindingInfoModel model);
    public Task AlertVulnerableDependencies(DependencyReportModel model, string? subject = null);
    public Task AlertProjectWithoutMember(AlertProjectWithoutMemberModel model);
}