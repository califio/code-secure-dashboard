using CodeSecure.Application.Helpers;
using CodeSecure.Core.Entity;
using FluentResults;

namespace CodeSecure.Application.Module.Integration;

public record AlertProjectWithoutMemberModel
{
    public required Projects Project { get; set; }

    public string ProjectUrl()
    {
        return $"{FrontendUrlHelper.ProjectFindingUrl(Project.Id)}";
    }
}

public interface IAlertProjectWithoutMember
{
    Task<Result<bool>> AlertAsync(List<string> receivers, AlertProjectWithoutMemberModel model);

}