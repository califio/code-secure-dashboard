using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Core.EntityFramework;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding.Command;

public class GetFindingByFilterCommand(AppDbContext context, JwtUserClaims currentUser)
{
    public async Task<Result<Page<FindingSummary>>> ExecuteAsync(FindingFilter filter)
    {
        return await context.Findings
            .Include(finding => finding.Scanner)
            .FindingFilter(context, currentUser, filter)
            .Distinct()
            .OrderBy(filter.SortBy.ToString(), filter.Desc)
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
            .PageAsync(filter.Page, filter.Size);
    }
}