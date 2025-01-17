namespace CodeSecure.Api.CI.Model;

public record CiUploadFindingResponse
{
    public required string FindingUrl { get; set; }
    public required IEnumerable<CiFinding> NewFindings { get; set; }
    public required IEnumerable<CiFinding> ConfirmedFindings { get; set; }
    public required IEnumerable<CiFinding> NeedsTriageFindings { get; set; }
    public required IEnumerable<CiFinding> FixedFindings { get; set; }
    public required bool IsBlock { get; set; }
}