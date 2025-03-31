using CodeSecure.Core.Entity;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Token;

public interface IListTokenHandler : IOutputHandler<List<CiTokens>>;

public class ListTokenHandler(AppDbContext context) : IListTokenHandler
{
    public async Task<Result<List<CiTokens>>> HandleAsync()
    {
        return await context.CiTokens.OrderByDescending(record => record.CreatedAt).ToListAsync();
    }
}