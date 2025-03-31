using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application;

public static class AppDbContextExtension
{
    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(Configuration.DbConnectionString));
        var provider = services.BuildServiceProvider();
        var appDbContext = provider.GetRequiredService<AppDbContext>();
        appDbContext.Database.Migrate();
        return services;
    }
}