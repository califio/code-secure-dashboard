using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.Stats.Model;

public record TopFinding
{
    [Required] public required string Category { get; set; }

    [Required] public required int Count { get; set; }
}