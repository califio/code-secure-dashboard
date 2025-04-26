using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding.Command;

public class GetFindingActivityCommand(AppDbContext context)
{
    public async Task<Result<Page<FindingActivity>>> ExecuteAsync(Guid findingId, QueryFilter filter)
    {
        var finding = await context.Findings.FirstOrDefaultAsync(finding => finding.Id == findingId);
        if (finding == null)
        {
            return Result.Fail("Finding not found");
        }

        return await context.FindingActivities
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
    }
}