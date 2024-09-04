using Microsoft.AspNetCore.Mvc;
using TetraLeagueOverlay.Api;

namespace TetraLeagueOverlay.Controllers;

[ApiController]
[Produces("application/json")]
[Route("[controller]")]
public class BaseController : ControllerBase
{
    protected readonly TetrioApi _api;

    public BaseController(TetrioApi api)
    {
        _api = api;
    }
}