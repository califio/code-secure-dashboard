using CodeSecure.Core.Entity;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Rule;

public interface IListScannerRuleHandler : IOutputHandler<List<Scanners>>;
public class ListScannerRuleHandler(AppDbContext context): IListScannerRuleHandler
{
    public async Task<Result<List<Scanners>>> HandleAsync()
    {
        var scanners = await context.Rules.GroupBy(rule => rule.ScannerId).Select(group => group.Key).ToListAsync();
        return context.Scanners.Where(scanner => scanners.Contains(scanner.Id)).ToList();
    }
}