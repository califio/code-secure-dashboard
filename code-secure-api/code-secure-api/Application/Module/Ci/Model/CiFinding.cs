using System.ComponentModel.DataAnnotations;
using CodeSecure.Application.Module.Finding;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Utils;

namespace CodeSecure.Application.Module.Ci.Model;

public record CiFinding
{
    public Guid? Id { get; set; }
    public string? RuleId { get; set; }
    public DateTime? FixDeadline { get; set; }

    [Required] public required string Identity { get; init; }

    [Required] public required string Name { get; set; }

    [Required] public required string Description { get; set; }

    public string? Category { get; set; }
    public string? Recommendation { get; set; }

    [Required] public required FindingSeverity Severity { get; set; }

    public FindingLocation? Location { get; set; }
    public FindingMetadata? Metadata { get; set; }

    public static CiFinding FromFinding(Findings finding)
    {
        FindingMetadata? metadata = null;
        if (!string.IsNullOrEmpty(finding.Metadata))
            try
            {
                metadata = JSONSerializer.Deserialize<FindingMetadata>(finding.Metadata);
            }
            catch (Exception)
            {
                metadata = null;
            }

        return new CiFinding
        {
            Id = finding.Id,
            RuleId = finding.RuleId,
            Identity = finding.Identity,
            Name = finding.Name,
            Description = finding.Description,
            Recommendation = finding.Recommendation,
            Severity = finding.Severity,
            FixDeadline = finding.FixDeadline,
            Location = finding.Location == null
                ? null
                : new FindingLocation
                {
                    Path = finding.Location,
                    Snippet = finding.Snippet,
                    StartLine = finding.StartLine,
                    EndLine = finding.EndLine,
                    StartColumn = finding.StartColumn,
                    EndColumn = finding.EndColumn
                },
            Metadata = metadata
        };
    }

    public override int GetHashCode()
    {
        return Identity.GetHashCode();
    }
    public virtual bool Equals(CiFinding? other)
    {
        return Identity.Equals(other?.Identity);
    }
}