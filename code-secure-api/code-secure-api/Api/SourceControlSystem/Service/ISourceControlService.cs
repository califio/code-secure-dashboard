using CodeSecure.Api.SourceControlSystem.Model;

namespace CodeSecure.Api.SourceControlSystem.Service;

public interface ISourceControlService
{
    List<SourceControl> GetSourceControlSystem();
}