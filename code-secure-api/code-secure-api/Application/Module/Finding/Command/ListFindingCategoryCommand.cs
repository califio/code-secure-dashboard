using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Authentication.Jwt;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding.Command;

public class ListFindingCategoryCommand(AppDbContext context, JwtUserClaims currentUser)
{
    public async Task<Result<List<string>>> ExecuteAsync(FindingFilter filter)
    {
        filter.RuleId = null;
        filter.Category = null;
        return await context.Findings
            .Where(finding => finding.Category != null)
            .FindingFilter(context, currentUser, filter)
            .GroupBy(record => record.Category)
            .Select(group => group.Key!).ToListAsync();
    }
}