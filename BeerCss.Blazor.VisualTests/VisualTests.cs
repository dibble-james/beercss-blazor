using Microsoft.Playwright;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace BeerCss.Blazor.VisualTests;

[Collection(DemoCollectionDefinition.Name)]
public class VisualTests
{
    /// <summary>Kills animation and carets so a capture depends only on layout and paint.</summary>
    private const string Freeze =
        "*,*::before,*::after{animation:none!important;transition:none!important;animation-play-state:paused!important;caret-color:transparent!important}";

    /// <summary>
    /// A stylesheet cannot stop a playing video, and the shapes story embeds one. Pausing and
    /// rewinding every media element pins it to a single frame, so the capture no longer depends
    /// on how far playback happened to get.
    /// </summary>
    private const string FreezeMedia =
        """
        async () => {
            const media = [...document.querySelectorAll('video, audio')];
            for (const element of media) {
                element.autoplay = false;
                element.loop = false;
                element.pause();
                if (element.readyState < 1) continue;
                element.currentTime = 0;
                await new Promise(resolve => {
                    if (!element.seeking) return resolve();
                    element.addEventListener('seeked', resolve, { once: true });
                });
            }
        }
        """;

    /// <summary>
    /// CSS cannot stop animation inside an SVG used as a mask or background: the shapes story masks
    /// with a <c>loading-indicator.svg</c> whose path is morphed by a SMIL <c>&lt;animate&gt;</c> element,
    /// and that keeps repainting no matter what the stylesheet says. Replacing those images with a
    /// copy that has the animation elements removed pins them to their first frame.
    /// </summary>
    private const string FreezeAnimatedImages =
        """
        async () => {
            const rules = [];
            const frozen = new Map();
            for (const element of document.querySelectorAll('*')) {
                for (const property of ['maskImage', 'backgroundImage']) {
                    const value = getComputedStyle(element)[property];
                    const url = /url\("?'?([^"')]+)"?'?\)/.exec(value ?? '');
                    if (!url) continue;

                    if (!frozen.has(url[1])) {
                        let data = null;
                        if (/\.svg(\?|$)/i.test(url[1])) {
                            try {
                                const text = await (await fetch(url[1])).text();
                                const still = text
                                    .replace(/<animate[\s\S]*?(?:\/>|<\/animate>)/gi, '')
                                    .replace(/<animateTransform[\s\S]*?(?:\/>|<\/animateTransform>)/gi, '')
                                    .replace(/<animateMotion[\s\S]*?(?:\/>|<\/animateMotion>)/gi, '');
                                data = 'data:image/svg+xml;charset=utf-8,' + encodeURIComponent(still);
                            } catch { /* Leave a cross-origin or missing image alone. */ }
                        }
                        frozen.set(url[1], data);
                    }

                    const data = frozen.get(url[1]);
                    if (!data) continue;
                    const index = rules.length;
                    element.setAttribute('data-frozen-image', String(index));
                    rules.push(`[data-frozen-image="${index}"]{${property === 'maskImage' ? 'mask-image' : 'background-image'}:url("${data}")!important}`);
                }
            }

            if (rules.length === 0) return;
            const style = document.createElement('style');
            style.textContent = rules.join('\n');
            document.head.append(style);
        }
        """;

    private const int CaptureAttempts = 10;

    private readonly DemoFixture _demo;

    public VisualTests(DemoFixture demo) => _demo = demo;

    /// <summary>
    /// Every story, or just the ones named in <c>BEERCSS_STORY</c> (comma separated ids)
    /// while iterating on a single component.
    /// </summary>
    public static TheoryData<string> Stories
    {
        get
        {
            var only = Environment.GetEnvironmentVariable("BEERCSS_STORY");
            var ids = StoryCatalog.All.Select(s => s.Id);
            if (!string.IsNullOrWhiteSpace(only))
            {
                var wanted = only.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                ids = ids.Where(wanted.Contains);
            }

            var data = new TheoryData<string>();
            foreach (var id in ids)
                data.Add(id);

            return data;
        }
    }

