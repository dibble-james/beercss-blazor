namespace BeerCss.Blazor.Common;

public enum Rotation
{
    /// <summary>The shape sits still.</summary>
    Default,

    /// <summary>One turn every 12 seconds.</summary>
    Rotate,

    /// <summary>One turn every 6 seconds.</summary>
    Fast,

    /// <summary>One turn every 24 seconds.</summary>
    Slow,
}

public static class RotationExtensions
{
    public static string ToCssClass(this Rotation rotation) => rotation switch
    {
        Rotation.Rotate => "rotate",
        Rotation.Fast => "fast-rotate",
        Rotation.Slow => "slow-rotate",
        _ => string.Empty,
    };
}
