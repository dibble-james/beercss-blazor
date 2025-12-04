namespace BeerCss.Blazor.Components.Dialogs;

using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;

public abstract class Dialog : ComponentBase
{
    private bool isOpen;

    [Parameter, EditorRequired]
    public required RenderFragment Content { get; set; }

    [Parameter]
    public bool Open { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; } = new Dictionary<string, object>();

    public virtual CssBuilder CssClass => new CssBuilder()
        .AddClass("active", this.Open)
        .AddClassFromAttributes(this.AdditionalAttributes);

    [CascadingParameter]
    public DialogProvider? Provider { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        if (this.Provider is null)
        {
            throw new InvalidOperationException("Dialog used without a parent DialogProvider");
        }

        if (this.Open && this.isOpen != this.Open)
        {
            this.isOpen = true;
            await this.Provider.AddDialog(this);
        }
        else if (!this.Open && this.isOpen != this.Open)
        {
            this.isOpen = false;
            await this.Provider.RemoveDialog(this);
        }
    }
}