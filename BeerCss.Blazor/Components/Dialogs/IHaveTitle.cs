namespace BeerCss.Blazor.Components.Dialogs;

using Microsoft.AspNetCore.Components;

public interface IHaveTitle
{
    public RenderFragment Title { get; set; }
}