using CodeSecure.Core.Entity;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Rule.Command;

public class ListRuleScannerCommand(AppDbContext context)
{
    public async Task<Result<List<Scanners>>> ExecuteAsync()
    {
        var scanners = await context.Rules.GroupBy(rule => rule.ScannerId).Select(group => group.Key).ToListAsync();
        return context.Scanners.Where(scanner => scanners.Contains(scanner.Id)).ToList();
    }
}