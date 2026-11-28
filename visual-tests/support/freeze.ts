/**
 * Kills animation and carets so a capture depends only on layout and paint.
 */
export const freezeStyles =
  '*,*::before,*::after{animation:none!important;transition:none!important;animation-play-state:paused!important;caret-color:transparent!important}';

/**
 * A stylesheet cannot stop a playing video, and the shapes story embeds one. Pausing and rewinding
 * every media element pins it to a single frame, so the capture no longer depends on how far
 * playback happened to get.
 */
export async function freezeMedia(): Promise<void> {
  const media = [...document.querySelectorAll<HTMLMediaElement>('video, audio')];
  for (const element of media) {
    element.autoplay = false;
    element.loop = false;
    element.pause();
    if (element.readyState < 1) continue;
    element.currentTime = 0;
    await new Promise<void>((resolve) => {
      if (!element.seeking) return resolve();
      element.addEventListener('seeked', () => resolve(), { once: true });
    });
  }
}

/**
 * CSS cannot stop animation inside an SVG used as a mask or background: the shapes story masks
 * with a loading-indicator.svg whose path is morphed by a SMIL <animate> element, and that keeps
 * repainting no matter what the stylesheet says. Replacing those images with a copy that has the
 * animation elements removed pins them to their first frame.
 */
export async function freezeAnimatedImages(): Promise<void> {
  const rules: string[] = [];
  const frozen = new Map<string, string | null>();

  for (const element of document.querySelectorAll<HTMLElement>('*')) {
    for (const property of ['maskImage', 'backgroundImage'] as const) {
      const style = getComputedStyle(element);
      const value = property === 'maskImage' ? style.maskImage : style.backgroundImage;
      const url = /url\("?'?([^"')]+)"?'?\)/.exec(value ?? '');
      if (!url) continue;

      const source = url[1];
      if (!frozen.has(source)) {
        let data: string | null = null;
        if (/\.svg(\?|$)/i.test(source)) {
          try {
            const text = await (await fetch(source)).text();
            const still = text
              .replace(/<animate[\s\S]*?(?:\/>|<\/animate>)/gi, '')
              .replace(/<animateTransform[\s\S]*?(?:\/>|<\/animateTransform>)/gi, '')
              .replace(/<animateMotion[\s\S]*?(?:\/>|<\/animateMotion>)/gi, '');
            data = 'data:image/svg+xml;charset=utf-8,' + encodeURIComponent(still);
          } catch {
            // Leave a cross-origin or missing image alone.
          }
        }
        frozen.set(source, data);
      }

      const data = frozen.get(source);
      if (!data) continue;

      const index = rules.length;
      element.setAttribute('data-frozen-image', String(index));
      const cssProperty = property === 'maskImage' ? 'mask-image' : 'background-image';
      rules.push(`[data-frozen-image="${index}"]{${cssProperty}:url("${data}")!important}`);
    }
  }

  if (rules.length === 0) return;

  const style = document.createElement('style');
  style.textContent = rules.join('\n');
  document.head.append(style);
}

/**
 * BlazingStory fades a full-page backdrop out and only then marks the document ready; capturing
 * before that yields a blank or half-faded canvas. Newer versions fade it with an opacity transition
 * (`.default-background.init-level-1` → `opacity: 0`) instead of hiding it with `display: none`, so
 * either end state counts as gone.
 */
export function storyReady(): boolean {
  if (!document.documentElement.classList.contains('_blazing_story_ready_for_visible')) return false;
  const backdrop = document.querySelector('.default-background');
  if (!backdrop) return true;

  const style = getComputedStyle(backdrop);
  return style.display === 'none' || style.visibility === 'hidden' || style.opacity === '0';
}
