using CodeSecure.Application.Helpers;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Integration;

public record AlertStatusFindingModel
{
    public required SourceType SourceType { get; init; }
    public required Projects Project { get; set; }
    public required Scanners Scanner { get; set; }
    public required GitCommits GitCommit { get; set; }
    public required List<Findings> Findings { get; set; }

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

    public string ProjectUrl()
    {
        return FrontendUrlHelper.ProjectUrl(Project.Id);
    }

    public string FindingUrl(FindingStatus? status = null)
    {
        var findingUrl =
            $"{FrontendUrlHelper.ProjectFindingUrl(Project.Id)}?commitId={GitCommit.Id}&scanner={Scanner.Name}&type={Scanner.Type.ToString()}";
        if (status != null)
        {
            findingUrl = $"{findingUrl}&status={status.ToString()}";
        }

        return findingUrl;
    }
}