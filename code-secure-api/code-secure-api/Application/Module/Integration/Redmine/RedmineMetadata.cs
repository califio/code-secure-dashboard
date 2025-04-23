namespace CodeSecure.Application.Module.Integration.Redmine;

public record RedmineMetadata
{
    public required List<IdName> Projects { get; set; }
    public required List<IdName> Trackers { get; set; }
    public required List<IdName> Priorities { get; set; }
    public required List<IdName> Statuses { get; set; }
}