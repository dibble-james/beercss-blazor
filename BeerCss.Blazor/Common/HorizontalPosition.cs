namespace BeerCss.Blazor.Common;

public enum HorizontalPosition
{
    Left,
    Center,
    Right,
}

public static class HorizontalPositionExtensions
{
    public static string ToCssClass(this HorizontalPosition position) => position switch
    {
        HorizontalPosition.Center => "center-align",
        HorizontalPosition.Right => "right-align",
        HorizontalPosition.Left => string.Empty,
        _ => string.Empty,
    };
}