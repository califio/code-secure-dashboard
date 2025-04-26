using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Core.Utils;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Command;

public class GetProjectThresholdCommand(AppDbContext context)
{
    public async Task<Result<ThresholdProject>> ExecuteAsync(Guid projectId)
    {
        var projectSetting = await context.ProjectSettings
            .Where(record => record.ProjectId == projectId)
            .FirstOrDefaultAsync();
        if (projectSetting == null) return Result.Fail("Project setting not found");
        return new ThresholdProject
        {
            Sast = JSONSerializer.DeserializeOrDefault(projectSetting.SastSetting,
                new ThresholdSetting()),
            Sca = JSONSerializer.DeserializeOrDefault(projectSetting.ScaSetting,
                new ThresholdSetting()),
        };
    }
}