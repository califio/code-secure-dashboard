namespace CodeSecure.Manager.Integration.Model;

public class NeedsTriageFindingInfoModel
{
    public required string ProjectName { get; set; }
    public required int NeedsTriage { get; set; }
    public required string OpenFindingUrl { get; set; }
}