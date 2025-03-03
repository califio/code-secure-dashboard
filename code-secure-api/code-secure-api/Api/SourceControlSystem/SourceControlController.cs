using CodeSecure.Api.SourceControlSystem.Model;
using CodeSecure.Api.SourceControlSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.SourceControlSystem;

public class SourceControlController(ISourceControlService sourceControlService) : BaseController
{
    [HttpGet]
    public List<SourceControl> GetSourceControlSystem()
    {
        return sourceControlService.GetSourceControlSystem();
    }
}