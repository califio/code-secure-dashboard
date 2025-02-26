using CodeSecure.Api.SourceControlSystem.Model;
using CodeSecure.Database;

namespace CodeSecure.Api.SourceControlSystem.Service;

public class SourceControlSystemService(AppDbContext context) : ISourceControlSystemService
{
    public List<SourceControl> GetSourceControlSystem()
    {
        var sourceControls = context.SourceControls.OrderByDescending(record => record.CreatedAt).ToList();
        return sourceControls.Select(record => new SourceControl
        {
            Id = record.Id,
            Name = record.Url,
            Type = record.Type
        }).ToList();
    }
}