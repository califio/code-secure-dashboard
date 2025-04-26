using CodeSecure.Application.Module.SourceControl;
using CodeSecure.Application.Module.SourceControl.Model;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.SourceControl;

public class SourceControlController(ISourceControlService sourceControlService) : BaseController
{
    [HttpGet]
    public Task<List<SourceControlSummary>> GetSourceControlSystem()
    {
        return sourceControlService.ListSourceControlAsync();
    }
}