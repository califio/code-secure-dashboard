using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Enum;
using CodeSecure.Manager.Scanner;
using CodeSecure.Manager.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

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

    public async Task<Findings> CreateAsync(Findings finding)
    {
        var findings = await FindByIdentityAsync(finding.ProjectId, finding.Identity);
        if (findings != null)
        {
            return findings;
        }

        finding.Id = Guid.NewGuid();
        if (await scannerManager.IsScaScanner(finding.ScannerId))
        {
            finding.Status = FindingStatus.Confirmed;
            if (finding.FixDeadline == null)
            {
                var sla = await GetSlaAsync(finding);
                if (sla > 0)
                {
                    finding.FixDeadline = DateTime.UtcNow.AddDays(sla);
                }
            }
        }

        context.Findings.Add(finding);
        await context.SaveChangesAsync();
        return finding;
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
        var setting = await settingManager.AppSettingAsync();
        SLA sla;
        if (await scannerManager.IsScaScanner(finding.ScannerId))
        {
            sla = setting.SlaScaSetting;
        }
        else
        {
            sla = setting.SlaSastSetting;
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