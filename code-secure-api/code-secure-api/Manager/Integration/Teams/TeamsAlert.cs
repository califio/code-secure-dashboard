using CodeSecure.Enum;
using CodeSecure.Extension;
using CodeSecure.Manager.Integration.Model;
using CodeSecure.Manager.Integration.Teams.Client;
using CodeSecure.Manager.Integration.Teams.Client.Action;
using CodeSecure.Manager.Setting;

namespace CodeSecure.Manager.Integration.Teams;

public class TeamsAlert(TeamsSetting setting, ILogger<IAlert>? logger = null) : IAlert
{
    private readonly TeamsClient teamsClient = new TeamsClient(setting.Webhook);

    public async Task<NotificationResult> TestAlert(string receiver)
    {
        var text = "This is test message";
        if (!string.IsNullOrEmpty(receiver))
        {
            text += $" by {receiver}";
        }

        var message = new MessageCard("Test Notification")
        {
            Text = text
        };
        try
        {
            var response = await teamsClient.PostAsync(message);
            if (response.IsSuccessStatusCode)
            {
                return NotificationResult.Success;
            }

            return NotificationResult.Failed($"Error response status {response.StatusCode}");
        }
        catch (System.Exception e)
        {
            return NotificationResult.Failed(e.Message);
        }
    }

    public async Task AlertScanCompletedInfo(ScanInfoModel model, List<string>? receivers = null)
    {
        if (!setting.ScanCompletedEvent)
        {
            return;
        }

        logger?.LogInformation($"send mail scan result {model.ScanName} on {model.ProjectName} to ms teams channel");
        var title = $"Scan on \"{model.ProjectName}\" by {model.ScannerName} completed";
        var text = $"**Commit:** [{model.ScanName}]({model.CommitUrl})\n\n";
        if (model.Action == GitAction.CommitBranch)
        {
            text += $"**Branch:** {model.CommitBranch}\n\n";
        }
        else if (model.Action == GitAction.CommitTag)
        {
            text += $"\n\n**Tag:** {model.CommitBranch}\n\n";
        }
        else if (model.Action == GitAction.MergeRequest)
        {
            text +=
                $"\n\n**Merge Request:** **[{model.CommitBranch}]({model.MergeRequestUrl})** to **{model.TargetBranch}**\n\n\n";
        }

        text += "| **New** | **Needs Confirm** | **Pending Fix** |\n";
        text += "|-------------|-------------|-------------|\n";
        text += $"| {model.NewFinding} | {model.NeedsTriage} | {model.Confirmed} |\n";
        var message = new MessageCard(title)
        {
            Text = text,
            Actions =
            [
                new OpenUriAction("View Detail", model.FindingUrl),
            ]
        };
        await PushMessage(message);
    }

    public async Task AlertNewFinding(NewFindingInfoModel model, List<string>? receivers = null)
    {
        if (!setting.NewFindingEvent || model.Findings.Count == 0)
        {
            return;
        }

        model.Findings.Sort((first, two) => two.Severity - first.Severity);
        var title =
            $"Security Alert: Found new finding on \"{model.ProjectName}\" project by {model.ScannerName} - {model.ScannerType}";
        var text =
            $"We are notifying you that the latest {model.ScannerName} scan has detected {model.Findings.Count()} new security findings.<br>";
        text += $"**Commit:** {model.ScanName}<br>";
        if (model.Action == GitAction.CommitBranch)
        {
            text += $"**Branch:** {model.CommitBranch}<br>";
        }
        else if (model.Action == GitAction.CommitTag)
        {
            text += $"**Tag:** {model.CommitBranch}<br>";
        }
        else if (model.Action == GitAction.MergeRequest)
        {
            text += $"**Merge Request:** **{model.CommitBranch}** to **{model.TargetBranch}**<br><br>";
        }

        text += "**List Finding**<br>";
        text += "<table><thead><tr><td>**ID**</td><td>**NAME**</td><td>**SEVERITY**</td></tr></thread><tbody>";
        for (int i = 0; i < model.Findings.Count; i++)
        {
            text +=
                $"<tr><td style='width: 30px;'>{i + 1}</td><td>[{model.Findings[i].Name}]({model.Findings[i].Url})</td><td style='width: 100px'>{model.Findings[i].Severity.ToString().ToUpper()}</td></tr>";
        }

        text += "</tbody></table><br>";
        text +=
            "Please verify and resolve the finding as soon as possible to maintain the security and integrity of the project.<br>";
        if (receivers != null && receivers.Any())
        {
            text += $"**Assignee:** {string.Join(", ", receivers)}";
        }

        var message = new MessageCard(title)
        {
            Text = text
        };
        // action
        message.AddAction(new OpenUriAction("View Detail", model.OpenFindingUrl));
        message.AddAction(new OpenUriAction("View Commit", model.CommitUrl));
        if (model.Action == GitAction.MergeRequest && model.MergeRequestUrl.IsHttpUrl())
        {
            message.AddAction(new OpenUriAction("View Merge Request", model.MergeRequestUrl!));
        }

        await PushMessage(message);
    }

