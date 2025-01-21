namespace CodeSecure.Config;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class OptionAttribute : Attribute
{
    public string Env { get; set; } = string.Empty;
    public object? Default { get; set; }
    
    public OptionAttribute()
    {
    }
    
    public OptionAttribute(object @default)
    {
        Default = @default;
    }
}