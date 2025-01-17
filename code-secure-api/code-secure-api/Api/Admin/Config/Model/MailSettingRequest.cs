using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Api.Admin.Config.Model;

public class MailSettingRequest
{
    [Required] 
    public required string Server { get; set; }
    [Required] 
    public int Port { get; set; }
    
    [Required]
    [EmailAddress] 
    public required string UserName { get; set; }
    public string Password { get; set; } = string.Empty;
    [Required] 
    public bool UseSsl { get; set; }
}