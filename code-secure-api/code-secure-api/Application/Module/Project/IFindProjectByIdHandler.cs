using CodeSecure.Application.Module.Project.Model;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project;

public interface IFindProjectByIdHandler : IHandler<Guid, ProjectInfo>;
public class FindProjectByIdHandler(AppDbContext context): IFindProjectByIdHandler
{
    public async Task<Result<ProjectInfo>> HandleAsync(Guid request)
    {
        var project = await context.Projects
            .Include(projects => projects.SourceControl!)
            .FirstOrDefaultAsync(project => project.Id == request);
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