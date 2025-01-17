namespace CodeSecure.Exception;

public class UnauthorizedException : WebException
{
    public UnauthorizedException(string message) : base(message, 401)
    {
    }

    public UnauthorizedException() : base("Unauthorized", 401)
    {
    }
}