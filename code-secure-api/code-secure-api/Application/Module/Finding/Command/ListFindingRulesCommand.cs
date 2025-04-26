using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Authentication.Jwt;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding.Command;


public class ListFindingRulesCommand(AppDbContext context, JwtUserClaims currentUser)
{
    public async Task<Result<List<string>>> ExecuteAsync(FindingFilter request)
    {
        request.RuleId = null;
        return await context.Findings
            .Where(record => record.RuleId != null)
            .FindingFilter(context, currentUser, request)
            .GroupBy(record => record.RuleId)
            .Select(group => group.Key!).ToListAsync();
    }
}