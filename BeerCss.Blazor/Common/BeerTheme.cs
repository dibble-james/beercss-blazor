namespace BeerCss.Blazor.Common;

using System.Text;

public class BeerTheme
{
    public required string Primary { get; init; }
    public required string OnPrimary { get; init; }
    public required string PrimaryContainer { get; init; }
    public required string OnPrimaryContainer { get; init; }
    public required string Secondary { get; init; }
    public required string OnSecondary { get; init; }
    public required string SecondaryContainer { get; init; }
    public required string OnSecondaryContainer { get; init; }
    public required string Tertiary { get; init; }
    public required string OnTertiary { get; init; }
    public required string TertiaryContainer { get; init; }
    public required string OnTertiaryContainer { get; init; }
    public required string Error { get; init; }
    public required string OnError { get; init; }
    public required string ErrorContainer { get; init; }
    public required string OnErrorContainer { get; init; }
    public required string Background { get; init; }
    public required string OnBackground { get; init; }
    public required string Surface { get; init; }
    public required string OnSurface { get; init; }
    public required string SurfaceVariant { get; init; }
    public required string OnSurfaceVariant { get; init; }
    public required string Outline { get; init; }
    public required string OutlineVariant { get; init; }
    public required string Shadow { get; init; }
    public required string Scrim { get; init; }
    public required string InverseSurface { get; init; }
    public required string InverseOnSurface { get; init; }
    public required string InversePrimary { get; init; }
    public required string SurfaceDim { get; init; }
    public required string SurfaceBright { get; init; }
    public required string SurfaceContainerLowest { get; init; }
    public required string SurfaceContainerLow { get; init; }
    public required string SurfaceContainer { get; init; }
    public required string SurfaceContainerHigh { get; init; }
    public required string SurfaceContainerHighest { get; init; }
    public string Font { get; init; } = @"Inter, Roboto, ""Helvetica Neue"", ""Arial Nova"", ""Nimbus Sans"", Noto Sans, Arial, sans-serif";

    public string GetTheme()
    {
        var theme = new StringBuilder();

        theme.AppendLine(":root {");
        theme.AppendLine($"--primary: {Primary};");
        theme.AppendLine($"--on-primary: {OnPrimary};");
        theme.AppendLine($"--primary-container: {PrimaryContainer};");
        theme.AppendLine($"--on-primary-container: {OnPrimaryContainer};");
        theme.AppendLine($"--secondary: {Secondary};");
        theme.AppendLine($"--on-secondary: {OnSecondary};");
        theme.AppendLine($"--secondary-container: {SecondaryContainer};");
        theme.AppendLine($"--on-secondary-container: {OnSecondaryContainer};");
        theme.AppendLine($"--tertiary: {Tertiary};");
        theme.AppendLine($"--on-tertiary: {OnTertiary};");
        theme.AppendLine($"--tertiary-container: {TertiaryContainer};");
        theme.AppendLine($"--on-tertiary-container: {OnTertiaryContainer};");
        theme.AppendLine($"--error: {Error};");
        theme.AppendLine($"--on-error: {OnError};");
        theme.AppendLine($"--error-container: {ErrorContainer};");
        theme.AppendLine($"--on-error-container: {OnErrorContainer};");
        theme.AppendLine($"--background: {Background};");
        theme.AppendLine($"--on-background: {OnBackground};");
        theme.AppendLine($"--surface: {Surface};");
        theme.AppendLine($"--on-surface: {OnSurface};");
        theme.AppendLine($"--surface-variant: {SurfaceVariant};");
        theme.AppendLine($"--on-surface-variant: {OnSurfaceVariant};");
        theme.AppendLine($"--outline: {Outline};");
        theme.AppendLine($"--outline-variant: {OutlineVariant};");
        theme.AppendLine($"--shadow: {Shadow};");
        theme.AppendLine($"--scrim: {Scrim};");
        theme.AppendLine($"--inverse-surface: {InverseSurface};");
        theme.AppendLine($"--inverse-on-surface: {InverseOnSurface};");
        theme.AppendLine($"--inverse-primary: {InversePrimary};");
        theme.AppendLine($"--surface-dim: {SurfaceDim};");
        theme.AppendLine($"--surface-bright: {SurfaceBright};");
        theme.AppendLine($"--surface-container-lowest: {SurfaceContainerLowest};");
        theme.AppendLine($"--surface-container-low: {SurfaceContainerLow};");
        theme.AppendLine($"--surface-container: {SurfaceContainer};");
        theme.AppendLine($"--surface-container-high: {SurfaceContainerHigh};");
        theme.AppendLine($"--surface-container-highest: {SurfaceContainerHighest};");
        theme.AppendLine($"--font: {Font};");
        theme.AppendLine("}");

        return theme.ToString();
    }

