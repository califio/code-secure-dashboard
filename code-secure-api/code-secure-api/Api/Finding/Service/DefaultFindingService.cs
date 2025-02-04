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
using CodeSecure.Manager.Integration.TicketTracker;
using CodeSecure.Manager.Integration.TicketTracker.Jira;
using CodeSecure.Manager.Scanner;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Api.Finding.Service;

public class DefaultFindingService(
    AppDbContext context,
    IHttpContextAccessor contextAccessor,
    IScannerManager scannerManager,
    IFindingManager findingManager,
    JiraTicketTracker jiraTicketTracker,
    ILogger<DefaultFindingService> logger
) : BaseService<Findings>(contextAccessor), IFindingService
{
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
                Action = record.Scan.Commit.Action,
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
            Ticket = ticket
        };
    }

    public async Task<FindingDetail> UpdateFindingAsync(Guid findingId, UpdateFindingRequest request)
    {
        var finding = await FindByIdAsync(findingId);
        if (!HasPermission(finding, PermissionAction.Update)) throw new AccessDeniedException();

        if (request.Status != null && request.Status != finding.Status)
        {
            // can't change fixed finding to other status
            if (finding.Status == FindingStatus.Fixed)
                throw new BadRequestException("the finding was fixed. can't change status");

            // fixed status will auto update by CI
            if (request.Status != FindingStatus.Fixed)
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
                await context.ScanFindings
                    .Where(record => record.FindingId == findingId)
                    .ExecuteUpdateAsync(setter => setter.SetProperty(record => record.Status, (FindingStatus)request.Status));
                finding.Status = (FindingStatus)request.Status;
            }
        }

        if (request.FixDeadline != null && request.FixDeadline != finding.FixDeadline)
        {
            if (request.FixDeadline < DateTime.UtcNow) throw new BadRequestException("Fix deadline invalid");

            if (finding.Status != FindingStatus.Confirmed) throw new BadRequestException("Can't set fix deadline");

            var activity = FindingActivities.ChangeDeadline(
                CurrentUser().Id,
                findingId,
                finding.FixDeadline,
                request.FixDeadline
            );
            context.FindingActivities.Add(activity);
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

    public async Task<Page<FindingActivity>> GetFindingActivitiesAsync(Guid id, QueryFilter filter)
    {
        var finding = await FindByIdAsync(id);
        if (!HasPermission(finding, PermissionAction.Read)) throw new AccessDeniedException();

        var result = await context.FindingActivities
            .Include(activity => activity.User)
            .Where(activity => activity.FindingId == finding.Id)
            .Select(activity => new FindingActivity
            {
                UserId = activity.UserId,
                Username = activity.User!.UserName!,
                Fullname = activity.User!.FullName,
                Avatar = activity.User!.Avatar,
                Comment = activity.Comment,
                Type = activity.Type,
                MetadataString = activity.Metadata,
                Metadata = null,
                CreatedAt = activity.CreatedAt
            })
            .OrderBy(nameof(FindingActivities.CreatedAt), filter.Desc)
            .PageAsync(filter.Page, filter.Size);
        foreach (var item in result.Items)
        {
            if (string.IsNullOrEmpty(item.MetadataString)) continue;
            try
            {
                item.Metadata = JSONSerializer.Deserialize<FindingActivityMetadata>(item.MetadataString);
            }
            catch (System.Exception e)
            {
                item.Metadata = null;
                logger.LogError(e.Message);
            }

            item.MetadataString = null;
        }

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
            Metadata = null,
            MetadataString = null,
            Fullname = string.Empty,
            Avatar = null
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