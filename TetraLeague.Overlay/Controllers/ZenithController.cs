using Microsoft.AspNetCore.Mvc;
using TetraLeague.Overlay.Generator;
using TetraLeague.Overlay.Network.Api;

namespace TetraLeague.Overlay.Controllers;

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

        var stats = await _api.GetUserSummaries(username);

        MemoryStream? notFoundImage;

        switch (stats)
        {
            case null:
                notFoundImage = new BaseImageGenerator().GenerateUserNotFound();

                return File(notFoundImage.ToArray(), "image/png");
            default:
            {
                if (stats.Zenith?.Record == null)
                {
                    // If the user didn't play this week yet, but played before we show the pb instead
                    if (stats.Zenith?.Record == null && stats.Zenith?.Best?.Record != null)
                    {
                        stats.Zenith.Record = stats.Zenith.Best.Record;
                    }
                    else
                    {
                        notFoundImage = new BaseImageGenerator().GenerateUserNotFound();

                        return File(notFoundImage.ToArray(), "image/png");
                    }
                }

                // Same for expert as well
                if (stats.ZenithExpert?.Record == null && stats.ZenithExpert?.Best?.Record != null)
                {
                    stats.ZenithExpert.Record = stats.ZenithExpert.Best.Record;
                }

                var statsImage = new ZenithImageGenerator().GenerateZenithImage(username, stats.Zenith, stats.ZenithExpert, textColor, backgroundColor, displayUsername);

                return File(statsImage.ToArray(), "image/png");
            }
        }
    }

    [HttpGet]
    [Route("splits/{username}")]
    public async Task<ActionResult> Split(string username, string? textColor = null, string? backgroundColor = null, bool displayUsername = true, bool expert = false)
    {
        username = username.ToLower();

        var recentGames = await _api.GetRecentZenithRecords(username, expert);
        var personalBest = await _api.GetZenithStats(username, expert);

        MemoryStream? notFoundImage;

        switch (recentGames)
        {
            case null:
                notFoundImage = new BaseImageGenerator().GenerateUserNotFound();

                return File(notFoundImage.ToArray(), "image/png");
            default:
            {
                if (recentGames.Entries.Any() == false || personalBest == null)
                {
                    // If the user didn't play this week yet, but played before we show the pb instead
                    notFoundImage = new BaseImageGenerator().GenerateUserNotFound();

                    return File(notFoundImage.ToArray(), "image/png");

                }

                var statsImage = new ZenithImageGenerator().GenerateZenithSplitsImage(username, recentGames, personalBest, textColor, backgroundColor, displayUsername);

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