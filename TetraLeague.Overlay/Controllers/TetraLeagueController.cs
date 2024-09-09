using Microsoft.AspNetCore.Mvc;
using TetraLeague.Overlay.Generator;
using TetraLeague.Overlay.Network.Api;

namespace TetraLeague.Overlay.Controllers;

public class TetraLeagueController : BaseController
{
    public TetraLeagueController(TetrioApi api) : base(api) { }

    [HttpGet]
    public ActionResult<string> Get()
    {
        return Ok("This Endpoint is for Tetra League Overlays");
    }

    [HttpGet]
    [Route("stats/{username}")]
    public async Task<ActionResult> Stats(string username, string? textcolor = null, string? backgroundColor = null, bool displayUsername = true)
    {
        return await StatsNew(username, textcolor, backgroundColor, displayUsername);
    }

    [HttpGet]
    [Route("{username}")]
    public async Task<ActionResult> StatsNew(string username, string? textcolor = null, string? backgroundColor = null, bool displayUsername = true)
    {
        username = username.ToLower();

        var user = await _api.GetUserInformation(username);

        if (user == null)
        {
            var notFoundImage = new BaseImageGenerator().GenerateUserNotFound();

            return File(notFoundImage.ToArray(), "image/png");
        }

        var stats = await _api.GetTetraLeagueStats(username);

        switch (stats)
        {
            case null:
                var notFoundImage = new BaseImageGenerator().GenerateUserNotFound();

                return File(notFoundImage.ToArray(), "image/png");
            default:
            {
                var statsImage = await new TetraLeagueImageGenerator().GenerateTetraLeagueImage(user, stats, textcolor, backgroundColor, displayUsername);

                return File(statsImage.ToArray(), "image/png");
            }
        }
    }

    [HttpGet]
    [Route("stats/{username}/web")]
    public async Task<ActionResult> WebAlt(string username, string? textcolor = null, string? backgroundColor = null)
    {
        return await Web(username, textcolor, backgroundColor);
    }
}