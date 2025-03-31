using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Rule;

public record RuleFilter: QueryFilter
{
    public string? Name { get; set; }
    public Guid? ProjectId { get; set; }
    public List<Guid>? ScannerId { get; set; }
    public List<RuleStatus>? Status { get; set; }
    public List<RuleConfidence>? Confidence { get; set; }
    
}