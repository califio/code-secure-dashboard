using CodeSecure.Enum;

namespace CodeSecure.Extension;

public static class RepoHelpers
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
    
    public static string GetCommitUrl(SourceType sourceType, string repoUrl, string commitSha)
    {
        if (sourceType == SourceType.GitLab) return $"{repoUrl}/-/commit/{commitSha}";
        // todo: add other source
        return repoUrl;
    }
    
}