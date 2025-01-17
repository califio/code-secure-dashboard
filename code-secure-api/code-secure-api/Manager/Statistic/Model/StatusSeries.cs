using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Manager.Statistic.Model;

public record StatusSeries
{
    [Required] public required int Open { get; set; }

    [Required] public required int Confirmed { get; set; }

    [Required] public required int AcceptedRisk { get; set; }

    [Required] public required int Fixed { get; set; }
}