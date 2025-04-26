using CodeSecure.Application.Module.Project.Command;
using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Core.Extension;
using CodeSecure.Core.Utils;
using FluentResults;
using FluentResults.Extensions;

namespace CodeSecure.Application.Module.Project;

public interface IProjectSettingService
{
    Task<Result<HashSet<string>>> GetDefaultBranchesAsync(Guid projectId);
    Task<Result<bool>> UpdateDefaultBranchesAsync(Guid projectId, HashSet<string> defaultBranches);

    Task<ThresholdProject> GetProjectThresholdAsync(Guid projectId);
    Task<bool> UpdateProjectThresholdAsync(Guid projectId, UpdateProjectThresholdRequest request);
}

public class ProjectSettingService(AppDbContext context) : IProjectSettingService
{
    public async Task<Result<HashSet<string>>> GetDefaultBranchesAsync(Guid projectId)
    {
         return await context.GetProjectSettingsAsync(projectId)
            .Bind(setting => Result.Ok(setting.GetDefaultBranches()));
    }

    public async Task<Result<bool>> UpdateDefaultBranchesAsync(Guid projectId, HashSet<string> defaultBranches)
    {
        return await context.GetProjectSettingsAsync(projectId).Bind(setting =>
        {
            setting.DefaultBranch = JSONSerializer.Serialize(defaultBranches);
            context.Update(setting);
            context.SaveChanges();
            return Result.Ok();
        });
    }

    public async Task<ThresholdProject> GetProjectThresholdAsync(Guid projectId)
    {
        return (await new GetProjectThresholdCommand(context).ExecuteAsync(projectId)).GetResult();
    }

    public async Task<bool> UpdateProjectThresholdAsync(Guid projectId, UpdateProjectThresholdRequest request)
    {
        return (await new UpdateProjectThresholdCommand(context).ExecuteAsync(projectId, request)).GetResult();
    }
}