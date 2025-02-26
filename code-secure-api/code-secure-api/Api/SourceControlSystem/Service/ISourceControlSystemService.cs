using CodeSecure.Api.SourceControlSystem.Model;

namespace CodeSecure.Api.SourceControlSystem.Service;

public interface ISourceControlSystemService
{
    List<SourceControl> GetSourceControlSystem();
}