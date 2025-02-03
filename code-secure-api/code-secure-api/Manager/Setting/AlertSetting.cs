namespace CodeSecure.Manager.Setting;

public record AlertSetting
{
    public bool Active { get; set; }
    public bool SecurityAlertEvent { get; set; }
    public bool NewFindingEvent { get; set; }
    public bool FixedFindingEvent { get; set; }
    public bool ScanCompletedEvent { get; set; }
    public bool ScanFailedEvent { get; set; }
    
}