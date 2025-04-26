using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Token.Command;

public class DeleteTokenCommand(AppDbContext context)
{
    public async Task<Result<bool>> ExecuteAsync(Guid tokenId)
    {
        var token = await context.CiTokens.FirstOrDefaultAsync(token => token.Id == tokenId);
        if (token == null)
        {
            return Result.Fail("Token not found");
        }

        context.CiTokens.Remove(token);
        await context.SaveChangesAsync();
        return true;
    }
}