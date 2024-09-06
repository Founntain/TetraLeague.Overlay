using System.Text.Json.Serialization;

namespace TetraLeagueOverlay.Api.Models;

public class ZenithRecords
{
    [JsonPropertyName("entries")] public IList<Record> Entries { get; set; }
}