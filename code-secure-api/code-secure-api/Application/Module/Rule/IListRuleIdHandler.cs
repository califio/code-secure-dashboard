using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Rule;

public interface IListRuleIdHandler : IHandler<RuleFilter, List<string>>;

public class ListRuleIdHandler(AppDbContext context) : IListRuleIdHandler
{
    public async Task<Result<List<string>>> HandleAsync(RuleFilter request)
    {
        var query = context.Findings
            .Where(finding => finding.RuleId != null);
        if (!string.IsNullOrEmpty(request.Name))
        {
            query = query.Where(finding => finding.RuleId!.Contains(finding.Name));
        }

        if (request.ProjectId != null)
        {
            query = query.Where(finding => finding.ProjectId == request.ProjectId);
        }

        return await query
            .GroupBy(finding => finding.RuleId)
            .Select(finding => finding.Key!).ToListAsync();
    }
}