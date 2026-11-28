namespace BeerCss.Blazor.Common;

/// <summary>
/// Which of a theme's two variants <c>Beer</c> renders.
/// </summary>
/// <remarks>
/// A <see cref="BeerTheme"/> is one palette, not a mode; say which variant of it you want here. There is
/// deliberately no automatic value: BeerCSS's dark mode is the <c>dark</c> class and it ships no
/// <c>prefers-color-scheme</c> rules, so following the operating system would mean this library
/// reimplementing those rules. Write that query yourself for now.
/// </remarks>
public enum BeerMode
{
    /// <summary>The default. Renders the <c>light</c> class and the <see cref="BeerTheme"/> passed as the theme.</summary>
    Light,

    /// <summary>Renders the <c>dark</c> class and the <c>DarkTheme</c>, falling back to the built-in <see cref="BeerTheme.Dark"/>.</summary>
    Dark,
}
