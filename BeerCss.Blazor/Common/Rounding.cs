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
        // v5 moved `no-round` to `border-radius: 0 !important`, emitted after the shared `.5rem`
        // shorthand of the side classes but before their per-side longhands. So the side class has to
        // name the side that keeps its radius; the other side stays square. The old pairing rounded
        // exactly the side these members say is square, and v5's `.field[class*=round]` then gave the
        // field rounded-field padding as well.
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
