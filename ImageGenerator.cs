using SkiaSharp;
using TetraLeagueOverlay.Api.Models;

namespace TetraLeagueOverlay;

/// <summary>
/// Provides functionality for generating images based on TetraLeague statistics.
/// </summary>
public static class ImageGenerator
{
    /// <summary>
    /// Generates an image containing the statistics for a specified user.
    /// </summary>
    /// <param name="username">The username of the player whose statistics are to be displayed.</param>
    /// <param name="stats">An object containing the player's TetraLeague statistics.</param>
    /// <param name="textColor">Optional. The color of the text in the image. Defaults to null.</param>
    /// <param name="backgroundColor">Optional. The background color of the image. Defaults to null.</param>
    /// <returns>A memory stream containing the generated image with the player's statistics.</returns>
    public static MemoryStream GenerateTetraLeagueImage(string username, TetraLeague stats, string? textColor = null, string? backgroundColor = null)
    {
        var width = 900;
        var height = 300;
        var stream = new MemoryStream();
        var typeFace = SKTypeface.FromFile("Resources/cr.ttf");

        var unranked = stats.Gamesplayed < 10;

        #region Paints

        textColor ??= "FFFFFF";

        var bigTextPaint = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 64,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        var bigTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 64,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        var normalTextPaint = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Left,
        };

