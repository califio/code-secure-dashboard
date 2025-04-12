using CodeSecure.Application.Helpers;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;

namespace CodeSecure.Application.Module.Integration;

public record AlertConfirmedFindingModel
{
    public required Projects Project { get; set; }
    public required List<Findings> Findings { get; set; }

    public string ProjectUrl()
    {
        return FrontendUrlHelper.ProjectUrl(Project.Id);
    }

    public string FindingUrl()
    {
        var findingUrl = $"{FrontendUrlHelper.ProjectFindingUrl(Project.Id)}?status={FindingStatus.Open.ToString()}";

        return findingUrl;
    }
}

public interface IAlertConfirmedFinding
{
    Task<Result<bool>> AlertAsync(List<string> receivers, AlertConfirmedFindingModel model);
}