namespace BeerCss.Blazor.Common;

using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

public abstract class CssClassInjector : ComponentBase
{
    [Parameter, EditorRequired]
    public required RenderFragment ChildContent { get; set; }

    protected abstract string CssClass { get; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);

        builder.AddContent(0, this.ChildContent);

        var childContentFrame = builder.GetFrames().Array[1];
        var childContentMarkup = childContentFrame.MarkupContent;
        builder.Clear();

        var firstOpeningBraceIndex = childContentMarkup.IndexOf('<');
        var firstClosingBraceIndex = childContentMarkup.IndexOf('>');

        var elementDefinition = childContentMarkup[firstOpeningBraceIndex..(firstClosingBraceIndex + 1)];

        var classStart = elementDefinition.IndexOf("class", StringComparison.InvariantCultureIgnoreCase);

        Console.WriteLine(elementDefinition);

        var newMarkup = new StringBuilder();

        if (classStart == -1)
        {
            newMarkup.Append(childContentMarkup[firstOpeningBraceIndex..firstClosingBraceIndex]);
            newMarkup.Append(CultureInfo.CurrentCulture, @$" class=""{this.CssClass}""");
            newMarkup.Append(childContentMarkup[firstClosingBraceIndex..]);
        }
        else
        {
            var classEnd = elementDefinition.IndexOf('"', classStart + 7);
            var cssClass = elementDefinition[(classStart + 7)..classEnd];
            cssClass += $" {this.CssClass}";

            newMarkup.Append(childContentMarkup[firstOpeningBraceIndex..(firstOpeningBraceIndex + classStart)]);
            newMarkup.Append(@"class=""");
            newMarkup.Append(cssClass);
            newMarkup.Append(childContentMarkup[(firstOpeningBraceIndex + classEnd)..]);
        }

        builder.AddMarkupContent(0, newMarkup.ToString());
    }
}