        var normalTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Left,
        };

        var smallTextPaint = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 20,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Right,
        };

        var smallTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 20,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Right,
        };

        var statsPaintAlt = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        var progressBarBg = new SKPaint
        {
            Color = SKColor.Parse("DD000000"),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        var progressBarBgAlt = new SKPaint
        {
            Color = SKColor.Parse($"FF{textColor}"),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        #endregion

        using var surface = SKSurface.Create(new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Unpremul));

        SetBackground(surface, width, height, backgroundColor);

        var rankBitmap = unranked ? GetBitmap($"Resources/z.png") : GetBitmap($"Resources/{stats.Rank}.png");
        var toprankBitmap = unranked ? GetBitmap($"Resources/z.png") : GetBitmap($"Resources/{stats.TopRank}.png");

        var prevRank = stats.PrevRank == null ? null : GetBitmap($"Resources/{stats.PrevRank}.png");
        var nextRank = stats.NextRank == null ? GetBitmap("Resources/leaderboard1.png") : GetBitmap($"Resources/{stats.NextRank}.png");

        // Big Rank Letter
        surface.Canvas.DrawBitmap(ResizeBitmap(rankBitmap, 200, 200), 40, 40);

        // Username
        DrawTextWithShadow(surface, username.ToUpper(), 275, 75, bigTextPaint, bigTextShadowPaint);

        // TR
        if(!unranked)
            DrawTextWithShadow(surface, $"{stats.Tr:#.##} TR", 275, 140, bigTextPaint, bigTextShadowPaint);
        else
            DrawTextWithShadow(surface, $"{stats.Gamesplayed} / 10 placements", 275, 140, bigTextPaint, bigTextShadowPaint);

        // Stats
        surface.Canvas.DrawRect(462, 162, 3, 85, normalTextPaint);

        // Left Side
        var apm = stats.Apm ?? 0;
        var pps = stats.Pps ?? 0;
        var vs = stats.Vs ?? 0;

        if (stats.Gamesplayed > 0)
        {
            DrawTextWithShadow(surface, $"{apm}", 275, 185, normalTextPaint, normalTextShadowPaint); DrawTextWithShadow(surface, $"APM", 385, 185, statsPaintAlt, normalTextShadowPaint);
            DrawTextWithShadow(surface, $"{pps}", 275, 215, normalTextPaint, normalTextShadowPaint); DrawTextWithShadow(surface, $"PPS", 385, 215, statsPaintAlt, normalTextShadowPaint);
            DrawTextWithShadow(surface, $"{vs}", 275, 245, normalTextPaint, normalTextShadowPaint); DrawTextWithShadow(surface, $"VS", 385, 245, statsPaintAlt, normalTextShadowPaint);

            // Right Side
            var globalRank = stats.StandingGlobal == -1 ? "UNRANKED" : $"# {stats.StandingGlobal!.Value}";
            var localRank = unranked ? "UNRANKED" : stats.StandingLocal!.Value == -1 ? "HIDDEN" : $"# {stats.StandingLocal!.Value}";
            DrawTextWithShadow(surface, $"GLOBAL", 475, 185, normalTextPaint, normalTextShadowPaint); DrawTextWithShadow(surface, globalRank, 630, 185, normalTextPaint, normalTextShadowPaint);
           DrawTextWithShadow(surface, $"COUNTRY", 475, 215, normalTextPaint, normalTextShadowPaint); DrawTextWithShadow(surface, localRank, 630, 215, normalTextPaint, normalTextShadowPaint);

           if (!unranked)
           {
               DrawTextWithShadow(surface, $"TOP RANK", 475, 245, normalTextPaint, normalTextShadowPaint); surface.Canvas.DrawBitmap(ResizeBitmap(toprankBitmap, 32, 32), 630, 218);
           }
        }

        // Progressbar
        if (stats.PrevAt.HasValue && stats.NextAt.HasValue && stats.StandingGlobal.HasValue && !unranked)
        {
            double range = stats.PrevAt.Value - stats.NextAt.Value;
            double distance = stats.PrevAt.Value - stats.StandingGlobal.Value;
            double rankPercentage = distance / range * 100;

            var highlight = (int)(rankPercentage / 100 * (width - 200));

            if (highlight < 0) highlight = 0;

            highlight = highlight > width-200 ? width - 200 : highlight;

            surface.Canvas.DrawRect(100, 275, width - 200, 10, progressBarBg);
            surface.Canvas.DrawRect(100, 275, highlight, 10, progressBarBgAlt);

            if(stats.PrevRank != null && prevRank != null)
                surface.Canvas.DrawBitmap(ResizeBitmap(prevRank, 32, 32), 60, 265);

            if (stats.Rank == "d" && stats.PrevAt != -1)
            {
                DrawTextWithShadow(surface, $"#{stats.PrevAt}", 90, 286, smallTextPaint, smallTextShadowPaint);
            }

            if (stats.NextRank == null )
                surface.Canvas.DrawBitmap(ResizeBitmap(nextRank, 32, 32), width - 90, 262);
            else
                surface.Canvas.DrawBitmap(ResizeBitmap(nextRank, 32, 32), width - 90, 265);
        }

        using var data = surface.Snapshot().Encode(SKEncodedImageFormat.Png, 80);

        data.SaveTo(stream);

        return stream;
    }

    public static MemoryStream GenerateUserNotFound()
    {
        var width = 900;
        var height = 300;
        var stream = new MemoryStream();
        var typeFace = SKTypeface.FromFile("Resources/cr.ttf");

        #region Paints

        var bigTextPaint = new SKPaint
        {
            Color = SKColor.Parse("5a6e49"),
            Style = SKPaintStyle.Fill,
            TextSize = 64,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var bigTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 64,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var normalTextPaint = new SKPaint
        {
            Color = SKColor.Parse("8bca95"),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var normalTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        #endregion

        using var surface = SKSurface.Create(new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Unpremul));

        var errorBitmap = GetBitmap("Resources/error.png");

        SetBackground(surface, width, height, "0f160d");

        DrawTextWithShadow(surface, "No such user / record", (float) width / 2, 60, bigTextPaint, bigTextShadowPaint);
        DrawTextWithShadow(surface, $"Either you mistyped something", (float) width / 2, 100, normalTextPaint, normalTextShadowPaint);
        DrawTextWithShadow(surface, $"or the account no longer exists.", (float) width / 2, 130, normalTextPaint, normalTextShadowPaint);

        surface.Canvas.DrawBitmap(errorBitmap, (float) ((width / 2) - (errorBitmap.Width / 2)), 140);

        using var data = surface.Snapshot().Encode(SKEncodedImageFormat.Png, 80);

        data.SaveTo(stream);

        return stream;
    }

    public static MemoryStream GenerateErrorImage(string title, string? subText1 = null, string? subText2 = null)
    {
        var width = 900;
        var height = 300;
        var stream = new MemoryStream();
        var typeFace = SKTypeface.FromFile("Resources/cr.ttf");

        #region Paints

        var bigTextPaint = new SKPaint
        {
            Color = SKColor.Parse("5a6e49"),
            Style = SKPaintStyle.Fill,
            TextSize = 64,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var bigTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 64,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var normalTextPaint = new SKPaint
        {
            Color = SKColor.Parse("8bca95"),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var normalTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        #endregion

        using var surface = SKSurface.Create(new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Unpremul));

        var errorBitmap = GetBitmap("Resources/error.png");

        SetBackground(surface, width, height, "0f160d");

        DrawTextWithShadow(surface, title, (float) width / 2, 60, bigTextPaint, bigTextShadowPaint);
        DrawTextWithShadow(surface, subText1, (float) width / 2, 100, normalTextPaint, normalTextShadowPaint);
        DrawTextWithShadow(surface, subText2, (float) width / 2, 130, normalTextPaint, normalTextShadowPaint);

        surface.Canvas.DrawBitmap(errorBitmap, (float) ((width / 2) - (errorBitmap.Width / 2)), 140);

        using var data = surface.Snapshot().Encode(SKEncodedImageFormat.Png, 80);

        data.SaveTo(stream);

        return stream;
    }

    public static MemoryStream GenerateSprintImage(string username, Sprint stats, string? textColor, string? backgroundColor, bool displayUsername = true)
    {
        var width = 700;
        var height = 225;

        var center = (float) width / 2;

        var stream = new MemoryStream();

        var typeFace = SKTypeface.FromFile("Resources/cr.ttf");

        #region Paints

        textColor ??= "FFFFFF";

        var bigTextPaint = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 64,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var bigTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 64,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var normalTextPaint = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var normalTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var smallTextPaint = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 20,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Right,
        };

        var smallTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 20,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Right,
        };

        var statsPaintAlt = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        var progressBarBg = new SKPaint
        {
            Color = SKColor.Parse("DD000000"),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        var progressBarBgAlt = new SKPaint
        {
            Color = SKColor.Parse($"FF{textColor}"),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        #endregion

        using var surface = SKSurface.Create(new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Unpremul));

        SetBackground(surface, width, height, backgroundColor);

        var offset = 0;

        if (displayUsername)
        {
            DrawTextWithShadow(surface, username.ToUpper(), center, 65, bigTextPaint, bigTextShadowPaint);
        }
        else
        {
            DrawTextWithShadow(surface, "40L", center, 65, bigTextPaint, bigTextShadowPaint);
        }

        offset += 60;

        var pps = $"{stats.Record!.Results.Aggregatestats.Pps!.Value:#.##}  PPS";
        var kpp = $"{(double) stats.Record.Results.Stats.Inputs! / (double) stats.Record.Results.Stats.Piecesplaced!:F2}  KPP";
        var kps = $"{stats.Record.Results.Stats.Inputs / (stats.Record.Results.Stats.Finaltime / 1000):F2}  KPS";
        var finesse = $"{stats.Record.Results.Stats.Finesse.Faults}F";

        // TIME
        DrawTextWithShadow(surface, TimeSpan.FromMilliseconds(Math.Round(stats.Record.Results.Stats.Finaltime!.Value, MidpointRounding.ToEven)).ToString(@"m\:ss\.fff"), center, 65 + offset, bigTextPaint, bigTextShadowPaint);
        // STATS
        DrawTextWithShadow(surface, $"{pps} | {kpp} | {kps} | {finesse}", center, 105 + offset, normalTextPaint, normalTextShadowPaint);
        // PLACEMENTS
        DrawTextWithShadow(surface, $"# {stats.Rank} | # {stats.RankLocal}", center, 140 + offset, normalTextPaint, normalTextShadowPaint);

        using var data = surface.Snapshot().Encode(SKEncodedImageFormat.Png, 80);

        data.SaveTo(stream);

        return stream;
    }

    public static MemoryStream GenerateBlitzImage(string username, Blitz stats, string? textColor, string? backgroundColor, bool displayUsername = true)
    {
        var width = 700;
        var height = 225;

        var center = (float) width / 2;

        var stream = new MemoryStream();

        var typeFace = SKTypeface.FromFile("Resources/cr.ttf");

        #region Paints

        textColor ??= "FFFFFF";

        var bigTextPaint = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 64,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var bigTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 64,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var normalTextPaint = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var normalTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var smallTextPaint = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 20,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Right,
        };

        var smallTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 20,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Right,
        };

        var statsPaintAlt = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        var progressBarBg = new SKPaint
        {
            Color = SKColor.Parse("DD000000"),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        var progressBarBgAlt = new SKPaint
        {
            Color = SKColor.Parse($"FF{textColor}"),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        #endregion

        using var surface = SKSurface.Create(new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Unpremul));

        SetBackground(surface, width, height, backgroundColor);

        var offset = 0;

        if (displayUsername)
        {
            DrawTextWithShadow(surface, username.ToUpper(), center, 65, bigTextPaint, bigTextShadowPaint);
        }
        else
        {
            DrawTextWithShadow(surface, "BLITZ", center, 65, bigTextPaint, bigTextShadowPaint);
        }

        offset += 60;

        var pps = $"{stats.Record!.Results.Aggregatestats.Pps!.Value:#.##}  PPS";
        var kpp = $"{(double) stats.Record.Results.Stats.Inputs! / (double) stats.Record.Results.Stats.Piecesplaced!:F2}  KPP";
        var sps = $"{(double) stats.Record.Results.Stats.Score! / (double) stats.Record.Results.Stats.Piecesplaced:F2}  SPS";
        var finesse = $"{(stats.Record.Results.Stats.Finesse?.Faults.ToString() ?? "na")} F";

        // SCORE
        DrawTextWithShadow(surface, stats.Record.Results.Stats.Score!.Value.ToString("###,###")!, center, 65 + offset, bigTextPaint, bigTextShadowPaint);
        // STATS
        DrawTextWithShadow(surface, $"{pps} | {kpp} | {sps} | {finesse}", center, 105 + offset, normalTextPaint, normalTextShadowPaint);
        // PLACEMENTS
        DrawTextWithShadow(surface, $"# {stats.Rank} | # {stats.RankLocal}", center, 140 + offset, normalTextPaint, normalTextShadowPaint);

        using var data = surface.Snapshot().Encode(SKEncodedImageFormat.Png, 80);

        data.SaveTo(stream);

        return stream;
    }

    public static MemoryStream GenerateZenithImage(string username, QuickPlay stats, QuickPlay? expert, string? textColor, string? backgroundColor, bool displayUsername)
    {
        var width = 900;
        var height = 300;
        var center = (float) width / 2;
        var stream = new MemoryStream();
        var typeFace = SKTypeface.FromFile("Resources/cr.ttf");

        #region Paints

        textColor ??= "FFFFFF";

        var bigTextPaint = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 64,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var bigTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 64,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var normalTextPaint = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var normalTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var smallTextPaint = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 20,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var smallTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 20,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var statsPaintAlt = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        var progressBarBg = new SKPaint
        {
            Color = SKColor.Parse("DD000000"),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        var progressBarBgAlt = new SKPaint
        {
            Color = SKColor.Parse($"FF{textColor}"),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        #endregion

        using var surface = SKSurface.Create(new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Unpremul));

        SetBackground(surface, width, height, backgroundColor);

        var offset = 0;

        // USERNAME
        if (displayUsername)
        {
            DrawTextWithShadow(surface, username.ToUpper(), center, 55, bigTextPaint, bigTextShadowPaint);
        }
        else
        {
            DrawTextWithShadow(surface, "QUICK PLAY", center, 55, bigTextPaint, bigTextShadowPaint);
        }

        offset += 65;

        var normalCenterValue = expert.Record == null ? center : center / 2;

        // NORMAL
        var currentWeekPps = $"{stats.Record.Results.Aggregatestats.Pps:F2} PPS";
        var currentWeekApm = $"{stats.Record.Results.Aggregatestats.Apm:F2} APM";
        var currentWeekVs = $"{stats.Record.Results.Aggregatestats.Vsscore:F2} VS";

        // We only draw the NORMAL text if we have an expert record as well
        if(expert.Record != null)
            DrawTextWithShadow(surface, $"NORMAL", normalCenterValue, 15 + offset, smallTextPaint, smallTextShadowPaint);

        DrawTextWithShadow(surface, $"{stats.Record!.Results.Stats.Zenith.Altitude:F2} m", normalCenterValue, 65 + offset, bigTextPaint, bigTextShadowPaint);
        DrawTextWithShadow(surface, $"{currentWeekPps} | {currentWeekApm} | {currentWeekVs}", normalCenterValue, 88 + offset, smallTextPaint, smallTextShadowPaint);
        // We only draw the personal best when it exists AND is better than the current week
        if (stats.Best?.Record != null && stats.Best.Record.Results.Stats.Zenith.Altitude != stats.Record.Results.Stats.Zenith.Altitude)
        {
            DrawTextWithShadow(surface, $"PB {stats.Best.Record.Results.Stats.Zenith.Altitude:F2} m", normalCenterValue, 120 + offset, normalTextPaint, normalTextShadowPaint);
        }
        else
        {
            offset -= 30;
        }

        var modsCurrentWeek = stats.Record.Extras.Zenith.Mods;
        // modsCurrentWeek = new []{"allspin", "allspin", "allspin", "allspin", "allspin", "allspin", "allspin", "allspin", "allspin", "allspin"};

        var modSize = 48;
        var modCanvasWidthCurrentWeek = (modSize * modsCurrentWeek.Length) + (10 * (modsCurrentWeek.Length - 1));

        if (modsCurrentWeek.Length > 0)
        {
            var currentWeekModsImage = GenerateModImage(ref modCanvasWidthCurrentWeek, modSize, modsCurrentWeek);

            surface.Canvas.DrawImage(currentWeekModsImage, normalCenterValue - modCanvasWidthCurrentWeek / 2, 130 + offset);
        }

        // EXPERT
        if (stats.Best?.Record == null || stats.Best.Record.Results.Stats.Zenith.Altitude == stats.Record.Results.Stats.Zenith.Altitude)
        {
            offset += 30;
        }

        if(expert.Record != null)
        {
            var modsExpert = expert.Record.Extras.Zenith.Mods;
            int modCanvasWidthPersonalBest = (modSize * modsExpert.Length) + (10 * (modsExpert.Length - 1));

            var expertPps = $"{expert.Record.Results.Aggregatestats.Pps:F2} PPS";
            var expertApm = $"{expert.Record.Results.Aggregatestats.Apm:F2} APM";
            var expertVs = $"{expert.Record.Results.Aggregatestats.Vsscore:F2} VS";

            DrawTextWithShadow(surface, $"EXPERT", (normalCenterValue) * 3, 15 + offset, smallTextPaint, smallTextShadowPaint);
            DrawTextWithShadow(surface, $"{expert.Record.Results.Stats.Zenith.Altitude:F2} m", (normalCenterValue) * 3, 65 + offset, bigTextPaint, bigTextShadowPaint);
            DrawTextWithShadow(surface, $"{expertPps} | {expertApm} | {expertVs}", (normalCenterValue) * 3, 88 + offset, smallTextPaint, smallTextShadowPaint);

            // We only draw the personal best when it exists AND is better than the current week
            if (expert.Best?.Record != null && expert.Best.Record.Results.Stats.Zenith.Altitude != expert.Record.Results.Stats.Zenith.Altitude)
            {
                DrawTextWithShadow(surface, $"PB {expert.Best.Record.Results.Stats.Zenith.Altitude:F2} m", (normalCenterValue) * 3, 120 + offset, normalTextPaint, normalTextShadowPaint);
            }
            else
            {
                offset -= 30;
            }

            if (modsExpert.Length > 0)
            {
                var personalBestModsImage = GenerateModImage(ref modCanvasWidthPersonalBest, modSize, modsExpert);

                surface.Canvas.DrawImage(personalBestModsImage, 3 * (center / 2) - modCanvasWidthPersonalBest / 2, 130 + offset);
            }
        }

        using var data = surface.Snapshot().Encode(SKEncodedImageFormat.Png, 80);

        data.SaveTo(stream);

        return stream;
    }

    public static MemoryStream GenerateZenithSplitsImage(string username, ZenithRecords stats, QuickPlay careerBest, string? textColor, string? backgroundColor, bool displayUsername)
    {
        var width = 1500;
        var height = 200;
        var center = (float) width / 2;
        var stream = new MemoryStream();
        var typeFace = SKTypeface.FromFile("Resources/cr.ttf");

        #region Paints

        textColor ??= "FFFFFF";

        var bigTextPaint = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 64,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var bigTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 64,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var normalTextPaint = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var normalTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var smallTextPaint = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 20,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var smallTextShadowPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            TextSize = 20,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Center,
        };

        var statsPaintAlt = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        var progressBarBg = new SKPaint
        {
            Color = SKColor.Parse("DD000000"),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        var progressBarBgAlt = new SKPaint
        {
            Color = SKColor.Parse($"FF{textColor}"),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        #endregion

        if (!displayUsername)
            height -= 65;

        using var surface = SKSurface.Create(new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Unpremul));

        SetBackground(surface, width, height, backgroundColor);

        var offset = 0;

        // TITLE
        if (displayUsername)
        {
            DrawTextWithShadow(surface, $"{username.ToUpper()}'S SPLITS", center, 65, bigTextPaint, bigTextShadowPaint);

            offset = 65;
        }

        var goldSplits = new int[9];

        foreach (var entry in stats.Entries)
        {
            var splits = entry.Results.Stats.Zenith.Splits;

            for (int i = 0; i < splits.Count; i++)
            {
                if (goldSplits[i] == 0)
                {
                    goldSplits[i] = (int) splits[i];
                    continue;
                }

                if (goldSplits[i] > splits[i] && splits[i] != 0)
                {
                    goldSplits[i] = (int) splits[i];
                }
            }
        }

        var recentSplits = stats.Entries.First().Results.Stats.Zenith.Splits.Select(x => (int) (x ?? 0)).ToArray();
        var careerBestSplits = careerBest.Best.Record.Results.Stats.Zenith.Splits.Select(x => (int) (x ?? 0)).ToArray();

        var splitsImageWidth = 0;

        var splitsImage = GenerateSplitsImage(ref splitsImageWidth, goldSplits, recentSplits, careerBestSplits, normalTextPaint, smallTextPaint, normalTextShadowPaint, smallTextShadowPaint);

        surface.Canvas.DrawImage(splitsImage, center - ( splitsImageWidth / 2 ), 25 + offset);

        using var data = surface.Snapshot().Encode(SKEncodedImageFormat.Png, 80);

        data.SaveTo(stream);

        return stream;
    }

    #region Private Methods

    private static SKBitmap GetBitmap(string relativePath)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var fullPath = Path.Combine(currentDirectory, relativePath);

        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"The file {fullPath} does not exist.");
        }

        // Ensure the file is readable
        var fileInfo = new FileInfo(fullPath);

        if ((fileInfo.Attributes & FileAttributes.ReadOnly) != 0)
        {
            throw new UnauthorizedAccessException($"The file {fullPath} is not accessible for reading.");
        }

        using var stream = File.OpenRead(fullPath);
        using var managedStream = new SKManagedStream(stream);

        var bitmap = SKBitmap.Decode(managedStream);

        if (bitmap == null)
        {
            throw new ArgumentException($"Failed to decode the bitmap from {fullPath}");
        }

        return bitmap;
    }

    private static SKBitmap ResizeBitmap(SKBitmap bitmap, int width, int height)
    {
        var resizedBitmap = new SKBitmap(width, height);

        using var canvas = new SKCanvas(resizedBitmap);
        using var paint = new SKPaint();

        paint.IsAntialias = true;
        paint.FilterQuality = SKFilterQuality.High;

        canvas.Clear(SKColors.Transparent);

        var destRect = new SKRect(0, 0, width, height);

        canvas.DrawBitmap(bitmap, destRect, paint);

        return resizedBitmap;
    }

    private static void DrawTextWithShadow(SKSurface surface, string text, float x, float y, SKPaint textPaint, SKPaint shawdowPaint)
    {
        surface.Canvas.DrawText(text, x + 2, y + 2, shawdowPaint);
        surface.Canvas.DrawText(text, x, y, textPaint);
    }

    private static void SetBackground(SKSurface surface, float width, float height, string? backgroundColor)
    {
        if (backgroundColor == null) return;

        var backgroundColorPaint = new SKPaint
        {
            Color = SKColor.Parse(backgroundColor),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
        };

        surface.Canvas.DrawRect(0, 0, width, height, backgroundColorPaint);
    }

    private static SKImage GenerateModImage(ref int modCanvasWidth, int modSize, string[] mods)
    {
        var height = mods.Length > 4 ? modSize * 2 : modSize;
        var factor = mods.Length > 4 ? 4 : mods.Length;

        modCanvasWidth = (modSize * factor) + (10 * (factor - 1));

        using var modSurface = SKSurface.Create(new SKImageInfo(modCanvasWidth, height + 5, SKColorType.Bgra8888, SKAlphaType.Unpremul));

        for (var i = 0; i < mods.Length; i++)
        {
            var mod = mods[i];
            var modBitmap = ResizeBitmap(GetBitmap($"Resources/{mod}.png"), modSize, modSize);

            if (i >= 4)
            {
                var subImageWidth = modSize * (mod.Length - i) + (10 * (mod.Length - i - 1));
                var image = GenerateModImage(ref subImageWidth, modSize, mods.Skip(i).ToArray());

                modSurface.Canvas.DrawImage(image, modCanvasWidth / 2 - subImageWidth / 2, modSize + 5);

                break;
            }

            var offset = modSize * i + 10 * i;

            modSurface.Canvas.DrawBitmap(modBitmap, offset, 0);
        }

        var modImage = modSurface.Snapshot();

        return modImage;
    }

    private static SKImage GenerateSplitsImage(ref int splitsImageWidth, int[] goldSplits, int[] recentSplits, int[] careerBestSplits, SKPaint textPaint, SKPaint smallTextPain, SKPaint shawdowPaint, SKPaint smallShadowPaint)
    {
        var height = 85;
        var rectWidth = 165;
        var rectWidthHalf = rectWidth / 2;

        var floorColors = new[]
        {
            "AAfde692",
            "AAffc788",
            "AAffb7c2",
            "AAffba43",
            "AAff917b",
            "AA00ddff",
            "AAff006f",
            "AA98ffb2",
            "AAd677ff",
        };

        var floorNames = new[]
        {
            "HOTEL",
            "CASINO",
            "ARENA",
            "MUSEUM",
            "OFFICES",
            "LABORATORY",
            "THE CORE",
            "CORRUPTION",
            "POTG",
        };

        var amountOfSplits = 0;

        if (recentSplits.All(x => x > 0))
        {
            amountOfSplits = goldSplits.Length;
        }
        else
        {
            for (var i = 0; i < goldSplits.Length; i++)
            {
                if (goldSplits[i] == 0)
                {
                    amountOfSplits = i + 1;

                    break;
                }
            }
        }

        splitsImageWidth = (rectWidth * amountOfSplits) - 6;

        using var surface = SKSurface.Create(new SKImageInfo(splitsImageWidth, height, SKColorType.Bgra8888, SKAlphaType.Unpremul));

        bool notReached = false;

        for (var i = 0; i < goldSplits.Length; i++)
        {
            var split = goldSplits[i];

            var backgroundPaint = new SKPaint
            {
                Color = SKColor.Parse(floorColors[i]),
                Style = SKPaintStyle.Fill,
                FakeBoldText = true,
                IsAntialias = true,
            };

            var blackOverlayPaint = new SKPaint
            {
                Color = SKColor.Parse("AA000000"),
                Style = SKPaintStyle.Fill,
                FakeBoldText = true,
                IsAntialias = true,
            };

            var time = TimeSpan.FromMilliseconds(split);

            var isRecentSplitsEmpty = recentSplits.All(x => x == 0);

            var recentTime = recentSplits[i] == 0 ? TimeSpan.FromMilliseconds(0) : TimeSpan.FromMilliseconds(recentSplits[i] - split);

            if (isRecentSplitsEmpty)
            {
                if (careerBestSplits.Any(x => x != 0))
                {
                    isRecentSplitsEmpty = false;
                }

                recentTime = TimeSpan.FromMilliseconds(careerBestSplits[i] - split);
            }

            var isSlower = recentTime.TotalMilliseconds > 0;
            var prefix = isSlower ? "+" : "-";

            if (split == 0)
            {
                surface.Canvas.DrawRect(0 + (i * rectWidth), 0, rectWidth - 6, 85, backgroundPaint);
                surface.Canvas.DrawRect(3 + (i * rectWidth), 3, rectWidth - 12, 79, blackOverlayPaint);

                DrawTextWithShadow(surface, $"{floorNames[i]}", rectWidthHalf + (i * rectWidth), 25, smallTextPain, smallShadowPaint);
                DrawTextWithShadow(surface, "DNF", rectWidthHalf + (i * rectWidth), 55, textPaint, shawdowPaint);

                break;
            }

            surface.Canvas.DrawRect(0 + (i * rectWidth), 0, rectWidth - 6, 85, backgroundPaint);
            surface.Canvas.DrawRect(3 + (i * rectWidth), 3, rectWidth - 12, 79, blackOverlayPaint);

            DrawTextWithShadow(surface, $"{floorNames[i]}", rectWidthHalf + (i * rectWidth), 25, smallTextPain, smallShadowPaint);
            DrawTextWithShadow(surface, time.ToString(@"m\:ss\.fff"), rectWidthHalf + (i * rectWidth), 55, textPaint, shawdowPaint);

            smallTextPain.Color = isSlower ? SKColor.Parse("EE005B") : SKColor.Parse("00EE00");

            if(recentTime.TotalMilliseconds != 0 && !isRecentSplitsEmpty)
                DrawTextWithShadow(surface, prefix + recentTime.ToString(@"s\.fff"), rectWidthHalf + (i * rectWidth), 75, smallTextPain, smallShadowPaint);
            else if(!notReached)
            {
                smallTextPain.Color = SKColors.DarkGray;

                DrawTextWithShadow(surface, "NOT REACHED", rectWidthHalf + (i * rectWidth), 75, smallTextPain, smallShadowPaint);

                notReached = true;
            }

            smallTextPain.Color = SKColor.Parse("FFFFFF");
        }


        var image = surface.Snapshot();

        return image;
    }

    #endregion
}