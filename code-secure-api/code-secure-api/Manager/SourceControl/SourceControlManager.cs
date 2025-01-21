using CodeSecure.Database;
using CodeSecure.Database.Entity;
using CodeSecure.Enum;
using CodeSecure.Extension;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Manager.SourceControl;

public class SourceControlManager(AppDbContext context): ISourceControlManager
{
    private static readonly SemaphoreSlim Lock = new(1, 1);
    public async Task<SourceControls> CreateOrUpdateAsync(SourceControls sourceControl)
    {
        await Lock.WaitAsync();
        try
        {
            var source = await FindByTypeAndUrlAsync(sourceControl.Type, sourceControl.Url);
            if (source != null)
            {
                return source;
            }

            sourceControl.Id = Guid.NewGuid();
            sourceControl.NormalizedUrl = sourceControl.Url.NormalizeUpper();
            context.SourceControls.Add(sourceControl);
            await context.SaveChangesAsync();
            return sourceControl;
        }
        finally
        {
            Lock.Release();
        }
    }

    public async Task<SourceControls?> FindByTypeAndUrlAsync(SourceType type, string url)
    {
        return await context.SourceControls.FirstOrDefaultAsync(record =>
            record.Type == type && record.NormalizedUrl == url.NormalizeUpper());
    }
}