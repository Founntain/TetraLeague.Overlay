using System.Collections.Concurrent;
using System.Text.Json;
using TetraLeagueOverlay.Api.Models;

namespace TetraLeagueOverlay.Api;

public class TetraLeagueApi : ApiBase
{
    private static ConcurrentDictionary<string, (DateTimeOffset, TetraLeague?)> _cache = new();

    private string Url => ApiBaseUrl + "users/{0}/summaries/league";

    public async Task<TetraLeague?> GetTetraLeagueStats(string username)
    {
        // Let's check the cache first
        if (_cache.TryGetValue(username, out var data))
        {
            Console.WriteLine($"Found {username} in cache");

            // If the cache is still valid we return that
            if (data.Item1 >= DateTimeOffset.UtcNow)
            {
                Console.WriteLine("League stats in cache found and still valid");

                return data.Item2;
            }
        }

        Console.WriteLine($"Getting league stats for {username}, as nothing was found in the cache");

        try
        {
            using var client = new HttpClient();

            var uri = new Uri(string.Format(Url, username));

            client.DefaultRequestHeaders.Add("X-Session-ID", Guid.NewGuid().ToString());

            var responseFromApi = await client.GetStringAsync(uri);
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<TetraLeague>>(responseFromApi, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null) return default;
            if (!apiResponse.Success) return default;

            if (apiResponse.Cache.Status == "hit")
            {
                Console.WriteLine("We hit the cache again, so we return that instead");

                _cache.TryGetValue(username, out var result);

                if(result.Item2 != null) return result.Item2;

                Console.WriteLine("We hit the cache, but we don't have something stored in there. So we update it.");
            }

            if (apiResponse.Data == default) return default;

            Console.WriteLine("Updating cache and returning");

            var cacheValidUntil = DateTimeOffset.UtcNow.AddSeconds(30);

            if (_cache.ContainsKey(username))
            {
                // _cache[username] = (DateTimeOffset.FromUnixTimeMilliseconds(apiResponse.Cache.CacheUntil), apiResponse.Data);
                _cache[username] = (cacheValidUntil, apiResponse.Data);

                return _cache[username].Item2;
            }

            // _cache.TryAdd(username, (DateTimeOffset.FromUnixTimeMilliseconds(apiResponse.Cache.CacheUntil), apiResponse.Data));
            _cache.TryAdd(username, (cacheValidUntil, apiResponse.Data));

            return apiResponse.Data;
        }
        catch (Exception _)
        {
            return default;
        }
    }
}