# BeerCss.Blazor

Here we have a Blazor wrapper for the amazing [BeerCSS](https://www.beercss.com/) Material UI CSS library.

Source is available on [Github](https://github.com/dibble-james/beercss-blazor)

## Installation
Grab the package from Nuget [![NuGet Version](https://img.shields.io/nuget/v/BeerCss.Blazor)](https://www.nuget.org/packages/BeerCss.Blazor)

Install the BeerCss CSS & Javascript files into your app in your favourite style (CDN, libman etc...)

Soak your app with beer (AKA add the namespace your your `_Imports.razor` and the root component to your `_MainLayout.razor`)

```razor
@using BeerCss.Blazor.Components
@using BeerCss.Blazor.Common
```

```razor
@inherits LayoutComponentBase
<Beer>
    @Body
</Beer>
```

Check out [the docs](https://dibble-james.github.io/beercss-blazor/) for the available components, and [the changelog](CHANGELOG.md) for what each release adds.

## BeerCSS version

The wrapper targets **BeerCSS 5.0.3**, the version the demo vendors under `wwwroot/beer/dist/cdn`. Use the 5.0.3 assets: the `Rounding` side values (`NoRoundLeft`, `NoRoundRight`) rely on the order v5 emits its rounding classes in.

### Nav-group markup

v5 rounds the buttons inside a `nav.group` itself, so the ones in the middle no longer carry `no-round`:

```html
<nav class="group">
  <button class="left-round">Button</button>
  <button>Button</button>
  <button>Button</button>
  <button class="right-round">Button</button>
</nav>
```

`Group` renders the `nav.group` already, so only the buttons change: round the outer edges and leave the middle ones alone.

```razor
<Group Connected>
    <Button Rounding="Rounding.LeftRound">Left</Button>
    <Button>Middle</Button>
    <Button Rounding="Rounding.RightRound">Right</Button>
</Group>
```

For fully square corners, every button takes `no-round`, and the outer two keep the side that stays rounded — `Rounding.NoRoundLeft` and `Rounding.NoRoundRight` do exactly that.

## Modules

BeerCSS 5.0.3 can load only the modules a page uses. `beer.loader.js` fetches the modules named in its own query string, from the folders next to itself:

```html
<script type="module" src="beer/dist/cdn/beer.loader.js?settings=global,light,reset&elements=button,field,typography,media&helpers=form"></script>
```

The demo's [`modules.html`](BeerCss.Blazor.Stories/wwwroot/modules.html) is a form built that way, and the storybook shows it as the `Modules` story. It asks for three settings modules, the button, field, typography and media elements, and the form helper — which is where `round`, `circle`, `square` and `border` live — so:

| What | Size |
|---|---|
| The 9 module files that page asks for | 23,876 B |
| `beer.loader.js` | 10,429 B |
| All-in-one: `beer.scoped.min.css` + `beer.min.js` | 106,949 B |
| Every module, `.min.css` and `.min.js` | 110,356 B |

The last row is the caveat: loading everything is larger than the bundle, so modules only pay off for a page that needs a few elements. **The all-in-one bundle stays the default** — link `beer.scoped.min.css` and load `beer.min.js`, as the demo's `iframe.html` does. Reach for the loader when a page needs a small subset.

Media is the one element with no wrapper type: `media.min.css` sizes plain `<img>`, `<svg>` and `<video>` elements through the `tiny`–`extra`, `responsive` and `empty-state` classes, so those are written as plain markup in your own components. Its `round`, `circle` and `square` modifiers only take effect with the form helper loaded, which is where those border-radius rules are. The `Media` story renders those elements on their own canvas, so the visual suite covers them.

`libman.json` vendors those nine files plus `helpers/margin.min.css`, which `<Margin>` needs, so a module added to the query string needs its files added there too. The margin module is published under `helpers/`, not `elements/` — `elements/margin.min.css` is a 404.

## Icons

`Icon` renders the Material Symbols ligature you put inside it, optionally filled or sized:

```razor
<Icon>home</Icon>
<Icon Fill Size="Size.Large">home</Icon>
```

Icons from another font, such as Font Awesome, are given by class instead — the class has to land on the `i`, so the ligature inside is only a fallback:

```razor
<Icon class="fa-solid fa-user">person</Icon>
```

A `Badge` goes inside the icon and takes the corner, which needs `position: relative` on the icon:

```razor
<Icon style="position: relative">
    notifications
    <Badge>3</Badge>
</Icon>
```

## Modes and theming

`<Beer>` takes a palette and says which variant of it to render:

```razor
<Beer Mode="BeerMode.Dark">
    @Body
</Beer>
```

`Mode` defaults to `Light`. The component renders `light` or `dark` on its root element, paints the page
background from the palette and writes the matching `color-scheme`, so the page scrollbar and the native
controls Blazor does not draw follow the mode too.

A `BeerTheme` is one palette, not a mode. For a branded dark mode, supply both; leaving `DarkTheme` out
falls back to the built-in `BeerTheme.Dark`, so `Mode="Dark"` alone always renders something coherent:

```razor
<Beer Mode="_mode" Theme="Brand.Light" DarkTheme="Brand.Dark">
    @Body
</Beer>
```

### Following the operating system

There is no automatic mode, on purpose. BeerCSS's dark mode is the `dark` class, and the stylesheet it
ships has no `prefers-color-scheme` rules at all — supporting them here would mean reimplementing
them. Read the preference yourself and set `Mode`:

```razor
@inject IJSRuntime JS

<Beer Mode="_mode">@Body</Beer>

@code {
    private BeerMode _mode = BeerMode.Light;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var dark = await JS.InvokeAsync<bool>(
            "matchMedia", "(prefers-color-scheme: dark)");

        _mode = dark ? BeerMode.Dark : BeerMode.Light;
        StateHasChanged();
    }
}
```

`matchMedia` above is the browser's own call, so a one-line wrapper that invokes
`window.matchMedia(media).matches` is enough; rerender when you want to react to the preference
changing.

## Components

Corner rounding on buttons, cards and the fields is a `Rounding` value — `Round`, `NoRound`, `LeftRound`, `RightRound`, `NoRoundLeft`, `NoRoundRight` — with the older `Round` bool kept as a shorthand for `Rounding.Round`.

- [x] Align
- [x] Appbar
- [x] Badge
- [x] Buttons
  - [x] FAB
  - [x] Standard
  - [x] Group
  - [x] Split
- [x] Card
- [x] Chip
- [x] Dialogs
- [x] Divider
- [x] Expansion
- [x] Grid
- [x] Icon
- [x] Inputs
  - [x] Checkbox
  - [x] Radio
- [x] List
- [x] Margin
- [x] Menu
- [x] NavBar
- [x] Navigation
- [x] Overlay
- [x] Padding
- [x] Page
- [x] Position
- [x] Progress
- [x] Row
- [x] Scroll
- [x] Shapes
  - [x] Shape components
- [x] SideContent
- [x] Snackbar
- [x] Spacer
- [x] Stepper
- [x] Table
- [x] Tabs
- [x] Toolbar
- [x] Tooltip
- [x] Typography
  - [x] Heading
  - [x] H1–H6
- [x] Waves
  - [x] Wave components
