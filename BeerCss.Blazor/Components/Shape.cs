namespace BeerCss.Blazor.Components;

using BeerCss.Blazor.Common;
using Microsoft.AspNetCore.Components;

public class Shape : CssClassInjector
{
    [Parameter, EditorRequired]
    public required string ShapeType { get; set; }

    protected override string CssClass => $"shape {this.ShapeType}";
}