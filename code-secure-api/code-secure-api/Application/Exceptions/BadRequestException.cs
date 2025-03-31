namespace CodeSecure.Application.Exceptions;

public class BadRequestException : WebException
{
    public BadRequestException(params string[] errors) : base(400, errors)
    {
    }
    public BadRequestException(IEnumerable<string> errors) : base(400, errors)
    {
    }
    public BadRequestException() : base(400, ["Bad Request"])
    {
    }
}