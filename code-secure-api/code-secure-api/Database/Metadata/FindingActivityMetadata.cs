using CodeSecure.Enum;

namespace CodeSecure.Database.Metadata;

public record FindingActivityMetadata
{
    public FindingScanActivity? ScanInfo { get; set; }
    public ChangeStatusFinding? ChangeStatus { get; set; }
    public ChangeDeadlineFinding? ChangeDeadline { get; set; }
}

public record FindingScanActivity
{
    public required string Branch { get; set; }
    public required GitAction Action { get; set; }
    public string? TargetBranch { get; set; }
    public required bool IsDefault { get; set; }
}

public record ChangeStatusFinding
{
    public required FindingStatus PreviousStatus { get; set; }
    public required FindingStatus CurrentStatus { get; set; }
}

public record ChangeDeadlineFinding
{
    public DateTime? PreviousDate { get; set; }
    public DateTime? CurrentDate { get; set; }
}