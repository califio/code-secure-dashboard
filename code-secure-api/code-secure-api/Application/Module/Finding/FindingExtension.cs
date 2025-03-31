using CodeSecure.Core.Entity;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding;

public static class FindingExtension
{
    public static async Task<Result<Findings>> CreateFindingAsync(this AppDbContext context, Findings finding)
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
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }
}