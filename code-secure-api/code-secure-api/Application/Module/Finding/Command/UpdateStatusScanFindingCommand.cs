using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding.Command;

public class UpdateStatusScanFindingCommand(AppDbContext context, JwtUserClaims currentUser)
{
    public async Task<Result<FindingStatus>> ExecuteAsync(Guid findingId, UpdateStatusScanFindingRequest request)
    {
        var finding = await context.Findings.FirstOrDefaultAsync(finding => finding.Id == findingId);
        if (finding == null)
        {
            return Result.Fail("Finding not found");
        }

        var scanFinding = await context.ScanFindings
            .Include(record => record.Scan)
            .FirstOrDefaultAsync(record => record.ScanId == request.ScanId && record.FindingId == findingId);
        if (scanFinding == null)
        {
            return Result.Fail("Scan not found");
        }

        if (scanFinding.Status != request.Status)
        {
            var commentActivity = FindingActivities.ChangeStatus(currentUser.Id, finding.Id, scanFinding.Status,
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