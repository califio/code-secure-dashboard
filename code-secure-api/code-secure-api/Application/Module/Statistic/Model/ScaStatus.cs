using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.Statistic.Model;

public record ScaStatus
{
    [Required] public required int Open { get; set; }
    [Required] public required int Ignore { get; set; }

    [Required] public required int Fixed { get; set; }
}