namespace CodeSecure.Manager.Integration.TicketTracker;

public class TicketResult<T>
{
    public required bool Succeeded { get; set; }
    public string Error { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static TicketResult<T> Failed(string error)
    {
        return new TicketResult<T>
        {
            Succeeded = false,
            Error = error
        };
    }

    public static TicketResult<T> Success(T key)
    {
        return new TicketResult<T> { Succeeded = true, Data = key};
    }
}