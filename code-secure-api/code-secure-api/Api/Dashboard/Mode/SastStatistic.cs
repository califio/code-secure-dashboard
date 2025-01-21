using System.ComponentModel.DataAnnotations;
using CodeSecure.Manager.Statistic.Model;

namespace CodeSecure.Api.Dashboard.Mode;

public record SastStatistic
{
    [Required] public required SeveritySeries Severity { get; set; }

    [Required] public required StatusSeries Status { get; set; }

    [Required] public required List<TopFinding> TopFindings { get; set; }
}