using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Manager.Statistic.Model;

public record TopDependency
{
    public string? Group { get; set; }

    [Required] public required string Name { get; set; }

    [Required] public required string Version { get; set; }

    [Required] public required int Critical { get; set; }

    [Required] public required int High { get; set; }

    [Required] public required int Medium { get; set; }

    [Required] public required int Low { get; set; }

    [Required] public required int Info { get; set; }
}