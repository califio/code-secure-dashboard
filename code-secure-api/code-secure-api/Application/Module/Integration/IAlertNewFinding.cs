using FluentResults;

namespace CodeSecure.Application.Module.Integration;

public interface IAlertNewFinding
{
    Task<Result<bool>> AlertAsync(List<string> receivers, AlertStatusFindingModel model);
}