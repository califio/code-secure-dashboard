using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeSecure.Api;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
[Consumes("application/json")]
public abstract class BaseController : Controller
{
    
}