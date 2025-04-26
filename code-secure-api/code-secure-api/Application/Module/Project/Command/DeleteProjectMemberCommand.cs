using CodeSecure.Application.Module.Mail;
using CodeSecure.Application.Module.Project.Model;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Command;


public class DeleteProjectMemberCommand(AppDbContext context, IMailRemoveUserFromProject mailRemoveUserFromProject)
{
    public async Task<Result<bool>> ExecuteAsync(DeleteProjectMemberRequest request)
    {
        var projectUser = await context.ProjectUsers
            .Include(record => record.User)
            .FirstOrDefaultAsync(record => record.ProjectId == request.ProjectId && record.UserId == request.UserId);
        if (projectUser == null) return Result.Fail("Project user not found");
        context.ProjectUsers.Remove(projectUser);
        await context.SaveChangesAsync();
        var project = await context.Projects.FirstAsync(project => project.Id == request.ProjectId);
        _ = mailRemoveUserFromProject.SendAsync(projectUser.User!.Email!, new MailRemoveUserFromProjectModel
        {
            Username = projectUser.User.UserName!,
            Project = project,
        });
        return true;
    }
}