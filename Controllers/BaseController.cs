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

    [HttpGet]
    [Route("{username}/web")]
    public async Task<ActionResult> Web(string username, string? textcolor = null, string? backgroundColor = null, bool displayUsername = true)
    {
        username = username.ToLower();

        var html = await System.IO.File.ReadAllTextAsync("Web/index.html");

        html = html.Replace("{mode}", ControllerContext.ActionDescriptor.ControllerName);

        html = html.Replace("{username}", username);
        html = html.Replace("{textColor}", textcolor ?? "FFFFFF");
        html = html.Replace("{backgroundColor}", backgroundColor ?? "00FFFFFF");
        html = html.Replace("{displayUsername}", displayUsername.ToString());

        return Content(html, "text/html");
    }
}