using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodeSecure.Extension;

public static class JSONSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static string Serialize(object? obj)
    {
        return JsonSerializer.Serialize(obj, Options);
    }

    public static TValue? Deserialize<TValue>(string? json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return default;
        }

        try
        {
            return JsonSerializer.Deserialize<TValue>(json, Options);
        }
        catch (System.Exception)
        {
            return default;
        }
    }
    public static TValue DeserializeOrDefault<TValue>(string? json, TValue defaultValue)
    {
        if (string.IsNullOrEmpty(json))
        {
            return defaultValue;
        }
        return Deserialize<TValue>(json) ?? defaultValue;
    }
    
}