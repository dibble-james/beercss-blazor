using System.Text.RegularExpressions;

namespace BeerCss.Blazor.VisualTests;

/// <summary>
/// One story as BlazingStory addresses it: a <c>[Stories("Kind")]</c> group plus a
/// <c>&lt;Story Name="Name" /&gt;</c> entry.
/// </summary>
public sealed record Story(string Id, string Kind, string Name)
{
    /// <summary>The isolated canvas for this story — no sidebar, no addon panels.</summary>
    public string CanvasPath => $"/iframe.html?id={Uri.EscapeDataString(Id)}&viewMode=story";

    /// <summary>Story id, which is also the baseline file name.</summary>
    public override string ToString() => Id;
}

/// <summary>
/// The story list, read from the story sources rather than hardcoded, so a story added,
/// renamed or removed changes the test matrix without anyone editing this project.
/// </summary>
public static partial class StoryCatalog
{
    public static IReadOnlyList<Story> All { get; } = Load();

    private static List<Story> Load()
    {
        var storiesDirectory = Path.Combine(RepoRoot.Path, "BeerCss.Blazor.Stories", "Stories");
        if (!Directory.Exists(storiesDirectory))
            throw new InvalidOperationException($"Story sources not found at '{storiesDirectory}'.");

        var stories = new List<Story>();
        foreach (var file in Directory.EnumerateFiles(storiesDirectory, "*.razor", SearchOption.AllDirectories).OrderBy(f => f, StringComparer.Ordinal))
        {
            var text = File.ReadAllText(file);

            var kindMatch = KindPattern().Match(text);
            if (!kindMatch.Success)
                continue;

            var kind = kindMatch.Groups["kind"].Value;
            foreach (Match match in StoryPattern().Matches(text))
            {
                var name = match.Groups["name"].Value;
                stories.Add(new Story((Normalize(kind) + "--" + Normalize(name)).ToLowerInvariant(), kind, name));
            }
        }

        if (stories.Count == 0)
            throw new InvalidOperationException($"No stories found under '{storiesDirectory}'.");

        var duplicates = stories.GroupBy(s => s.Id, StringComparer.Ordinal).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
        if (duplicates.Count > 0)
            throw new InvalidOperationException($"Duplicate story ids: {string.Join(", ", duplicates)}.");

        // Stable order keeps the test run and the written baselines reproducible.
        return stories.OrderBy(s => s.Id, StringComparer.Ordinal).ToList();
    }

    [GeneratedRegex(@"\[Stories\(""(?<kind>[^""]+)""\)\]")]
    private static partial Regex KindPattern();

    [GeneratedRegex(@"<Story\s+Name=""(?<name>[^""]*)""")]
    private static partial Regex StoryPattern();

    [GeneratedRegex(@"[^\w\p{IsCJKUnifiedIdeographs}]+")]
    private static partial Regex NotNameOrDefault();

    /// <summary>
    /// BlazingStory's own id segment (see its NavigationPath.Create): every run of
    /// non-word characters becomes one '-'. Deliberately not trimmed, so ids match the app exactly.
    /// </summary>
    private static string Normalize(string value) => NotNameOrDefault().Replace(value, "-");
}
