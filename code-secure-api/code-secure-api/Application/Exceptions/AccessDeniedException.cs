namespace CodeSecure.Application.Exceptions;

public class AccessDeniedException : WebException
{
    public AccessDeniedException(params string[] message) : base(403, message)
    {
    }
    public AccessDeniedException(IEnumerable<string> errors) : base(403, errors)
    {
    }

    public AccessDeniedException() : base(403, ["Access Denied"])
    {
    }
}