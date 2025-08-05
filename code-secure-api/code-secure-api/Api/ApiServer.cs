using System.Net;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using CodeSecure.Application;
using CodeSecure.Application.Schedulers;
using CodeSecure.Authentication;
using CodeSecure.Core;
using CodeSecure.Middleware;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Scalar.AspNetCore;
using Serilog;

namespace CodeSecure.Api;

public static class ApiServer
{
    public static void Run(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Load app configuration to get trusted proxies
        var appConfig = AppConfig.Load();
        
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedFor;
            
            // Parse trusted proxies from configuration
            ParseTrustedProxies(appConfig.TrustedProxies, options);
        });
        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));
        // App DB Context
        builder.Services.AddDbContext();
        // Memory Cache
        builder.Services.AddMemoryCache();
        // App Module
        builder.Services.AddAppModules();
        // Authentication
        builder.Services.AddAppAuthentication();
        // Controller
        builder.Services.AddControllers(options =>
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
        builder.Services.AddScheduleJobs();
        // Handler model state
        builder.Services.ConfigureInvalidModelState();
        builder.Services.AddHttpContextAccessor();
        // add init data service
        builder.Services.AddScoped<InitDataService>();
        //cors 
        builder.Services.AddCors();
        // swagger
        builder.Services.AddSwaggers();
        // config upload 
        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 26843545; // 25MB
        });
        builder.Services.AddSpaStaticFiles(configuration => { configuration.RootPath = "wwwroot"; });
        builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
        var app = builder.Build();
        app.MapHealthChecks("/healthz");
        app.InitApp();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(options => { options.RouteTemplate = "/openapi/{documentName}.json"; });

            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "API V1"); });
            app.MapScalarApiReference();
        }

        app.ConfigureExceptionHandler();
        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials()
        );
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseDefaultFiles();
        app.UseSpaStaticFiles();
        app.UseRouting();
        app.UseForwardedHeaders();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.LoadAuthenticationProviders();
        app.Run();
    }

    private static void ParseTrustedProxies(string trustedProxies, ForwardedHeadersOptions options)
    {
        if (string.IsNullOrWhiteSpace(trustedProxies))
            return;

        var proxies = trustedProxies.Split(',', StringSplitOptions.RemoveEmptyEntries);
        
        foreach (var proxy in proxies)
        {
            var trimmedProxy = proxy.Trim();
            
            // Check if it's a CIDR block
            if (trimmedProxy.Contains('/'))
            {
                if (TryParseIPNetwork(trimmedProxy, out var network, out var prefixLength))
                {
                    options.KnownNetworks.Add(new Microsoft.AspNetCore.HttpOverrides.IPNetwork(network, prefixLength));
                }
            }
            // Single IP address
            else if (IPAddress.TryParse(trimmedProxy, out var ipAddress))
            {
                options.KnownProxies.Add(ipAddress);
            }
        }
    }

    private static bool TryParseIPNetwork(string cidr, out IPAddress network, out int prefixLength)
    {
        network = IPAddress.None;
        prefixLength = 0;

        var parts = cidr.Split('/');
        if (parts.Length != 2)
            return false;

        if (!IPAddress.TryParse(parts[0], out network))
            return false;

        if (!int.TryParse(parts[1], out prefixLength))
            return false;

        // Validate prefix length based on address family
        var maxPrefixLength = network.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork ? 32 : 128;
        if (prefixLength < 0 || prefixLength > maxPrefixLength)
            return false;

        return true;
    }
}

internal sealed partial class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    private readonly Regex myRegex = MyRegex();

    public string? TransformOutbound(object? value)
    {
        if (value == null) return null;

        var str = value.ToString();
        return string.IsNullOrEmpty(str) ? null : myRegex.Replace(str, "$1-$2").ToLower();
    }

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex MyRegex();
}