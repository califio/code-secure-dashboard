using CodeSecure.Application.Module.SourceControl;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api.SourceControl;

public class SourceControlController(IListSourceControlHandler listSourceControlHandler) : BaseController
{
    [HttpGet]
    public async Task<List<SourceControlSummary>> GetSourceControlSystem()
    {
        var result = await listSourceControlHandler.HandleAsync();
        return result.Value;
    }
}