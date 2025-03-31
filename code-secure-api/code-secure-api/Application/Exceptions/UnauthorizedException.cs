namespace CodeSecure.Application.Exceptions;

public class UnauthorizedException : WebException
{
    public UnauthorizedException(params string[] errors) : base(401, errors)
    {
    }
    
    public UnauthorizedException(IEnumerable<string> errors) : base(404, errors)
    {
    }

    public UnauthorizedException() : base(401, ["Unauthorized"])
    {
    }
}