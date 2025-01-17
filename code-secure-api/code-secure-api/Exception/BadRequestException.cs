namespace CodeSecure.Exception;

public class BadRequestException : WebException
{
    public BadRequestException(string message) : base(message, 400)
    {
    }

    public BadRequestException() : base("Bad Request", 400)
    {
    }
}