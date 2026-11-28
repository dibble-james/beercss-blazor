import { defineConfig } from '@playwright/test';
import { defaultViewport } from './support/captures';

const port = 4173;
const localURL = `http://127.0.0.1:${port}`;

// Point the run at a demo you already have running instead of publishing and serving one.
const demoURL = process.env.BEERCSS_DEMO_URL?.replace(/\/+$/, '');

export default defineConfig({
  testDir: './tests',
  timeout: 120_000,
  fullyParallel: true,
  forbidOnly: Boolean(process.env.CI),
  retries: 0,
  workers: process.env.CI ? 4 : undefined,
  reporter: process.env.CI ? [['list'], ['html', { open: 'never' }]] : 'list',

  // Baselines live in visual-tests/baselines/<story id>.png, or <story id>--<profile>.png for a
  // capture profile other than the canonical one: the name is built in support/captures.ts and only
  // the extension is added here. No platform suffix: the canonical set is generated on the CI image,
  // and a per-OS suffix would quietly accept a second one.
  snapshotPathTemplate: '{testDir}/../baselines/{arg}{ext}',
  outputDir: './test-results',

  expect: {
    toHaveScreenshot: {
      // 0.2 is Playwright's default per-pixel colour tolerance (YIQ), which absorbs the faint
      // antialiasing drift between runs. 0.1% of the canvas may still differ: that is ~1000
      // pixels on a 1280x800 page, so a recoloured component fails while a stray glyph does not.
      threshold: 0.2,
      maxDiffPixelRatio: 0.001,
      animations: 'disabled',
      caret: 'hide',
      scale: 'css',
    },
  },

  use: {
    baseURL: demoURL ?? localURL,
    viewport: defaultViewport,
    deviceScaleFactor: 1,
    colorScheme: 'light',
    reducedMotion: 'reduce',
    locale: 'en-US',
    timezoneId: 'UTC',
    launchOptions: {
      args: [
        // Grayscale antialiasing and sRGB keep text and colour identical across machines.
        '--disable-lcd-text',
        '--force-color-profile=srgb',
        '--font-render-hinting=none',
        '--hide-scrollbars',
      ],
    },
  },

  // Publishes the story app and serves the published wwwroot as static files: the baselines come
  // from the asset graph that ships, not from the WebAssembly dev server.
  webServer: demoURL
    ? undefined
    : {
        command: 'node scripts/serve-demo.mjs',
        url: `${localURL}/`,
        reuseExistingServer: !process.env.CI,
        timeout: 600_000,
      },
});
