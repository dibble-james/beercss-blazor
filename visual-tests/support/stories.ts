import { readdirSync, readFileSync } from 'node:fs';
import path from 'node:path';

export interface Story {
  /** Story id, which is also the baseline file name. */
  id: string;
  /** The `[Stories("Kind")]` group the story belongs to. */
  kind: string;
  /** The `<Story Name="Name" />` entry inside that group. */
  name: string;
}

const repoRoot = path.resolve(__dirname, '../..');
const storiesDirectory = path.join(repoRoot, 'BeerCss.Blazor.Stories', 'Stories');

const KIND = /\[Stories\("([^"]+)"\)\]/;
const NAME = /<Story\s+Name="([^"]*)"/g;

// BlazingStory's own id segment (see its NavigationPath.Create): every run of non-word
// characters becomes one '-'. Deliberately not trimmed, so ids match the app exactly.
// .NET's \w is [\p{L}\p{Mn}\p{Nd}\p{Pc}], which is what this spells out.
const NOT_WORD = /[^\p{L}\p{Mn}\p{Nd}\p{Pc}]+/gu;

const normalize = (value: string) => value.replace(NOT_WORD, '-');

/**
 * The story list, read from the story sources rather than hardcoded, so a story added, renamed or
 * removed changes the test matrix without anyone editing this project.
 */
function load(): Story[] {
  const files = readdirSync(storiesDirectory, { recursive: true, encoding: 'utf8' })
    .filter((entry) => entry.endsWith('.razor'))
    .map((entry) => path.join(storiesDirectory, entry))
    .sort((a, b) => (a < b ? -1 : a > b ? 1 : 0));

  const stories: Story[] = [];
  for (const file of files) {
    const text = readFileSync(file, 'utf8');

    const kind = KIND.exec(text)?.[1];
    if (!kind) continue;

    for (const match of text.matchAll(NAME)) {
      const name = match[1];
      stories.push({ id: `${normalize(kind)}--${normalize(name)}`.toLowerCase(), kind, name });
    }
  }

  if (stories.length === 0) {
    throw new Error(`No stories found under '${storiesDirectory}'.`);
  }

  const seen = new Set<string>();
  const duplicates = new Set<string>();
  for (const story of stories) {
    if (seen.has(story.id)) duplicates.add(story.id);
    seen.add(story.id);
  }

  if (duplicates.size > 0) {
    throw new Error(`Duplicate story ids: ${[...duplicates].join(', ')}.`);
  }

  // Stable order keeps the test run and the written baselines reproducible.
  return stories.sort((a, b) => (a.id < b.id ? -1 : a.id > b.id ? 1 : 0));
}

export const stories: Story[] = load();

export const repoPath = repoRoot;
export const baselinesDirectory = path.join(__dirname, '..', 'baselines');
