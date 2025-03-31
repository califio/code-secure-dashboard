using Microsoft.IdentityModel.Tokens;

namespace CodeSecure.Application;

public static class Configuration
{
    private static readonly AppConfig Config = AppConfig.Load();
    public const string AppName = "CodeSecure";
    public static string FrontendUrl => Config.FrontendUrl;

    public static string DbConnectionString =>
        $"Host={Config.DbServer};Database={Config.DbName};Username={Config.DbUsername};Password={Config.DbPassword}";

    public static string SystemPassword => Config.SystemPassword;
    public static SecurityKey AccessTokenKey => Config.AccessTokenSecurityKey;
    public static SecurityKey RefreshTokenKey => Config.RefreshTokenSecurityKey;
}