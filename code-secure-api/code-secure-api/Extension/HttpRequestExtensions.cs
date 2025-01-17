namespace CodeSecure.Extension;

public static class HttpRequestExtensions
{
    public static string FrontendUrl(this IHttpContextAccessor context)
    {
        if (context.HttpContext != null)
        {
            return FrontendUrl(context.HttpContext.Request);
        }
        return string.Empty;
    }
    public static string FrontendUrl(this HttpRequest? request)
    {
        if (request == null)
        {
            return string.Empty;
        }
        var scheme = request.Scheme;
        var host = request.Host.Host;
        var port = request.Host.Port;
        if (request.Headers.TryGetValue("X-Forwarded-Port", out var value))
        {
            if (int.TryParse(value, out var portInt))
            {
                port = portInt;
            }
        }
        if (port != null && port != 80 && port != 443)
        {
            return $"{scheme}://{host}:{port}";
        }
        return $"{scheme}://{host}";
    }
}