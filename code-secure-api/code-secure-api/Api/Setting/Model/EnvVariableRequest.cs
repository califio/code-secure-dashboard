using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Api.Setting.Model;

public class EnvVariableRequest
{
    [Required]
    public required string Name { get; set; }
}