using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;

namespace CodeSecure.Api;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggers(this IServiceCollection builder)
    {
        builder.AddEndpointsApiExplorer();
        builder.AddSwaggerGen(config =>
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
                    []
                }
            });
            config.DocInclusionPredicate((docName, apiDesc) => true);
            config.TagActionsBy(api =>
            {
                if (api.GroupName != null)
                {
                    return [api.GroupName];
                }
                if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                {
                    return [controllerActionDescriptor.ControllerName];
                }
                throw new InvalidOperationException("Unable to determine tag for endpoint.");
            });
        });
        return builder;
    }
}