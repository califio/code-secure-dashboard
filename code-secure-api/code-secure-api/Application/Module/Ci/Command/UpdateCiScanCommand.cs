using CodeSecure.Application.Module.Ci.Model;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Ci.Command;

public class UpdateCiScanCommand(AppDbContext context)
{
    public async Task<Result<bool>> ExecuteAsync(Guid scanId, UpdateCiScanRequest request)
    {
        var scan = await context.Scans.FirstOrDefaultAsync(scan => scan.Id == scanId);
        if (scan == null)
        {
            return Result.Fail("Scan not found");
        }

        if (request.Status != null && request.Status != scan.Status)
        {
            scan.Status = (ScanStatus)request.Status;
            if (request.Status == ScanStatus.Completed) scan.CompletedAt = DateTime.UtcNow;
        }

        if (!string.IsNullOrEmpty(request.Description))
        {
            if (request.Description.Length > 1024) request.Description = request.Description.Substring(0, 1024);

            scan.Description = request.Description;
        }

        context.Scans.Update(scan);
        await context.SaveChangesAsync();
        return true;
    }
}