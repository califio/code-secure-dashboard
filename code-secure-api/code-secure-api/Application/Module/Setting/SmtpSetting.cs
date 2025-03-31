using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.Setting;

public record SmtpSetting
{
    [Required]
    public string Server { get; set; } = string.Empty;
    [Required]
    public int Port { get; set; }
    public string Name { get; set; } = string.Empty;
    [Required]
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool UseSsl { get; set; }
    public bool IgnoreSsl { get; set; }
}