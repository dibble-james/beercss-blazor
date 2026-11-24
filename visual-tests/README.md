# visual-tests

Screenshots every story in `BeerCss.Blazor.Stories` and compares it with a baseline, so a BeerCSS or
component change shows up as a reviewable image diff instead of something nobody notices.

This is a [Playwright](https://playwright.dev) project: `npx playwright test` is the whole harness.

## Run it

```bash
cd visual-tests
npm install                          # once
npx playwright install chromium      # once; needs the browser's system libraries on Linux
npx playwright test
```

On Linux the browser also needs its system libraries
(`npx playwright install-deps chromium`, which needs root) and at least one installed font — with no
fonts, text renders with zero height and every story looks empty.

The run publishes the app itself: `scripts/serve-demo.mjs` runs
`dotnet publish BeerCss.Blazor.Stories -c Release -o ../.demo-publish` and serves the published
`wwwroot` with `http-server` on `127.0.0.1:4173`, which Playwright starts and stops around the suite.
Use `./install-dotnet.sh` from the repository root for the pinned SDK; the script prefers `./.dotnet`
and `DOTNET_HOST_PATH` before falling back to `dotnet` on the `PATH`.

### Just one component

```bash
BEERCSS_STORY=components-card--default npx playwright test
```

`BEERCSS_STORY` takes a comma-separated list of story ids (the ids are the baseline file names).

### Against a demo you already have running

```bash
BEERCSS_DEMO_URL=http://127.0.0.1:5080 npx playwright test
```

That skips publishing and serving entirely.

## How a story is captured

Each story is one test against its isolated canvas (`/iframe.html?id=<story id>&viewMode=story`) in a
fresh browser context: viewport 1280×800, `deviceScaleFactor: 1`, light colour scheme,
`reducedMotion: reduce`, locale `en-US`, timezone UTC. Chromium runs with
`--disable-lcd-text --force-color-profile=srgb --font-render-hinting=none --hide-scrollbars` so text is
antialiased in grayscale and colour is not subject to a display profile.

Before the capture the test waits for the Blazor app to render, for `document.fonts.ready`, for
BlazingStory to finish its canvas preparation (`html._blazing_story_ready_for_visible` once the
`.default-background` backdrop is gone), and then freezes what a stylesheet cannot: animations and
carets are disabled by an injected sheet, `<video>`/`<audio>` elements are paused and rewound (the
shapes story embeds an autoplaying video), and an SVG used as a `mask-image` or `background-image` that
still contains SMIL animation is replaced with a copy minus those elements, at its first frame (the
same story masks with a `loading-indicator.svg` whose path is morphed by `<animate>`).

Playwright then retries the screenshot until two consecutive captures match, so a frame taken
mid-render can never become a baseline.

If a story id does not resolve, the canvas shows "Couldn't find story matching …" and the test fails
naming the story rather than capturing an empty image: every story renders inside the layout's
`<Beer>` root, and its absence is treated as a catalog bug.

## Baselines

`baselines/<story id>.png` (see `snapshotPathTemplate` in `playwright.config.ts`). A failure writes
the actual, expected and diff images under `test-results/`.

Comparison uses Playwright's own image comparison: per-pixel colour tolerance `threshold: 0.2` in
YIQ space, and at most `maxDiffPixelRatio: 0.001` of the canvas may differ — about 1000 pixels on a
1280×800 page. Faint antialiasing drift passes; a recoloured component, or any change to the image
size, fails.

To accept a change, run `npx playwright test --update-snapshots`, review the images, and explain in
the pull request why the rendering changed.

**Baselines must be generated on the CI Linux image.** Chromium's rendering depends on the OS, the
font stack and the browser build, so a capture from a developer machine (macOS, Windows, or a Linux
box with different fonts) is not a valid baseline and will fail in CI. New stories must arrive with
their baselines for the same reason.

### Renamed or deleted stories

Baselines are only compared with stories that exist, so a renamed or deleted story would leave a PNG
behind that silently approves an old rendering. `tests/baselines.spec.ts` fails the run and names
every orphaned baseline; delete those PNGs.
