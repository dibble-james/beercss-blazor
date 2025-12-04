namespace BeerCss.Blazor.Components.Dialogs;

using Microsoft.AspNetCore.Components;

public interface IHaveNav
{
    public RenderFragment? Nav { get; set; }
}