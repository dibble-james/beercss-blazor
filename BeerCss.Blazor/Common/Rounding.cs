namespace BeerCss.Blazor.Common;

public enum Rounding
{
    Default,
    Round,
    NoRound,
    LeftRound,
    RightRound,
    /// <summary>Square on the left, rounded on the right.</summary>
    NoRoundLeft,
    /// <summary>Rounded on the left, square on the right.</summary>
    NoRoundRight,
}

public static class RoundingExtensions
{
    public static string ToCssClass(this Rounding rounding) => rounding switch
    {
        Rounding.Round => "round",
        Rounding.NoRound => "no-round",
        Rounding.LeftRound => "left-round",
        Rounding.RightRound => "right-round",
        // v5's `border-radius: 0 !important` for `no-round` comes after the side classes' shared
        // `.5rem` shorthand but before their per-side longhands, so the side class names the rounded side.
        Rounding.NoRoundLeft => "no-round right-round",
        Rounding.NoRoundRight => "no-round left-round",
        _ => string.Empty,
    };

    /// <summary>Resolves the shared precedence between a component's <c>Rounding</c> parameter
    /// and its legacy <c>Round</c> bool: an explicit rounding wins, otherwise <c>true</c> means
    /// <see cref="Rounding.Round"/>.</summary>
    public static Rounding Resolve(Rounding rounding, bool round) =>
        rounding != Rounding.Default ? rounding : round ? Rounding.Round : Rounding.Default;
}
