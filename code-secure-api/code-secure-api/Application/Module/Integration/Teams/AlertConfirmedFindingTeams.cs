using CodeSecure.Application.Module.Integration.Teams.Client;
using CodeSecure.Application.Module.Integration.Teams.Client.Action;
using FluentResults;

namespace CodeSecure.Application.Module.Integration.Teams;

public class AlertConfirmedFindingTeams(string webhook) : IAlertConfirmedFinding
{
    public async Task<Result<bool>> AlertAsync(List<string> receivers, AlertConfirmedFindingModel model)
    {
        try
        {
            var title = $"Security Alert: \"{model.Project.Name}\" project has {model.Findings.Count} issues to fix";
            var text =
                $"We are notifying you that \"{model.Project.Name}\" project has {model.Findings.Count} issues to fix.<br>";
            text += "**List Finding**<br>";
            text += "<table><thead><tr><td>**ID**</td><td>**NAME**</td><td>**SEVERITY**</td></tr></thread><tbody>";
            for (int i = 0; i < model.Findings.Count; i++)
            {
                var findingUrl = $"{Configuration.FrontendUrl}/#/finding/{model.Findings[i].Id}";
                text +=
                    $"<tr><td style='width: 30px;'>{i + 1}</td><td>[{model.Findings[i].Name}]({findingUrl})</td><td style='width: 100px'>{model.Findings[i].Severity.ToString().ToUpper()}</td></tr>";
            }

            text += "</tbody></table><br>";
            var message = new MessageCard(title)
            {
                Text = text
            };
            // action
            message.AddAction(new OpenUriAction("View Detail", model.FindingUrl()));
            await new TeamsClient(webhook).PostAsync(message);
            return true;
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }
}