    public async Task AlertFixedFinding(FixedFindingInfoModel model, List<string>? receivers = null)
    {
        if (!setting.FixedFindingEvent || model.Findings.Count == 0)
        {
            return;
        }

        model.Findings.Sort((first, two) => two.Severity - first.Severity);
        var title = $"Notification: Some findings have been fixed on \"{model.ProjectName}\" project";
        var text =
            $"We are pleased to inform you that some previously reported findings have been fixed in **{model.ProjectName}** project.\n\n";
        text += $"**Commit:** {model.ScanName}<br>";
        if (model.Action == GitAction.CommitBranch)
        {
            text += $"**Branch:** {model.CommitBranch}<br>";
        }
        else if (model.Action == GitAction.CommitTag)
        {
            text += $"**Tag:** {model.CommitBranch}<br>";
        }
        else if (model.Action == GitAction.MergeRequest)
        {
            text += $"**Merge Request:** **{model.CommitBranch}** to **{model.TargetBranch}**<br><br>";
        }

        text += "**Fixed Findings**<br>";
        text += "<table><thead><tr><td>**ID**</td><td>**NAME**</td><td>**SEVERITY**</td></tr></thread><tbody>";
        for (int i = 0; i < model.Findings.Count; i++)
        {
            text +=
                $"<tr><td style='width: 30px;'>{i + 1}</td><td>[{model.Findings[i].Name}]({model.Findings[i].Url})</td><td style='width: 100px'>{model.Findings[i].Severity.ToString().ToUpper()}</td></tr>";
        }

        text += "</tbody></table><br>";
        text += $"Please verify the patch to ensure they align with the expected behavior.<br>";
        if (receivers != null && receivers.Count != 0)
        {
            text += $"**Assignee:** {string.Join(", ", receivers)}";
        }

        var message = new MessageCard(title)
        {
            Text = text,
        };
        // action
        message.AddAction(new OpenUriAction("View Detail", model.FixedFindingUrl));
        message.AddAction(new OpenUriAction("View Commit", model.CommitUrl));
        if (model.Action == GitAction.MergeRequest && model.MergeRequestUrl.IsHttpUrl())
        {
            message.AddAction(new OpenUriAction("View Merge Request", model.MergeRequestUrl!));
        }

        await PushMessage(message);
    }

    public async Task AlertNeedsTriageFinding(NeedsTriageFindingInfoModel model, List<string>? receivers = null)
    {
        if (!setting.SecurityAlertEvent)
        {
            return;
        }

        var title = $"Reminder: Please verify unconfirmed finding on \"{model.ProjectName}\" project";
        var text =
            $"This is a reminder that {model.NeedsTriage} unconfirmed security findings have been detected in **{model.ProjectName}** project and require your verification.\n\n" +
            $"Please verify and resolve these findings as soon as possible to maintain the security and integrity of the project.\n\n";
        if (receivers != null && receivers.Count != 0)
        {
            text += $"**Assignee:** {string.Join(", ", receivers)}";
        }

        var message = new MessageCard(title)
        {
            Text = text,
            Actions =
            [
                new OpenUriAction("View Detail", model.OpenFindingUrl),
            ]
        };
        await PushMessage(message);
    }

    public async Task AlertVulnerableDependencies(DependencyReportModel model, string? subject = null,
        List<string>? receivers = null)
    {
        if (!setting.SecurityAlertEvent || model.Packages.Count == 0)
        {
            return;
        }

        logger?.LogInformation($"send dependency report of repo {model.RepoName} to ms teams channel");
        subject ??= $"Security Alert: Vulnerability found in dependencies of \"{model.RepoName}\" project";
        var text = "| **Package** | **Recommendation** |\n|-------------|---------------------|";
        foreach (var package in model.Packages)
        {
            text += $"\n| {package.Name} | {package.Recommendation} |";
        }

        var message = new MessageCard(subject)
        {
            Text = text,
            Actions =
            [
                new OpenUriAction("View Detail", model.ProjectDependencyUrl),
                new OpenUriAction("Open Repo", model.RepoUrl)
            ]
        };
        await PushMessage(message);
    }

    public async Task AlertProjectWithoutMember(AlertProjectWithoutMemberModel model, List<string>? receivers = null)
    {
        var action = new OpenUriAction("View Project");
        action.AddTarget(TargetOs.@default, model.ProjectUrl);
        var message = new MessageCard($"Action Required: Add at least one member to \"{model.ProjectName}\" project")
        {
            Text =
                $"<p>We have detected that the project <b>{model.ProjectName}</b> currently has no members assigned. " +
                $"To ensure that the project receives necessary notifications, please add at least one member to the project as soon as possible.</p>" +
                $"<p>This will help ensure that your team receives the security alerts about the project.</p>",
            Actions = [action]
        };
        await PushMessage(message);
    }

    private async Task PushMessage(MessageCard message)
    {
        try
        {
            await teamsClient.PostAsync(message);
        }
        catch (System.Exception e)
        {
            logger?.LogError(e.Message);
        }
    }
}