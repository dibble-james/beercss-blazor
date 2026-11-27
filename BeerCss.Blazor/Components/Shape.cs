namespace BeerCss.Blazor.Components;

using BeerCss.Blazor.Common;
using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;

public class Shape : CssClassInjector
{
    [Parameter, EditorRequired]
    public required string ShapeType { get; set; }

    /// <summary>
    /// Shape size. <see cref="Size.Medium"/> adds no class: the module's bare <c>.shape</c> rule is
    /// already 3.5rem, and there is no <c>.shape.small</c> for <see cref="Size.Small"/> to reach.
    /// </summary>
    [Parameter]
    public Size Size { get; set; } = Size.Medium;

    /// <summary>Spins the shape. Still by default.</summary>
    [Parameter]
    public Rotation Rotate { get; set; } = Rotation.Default;

    protected override string CssClass => new CssBuilder()
        .AddClass("shape")
        .AddClass(ShapeType)
        .AddClass(Size.ToCssClass())
        .AddClass(Rotate.ToCssClass())
        .ToString();
}
