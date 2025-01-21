namespace CodeSecure.Exception;

public class AccessDeniedException : WebException
{
    public AccessDeniedException(string message) : base(message, 403)
    {
    }

    public AccessDeniedException() : base("Access Denied", 403)
    {
    }
}