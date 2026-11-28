import { expect, test } from '@playwright/test';
import { existsSync, readdirSync } from 'node:fs';
import { baselineName, capturesFor } from '../support/captures';
import { baselinesDirectory, stories } from '../support/stories';

/**
 * Keeps the baseline folder honest: a renamed or deleted story leaves a PNG that no longer
 * describes anything, and it would otherwise sit there forever, quietly green. Every capture a
 * story is run for owns one baseline, so a profile that goes away orphans its PNGs the same way.
 */
test('every baseline belongs to a story', () => {
  if (!existsSync(baselinesDirectory)) return;

  const known = new Set(
    stories.flatMap((story) => capturesFor(story).map((capture) => baselineName(story, capture))),
  );

  const orphans = readdirSync(baselinesDirectory)
    .filter((name) => name.endsWith('.png') && !known.has(name))
    .sort();

  expect(
    orphans,
    'These baselines no longer match a story or a capture profile; delete them or restore it',
  ).toEqual([]);
});
