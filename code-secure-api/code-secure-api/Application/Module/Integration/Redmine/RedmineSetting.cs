namespace CodeSecure.Application.Module.Integration.Redmine;

public record RedmineSetting
{
    public bool Active { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public int StatusId { get; set; }
    public int TrackerId { get; set; }
    public int PriorityId { get; set; }
}