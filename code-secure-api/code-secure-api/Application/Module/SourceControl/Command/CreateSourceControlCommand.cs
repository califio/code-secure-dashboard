using CodeSecure.Application.Module.SourceControl.Model;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Extension;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.SourceControl.Command;

public class CreateSourceControlCommand(AppDbContext context)
{
    public async Task<Result<SourceControls>> ExecuteAsync(CreateSourceControlRequest request)
    {
        var normalizedUrl = request.Url.NormalizeUpper();
        var source =
            await context.SourceControls.FirstOrDefaultAsync(x =>
                x.Type == request.Type && x.NormalizedUrl == normalizedUrl);
        if (source != null)
        {
            return source;
        }

        source = new SourceControls
        {
            Id = Guid.NewGuid(),
            Url = request.Url,
            NormalizedUrl = normalizedUrl,
            Type = request.Type,
        };
        context.SourceControls.Add(source);
        await context.SaveChangesAsync();
        return source;
    }
}