using System.ComponentModel.DataAnnotations;
using CodeSecure.Database.Metadata;

namespace CodeSecure.Api.Admin.Config.Model;

public record SlaSetting
{
    [Required]
    public required SLA Sast { get; set; }
    [Required]
    public required SLA Sca { get; set; }
}