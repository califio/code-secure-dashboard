using CodeSecure.Core.Utils;
using FluentResults;
using FluentResults.Extensions;

namespace CodeSecure.Application.Module.Project.Setting;

public interface IGeneralSettingProjectService
{
    Task<Result<HashSet<string>>> GetDefaultBranchesAsync(Guid projectId);
    Task<Result<bool>> UpdateDefaultBranchesAsync(Guid projectId, HashSet<string> defaultBranches);
}

public class GeneralSettingProjectService(AppDbContext context) : IGeneralSettingProjectService
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
}