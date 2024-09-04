using Microsoft.AspNetCore.Mvc;
using TetraLeagueOverlay.Api;

namespace TetraLeagueOverlay.Controllers;

public class ZenithController : BaseController
{
    public ZenithController(TetrioApi api) : base(api) { }

    [HttpGet]
    public ActionResult<string> Get()
    {
        return Ok("This Endpoint is for Quick Play Overlays");
    }

    [HttpGet]
    [Route("{username}")]
    public async Task<ActionResult> StaticImage(string username, string? textcolor = null, string? backgroundColor = null, bool displayUsername = true)
    {
        username = username.ToLower();

        var stats = await _api.GetZenithStats(username);

        MemoryStream? notFoundImage = null;

        switch (stats)
        {
            case null:
                notFoundImage = ImageGenerator.GenerateUserNotFound(username);

                return File(notFoundImage.ToArray(), "image/png");
            default:
            {
                if (stats.Record == null)
                {
                    notFoundImage = ImageGenerator.GenerateUserNotFound(username);

                    return File(notFoundImage.ToArray(), "image/png");
                }

                var statsImage = await ImageGenerator.GenerateZenithImage(username, stats, textcolor, backgroundColor, displayUsername);

                return File(statsImage.ToArray(), "image/png");
            }
        }
    }

    [HttpGet]
    [Route("{username}/web")]
    public async Task<ActionResult> Web(string username, string? textcolor = null, string? backgroundColor = null, bool? displayUsernamer = true)
    {
        username = username.ToLower();

        var html = await System.IO.File.ReadAllTextAsync("Web/index.html");

        html = html.Replace("{username}", username);
        html = html.Replace("{textColor}", textcolor ?? "FFFFFF");
        html = html.Replace("{backgroundColor}", backgroundColor ?? "00FFFFFF");

        return Content(html, "text/html");
    }
}