using CodeSecure.Application.Module.Project.Model;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Command;

public class GetProjectByIdCommand(AppDbContext context)
{
    public async Task<Result<ProjectInfo>> ExecuteAsync(Guid projectId)
    {
        var project = await context.Projects
            .Include(projects => projects.SourceControl!)
            .FirstOrDefaultAsync(project => project.Id == projectId);
        if (project == null)
        {
            return Result.Fail("Project not found");
        }
        return new ProjectInfo
        {
            Id = project.Id,
            Name = project.Name,
            RepoId = project.RepoId,
            RepoUrl = project.RepoUrl,
            SourceType = project.SourceControl!.Type,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt
        };
    }
}