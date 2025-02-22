using CodeSecure.Api.Rule.Model;
using CodeSecure.Database;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Api.Rule.Service;

public class RuleService(AppDbContext context) : IRuleService
{
    public Task<List<string>> GetRuleIdAsync(RuleFilter filter)
    {
        var query = context.Findings
            .Where(finding => finding.RuleId != null);
        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(finding => finding.RuleId!.Contains(finding.Name));
        }

        if (filter.ProjectId != null)
        {
            query = query.Where(finding => finding.ProjectId == filter.ProjectId);
        }

        return query
            .GroupBy(finding => finding.RuleId)
            .Select(finding => finding.Key!).ToListAsync();
    }
}