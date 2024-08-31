using SkiaSharp;
using TetraLeagueOverlay.Api.Models;

namespace TetraLeagueOverlay;

public static class ImageGenerator
{
    public static MemoryStream GenerateStatsImage(string username, TetraLeague stats, string? textColor = null, string? backgroundColor = null)
    {
        var width = 800;
        var height = 300;
        var stream = new MemoryStream();
        var typeFace = SKTypeface.FromFile("Resources/cr.ttf");

        #region Paints

        textColor ??= "FFFFFF";

        var namePaint = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 64,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace
        };

        var statsPaint = new SKPaint
        {
            Color = SKColor.Parse(textColor),
            Style = SKPaintStyle.Fill,
            TextSize = 32,
            FakeBoldText = true,
            IsAntialias = true,
            Typeface = typeFace,
            TextAlign = SKTextAlign.Left,
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

        var rankBitmap = GetBitmap($"Resources/{stats.Rank}.png");
        var toprankBitmap = GetBitmap($"Resources/{stats.TopRank}.png");

        var prevRank = stats.PrevRank == null ? null : GetBitmap($"Resources/{stats.PrevRank}.png");
        var nextRank = stats.NextRank == null ? null : GetBitmap($"Resources/{stats.NextRank}.png");

        // Big Rank Letter
        surface.Canvas.DrawBitmap(ResizeBitmap(rankBitmap, 200, 200), 40, 40);

        // Username
        surface.Canvas.DrawText(username.ToUpper(), 275, 75, namePaint);

        // TR
        surface.Canvas.DrawText($"{stats.Tr:#.##} TR", 275, 140, namePaint);

        // Stats
        surface.Canvas.DrawRect(462, 162, 3, 85, statsPaint);

        // Left Side
        surface.Canvas.DrawText($"{stats.Apm}", 275, 185, statsPaint); surface.Canvas.DrawText($"APM", 385, 185, statsPaintAlt);
        surface.Canvas.DrawText($"{stats.Pps}", 275, 215, statsPaint); surface.Canvas.DrawText($"PPS", 385, 215, statsPaintAlt);
        surface.Canvas.DrawText($"{stats.Vs}", 275, 245, statsPaint); surface.Canvas.DrawText($"VS", 385, 245, statsPaintAlt);

        // Right Side
        surface.Canvas.DrawText($"GLOBAL", 475, 185, statsPaint); surface.Canvas.DrawText($"# {stats.StandingGlobal!.Value}", 630, 185, statsPaint);
        surface.Canvas.DrawText($"COUNTRY", 475, 215, statsPaint);surface.Canvas.DrawText($"# {stats.StandingLocal!.Value}", 630, 215, statsPaint);
        surface.Canvas.DrawText($"TOP RANK", 475, 245, statsPaint); surface.Canvas.DrawBitmap(ResizeBitmap(toprankBitmap, 32, 32), 630, 218);

        // Progressbar
        if (stats.PrevAt.HasValue && stats.NextAt.HasValue && stats.StandingGlobal.HasValue)
        {
            double range = stats.PrevAt.Value - stats.NextAt.Value;
            double distance = stats.PrevAt.Value - stats.StandingGlobal.Value;
            double rankPercentage = distance / range * 100;

            var highlight = (int)(rankPercentage / 100 * (width - 200));

            highlight = highlight > width-200 ? width - 200 : highlight;

            surface.Canvas.DrawRect(100, 275, width - 200, 10, progressBarBg);
            surface.Canvas.DrawRect(100, 275, highlight, 10, progressBarBgAlt);

            if(stats.PrevRank != null && prevRank != null)
                surface.Canvas.DrawBitmap(ResizeBitmap(prevRank, 32, 32), 60, 265);

            if(stats.NextRank != null && nextRank != null)
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

        if (!System.IO.File.Exists(fullPath))
        {
            throw new FileNotFoundException($"The file {fullPath} does not exist.");
        }

        // Ensure the file is readable
        var fileInfo = new FileInfo(fullPath);

        if ((fileInfo.Attributes & FileAttributes.ReadOnly) != 0)
        {
            throw new UnauthorizedAccessException($"The file {fullPath} is not accessible for reading.");
        }

        Console.WriteLine($"Loading bitmap from {fullPath}");

        using var stream = System.IO.File.OpenRead(fullPath);
        using var managedStream = new SKManagedStream(stream);

        var bitmap = SKBitmap.Decode(managedStream);

        if (bitmap == null)
        {
            throw new ArgumentException($"Failed to decode the bitmap from {fullPath}");
        }

        Console.WriteLine($"Loaded bitmap from {fullPath} successfully");

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
}