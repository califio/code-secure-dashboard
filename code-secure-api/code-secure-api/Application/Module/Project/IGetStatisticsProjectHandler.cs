using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Application.Module.Statistic;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project;

public interface IGetStatisticsProjectHandler : IHandler<Guid, ProjectStatistics>;

public class GetStatisticsProjectHandler(
    AppDbContext context,
    IStatsSastFinding statisticsSastFinding,
    IStatsPackageProject statisticsPackageProject
) : IGetStatisticsProjectHandler
{
    public async Task<Result<ProjectStatistics>> HandleAsync(Guid request)
    {
        StatisticFilter filter = new StatisticFilter
        {
            ProjectId = request
        };
        return new ProjectStatistics
        {
            OpenFinding = await context.Findings.CountAsync(finding =>
                finding.ProjectId == request && finding.Status == FindingStatus.Open),
            SeveritySast = await statisticsSastFinding.StatsSastFindingBySeverityAsync(filter),
            StatusSast = await statisticsSastFinding.StatsSastFindingByStatusAsync(filter),
            SeveritySca = await statisticsPackageProject.StatsPackageProjectBySeverityAsync(filter),
            StatusSca = await statisticsPackageProject.StatsPackageProjectByStatusAsync(filter)
        };
    }
}