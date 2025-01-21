namespace CodeSecure.Exception;

public class NotFoundException : WebException
{
    public NotFoundException(string message) : base(message, 404)
    {
    }

    public NotFoundException() : base("Not Found", 404)
    {
    }
}