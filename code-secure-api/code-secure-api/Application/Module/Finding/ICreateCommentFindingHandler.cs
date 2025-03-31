using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding;

public interface ICreateCommentFindingHandler: IHandler<CreateCommentFindingRequest, FindingActivity>;

public class CreateCommentFindingHandler(AppDbContext context) : ICreateCommentFindingHandler
{
    public async Task<Result<FindingActivity>> HandleAsync(CreateCommentFindingRequest request)
    {
        var finding = await context.Findings.FirstOrDefaultAsync(finding => finding.Id == request.FindingId);
        if (finding == null)
        {
            return Result.Fail("Finding not found");
        }
        var commentActivity = FindingActivities.AddComment(request.CurrentUser.Id, request.FindingId, request.Comment);
        context.FindingActivities.Add(commentActivity);
        await context.SaveChangesAsync();
        return new FindingActivity
        {
            UserId = request.CurrentUser.Id,
            Username = request.CurrentUser.UserName,
            Comment = commentActivity.Comment,
            CreatedAt = commentActivity.CreatedAt,
            Type = FindingActivityType.Comment,
            Avatar = null,
        };
    }
}