namespace CodeSecure.Application.Exceptions;

public class NotFoundException : WebException
{
    public NotFoundException(params string[] errors) : base(404, errors)
    {
    }
    public NotFoundException(IEnumerable<string> errors) : base(404, errors)
    {
    }

    public NotFoundException() : base(404, ["Not Found"])
    {
    }
}