namespace BeerCss.Blazor.Components;

using BeerCss.Blazor.Common;
using Microsoft.AspNetCore.Components;

public class Wave : CssClassInjector
{
    [Parameter]
    public Speed Speed { get; set; }

    protected override string CssClass => $"{this.Speed.ToCssClass()}wave";
}