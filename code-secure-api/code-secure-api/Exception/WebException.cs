namespace CodeSecure.Exception;

public abstract class WebException(string message, int status) : System.Exception(message)
{
    public int Status { get; init; } = status;
}