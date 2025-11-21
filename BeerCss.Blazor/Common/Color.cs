namespace BeerCss.Blazor.Common;

public enum Color
{
    Default,
    Primary,
    Secondary,
    Tertiary,
    Error,
}

public static class ColorExtensions
{
    public static string ToCssClass(this Color elevation) => elevation switch
    {
        Color.Primary => "primary",
        Color.Secondary => "secondary",
        Color.Tertiary => "tertiary",
        Color.Error => "error",
        _ => string.Empty,
    };
}