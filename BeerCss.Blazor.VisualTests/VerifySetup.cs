using System.Runtime.CompilerServices;
using VerifyTests;

namespace BeerCss.Blazor.VisualTests;

internal static class VerifySetup
{
    /// <summary>
    /// Structural similarity rather than byte equality: a whole component screenshot can gain a
    /// stray antialiased pixel without looking any different. Measured on this suite's own
    /// captures: identical renders score 1.0, +-2 per-channel noise on 1% of the canvas scores
    /// ~0.9999, a 4px band recoloured across the full width scores ~0.990, and a 300x60
    /// component recoloured ~0.988.
    /// </summary>
    private const double SsimThreshold = 0.995;

    [ModuleInitializer]
    internal static void Initialize()
    {
        // Never try to open a diff tool from a test run (headless machines and CI).
        Environment.SetEnvironmentVariable("DiffEngine_Disabled", "true");

        // Downloads Chromium for the pinned Playwright version on first use.
        VerifyPlaywright.Initialize(installPlaywright: true);

        // Verify's own PNG comparer, so there is no image comparison of ours to maintain.
        VerifierSettings.UseSsimForPng(threshold: SsimThreshold);
    }
}
