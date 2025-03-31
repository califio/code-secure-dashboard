using CodeSecure.Application.Helpers;
using CodeSecure.Core.Entity;
using FluentResults;

namespace CodeSecure.Application.Module.Integration;

public record AlertVulnerableProjectPackageModel
{
    public required Projects Project { get; set; }
    public required List<ProjectPackages> ProjectPackages { get; set; }

    public string ProjectUrl()
    {
        return FrontendUrlHelper.ProjectUrl(Project.Id);
    }

    public string RepoUrl()
    {
        return Project.RepoUrl;
    }
}

public interface IAlertVulnerableProjectPackage
{
    Task<Result<bool>> AlertAsync(List<string> receivers, AlertVulnerableProjectPackageModel model);
}