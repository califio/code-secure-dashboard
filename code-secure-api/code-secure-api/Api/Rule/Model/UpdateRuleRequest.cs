using System.ComponentModel.DataAnnotations;
using CodeSecure.Enum;

namespace CodeSecure.Api.Rule.Model;

public class UpdateRuleRequest
{
    [Required]
    public required string RuleId { get; set; }
    [Required]
    public required Guid ScannerId { get; set; }
    public RuleStatus? Status { get; set; }
    public RuleConfidence? Confidence { get; set; }
}