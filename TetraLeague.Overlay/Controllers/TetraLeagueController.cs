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

        var html = await System.IO.File.ReadAllTextAsync("wwwroot/web/league.html");

        html = html.Replace("{mode}", ControllerContext.ActionDescriptor.ControllerName);

        html = html.Replace("{username}", username);
        html = html.Replace("{textColor}", textcolor ?? "FFFFFF");
        html = html.Replace("{backgroundColor}", backgroundColor ?? "00FFFFFF");

        return Content(html, "text/html");

        // username = username.ToLower();
        //
        // var user = await _api.GetUserInformation(username);
        //
        // if (user == null)
        // {
        //     var notFoundImage = new BaseImageGenerator().GenerateUserNotFound();
        //
        //     return File(notFoundImage.ToArray(), "image/png");
        // }
        //
        // var stats = await _api.GetTetraLeagueStats(username);
        //
        // switch (stats)
        // {
        //     case null:
        //         var notFoundImage = new BaseImageGenerator().GenerateUserNotFound();
        //
        //         return File(notFoundImage.ToArray(), "image/png");
        //     default:
        //     {
        //         var statsImage = await new TetraLeagueImageGenerator().GenerateTetraLeagueImage(user, stats, textcolor, backgroundColor, displayUsername);
        //
        //         return File(statsImage.ToArray(), "image/png");
        //     }
        // }
    }

    [HttpGet]
    [Route("stats/{username}/web")]
    public async Task<ActionResult> WebAlt(string username, string? textcolor = null, string? backgroundColor = null)
    {
        return await StatsNew(username, textcolor, backgroundColor);
    }

    [HttpGet]
    [Route("{username}/stats")]
    public async Task<ActionResult> GetStats(string username)
    {
        username = username.ToLower();

        var user = await _api.GetUserInformation(username);
        var stats = await _api.GetTetraLeagueStats(username);

        return Ok(new
        {
            Username= user.Username,
            Country = user.Country,
            Tr = stats.Tr,
            Rank = stats.Rank,
            Apm = stats.Apm,
            Pps = stats.Pps,
            Vs = stats.Vs,
            GlobalRank = stats.StandingGlobal,
            CountryRank = stats.StandingLocal,
            TopRank = stats.TopRank,
            PrevRank = stats.PrevRank,
            PrevAt = stats.PrevAt,
            NextRank = stats.NextRank,
            NextAt = stats.NextAt,
            GamesPlayed = stats.Gamesplayed
        });
    }
}