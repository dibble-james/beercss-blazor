namespace BeerCss.Blazor.Common;

public enum Size
{
    Medium,
    Tiny,
    Small,
    Large,
    Extra,
}

public static class SizeExtensions
{
    public static string ToCssClass(this Size elevation) => elevation switch
    {
        Size.Tiny => "tiny",
        Size.Small => "small",
        Size.Large => "large",
        Size.Extra => "extra",
        _ => string.Empty,
    };
}