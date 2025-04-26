using CodeSecure.Application.Module.SourceControl.Model;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.SourceControl;

public interface ISourceControlService
{
    Task<List<SourceControlSummary>> ListSourceControlAsync();
}

public class SourceControlService(AppDbContext context) : ISourceControlService
{
    public Task<List<SourceControlSummary>> ListSourceControlAsync()
    {
        return context.SourceControls
            .OrderByDescending(record => record.CreatedAt)
            .Select(record => new SourceControlSummary
            {
                Id = record.Id,
                Name = record.Url,
                Type = record.Type
            }).ToListAsync();
    }
}