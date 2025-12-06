namespace BeerCss.Blazor.Common;

public abstract record MenuItem(string Text);
public record LinkMenuItem(string Text, string? Href) : MenuItem(Text);
public record ActionMenuItem(string Text, Action OnClick) : MenuItem(Text);