using Microsoft.AspNetCore.Mvc;
using TetraLeagueOverlay.Api;

namespace TetraLeagueOverlay.Controllers;

[Route("[controller]")]
public class BlitzController : BaseController
{
    public BlitzController(TetrioApi api) : base(api) { }

    [HttpGet]
    public ActionResult<string> Get()
    {
        return Ok("This Endpoint is for Blitz Overlays");
    }

    [HttpGet]
    [Route("{username}")]
    public async Task<ActionResult> StaticImage(string username, string? textcolor = null, string? backgroundColor = null, bool displayUsername = true)
    {
        username = username.ToLower();

        var stats = await _api.GetBlitzStats(username);

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

                var statsImage = ImageGenerator.GenerateBlitzImage(username, stats, textcolor, backgroundColor, displayUsername);

                return File(statsImage.ToArray(), "image/png");
            }
        }
    }
}