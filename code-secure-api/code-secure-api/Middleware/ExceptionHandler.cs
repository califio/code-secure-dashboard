using CodeSecure.Exception;
using Microsoft.AspNetCore.Diagnostics;

namespace CodeSecure.Middleware;

internal record Error(int Status, List<string> Errors);

public static class ExceptionHandler
{
    public static IApplicationBuilder ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(c => c.Run(async context =>
        {
            var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
            if (exception != null)
            {
                Error message;
                if (exception is WebException webException)
                {
                    context.Response.StatusCode = webException.Status;
                    message = new Error(webException.Status, [webException.Message]);
                }
                else
                {
                    context.Response.StatusCode = 500;
                    message = exception switch
                    {
                        NotImplementedException => new Error(500, ["Not Implemented"]),
                        _ => new Error(500, [exception.Message])
                    };
                }

                await context.Response.WriteAsJsonAsync(message);
            }
        }));
        return app;
    }
}