using AutoMapper;
using CodeSecure.Api.Project.Model;
using CodeSecure.Authentication;
using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Extension;
using CodeSecure.Enum;
using CodeSecure.Exception;
using CodeSecure.Manager.Integration.Mail;
using CodeSecure.Manager.Integration.Model;
using CodeSecure.Manager.Integration.Teams;
using CodeSecure.Manager.Integration.TicketTracker.Jira;
using CodeSecure.Manager.Project;
using CodeSecure.Manager.Project.Model;
using CodeSecure.Manager.Setting;
using CodeSecure.Manager.Statistic;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Api.Project.Service;

public class ProjectService(
    IMapper mapper,
    AppDbContext context,
    IProjectManager projectManager,
    IJiraManager jiraManager,
    IStatisticManager statisticManager,
    IMailSender mailSender,
    IHttpContextAccessor contextAccessor) : BaseService<Projects>(contextAccessor), IProjectService
{
    public async Task<Page<ProjectSummary>> GetProjectsAsync(ProjectFilter filter)
    {
        // note: currently it allows authenticated users list all projects but can't see detail project.
        var query = context.Projects
            .Include(record => record.SourceControl)
            .Distinct();
        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(p => p.Name.Contains(filter.Name));
        }

        return await query.OrderBy(filter.SortBy.ToString(), filter.Desc).Select(p => new ProjectSummary
        {
            Id = p.Id,
            Name = p.Name,
            SeverityCritical = context.Findings.Count(finding =>
                finding.ProjectId == p.Id && finding.Severity == FindingSeverity.Critical &&
                finding.Status != FindingStatus.Incorrect),
            SeverityHigh = context.Findings.Count(finding =>
                finding.ProjectId == p.Id && finding.Severity == FindingSeverity.High &&
                finding.Status != FindingStatus.Incorrect),
            SeverityMedium = context.Findings.Count(finding =>
                finding.ProjectId == p.Id && finding.Severity == FindingSeverity.Medium &&
                finding.Status != FindingStatus.Incorrect),
            SeverityLow = context.Findings.Count(finding =>
                finding.ProjectId == p.Id && finding.Severity == FindingSeverity.Low &&
                finding.Status != FindingStatus.Incorrect),
            SeverityInfo = context.Findings.Count(finding =>
                finding.ProjectId == p.Id && finding.Severity == FindingSeverity.Info &&
                finding.Status != FindingStatus.Incorrect),
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            SourceType = p.SourceControl!.Type
        }).PageAsync(filter.Page, filter.Size);
    }

    public async Task<ProjectInfo> GetProjectInfoAsync(Guid projectId)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Read)) throw new AccessDeniedException();

        var setting = await context.ProjectSettings
            .Where(record => record.ProjectId == project.Id)
            .FirstAsync();
        var result = mapper.Map<ProjectInfo>(project);
        return result;
    }

    public async Task<Page<ProjectScan>> GetScansAsync(Guid projectId, ProjectScanFilter filter)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Read)) throw new AccessDeniedException();

        var query = context.Scans
            .Include(scan => scan.Scanner)
            .Include(scan => scan.Commit)
            .Where(scan => scan.ProjectId == project.Id);
        if (filter.Status != null) query = query.Where(scan => scan.Status == filter.Status);

        if (!string.IsNullOrEmpty(filter.Scanner))
            query = query.Where(scan => scan.Scanner != null && scan.Scanner.Name.Equals(filter.Scanner));

        if (filter.Type != null) query = query.Where(scan => scan.Scanner != null && scan.Scanner.Type == filter.Type);

        var results = await query.OrderBy(nameof(Scans.CompletedAt), true).Select(scan => new ProjectScan
        {
            Id = scan.Id,
            GitAction = scan.Commit!.Action,
            Metadata = scan.Metadata,
            ScannerId = scan.Scanner!.Id,
            Scanner = scan.Scanner!.Name,
            Type = scan.Scanner!.Type,
            Status = scan.Status,
            StartedAt = scan.StartedAt,
            CompletedAt = scan.CompletedAt,
            SeverityCritical = context.ScanFindings.Include(e => e.Finding)
                .Count(e =>
                    e.ScanId == scan.Id &&
                    e.Finding!.Severity == FindingSeverity.Critical &&
                    e.Status != FindingStatus.Incorrect &&
                    e.Status != FindingStatus.Fixed),
            SeverityHigh = context.ScanFindings.Include(e => e.Finding)
                .Count(e =>
                    e.ScanId == scan.Id &&
                    e.Finding!.Severity == FindingSeverity.High &&
                    e.Status != FindingStatus.Incorrect &&
                    e.Status != FindingStatus.Fixed),
            SeverityMedium = context.ScanFindings.Include(e => e.Finding)
                .Count(e =>
                    e.ScanId == scan.Id &&
                    e.Finding!.Severity == FindingSeverity.Medium &&
                    e.Status != FindingStatus.Incorrect &&
                    e.Status != FindingStatus.Fixed),
            SeverityLow = context.ScanFindings.Include(e => e.Finding)
                .Count(e =>
                    e.ScanId == scan.Id &&
                    e.Finding!.Severity == FindingSeverity.Low &&
                    e.Status != FindingStatus.Incorrect &&
                    e.Status != FindingStatus.Fixed),
            SeverityInfo = context.ScanFindings.Include(e => e.Finding)
                .Count(e =>
                    e.ScanId == scan.Id &&
                    e.Finding!.Severity == FindingSeverity.Info &&
                    e.Status != FindingStatus.Incorrect &&
                    e.Status != FindingStatus.Fixed),
            CommitBranch = scan.Commit.Branch,
            TargetBranch = scan.Commit.TargetBranch,
            CommitTitle = scan.Name,
            JobUrl = scan.JobUrl,
            CommitId = scan.CommitId
        }).PageAsync(filter.Page, filter.Size);
        return results;
    }

    public async Task<List<ProjectCommitSummary>> GetCommitsAsync(Guid projectId)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Read)) throw new AccessDeniedException();

        return await context.ProjectCommits
            .Where(record => record.ProjectId == project.Id)
            .Select(record => mapper.Map<ProjectCommitSummary>(record))
            .ToListAsync();
    }

    public async Task<List<Scanners>> GetScannersAsync(Guid projectId)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Read)) throw new AccessDeniedException();
        return await context.Scans
            .Include(scan => scan.Scanner)
            .Where(scan => scan.ProjectId == project.Id)
            .Select(scan => scan.Scanner!)
            .Distinct().ToListAsync();
    }


    public async Task<Page<ProjectFinding>> GetFindingsAsync(Guid projectId, ProjectFindingFilter filter)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Read)) throw new AccessDeniedException();

        var query = context.Findings
            .Include(finding => finding.Scanner)
            .Where(finding => finding.ProjectId == project.Id);
        if (filter.CommitId != null)
            query = query.Where(finding => context.ScanFindings.Any(record =>
                record.FindingId == finding.Id
                && record.Scan!.CommitId == filter.CommitId)
            );
        if (filter.Status is { Count: > 0 })
        {
            query = query.Where(finding => filter.Status.Contains(finding.Status));
        }

        if (filter.Scanner is { Count: > 0 })
        {
            query = query.Where(finding => filter.Scanner.Contains(finding.ScannerId));
        }

        if (filter.Severity != null) query = query.Where(finding => finding.Severity == filter.Severity);

        if (!string.IsNullOrEmpty(filter.Name)) query = query.Where(finding => finding.Name.Contains(filter.Name));

        return await query.Distinct()
            .OrderBy(filter.SortBy.ToString(), filter.Desc)
            .Select(finding => mapper.Map<ProjectFinding>(finding))
            .PageAsync(filter.Page, filter.Size);
    }

    public async Task<Page<ProjectPackage>> GetPackagesAsync(Guid projectId, ProjectPackageFilter filter)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Read)) throw new AccessDeniedException();

        var query = context.ProjectPackages
            .Include(record => record.Package)
            .Where(record => record.ProjectId == project.Id);
        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(record => record.Package!.Name.Contains(filter.Name));

        return await query.Select(record => new ProjectPackage
        {
            Location = record.Location,
            Group = record.Package!.Group,
            Name = record.Package.Name,
            Version = record.Package.Version,
            Type = record.Package.Type,
            Id = record.PackageId,
            FixedVersion = record.Package.FixedVersion,
            RiskImpact = record.Package.RiskImpact,
            RiskLevel = record.Package.RiskLevel
        }).OrderBy(filter.SortBy.ToString(), filter.Desc).PageAsync(filter.Page, filter.Size);
    }

    public async Task<ProjectStatistics> GetStatisticsAsync(Guid projectId)
    {
        var project = await FindByIdAsync(projectId);

        if (!HasPermission(project, PermissionAction.Read)) throw new AccessDeniedException();
        return new ProjectStatistics
        {
            OpenFinding = await context.Findings.CountAsync(finding =>
                finding.ProjectId == project.Id && finding.Status == FindingStatus.Open),
            SeveritySast = await statisticManager.SeveritySastAsync(project.Id),
            SeveritySca = await statisticManager.SeverityScaAsync(project.Id),
            StatusSast = await statisticManager.StatusSastAsync(project.Id),
            StatusSca = await statisticManager.StatusScaAsync(project.Id)
        };
    }

    public async Task<Page<ProjectUser>> GetMembersAsync(Guid projectId, ProjectUserFilter filter)
    {
        var project = await FindByIdAsync(projectId);

        if (!HasPermission(project, PermissionAction.Read)) throw new AccessDeniedException();

        var query = context.ProjectUsers.Include(record => record.User)
            .Where(projectUsers => projectUsers.ProjectId == project.Id);
        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(record => record.User!.UserName!.Contains(filter.Name!));

        if (filter.Role != null) query = query.Where(record => record.Role == filter.Role);

        return await query.OrderBy(nameof(ProjectUsers.CreatedAt), filter.Desc).Select(record => new ProjectUser
        {
            UserId = record.UserId,
            UserName = record.User!.UserName!,
            Avatar = record.User.Avatar,
            Role = record.Role,
            FullName = record.User.FullName,
            Email = record.User.Email,
            CreatedAt = record.CreatedAt
        }).PageAsync(filter.Page, filter.Size);
    }

    public async Task<ProjectUser> AddMemberAsync(Guid projectId, AddMemberRequest request)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Update)) throw new AccessDeniedException();

        var user = await context.Users.FirstOrDefaultAsync(user => user.Id == request.UserId);
        if (user == null) throw new BadRequestException("User not found");

        if (await context.ProjectUsers.AnyAsync(record => record.ProjectId == project.Id && record.UserId == user.Id))
            throw new BadRequestException("The user is already a member of the project");
        var projectUser = new ProjectUsers
        {
            UserId = user.Id,
            ProjectId = project.Id,
            AddedById = CurrentUser().Id,
            Role = request.Role,
            ReceiveNotification = true,
            CreatedAt = DateTime.UtcNow,
            User = user
        };
        context.ProjectUsers.Add(projectUser);
        await context.SaveChangesAsync();
        mailSender.SendAddUserToProject([user.Email!], new AddUserToProjectModel
        {
            ProjectName = project.Name,
            ProjectUrl = $"{FrontendUrl()}/#/project/{project.Id}",
            Role = request.Role.ToString(),
            Username = user.UserName!,
        });
        return mapper.Map<ProjectUser>(user);
    }

    public async Task<ProjectUser> UpdateMemberAsync(Guid projectId, Guid userId, UpdateMemberRequest request)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Update)) throw new AccessDeniedException();
        var projectUser = context.ProjectUsers
                              .Include(record => record.User)
                              .FirstOrDefault(record => record.ProjectId == project.Id && record.UserId == userId)
                          ?? throw new BadRequestException("the user is not member of project");
        projectUser.Role = request.Role;
        context.ProjectUsers.Update(projectUser);
        await context.SaveChangesAsync();
        var result = mapper.Map<ProjectUser>(projectUser.User);
        result.Role = projectUser.Role;
        return result;
    }

    public async Task DeleteMemberAsync(Guid projectId, Guid userId)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Update)) throw new AccessDeniedException();

        var projectUser = context.ProjectUsers
                              .Include(record => record.User)
                              .FirstOrDefault(record => record.ProjectId == project.Id && record.UserId == userId)
                          ?? throw new BadRequestException("the user is not member of project");
        context.ProjectUsers.Remove(projectUser);
        await context.SaveChangesAsync();
        mailSender.SendRemoveProjectMember([projectUser.User!.Email!], new RemoveProjectMemberModel
        {
            Username = projectUser.User.UserName!,
            ProjectName = project.Name,
            ProjectUrl = $"{FrontendUrl()}/#/project/{project.Id.ToString()}",
        });
    }

    public async Task<ProjectSetting> GetProjectSettingAsync(Guid projectId)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Read)) throw new AccessDeniedException();
        return await projectManager.GetProjectSettingAsync(projectId);
    }

    public async Task UpdateSastSettingAsync(Guid projectId, ThresholdSetting request)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Update)) throw new AccessDeniedException();
        await projectManager.UpdateSastSettingAsync(projectId, request);
    }

    public async Task UpdateScaSettingAsync(Guid projectId, ThresholdSetting request)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Update)) throw new AccessDeniedException();
        await projectManager.UpdateScaSettingAsync(projectId, request);
    }

    public async Task<ProjectIntegration> GetIntegrationSettingAsync(Guid projectId)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Read)) throw new AccessDeniedException();
        return new ProjectIntegration
        {
            Jira = (await projectManager.GetJiraSettingAsync(projectId)).Active,
            Teams = (await projectManager.GetTeamsSettingAsync(projectId)).Active
        };
    }

    public async Task<JiraProjectSettingResponse> GetJiraIntegrationSettingAsync(Guid projectId, bool reload)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Update)) throw new AccessDeniedException();
        var jiraSetting = await projectManager.GetJiraSettingAsync(projectId);
        
        return new JiraProjectSettingResponse
        {
            Active = jiraSetting.Active,
            ProjectKey = jiraSetting.ProjectKey,
            JiraProjects = await jiraManager.GetProjectsSummaryAsync(reload),
            IssueType = jiraSetting.IssueType
        };
    }

    public async Task UpdateJiraIntegrationSettingAsync(Guid projectId, JiraProjectSetting request)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Update)) throw new AccessDeniedException();
        if (await jiraManager.GetProjectAsync(request.ProjectKey) == null)
        {
            throw new BadRequestException("Jira project not found");
        }
        await projectManager.UpdateJiraSettingAsync(projectId, request);
    }

    public async Task<TeamsSetting> GetTeamsIntegrationSettingAsync(Guid projectId)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Update)) throw new AccessDeniedException();
        var teamsIntegrationSetting = await projectManager.GetTeamsSettingAsync(projectId);
        return teamsIntegrationSetting with { Webhook = string.Empty };
    }

    public async Task UpdateTeamsIntegrationSettingAsync(Guid projectId, TeamsSetting request)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Update)) throw new AccessDeniedException();
        var teamsIntegrationSetting = await projectManager.GetTeamsSettingAsync(projectId);
        if (string.IsNullOrEmpty(request.Webhook))
        {
            request.Webhook = teamsIntegrationSetting.Webhook;
        }

        await projectManager.UpdateTeamsSettingAsync(projectId, request);
    }

    public async Task TestTeamsIntegrationSettingAsync(Guid projectId)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Update)) throw new AccessDeniedException();
        var setting = await projectManager.GetTeamsSettingAsync(projectId);
        var teamsAlert = new TeamsAlert(setting);
        var result = await teamsAlert.TestAlert(CurrentUser().UserName);
        if (!result.Succeeded)
        {
            throw new BadRequestException(result.Error);
        }
    }

    public async Task<AlertSetting> GetMailIntegrationSettingAsync(Guid projectId)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Update)) throw new AccessDeniedException();
        return await projectManager.GetMailSettingAsync(projectId);
    }

    public async Task UpdateMailIntegrationSettingAsync(Guid projectId, AlertSetting request)
    {
        var project = await FindByIdAsync(projectId);
        if (!HasPermission(project, PermissionAction.Update)) throw new AccessDeniedException();
        await projectManager.UpdateMailSettingAsync(projectId, request);
    }

    protected override bool HasPermission(Projects entity, string action)
    {
        var currentUser = CurrentUser();
        if (currentUser.HasClaim(PermissionType.Project, action)) return true;

        if (action == PermissionAction.Read)
            return context.ProjectUsers.Any(member =>
                member.UserId == currentUser.Id &&
                member.ProjectId == entity.Id);

        if (action == PermissionAction.Update)
            return context.ProjectUsers.Any(member =>
                member.UserId == currentUser.Id &&
                member.ProjectId == entity.Id &&
                member.Role == ProjectRole.Manager);

        return false;
    }

    protected override async Task<Projects> FindByIdAsync(Guid id)
    {
        var project = await context.Projects
            .Include(record => record.SourceControl)
            .FirstOrDefaultAsync(project => project.Id == id);
        if (project == null) throw new BadRequestException("project not found");

        return project;
    }
}