using System.Collections.Concurrent;
using System.Text.Json;
using TetraLeagueOverlay.Api.Models;

namespace TetraLeagueOverlay.Api;

public class TetrioApi : ApiBase
{
    private static ConcurrentDictionary<string, (DateTimeOffset, TetraLeague?)> _leagueCache = new();
    private static ConcurrentDictionary<string, (DateTimeOffset, Sprint?)> _sprintCache = new();
    private static ConcurrentDictionary<string, (DateTimeOffset, Blitz?)> _blitzCache = new();
    private static ConcurrentDictionary<string, (DateTimeOffset, QuickPlay?)> _zenithCache = new();
    private static ConcurrentDictionary<string, (DateTimeOffset, QuickPlay?)> _zenithExpertCache = new();

    private string LeagueUrl => ApiBaseUrl + "users/{0}/summaries/league";
    private string SprintUrl => ApiBaseUrl + "users/{0}/summaries/40l";
    private string BlitzUrl => ApiBaseUrl + "users/{0}/summaries/blitz";
    private string ZenithUrl => ApiBaseUrl + "users/{0}/summaries/zenith";
    private string ZenithExpertUrl => ApiBaseUrl + "users/{0}/summaries/zenithex";

    public async Task<TetraLeague?> GetTetraLeagueStats(string username)
    {
        // Let's check the cache first
        if (_leagueCache.TryGetValue(username, out var data))
        {
            Console.WriteLine($"[TL] Found {username} in cache");

            // If the cache is still valid we return that
            if (data.Item1 >= DateTimeOffset.UtcNow)
            {
                Console.WriteLine("[TL] Found valid cache data to return");

                return data.Item2;
            }
        }

        Console.WriteLine($"[TL] Getting league stats for {username}, as nothing was found in the cache");

        try
        {
            var responseFromApi = await GetString(string.Format(LeagueUrl, username));

            if (responseFromApi == null) return default;

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<TetraLeague>>(responseFromApi, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null) return default;
            if (!apiResponse.Success) return default;

            if (apiResponse.Cache.Status == "hit")
            {
                Console.WriteLine("[TL] Cache hit... returning cache");

                _leagueCache.TryGetValue(username, out var result);

                if(result.Item2 != null) return result.Item2;

                Console.WriteLine("[TL] Cache hit, but nothing in there, fetching data again...");
            }

            if (apiResponse.Data == default) return default;

            Console.WriteLine("[TL] Updating cache and returning");

            // var cacheValidUntil = DateTimeOffset.FromUnixTimeMilliseconds(apiResponse.Cache.CacheUntil);
            var cacheValidUntil = DateTimeOffset.UtcNow.AddSeconds(30);

            if (_leagueCache.ContainsKey(username))
            {
                _leagueCache[username] = (cacheValidUntil, apiResponse.Data);

                return _leagueCache[username].Item2;
            }

            _leagueCache.TryAdd(username, (cacheValidUntil, apiResponse.Data));

            return apiResponse.Data;
        }
        catch (Exception _)
        {
            return default;
        }
    }

    public async Task<Sprint?> GetSprintStats(string username)
    {
        // Let's check the cache first
        if (_sprintCache.TryGetValue(username, out var data))
        {
            Console.WriteLine($"[40L] Found {username} in cache");

            // If the cache is still valid we return that
            if (data.Item1 >= DateTimeOffset.UtcNow)
            {
                Console.WriteLine("[40L] Found valid cache data to return");

                return data.Item2;
            }
        }

        Console.WriteLine($"[40L] Getting 40L stats for {username}, as nothing was found in the cache");

        try
        {
            var responseFromApi = await GetString(string.Format(SprintUrl, username));

            if (responseFromApi == null) return default;

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Sprint>>(responseFromApi, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null) return default;
            if (!apiResponse.Success) return default;

            if (apiResponse.Cache.Status == "hit")
            {
                Console.WriteLine("[40L] Cache hit... returning cache");

                _sprintCache.TryGetValue(username, out var result);

                if(result.Item2 != null) return result.Item2;

                Console.WriteLine("[40L] Cache hit, but nothing in there, fetching data again...");
            }

            if (apiResponse.Data == default) return default;

            Console.WriteLine("[40L] Updating cache and returning");

            var cacheValidUntil = DateTimeOffset.FromUnixTimeMilliseconds(apiResponse.Cache.CacheUntil);

            if (_sprintCache.ContainsKey(username))
            {
                _sprintCache[username] = (cacheValidUntil, apiResponse.Data);

                return _sprintCache[username].Item2;
            }

            _sprintCache.TryAdd(username, (cacheValidUntil, apiResponse.Data));

            return apiResponse.Data;
        }
        catch (Exception _)
        {
            return default;
        }
    }

