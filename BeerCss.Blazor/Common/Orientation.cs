namespace BeerCss.Blazor.Common;

public enum Orientation
{
    Horizontal,
    Vertical,
}

public static class OrientationExtensions
{
    public static string ToCssClass(this Orientation orientation) => orientation switch
    {
        Orientation.Horizontal => "horizontal",
        Orientation.Vertical => "vertical",
        _ => string.Empty,
    };
}