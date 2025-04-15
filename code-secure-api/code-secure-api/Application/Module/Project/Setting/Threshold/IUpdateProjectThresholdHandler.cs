using System.Text.Json.Serialization;
using CodeSecure.Core.Utils;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Setting.Threshold;

public record UpdateProjectThresholdRequest
{
    public ThresholdSetting? Sast { get; set; }
    public ThresholdSetting? Sca { get; set; }
    [JsonIgnore] public Guid ProjectId { get; set; }
}

public interface IUpdateProjectThresholdHandler : IHandler<UpdateProjectThresholdRequest, bool>;

public class UpdateProjectThresholdHandler(AppDbContext context) : IUpdateProjectThresholdHandler
{
    public async Task<Result<bool>> HandleAsync(UpdateProjectThresholdRequest request)
    {
        var projectSetting =
            await context.ProjectSettings.FirstOrDefaultAsync(record => record.ProjectId == request.ProjectId);
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