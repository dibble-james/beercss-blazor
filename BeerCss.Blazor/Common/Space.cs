namespace BeerCss.Blazor.Common;

public enum Space
{
    Small,
    None,
    Medium,
    Large,
}

public static class SpaceExtensions
{
    public static string ToCssClass(this Space elevation) => elevation switch
    {
        Space.None => "no-space",
        Space.Medium => "medium-space",
        Space.Large => "large-space",
        _ => string.Empty,
    };
}