    public static BeerTheme Dark = new()
    {
        Primary = "#cfbcff",
        OnPrimary = "#381e72",
        PrimaryContainer = "#4f378a",
        OnPrimaryContainer = "#e9ddff",
        Secondary = "#cbc2db",
        OnSecondary = "#332d41",
        SecondaryContainer = "#4a4458",
        OnSecondaryContainer = "#e8def8",
        Tertiary = "#efb8c8",
        OnTertiary = "#4a2532",
        TertiaryContainer = "#633b48",
        OnTertiaryContainer = "#ffd9e3",
        Error = "#ffb4ab",
        OnError = "#690005",
        ErrorContainer = "#93000a",
        OnErrorContainer = "#ffb4ab",
        Background = "#1c1b1e",
        OnBackground = "#e6e1e6",
        Surface = "#141316",
        OnSurface = "#e6e1e6",
        SurfaceVariant = "#49454e",
        OnSurfaceVariant = "#cac4cf",
        Outline = "#948f99",
        OutlineVariant = "#49454e",
        Shadow = "#000000",
        Scrim = "#000000",
        InverseSurface = "#e6e1e6",
        InverseOnSurface = "#313033",
        InversePrimary = "#6750a4",
        SurfaceDim = "#141316",
        SurfaceBright = "#3a383c",
        SurfaceContainerLowest = "#0f0e11",
        SurfaceContainerLow = "#1c1b1e",
        SurfaceContainer = "#201f22",
        SurfaceContainerHigh = "#2b292d",
        SurfaceContainerHighest = "#363438",
    };

    public static BeerTheme Light = new()
    {
        Primary = "#6750a4",
        OnPrimary = "#ffffff",
        PrimaryContainer = "#e9ddff",
        OnPrimaryContainer = "#22005d",
        Secondary = "#625b71",
        OnSecondary = "#ffffff",
        SecondaryContainer = "#e8def8",
        OnSecondaryContainer = "#1e192b",
        Tertiary = "#7e5260",
        OnTertiary = "#ffffff",
        TertiaryContainer = "#ffd9e3",
        OnTertiaryContainer = "#31101d",
        Error = "#ba1a1a",
        OnError = "#ffffff",
        ErrorContainer = "#ffdad6",
        OnErrorContainer = "#410002",
        Background = "#fffbff",
        OnBackground = "#1c1b1e",
        Surface = "#fdf8fd",
        OnSurface = "#1c1b1e",
        SurfaceVariant = "#e7e0eb",
        OnSurfaceVariant = "#49454e",
        Outline = "#7a757f",
        OutlineVariant = "#cac4cf",
        Shadow = "#000000",
        Scrim = "#000000",
        InverseSurface = "#313033",
        InverseOnSurface = "#f4eff4",
        InversePrimary = "#cfbcff",
        SurfaceDim = "#ddd8dd",
        SurfaceBright = "#fdf8fd",
        SurfaceContainerLowest = "#ffffff",
        SurfaceContainerLow = "#f7f2f7",
        SurfaceContainer = "#f2ecf1",
        SurfaceContainerHigh = "#ece7eb",
        SurfaceContainerHighest = "#e6e1e6",
    };
}