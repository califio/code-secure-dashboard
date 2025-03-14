using CodeSecure.Authentication;
using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Database.Extension;
using CodeSecure.Enum;
using CodeSecure.Manager.Finding.Model;
using CodeSecure.Manager.Scanner;
using CodeSecure.Manager.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using UserClaims = CodeSecure.Authentication.Jwt.UserClaims;

namespace CodeSecure.Manager.Finding;

public class FindingManager(
    AppDbContext context,
    IMemoryCache cache,
    IScannerManager scannerManager,
    ISettingManager settingManager) : IFindingManager
{
    private const int ExpiredTime = 1;

    public async Task<Findings?> FindByIdAsync(Guid id)
    {
        if (cache.TryGetValue(CacheKey(id), out Findings? finding))
        {
            return finding;
        }

        finding = await context.Findings.FirstOrDefaultAsync(record => record.Id == id);
        CacheFinding(CacheKey(id), finding);
        return finding;
    }

    public async Task<Findings?> FindByIdentityAsync(Guid projectId, string identity)
    {
        if (cache.TryGetValue(CacheKey(projectId, identity), out Findings? finding))
        {
            return finding;
        }

        finding = await context.Findings.FirstOrDefaultAsync(record =>
            record.Identity == identity && record.ProjectId == projectId);
        CacheFinding(CacheKey(projectId, identity), finding);
        return finding;
    }

    public async Task<Page<FindingSummary>> GetFindingAsync(FindingFilter filter, UserClaims actor)
    {
        var allowReadFinding = actor.HasClaim(PermissionType.Finding, PermissionAction.Read);

        var query = context.Findings
            .Include(finding => finding.Scanner)
            .Where(finding =>
                allowReadFinding == true ||
                context.ProjectUsers.Any(projectUser =>
                    projectUser.ProjectId == finding.ProjectId &&
                    projectUser.UserId == actor.Id
                )
            );
        if (filter.SourceControlId != null)
        {
            query = query.Where(finding => context.Projects.Any(record =>
                record.Id == finding.ProjectId && record.SourceControlId == filter.SourceControlId));
        }
        if (filter.ProjectId != null)
        {
            query = query.Where(finding => finding.ProjectId == filter.ProjectId);
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
        if (!string.IsNullOrEmpty(filter.Category))
        {
            query = query.Where(finding => finding.Category == filter.Category);
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

        return await query.Distinct()
            .OrderBy(filter.SortBy.ToString(), filter.Desc)
            .Select(finding => new FindingSummary
            {
                Id = finding.Id,
                Identity = finding.Identity,
                Name = finding.Name,
                Status = finding.Status,
                Severity = finding.Severity,
                Scanner = finding.Scanner!.Name,
                Type = finding.Scanner.Type
            })
            .PageAsync(filter.Page, filter.Size);
    }

    public async Task<Findings> CreateAsync(Findings finding)
    {
        try
        {
            var existFinding = await context.Findings.FirstOrDefaultAsync(record =>
                record.Identity == finding.Identity && record.ProjectId == finding.ProjectId);
            if (existFinding != null)
            {
                return existFinding;
            }
            finding.Id = Guid.NewGuid();
            context.Findings.Add(finding);
            await context.SaveChangesAsync();
            return finding;
        }
        catch (System.Exception)
        {
            return finding;
        }
    }

    public async Task<Findings> UpdateAsync(Findings finding)
    {
        context.Findings.Update(finding);
        await context.SaveChangesAsync();
        CacheFinding(CacheKey(finding.Id), finding);
        return finding;
    }

    public async Task<int> GetSlaAsync(Findings finding)
    {
        var severity = finding.Severity;
        SLA sla;
        if (await scannerManager.IsScaScanner(finding.ScannerId))
        {
            sla = await settingManager.GetSlaScaSettingAsync();
        }
        else
        {
            sla = await settingManager.GetSlaSastSettingAsync();
        }

        if (severity == FindingSeverity.Critical && sla.Critical > 0) return sla.Critical;
        if (severity == FindingSeverity.High && sla.High > 0) return sla.High;
        if (severity == FindingSeverity.Medium && sla.Medium > 0) return sla.Medium;
        if (severity == FindingSeverity.Low && sla.Low > 0) return sla.Low;
        if (severity == FindingSeverity.Info && sla.Info > 0) return sla.Info;
        return 0;
    }

    private void CacheFinding(string key, Findings? finding)
    {
        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(ExpiredTime));
        cache.Set(key, finding, options);
    }

    private string CacheKey(Guid projectId, string identity)
    {
        return $"finding_identity:{projectId}_{identity}";
    }

    private string CacheKey(Guid id)
    {
        return $"finding_id:{id.ToString()}";
    }
}