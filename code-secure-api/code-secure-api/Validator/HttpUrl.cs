using System.ComponentModel.DataAnnotations;
using CodeSecure.Extension;

namespace CodeSecure.Validator;

public class HttpUrlAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is string url)
        {
            return url.IsHttpUrl();
        }
        return false;
    }
}