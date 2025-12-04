namespace BeerCss.Blazor.Common;

using System.Globalization;
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
        theme.AppendLine(CultureInfo.InvariantCulture, $"--primary: {this.Primary};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--on-primary: {this.OnPrimary};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--primary-container: {this.PrimaryContainer};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--on-primary-container: {this.OnPrimaryContainer};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--secondary: {this.Secondary};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--on-secondary: {this.OnSecondary};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--secondary-container: {this.SecondaryContainer};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--on-secondary-container: {this.OnSecondaryContainer};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--tertiary: {this.Tertiary};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--on-tertiary: {this.OnTertiary};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--tertiary-container: {this.TertiaryContainer};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--on-tertiary-container: {this.OnTertiaryContainer};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--error: {this.Error};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--on-error: {this.OnError};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--error-container: {this.ErrorContainer};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--on-error-container: {this.OnErrorContainer};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--background: {this.Background};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--on-background: {this.OnBackground};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--surface: {this.Surface};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--on-surface: {this.OnSurface};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--surface-variant: {this.SurfaceVariant};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--on-surface-variant: {this.OnSurfaceVariant};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--outline: {this.Outline};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--outline-variant: {this.OutlineVariant};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--shadow: {this.Shadow};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--scrim: {this.Scrim};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--inverse-surface: {this.InverseSurface};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--inverse-on-surface: {this.InverseOnSurface};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--inverse-primary: {this.InversePrimary};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--surface-dim: {this.SurfaceDim};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--surface-bright: {this.SurfaceBright};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--surface-container-lowest: {this.SurfaceContainerLowest};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--surface-container-low: {this.SurfaceContainerLow};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--surface-container: {this.SurfaceContainer};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--surface-container-high: {this.SurfaceContainerHigh};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--surface-container-highest: {this.SurfaceContainerHighest};");
        theme.AppendLine(CultureInfo.InvariantCulture, $"--font: {this.Font};");
        theme.AppendLine("}");

        return theme.ToString();
    }

    public static readonly BeerTheme Dark = new()
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

    public static readonly BeerTheme Light = new()
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