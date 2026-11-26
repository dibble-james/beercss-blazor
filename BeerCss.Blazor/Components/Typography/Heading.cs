namespace BeerCss.Blazor.Components.Typography;

using Microsoft.AspNetCore.Components;

/// <summary>
/// A heading whose element is chosen at the call site, so the heading scale can be used on
/// something that is not a heading tag. Use <see cref="H1"/>–<see cref="H6"/> when it is.
/// </summary>
public class Heading : HeadingBase
{
    private static readonly string[] HeadingTags = ["h1", "h2", "h3", "h4", "h5", "h6"];
    private static readonly string[] OtherTags = ["div", "span", "p"];

    /// <summary>
    /// The heading scale, 1 to 6. It picks the element to render when <see cref="As"/> is unset,
    /// and always picks the <c>h{level}</c> size class when the rendered element is not that
    /// level's own heading tag.
    /// </summary>
    [Parameter, EditorRequired]
    public int Level { get; set; }

    /// <summary>
    /// The element to render: a heading tag (<c>h1</c>–<c>h6</c>), <c>div</c>, <c>span</c> or
    /// <c>p</c>. Defaults to the heading tag for <see cref="Level"/>. Every value except that same
    /// heading tag also gets the <c>h{level}</c> class, because only the native tag is sized by the
    /// tag itself.
    /// </summary>
    [Parameter]
    public string? As { get; set; }

    private string _element = string.Empty;

    protected override int HeadingLevel => Level;

    protected override string Element => _element;

    protected override void OnParametersSet()
    {
        if (Level is < 1 or > 6)
        {
            throw new ArgumentOutOfRangeException(nameof(Level), Level, "Heading.Level must be between 1 and 6.");
        }

        var element = As ?? $"h{Level}";

        if (!HeadingTags.Contains(element) && !OtherTags.Contains(element))
        {
            throw new ArgumentException(
                $"'{element}' is not a supported element. Use one of {string.Join(", ", HeadingTags.Concat(OtherTags))}, or leave As unset to follow Level.",
                nameof(As));
        }

        _element = element;
    }
}
