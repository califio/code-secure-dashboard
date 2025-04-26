using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Core.Utils;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Command;


public class UpdateProjectThresholdCommand(AppDbContext context)
{
    public async Task<Result<bool>> ExecuteAsync(Guid projectId, UpdateProjectThresholdRequest request)
    {
        var projectSetting =
            await context.ProjectSettings.FirstOrDefaultAsync(record => record.ProjectId == projectId);
        if (projectSetting == null) return Result.Fail("Project setting not found");
        if (request.Sca != null)
        {
            projectSetting.ScaSetting = JSONSerializer.Serialize(request.Sca);
        }

        if (request.Sast != null)
        {
            projectSetting.SastSetting = JSONSerializer.Serialize(request.Sast);
        }

        context.ProjectSettings.Update(projectSetting);
        await context.SaveChangesAsync();
        return true;
    }
}