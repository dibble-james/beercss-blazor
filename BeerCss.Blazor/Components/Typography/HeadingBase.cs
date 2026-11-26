namespace BeerCss.Blazor.Components.Typography;

using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;

/// <summary>
/// The parameters and the class building shared by <see cref="Heading"/> and the <c>H1</c>–<c>H6</c>
/// wrappers. A subclass owns its markup and says which scale it renders; this type owns the
/// surface and the <c>h{level}</c>/<c>small</c> classes, so a wrapper never has to reach into
/// <see cref="Heading"/> to get them.
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
}
