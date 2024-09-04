using System.Text.Json.Serialization;

namespace TetraLeagueOverlay.Api.Models;

public class ApiRecord
{
    [JsonPropertyName("record")] public Record? Record { get; set; }

    [JsonPropertyName("rank")] public int? Rank { get; set; }

    [JsonPropertyName("rank_local")] public int? RankLocal { get; set; }
}

public class Aggregatestats
{
    [JsonPropertyName("apm")] public int? Apm { get; set; }

    [JsonPropertyName("pps")] public double? Pps { get; set; }

    [JsonPropertyName("vsscore")] public int? Vsscore { get; set; }
}

public class Clears
{
    [JsonPropertyName("singles")] public int? Singles { get; set; }

    [JsonPropertyName("doubles")] public int? Doubles { get; set; }

    [JsonPropertyName("triples")] public int? Triples { get; set; }

    [JsonPropertyName("quads")] public int? Quads { get; set; }

    [JsonPropertyName("pentas")] public int? Pentas { get; set; }

    [JsonPropertyName("realtspins")] public int? Realtspins { get; set; }

    [JsonPropertyName("minitspins")] public int? Minitspins { get; set; }

    [JsonPropertyName("minitspinsingles")] public int? Minitspinsingles { get; set; }

    [JsonPropertyName("tspinsingles")] public int? Tspinsingles { get; set; }

    [JsonPropertyName("minitspindoubles")] public int? Minitspindoubles { get; set; }

    [JsonPropertyName("tspindoubles")] public int? Tspindoubles { get; set; }

    [JsonPropertyName("minitspintriples")] public int? Minitspintriples { get; set; }

    [JsonPropertyName("tspintriples")] public int? Tspintriples { get; set; }

    [JsonPropertyName("minitspinquads")] public int? Minitspinquads { get; set; }

    [JsonPropertyName("tspinquads")] public int? Tspinquads { get; set; }

    [JsonPropertyName("tspinpentas")] public int? Tspinpentas { get; set; }

    [JsonPropertyName("allclear")] public int? Allclear { get; set; }
}

public class Extras
{
    public object League { get; set; }
    public object Result { get; set; }
    public object Zenith { get; set; }
}

public class Finesse
{
    [JsonPropertyName("combo")] public int? Combo { get; set; }

    [JsonPropertyName("faults")] public int? Faults { get; set; }

    [JsonPropertyName("perfectpieces")] public int? Perfectpieces { get; set; }
}

public class Garbage
{
    [JsonPropertyName("sent")] public int? Sent { get; set; }

    [JsonPropertyName("sent_nomult")] public int? SentNomult { get; set; }

    [JsonPropertyName("maxspike")] public int? Maxspike { get; set; }

    [JsonPropertyName("maxspike_nomult")] public int? MaxspikeNomult { get; set; }

    [JsonPropertyName("received")] public int? Received { get; set; }

    [JsonPropertyName("attack")] public int? Attack { get; set; }

    [JsonPropertyName("cleared")] public int? Cleared { get; set; }
}

public class P
{
    [JsonPropertyName("pri")] public double? Pri { get; set; }

    [JsonPropertyName("sec")] public int? Sec { get; set; }

    [JsonPropertyName("ter")] public double? Ter { get; set; }
}

public class Record
{
    [JsonPropertyName("_id")] public string Id { get; set; }

    [JsonPropertyName("replayid")] public string Replayid { get; set; }

    [JsonPropertyName("stub")] public bool? Stub { get; set; }

    [JsonPropertyName("gamemode")] public string Gamemode { get; set; }

    [JsonPropertyName("pb")] public bool? Pb { get; set; }

    [JsonPropertyName("oncepb")] public bool? Oncepb { get; set; }

    [JsonPropertyName("ts")] public DateTime? Ts { get; set; }

    [JsonPropertyName("revolution")] public object Revolution { get; set; }

    [JsonPropertyName("user")] public User User { get; set; }

    [JsonPropertyName("otherusers")] public List<object> Otherusers { get; set; }

