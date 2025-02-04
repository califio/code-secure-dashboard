using CodeSecure.Enum;

namespace CodeSecure.Extension;

public static class RepoHelpers
{
    public static string UrlByCommit(SourceType sourceType, string repoUrl, string commit, string path, int? startLine = null, int? endLine = null)
    {
        if (sourceType == SourceType.GitLab)
        {
            string url = $"{repoUrl}/-/blob/{commit}/{path}";
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
        return $"{repoUrl}/{commit}/{path}";
    }
}