    public async Task<Blitz?> GetBlitzStats(string username)
    {
        // Let's check the cache first
        if (_blitzCache.TryGetValue(username, out var data))
        {
            Console.WriteLine($"[BLITZ] Found {username} in cache");

            // If the cache is still valid we return that
            if (data.Item1 >= DateTimeOffset.UtcNow)
            {
                Console.WriteLine("[BLITZ] Found valid cache data to return");

                return data.Item2;
            }
        }

        Console.WriteLine($"[BLITZ] Getting Blitz stats for {username}, as nothing was found in the cache");

        try
        {
            var responseFromApi = await GetString(string.Format(BlitzUrl, username));

            if (responseFromApi == null) return default;

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Blitz>>(responseFromApi, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null) return default;
            if (!apiResponse.Success) return default;

            if (apiResponse.Cache.Status == "hit")
            {
                Console.WriteLine("[BLITZ] Cache hit... returning cache");

                _blitzCache.TryGetValue(username, out var result);

                if(result.Item2 != null) return result.Item2;

                Console.WriteLine("[BLITZ] Cache hit, but nothing in there, fetching data again...");
            }

            if (apiResponse.Data == default) return default;

            Console.WriteLine("[BLITZ] Updating cache and returning");

            var cacheValidUntil = DateTimeOffset.FromUnixTimeMilliseconds(apiResponse.Cache.CacheUntil);

            if (_blitzCache.ContainsKey(username))
            {
                _blitzCache[username] = (cacheValidUntil, apiResponse.Data);

                return _blitzCache[username].Item2;
            }

            _blitzCache.TryAdd(username, (cacheValidUntil, apiResponse.Data));

            return apiResponse.Data;
        }
        catch (Exception _)
        {
            return default;
        }
    }

     public async Task<QuickPlay?> GetZenithStats(string username, bool expert = false)
     {
         var prefix = expert ? "QP EX" : "QP";
             ;
        // Let's check the cache first
        if (!expert && _zenithCache.TryGetValue(username, out var normalData))
        {
            Console.WriteLine($"[{prefix}] Found {username} in cache");

            // If the cache is still valid we return that
            if (normalData.Item1 >= DateTimeOffset.UtcNow)
            {
                Console.WriteLine($"[{prefix}] Found valid cache data to return");

                return normalData.Item2;
            }
        }

        if (expert && _zenithExpertCache.TryGetValue(username, out var expertData))
        {
            Console.WriteLine($"[{prefix}] Found {username} in cache");

            // If the cache is still valid we return that
            if (expertData.Item1 >= DateTimeOffset.UtcNow)
            {
                Console.WriteLine($"[{prefix}] Found valid cache data to return");

                return expertData.Item2;
            }
        }

        Console.WriteLine($"[{prefix}] Getting Zenith stats for {username}, as nothing was found in the cache");

        try
        {
            var responseFromApi = await GetString(string.Format(expert ? ZenithExpertUrl : ZenithUrl, username));

            if (responseFromApi == null) return default;

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<QuickPlay>>(responseFromApi, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse == null) return default;
            if (!apiResponse.Success) return default;

            if (apiResponse.Cache.Status == "hit")
            {
                Console.WriteLine($"[{prefix}] Cache hit... returning cache");

                if (!expert)
                {
                    _zenithCache.TryGetValue(username, out var result);

                    if(result.Item2 != null) return result.Item2;
                }
                else
                {
                    _zenithExpertCache.TryGetValue(username, out var result);

                    if(result.Item2 != null) return result.Item2;
                }

                Console.WriteLine($"[{prefix}] Cache hit, but nothing in there, fetching data again...");
            }

            if (apiResponse.Data == default) return default;

            Console.WriteLine($"[{prefix}] Updating cache and returning");

            // var cacheValidUntil = DateTimeOffset.FromUnixTimeMilliseconds(apiResponse.Cache.CacheUntil);
            var cacheValidUntil = DateTimeOffset.UtcNow.AddSeconds(30);

            if (!expert)
            {
                if (_zenithCache.ContainsKey(username))
                {
                    _zenithCache[username] = (cacheValidUntil, apiResponse.Data);

                    return _zenithCache[username].Item2;
                }

                _zenithCache.TryAdd(username, (cacheValidUntil, apiResponse.Data));
            }
            else
            {
                if (_zenithExpertCache.ContainsKey(username))
                {
                    _zenithExpertCache[username] = (cacheValidUntil, apiResponse.Data);

                    return _zenithExpertCache[username].Item2;
                }

                _zenithExpertCache.TryAdd(username, (cacheValidUntil, apiResponse.Data));
            }

            return apiResponse.Data;
        }
        catch (Exception _)
        {
            return default;
        }
    }
}