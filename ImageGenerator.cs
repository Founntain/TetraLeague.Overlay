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
    public static MemoryStream GenerateTetraLeagueStatsImage(string username, TetraLeague stats, string? textColor = null, string? backgroundColor = null)
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

        if (backgroundColor != null)
        {
            var backgroundColorPaint = new SKPaint
            {
                Color = SKColor.Parse(backgroundColor),
                Style = SKPaintStyle.Fill,
                TextSize = 32,
                FakeBoldText = true,
                IsAntialias = true,
                Typeface = typeFace
            };

            surface.Canvas.DrawRect(0, 0, width, height, backgroundColorPaint);
        }

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
}