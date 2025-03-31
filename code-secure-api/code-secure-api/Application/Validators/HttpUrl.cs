using System.ComponentModel.DataAnnotations;
using CodeSecure.Core.Extension;

namespace CodeSecure.Application.Validators;

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