using CodeSecure.Application.Module.Integration.Teams.Client;
using CodeSecure.Application.Module.Integration.Teams.Client.Action;
using FluentResults;

namespace CodeSecure.Application.Module.Integration.Teams;

public class AlertVulnerableProjectPackageTeams(string webhook) : IAlertVulnerableProjectPackage
{
    public async Task<Result<bool>> AlertAsync(List<string> receivers, AlertVulnerableProjectPackageModel model)
    {
        try
        {
            var subject = $"Security Alert: Vulnerability found in dependencies of \"{model.Project.Name}\" project";
            var text = "| **Package** | **Location** | **Fix Version** |\n|-------------|---------------------|";
            foreach (var package in model.ProjectPackages)
            {
                text += $"\n| {package.Package?.Name} | {package.Location} | {package.Package?.FixedVersion} |";
            }

            var message = new MessageCard(subject)
            {
                Text = text,
                Actions =
                [
                    new OpenUriAction("View Detail", model.ProjectUrl()),
                    new OpenUriAction("Open Repo", model.RepoUrl())
                ]
            };
            await new TeamsClient(webhook).PostAsync(message);
            return true;
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }
}