using CodeSecure.Application.Module.Project.Model;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Command;

public class ListProjectCommitCommand(AppDbContext context)
{
    public async Task<Result<List<ProjectCommitSummary>>> ExecuteAsync(Guid projectId)
    {
        return await context.ProjectCommits
            .Where(record => record.ProjectId == projectId)
            .Select(record => new ProjectCommitSummary
            {
                CommitId = record.Id,
                Branch = record.Branch,
                Type = record.Type,
                TargetBranch = record.TargetBranch,
                IsDefault = record.IsDefault
            })
            .ToListAsync();
    }
}