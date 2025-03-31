using CodeSecure.Application.Exceptions;
using FluentResults;

namespace CodeSecure.Core.Extension;

public static class ResultExtension
{
    public static IEnumerable<string> ListErrors<T>(this Result<T> result)
    {
        return result.Errors.Select(error => error.Message);
    }
    
    public static T GetResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return result.Value;
        }

        throw new BadRequestException(result.Errors.Select(error => error.Message));
    }
    
}