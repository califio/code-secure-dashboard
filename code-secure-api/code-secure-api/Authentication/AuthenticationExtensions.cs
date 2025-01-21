using Aguacongas.AspNetCore.Authentication;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Database;
using CodeSecure.Database.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CodeSecure.Authentication;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAppAuthentication(this IServiceCollection services)
    {
        // Add Identity
        services.AddIdentity<Users, Roles>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddUserManager<JwtUserManager>();
        
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = Application.Config.AccessTokenSecurityKey
            };
        });
        var authBuilder = services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });
        // dynamic schema
        var dynamicBuilder = authBuilder
            .AddDynamic()
            .AddEntityFrameworkStore<AppDbContext>();
        dynamicBuilder.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, _ => { });
        return services;
    }

    public static DynamicAuthenticationBuilder AddDynamic(
        this AuthenticationBuilder builder)
    {
        DynamicAuthenticationBuilder dynamicBuilder =
            new DynamicAuthenticationBuilder(builder.Services, typeof(AuthProviders));
        builder.Services.AddSingleton<OptionsMonitorCacheWrapperFactory>()
            .AddTransient(provider =>
                new AuthProviderManager(provider.GetRequiredService<IAuthenticationSchemeProvider>(),
                    provider.GetRequiredService<OptionsMonitorCacheWrapperFactory>(),
                    provider.GetRequiredService<IDynamicProviderStore<AuthProviders>>(), dynamicBuilder.HandlerTypes))
            .AddTransient(
                (Func<IServiceProvider, NoPersistentDynamicManager<AuthProviders>>)(provider =>
                    new NoPersistentDynamicManager<AuthProviders>(
                        provider.GetRequiredService<IAuthenticationSchemeProvider>(),
                        provider.GetRequiredService<OptionsMonitorCacheWrapperFactory>(),
                        dynamicBuilder.HandlerTypes)));
        return dynamicBuilder;
    }

    public static IApplicationBuilder LoadAuthenticationProviders(this IApplicationBuilder builder)
    {
        builder.ApplicationServices.LoadAuthenticationProviders();
        return builder;
    }

    public static IServiceProvider LoadAuthenticationProviders(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<AuthProviderManager>().Load();
        return provider;
    }
}