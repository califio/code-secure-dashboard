using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Manager.EnvVariable.Model;

public record EnvironmentVariable
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Value { get; set; }
}