using CodeSecure.Application.Module.Mail;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Member;

public record DeleteProjectMemberRequest
{
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
}

public interface IDeleteProjectMemberHandler : IHandler<DeleteProjectMemberRequest, bool>;

public class DeleteProjectMemberHandler(AppDbContext context, IMailRemoveUserFromProject mailRemoveUserFromProject)
    : IDeleteProjectMemberHandler
{
    public async Task<Result<bool>> HandleAsync(DeleteProjectMemberRequest request)
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