namespace CodeSecure.Application.Module.Project.Integration.Redmine;

public record RedmineProjectSetting
{
    public bool Active { get; set; }
    public int ProjectId { get; set; }
    public int StatusId { get; set; }
    public int TrackerId { get; set; }
    public int PriorityId { get; set; }
}