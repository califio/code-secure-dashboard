namespace CodeSecure.Manager.Integration.Model;

public record RemoveProjectMemberModel
{
    public required string Username { get; set; }
    public required string ProjectName { get; set; }
    public required string ProjectUrl { get; set; }
}