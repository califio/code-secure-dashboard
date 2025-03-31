using CodeSecure.Application.Module.Finding.Model;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding;

public interface IListFindingRulesHandler : IHandler<FindingFilter, List<string>>;

public class ListFindingRulesHandler(AppDbContext context) : IListFindingRulesHandler
{
    public async Task<Result<List<string>>> HandleAsync(FindingFilter request)
    {
        request.RuleId = null;
        return await context.Findings
            .Where(record => record.RuleId != null)
            .FindingFilter(context, request)
            .GroupBy(record => record.RuleId)
            .Select(group => group.Key!).ToListAsync();
    }
}