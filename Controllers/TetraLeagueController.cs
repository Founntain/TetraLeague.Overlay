using Microsoft.AspNetCore.Mvc;
using TetraLeagueOverlay.Api;

namespace TetraLeagueOverlay.Controllers;

[ApiController]
[Produces("application/json")]
[Route("[controller]")]
public class TetraLeagueController : ControllerBase
{
    private readonly TetraLeagueApi _api;

    public TetraLeagueController(TetraLeagueApi api)
    {
        _api = api;
    }

    [HttpGet]
    public ActionResult<string> Get()
    {
        return Ok("Tetra League API");
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
                return NotFound($"No stats found for {username}");
            default:
            {
                var statsImage = ImageGenerator.GenerateTetraLeagueStatsImage(username, stats, textcolor, backgroundColor);

                return File(statsImage.ToArray(), "image/png");
            }
        }
    }

    [HttpGet]
    [Route("stats/{username}/web")]
    public async Task<ActionResult> Web(string username, string? textcolor = null, string? backgroundColor = null)
    {
        username = username.ToLower();

        var html = await System.IO.File.ReadAllTextAsync("Web/index.html");

        html = html.Replace("{username}", username);
        html = html.Replace("{textColor}", textcolor ?? "FFFFFF");
        html = html.Replace("{backgroundColor}", backgroundColor ?? "00FFFFFF");

        return Content(html, "text/html");
    }
}