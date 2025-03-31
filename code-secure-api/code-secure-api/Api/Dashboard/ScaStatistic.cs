using System.ComponentModel.DataAnnotations;
using CodeSecure.Application.Module.Statistic;
using CodeSecure.Application.Module.Statistic.Model;

namespace CodeSecure.Api.Dashboard;

public record ScaStatistic
{
    [Required] public required SeveritySeries Severity { get; set; }

    [Required] public required ScaStatus Status { get; set; }

    [Required] public required List<TopDependency> TopDependencies { get; set; }
}