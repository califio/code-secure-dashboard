using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Database;

public static class AppDbContextExtensions
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString), ServiceLifetime.Scoped);
        var provider = services.BuildServiceProvider();
        var appDbContext = provider.GetRequiredService<AppDbContext>();
        appDbContext.Database.Migrate();
        return services;
    }
}