namespace BeerCss.Blazor.Common;

/// <summary>
/// Spacing a <c>Margin</c> applies. BeerCSS 5.0.3 has no <c>medium-margin</c> and no
/// <c>extra-margin</c>: a bare <c>.margin</c> is already 1rem.
/// </summary>
public enum MarginSize
{
    /// <summary>1rem, the value a bare <c>.margin</c> carries.</summary>
    Default,

    /// <summary>No margin at all.</summary>
    None,

    /// <summary>0.25rem.</summary>
    Tiny,

    /// <summary>0.5rem.</summary>
    Small,

    /// <summary>1.5rem.</summary>
    Large,
}

public static class MarginSizeExtensions
{
    public static string ToCssClass(this MarginSize marginSize) => marginSize switch
    {
        MarginSize.None => "no-margin",
        MarginSize.Tiny => "tiny-margin",
        MarginSize.Small => "small-margin",
        MarginSize.Large => "large-margin",
        _ => string.Empty,
    };
}
