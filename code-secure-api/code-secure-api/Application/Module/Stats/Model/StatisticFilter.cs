namespace CodeSecure.Application.Module.Stats.Model;

public class StatisticFilter
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? SourceId { get; set; }
}