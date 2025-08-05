using System.Text;
using System.Text.Json.Serialization;
using CodeSecure.Authentication;
using CodeSecure.Core.Utils;
using Microsoft.IdentityModel.Tokens;

namespace CodeSecure.Application;

public class AppConfig
{
    // database
    [Option(Env = "DB_USERNAME", Default = "codesecure")]
    public string DbUsername { get; set; } = string.Empty;

    [Option(Env = "DB_PASSWORD", Default = "codesecure")]
    public string DbPassword { get; set; } = string.Empty;

    [Option(Env = "DB_NAME", Default = "codesecure")]
    public string DbName { get; set; } = string.Empty;

    [Option(Env = "DB_SERVER", Default = "localhost")]

    public string DbServer { get; set; } = string.Empty;

    // app config
    [Option(Env = "SYSTEM_PASSWORD", Default = "")]
    public string SystemPassword { get; set; } = string.Empty;

    [Option(Env = "ACCESS_TOKEN_KEY")] public string AccessTokenKey { get; set; } = string.Empty;

    [Option(Env = "REFRESH_TOKEN_KEY")] public string RefreshTokenKey { get; set; } = string.Empty;

    [Option(Env = "FRONTEND_URL", Default = "")]
    public string FrontendUrl { get; set; } = string.Empty;
    
    [Option(Env = "TRUSTED_PROXIES", Default = "")]
    public string TrustedProxies { get; set; } = string.Empty;

    [JsonIgnore] internal SecurityKey AccessTokenSecurityKey = null!;
    [JsonIgnore] internal SecurityKey RefreshTokenSecurityKey = null!;

    public static AppConfig Load()
    {
        var config = ConfigParser.Parse<AppConfig>();
        if (string.IsNullOrEmpty(config.AccessTokenKey))
        {
            config.AccessTokenKey = PasswordGenerator.GeneratePassword(32);
        }

        if (string.IsNullOrEmpty(config.RefreshTokenKey))
        {
            config.RefreshTokenKey = PasswordGenerator.GeneratePassword(32);
        }

        if (string.IsNullOrEmpty(config.SystemPassword))
        {
            config.SystemPassword = PasswordGenerator.GeneratePassword(32);
        }

        ConfigParser.Save(config);
        config.AccessTokenSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.AccessTokenKey));
        config.RefreshTokenSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.RefreshTokenKey));
        return config;
    }
}