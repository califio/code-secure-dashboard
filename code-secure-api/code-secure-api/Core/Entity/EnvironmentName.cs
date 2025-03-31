using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Core.Entity;

public class EnvironmentName
{
    [Key] public required string Name { get; set; }
}