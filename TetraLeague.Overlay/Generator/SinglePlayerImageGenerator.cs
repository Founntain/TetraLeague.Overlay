using SkiaSharp;
using TetraLeague.Overlay.Network.Api.Models;

namespace TetraLeague.Overlay.Generator;

public class SinglePlayerImageGenerator : BaseImageGenerator
{
    public MemoryStream GenerateSprintImage(string username, Sprint stats, string? textColor, string? backgroundColor, bool displayUsername = true)
    {
        var width = 700;
        var height = 225;

        var center = (float)width / 2;

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
        var kpp = $"{(double)stats.Record.Results.Stats.Inputs! / (double)stats.Record.Results.Stats.Piecesplaced!:F2}  KPP";
        var kps = $"{stats.Record.Results.Stats.Inputs / (stats.Record.Results.Stats.Finaltime / 1000):F2}  KPS";
        var finesse = $"{stats.Record.Results.Stats.Finesse!.Faults}F";

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

    public MemoryStream GenerateBlitzImage(string username, Blitz stats, string? textColor, string? backgroundColor, bool displayUsername = true)
    {
        var width = 700;
        var height = 225;

        var center = (float)width / 2;

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
        var kpp = $"{(double)stats.Record.Results.Stats.Inputs! / (double)stats.Record.Results.Stats.Piecesplaced!:F2}  KPP";
        var sps = $"{(double)stats.Record.Results.Stats.Score! / (double)stats.Record.Results.Stats.Piecesplaced:F2}  SPS";
        var finesse = $"{(stats.Record.Results.Stats.Finesse?.Faults.ToString() ?? "na")} F";

        // SCORE
        DrawTextWithShadow(surface, stats.Record.Results.Stats.Score!.Value.ToString("###,###"), center, 65 + offset, bigTextPaint, bigTextShadowPaint);
        // STATS
        DrawTextWithShadow(surface, $"{pps} | {kpp} | {sps} | {finesse}", center, 105 + offset, normalTextPaint, normalTextShadowPaint);
        // PLACEMENTS
        DrawTextWithShadow(surface, $"# {stats.Rank} | # {stats.RankLocal}", center, 140 + offset, normalTextPaint, normalTextShadowPaint);

        using var data = surface.Snapshot().Encode(SKEncodedImageFormat.Png, 80);

        data.SaveTo(stream);

        return stream;
    }
}