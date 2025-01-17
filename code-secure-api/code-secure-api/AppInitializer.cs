using CodeSecure.Database;
using CodeSecure.Manager.Setting;

namespace CodeSecure;

public static class AppInitializer
{
    public static IApplicationBuilder InitApp(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var settingManager = serviceScope.ServiceProvider.GetRequiredService<IAppSettingManager>();
        Application.Setting = settingManager.AppSettingAsync().Result;
        var context = serviceScope.ServiceProvider.GetRequiredService<InitDataService>();
        context.InitDataAsync(app.Environment.IsDevelopment()).Wait();
        return app;
    }
}