using CodeSecure.Application.Module.Integration.Teams.Client;
using CodeSecure.Application.Module.Integration.Teams.Client.Action;
using FluentResults;

namespace CodeSecure.Application.Module.Integration.Teams;

public class AlertProjectWithoutMemberTeams(string webhook) : IAlertProjectWithoutMember
{
    public async Task<Result<bool>> AlertAsync(List<string> receivers, AlertProjectWithoutMemberModel model)
    {
        try
        {
            var action = new OpenUriAction("View Project");
            action.AddTarget(TargetOs.@default, model.ProjectUrl());
            var message =
                new MessageCard($"Action Required: Add at least one member to \"{model.Project.Name}\" project")
                {
                    Text =
                        $"<p>We have detected that the project <b>{model.Project.Name}</b> currently has no members assigned. " +
                        $"To ensure that the project receives necessary notifications, please add at least one member to the project as soon as possible.</p>" +
                        $"<p>This will help ensure that your team receives the security alerts about the project.</p>",
                    Actions = [action]
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