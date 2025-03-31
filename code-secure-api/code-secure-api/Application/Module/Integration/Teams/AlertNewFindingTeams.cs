using CodeSecure.Application.Module.Integration.Teams.Client;
using CodeSecure.Application.Module.Integration.Teams.Client.Action;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Extension;
using FluentResults;

namespace CodeSecure.Application.Module.Integration.Teams;

public class AlertNewFindingTeams(string webhook) : IAlertNewFinding
{
    public async Task<Result<bool>> AlertAsync(List<string> receivers, AlertStatusFindingModel model)
    {
        try
        {
            var title =
                $"Security Alert: Found new finding on \"{model.Project.Name}\" project by {model.Scanner.Name} - {model.Scanner.Type}";
            var text =
                $"We are notifying you that the latest {model.Scanner.Name} scan has detected {model.Findings.Count()} new security findings.<br>";
            text += $"**Commit:** {model.GitCommit.CommitHash}<br>";
            if (model.GitCommit.Type == CommitType.CommitBranch)
            {
                text += $"**Branch:** {model.GitCommit.Branch}<br>";
            }
            else if (model.GitCommit.Type == CommitType.CommitTag)
            {
                text += $"**Tag:** {model.GitCommit.Branch}<br>";
            }
            else if (model.GitCommit.Type == CommitType.MergeRequest)
            {
                text +=
                    $"**Merge Request:** **{model.GitCommit.Branch}** to **{model.GitCommit.TargetBranch}**<br><br>";
            }

            text += "**List Finding**<br>";
            text += "<table><thead><tr><td>**ID**</td><td>**NAME**</td><td>**SEVERITY**</td></tr></thread><tbody>";
            for (int i = 0; i < model.Findings.Count; i++)
            {
                var findingUrl = $"{Configuration.FrontendUrl}/#/finding/{model.Findings[i].Id}";
                text +=
                    $"<tr><td style='width: 30px;'>{i + 1}</td><td>[{model.Findings[i].Name}]({findingUrl})</td><td style='width: 100px'>{model.Findings[i].Severity.ToString().ToUpper()}</td></tr>";
            }

            text += "</tbody></table><br>";
            text +=
                "Please verify and resolve the finding as soon as possible to maintain the security and integrity of the project.<br>";
            if (receivers.Count != 0)
            {
                text += $"**Assignee:** {string.Join(", ", receivers)}";
            }

            var message = new MessageCard(title)
            {
                Text = text
            };
            // action

            message.AddAction(new OpenUriAction("View Detail", model.FindingUrl(FindingStatus.Open)));
            message.AddAction(new OpenUriAction("View Commit", model.CommitUrl()));
            if (model.GitCommit.Type == CommitType.MergeRequest && model.MergeRequestUrl().IsHttpUrl())
            {
                message.AddAction(new OpenUriAction("View Merge Request", model.MergeRequestUrl()));
            }

            await new TeamsClient(webhook).PostAsync(message);
            return true;
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }
}