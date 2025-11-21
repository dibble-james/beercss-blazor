namespace BeerCss.Blazor.Common;

public enum Elevation
{
    NoElevate,
    SmallElevate,
    MediumElevate,
    LargeElevate,
}

public static class ElevationExtensions
{
    public static string ToCssClass(this Elevation elevation) => elevation switch
    {
        Elevation.SmallElevate => "small-elevate",
        Elevation.MediumElevate => "medium-elevate",
        Elevation.LargeElevate => "large-elevate",
        _ => string.Empty,
    };
}