import { expect, test } from '@playwright/test';
import { existsSync, readdirSync } from 'node:fs';
import { baselinesDirectory, stories } from '../support/stories';

/**
 * Keeps the baseline folder honest: a renamed or deleted story leaves a PNG that no longer
 * describes anything, and it would otherwise sit there forever, quietly green.
 */
test('every baseline belongs to a story', () => {
  if (!existsSync(baselinesDirectory)) return;

  const known = new Set(stories.map((story) => `${story.id}.png`));
  const orphans = readdirSync(baselinesDirectory)
    .filter((name) => name.endsWith('.png') && !known.has(name))
    .sort();

  expect(
    orphans,
    'These baselines no longer match a story; delete them or restore the story',
  ).toEqual([]);
});
