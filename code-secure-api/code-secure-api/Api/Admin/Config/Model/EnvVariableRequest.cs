using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Api.Admin.Config.Model;

public class EnvVariableRequest
{
    [Required]
    public required string Name { get; set; }
}