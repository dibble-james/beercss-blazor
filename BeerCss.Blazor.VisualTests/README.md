# BeerCss.Blazor.VisualTests

Screenshots every story in `BeerCss.Blazor.Stories` and compares it with a committed baseline, so a
BeerCSS or component change shows up as a reviewable image diff instead of something nobody notices.

`dotnet test` is the whole harness: xUnit v3, `Microsoft.Playwright` and `Verify.Playwright`. There is
no Node toolchain, no `package.json` and no second thing to install.

## Run it

```bash
./install-dotnet.sh                  # once, installs the .NET 10 SDK into ./.dotnet (see global.json)
dotnet test --project BeerCss.Blazor.VisualTests
```

Chromium itself is downloaded on first run by `VerifyPlaywright.Initialize(installPlaywright: true)`
in `VerifySetup.cs`. On a Linux machine that also needs the browser's system libraries
(`sudo pwsh bin/Debug/net10.0/playwright.ps1 install-deps`, or `sudo npx playwright install-deps chromium`)
and at least one installed font — with no fonts, text renders with zero height and every story looks empty.

### Just one component

```bash
BEERCSS_STORY=components-card--default dotnet test --project BeerCss.Blazor.VisualTests
```

`BEERCSS_STORY` takes a comma-separated list of story ids (the ids are the baseline file names).

### Against a demo you already have running

```bash
BEERCSS_DEMO_URL=http://127.0.0.1:5080 dotnet test --project BeerCss.Blazor.VisualTests
```

Without it, the test run starts `BeerCss.Blazor.Stories` itself on port 5080 (environment
`Production`, so dev-only UI stays out of the images) and stops it afterwards.

## How a story is captured

Each story runs as its own test case against its isolated canvas
(`/iframe.html?id=<story id>&viewMode=story`) in a fresh Chromium context: viewport 1280×800,
`deviceScaleFactor: 1`, light colour scheme, `reducedMotion: reduce`, locale `en-US`, timezone UTC.
Chromium runs with `--disable-lcd-text --force-color-profile=srgb --hide-scrollbars` so text is
antialiased in grayscale and colour is not subject to a display profile.

Before a capture the test waits for the Blazor app to render, for `document.fonts.ready`, for
BlazingStory to finish its canvas preparation (`html._blazing_story_ready_for_visible` once the
`.default-background` backdrop is gone), and then for the story to stop changing: two consecutive
screenshots must be byte-identical. Animations, transitions and carets are frozen with an injected
stylesheet first.

A stylesheet cannot stop everything, so two kinds of content are frozen in the page before the
capture: `<video>`/`<audio>` elements are paused and rewound (the shapes story embeds an autoplaying
video), and an SVG used as a `mask-image` or `background-image` that contains SMIL animation elements
is replaced with a copy of itself minus those elements at its first frame (the same story masks with a
`loading-indicator.svg` whose blob path is morphed by `<animate>`). Both would otherwise change on
every screenshot and no story would ever be stable.

If a story id does not resolve, the canvas shows "Couldn't find story matching …" and the test fails
naming the story rather than capturing an empty image: every story renders inside the layout's
`<Beer>` root, and its absence is treated as a catalog bug.

## Baselines

`baselines/<story id>.verified.png`. A mismatch writes `<story id>.received.png` next to it, plus
`<story id>.diff.png` marking the changed pixels.

Comparison is done by `PngComparer`, registered for `.png` with `VerifierSettings.RegisterStreamComparer`:

- a pixel counts as different only when a colour channel differs by more than 10% (25/255);
- a story fails when more than **0.5%** of its pixels differ;
- the failure message reports the percentage and the changed region.

That budget absorbs antialiasing noise (a Material Symbols glyph can shift a couple of pixels between
runs) while a real spacing, colour or size change is far above it. To accept a change, replace the
`*.verified.png` with the `*.received.png` Verify wrote for it, and explain in the pull request why the
rendering changed.

**Baselines must be generated on the CI Linux image.** Chromium's rendering depends on the OS, the font
stack and the browser build, so a capture from a developer machine (macOS, Windows, or a Linux box with
different fonts) is not a valid baseline and will fail in CI. New stories must arrive with their
baselines for the same reason.

### Renamed or deleted stories

Baselines are only compared with stories that exist, so a renamed or deleted story would leave a PNG
behind that silently approves an old rendering. `BaselineTests.EveryBaselineBelongsToAStory` fails the
run and names every orphaned baseline; delete those PNGs.

## Verify's maintenance fee

The test project declares the documented `OpenSource` exemption in its `.csproj`
(`Verify_SponsorshipExemption`), because BeerCss.Blazor is a non-revenue MIT project. The declaration
is time-bounded (`Verify_SponsorshipExemptionUntil`); renew it, or replace it with a sponsorship or
licence, before it expires, or the build fails with `SC021`.
