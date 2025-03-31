using CodeSecure.Core.Extension;
using CodeSecure.Core.Utils;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project.Threshold;

public interface IGetProjectThresholdHandler : IHandler<Guid, ThresholdProject>;

public class GetProjectThresholdHandler(AppDbContext context) : IGetProjectThresholdHandler
{
    public async Task<Result<ThresholdProject>> HandleAsync(Guid request)
    {
        var projectSetting = await context.ProjectSettings
            .Where(record => record.ProjectId == request)
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