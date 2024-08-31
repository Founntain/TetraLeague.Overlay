using System.Text.Json.Serialization;

namespace TetraLeagueOverlay.Api;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public object Error { get; set; }
    public CacheResponse Cache { get; set; }
    public T? Data { get; set; }
}

public class CacheResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; }
    [JsonPropertyName("cached_at")]
    public long CachedAt { get; set; }
    [JsonPropertyName("cache_until")]
    public long CacheUntil { get; set; }
}