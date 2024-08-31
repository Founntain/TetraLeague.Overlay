using Microsoft.AspNetCore.Mvc;
using SkiaSharp;
using TetraLeagueOverlay.Api;
using TetraLeagueOverlay.Api.Models;

namespace TetraLeagueOverlay.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
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
            case { Glicko: -1 } or { Tr: -1 }:
                return NotFound($"{username} is not yet ranked");
            default:
            {
                var statsImage = ImageGenerator.GenerateStatsImage(username, stats, textcolor, backgroundColor);

                return File(statsImage.ToArray(), "image/png");
            }
        }
    }
}