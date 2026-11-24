using SkiaSharp;
using VerifyTests;

namespace BeerCss.Blazor.VisualTests;

/// <summary>
/// Compares PNG screenshots pixel by pixel and tolerates a small amount of noise, so
/// antialiasing differences do not fail a run while a real style change does.
/// </summary>
public static class PngComparer
{
    /// <summary>Allowed difference per colour channel, as a fraction of 255.</summary>
    private const double ChannelTolerance = 0.1;

    /// <summary>Allowed share of differing pixels before a story fails.</summary>
    private const double MaxDifferingPixelRatio = 0.005;

    /// <summary>The story being verified, so a mismatch can also write a diff image.</summary>
    public static readonly AsyncLocal<string?> CurrentStory = new();

    public static void Register() => VerifierSettings.RegisterStreamComparer("png", CompareAsync);

    private static Task<CompareResult> CompareAsync(Stream received, Stream verified, IReadOnlyDictionary<string, object> context)
    {
        using var receivedBitmap = SKBitmap.Decode(received);
        using var verifiedBitmap = SKBitmap.Decode(verified);

        if (receivedBitmap is null || verifiedBitmap is null)
            return Task.FromResult(CompareResult.NotEqual("One of the images is not a readable PNG."));

        if (receivedBitmap.Width != verifiedBitmap.Width || receivedBitmap.Height != verifiedBitmap.Height)
            return Task.FromResult(CompareResult.NotEqual(
                $"Size changed: {receivedBitmap.Width}x{receivedBitmap.Height} (received) vs {verifiedBitmap.Width}x{verifiedBitmap.Height} (verified)."));

        var expected = verifiedBitmap.Pixels;
        var actual = receivedBitmap.Pixels;
        var total = expected.Length;

        var differing = 0;
        int minX = int.MaxValue, minY = int.MaxValue, maxX = -1, maxY = -1;
        var diffPixels = new SKColor[total];

        for (var i = 0; i < total; i++)
        {
            var a = expected[i];
            var b = actual[i];
            var delta = Math.Max(
                Math.Max(Math.Abs(a.Red - b.Red), Math.Abs(a.Green - b.Green)),
                Math.Max(Math.Abs(a.Blue - b.Blue), Math.Abs(a.Alpha - b.Alpha)));

            if (delta / 255.0 > ChannelTolerance)
            {
                differing++;
                var x = i % receivedBitmap.Width;
                var y = i / receivedBitmap.Width;
                minX = Math.Min(minX, x);
                minY = Math.Min(minY, y);
                maxX = Math.Max(maxX, x);
                maxY = Math.Max(maxY, y);
                diffPixels[i] = new SKColor(255, 0, 0);
            }
            else
            {
                // Fade the matching background so the changed region stands out.
                diffPixels[i] = new SKColor(a.Red, a.Green, a.Blue, 70);
            }
        }

        var ratio = (double)differing / total;
        if (ratio <= MaxDifferingPixelRatio)
            return Task.FromResult(CompareResult.Equal);

        WriteDiffImage(diffPixels, receivedBitmap.Width, receivedBitmap.Height);

        return Task.FromResult(CompareResult.NotEqual(
            $"{differing:N0} of {total:N0} pixels differ ({ratio:P2}, budget {MaxDifferingPixelRatio:P1}); " +
            $"changed region x{minX}-{maxX} y{minY}-{maxY}."));
    }

    private static void WriteDiffImage(SKColor[] pixels, int width, int height)
    {
        var story = CurrentStory.Value;
        if (string.IsNullOrEmpty(story))
            return;

        try
        {
            Directory.CreateDirectory(RepoRoot.Baselines);
            using var bitmap = new SKBitmap(width, height);
            bitmap.Pixels = pixels;
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 90);
            using var file = File.Create(Path.Combine(RepoRoot.Baselines, $"{story}.diff.png"));
            data.SaveTo(file);
        }
        catch (IOException)
        {
            // A diff image is a convenience; never let writing it mask the real failure.
        }
    }
}
