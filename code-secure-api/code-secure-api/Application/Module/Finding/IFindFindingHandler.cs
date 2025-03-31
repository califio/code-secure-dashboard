using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Core.EntityFramework;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding;

public interface IFindFindingHandler : IHandler<FindingFilter, Page<FindingSummary>>;

public class FindFindingHandler(AppDbContext context) : IFindFindingHandler
{
    public async Task<Result<Page<FindingSummary>>> HandleAsync(FindingFilter request)
    {
        return await context.Findings
            .Include(finding => finding.Scanner)
            .FindingFilter(context, request)
            .Distinct()
            .OrderBy(request.SortBy.ToString(), request.Desc)
            .Select(finding => new FindingSummary
            {
                Id = finding.Id,
                Identity = finding.Identity,
                Name = finding.Name,
                Status = finding.Status,
                Severity = finding.Severity,
                Scanner = finding.Scanner!.Name,
                Type = finding.Scanner.Type
            })
            .PageAsync(request.Page, request.Size);
    }
}