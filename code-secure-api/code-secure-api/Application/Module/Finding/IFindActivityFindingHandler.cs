using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding;

public interface IFindActivityFindingHandler : IHandler<ActivityFindingFilter, Page<FindingActivity>>;

public class FindActivityFindingHandler(AppDbContext context) : IFindActivityFindingHandler
{
    public async Task<Result<Page<FindingActivity>>> HandleAsync(ActivityFindingFilter request)
    {
        var finding = await context.Findings.FirstOrDefaultAsync(finding => finding.Id == request.FindingId);
        if (finding == null)
        {
            return Result.Fail("Finding not found");
        }

        return await context.FindingActivities
            .Include(activity => activity.User)
            .Include(activity => activity.Commit)
            .Where(activity => activity.FindingId == finding.Id)
            .OrderBy(nameof(FindingActivities.CreatedAt), request.Desc)
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
            .PageAsync(request.Page, request.Size);
    }
}