using FluentResults;

namespace CodeSecure.Application;

public interface IHandler<in TInput, TOutput>
{
    Task<Result<TOutput>> HandleAsync(TInput request);
}

public interface IOutputHandler<TOutput>
{
    Task<Result<TOutput>> HandleAsync();
}