using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.Integration;

public record AlertSetting
{
    [Required]
    public bool Active { get; set; }
    [Required]
    public bool SecurityAlertEvent { get; set; }
    [Required]
    public bool NewFindingEvent { get; set; }
    [Required]
    public bool FixedFindingEvent { get; set; }
    [Required]
    public bool NeedTriageFindingEvent { get; set; }
    [Required]
    public bool ScanCompletedEvent { get; set; }
    [Required]
    public bool ScanFailedEvent { get; set; }
    [Required]
    public bool ProjectWithoutMemberEvent { get; set; }
}