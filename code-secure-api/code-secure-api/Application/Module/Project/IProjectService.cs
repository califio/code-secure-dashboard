using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Application.Module.Project.Command;
using CodeSecure.Application.Module.Project.Model;
using CodeSecure.Application.Module.Stats;
using CodeSecure.Application.Module.Stats.Model;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Extension;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Project;

public interface IProjectService
{
    Task<ProjectStatistics> GetStatsAsync(Guid projectId);
    Task<byte[]> ExportAsync(Guid projectId, ExportType exportType, FindingFilter filter);
    Task<Page<ProjectSummary>> GetProjectByFilterAsync(ProjectFilter filter);
    Task<ProjectInfo> GetProjectByIdAsync(Guid projectId);
    Task<Page<ProjectScan>> GetProjectScanByFilterAsync(Guid projectId, ProjectScanFilter filter);
    Task<Page<ProjectCommitScanSummary>> GetProjectCommitScanSummaryAsync(Guid projectId, ProjectCommitFilter filter);
    Task<List<ProjectCommitSummary>> ListProjectCommitAsync(Guid projectId);
}

public class ProjectService(
    IHttpContextAccessor accessor, 
    AppDbContext context) : BaseService(accessor), IProjectService
{
    public async Task<ProjectStatistics> GetStatsAsync(Guid projectId)
    {
        StatisticFilter filter = new StatisticFilter
        {
            ProjectId = projectId
        };
        return new ProjectStatistics
        {
            OpenFinding = await context.Findings.CountAsync(finding =>
                finding.ProjectId == projectId && finding.Status == FindingStatus.Open),
            SeveritySast = await context.StatsSastFindingBySeverityAsync(filter),
            StatusSast = await context.StatsSastFindingByStatusAsync(filter),
            SeveritySca = await context.StatsPackageProjectBySeverityAsync(filter),
            StatusSca = await context.StatsPackageProjectByStatusAsync(filter)
        };
    }

    public async Task<byte[]> ExportAsync(Guid projectId, ExportType exportType, FindingFilter filter)
    {
        filter.ProjectId = projectId;
        return (await new ExportFindingByTypeCommand(context, CurrentUser)
            .ExecuteAsync(exportType, filter)).GetResult();
    }

    public async Task<Page<ProjectSummary>> GetProjectByFilterAsync(ProjectFilter filter)
    {
        return (await new GetProjectByFilterCommand(context, CurrentUser)
            .ExecuteAsync(filter)).GetResult();
    }

    public async Task<ProjectInfo> GetProjectByIdAsync(Guid projectId)
    {
        return (await new GetProjectByIdCommand(context)
            .ExecuteAsync(projectId)).GetResult();
    }

    public async Task<Page<ProjectScan>> GetProjectScanByFilterAsync(Guid projectId, ProjectScanFilter filter)
    {
        return (await new GetProjectScanByFilterCommand(context)
            .ExecuteAsync(projectId, filter)).GetResult();
    }

    public async Task<Page<ProjectCommitScanSummary>> GetProjectCommitScanSummaryAsync(Guid projectId, ProjectCommitFilter filter)
    {
        return (await new GetProjectCommitScanSummaryCommand(context)
            .ExecuteAsync(projectId, filter)).GetResult();
    }

    public async Task<List<ProjectCommitSummary>> ListProjectCommitAsync(Guid projectId)
    {
        return (await new ListProjectCommitCommand(context)
            .ExecuteAsync(projectId)).GetResult();
    }
}