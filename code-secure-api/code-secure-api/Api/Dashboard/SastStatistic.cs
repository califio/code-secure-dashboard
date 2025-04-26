using System.ComponentModel.DataAnnotations;
using CodeSecure.Application.Module.Stats.Model;

namespace CodeSecure.Api.Dashboard;

public record SastStatistic
{
    [Required] public required SeveritySeries Severity { get; set; }

    [Required] public required SastStatus Status { get; set; }

    [Required] public required List<TopFinding> TopFindings { get; set; }
}