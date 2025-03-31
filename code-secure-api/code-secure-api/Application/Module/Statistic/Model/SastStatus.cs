using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.Statistic.Model;

public record SastStatus
{
    [Required] public required int Open { get; set; }

    [Required] public required int Confirmed { get; set; }

    [Required] public required int AcceptedRisk { get; set; }

    [Required] public required int Fixed { get; set; }
}