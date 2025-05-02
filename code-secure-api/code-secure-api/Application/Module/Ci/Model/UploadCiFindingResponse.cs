namespace CodeSecure.Application.Module.Ci.Model;

public record UploadCiFindingResponse
{
    public required string FindingUrl { get; set; }
    public required IEnumerable<CiFinding> NewFindings { get; set; }
    public required IEnumerable<CiFinding> ConfirmedFindings { get; set; }
    public required IEnumerable<CiFinding> OpenFindings { get; set; }
    public required IEnumerable<CiFinding> FixedFindings { get; set; }
    public required bool IsBlock { get; set; }
}