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
    public async Task<ActionResult> StaticImage(string username, string? textColor = null, string? backgroundColor = null, bool displayUsername = true)
    {
        username = username.ToLower();

        var stats = await _api.GetZenithStats(username);
        var expert = await _api.GetZenithStats(username, true);

        MemoryStream? notFoundImage = null;

        switch (stats)
        {
            case null:
                notFoundImage = ImageGenerator.GenerateUserNotFound();

                return File(notFoundImage.ToArray(), "image/png");
            default:
            {
                if (stats.Record == null)
                {
                    // If the user didn't play this week yet, but played before we show the pb instead
                    if (stats.Record == null && stats.Best?.Record != null)
                    {
                        stats.Record = stats.Best.Record;
                    }
                    else
                    {
                        notFoundImage = ImageGenerator.GenerateUserNotFound();

                        return File(notFoundImage.ToArray(), "image/png");
                    }
                }

                // Same for expert as well
                if (expert?.Record == null && expert?.Best?.Record != null)
                {
                    expert.Record = expert.Best.Record;
                }

                var statsImage = ImageGenerator.GenerateZenithImage(username, stats, expert, textColor, backgroundColor, displayUsername);

                return File(statsImage.ToArray(), "image/png");
            }
        }
    }

    [HttpGet]
    [Route("splits/{username}")]
    public async Task<ActionResult> Split(string username, string? textColor = null, string? backgroundColor = null, bool displayUsername = true)
    {
        username = username.ToLower();

        var normal = await _api.GetRecentZenithRecords(username);
        var normalPb = await _api.GetZenithStats(username);

        var expert = await _api.GetRecentZenithRecords(username, true);
        var expertPb = await _api.GetZenithStats(username);

        MemoryStream? notFoundImage = null;

        switch (normal)
        {
            case null:
                notFoundImage = ImageGenerator.GenerateUserNotFound();

                return File(notFoundImage.ToArray(), "image/png");
            default:
            {
                if (normal.Entries?.Any() == false)
                {
                    // If the user didn't play this week yet, but played before we show the pb instead
                    notFoundImage = ImageGenerator.GenerateUserNotFound();

                    return File(notFoundImage.ToArray(), "image/png");

                }

                var statsImage = ImageGenerator.GenerateZenithSplitsImage(username, normal, normalPb, textColor, backgroundColor, displayUsername);

                return File(statsImage.ToArray(), "image/png");
            }
        }
    }

    [HttpGet]
    [Route("splits/{username}/web")]
    public async Task<ActionResult> SplitsWeb(string username, string? textcolor = null, string? backgroundColor = null, bool displayUsername = true)
    {
        username = username.ToLower();

        var html = await System.IO.File.ReadAllTextAsync("Web/overlay.html");

        html = html.Replace("{mode}", ControllerContext.ActionDescriptor.ControllerName + "/splits");

        html = html.Replace("{username}", username);
        html = html.Replace("{textColor}", textcolor ?? "FFFFFF");
        html = html.Replace("{backgroundColor}", backgroundColor ?? "00FFFFFF");
        html = html.Replace("{displayUsername}", displayUsername.ToString());

        return Content(html, "text/html");
    }
}