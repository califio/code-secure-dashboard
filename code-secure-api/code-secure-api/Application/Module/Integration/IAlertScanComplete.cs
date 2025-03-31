using CodeSecure.Application.Helpers;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;

namespace CodeSecure.Application.Module.Integration;

public record AlertScanCompleteModel
{
    public required SourceType SourceType { get; init; }
    public required Projects Project { get; init; }
    public required Scanners Scanner { get; init; }
    public required GitCommits GitCommit { get; init; }
    public required int NewFindingCount { get; set; }
    public required int ConfirmedFindingCount { get; set; }
    public required int FixedFindingCount { get; set; }
    public required bool IsBlock { get; set; }
    
    public string CommitUrl()
    {
        return GitRepoHelpers.BuildCommitUrl(SourceType, Project.RepoUrl, GitCommit.CommitHash ?? string.Empty);
    }
    
    public string MergeRequestUrl()
    {
        if (string.IsNullOrEmpty(GitCommit.MergeRequestId))
        {
            return string.Empty;
        }

        return GitRepoHelpers.BuildMergeRequestUrl(SourceType, Project.RepoUrl, GitCommit.MergeRequestId);
    }
    
    public string FindingUrl()
    {
        return $"{FrontendUrlHelper.ProjectFindingUrl(Project.Id)}?commitId={GitCommit.Id}";
    }
    
    public string ProjectUrl()
    {
        return FrontendUrlHelper.ProjectUrl(Project.Id);
    }
}
public interface IAlertScanComplete
{
    Task<Result<bool>> AlertAsync(List<string> receivers, AlertScanCompleteModel model);

}