import { expect, test, type Page } from '@playwright/test';
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

/**
 * Blazor replaces a component that throws with its error banner, and the layout's <Beer> root
 * survives, so the guard above would not notice: the capture would quietly become a picture of the
 * error. Two select baselines were exactly that before those stories stopped passing a null model
 * to EditForm.
 */
async function failOnErrorBanner(page: Page, storyId: string): Promise<void> {
  const banner = page.locator('#blazor-error-ui');
  if (!(await banner.isVisible())) return;

  const shown = ((await banner.textContent()) ?? '').replace(/\s+/g, ' ').trim();
  throw new Error(
    `Story '${storyId}' rendered Blazor's error banner instead of the story: '${shown}'`,
  );
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

    // A story may embed another page (the modules demo) that fetches and injects its own styles.
    for (const frame of page.frames().filter((frame) => frame !== page.mainFrame())) {
      await frame.waitForLoadState('networkidle', { timeout: 60_000 });
    }

    await page.evaluate(() => document.fonts.ready);
    await page.waitForFunction(storyReady, null, { timeout: 60_000 });

    // The WebAssembly app finishes its first render after load; give the final layout a beat to settle.
    await page.waitForTimeout(150);

    await page.addStyleTag({ content: freezeStyles });
    await page.evaluate(freezeMedia);
    await page.evaluate(freezeAnimatedImages);

    // Last thing before the capture: a crashed render must fail the story, never become a baseline.
    await failOnErrorBanner(page, story.id);

    // Playwright retries the screenshot until two consecutive captures match, so a frame taken
    // mid-render can never become a baseline.
    await expect(page).toHaveScreenshot(`${story.id}.png`);
  });
}
