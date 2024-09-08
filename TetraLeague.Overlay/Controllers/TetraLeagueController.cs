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
    public async Task<ActionResult> Stats(string username, string? textcolor = null, string? backgroundColor = null)
    {
        username = username.ToLower();

        var stats = await _api.GetTetraLeagueStats(username);

        switch (stats)
        {
            case null:
                var notFoundImage = new BaseImageGenerator().GenerateUserNotFound();

                return File(notFoundImage.ToArray(), "image/png");
            default:
            {
                var statsImage = new TetraLeagueImageGenerator().GenerateTetraLeagueImage(username, stats, textcolor, backgroundColor);

                return File(statsImage.ToArray(), "image/png");
            }
        }
    }

    [HttpGet]
    [Route("{username}")]
    public async Task<ActionResult> StatsAlt(string username, string? textcolor = null, string? backgroundColor = null)
    {
        return await Stats(username, textcolor, backgroundColor);
    }

    [HttpGet]
    [Route("stats/{username}/web")]
    public async Task<ActionResult> WebAlt(string username, string? textcolor = null, string? backgroundColor = null)
    {
        return await Web(username, textcolor, backgroundColor);
    }
}