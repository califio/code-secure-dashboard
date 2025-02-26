using CodeSecure.Api.SourceControlSystem.Model;
using CodeSecure.Api.SourceControlSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.SourceControlSystem;

public class SourceControlSystemController(ISourceControlSystemService sourceControlSystemService) : BaseController
{
    [HttpGet]
    public List<SourceControl> GetSourceControlSystem()
    {
        return sourceControlSystemService.GetSourceControlSystem();
    }
}