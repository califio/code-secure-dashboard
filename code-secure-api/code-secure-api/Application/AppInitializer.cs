namespace CodeSecure.Application;

public static class AppInitializer
{
    public static IApplicationBuilder InitApp(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<InitDataService>();
        context.InitDataAsync(app.Environment.IsDevelopment()).Wait();
        return app;
    }
}