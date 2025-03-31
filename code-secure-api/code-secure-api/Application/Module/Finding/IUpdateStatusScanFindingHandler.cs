using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding;

public interface IUpdateStatusScanFindingHandler : IHandler<UpdateStatusScanFindingRequest, FindingStatus>;

public class UpdateStatusScanFindingHandler(AppDbContext context) : IUpdateStatusScanFindingHandler
{
    public async Task<Result<FindingStatus>> HandleAsync(UpdateStatusScanFindingRequest request)
    {
        var finding = await context.Findings.FirstOrDefaultAsync(finding => finding.Id == request.FindingId);
        if (finding == null)
        {
            return Result.Fail("Finding not found");
        }

        var scanFinding = await context.ScanFindings
            .Include(record => record.Scan)
            .FirstOrDefaultAsync(record => record.ScanId == request.ScanId && record.FindingId == request.FindingId);
        if (scanFinding == null)
        {
            return Result.Fail("Scan not found");
        }

        if (scanFinding.Status != request.Status)
        {
            var commentActivity = FindingActivities.ChangeStatus(request.CurrentUserId, finding.Id, scanFinding.Status,
                request.Status, scanFinding.Scan!.CommitId);
            context.FindingActivities.Add(commentActivity);
            scanFinding.Status = request.Status;
            if (request.Status == FindingStatus.Fixed)
            {
                scanFinding.FixedAt = DateTime.UtcNow;
            }

            context.ScanFindings.Update(scanFinding);
            await context.SaveChangesAsync();
        }

        return scanFinding.Status;
    }
}