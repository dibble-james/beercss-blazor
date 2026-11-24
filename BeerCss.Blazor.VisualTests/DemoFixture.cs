using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Xunit;

namespace BeerCss.Blazor.VisualTests;

/// <summary>
/// Publishes the story demo, serves it and starts Chromium once for the whole run. Everything
/// that makes a screenshot reproducible lives here: how the app is served, the browser flags, the
/// viewport and the colour scheme, so no individual test can drift away from the rest.
/// </summary>
public sealed class DemoFixture : IAsyncLifetime
{
    private WebApplication? _host;
    private IPlaywright? _playwright;

    public string Origin { get; private set; } = "";

    public IBrowser Browser { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
        var configured = Environment.GetEnvironmentVariable("BEERCSS_DEMO_URL");
        Origin = string.IsNullOrWhiteSpace(configured)
            ? await StartDemoAsync()
            : configured.TrimEnd('/');

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

        if (_host is not null)
            await _host.DisposeAsync();
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

    /// <summary>
    /// Publishes the story demo and serves the published <c>wwwroot</c> from an in-process Kestrel
    /// on an ephemeral port. The app is a Blazor WebAssembly project, so its published output is a
    /// folder of static files, but Chromium still needs a real http origin: it cannot run the
    /// runtime from <c>file://</c>. Serving the publish output means the screenshots come from the
    /// asset graph a static host would serve, not from the dev server's runtime-pack wiring.
    /// </summary>
    private async Task<string> StartDemoAsync()
    {
        var publishDir = Path.Combine(RepoRoot.Path, "BeerCss.Blazor.VisualTests", "bin", "demo-publish");
        if (Directory.Exists(publishDir))
            Directory.Delete(publishDir, recursive: true);

        await PublishAsync(publishDir);

        var webRoot = Path.Combine(publishDir, "wwwroot");
        if (!Directory.Exists(webRoot))
            throw new InvalidOperationException($"The published story demo has no wwwroot at '{webRoot}'.");

        var builder = WebApplication.CreateSlimBuilder(new WebApplicationOptions
        {
            ContentRootPath = publishDir,
            WebRootPath = webRoot,
            // A production environment keeps dev-only UI (the error bar, detailed errors) out of the baselines.
            EnvironmentName = Environments.Production,
        });
        builder.Logging.ClearProviders();
        builder.WebHost.UseUrls("http://127.0.0.1:0");

        // Streaming compilation needs the wasm MIME type, and webcil is served as opaque bytes.
        var contentTypes = new FileExtensionContentTypeProvider();
        contentTypes.Mappings[".wasm"] = "application/wasm";
        contentTypes.Mappings[".webcil"] = "application/octet-stream";

        var app = builder.Build();
        app.UseDefaultFiles();
        app.UseStaticFiles(new StaticFileOptions
        {
            ContentTypeProvider = contentTypes,
            // The runtime ships payloads ASP.NET has no mapping for (.dat for ICU, .blat, ...).
            // Without this they 404 and the app fails to boot with "Failed to fetch icudt...dat".
            ServeUnknownFileTypes = true,
            DefaultContentType = "application/octet-stream",
        });
        await app.StartAsync();

        var address = app.Services.GetRequiredService<IServer>()
                          .Features.Get<IServerAddressesFeature>()?.Addresses.FirstOrDefault()
                      ?? throw new InvalidOperationException("The static host reported no listening address.");

        _host = app;
        return address;
    }

    private static async Task PublishAsync(string publishDir)
    {
        var startInfo = new ProcessStartInfo(ResolveDotnet())
        {
            WorkingDirectory = RepoRoot.Path,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        foreach (var arg in new[]
                 {
                     "publish", RepoRoot.DemoProject,
                     "--configuration", "Release",
                     "--output", publishDir,
                     "--nologo",
                 })
        {
            startInfo.ArgumentList.Add(arg);
        }

        startInfo.Environment["DOTNET_CLI_TELEMETRY_OPTOUT"] = "1";
        startInfo.Environment["DOTNET_NOLOGO"] = "1";

        using var process = Process.Start(startInfo)
                            ?? throw new InvalidOperationException("Could not start dotnet publish for the story demo.");

        var log = new List<string>();
        process.OutputDataReceived += (_, e) => { if (e.Data is not null) lock (log) log.Add(e.Data); };
        process.ErrorDataReceived += (_, e) => { if (e.Data is not null) lock (log) log.Add(e.Data); };
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(10));
        try
        {
            await process.WaitForExitAsync(timeout.Token);
        }
        catch (OperationCanceledException)
        {
            process.Kill(entireProcessTree: true);
            throw new TimeoutException(
                $"dotnet publish of the story demo did not finish within 10 minutes:{Environment.NewLine}{string.Join(Environment.NewLine, LogSnapshot(log))}");
        }

        if (process.ExitCode != 0)
            throw new InvalidOperationException(
                $"dotnet publish of the story demo failed with exit code {process.ExitCode}:{Environment.NewLine}{string.Join(Environment.NewLine, LogSnapshot(log))}");
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
