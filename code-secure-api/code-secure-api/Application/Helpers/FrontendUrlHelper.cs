namespace CodeSecure.Application.Helpers;

public static class FrontendUrlHelper
{
    public static string ProjectUrl(Guid projectId)
    {
        return $"{Configuration.FrontendUrl}/#/project/{projectId}";
    }

    public static string ProjectFindingUrl(Guid projectId)
    {
        return $"{Configuration.FrontendUrl}/#/project/{projectId}/finding";
    }

    public static string FindingUrl(Guid findingId)
    {
        return $"{Configuration.FrontendUrl}/#/finding/{findingId}";
    }
}