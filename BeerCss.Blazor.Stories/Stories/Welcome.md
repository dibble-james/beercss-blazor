---
$attribute: CustomPage("Welcome")
---

# BeerCss.Blazor

Here we have a Blazor wrapper for the amazing [BeerCSS](https://www.beercss.com/) Material UI CSS library

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

## BeerCSS version

This storybook runs on **BeerCSS 5.0.3**, the same version the wrapper targets and the demo vendors. If you use `Group`, v5 rounds the buttons in the middle itself: the first button keeps `left-round`, the last `right-round`, and the ones between them carry no rounding class.

5.0.3 can also load only the modules a page uses. The `Modules` story ([canvas](iframe.html?id=modules--loading&viewMode=story)) loads three settings modules plus the button, field and typography elements — 20,911 B with a 10,429 B loader, against 106,949 B for the all-in-one bundle. That bundle is still the default; modules only win for a page that needs a few elements, since every module together is 110,356 B.
