using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.SourceControl;

public interface IListSourceControlHandler : IOutputHandler<List<SourceControlSummary>>;

public class ListSourceControlHandler(AppDbContext context) : IListSourceControlHandler
{
    public async Task<Result<List<SourceControlSummary>>> HandleAsync()
    {
        return await context.SourceControls
            .OrderByDescending(record => record.CreatedAt)
            .Select(record => new SourceControlSummary
            {
                Id = record.Id,
                Name = record.Url,
                Type = record.Type
            }).ToListAsync();
    }
}