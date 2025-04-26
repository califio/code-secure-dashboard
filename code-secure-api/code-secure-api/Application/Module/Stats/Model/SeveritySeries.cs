using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.Stats.Model;

public record SeveritySeries
{
    [Required] public required int Critical { get; set; }

    [Required] public required int High { get; set; }

    [Required] public required int Medium { get; set; }

    [Required] public required int Low { get; set; }
}