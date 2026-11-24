namespace BeerCss.Blazor.VisualTests;

/// <summary>
/// Locates the repository from the test assembly's output folder, so baselines and the
/// story demo are found the same way from a developer machine and CI.
/// </summary>
internal static class RepoRoot
{
    public static string Path { get; } = FindRepoRoot();

    public static string DemoProject => System.IO.Path.Combine(Path, "BeerCss.Blazor.Stories");

    public static string Baselines => System.IO.Path.Combine(Path, "BeerCss.Blazor.VisualTests", "baselines");

    private static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(System.IO.Path.Combine(dir.FullName, "BeerCss.Blazor.slnx")))
                return dir.FullName;
            dir = dir.Parent;
        }

        throw new InvalidOperationException(
            $"Could not find BeerCss.Blazor.slnx above '{AppContext.BaseDirectory}'.");
    }
}
