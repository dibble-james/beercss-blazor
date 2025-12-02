namespace BeerCss.Blazor.Common;

public enum Direction
{
    Default,
    Left,
    Right,
    Top,
    Bottom,
}

public static class DirectionExtensions
{
    public static string ToCssClass(this Direction elevation) => elevation switch
    {
        Direction.Left => "left",
        Direction.Right => "right",
        Direction.Top => "top",
        Direction.Bottom => "bottom",
        _ => string.Empty,
    };
}