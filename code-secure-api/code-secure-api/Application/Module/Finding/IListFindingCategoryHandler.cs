using CodeSecure.Application.Module.Finding.Model;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding;

public interface IListFindingCategoryHandler : IHandler<FindingFilter, List<string>>;

public class ListFindingCategoryHandler(AppDbContext context) : IListFindingCategoryHandler
{
    public async Task<Result<List<string>>> HandleAsync(FindingFilter request)
    {
        request.RuleId = null;
        request.Category = null;
        return await context.Findings
            .Where(finding => finding.Category != null)
            .FindingFilter(context, request)
            .GroupBy(record => record.Category)
            .Select(group => group.Key!).ToListAsync();
    }
}