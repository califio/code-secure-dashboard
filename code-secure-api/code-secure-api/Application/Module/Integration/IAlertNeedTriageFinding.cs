using CodeSecure.Application.Helpers;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;

namespace CodeSecure.Application.Module.Integration;

public record AlertNeedTriageFindingModel
{
    public required Projects Project { get; set; }
    public required int NeedTriageCount { get; set; }

    public string FindingUrl()
    {
        return $"{FrontendUrlHelper.ProjectFindingUrl(Project.Id)}?status={FindingStatus.Open.ToString()}";
    }
}
public interface IAlertNeedTriageFinding
{
    Task<Result<bool>> AlertAsync(List<string> receivers, AlertNeedTriageFindingModel model);
}