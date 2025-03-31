namespace CodeSecure.Application.Exceptions;

public abstract class WebException(int status, IEnumerable<string> errors) : System.Exception
{
    public int Status { get; init; } = status;
    public IEnumerable<string> Errors { get; init; } = errors;
}