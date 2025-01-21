namespace CodeSecure.Api.Finding.Model;

public record FindingBranch
{
    public required Guid ScanId { get; set; }
    public required string Branch { get; set; }
    public required string CommitHash { get; set; }
    public required bool IsDefault { get; set; }
}