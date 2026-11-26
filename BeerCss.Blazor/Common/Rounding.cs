namespace BeerCss.Blazor.Common;

public enum Rounding
{
    Default,
    Round,
    NoRound,
    LeftRound,
    RightRound,
    NoRoundLeft,
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
        Rounding.NoRoundLeft => "no-round left-round",
        Rounding.NoRoundRight => "no-round right-round",
        _ => string.Empty,
    };

    /// <summary>Resolves the shared precedence between a component's <c>Rounding</c> parameter
    /// and its legacy <c>Round</c> bool: an explicit rounding wins, otherwise <c>true</c> means
    /// <see cref="Rounding.Round"/>.</summary>
    public static Rounding Resolve(Rounding rounding, bool round) =>
        rounding != Rounding.Default ? rounding : round ? Rounding.Round : Rounding.Default;
}