    [JsonPropertyName("leaderboards")] public List<string> Leaderboards { get; set; }

    [JsonPropertyName("results")] public Results Results { get; set; }

    [JsonPropertyName("extras")] public Extras Extras { get; set; }

    [JsonPropertyName("disputed")] public bool? Disputed { get; set; }

    [JsonPropertyName("p")] public P P { get; set; }
}

public class Results
{
    [JsonPropertyName("aggregatestats")] public Aggregatestats Aggregatestats { get; set; }

    [JsonPropertyName("stats")] public Stats Stats { get; set; }

    [JsonPropertyName("gameoverreason")] public string Gameoverreason { get; set; }
}

public class Stats
{
    [JsonPropertyName("lines")] public int? Lines { get; set; }

    [JsonPropertyName("level_lines")] public int? LevelLines { get; set; }

    [JsonPropertyName("level_lines_needed")]
    public int? LevelLinesNeeded { get; set; }

    [JsonPropertyName("inputs")] public int? Inputs { get; set; }

    [JsonPropertyName("holds")] public int? Holds { get; set; }

    [JsonPropertyName("score")] public int? Score { get; set; }

    [JsonPropertyName("zenlevel")] public int? Zenlevel { get; set; }

    [JsonPropertyName("zenprogress")] public int? Zenprogress { get; set; }

    [JsonPropertyName("level")] public int? Level { get; set; }

    [JsonPropertyName("combo")] public int? Combo { get; set; }

    [JsonPropertyName("topcombo")] public int? Topcombo { get; set; }

    [JsonPropertyName("combopower")] public int? Combopower { get; set; }

    [JsonPropertyName("btb")] public int? Btb { get; set; }

    [JsonPropertyName("topbtb")] public int? Topbtb { get; set; }

    [JsonPropertyName("btbpower")] public int? Btbpower { get; set; }

    [JsonPropertyName("tspins")] public int? Tspins { get; set; }

    [JsonPropertyName("piecesplaced")] public int? Piecesplaced { get; set; }

    [JsonPropertyName("clears")] public Clears Clears { get; set; }

    [JsonPropertyName("garbage")] public Garbage Garbage { get; set; }

    [JsonPropertyName("kills")] public int? Kills { get; set; }

    [JsonPropertyName("finesse")] public Finesse Finesse { get; set; }

    [JsonPropertyName("zenith")] public Zenith Zenith { get; set; }

    [JsonPropertyName("finaltime")] public double? Finaltime { get; set; }
}

public class User
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("username")] public string Username { get; set; }

    [JsonPropertyName("avatar_revision")] public long? AvatarRevision { get; set; }

    [JsonPropertyName("banner_revision")] public long? BannerRevision { get; set; }

    [JsonPropertyName("country")] public string Country { get; set; }

    [JsonPropertyName("supporter")] public bool? Supporter { get; set; }
}

public class Zenith
{
    [JsonPropertyName("altitude")] public int? Altitude { get; set; }

    [JsonPropertyName("rank")] public int? Rank { get; set; }

    [JsonPropertyName("peakrank")] public int? Peakrank { get; set; }

    [JsonPropertyName("avgrankpts")] public int? Avgrankpts { get; set; }

    [JsonPropertyName("floor")] public int? Floor { get; set; }

    [JsonPropertyName("targetingfactor")] public int? Targetingfactor { get; set; }

    [JsonPropertyName("targetinggrace")] public int? Targetinggrace { get; set; }

    [JsonPropertyName("totalbonus")] public int? Totalbonus { get; set; }

    [JsonPropertyName("revives")] public int? Revives { get; set; }

    [JsonPropertyName("revivesTotal")] public int? RevivesTotal { get; set; }

    [JsonPropertyName("speedrun")] public bool? Speedrun { get; set; }

    [JsonPropertyName("speedrun_seen")] public bool? SpeedrunSeen { get; set; }

    [JsonPropertyName("splits")] public List<int?> Splits { get; set; }
}