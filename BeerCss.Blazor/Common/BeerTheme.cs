namespace BeerCss.Blazor.Common;

using System.Text;

public class BeerTheme
{
    public string Primary { get; set; } = "#cfbcff";
    public string OnPrimary { get; set; } = "#381e72";
    public string PrimaryContainer { get; set; } = "#4f378a";
    public string OnPrimaryContainer { get; set; } = "#e9ddff";
    public string Secondary { get; set; } = "#cbc2db";
    public string OnSecondary { get; set; } = "#332d41";
    public string SecondaryContainer { get; set; } = "#4a4458";
    public string OnSecondaryContainer { get; set; } = "#e8def8";
    public string Tertiary { get; set; } = "#efb8c8";
    public string OnTertiary { get; set; } = "#4a2532";
    public string TertiaryContainer { get; set; } = "#633b48";
    public string OnTertiaryContainer { get; set; } = "#ffd9e3";
    public string Error { get; set; } = "#ffb4ab";
    public string OnError { get; set; } = "#690005";
    public string ErrorContainer { get; set; } = "#93000a";
    public string OnErrorContainer { get; set; } = "#ffb4ab";
    public string Background { get; set; } = "#1c1b1e";
    public string OnBackground { get; set; } = "#e6e1e6";
    public string Surface { get; set; } = "#141316";
    public string OnSurface { get; set; } = "#e6e1e6";
    public string SurfaceVariant { get; set; } = "#49454e";
    public string OnSurfaceVariant { get; set; } = "#cac4cf";
    public string Outline { get; set; } = "#948f99";
    public string OutlineVariant { get; set; } = "#49454e";
    public string Shadow { get; set; } = "#000000";
    public string Scrim { get; set; } = "#000000";
    public string InverseSurface { get; set; } = "#e6e1e6";
    public string InverseOnSurface { get; set; } = "#313033";
    public string InversePrimary { get; set; } = "#6750a4";
    public string SurfaceDim { get; set; } = "#141316";
    public string SurfaceBright { get; set; } = "#3a383c";
    public string SurfaceContainerLowest { get; set; } = "#0f0e11";
    public string SurfaceContainerLow { get; set; } = "#1c1b1e";
    public string SurfaceContainer { get; set; } = "#201f22";
    public string SurfaceContainerHigh { get; set; } = "#2b292d";
    public string SurfaceContainerHighest { get; set; } = "#363438";
    public string Font { get; set; } = @"Inter, Roboto, ""Helvetica Neue"", ""Arial Nova"", ""Nimbus Sans"", Noto Sans, Arial, sans-serif";

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
}