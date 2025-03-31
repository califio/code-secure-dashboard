using CodeSecure.Core.Entity;
using CodeSecure.Core.Extension;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.SourceControl;

public static class SourceControlExtension
{
    public static async Task<Result<SourceControls>> CreateSourceControlsAsync(this AppDbContext context, SourceControls request)
    {
        var source = await context.SourceControls.FirstOrDefaultAsync(record => record.Type == request.Type && record.NormalizedUrl == request.Url.NormalizeUpper());
        if (source != null)
        {
            return source;
        }
        request.Id = Guid.NewGuid();
        request.NormalizedUrl = request.Url.NormalizeUpper();
        context.SourceControls.Add(request);
        await context.SaveChangesAsync();
        return request;
    }
    
    public static async Task<Result<SourceControls>> FindSourceControlsByIdAsync(this AppDbContext context, Guid id)
    {
        var source = await context.SourceControls.FirstOrDefaultAsync(x => x.Id == id);
        if (source != null)
        {
            return source;
        }
        return Result.Fail("SourceControl not found");
    }
}