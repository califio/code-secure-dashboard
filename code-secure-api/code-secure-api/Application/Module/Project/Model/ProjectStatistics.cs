using System.ComponentModel.DataAnnotations;
using CodeSecure.Application.Module.Statistic;
using CodeSecure.Application.Module.Statistic.Model;

namespace CodeSecure.Application.Module.Project.Model;

public record ProjectStatistics
{
    [Required] public required ScaStatus StatusSca { get; set; }

    [Required] public required SastStatus StatusSast { get; set; }

    [Required] public required SeveritySeries SeveritySast { get; set; }

    [Required] public required SeveritySeries SeveritySca { get; set; }

    public required int OpenFinding { get; set; }
}