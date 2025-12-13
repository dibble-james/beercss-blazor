namespace BeerCss.Blazor.Common;

public enum Speed
{
    Default,
    Slow,
    Fast,
}

public static class SpeedExtensions
{
    public static string ToCssClass(this Speed speed) => speed switch
    {
        Speed.Slow => "slow-",
        Speed.Fast => "fast-",
        Speed.Default => string.Empty,
        _ => string.Empty,
    };
}