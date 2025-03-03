namespace CodeSecure.Manager.Rule;

public record RuleResult
{
    public static readonly RuleResult Success = new()
    {
        Succeeded = true
    };
    public bool Succeeded { get; set; }
    public IEnumerable<string> Errors { get; set; } = [];
    
    
    public static RuleResult Failed(params string[] errors)
    {
        var identityResult = new RuleResult
        {
            Succeeded = false,
            Errors = errors
        };
        return identityResult;
    }
    
}