namespace CodeSecure.Application.Module.Finding;

public record FindingMetadata
{
    public List<FindingLocation>? FindingFlow { get; set; }
    public List<string>? References { get; set; }
    public List<string>? Cwes { get; set; }
    public string? Cvss { get; set; }
    public string? CvssScore { get; set; }
}

public record FindingLocation
{
    public required string Path { get; set; }
    public string? Snippet { get; set; }
    public int? StartLine { get; set; }
    public int? EndLine { get; set; }
    public int? StartColumn { get; set; }
    public int? EndColumn { get; set; }
}