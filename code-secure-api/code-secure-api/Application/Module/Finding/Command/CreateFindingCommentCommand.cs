using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding.Command;

public class CreateFindingCommentCommand(AppDbContext context, JwtUserClaims currentUser)
{
    public async Task<Result<FindingActivity>> ExecuteAsync(Guid findingId, string comment)
    {
        var finding = await context.Findings.FirstOrDefaultAsync(finding => finding.Id == findingId);
        if (finding == null)
        {
            return Result.Fail("Finding not found");
        }

        var commentActivity = FindingActivities.AddComment(currentUser.Id, findingId, comment);
        context.FindingActivities.Add(commentActivity);
        await context.SaveChangesAsync();
        return new FindingActivity
        {
            UserId = currentUser.Id,
            Username = currentUser.UserName,
            Comment = commentActivity.Comment,
            CreatedAt = commentActivity.CreatedAt,
            Type = FindingActivityType.Comment,
            Avatar = null,
        };
    }
}