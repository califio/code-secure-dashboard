using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Database.Entity;

public class EnvironmentName
{
    [Key] public required string Name { get; set; }
}