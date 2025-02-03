using System.ComponentModel.DataAnnotations;
using CodeSecure.Manager.Setting;

namespace CodeSecure.Api.Setting.Model;

public record SlaSetting
{
    [Required]
    public required SLA Sast { get; set; }
    [Required]
    public required SLA Sca { get; set; }
}