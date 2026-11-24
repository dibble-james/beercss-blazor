using Xunit;

namespace BeerCss.Blazor.VisualTests;

/// <summary>
/// Keeps the baseline folder honest: a renamed or deleted story leaves a PNG that no
/// longer describes anything, and it would otherwise sit there forever, quietly green.
/// </summary>
public class BaselineTests
{
    private const string VerifiedSuffix = ".verified.png";

    [Fact]
    public void EveryBaselineBelongsToAStory()
    {
        if (!Directory.Exists(RepoRoot.Baselines))
            return;

        var stories = StoryCatalog.All.Select(s => s.Id).ToHashSet(StringComparer.Ordinal);

        var orphans = Directory.EnumerateFiles(RepoRoot.Baselines, "*" + VerifiedSuffix)
            .Select(Path.GetFileName)
            .Where(name => !stories.Contains(name![..^VerifiedSuffix.Length]))
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToList();

        Assert.True(
            orphans.Count == 0,
            "These baselines no longer match a story; delete them or restore the story: " + string.Join(", ", orphans));
    }
}
