namespace BeerCss.Blazor.Stories;

using BeerCss.Blazor.Common;
using Microsoft.AspNetCore.Components;

/// <summary>
/// Reads the capture matrix's parameters out of the story canvas URL. The visual tests append
/// <c>theme=dark</c> to the story they open, and nothing else in the storybook chooses a mode, so this
/// is the only place the canvas learns which one to render.
/// </summary>
internal static class StoryQuery
{
    private const string Parameter = "theme=";

    public static BeerMode ReadMode(NavigationManager navigation)
    {
        var query = new Uri(navigation.Uri).Query.TrimStart('?');

        foreach (var pair in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            if (!pair.StartsWith(Parameter, StringComparison.Ordinal))
            {
                continue;
            }

            if (pair[Parameter.Length..].Equals("dark", StringComparison.OrdinalIgnoreCase))
            {
                return BeerMode.Dark;
            }
        }

        return BeerMode.Light;
    }
}
