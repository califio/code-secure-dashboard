using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Middleware;

public static class InvalidModelStateHandler
{
    public static IServiceCollection ConfigureInvalidModelState(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(option =>
        {
            option.InvalidModelStateResponseFactory = context =>
            {
                var errors = (from stateValue in context.ModelState.Values
                    from error in stateValue.Errors
                    select error.ErrorMessage).ToList();
                var message = new Error(400, errors);
                return new BadRequestObjectResult(message);
            };
        });
        return services;
    }
}