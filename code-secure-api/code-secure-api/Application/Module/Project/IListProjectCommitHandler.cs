using CodeSecure.Application.Module.Project.Model;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project;

public interface IListProjectCommitHandler : IHandler<Guid, List<ProjectCommitSummary>>;

public class ListProjectCommitHandler(AppDbContext context) : IListProjectCommitHandler
{
    public async Task<Result<List<ProjectCommitSummary>>> HandleAsync(Guid request)
    {
        return await context.ProjectCommits
            .Where(record => record.ProjectId == request)
            .Select(record => new ProjectCommitSummary
            {
                CommitId = record.Id,
                Branch = record.Branch,
                Action = record.Type,
                TargetBranch = record.TargetBranch,
                IsDefault = record.IsDefault
            })
            .ToListAsync();
    }
}