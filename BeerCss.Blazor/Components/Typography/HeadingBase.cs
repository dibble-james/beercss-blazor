namespace BeerCss.Blazor.Components.Typography;

using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

/// <summary>
/// The parameters, the class building and the element rendering shared by <see cref="Heading"/> and
/// the <c>H1</c>–<c>H6</c> wrappers. A subclass only says which scale it renders and, if it can
/// change element, which element is being rendered; this type owns the surface and the
/// <c>h{level}</c>/<c>small</c> classes, so no subclass reaches into another to get them.
/// </summary>
public abstract class HeadingBase : ComponentBase
{
    /// <summary>
    /// The heading scale this component renders, 1 to 6.
    /// </summary>
    protected abstract int HeadingLevel { get; }

    /// <summary>
    /// The element being rendered. The <c>h{level}</c> size class is only added when this is not the
    /// native heading for <see cref="HeadingLevel"/>, because the native tag is already sized by the
    /// tag itself. Defaults to that native tag.
    /// </summary>
    protected virtual string Element => $"h{HeadingLevel}";

    /// <summary>
    /// Renders the <c>small</c> variant of the heading scale.
    /// </summary>
    [Parameter]
    public bool Small { get; set; }

    [Parameter, EditorRequired]
    public required RenderFragment ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; } = new Dictionary<string, Object>();

    /// <summary>
    /// The class for the element named by <see cref="Element"/>, including any <c>class</c> the
    /// caller passed.
    /// </summary>
    protected CssBuilder CssClass => new CssBuilder()
        .AddClass($"h{HeadingLevel}", Element != $"h{HeadingLevel}")
        .AddClass("small", Small)
        .AddClassFromAttributes(AdditionalAttributes);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "The heading components render the element their subclass chooses.")]
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);

        builder.OpenElement(0, Element);
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddAttribute(2, "class", CssClass);
        builder.AddContent(3, ChildContent);
        builder.CloseElement();
    }
}
