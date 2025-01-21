using CodeSecure.Database.Entity;
using CodeSecure.Enum;

namespace CodeSecure.Manager.SourceControl;

public interface ISourceControlManager
{
    Task<SourceControls> CreateOrUpdateAsync(SourceControls sourceControl);
    Task<SourceControls?> FindByTypeAndUrlAsync(SourceType type, string url);
}