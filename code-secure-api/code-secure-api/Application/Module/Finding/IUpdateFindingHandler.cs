using CodeSecure.Application.Exceptions;
using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Application.Module.Scanner;
using CodeSecure.Application.Module.Setting;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Extension;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding;

public interface IUpdateFindingHandler : IHandler<UpdateFindingRequest, Findings>;

public class UpdateFindingHandler(AppDbContext context) : IUpdateFindingHandler
{
    public async Task<Result<Findings>> HandleAsync(UpdateFindingRequest request)
    {
        var finding = await context.Findings.FirstOrDefaultAsync(finding => finding.Id == request.FindingId);
        if (finding == null)
        {
            return Result.Fail("Finding not found");
        }

        if (request.Status != null && request.Status != finding.Status)
        {
            var activity = FindingActivities.ChangeStatus(
                request.CurrentUserId,
                finding.Id,
                finding.Status,
                (FindingStatus)request.Status
            );
            context.FindingActivities.Add(activity);
            await context.SaveChangesAsync();
            // auto change fix deadline base on SLA when change status to confirmed
            if (finding.FixDeadline == null && request.Status == FindingStatus.Confirmed)
            {
                var sla = await GetSlaAsync(finding);
                if (sla > 0)
                {
                    finding.FixDeadline = DateTime.UtcNow.AddDays(sla);
                    activity = FindingActivities.ChangeDeadline(
                        null,
                        finding.Id,
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
                    .Where(record => record.FindingId == finding.Id && record.Status == finding.Status)
                    .ExecuteUpdateAsync(setter => setter
                        .SetProperty(record => record.Status, (FindingStatus)request.Status)
                        .SetProperty(record => record.FixedAt, DateTime.UtcNow)
                    );
                finding.FixedAt = DateTime.UtcNow;
            }
            else
            {
                await context.ScanFindings
                    .Where(record => record.FindingId == finding.Id && record.Status == finding.Status)
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
                request.CurrentUserId,
                finding.Id,
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
        context.Findings.Update(finding);
        await context.SaveChangesAsync();
        return finding;
    }
    
    public async Task<int> GetSlaAsync(Findings finding)
    {
        var severity = finding.Severity;
        var setting = await context.GetSlaSettingAsync();
        SLA sla;
        var scanner = (await context.FindScannerByIdAsync(finding.ScannerId)).GetResult();
        if (scanner.Type == ScannerType.Dependency || scanner.Type == ScannerType.Container)
        {
            sla = setting.Sca;
        }
        else
        {
            sla = setting.Sast;
        }

        if (severity == FindingSeverity.Critical && sla.Critical > 0) return sla.Critical;
        if (severity == FindingSeverity.High && sla.High > 0) return sla.High;
        if (severity == FindingSeverity.Medium && sla.Medium > 0) return sla.Medium;
        if (severity == FindingSeverity.Low && sla.Low > 0) return sla.Low;
        if (severity == FindingSeverity.Info && sla.Info > 0) return sla.Info;
        return 0;
    }
}