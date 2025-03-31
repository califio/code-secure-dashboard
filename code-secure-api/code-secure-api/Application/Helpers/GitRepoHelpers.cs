using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Helpers;

public static class GitRepoHelpers
{
    public static string UrlByCommit(SourceType sourceType, string repoUrl, string commitSha, string path, int? startLine = null, int? endLine = null)
    {
        if (sourceType == SourceType.GitLab)
        {
            string url = $"{repoUrl}/-/blob/{commitSha}/{path}";
            if (startLine is > 0)
            {
                url += $"#L{startLine}";
                if (endLine is > 0)
                {
                    url += $"-{endLine}";
                }
            }
            return url;
        }
        // todo: other source type
        return $"{repoUrl}/{commitSha}/{path}";
    }
    
    public static string BuildCommitUrl(SourceType sourceType, string repoUrl, string commitSha)
    {
        if (sourceType == SourceType.GitLab) return $"{repoUrl}/-/commit/{commitSha}";
        // todo: add other source
        return repoUrl;
    }
    
    public static string BuildMergeRequestUrl(SourceType sourceType, string repoUrl, string mergeRequestId)
    {
        if (sourceType == SourceType.GitLab) return $"{repoUrl}/-/merge_requests/{mergeRequestId}";
        // todo: add other source
        return string.Empty;
    }
    
}