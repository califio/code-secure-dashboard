using System.Reflection;
using CodeSecure.Extension;

namespace CodeSecure.Config;

public static class ConfigParser
{
    private const string DefaultConfigFile = "codesecure-config.json";
    private static readonly string DefaultConfigFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    
    public static T Parse<T>(string? configFile = DefaultConfigFile) where T : new()
    {
        var config = new T();
        if (configFile != null)
        {
            config = Load<T>(configFile) ?? new T();
        }
        var properties = config.GetType().GetProperties();
        foreach (var property in properties)
        {
            var optionAttr = property.GetCustomAttribute<OptionAttribute>();
            if (optionAttr != null)
            {
                var envValue = Environment.GetEnvironmentVariable(optionAttr.Env);
                if (!string.IsNullOrEmpty(envValue))
                {
                    property.SetValue(config, Convert.ChangeType(envValue, property.PropertyType));
                    continue;
                }
                if (optionAttr.Default == null) continue;
                var propertyValue = property.GetValue(config);
                if (propertyValue is null or "" or 0)
                {
                    property.SetValue(config, Convert.ChangeType(optionAttr.Default, property.PropertyType));
                }
            }
        }
        return config;
    }

    public static T? Load<T>(string configFile = DefaultConfigFile)
    {
        var configFilePath = Path.Combine(DefaultConfigFolder, configFile);
        return File.Exists(configFilePath) ? JSONSerializer.Deserialize<T>(File.ReadAllText(configFilePath)) : default;
    }

    public static void Save<T>(T config, string configFile = DefaultConfigFile)
    {
        var configFilePath = Path.Combine(DefaultConfigFolder, configFile);
        File.WriteAllText(configFilePath, JSONSerializer.Serialize(config));
    }
}