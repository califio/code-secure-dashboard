namespace CodeSecure.Api.Auth.Model;

public class ConfirmEmailResult
{
    public static readonly ConfirmEmailResult Success = new() { Succeeded = true };
    public required bool Succeeded { get; set; }
    public string Error { get; set; } = string.Empty;

    public static ConfirmEmailResult Failed(string error)
    {
        return new ConfirmEmailResult
        {
            Succeeded = false,
            Error = error
        };
    }
}