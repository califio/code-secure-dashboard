using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using CodeSecure.Authentication;
using CodeSecure.Database;
using CodeSecure.Middleware;
using CodeSecure.Scheduler;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OpenApi.Models;

namespace CodeSecure;

public static class Application
{
    public static readonly ApplicationConfig Config = ApplicationConfig.Load();

    public static string GetConnectionString()
    {
        return
            $"Host={Config.DbServer};Database={Config.DbName};Username={Config.DbUsername};Password={Config.DbPassword}";
    }

    public static void Run(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
        });
        builder.Logging.AddConsole();
        // Add db context
        builder.Services.AddAppDbContext(GetConnectionString());
        // memory cache
        builder.Services.AddMemoryCache();
        // Add Managers
        builder.Services.AddAppModules();
        // Authentication
        builder.Services.AddAppAuthentication();
        // Add controllers
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
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(config =>
        {
            //config.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            config.CustomOperationIds(e => e.ActionDescriptor.RouteValues["action"]);
            config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            config.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
        // config upload 
        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 26843545; // 25MB
        });
        builder.Services.AddSpaStaticFiles(configuration => { configuration.RootPath = "wwwroot"; });
        var app = builder.Build();
        app.InitApp();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
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