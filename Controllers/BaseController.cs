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

        var html = await System.IO.File.ReadAllTextAsync("Web/overlay.html");

        html = html.Replace("{mode}", ControllerContext.ActionDescriptor.ControllerName);

        html = html.Replace("{username}", username);
        html = html.Replace("{textColor}", textcolor ?? "FFFFFF");
        html = html.Replace("{backgroundColor}", backgroundColor ?? "00FFFFFF");
        html = html.Replace("{displayUsername}", displayUsername.ToString());

        return Content(html, "text/html");
    }

    [HttpGet]
    [Route("{username}/slide")]
    public async Task<ActionResult> Slide(string username, string? textcolor = null, string? backgroundColor = null, bool displayUsername = false, string? modes = null)
    {
        if (modes == null)
        {
            modes = "tetraleague,zenith,sprint,blitz";
        }

        var hasValidSeperator = modes.Contains(',');

        var validation = modes.Split(',');

        if (validation.Length == 0 || !hasValidSeperator)
        {
            return File(ImageGenerator.GenerateErrorImage("VALIDATION ERROR", "Please make sure you provide modes as CSV:", "modes=tetraleague,zenith,40l").ToArray(), "image/png");
        }

        username = username.ToLower();

        var html = await System.IO.File.ReadAllTextAsync("Web/slide.html");

        html = html.Replace("{username}", username);
        html = html.Replace("{modes}", modes);
        html = html.Replace("{textColor}", textcolor ?? "FFFFFF");
        html = html.Replace("{backgroundColor}", backgroundColor ?? "00FFFFFF");
        html = html.Replace("{displayUsername}", displayUsername.ToString());

        return Content(html, "text/html");
    }
}