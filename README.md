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

Check out [the docs](https://dibble-james.github.io/beercss-blazor/) for the available components.

## Components
- [x] Appbar
- [x] Buttons
  - [x] FAB
  - [x] Standard
  - [x] Group
  - [x] Split
- [x] Card
- [x] Chip
- [x] Container
- [x] Dialogs
- [x] Divider
- [x] Expansion
- [x] Grid
- [x] Inputs
- [x] Layout
- [x] List
- [x] Menu
- [x] Navigation
- [x] Page
- [x] Progress
- [x] Scroll
- [x] Search
- [x] Shapes
  - [ ] Shape components
- [x] Snackbar
- [x] Space
- [x] Stepper
- [x] Table
- [x] Tabs
- [x] Toolbar
- [x] Tooltip
- [x] Waves
  - [ ] Wave components