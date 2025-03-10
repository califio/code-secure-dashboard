using System.Data;
using ClosedXML.Excel;
using CodeSecure.Api.Finding.Model;
using CodeSecure.Authentication;
using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Extension;
using CodeSecure.Database.Metadata;
using CodeSecure.Enum;
using CodeSecure.Exception;
using CodeSecure.Extension;
using CodeSecure.Manager.Finding;
using CodeSecure.Manager.Finding.Model;
using CodeSecure.Manager.Integration.TicketTracker;
using CodeSecure.Manager.Integration.TicketTracker.Jira;
using CodeSecure.Manager.Project;
using CodeSecure.Manager.Scanner;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Api.Finding.Service;

public class DefaultFindingService(
    AppDbContext context,
    IHttpContextAccessor contextAccessor,
    IScannerManager scannerManager,
    IFindingManager findingManager,
    IProjectManager projectManager,
    JiraTicketTracker jiraTicketTracker,
    ILogger<DefaultFindingService> logger
) : BaseService<Findings>(contextAccessor), IFindingService
{
    public Task<Page<FindingSummary>> GetFindingsAsync(FindingFilter filter)
    {
        return findingManager.GetFindingAsync(filter, CurrentUser());
    }

    public async Task<byte[]> Export(FindingFilter filter)
    {
        var actor = CurrentUser();
        var allowReadFinding = actor.HasClaim(PermissionType.Finding, PermissionAction.Read);
        var query = context.Findings
            .Include(finding => finding.Scanner)
            .Include(finding => finding.Project)
            .Include(finding => finding.Ticket)
            .Where(finding =>
                allowReadFinding == true ||
                context.ProjectUsers.Any(projectUser =>
                    projectUser.ProjectId == finding.ProjectId &&
                    projectUser.UserId == actor.Id
                )
            );
        if (filter.ProjectId != null)
        {
            query = query.Where(finding => finding.ProjectId == filter.ProjectId);
        }

        if (filter.Status is { Count: > 0 })
        {
            query = query.Where(finding => filter.Status.Contains(finding.Status));
        }

        if (filter.Scanner is { Count: > 0 })
        {
            query = query.Where(finding => filter.Scanner.Contains(finding.ScannerId));
        }

        if (filter.Severity is { Count: > 0 })
        {
            query = query.Where(finding => filter.Severity.Contains(finding.Severity));
        }

        if (filter.StartCreatedAt != null)
        {
            query = query.Where(finding => finding.CreatedAt >= filter.StartCreatedAt);
        }

        if (filter.EndCreatedAt != null)
        {
            query = query.Where(finding => finding.CreatedAt <= filter.EndCreatedAt);
        }

        if (filter.StartFixedAt != null)
        {
            query = query.Where(finding => finding.FixedAt != null && finding.FixedAt >= filter.StartFixedAt);
        }
        if (filter.EndFixedAt != null)
        {
            query = query.Where(finding => finding.FixedAt != null && finding.FixedAt <= filter.EndFixedAt);
        }
        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(finding => finding.Name.Contains(filter.Name));
        }
        if (!string.IsNullOrEmpty(filter.RuleId))
        {
            query = query.Where(finding => finding.RuleId == filter.RuleId);
        }

        if (filter.ProjectManagerId != null)
        {
            query = query.Where(finding => context.ProjectUsers.Any(record =>
                record.Role == ProjectRole.Manager 
                && finding.ProjectId == record.ProjectId
                && record.UserId == filter.ProjectManagerId)
            );
        }

        query = query.Distinct();
        if (query.Count() > 10000)
        {
            throw new BadRequestException("Total record too large. Max record 10000");
        }
        var findings = await query.OrderBy(record => record.ProjectId).ToListAsync();
        using var wb = new XLWorkbook();
        var findingDt = new DataTable();
        findingDt.TableName = "List Finding";
        //Add Columns  
        findingDt.Columns.Add("Repo", typeof(string));
        findingDt.Columns.Add("Repo URL", typeof(string));
        findingDt.Columns.Add("Scanner", typeof(string));
        findingDt.Columns.Add("Finding", typeof(string));
        findingDt.Columns.Add("Severity", typeof(string));
        findingDt.Columns.Add("Status", typeof(string));
        findingDt.Columns.Add("Location", typeof(string));
        findingDt.Columns.Add("Description", typeof(string));
        findingDt.Columns.Add("Recommendation", typeof(string));
        findingDt.Columns.Add("Ticket", typeof(string));
        //Add Rows in DataTable  
        findings.ForEach(finding =>
        {
            findingDt.Rows.Add(
                finding.Project!.Name,
                finding.Project!.RepoUrl,
                finding.Scanner!.Name,
                finding.Name,
                finding.Severity.ToString().ToUpper(),
                finding.Status.ToString().ToUpper(),
                finding.Location,
                finding.Description,
                finding.Recommendation ?? string.Empty,
                finding.Ticket?.Url ?? string.Empty
            );
        });
        findingDt.AcceptChanges();
        wb.Worksheets.Add(findingDt);
        var stream = new MemoryStream();
        wb.SaveAs(stream);
        return stream.GetBuffer();
    }

    public async Task<FindingDetail> GetFindingAsync(Guid id)
    {
        var finding = await FindByIdAsync(id);
        if (!HasPermission(finding, PermissionAction.Read)) throw new AccessDeniedException();

        var project = await context.Projects
            .Include(record => record.SourceControl)
            .FirstAsync(project => project.Id == finding.ProjectId);
        var scanner = await scannerManager.FindByIdAsync(finding.ScannerId);
        var scans = await context.ScanFindings
            .Include(record => record.Scan)
            .ThenInclude(scan => scan!.Commit)
            .Where(record => record.FindingId == id)
            .Select(record => new FindingScan
            {
                Branch = record.Scan!.Commit!.Branch,
                CommitHash = record.CommitHash,
                IsDefault = record.Scan.Commit.IsDefault,
                ScanId = record.ScanId,
                Action = record.Scan.Commit.Type,
                TargetBranch = record.Scan.Commit.TargetBranch,
                Status = record.Status
            }).ToListAsync();
        Tickets? ticket = null;
        if (finding.TicketId != null)
        {
            ticket = await context.Tickets.FirstOrDefaultAsync(record => record.Id == finding.TicketId);
        }
        var metadata = JSONSerializer.Deserialize<FindingMetadata>(finding.Metadata);
        return new FindingDetail
        {
            Id = finding.Id,
            Identity = finding.Identity,
            Name = finding.Name,
            Description = finding.Description,
            Recommendation = finding.Recommendation,
            Status = finding.Status,
            Severity = finding.Severity,
            Project = new FindingProject
            {
                Id = project.Id,
                Name = project.Name,
                SourceType = project.SourceControl!.Type,
                RepoUrl = project.RepoUrl,
                RepoId = project.RepoId
            },
            Scanner = scanner!.Name,
            Type = scanner.Type,
            Location = new FindingLocation
            {
                Path = finding.Location ?? string.Empty,
                Snippet = finding.Snippet,
                StartLine = finding.StartLine,
                EndLine = finding.EndLine,
                StartColumn = finding.StartColumn,
                EndColumn = finding.EndColumn
            },
            Scans = scans,
            Metadata = metadata,
            FixDeadline = finding.FixDeadline,
            Ticket = ticket,
            RuleId = finding.RuleId
        };
    }

    public async Task<FindingDetail> UpdateFindingAsync(Guid findingId, UpdateFindingRequest request)
    {
        var finding = await FindByIdAsync(findingId);
        if (!HasPermission(finding, PermissionAction.Update)) throw new AccessDeniedException();

        if (request.Status != null && request.Status != finding.Status)
        {
            var activity = FindingActivities.ChangeStatus(
                CurrentUser().Id,
                findingId,
                finding.Status,
                (FindingStatus)request.Status
            );
            context.FindingActivities.Add(activity);
            await context.SaveChangesAsync();
            // auto change fix deadline base on SLA when change status to confirmed
            if (finding.FixDeadline == null && request.Status == FindingStatus.Confirmed)
            {
                var sla = await findingManager.GetSlaAsync(finding);
                if (sla > 0)
                {
                    finding.FixDeadline = DateTime.UtcNow.AddDays(sla);
                    activity = FindingActivities.ChangeDeadline(
                        null,
                        findingId,
                        null,
                        finding.FixDeadline
                    );
                    context.FindingActivities.Add(activity);
                    await context.SaveChangesAsync();
                }
            }
            if (request.Status == FindingStatus.Fixed)
            {
                await context.ScanFindings
                    .Where(record => record.FindingId == findingId && record.Status == finding.Status)
                    .ExecuteUpdateAsync(setter => setter
                        .SetProperty(record => record.Status, (FindingStatus)request.Status)
                        .SetProperty(record => record.FixedAt, DateTime.UtcNow)
                    );
                finding.FixedAt = DateTime.UtcNow;
            }
            else
            {
                await context.ScanFindings
                    .Where(record => record.FindingId == findingId && record.Status == finding.Status)
                    .ExecuteUpdateAsync(setter => setter
                        .SetProperty(record => record.Status, (FindingStatus)request.Status)
                    );
            }
            finding.Status = (FindingStatus)request.Status;
        }

        if (request.Severity != null && request.Severity != finding.Severity)
        {
            finding.Severity = (FindingSeverity)request.Severity;
        }

        if (request.FixDeadline != null && request.FixDeadline != finding.FixDeadline)
        {
            if (request.FixDeadline < DateTime.UtcNow) throw new BadRequestException("Fix deadline invalid");

            if (finding.Status != FindingStatus.Confirmed) throw new BadRequestException("Can't set fix deadline");
            context.FindingActivities.Add(FindingActivities.ChangeDeadline(
                CurrentUser().Id,
                findingId,
                finding.FixDeadline,
                request.FixDeadline
            ));
            await context.SaveChangesAsync();
            finding.FixDeadline = request.FixDeadline;
        }

        if (request.Recommendation != null)
        {
            finding.Recommendation = request.Recommendation;
        }
        await findingManager.UpdateAsync(finding);
        return await GetFindingAsync(findingId);
    }

    public async Task UpdateStatusScanFindingAsync(Guid findingId, Guid scanId, FindingStatus status)
    {
        var finding = await FindByIdAsync(findingId);
        if (!HasPermission(finding, PermissionAction.Update)) throw new AccessDeniedException();
        var scanFinding = await context.ScanFindings
            .Include(record => record.Scan)
            .FirstOrDefaultAsync(record => record.ScanId == scanId && record.FindingId == findingId);
        if (scanFinding == null)
        {
            throw new BadRequestException("Not Found");
        }

        if (scanFinding.Status != status)
        {
            var currentUser = CurrentUser();
            var commentActivity = FindingActivities.ChangeStatus(currentUser.Id, findingId, scanFinding.Status, status, scanFinding.Scan!.CommitId);
            context.FindingActivities.Add(commentActivity);
            scanFinding.Status = status;
            if (status == FindingStatus.Fixed)
            {
                scanFinding.FixedAt = DateTime.UtcNow;
            }
            context.ScanFindings.Update(scanFinding);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Page<FindingActivity>> GetFindingActivitiesAsync(Guid id, QueryFilter filter)
    {
        var finding = await FindByIdAsync(id);
        if (!HasPermission(finding, PermissionAction.Read)) throw new AccessDeniedException();

        var result = await context.FindingActivities
            .Include(activity => activity.User)
            .Include(activity => activity.Commit)
            .Where(activity => activity.FindingId == finding.Id)
            .OrderBy(nameof(FindingActivities.CreatedAt), filter.Desc)
            .Select(activity => new FindingActivity
            {
                UserId = activity.UserId,
                Username = activity.User != null ? activity.User.UserName : null,
                Avatar = activity.User != null ? activity.User.Avatar : null,
                Type = activity.Type,
                Comment = activity.Comment,
                OldState = activity.OldState,
                NewState = activity.NewState,
                CreatedAt = activity.CreatedAt,
                Commit = activity.Commit
            })
            .PageAsync(filter.Page, filter.Size);
        return result;
    }

    public async Task<FindingActivity> AddComment(Guid findingId, FindingCommentRequest request)
    {
        var finding = await FindByIdAsync(findingId);
        // allow project member comment on finding
        if (!HasPermission(finding, PermissionAction.Read))
        {
            throw new AccessDeniedException();
        }
        var currentUser = CurrentUser();
        var commentActivity = FindingActivities.AddComment(currentUser.Id, findingId, request.Comment);
        context.FindingActivities.Add(commentActivity);
        await context.SaveChangesAsync();
        return new FindingActivity
        {
            UserId = currentUser.Id,
            Username = currentUser.UserName,
            Comment = commentActivity.Comment,
            CreatedAt = commentActivity.CreatedAt,
            Type = FindingActivityType.Comment,
            Avatar = null,
        };
    }

    public async Task<Tickets> CreateTicketAsync(Guid findingId, TicketType ticketType)
    {
        var finding = await FindByIdAsync(findingId);
        if (!HasPermission(finding, PermissionAction.Update)) throw new AccessDeniedException();
        if (await scannerManager.IsSastScanner(finding.ScannerId) == false)
        {
            throw new BadRequestException("Can't create ticket for this finding");
        }
        
        var project = context.Projects.First(record => record.Id == finding.ProjectId);
        var scanner = await scannerManager.FindByIdAsync(finding.ScannerId);
        var scanFinding = await context.ScanFindings
            .Include(record => record.Scan!)
            .OrderByDescending(record => record.Scan!.CompletedAt)
            .Where(record => record.FindingId == findingId)
            .FirstAsync();
        TicketResult<Tickets>? result = null;
        if (ticketType == TicketType.Jira)
        {
            result = await jiraTicketTracker.CreateTicketAsync(new SastTicket
            {
                Commit = scanFinding.CommitHash,
                Project = project,
                Finding = finding,
                Scanner = scanner!
            });
        }

        if (result != null)
        {
            if (result.Succeeded == false)
            {
                throw new BadRequestException(result.Error);
            }
            return result.Data!;
        }
        throw new BadRequestException();
    }

    public async Task DeleteTicketAsync(Guid findingId)
    {
        var finding = await FindByIdAsync(findingId);
        if (!HasPermission(finding, PermissionAction.Update)) throw new AccessDeniedException();
        if (finding.TicketId != null)
        {
            var ticketId = finding.TicketId;
            await context.Findings.Where(record => record.Id == findingId)
                .ExecuteUpdateAsync(setter => setter.SetProperty(record => record.TicketId, (Guid?)null));
            await context.Tickets.Where(record => record.Id == ticketId).ExecuteDeleteAsync();
        }
    }

    public async Task<List<string>> GetFindingRulesAsync(FindingFilter filter)
    {
        var query = FindingFilterAsQueryable(filter);
        if (string.IsNullOrEmpty(filter.Category))
        {
            query = query.Where(finding => finding.Category == filter.Category);
        }
        return await query.Where(record => record.RuleId != null)
            .GroupBy(record => record.RuleId)
            .Select(group => group.Key!).ToListAsync();
    }

    public async Task<List<string>> GetFindingCategoriesAsync(FindingFilter filter)
    {
        
        var query = FindingFilterAsQueryable(filter);
        return await query.Where(record => record.Category != null)
            .GroupBy(record => record.Category)
            .Select(group => group.Key!).ToListAsync();
    }

    private IQueryable<Findings> FindingFilterAsQueryable(FindingFilter filter)
    {
        var query = context.Findings.AsQueryable();
        if (filter.ProjectId != null)
        {
            query = query.Where(finding => finding.ProjectId == filter.ProjectId);
        }

        if (filter.SourceControlId != null)
        {
            query = query.Where(finding => context.Projects.Any(project => project.Id == finding.ProjectId &&
                project.SourceControlId == filter.SourceControlId));
        }

        if (filter.CommitId != null)
        {
            query = query.Where(finding =>
                context.ScanFindings.Any(record =>
                    record.FindingId == finding.Id &&
                    record.Scan!.CommitId == filter.CommitId
                )
            );
        }

        if (filter.Status is { Count: > 0 })
        {
            if (filter.CommitId != null)
            {
                query = query.Where(finding => 
                    context.ScanFindings.Any(record => 
                        record.FindingId == finding.Id && 
                        filter.Status.Contains(record.Status)
                    )
                );
            }
            else
            {
                query = query.Where(finding => filter.Status.Contains(finding.Status));
            }
        }

        if (filter.Scanner is { Count: > 0 })
        {
            query = query.Where(finding => filter.Scanner.Contains(finding.ScannerId));
        }

        if (filter.Severity is { Count: > 0 })
        {
            query = query.Where(finding => filter.Severity.Contains(finding.Severity));
        }
        
        if (filter.StartCreatedAt != null)
        {
            query = query.Where(finding => finding.CreatedAt >= filter.StartCreatedAt);
        }

        if (filter.EndCreatedAt != null)
        {
            query = query.Where(finding => finding.CreatedAt <= filter.EndCreatedAt);
        }

        if (filter.StartFixedAt != null)
        {
            query = query.Where(finding => finding.FixedAt != null && finding.FixedAt >= filter.StartFixedAt);
        }
        if (filter.EndFixedAt != null)
        {
            query = query.Where(finding => finding.FixedAt != null && finding.FixedAt <= filter.EndFixedAt);
        }

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(finding => finding.Name.Contains(filter.Name));
        }
        
        if (filter.ProjectManagerId != null)
        {
            query = query.Where(finding => context.ProjectUsers.Any(record =>
                record.Role == ProjectRole.Manager 
                && finding.ProjectId == record.ProjectId
                && record.UserId == filter.ProjectManagerId)
            );
        }

        return query;
    }

    protected override bool HasPermission(Findings entity, string action)
    {
        var currentUser = CurrentUser();
        if (currentUser.HasClaim(PermissionType.Finding, action)) return true;

        var member = context.ProjectUsers.FirstOrDefault(record =>
            record.ProjectId == entity.ProjectId && record.UserId == currentUser.Id);
        if (member == null) return false;

        return action switch
        {
            PermissionAction.Read => true,
            PermissionAction.Update => member.Role is ProjectRole.Manager or ProjectRole.Validator,
            _ => false
        };
    }

    protected override async Task<Findings> FindByIdAsync(Guid id)
    {
        return await context.Findings.FirstOrDefaultAsync(record => record.Id == id)
               ?? throw new BadRequestException("finding not found");
    }
}