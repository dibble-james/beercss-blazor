import type { Story } from './stories';

/**
 * One way of capturing a story. A profile names the viewport it renders in, whether it captures the
 * whole scrollable page, and which story groups it covers.
 */
export interface Capture {
  /** Names the profile: 'default' keeps the plain baseline names, every other id becomes a suffix. */
  id: string;
  viewport: { width: number; height: number };
  fullPage: boolean;
  /** The `[Stories("Kind")]` groups this profile covers. Left out for the profile every story gets. */
  kinds?: ReadonlySet<string>;
}

/** The canonical viewport, and what the config renders in before a profile sets its own. */
export const defaultViewport = { width: 1280, height: 800 };

/**
 * The capture matrix. The first profile is canonical: every story is captured that way and its
 * baseline is named after the story alone, which is the name the committed baselines already have.
 *
 * A further profile only covers the kinds it names, so adding one costs nothing until a kind opts
 * in — and when a kind does, the first run fails on the missing baselines instead of approving them.
 */
export const captures: readonly Capture[] = [
  { id: 'default', viewport: defaultViewport, fullPage: false },
  {
    id: 'narrow',
    viewport: { width: 390, height: 844 },
    fullPage: true,
    // Opt a kind in by adding its `[Stories("Kind")]` name, e.g. 'Components/Inputs', and capture the
    // new baselines from a dispatch: that kind's stories then get a second capture.
    kinds: new Set<string>(),
  },
];

/** The captures a story is run for: the canonical one always, the rest only for their kinds. */
export function capturesFor(story: Story): Capture[] {
  return captures.filter((capture) => !capture.kinds || capture.kinds.has(story.kind));
}

/**
 * The baseline a capture compares against. The canonical profile is unsuffixed because that is the
 * name the committed baselines have; any other profile is suffixed with its id.
 */
export function baselineName(story: Story, capture: Capture): string {
  return capture.id === 'default' ? `${story.id}.png` : `${story.id}--${capture.id}.png`;
}

/** The test title for a capture: the canonical profile keeps the bare story id. */
export function captureTitle(story: Story, capture: Capture): string {
  return capture.id === 'default' ? `story ${story.id}` : `story ${story.id} (${capture.id})`;
}
