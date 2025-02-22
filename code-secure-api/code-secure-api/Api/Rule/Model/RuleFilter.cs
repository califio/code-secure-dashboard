namespace CodeSecure.Api.Rule.Model;

public record RuleFilter
{
    public string? Name { get; set; }
    public Guid? ProjectId { get; set; }
}