    [Theory]
    [MemberData(nameof(Stories))]
    public async Task Story(string id)
    {
        var story = StoryCatalog.All.Single(s => s.Id == id);
        var page = await _demo.NewPageAsync();

        try
        {
            await page.GotoAsync(_demo.Origin + story.CanvasPath, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.Load,
                Timeout = 60_000,
            });

            await WaitForStoryAsync(page, story.Id);
            await page.AddStyleTagAsync(new PageAddStyleTagOptions { Content = Freeze });
            await page.EvaluateAsync(FreezeMedia);
            await page.EvaluateAsync(FreezeAnimatedImages);

            var bytes = await CaptureStableAsync(page.Locator("body"));

            var settings = new VerifySettings();
            settings.UseDirectory(RepoRoot.Baselines);
            settings.UseFileName(story.Id);
            settings.DisableDiff();

            PngComparer.CurrentStory.Value = story.Id;
            try
            {
                await Verify(bytes, "png", settings);
            }
            finally
            {
                PngComparer.CurrentStory.Value = null;
            }
        }
        finally
        {
            await page.Context.CloseAsync();
        }
    }

    /// <summary>
    /// BlazingStory fades a full-page backdrop out and only then marks the document ready;
    /// capturing before that yields a blank or half-faded canvas.
    /// </summary>
    private const string StoryReady =
        """
        () => {
            if (!document.documentElement.classList.contains('_blazing_story_ready_for_visible')) return false;
            const backdrop = document.querySelector('.default-background');
            return !backdrop || getComputedStyle(backdrop).display === 'none';
        }
        """;

    private static async Task WaitForStoryAsync(IPage page, string id)
    {
        await page.WaitForFunctionAsync("() => document.querySelector('#app')?.childElementCount > 0", null, new PageWaitForFunctionOptions
        {
            Timeout = 60_000,
        });

        // Every story renders inside the layout's <Beer> root. Its absence means the id resolved
        // to nothing (BlazingStory shows "Couldn't find story matching..."), which is a catalog bug.
        try
        {
            await page.WaitForFunctionAsync("() => !!document.querySelector('.beer')", null, new PageWaitForFunctionOptions
            {
                Timeout = 30_000,
            });
        }
        catch (TimeoutException)
        {
            var shown = (await page.TextContentAsync("body") ?? "").Trim();
            if (shown.Length > 200)
                shown = shown[..200];
            throw new InvalidOperationException($"Story '{id}' did not render on its canvas. The canvas showed: '{shown}'");
        }

        await page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions { Timeout = 60_000 });

        await page.EvaluateAsync("async () => { await document.fonts.ready; }");

        await page.WaitForFunctionAsync(StoryReady, null, new PageWaitForFunctionOptions { Timeout = 60_000 });

        // The WebAssembly app finishes its first render after load; give the final layout a beat to settle.
        await page.WaitForTimeoutAsync(150);
    }

    /// <summary>
    /// Captures until two consecutive screenshots are byte-identical, so a capture taken
    /// mid-render can never become a baseline.
    /// </summary>
    private static async Task<byte[]> CaptureStableAsync(ILocator target)
    {
        var previous = await ScreenshotAsync(target);

        for (var attempt = 1; attempt < CaptureAttempts; attempt++)
        {
            await Task.Delay(150);
            var current = await ScreenshotAsync(target);
            if (current.AsSpan().SequenceEqual(previous))
                return current;
            previous = current;
        }

        throw new InvalidOperationException("The story never rendered the same way twice in a row.");
    }

    private static Task<byte[]> ScreenshotAsync(ILocator target) => target.ScreenshotAsync(new LocatorScreenshotOptions
    {
        Animations = ScreenshotAnimations.Disabled,
        Caret = ScreenshotCaret.Hide,
        Scale = ScreenshotScale.Css,
        Type = ScreenshotType.Png,
    });
}
