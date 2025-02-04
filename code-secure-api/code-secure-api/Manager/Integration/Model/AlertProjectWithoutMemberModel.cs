namespace CodeSecure.Manager.Integration.Model;

public class AlertProjectWithoutMemberModel
{
    public required Guid ProjectId { get; set; }
    public required string ProjectName { get; set; }
    public required string ProjectUrl { get; set; }
}