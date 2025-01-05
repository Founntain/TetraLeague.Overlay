using Microsoft.AspNetCore.Mvc;
using TetraLeague.Overlay.Network.Api;
using TetraLeague.Overlay.Network.Api.Models;

namespace TetraLeague.Overlay.Controllers;

public class TourneyController : MinControllerBase
{
    public TourneyController(TetrioApi api) : base(api) { }

    [HttpGet]
    [Produces("text/plain")]
    public async Task<ActionResult> Seeder(string usernames, string? toprank = null )
    {
        var toprankValue = RankHelper.GetRankAsNumber(toprank ?? "x+");
        var usernamesArray = usernames.ToLower().Split(',');

        var validUsers = new List<(string, double)>();
        var unrankedUsers = new List<(string, double)>();
        var topRankOutedUsers = new List<(string, double)>();

        foreach (var username in usernamesArray)
        {
            var userData = await _api.GetUserSummaries(username);

            if (userData?.TetraLeague == null)
            {
                unrankedUsers.Add((username, 0));
                continue;
            }

            var userTopRank = RankHelper.GetRankAsNumber(userData.TetraLeague.TopRank ?? "z");

            if ( userTopRank > toprankValue )
            {
                topRankOutedUsers.Add((username, userData.TetraLeague.Tr ?? 0));
            }
            else if ( userTopRank == -1 || userData.TetraLeague.Rank == null || userData.TetraLeague.Rank == "z")
            {
                unrankedUsers.Add((username, 0));
            }
            else
            {
                validUsers.Add((username, userData.TetraLeague.Tr ?? 0));
            }
        }

        validUsers = validUsers.OrderByDescending(x => x.Item2).ToList();

        var outputString = string.Empty;

        outputString += string.Join(Environment.NewLine, validUsers.Select(x => x.Item1));

        outputString += Environment.NewLine;
        outputString += Environment.NewLine;

        outputString += "Unranked Players:";
        outputString += Environment.NewLine;
        outputString += string.Join(Environment.NewLine, unrankedUsers.Select(x => x.Item1));

        outputString += Environment.NewLine;
        outputString += Environment.NewLine;

        outputString += "Players above toprank:";
        outputString += Environment.NewLine;
        outputString += string.Join(Environment.NewLine, topRankOutedUsers.Select(x => x.Item1));


        return Ok(outputString);
    }
}