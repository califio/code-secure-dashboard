using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Token;

public interface IDeleteTokenHandler : IHandler<Guid, bool>;

public class DeleteTokenHandler(AppDbContext context) : IDeleteTokenHandler
{
    public async Task<Result<bool>> HandleAsync(Guid request)
    {
        var token = await context.CiTokens.FirstOrDefaultAsync(token => token.Id == request);
        if (token == null)
        {
            return Result.Fail("Token not found");
        }

        context.CiTokens.Remove(token);
        await context.SaveChangesAsync();
        return true;
    }
}