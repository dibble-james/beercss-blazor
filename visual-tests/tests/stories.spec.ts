import { expect, test } from '@playwright/test';
import { freezeAnimatedImages, freezeMedia, freezeStyles, storyReady } from '../support/freeze';
import { stories } from '../support/stories';

// Every story, or just the ones named in BEERCSS_STORY (comma separated ids) while iterating.
const only = (process.env.BEERCSS_STORY ?? '')
  .split(',')
  .map((id) => id.trim())
  .filter(Boolean);

const selected = only.length > 0 ? stories.filter((story) => only.includes(story.id)) : stories;

if (only.length > 0 && selected.length === 0) {
  throw new Error(`BEERCSS_STORY matched no story: ${only.join(', ')}.`);
}

for (const story of selected) {
  test(`story ${story.id}`, async ({ page }) => {
    await page.goto(`/iframe.html?id=${encodeURIComponent(story.id)}&viewMode=story`, {
      waitUntil: 'load',
      timeout: 60_000,
    });

    await page.waitForFunction(
      () => (document.querySelector('#app')?.childElementCount ?? 0) > 0,
      null,
      { timeout: 60_000 },
    );

    // Every story renders inside the layout's <Beer> root. Its absence means the id resolved to
    // nothing (BlazingStory shows "Couldn't find story matching..."), which is a catalog bug.
    try {
      await page.waitForFunction(() => Boolean(document.querySelector('.beer')), null, {
        timeout: 30_000,
      });
    } catch {
      const shown = ((await page.textContent('body')) ?? '').trim().slice(0, 200);
      throw new Error(
        `Story '${story.id}' did not render on its canvas. The canvas showed: '${shown}'`,
      );
    }

    await page.waitForLoadState('networkidle', { timeout: 60_000 });
    await page.evaluate(() => document.fonts.ready);
    await page.waitForFunction(storyReady, null, { timeout: 60_000 });

    // The WebAssembly app finishes its first render after load; give the final layout a beat to settle.
    await page.waitForTimeout(150);

    await page.addStyleTag({ content: freezeStyles });
    await page.evaluate(freezeMedia);
    await page.evaluate(freezeAnimatedImages);

    // Playwright retries the screenshot until two consecutive captures match, so a frame taken
    // mid-render can never become a baseline.
    await expect(page).toHaveScreenshot(`${story.id}.png`);
  });
}
