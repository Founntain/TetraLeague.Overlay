using System.Text.Json;
using TetraLeagueOverlay.Api.Models;

namespace TetraLeagueOverlay.Api;

public class TetraLeagueApi : ApiBase
{
    public string Url => ApiBaseUrl + "users/{0}/summaries/league";

    public async Task<TetraLeague?> GetTetraLeagueStats(string username)
    {
        using var client = new HttpClient();

        var uri = new Uri(string.Format(Url, username));

        var response = await client.GetStringAsync(uri);

        var responseDeserialized = JsonSerializer.Deserialize<ApiResponse<TetraLeague>>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (responseDeserialized == null)

            return default;

        if (responseDeserialized.Success)
        {
            if (responseDeserialized.Data != default) return responseDeserialized.Data;
        }

        return default;
    }
}