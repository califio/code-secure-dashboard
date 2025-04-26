namespace CodeSecure.Application.Module.Rule.Model;

public record CreateRuleRequest
{
    public required string Id { get; init; }
    public required Guid ScannerId { get; init; }
}