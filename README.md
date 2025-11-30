# BeerCss.Blazor

Here we have a Blazor wrapper for the amazing [BeerCSS](https://www.beercss.com/) Material UI CSS library

## Installation
Grab the package from Nuget [![NuGet Version](https://img.shields.io/nuget/v/BeerCss.Blazor)](https://www.nuget.org/packages/BeerCss.Blazor)

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