using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Metadata;
using CodeSecure.Enum;
using CodeSecure.Extension;
using CodeSecure.Manager.Project.Model;
using CodeSecure.Manager.Scanner;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Manager.Project;

public class ProjectManager(
    AppDbContext context,
    IScannerManager scannerManager,
    ILogger<ProjectManager> logger
) : IProjectManager
{
    private static readonly SemaphoreSlim Lock = new(1, 1);

    public async Task<Projects> CreateOrUpdateAsync(Projects input)
    {
        await Lock.WaitAsync();
        try
        {
            var project = await context.Projects.FirstOrDefaultAsync(record =>
                record.SourceControlId == input.SourceControlId && record.RepoId == input.RepoId);
            if (project == null)
            {
                project = input;
                project.Id = Guid.NewGuid();
                context.Projects.Add(project);
                await context.SaveChangesAsync();
                var severityThreshold = new SeverityThreshold();
                var setting = new ProjectSettings
                {
                    ProjectId = project.Id,
                    Metadata = JSONSerializer.Serialize(new ProjectSettingMetadata
                    {
                        SastSetting = new SastSetting
                        {
                            Mode = ThresholdMode.MonitorOnly,
                            SeverityThreshold = severityThreshold
                        },
                        ScaSetting = new ScaSetting
                        {
                            Mode = ThresholdMode.MonitorOnly,
                            SeverityThreshold = severityThreshold
                        }
                    })
                };
                context.ProjectSettings.Add(setting);
                await context.SaveChangesAsync();
            }
            else
            {
                if (project.Name != input.Name)
                {
                    project.Name = input.Name;
                }

                if (project.RepoUrl != input.RepoUrl)
                {
                    project.RepoUrl = input.RepoUrl;
                }

                context.Projects.Update(project);
                await context.SaveChangesAsync();
            }

            return project;
        }
        finally
        {
            Lock.Release();
        }
    }

    public Task<List<Member>> GetMembersAsync(Guid projectId)
    {
        return context.ProjectUsers
            .Include(record => record.User)
            .Where(record => record.ProjectId == projectId)
            .Select(record => new Member
            {
                UserId = record.UserId,
                UserName = record.User!.UserName!,
                Email = record.User.Email!,
                Role = record.Role,
                Status = record.User.Status,
                FullName = record.User.FullName,
                Avatar = record.User.Avatar
            })
            .ToListAsync();
    }

    public async Task<DependencyReport?> DependencyReportAsync(Guid projectId)
    {
        var project = await context.Projects.FirstOrDefaultAsync(record => record.Id == projectId);
        if (project == null)
        {
            return null;
        }

        return await DependencyReportAsync(project);
    }

    public async Task<DependencyReport> DependencyReportAsync(Projects project)
    {
        logger.LogInformation($"Dependency report for project: {project.Name}");
        var scanners = (await scannerManager.GetScannerByTypeAsync(ScannerType.Dependency))
            .Select(item => item.Id);
        var findings = context.Findings
            .Where(record =>
                record.ProjectId == project.Id &&
                (record.Status == FindingStatus.Open || record.Status == FindingStatus.Confirmed) &&
                scanners.Contains(record.ScannerId)
            ).OrderByDescending(record => record.Severity)
            .ToList();
        var projectPackages = context.ProjectPackages
            .Include(record => record.Package!)
            .Where(record => record.ProjectId == project.Id).ToList();
        var packages = projectPackages
            .FindAll(item => !string.IsNullOrEmpty(item.Package!.FixedVersion))
            .Select(item =>
            {
                var name = string.IsNullOrEmpty(item.Package!.Group)
                    ? $"{item.Package.Name}@{item.Package.Version}"
                    : $"{item.Package.Group}:{item.Package.Name}@{item.Package.Version}";
                var recommendation = "";
                if (!string.IsNullOrEmpty(item.Package.FixedVersion))
                {
                    recommendation = $"Upgrade to version {item.Package.FixedVersion}";
                }
                return new DependencyProject
                {
                    Name = name,
                    Location = item.Location,
                    Recommendation = recommendation,
                    Impact = item.Package.RiskLevel.ToString()
                };
            }).ToList();
        var report = new DependencyReport
        {
            Critical = findings.Count(finding => finding.Severity == FindingSeverity.Critical),
            High = findings.Count(finding => finding.Severity == FindingSeverity.High),
            Medium = findings.Count(finding => finding.Severity == FindingSeverity.Medium),
            Low = findings.Count(finding => finding.Severity == FindingSeverity.Low),
            Packages = packages,
            RepoUrl = project.RepoUrl,
            RepoName = project.Name,
            ProjectDependencyUrl = $"{Application.Config.FrontendUrl}/#/project/{project.Id.ToString()}/dependency"
        };
        return report;
    }
}