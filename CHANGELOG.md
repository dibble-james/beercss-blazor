# Changelog

Additive releases only: nothing is removed or renamed between minor versions, and existing shorthand
parameters keep working.

## 0.3.0 — 2026-11-27

Six components, full `Shape` coverage, the media loader option and corrected documentation.

### Components

- **`Checkbox`** and **`Radio`** — labelled inputs that render the BeerCSS field markup for you.
- **`Badge`** — `Position` (`Direction.Top` by default), plus `Border`, `Circle`, `Square`, `Min` and
  `None`.
- **`Icon`** — the five sizes, the `Fill` variant, and content that is either a Material Symbols
  ligature or a class such as `fa-solid fa-user`; a `Badge` can sit inside the icon.
- **`Overlay`** — `Active`. An inactive overlay stays in the document with no opacity and no
  `visibility`, so it never catches a click.
- **`Margin`** — `MarginSize`: `Default` (the 1rem a bare `.margin` already applies, so no class),
  `None`, `Tiny`, `Small` and `Large`. BeerCSS 5.0.3 has no `medium-margin` or `extra-margin`.

### Shape

- `Shape.ShapeType` is a `string`, and `ShapeTypes` lists every mask name 5.0.3 ships.
  `ShapeTypes.LeafClover8` keeps upstream's `leaft-clover8` selector typo, because the constant has
  to match the class.
- `Shape.Size` and `Shape.Rotate`; `Rotation` adds `Rotate`, `Fast` and `Slow`.
- `Size.Small` adds no class on a shape: there is no `.shape.small` rule upstream.

### Modules

- `media` joined the loader query, and the demo page shows the five sizes, `round`/`circle`/`square`,
  an inline SVG, a `responsive` image and an `empty-state` placeholder. Media has no wrapper type:
  `media.min.css` sizes plain `img`, `svg` and `video` elements, and its three shape modifiers only
  take effect with the form helper loaded.
- `Margin` needs `helpers/margin.min.css`; `elements/margin.min.css` is a 404 in 5.0.3, so the module
  is vendored from `helpers/`.
- The demo's modular page asks for 9 module files, 23,876 B, against 106,949 B for the all-in-one
  bundle. The bundle stays the default.

### Docs and tests

- README corrected for 5.0.3: the `nav.group` markup, the `Rounding` side values, the module table
  with the 110,356 B all-modules figure, the bundle caveat, and the media caveat.
- Every new public member has a story, and the visual suite gained baselines for the six components,
  the shape sizes and the media elements.
