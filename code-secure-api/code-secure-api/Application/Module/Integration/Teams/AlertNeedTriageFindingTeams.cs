using CodeSecure.Application.Module.Integration.Teams.Client;
using CodeSecure.Application.Module.Integration.Teams.Client.Action;
using FluentResults;

namespace CodeSecure.Application.Module.Integration.Teams;

public class AlertNeedTriageFindingTeams(string webhook) : IAlertNeedTriageFinding
{
    public async Task<Result<bool>> AlertAsync(List<string> receivers, AlertNeedTriageFindingModel model)
    {
        try
        {
            var title = $"Reminder: Please verify unconfirmed finding on \"{model.Project.Name}\" project";
            var text =
                $"This is a reminder that {model.NeedTriageCount} unconfirmed security findings have been detected in **{model.Project.Name}** project and require your verification.\n\n" +
                $"Please verify and resolve these findings as soon as possible to maintain the security and integrity of the project.\n\n";
            if (receivers.Count != 0)
            {
                text += $"**Assignee:** {string.Join(", ", receivers)}";
            }

            var message = new MessageCard(title)
            {
                Text = text,
                Actions =
                [
                    new OpenUriAction("View Detail", model.FindingUrl()),
                ]
            };
            await new TeamsClient(webhook).PostAsync(message);
            return true;
        }
        catch (Exception e)
        {
            return Result.Fail<bool>(e.Message);
        }
    }
}