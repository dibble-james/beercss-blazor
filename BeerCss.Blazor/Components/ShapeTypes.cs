namespace BeerCss.Blazor.Components;

/// <summary>
/// The mask class names BeerCSS 5.0.3 ships, for <see cref="Shape.ShapeType"/>. Each one loads its
/// own SVG from the same folder as the stylesheet.
/// </summary>
public static class ShapeTypes
{
    public const string Arch = "arch";
    public const string Arrow = "arrow";
    public const string Boom = "boom";
    public const string Bun = "bun";
    public const string Burst = "burst";
    public const string Circle = "circle";
    public const string Clamshell = "clamshell";
    public const string Diamond = "diamond";
    public const string Fan = "fan";
    public const string Flower = "flower";
    public const string Gem = "gem";
    public const string GhostIsh = "ghost-ish";
    public const string Heart = "heart";
    public const string LeafClover4 = "leaf-clover4";

    /// <summary>
    /// Upstream spells the selector <c>.shape.leaft-clover8</c> — "leaft", not "leaf" — and its rule
    /// loads <c>leaf-clover8.svg</c>. The value keeps the typo because the class has to match the
    /// selector; only this constant's name is spelled correctly.
    /// </summary>
    public const string LeafClover8 = "leaft-clover8";

    public const string LoadingIndicator = "loading-indicator";
    public const string Oval = "oval";
    public const string Pentagon = "pentagon";
    public const string Pill = "pill";
    public const string PixelCircle = "pixel-circle";
    public const string PixelTriangle = "pixel-triangle";
    public const string Puffy = "puffy";
    public const string PuffyDiamond = "puffy-diamond";
    public const string Semicircle = "semicircle";
    public const string SidedCookie4 = "sided-cookie4";
    public const string SidedCookie6 = "sided-cookie6";
    public const string SidedCookie7 = "sided-cookie7";
    public const string SidedCookie9 = "sided-cookie9";
    public const string SidedCookie12 = "sided-cookie12";
    public const string Slanted = "slanted";
    public const string SoftBoom = "soft-boom";
    public const string SoftBurst = "soft-burst";
    public const string Square = "square";
    public const string Sunny = "sunny";
    public const string Triangle = "triangle";
    public const string VerySunny = "very-sunny";
}
