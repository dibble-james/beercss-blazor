using System.Diagnostics;
using System.Net;
using Microsoft.Playwright;
using Xunit;

namespace BeerCss.Blazor.VisualTests;

/// <summary>
/// Starts the story demo and Chromium once for the whole run. Everything that makes a
/// screenshot reproducible lives here: the browser flags, the viewport and the colour
/// scheme, so no individual test can drift away from the rest.
/// </summary>
public sealed class DemoFixture : IAsyncLifetime
{
    public const int Port = 5080;

    private Process? _demo;
    private IPlaywright? _playwright;

    public string Origin { get; private set; } = "";

    public IBrowser Browser { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
        var configured = Environment.GetEnvironmentVariable("BEERCSS_DEMO_URL");
        if (string.IsNullOrWhiteSpace(configured))
        {
            (Origin, _demo) = await StartDemoAsync();
        }
        else
        {
            Origin = configured.TrimEnd('/');
        }

        _playwright = await Playwright.CreateAsync();
        Browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            Args =
            [
                // Grayscale antialiasing and sRGB keep text and colour identical across machines.
                "--disable-lcd-text",
                "--force-color-profile=srgb",
                "--font-render-hinting=none",
                "--hide-scrollbars",
            ],
        });
    }

    public async ValueTask DisposeAsync()
    {
        if (Browser is not null)
            await Browser.DisposeAsync();

        _playwright?.Dispose();

        if (_demo is { HasExited: false })
        {
            _demo.Kill(entireProcessTree: true);
            await _demo.WaitForExitAsync();
        }

        _demo?.Dispose();
    }

    /// <summary>A fresh, identically configured page for one story.</summary>
    public async Task<IPage> NewPageAsync()
    {
        var context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1280, Height = 800 },
            DeviceScaleFactor = 1,
            ColorScheme = ColorScheme.Light,
            ReducedMotion = ReducedMotion.Reduce,
            Locale = "en-US",
            TimezoneId = "UTC",
            JavaScriptEnabled = true,
        });

        return await context.NewPageAsync();
    }

    private static async Task<(string Origin, Process Process)> StartDemoAsync()
    {
        var origin = $"http://127.0.0.1:{Port}";
        var startInfo = new ProcessStartInfo(ResolveDotnet())
        {
            WorkingDirectory = RepoRoot.Path,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        foreach (var arg in new[]
                 {
                     "run", "--project", RepoRoot.DemoProject, "--urls", origin,
                     // A production environment keeps dev-only UI (the error bar, detailed errors) out of the baselines.
                     "--environment", "Production",
                 })
        {
            startInfo.ArgumentList.Add(arg);
        }

        startInfo.Environment["DOTNET_CLI_TELEMETRY_OPTOUT"] = "1";
        startInfo.Environment["DOTNET_NOLOGO"] = "1";

        var process = Process.Start(startInfo)
                      ?? throw new InvalidOperationException("Could not start the story demo.");

        var log = new List<string>();
        process.OutputDataReceived += (_, e) => { if (e.Data is not null) lock (log) log.Add(e.Data); };
        process.ErrorDataReceived += (_, e) => { if (e.Data is not null) lock (log) log.Add(e.Data); };
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
        var deadline = DateTime.UtcNow.AddSeconds(180);
        while (DateTime.UtcNow < deadline)
        {
            if (process.HasExited)
                throw new InvalidOperationException($"The story demo exited with code {process.ExitCode}:{Environment.NewLine}{string.Join(Environment.NewLine, LogSnapshot(log))}");

            try
            {
                var response = await client.GetAsync(origin + "/");
                if (response.StatusCode == HttpStatusCode.OK)
                    return (origin, process);
            }
            catch (HttpRequestException)
            {
                // Not listening yet.
            }
            catch (TaskCanceledException)
            {
                // Not answering yet.
            }

            await Task.Delay(500);
        }

        process.Kill(entireProcessTree: true);
        throw new TimeoutException(
            $"The story demo did not answer on {origin} within 180 seconds:{Environment.NewLine}{string.Join(Environment.NewLine, LogSnapshot(log))}");
    }

    private static string[] LogSnapshot(List<string> log)
    {
        lock (log)
            return log.TakeLast(50).ToArray();
    }

    private static string ResolveDotnet()
    {
        var host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH");
        if (!string.IsNullOrWhiteSpace(host) && File.Exists(host))
            return host;

        var local = Path.Combine(RepoRoot.Path, ".dotnet", OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        return File.Exists(local) ? local : "dotnet";
    }
}

[CollectionDefinition(Name)]
public sealed class DemoCollectionDefinition : ICollectionFixture<DemoFixture>
{
    public const string Name = "story demo";
}
