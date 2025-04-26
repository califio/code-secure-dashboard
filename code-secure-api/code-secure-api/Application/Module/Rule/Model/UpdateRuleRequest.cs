using System.ComponentModel.DataAnnotations;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Rule.Model;

public class UpdateRuleRequest
{
    [Required]
    public required string RuleId { get; set; }
    [Required]
    public required Guid ScannerId { get; set; }
    public RuleStatus? Status { get; set; }
    public RuleConfidence? Confidence { get; set; }
}