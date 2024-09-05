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

                var statsImage = ImageGenerator.GenerateZenithImage(username, stats, expert, textcolor, backgroundColor, displayUsername);

                return File(statsImage.ToArray(), "image/png");
            }
        }
    }
}